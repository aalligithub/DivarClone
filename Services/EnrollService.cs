using System.Data.SqlClient;
using System.Data;
using DivarClone.Controllers;
using DivarClone.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace DivarClone.Services
{
    public interface IEnrollService
    {
        Task<bool> EnrollUser(Enroll enroll);
        Task<ClaimsPrincipal> LogUserIn(Enroll e);
    }
    public class EnrollService : IEnrollService
    {
        public string Constr { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        private readonly ILogger<AddListingController> _logger;


        public EnrollService(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, ILogger<AddListingController> logger)
        {
            _configuration = configuration;
            Constr = _configuration.GetConnectionString("DivarCloneContextConnection");
            _logger = logger;

            con = new SqlConnection(Constr);
        }

        public async Task<bool> EnrollUser(Enroll e)
        {
            //HANDLE ERRORS IN THE FORM
            try
            {
                if (con != null && con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                var cmd = new SqlCommand("SP_EnrollDetail", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FirstName", e.FirstName);
                cmd.Parameters.AddWithValue("@Username", e.Username);
                cmd.Parameters.AddWithValue("@Password", e.Password);
                cmd.Parameters.AddWithValue("@Email", e.Email);
                cmd.Parameters.AddWithValue("@Phone", e.PhoneNumber);
                cmd.Parameters.AddWithValue("@Role", "User");
                cmd.Parameters.AddWithValue("@status", "INSERT");

                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating listing");
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public async Task<ClaimsPrincipal> LogUserIn(Enroll e)
        {
            try
            {
                if (con != null && con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                var cmd = new SqlCommand("SP_LogUserIn", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Email", e.Email);
                cmd.Parameters.AddWithValue("@Password", e.Password);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    if (rdr.Read())
                    {
                        var role = rdr["Role"].ToString();
                        // Create claims for the authenticated user
                        var claims = new List<Claim>
                        
                        {
                            new Claim(ClaimTypes.Name, e.Email),       // User's email as claim
                            new Claim(ClaimTypes.Role, role)         // Example user role claim
                        };

                        var identity = new ClaimsIdentity(claims, "Login"); // Create identity with claims
                        var principal = new ClaimsPrincipal(identity);      // Create principal

                        cmd = new SqlCommand("SP_AddLogToDb", con);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Operation", "LOGIN");
                        cmd.Parameters.AddWithValue("@Details", "User LOGGED IN with email address = " + e.Email + " and Role : " + role);
                        cmd.Parameters.AddWithValue("@LogDate", DateTime.Now);

                        cmd.ExecuteNonQuery();

                        // Sign in the user using the claims principal
                        return principal;
                    }
                    else
                    {
                        cmd = new SqlCommand("SP_AddLogToDb", con);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Operation", "LOGIN");
                        cmd.Parameters.AddWithValue("@Details", "User login FAILED with Role = " + rdr["Role"].ToString());
                        cmd.Parameters.AddWithValue("@LogDate", DateTime.Now);

                        cmd.ExecuteNonQuery();

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Logging user in");
                return null;
            }
            finally
            {
                con.Close();
            }
        }
    }
}

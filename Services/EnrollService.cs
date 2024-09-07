using System.Data.SqlClient;
using System.Data;
using DivarClone.Controllers;
using DivarClone.Models;

namespace DivarClone.Services
{
    public interface IEnrollService
    {
        Task<bool> EnrollUser(Enroll enroll);
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
                cmd.Parameters.AddWithValue("@Password", e.Password);
                cmd.Parameters.AddWithValue("@Email", e.Email);
                cmd.Parameters.AddWithValue("@Phone", e.PhoneNumber);
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
    }
}

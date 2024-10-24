using System.Data.SqlClient;
using System.Data;
using DivarClone.Controllers;
using DivarClone.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Azure;


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
                cmd.Parameters.AddWithValue("@status", "INSERT");

                await cmd.ExecuteNonQueryAsync();

                _logger.LogDebug("User Registered Successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Registering User");
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
				_logger.LogTrace("connection to db opened");
				
				var cmd = new SqlCommand("SP_LogUserIn", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", e.Email);
                cmd.Parameters.AddWithValue("@Password", e.Password);

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    try
                    {
                        if (rdr.Read())
                        {
                            var userId = rdr["ID"].ToString();
                            var username = rdr["Username"].ToString();

                            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, username),
                            new Claim(ClaimTypes.Email, e.Email),
                            new Claim(ClaimTypes.NameIdentifier, userId)
                        };

                            _logger.LogTrace("Base Claims added for user : " + username);

                            var roleCmd = new SqlCommand("SP_GetRoleFromUserRoles", con);
                            roleCmd.CommandType = CommandType.StoredProcedure;
                            roleCmd.Parameters.AddWithValue("@UserId", userId);

                            using (SqlDataReader roleReader = await roleCmd.ExecuteReaderAsync())
                            {
                                while (roleReader.Read())
                                {
                                    var role = roleReader["RoleName"].ToString();
                                    claims.Add(new Claim(ClaimTypes.Role, role));
                                    _logger.LogTrace($"{role}");
								}
                            }

                            var permissionsList = new List<String>();

                            var permCmd = new SqlCommand("SP_GetUserPermissions", con);
							permCmd.CommandType = CommandType.StoredProcedure;
							permCmd.Parameters.AddWithValue("@UserId", userId);

							using (SqlDataReader permReader = await permCmd.ExecuteReaderAsync())
							{
								while (permReader.Read())
								{
									var permission = permReader["PermissionName"].ToString();
                                    //claims.Add(new Claim(CustomClaims.Permission, permission));
                                    permissionsList.Add(permission);

                                    _logger.LogTrace($"{permission}");
								}
							}

							// Get special permissions
							var specialPermCmd = new SqlCommand("SP_GetSpecialUserPermissions", con); // Your new stored procedure
							specialPermCmd.CommandType = CommandType.StoredProcedure;
							specialPermCmd.Parameters.AddWithValue("@UserId", userId);

							using (SqlDataReader specialPermReader = await specialPermCmd.ExecuteReaderAsync())
							{
								while (specialPermReader.Read())
								{
									var specialPermission = specialPermReader["PermissionName"].ToString();

                                    //claims.Add(new Claim(CustomClaims.Permission, specialPermission));
                                    if (permissionsList.Contains(specialPermission) == false)
                                    {
                                        permissionsList.Add(specialPermission);

                                    } else
                                    {
                                        System.Diagnostics.Debug.WriteLine("\nError Message : Permission already granted by Role : " + specialPermission);
                                    }
                                    

                                    _logger.LogTrace($"Special permission added: {specialPermission}");
								}
							}

                            //permissionsList = permissionsList.Distinct().ToList();

                            foreach ( var permission in permissionsList)
                            {
                                claims.Add(new Claim(CustomClaims.Permission, permission));
                            }

							foreach (Claim claim in claims)
                            {
								System.Diagnostics.Debug.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
							}

                            var identity = new ClaimsIdentity(claims, "Login");
                            var principal = new ClaimsPrincipal(identity);

                            _logger.LogTrace("User logged in successfully");

                            return principal;
                        }
                        else { 
                            return null;
                        }
                    } catch (Exception ex)
                    {
						_logger.LogError(ex, "Error logging user in");
						return null;
					}
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging user in");
                return null;
            }
            finally
            {
                con.Close();
            }
        }

    }
}

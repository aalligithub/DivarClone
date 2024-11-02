using System.Data.SqlClient;
using System.Data;
using DivarClone.Controllers;
using DivarClone.Models;
using System.Reflection;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace DivarClone.Services
{
    public interface IAdminService
    {
        public List<Enroll> GetAllUsers();

        public List<Enroll> SearchUsers(string Username);

        Task<bool> ChangeUserRoles(int Id, int Role);

        Task<bool> GiveUserSpecialPermission(int Id, int Role);

        Task<bool> RemoveUserSpecialPermission(int Id, string PermissionName);

		Task<Dictionary<String, String>> GetAllPossibleRoles();

        Task<Dictionary<String, String>> GetAllPossiblePermissions();
    }
    public class AdminService : IAdminService
    {
        public string Constr { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<AdminDashboardController> _logger;

        public AdminService(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, ILogger<AdminDashboardController> logger)
        {
            _configuration = configuration;
            Constr = _configuration.GetConnectionString("DivarCloneContextConnection");
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;

            con = new SqlConnection(Constr);
        }

        public List<Enroll> GetAllUsers()
        {
            List<Enroll> UsersList = new List<Enroll>();
            try
            {
                if (con != null && con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                var cmd = new SqlCommand("SP_GetAllUsers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader rdr = cmd.ExecuteReader();

                return GiveUsersRolesAndPermissions(rdr);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating listing");
                throw;
            }
            finally { con.Close(); }
        }

        public List<Enroll> SearchUsers(string Username)
        {
            List<Enroll> UsersList = new List<Enroll>();
            try
            {
                if (con != null && con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                var cmd = new SqlCommand("SP_SearchUsers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@username", Username);

                SqlDataReader rdr = cmd.ExecuteReader();

                return GiveUsersRolesAndPermissions(rdr);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating listing");
                throw;
            }
            finally { con.Close(); }
        }

        public List<Enroll> GiveUsersRolesAndPermissions(SqlDataReader rdr)
        {
            List<Enroll> UsersList = new List<Enroll>();

            while (rdr.Read())
            {
                var ID = rdr.GetInt32("ID");

                var cmdb = new SqlCommand("SP_GetRoleFromUserRoles", con);
                cmdb.CommandType = System.Data.CommandType.StoredProcedure;

                cmdb.Parameters.AddWithValue("@UserId", ID);

                SqlDataReader roleRdr = cmdb.ExecuteReader();
                string role = null;
                if (roleRdr.Read())
                {
                    role = roleRdr["RoleName"].ToString();
                }
                roleRdr.Close();

                var cmdbs = new SqlCommand("SP_GetUserPermissions", con);
                cmdbs.CommandType = System.Data.CommandType.StoredProcedure;

                cmdbs.Parameters.AddWithValue("@UserId", ID);

                SqlDataReader perRdr = cmdbs.ExecuteReader();
                List<string> permissions = new List<string>();
                while (perRdr.Read())
                {
                    permissions.Add(perRdr["PermissionName"].ToString());
                }
                perRdr.Close();

                var cmdbps = new SqlCommand("SP_GetSpecialUserPermissions", con);
                cmdbps.CommandType = System.Data.CommandType.StoredProcedure;

                cmdbps.Parameters.AddWithValue("@UserId", ID);

                SqlDataReader sperRdr = cmdbps.ExecuteReader();
                List<string> specialPermissions = new List<string>();

                while (sperRdr.Read())
                {
                    specialPermissions.Add(sperRdr["PermissionName"].ToString());
                }
                sperRdr.Close();

                Enroll list = new Enroll
                {
                    ID = ID,
                    FirstName = rdr["FirstName"].ToString(),
                    Username = rdr["Username"].ToString(),
                    Email = rdr["Email"].ToString(),
                    Password = rdr["Password"].ToString(),
                    PhoneNumber = rdr["Phone"].ToString(),
                    Role = role,
                    Permissions = permissions,
                    SpecialPermissions = specialPermissions
                };
                UsersList.Add(list);

            }

            return UsersList.ToList();
        }

        [Authorize]
        public async Task<bool> ChangeUserRoles(int Id, int Role)
        {
            try
            {
                if (con != null && con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                var cmd = new SqlCommand("SP_ChangeUserRole", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", Id);
                cmd.Parameters.AddWithValue("@Role", Role);

                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to change users role");
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<bool> GiveUserSpecialPermission(int Id, int PermissionId)
        {
            try
            {
                if (con != null && con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                var cmd = new SqlCommand("SP_GiveUserSpecialPermission", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", Id);
                cmd.Parameters.AddWithValue("@PermissionId", PermissionId);

                await cmd.ExecuteNonQueryAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to give user special permission");
                return false;
            }
        }

		public async Task<bool> RemoveUserSpecialPermission(int Id, string PermissionName)
		{
			try
			{
				if (con != null && con.State == ConnectionState.Closed)
				{
					con.Open();
				}
				var cmd = new SqlCommand("SP_RemoveUserSpecialPermission", con);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;

				cmd.Parameters.AddWithValue("@UserId", Id);
                cmd.Parameters.AddWithValue("@PermissionName", PermissionName);

				await cmd.ExecuteNonQueryAsync();

				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to remove user special permission");
				return false;
			}
		}

		public async Task<Dictionary<String, String>> GetAllPossibleRoles()
        {
            try
            {
                if (con != null && con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                var cmd = new SqlCommand("SP_GetAllPossibleRoles", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader rdr = await cmd.ExecuteReaderAsync();

                Dictionary<string, string> AllPossibleRoles = new Dictionary<string, string>();

                while (rdr.Read()) {
                    var RoleId = rdr["RoleId"].ToString();
                    var RoleName = rdr["RoleName"].ToString();

                    AllPossibleRoles.Add(RoleId, RoleName);
                }

                return AllPossibleRoles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Get All Possible Roles");
                return null;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        public async Task<Dictionary<String, String>> GetAllPossiblePermissions()
        {
            try
            {
                if (con != null && con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                var cmd = new SqlCommand("SP_GetAllPossiblePermissions", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader rdr = await cmd.ExecuteReaderAsync();

                Dictionary<string, string> AllPossiblePermissions = new Dictionary<string, string>();

                while (rdr.Read())
                {
                    var permissionId = (rdr["PermissionId"].ToString());
                    var permissionName = (rdr["PermissionName"].ToString());

                    AllPossiblePermissions.Add(permissionId, permissionName);
                }

                return AllPossiblePermissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Get all Permissions");
                return null;
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
    }
}

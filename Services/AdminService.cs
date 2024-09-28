﻿using System.Data.SqlClient;
using System.Data;
using DivarClone.Controllers;
using DivarClone.Models;
using System.Reflection;

namespace DivarClone.Services
{
    public interface IAdminService
    {
        public List<Enroll> GetAllUsers();
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

					while (sperRdr.Read())
					{
						permissions.Add(sperRdr["PermissionName"].ToString());
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
                        Permissions = permissions
                    };
                    UsersList.Add(list);
                    
                }

                return UsersList.ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating listing");
                throw;
            }
            finally { con.Close(); }
        }
    }
}
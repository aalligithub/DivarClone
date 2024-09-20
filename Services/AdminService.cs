using System.Data.SqlClient;
using System.Data;
using DivarClone.Controllers;
using DivarClone.Models;

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
                    Enroll list = new Enroll
                    {
                        ID = rdr.GetInt32("ID"),
                        FirstName = rdr["FirstName"].ToString(),
                        Username = rdr["Username"].ToString(),
                        Email = rdr["Email"].ToString(),
                        Password = rdr["Password"].ToString(),                   
                        PhoneNumber = rdr["Phone"].ToString(),
                        Role = rdr["Role"].ToString(),
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

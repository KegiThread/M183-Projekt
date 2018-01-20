using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Role_Based_Authorization.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Dashboard()
        {

            var current_user = (string)Session["role"];
            var current_user_id = (int)Session["userid"];
            var user_roles = MvcApplication.UserRoles;
            var current_user_role = "";

            try
            {
                current_user_role = (string)user_roles[current_user];
            }
            catch (Exception)
            {

            }

            if (current_user_role == "User")
            {
                // Authentifizierung erfolgreich

                SqlConnection connection = new SqlConnection();
                if (System.Security.Principal.WindowsIdentity.GetCurrent().Name == "Kueng\\Samuels PC")
                {
                    connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Work\\Visual Studio\\Git\\Project\\Ressourcen_Projekt\\m183_project.mdf;Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30;Application Name=EntityFramework";
                }
                else
                {
                    connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Docs\\git\\Project\\Ressourcen_Projekt\\m183_project.mdf;Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30;Application Name=EntityFramework";
                }

                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;

                cmd.CommandText = "SELECT * FROM [dbo].[Post]";
                cmd.Connection = connection;

                connection.Open();

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    List<List<object>> table = new List<List<object>>(); 
                    while (reader.Read())
                    {
                        List<object> newTable = new List<object>();

                        for (int i = 0; i < 8; i++) 
                        {
                            try
                            {
                                newTable.Add(reader.GetString(i));
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    newTable.Add(reader.GetInt32(i));
                                }
                                catch (Exception)
                                {
                                    // Do nothing
                                }
                            }
                        }

                        if (newTable[0].Equals(current_user_id))
                        {
                            try
                            {
                                if (newTable[7] == null)
                                {
                                    table.Add(newTable);
                                }
                            }
                            catch (Exception)
                            {
                                table.Add(newTable);
                            }
                        }
                    }

                    ViewBag.table = table;
                }
                else
                {
                    ViewBag.Message = "Wrong Credentials";
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
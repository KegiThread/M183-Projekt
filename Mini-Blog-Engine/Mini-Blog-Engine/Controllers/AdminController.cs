using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Role_Based_Authorization.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Dashboard()
        {
            var current_user = (string)Session["role"];

            try
            {
                var current_user_id = (int)Session["userid"];
            }
            catch (Exception)
            {
                
            }

            var current_user_role = "";
            var user_roles = MvcApplication.UserRoles;

            try
            {
                current_user_role = (string)user_roles[current_user];
            }
            catch (Exception)
            {
                
            }

            var searchvalue = Request["searchvalue"];

            if (current_user_role == "Administrator")
            {
                // access granted

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

                        for (int i = 0; i < 8; i++) // one row
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
                                    // Does nothing
                                }
                            }
                        }

                        if (searchvalue != null)
                        {
                            bool toBeAdded = false;
                            for (int i = 0; i < 8; i++)
                            {
                                try
                                {
                                    if ((i != 0) && (i != 4) && (i != 5) && (i != 6) && (i != 7))
                                    {
                                        if (newTable[i].ToString().ToLower().Contains(searchvalue.ToLower()))
                                        {
                                            toBeAdded = true;
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    // Do nothing
                                }
                            }

                            if (toBeAdded)
                            {
                                table.Add(newTable);
                            }
                        }
                        else
                        {
                            table.Add(newTable);
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
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/*
 * 1) Warum haben Sie sich für gerade für den Hash Algorithmus (Usernamen & Passwort) entschieden?
 * Wir haben keinen eingebaut, doch wenn, dann SHA-3 oder SHA-256
 *  2) In der User-Login-Tabelle ist noch ein Feld für die IP-Adresse Reserviert. Welche Attacke lässt sich dadurch verhindern?
 * Eavesdropping 
 * 3) Erklären Sie, wie diese Attacke genau funktioniert und inwiefern die Gegenmassnahmen die Attacke vereitelt?
 * hat man die Session-id und zugangsdaten, kann genau gleich zugegriffen als waere der Hacker der user. Wird aber die IP (beim Request) auch verlangt,
   merkt das system das die IPs nicht uebereinanderstimmen und entdeckt dass moeglicherweise etwas gehackt worden ist. 
 * 
*/

namespace Role_Based_Authorization.Controllers
{
    public class HomeController : Controller
    {


        public ActionResult Index()
        {
            List<Models.BlogPost> posts = new List<Models.BlogPost>();
            SqlDataReader reader = createConnection("SELECT * FROM [Post] WHERE [DeletedOn] IS NULL");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Models.BlogPost post = new Models.BlogPost();
                    post.Id = reader.GetInt32(0);
                    post.Title = reader.GetString(2);
                    post.Description = reader.GetString(3);
                    post.Content = reader.GetString(4);
                    posts.Add(post);
                }
            }
            return View(posts);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Login()
        {
            var username = Request["username"];
            var password = Request["password"];

            string query = "SELECT [Id], [Username], [Password], [Firstname], [Familyname], [Mobilephonenumber], [Role], [Status] FROM [dbo].[User] WHERE [Username] = '" + username + "' AND [Password] = '" + password + "'";
            SqlDataReader reader = createConnection(query);

            if (reader.HasRows)
            {
                Session["username"] = username;
                string role = "";
                while (reader.Read())
                {
                    Session["userid"] = reader.GetInt32(0);
                    role = reader.GetString(6);
                }
                if (role == "admin")
                {
                    Session["role"] = "admin";
                    return RedirectToAction("Dashboard", "Admin");
                }
                else if (role == "user")
                {
                    Session["role"] = "user";
                    return RedirectToAction("Dashboard", "User");
                }
            }
            else
            {
                ViewBag.Message = "Wrong Credentials";
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        private SqlDataReader createConnection(string sql)
        {
            SqlConnection connection = new SqlConnection();
            if (System.Security.Principal.WindowsIdentity.GetCurrent().Name == "Kueng\\Samuels PC")
            {
                connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Work\\Visual Studio\\Git\\Project\\Ressourcen_Projekt\\m183_project.mdf;Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30;Application Name=EntityFramework";
            }
            else
            {
                connection.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Docs\\git\\Project\\Ressourcen_Projekt\\m183_project.mdf;Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30;Application Name=EntityFramework";
            }
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.Connection = connection;
            sqlcommand.CommandText = sql;
            connection.Open();
            return sqlcommand.ExecuteReader();
        }
    }
}
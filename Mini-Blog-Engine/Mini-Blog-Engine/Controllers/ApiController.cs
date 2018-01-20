using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;

namespace Role_Based_Authorization.Controllers
{
    public class ApiController : System.Web.Http.ApiController
    {
        public string posts()
        {
            List<Models.BlogPost> posts = new List<Models.BlogPost>();
            SqlCommand command = createConnection("SELECT * FROM [Post] WHERE [DeletedOn] IS NULL");
            SqlDataReader reader = command.ExecuteReader();
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
            string json = JsonConvert.SerializeObject(posts);
            return json;
        }

        public string posts(string postid)
        {
            Models.BlogPost post = new Models.BlogPost();
            string sql = "SELECT * FROM [Post] WHERE [Id] = '" + postid + "' AND [DeletedOn] IS NULL";
            string query2 = "SELECT * FROM [Comment] WHERE [PostId] = " + postid + "";
            SqlCommand command = createConnection(sql);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    post.Id = reader.GetInt32(0);
                    post.Title = reader.GetString(2);
                    post.Description = reader.GetString(3);
                    post.Content = reader.GetString(4);
                }
            }
            SqlCommand commandComment = createConnection(query2);
            SqlDataReader commentReader = commandComment.ExecuteReader();
            List<Models.Comment> comments = new List<Models.Comment>();
            if (commentReader.HasRows)
            {
                while (commentReader.Read())
                {
                    Models.Comment comment = new Models.Comment();
                    comment.Text = commentReader.GetString(3);
                    comments.Add(comment);
                }
            }
            post.Comments = comments;

            string json = JsonConvert.SerializeObject(post);
            return json;
        }

        private bool checkToken(string token)
        {
            HashAlgorithm algorithm = MD5.Create();
            byte[] hashedValue = algorithm.ComputeHash(Encoding.UTF8.GetBytes(token));

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashedValue)
                sb.Append(b.ToString("X2"));
            Console.WriteLine("Hash: " + sb.ToString());
            string sql = "SELECT * FROM [User] WHERE [Username] = 'API-User' AND [Password]=@password";
            SqlCommand command = createConnection(sql);
            command.Parameters.AddWithValue("password", sb.ToString());
            SqlDataReader datareader = command.ExecuteReader();
            if (datareader.HasRows && datareader.Read())
            {
                return true;
            }
            return false;
        }

        private SqlCommand createConnection(string sql)
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
            return sqlcommand;
        }
    }
}
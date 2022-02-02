using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using todo.Models;

namespace todo.Controllers
{
    public class TodoController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public void Insert(Todo todo)
        {
            using (SqlConnection con =
                   new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=Todo"))
            {
                using (var tableCmd = con.CreateCommand())
                {
                    con.Open();
                    tableCmd.CommandText = $"INSERT INTO todo (name) VALUES ('{todo.Name}')";
                    try
                    {
                        tableCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}

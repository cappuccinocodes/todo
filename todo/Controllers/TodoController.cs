using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using todo.Models;
using todo.ViewModels;

namespace todo.Controllers
{
    public class TodoController: Controller
    {
        public IActionResult Index()
        {
            var tdlvm = GetAllTodos();
            return View(tdlvm);
        }

        internal TodoViewModel GetAllTodos()
        {
            List<Todo> todoList = new();

            using (var connection =
                   new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=Todo"))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = "SELECT * FROM todo";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                todoList.Add(
                                    new Todo
                                    {
                                        Id = reader.GetInt32(0),
                                        Name = reader.GetString(1)
                                    });
                            }
                        }
                        else
                        {
                            return new TodoViewModel
                            {
                                TodoList = todoList
                            };
                        }
                    };
                }
            }

            return new TodoViewModel
            {
                TodoList = todoList
            };
        }

        public RedirectResult Insert(Todo todo)
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

            return Redirect("https://localhost:7000/");
        }


    }
}

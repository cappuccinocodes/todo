using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using todo.Models;
using todo.ViewModels;

namespace todo.Controllers
{
    public class TodoController: Controller
    {
        private readonly IConfiguration _configuration;

        public TodoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var todoListViewModel = GetAllTodos();
            return View(todoListViewModel);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (var connection =
                   new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"DELETE from todo WHERE Id = '{id}'";
                    tableCmd.ExecuteNonQuery();
                }
            }

            return new EmptyResult();
        }

        [HttpGet]
        public JsonResult PopulateForm(int id)
        {
            var todo = GetById(id);
            return Json(todo);
        }

        internal Todo GetById(int id)
        {
            Todo todo = new();

            using (var connection =
                   new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=Todo"))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"SELECT * FROM todo Where Id = '{id}'";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            todo.Id = reader.GetInt32(0);
                            todo.Name = reader.GetString(1);
                        }
                        else
                        {
                            return todo;
                        }
                    };
                }
            }

            return todo;
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

        public RedirectResult Update(Todo todo)
        {
            using (SqlConnection con =
                   new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=Todo"))
            {
                using (var tableCmd = con.CreateCommand())
                {
                    con.Open();
                    tableCmd.CommandText = $"UPDATE todo SET name = '{todo.Name}' WHERE Id = '{todo.Id}'";
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

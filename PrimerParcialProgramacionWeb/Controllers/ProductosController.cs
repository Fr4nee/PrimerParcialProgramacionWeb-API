using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrimerParcialProgramacionWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductosController : Controller
    {
        public static string ConnectionString = "data source=localhost; Initial Catalog=ecommerce; Integrated Security=True";

        [HttpGet("ListarProductos")]
        public object ListarProductos()
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = ConnectionString;

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        connection.Open();
                        command.CommandText = @"select * from Productos_deff";
                        command.ExecuteNonQuery();

                        SqlDataAdapter da = new SqlDataAdapter(command);

                        using (da)
                        {
                            da.Fill(ds);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return (IActionResult)ex;
            }

            Object jsonn = Logicas.dataSetToJSON(ds);
            return jsonn;
        }

        [HttpPost("CargarProducto")]
        public string CargarProducto(string nombre, long precio, string imagen, string categoria, int stock, string descripcion)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();
                    string sql = "insert into Productos_deff (nombre, precio, imagen, categoria, stock, descripcion) values (@nombre, @precio, @imagen, @categoria, @stock, @descripcion)";
                    SqlTransaction transaction = connection.BeginTransaction();

                    SqlCommand command = new SqlCommand(sql, connection, transaction);

                    command.Parameters.Add(new SqlParameter("@nombre", nombre));
                    command.Parameters.Add(new SqlParameter("@precio", precio));
                    command.Parameters.Add(new SqlParameter("@imagen", imagen));
                    command.Parameters.Add(new SqlParameter("@categoria", categoria));
                    command.Parameters.Add(new SqlParameter("@stock", stock));
                    command.Parameters.Add(new SqlParameter("@descripcion", descripcion));

                    command.Connection = connection;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    SqlDataAdapter da = new SqlDataAdapter(command);

                    try
                    {
                        int idProd = Logicas.DevolverIDProducto(nombre);
                        int idCat = Logicas.DevolverIDCategoria(categoria);
                        Logicas.CargarDatosCatProd(idProd, idCat);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return $"Ocurrio un error en insertar datos en la tabla intermedia. {ex}";
                    }
                }
                catch (Exception ex)
                {
                    return $"Ocurrio un error al cargar el producto. {ex}";
                }
                return "El producto se cargo correctamente.";
            }
        }

        [HttpPost("EditarProducto")]
        public string EditarCliente(string nombre, long precio, string imagen, string categoria, int stock, string descripcion, int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = ConnectionString;

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Parameters.Add(new SqlParameter("@nombre", nombre));
                        command.Parameters.Add(new SqlParameter("@precio", precio));
                        command.Parameters.Add(new SqlParameter("@imagen", imagen));
                        command.Parameters.Add(new SqlParameter("@categoria", categoria));
                        command.Parameters.Add(new SqlParameter("@stock", stock));
                        command.Parameters.Add(new SqlParameter("@descripcion", descripcion));
                        command.Parameters.Add(new SqlParameter("@id", id));

                        command.Connection = connection;
                        connection.Open();
                        command.CommandText = @"update Productos_deff set nombre = @nombre , precio = @precio, imagen = @imagen , categoria = @categoria, stock = @stock, descripcion = @descripcion where id = @id";
                        command.ExecuteNonQuery();

                        SqlDataAdapter da = new SqlDataAdapter(command);
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "El producto se editó correctamente.";
        }

        [HttpPost("DeleteProducto")]
        public string EliminarProducto(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = ConnectionString;

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Parameters.Add(new SqlParameter("@id", id));

                        command.Connection = connection;
                        connection.Open();
                        command.CommandText = @"Delete from Productos_deff where id = @id";
                        command.ExecuteNonQuery();

                        SqlDataAdapter da = new SqlDataAdapter(command);
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "El producto se eliminó correctamente.";
        }

        [HttpGet("DevolverProducto")]
        public object DevolverProducto(int id)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = ConnectionString;

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Parameters.Add(new SqlParameter("@id", id));

                        command.Connection = connection;
                        connection.Open();
                        command.CommandText = @"select * from Productos_deff where id = @id";
                        command.ExecuteNonQuery();

                        SqlDataAdapter da = new SqlDataAdapter(command);

                        using (da)
                        {
                            da.Fill(ds);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ex;
            }

            var jsonn = Logicas.dataSetToJSON(ds);
            return jsonn;
        }


    }
}

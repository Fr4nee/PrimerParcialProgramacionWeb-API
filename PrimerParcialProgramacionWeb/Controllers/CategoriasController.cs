using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrimerParcialProgramacionWeb.Controllers
{
    public class CategoriasController : Controller
    {
        public static string ConnectionString = "data source=localhost; Initial Catalog=ecommerce; Integrated Security=True";

        [HttpGet("ListarCategorias")]
        public object ListarCategorias()
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
                        command.CommandText = @"select * from Categorias_deff";
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

            }
            Object jsonn = Logicas.dataSetToJSON(ds);
            string jsasdf = jsonn.ToString();
            return jsasdf;
        }

        [HttpGet("DevolverCategoria")]
        public object DevolverCategoria(int id)
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
                        command.CommandText = @"select * from Categorias_deff where id = @id";
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

        [HttpPost("CargarCategoria")]
        public string CargarCategoria(string nombre, string descripcion)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();
                string sql = "insert into Categorias_deff (nombre, descripcion) values (@nombre, @descripcion)";
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    SqlCommand command = new SqlCommand(sql, connection, transaction);

                    command.Parameters.Add(new SqlParameter("@nombre", nombre));
                    command.Parameters.Add(new SqlParameter("@descripcion", descripcion));

                    command.Connection = connection;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    SqlDataAdapter da = new SqlDataAdapter(command);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return $"Ocurrio un error al cargar la categoria. {ex}";
                }
                return "La categoria se cargo correctamente.";
            }
        }

        [HttpPut("EditarCategoria")]
        public string EditarCategoria(int id, string nombre, string descripcion)
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
                        command.Parameters.Add(new SqlParameter("@nombre", nombre));
                        command.Parameters.Add(new SqlParameter("@descripcion", descripcion));

                        command.Connection = connection;
                        connection.Open();
                        command.CommandText = @"update Categorias_deff set nombre = @nombre, descripcion = @descripcion where id = @id";
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
                return ex.ToString();
            }

            return $"La categoria {nombre} se editó correctamente.";
        }

        [HttpDelete("EliminarCategoria")]
        public string EliminarCategoria(int id)
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
                        command.CommandText = @"delete from Categorias_deff where id = @id";
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
                return ex.ToString();
            }

            return "La categoria se eliminó correctamente.";
        }
    }
    
}

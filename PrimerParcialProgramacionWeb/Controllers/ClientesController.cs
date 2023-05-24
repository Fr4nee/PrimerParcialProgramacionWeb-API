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
    public class ClientesController : Controller
    {
        public static string ConnectionString = "data source=localhost; Initial Catalog=ecommerce; Integrated Security=True";

        [HttpGet("ListarClientes")]
        public object ListarClientes()
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
                        command.CommandText = @"select * from Clientes_deff";
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

            Object jsonn = Logicas.dataSetToJSON(ds);
            return jsonn;
        }      

        [HttpPost("RegistrarCliente")]
        public string RegistrarCliente(string nombre, string apellido, string email, string telefono, string direccion, string contraseña)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = ConnectionString;

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Parameters.Add(new SqlParameter("@nombre", nombre));
                        command.Parameters.Add(new SqlParameter("@apellido", apellido));
                        command.Parameters.Add(new SqlParameter("@email", email));
                        command.Parameters.Add(new SqlParameter("@telefono", telefono));
                        command.Parameters.Add(new SqlParameter("@direccion", direccion));
                        command.Parameters.Add(new SqlParameter("@contraseña", contraseña));

                        command.Connection = connection;
                        connection.Open();
                        command.CommandText = @"insert into Clientes_deff (nombre, apellido, email, telefono, direccion)
                        values (@nombre, @apellido, @email, @telefono, @direccion)";
                        command.ExecuteNonQuery();

                        SqlDataAdapter da = new SqlDataAdapter(command);
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "El cliente se registró correctamente.";
        }

        [HttpPost("EditarCliente")]
        public string EditarCliente(string nombre, string apellido, string email, string telefono, string direccion, string contraseña, int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = ConnectionString;

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Parameters.Add(new SqlParameter("@nombre", nombre));
                        command.Parameters.Add(new SqlParameter("@apellido", apellido));
                        command.Parameters.Add(new SqlParameter("@email", email));
                        command.Parameters.Add(new SqlParameter("@telefono", telefono));
                        command.Parameters.Add(new SqlParameter("@direccion", direccion));
                        command.Parameters.Add(new SqlParameter("@contraseña", contraseña));
                        command.Parameters.Add(new SqlParameter("@id", id));

                        command.Connection = connection;
                        connection.Open();
                        command.CommandText = @"update Clientes_deff set nombre = @nombre , apellido = @apellido, direccion = @direccion , telefono = @telefono, email = @email, contraseña = @contraseña where id = @id";
                        command.ExecuteNonQuery();

                        SqlDataAdapter da = new SqlDataAdapter(command);
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "El cliente se editó correctamente.";
        }
    }
}

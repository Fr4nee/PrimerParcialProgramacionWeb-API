using System.Collections;
using System.Data;
using Nancy.Json;
using System.Data.SqlClient;

namespace PrimerParcialProgramacionWeb
{
    public class Logicas
    {
        public static string ConnectionString = "data source=localhost; Initial Catalog=ecommerce; Integrated Security=True";

        public static object dataSetToJSON(DataSet ds)
        {
            ArrayList root = new ArrayList();
            List<Dictionary<string, object>> table;
            Dictionary<string, object> data;

            foreach (DataTable dt in ds.Tables)
            {
                table = new List<Dictionary<string, object>>();
                foreach (DataRow dr in dt.Rows)
                {
                    data = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        data.Add(col.ColumnName, dr[col]);
                    }
                    table.Add(data);
                }
                root.Add(table);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(root);
        }

        public static List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row => {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex) { }
                    }
                }
                return objT;
            }).ToList();
        }



        public static int DevolverIDProducto(string nombre)
        {
            int id = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = ConnectionString;

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Parameters.Add(new SqlParameter("@nombre", nombre));

                        command.Connection = connection;
                        connection.Open();
                        command.CommandText = @"select id from Productos_deff where nombre = @nombre";
                        command.ExecuteNonQuery();

                        SqlDataAdapter da = new SqlDataAdapter(command);

                        using (da)
                        {
                            id = (int)command.ExecuteScalar();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            return id;
        }

        public static int DevolverIDCategoria(string nombre)
        {
            int id = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = ConnectionString;

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Parameters.Add(new SqlParameter("@nombre", nombre));
                        command.Connection = connection;
                        connection.Open();
                        command.CommandText = @"select id from Categorias_deff where nombre = @nombre";
                        command.ExecuteNonQuery();

                        SqlDataAdapter da = new SqlDataAdapter(command);

                        using (da)
                        {
                            id = (int)command.ExecuteScalar();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            return id;
        }

        public static void CargarDatosCatProd(int idProd, int idCat)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = ConnectionString;

                using (SqlCommand command = new SqlCommand())
                {
                    command.Parameters.Add(new SqlParameter("@idProd", idProd));
                    command.Parameters.Add(new SqlParameter("@idCat", idCat));
                    command.Connection = connection;
                    connection.Open();
                    command.CommandText = @"insert into Productos_Categorias_Deff (idProducto, idCategoria) values (@idProd, @idCat)";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales
{
    public class PlantillaDeIdentificadoresGeneralesRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public PlantillaDeIdentificadoresGeneralesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public bool Insertar(string Num_Refe, int MiIDCliente, int idOficina, int MiOperacion, string Clave)
        {

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            try
            {
                cmd.CommandText = "NET_INSERT_PLANTILLADEIDENTIFICADORESGENERALES";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @MyNum_Refe VARCHAR(15)
                @param = cmd.Parameters.Add("@MyNum_Refe", SqlDbType.VarChar, 15);
                @param.Value = Num_Refe;

                // , @MyIDCliente INT
                @param = cmd.Parameters.Add("@MyIDCliente", SqlDbType.Int, 4);
                @param.Value = MiIDCliente;

                // , @MyOficina INT 
                @param = cmd.Parameters.Add("@MyOficina", SqlDbType.Int, 4);
                @param.Value = idOficina;

                // , @MyOperacion INT 
                @param = cmd.Parameters.Add("@MyOperacion", SqlDbType.Int, 4);
                @param.Value = MiOperacion;

                // , @MyCVEPEDI varchar(2)
                @param = cmd.Parameters.Add("@MyCVEPEDI", SqlDbType.VarChar, 2);
                @param.Value = Clave;



                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.ExecuteNonQuery();


                cn.Close();
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return true;

        }

    }
}

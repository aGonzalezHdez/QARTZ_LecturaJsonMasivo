using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesTurnado;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesOperaciones
{
    public class BitacoraGeneralRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public BitacoraGeneralRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int Insertar(BitacoraGeneral lbitacorageneral)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            try
            {
                cmd.CommandText = "NET_INSERT_CASAEI_BITACORAGENERAL";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                // ,@Descripcion  varchar
                @param = cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 250);
                @param.Value = lbitacorageneral.Descripcion;


                // ,@IdUsuario  int
                @param = cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4);
                @param.Value = lbitacorageneral.IdUsuario;

                // ,@IdReferencia  int
                @param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
                @param.Value = lbitacorageneral.IdReferencia;

                // ,@Modulo  varchar
                @param = cmd.Parameters.Add("@Modulo", SqlDbType.VarChar, 25);
                @param.Value = lbitacorageneral.Modulo;


                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;


                cn.ConnectionString = sConexion;
                cn.Open();
                cmd.ExecuteNonQuery();
                id = (int)cmd.Parameters["@newid_registro"].Value;
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_INSERT_CASAEI_BITACORAGENERAL");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }
    }
}

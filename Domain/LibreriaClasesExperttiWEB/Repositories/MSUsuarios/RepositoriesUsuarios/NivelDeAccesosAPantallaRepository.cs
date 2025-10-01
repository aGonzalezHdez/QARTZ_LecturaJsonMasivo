using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSUsuarios.RepositoriesUsuarios
{
    public class NivelDeAccesosAPantallaRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public NivelDeAccesosAPantallaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = configuration.GetConnectionString("dbCASAEI");
        }

        public async Task<DataTable> Nivel(int IdPantalla, int IdUsuario)
        {
            DataTable dt = new();
            using (SqlConnection cnn = new(sConexion))
            {
                using (SqlDataAdapter adaptador = new("NET_LOAD_VALIDAROLESUSUARIOPANTALLA", cnn))
                {
                    adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adaptador.SelectCommand.Parameters.Add("@IDPANTALLA", SqlDbType.Int).Value = IdPantalla;
                    adaptador.SelectCommand.Parameters.Add("@IDUSUARIO", SqlDbType.Int).Value = IdUsuario;
                    await cnn.OpenAsync();
                    using SqlDataReader reader = await adaptador.SelectCommand.ExecuteReaderAsync();
                    dt.Load(reader);
                }
            }
            return dt;
        }

    }
}

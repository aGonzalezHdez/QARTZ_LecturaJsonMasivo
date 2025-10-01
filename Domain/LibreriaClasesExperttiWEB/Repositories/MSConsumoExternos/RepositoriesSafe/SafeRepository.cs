using LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.Safe;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesSafe
{
    public class SafeRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public SafeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public Safe Buscar(int IdOficina, int @IDCliente)
        {
            Safe? objSafeUri = new();      

            try
            {
                using (con = new SqlConnection(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_CASAEI_ONECOREAPI_SAFE", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = IdOficina;
                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = @IDCliente;

                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        dr.Read();
                        objSafeUri.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                        objSafeUri.UriFirma = dr["UriFirma"].ToString();
                        objSafeUri.UriObtenerHallazgos = dr["UriObtenerHallazgos"].ToString();
                        objSafeUri.UriJustificarHallazgos = dr["UriJustificarHallazgos"].ToString();
                        objSafeUri.Usuario = dr["Usuario"].ToString();
                        objSafeUri.Token = dr["Token"].ToString();
                        objSafeUri.RFC = dr["RFC"].ToString();
                    }
                    else objSafeUri = null;
                }  
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objSafeUri;
        }       
    }
}

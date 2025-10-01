using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class WebApisRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public WebApisRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public WebApis Buscar(int IdWebApi)
        {
            WebApis objWebApis = new();

            try
            {
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_SEARCH_CASAEI_WebApis", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdWebApi", SqlDbType.Int, 4).Value = IdWebApi;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objWebApis.IdWebApi = Convert.ToInt32(dr["IdWebApi"]);
                        objWebApis.URL = dr["URL"].ToString();
                        objWebApis.Descripcion = dr["Descripcion"].ToString();
                        objWebApis.Usuario = dr["Usuario"].ToString();
                        objWebApis.Password = dr["Password"].ToString();
                        objWebApis.Api = dr["Api"].ToString();
                        objWebApis.Token = dr["Token"].ToString();
                    }
                    else
                    {
                        objWebApis = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objWebApis;
        }

    }
}

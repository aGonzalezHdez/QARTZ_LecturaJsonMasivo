using LibreriaClasesAPIExpertti.Entities.EntitiesControlSalidas;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas
{
    public class CatalogodeErroresDetectadosRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogodeErroresDetectadosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public List<CatalogodeErroresDetectados> Cargar(int idDepartamento, int idOficina, int Operacion, int IdReferencia)
        {
            List<CatalogodeErroresDetectados> lstCatalogodeErroresDetectados = new();

            try
            {
                using (con = new SqlConnection(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_CASAEI_CatalogodeErroresDetectados", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idDepartamento", SqlDbType.Int).Value = idDepartamento;
                    cmd.Parameters.Add("@idOficina", SqlDbType.Int).Value = idOficina;
                    cmd.Parameters.Add("@Operacion", SqlDbType.Int).Value = Operacion;
                    cmd.Parameters.Add("@IdReferencia", SqlDbType.Int).Value = IdReferencia;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            CatalogodeErroresDetectados objCatalogodeErroresDetectados = new()
                            {
                                idError = Convert.ToInt32(dr["idError"]),
                                _Error = dr["_Error"].ToString(),
                                idDepartamento = Convert.ToInt32(dr["idDepartamento"]),
                                idOficina = Convert.ToInt32(dr["idOficina"]),
                                Activo = Convert.ToBoolean(dr["Activo"]),
                                IdDepartamentoError = Convert.ToInt32(dr["IdDepartamentoError"]),
                                Operacion = Convert.ToInt32(dr["Operacion"]),
                                Seleccionado = Convert.ToInt32(dr["Seleccionado"])
                            };

                            lstCatalogodeErroresDetectados.Add(objCatalogodeErroresDetectados);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lstCatalogodeErroresDetectados;
        }

        public List<CatalogoDeErrores> Cargar(int idDepartamento, int idOficina)
        {
            List<CatalogoDeErrores> lstCatalogoDeErrores = new();
            try
            {
                using (con = new SqlConnection(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODEERRORESXID_OFICINA", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idDepartamento", SqlDbType.Int).Value = idDepartamento;
                    cmd.Parameters.Add("@idOficina", SqlDbType.Int).Value = idOficina;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            CatalogoDeErrores objCatalogoDeErrores = new()
                            {
                                IdError = Convert.ToInt32(dr["IdError"]),
                                IdPantalla = Convert.ToInt32(dr["IdPantalla"]),
                                Error = dr["Error"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            };
                            lstCatalogoDeErrores.Add(objCatalogoDeErrores);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return lstCatalogoDeErrores;
        }


    }
}

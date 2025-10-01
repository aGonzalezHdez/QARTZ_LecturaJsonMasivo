using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogodeTramitadoresRepository
    {
        IConfiguration _configuration;
        public string SConexion { get; set; }
        public CatalogodeTramitadoresRepository(IConfiguration configuracion)
        {
            _configuration = configuracion;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public CatalogodeTramitadores Buscar(int IdTramitador)
        {
            CatalogodeTramitadores objCatalogodeTramitadores = new CatalogodeTramitadores();
            using (SqlConnection cn = new SqlConnection(SConexion))
            using (SqlCommand cmd = new SqlCommand("NET_SEARCH_CASAEI_CATALOGODETRAMITADORES", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdTramitador", SqlDbType.Int) { Value = IdTramitador });

                try
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objCatalogodeTramitadores.IdTramitador = Convert.ToInt32(dr["IdTramitador"]);
                            objCatalogodeTramitadores.IdEmpTransportista = Convert.ToInt32(dr["IdEmpTransportista"]);
                            objCatalogodeTramitadores.Nombre = dr["Nombre"].ToString();
                            objCatalogodeTramitadores.GafeteUnico = dr["GafeteUnico"].ToString();
                            objCatalogodeTramitadores.ACTIVO = Convert.ToBoolean(dr["ACTIVO"]);
                        }
                        else
                        {
                            objCatalogodeTramitadores = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return objCatalogodeTramitadores;
        }

    }
}

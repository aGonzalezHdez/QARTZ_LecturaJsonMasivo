using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogoDeDocumentosRepository : ICatalogoDeDocumentosRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public CatalogoDeDocumentosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public bool BuscarPorIDReferencia(int IDReferencia, int IDTipoReferencia)
        {
            bool existeRegistro = false;

            using (SqlConnection conn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_SEARCH_VALIDARDOCUMENTOPORGUIA", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdReferencia", IDReferencia);
                    cmd.Parameters.AddWithValue("@IdTipoDocumento", IDTipoReferencia);

                    SqlParameter outputParam = new SqlParameter("@Existe", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    existeRegistro = outputParam.Value != DBNull.Value && (bool)outputParam.Value;
                }
            }

            return existeRegistro;
        }
    }
}

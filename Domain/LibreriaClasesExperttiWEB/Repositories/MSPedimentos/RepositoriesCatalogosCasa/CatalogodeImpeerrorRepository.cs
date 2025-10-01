using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCatalogosCasa
{
    public class CatalogodeImpeerrorRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogodeImpeerrorRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public CatalogodeImpeerror Buscar(string clave)
        {
            var objCatalogodeImpeerror = new CatalogodeImpeerror();

            using (var cn = new SqlConnection(SConexion))
            using (var cmd = new SqlCommand("NET_SEARCH_CATALOGODEIMPEERROR", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Clave", SqlDbType.VarChar, 8) { Value = clave });

                try
                {
                    cn.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objCatalogodeImpeerror.idImpeError = Convert.ToInt32(dr["idImpeError"]);
                            objCatalogodeImpeerror.Clave = dr["Clave"].ToString();
                            objCatalogodeImpeerror.Descripcion = dr["Descripcion"].ToString();
                            objCatalogodeImpeerror.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                            objCatalogodeImpeerror.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);
                        }
                        else
                        {
                            objCatalogodeImpeerror = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return objCatalogodeImpeerror;
        }

    }
}

using LibreriaClasesAPIExpertti.Entities.EntitiesDespacho;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSDespachos.RepositoriesCortes
{
    public class ConsolCortesRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;


        public ConsolCortesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public ConsolCortes Buscar(int IdCorte)
        {
            ConsolCortes objConsolCortes = new ConsolCortes();
            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_SEARCH_ConsolCortes1", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdCorte", SqlDbType.Int).Value = IdCorte;

                    try
                    {
                        cn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                objConsolCortes.IdCorte = Convert.ToInt32(dr["IdCorte"]);
                                objConsolCortes.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                                objConsolCortes.NoCorte = dr["NoCorte"].ToString();
                                objConsolCortes.CerradoConsol = Convert.ToBoolean(dr["CerradoConsol"]);
                                objConsolCortes.FechaCierreConsol = Convert.ToDateTime(dr["FechaCierreConsol"]);
                                objConsolCortes.CerradoDespachos = Convert.ToBoolean(dr["CerradoDespachos"]);
                                objConsolCortes.FechaCierreDespachos = Convert.ToDateTime(dr["FechaCierreDespachos"]);
                                objConsolCortes.CerradoCierre = Convert.ToBoolean(dr["CerradoCierre"]);
                                objConsolCortes.FechadeCierre = Convert.ToDateTime(dr["FechadeCierre"]);
                                objConsolCortes.CerradoImpresion = Convert.ToBoolean(dr["CerradoImpresion"]);
                                objConsolCortes.FechaImpresion = Convert.ToDateTime(dr["FechaImpresion"]);
                                objConsolCortes.Unitarias = Convert.ToBoolean(dr["Unitarias"]);
                                objConsolCortes.IdEstacion = Convert.ToInt32(dr["IdEstacion"]);
                                objConsolCortes.Operacion = Convert.ToInt32(dr["Operacion"]);
                            }
                            else
                            {
                                objConsolCortes = null; // or return null if preferred
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

            return objConsolCortes;
        }

    }
}

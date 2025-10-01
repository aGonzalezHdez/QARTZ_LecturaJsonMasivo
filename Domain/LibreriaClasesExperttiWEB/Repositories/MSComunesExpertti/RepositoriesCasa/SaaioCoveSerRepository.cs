using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesCasa
{
    public class SaaioCoveSerRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn = null!;

        public SaaioCoveSerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<SaaioCoveSer> CargarSeries(string numerodeReferencia, int consFact, int consPart)
        {
            List<SaaioCoveSer> lstSaaioCoveSer = new List<SaaioCoveSer>();
            using (SqlConnection cn = new SqlConnection(sConexion))
            using (SqlCommand cmd = new SqlCommand("NET_SEARCH_SAAIO_COVESER_GRID", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 25).Value = numerodeReferencia;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = consFact;
                cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4).Value = consPart;

                try
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                SaaioCoveSer objSaaioCoveSer = new SaaioCoveSer
                                {
                                    NUM_REFE = dr["NUM_REFE"].ToString(),
                                    CONS_FACT = Convert.ToInt32(dr["CONS_FACT"]),
                                    CONS_PART = Convert.ToInt32(dr["CONS_PART"]),
                                    CONS_SERI = Convert.ToInt32(dr["CONS_SERI"]),
                                    NUM_PART = dr["NUM_PART"].ToString(),
                                    MAR_MERC = dr["MAR_MERC"].ToString(),
                                    SUB_MODE = dr["SUB_MODE"].ToString(),
                                    NUM_SERI = dr["NUM_SERI"].ToString()
                                };

                                lstSaaioCoveSer.Add(objSaaioCoveSer);
                            }
                        }
                        else
                        {
                            lstSaaioCoveSer = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return lstSaaioCoveSer;
        }

    }
}

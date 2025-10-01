using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesCasa
{
    public class CtarcEntFedRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public CtarcEntFedRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public Ctarc_EntFed Buscar(string MyPais, string MyEntidad)
        {
            Ctarc_EntFed objCTARC_ENTFED = null;

            using (SqlConnection cn = new SqlConnection(sConexion))
            using (SqlCommand cmd = new SqlCommand("NET_SEARCH_CTARC_ENTFED", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CVE_PAIS", SqlDbType.VarChar, 3).Value = MyPais;
                cmd.Parameters.Add("@CVE_EFED", SqlDbType.VarChar, 2).Value = MyEntidad;

                try
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows && dr.Read())
                        {
                            objCTARC_ENTFED = new Ctarc_EntFed
                            {
                                CVE_PAIS = dr["CVE_PAIS"].ToString(),
                                CVE_EFED = dr["CVE_EFED"].ToString(),
                                NOM_EFED = dr["NOM_EFED"].ToString()
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return objCTARC_ENTFED;
        }

        public Ctarc_EntFed Buscar(string MyEntidad)
        {
            Ctarc_EntFed objCTARC_ENTFED = null;

            using (SqlConnection cn = new SqlConnection(sConexion))
            using (SqlCommand cmd = new SqlCommand("NET_SEARCH_CTARC_ENTFED_SINPAIS", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CVE_EFED", SqlDbType.VarChar, 2).Value = MyEntidad;

                try
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows && dr.Read())
                        {
                            objCTARC_ENTFED = new Ctarc_EntFed
                            {
                                CVE_PAIS = dr["CVE_PAIS"].ToString(),
                                CVE_EFED = dr["CVE_EFED"].ToString(),
                                NOM_EFED = dr["NOM_EFED"].ToString()
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return objCTARC_ENTFED;
        }

        public List<Ctarc_EntFed> Cargar(bool esCliente)
        {
            List<Ctarc_EntFed> lstCTARC_ENTFED = new List<Ctarc_EntFed>();

            using (SqlConnection cn = new SqlConnection(sConexion))
            using (SqlCommand cmd = new SqlCommand("NET_LOAD_CTARC_ENTFED", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CLIENTE", SqlDbType.Bit).Value = esCliente;

                try
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var objCTARC_ENTFED = new Ctarc_EntFed
                            {
                                CVE_PAIS = dr["CVE_PAIS"].ToString(),
                                CVE_EFED = dr["CVE_EFED"].ToString(),
                                NOM_EFED = dr["NOM_EFED"].ToString()
                            };

                            lstCTARC_ENTFED.Add(objCTARC_ENTFED);
                        }

                        if (lstCTARC_ENTFED.Count == 0)
                        {
                            lstCTARC_ENTFED = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return lstCTARC_ENTFED;
        }
    }
}

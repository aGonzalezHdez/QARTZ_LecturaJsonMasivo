using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
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
    public class CtracDestinRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn = null!;

        public CtracDestinRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public CtracProved Buscar(string MyCve_Dest)
        {
            CtracProved objCTRAC_DESTIN = new CtracProved();

            using (SqlConnection cn = new SqlConnection(sConexion))
            using (SqlCommand cmd = new SqlCommand("NET_SEARCH_CTRAC_DESTIN", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CVE_PRO", SqlDbType.VarChar, 6).Value = MyCve_Dest;

                try
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objCTRAC_DESTIN.CVE_PRO = dr["CVE_PRO"].ToString();
                            objCTRAC_DESTIN.NOM_PRO = dr["NOM_PRO"].ToString();
                            objCTRAC_DESTIN.DIR_PRO = dr["DIR_PRO"].ToString();
                            objCTRAC_DESTIN.POB_PRO = dr["POB_PRO"].ToString();
                            objCTRAC_DESTIN.ZIP_PRO = dr["ZIP_PRO"].ToString();
                            objCTRAC_DESTIN.TAX_PRO = dr["TAX_PRO"].ToString();
                            objCTRAC_DESTIN.PAI_PRO = dr["PAI_PRO"].ToString();
                            objCTRAC_DESTIN.CTA_PRO = dr["CTA_PRO"].ToString();
                            objCTRAC_DESTIN.EFE_PRO = dr["EFE_PRO"].ToString();
                            objCTRAC_DESTIN.NOI_PRO = dr["NOI_PRO"].ToString();
                            objCTRAC_DESTIN.NOE_PRO = dr["NOE_PRO"].ToString();
                            objCTRAC_DESTIN.VIN_PRO = dr["VIN_PRO"].ToString();
                            objCTRAC_DESTIN.EFE_DESP = dr["EFE_DESP"].ToString();
                            objCTRAC_DESTIN.TEL_PRO = dr["TEL_PRO"].ToString();
                            objCTRAC_DESTIN.CVE_PROC = dr["CVE_PROC"].ToString();
                            objCTRAC_DESTIN.INT_PRO = Convert.ToInt32(dr["INT_PRO"]);
                            objCTRAC_DESTIN.EXP_CONF = dr["EXP_CONF"].ToString();
                            objCTRAC_DESTIN.APE_PATE = dr["APE_PATE"].ToString();
                            objCTRAC_DESTIN.APE_MATE = dr["APE_MATE"].ToString();
                            objCTRAC_DESTIN.COL_PRO = dr["COL_PRO"].ToString();
                            objCTRAC_DESTIN.LOC_PRO = dr["LOC_PRO"].ToString();
                            objCTRAC_DESTIN.REFE_PRO = dr["REFE_PRO"].ToString();
                            objCTRAC_DESTIN.NOM_COVE = dr["NOM_COVE"].ToString();
                            objCTRAC_DESTIN.MUN_COVE = dr["MUN_COVE"].ToString();
                            objCTRAC_DESTIN.MAIL_COVE = dr["MAIL_COVE"].ToString();
                            objCTRAC_DESTIN.NOM_PAIS = dr["NOM_PAIS"].ToString();
                            objCTRAC_DESTIN.Ultimo = dr["Ultimo"].ToString();
                            objCTRAC_DESTIN.Primero = dr["Primero"].ToString();
                            objCTRAC_DESTIN.Siguiente = dr["Siguiente"].ToString();
                            objCTRAC_DESTIN.Anterior = dr["Anterior"].ToString();
                        }
                        else
                        {
                            objCTRAC_DESTIN = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return objCTRAC_DESTIN;
        }

    }
}

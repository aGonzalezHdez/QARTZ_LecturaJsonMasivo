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
    public class CtarcUnidadRepository
    {

        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn = null!;

        public CtarcUnidadRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public CtarcUnidad Buscar(int NumUni)
        {
            CtarcUnidad objCtarcUnidad = new CtarcUnidad();
            using (SqlConnection cn = new SqlConnection(sConexion))
            using (SqlCommand cmd = new SqlCommand("NET_SEARCH_Ctarc_Unidad", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@NumUni", SqlDbType.Int, 4).Value = NumUni;

                try
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objCtarcUnidad.NUM_UNI = Convert.ToInt32(dr["NUM_UNI"]);
                            objCtarcUnidad.DES_UNI = dr["DES_UNI"].ToString();
                            objCtarcUnidad.FRA_UNI = dr["FRA_UNI"].ToString();
                            objCtarcUnidad.ABR_UNI = dr["ABR_UNI"].ToString();
                            objCtarcUnidad.CVE_COVE = dr["CVE_COVE"].ToString();
                            objCtarcUnidad.FAC_EQUI = Convert.ToDouble(dr["FAC_EQUI"]);
                            objCtarcUnidad.UNI_EQUI = dr["UNI_EQUI"].ToString();
                        }
                        else
                        {
                            objCtarcUnidad = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return objCtarcUnidad;
        }

    }
}

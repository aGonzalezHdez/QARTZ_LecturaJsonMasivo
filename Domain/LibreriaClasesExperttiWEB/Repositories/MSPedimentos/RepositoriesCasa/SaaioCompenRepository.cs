using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class SaaioCompenRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn = null!;

        public SaaioCompenRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public List<SaaioCompen> Buscar(string MiReferencia)
        {
            List<SaaioCompen> lstSaaioCompen = new List<SaaioCompen>();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;

            try
            {
                cmd.CommandText = "NET_SEARCH_SAAIO_COMPEN";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                param.Value = MiReferencia;

                cn.ConnectionString = sConexion;
                cn.Open();

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        SaaioCompen objSaaioCompen = new SaaioCompen();

                        objSaaioCompen.NUM_REFE = dr["NUM_REFE"].ToString();
                        objSaaioCompen.NUM_PEDIO = dr["NUM_PEDIO"].ToString(); // Cambiado de NUM_REFE a NUM_PEDIO
                        objSaaioCompen.CVE_IMPU = dr["CVE_IMPU"].ToString();
                        objSaaioCompen.PAT_ORIG = dr["PAT_ORIG"].ToString();
                        objSaaioCompen.FEC_PAGOO = Convert.ToDateTime(dr["FEC_PAGOO"]); // Conversión a DateTime
                        objSaaioCompen.TOT_IMPU = dr["TOT_IMPU"].ToString();

                        lstSaaioCompen.Add(objSaaioCompen);
                    }
                }
                else
                {
                    lstSaaioCompen = null;
                }

                dr.Close();
                cn.Close();
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en Buscar: " + ex.Message);
            }

            return lstSaaioCompen;
        }

    }
}

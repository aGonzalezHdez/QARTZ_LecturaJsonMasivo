using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class CtarcTipCamRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn = null!;

        public CtarcTipCamRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public CtarcTipCam Buscar(DateTime lFecha)
        {
            var objCtarcTipCam = new CtarcTipCam();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            try
            {

                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_SEARCH_CTARC_TIPCAM";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @fecha datetime
                @param = cmd.Parameters.Add("@fecha", SqlDbType.DateTime, 10);
                @param.Value = lFecha;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objCtarcTipCam.FEC_CAM = Convert.ToDateTime(dr["FEC_CAM"]);
                    objCtarcTipCam.TIP_CAM = Convert.ToDouble(dr["TIP_CAM"]);
                }
                else
                {
                    objCtarcTipCam = default;
                }

                dr.Close();
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objCtarcTipCam;
        }

    }
}

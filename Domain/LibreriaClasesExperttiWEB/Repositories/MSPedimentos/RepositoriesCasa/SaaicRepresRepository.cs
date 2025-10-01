using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class SaaicRepresRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public SaaicRepresRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public SaaicRepres Buscar(string cveRep)
        {
            var objSAAIC_REPRES = new SaaicRepres();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_SAAIC_REPRES";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            @param = cmd.Parameters.Add("@CVE_REP", SqlDbType.VarChar, 2);
            @param.Value = cveRep.Trim();
            // Insertar Parametro de busqueda

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objSAAIC_REPRES.CVE_REP = dr["CVE_REP"].ToString();
                    objSAAIC_REPRES.NOM_REP = dr["NOM_REP"].ToString();
                    objSAAIC_REPRES.RFC_REP = dr["RFC_REP"].ToString();
                    objSAAIC_REPRES.CUR_REP = dr["CUR_REP"].ToString();
                    objSAAIC_REPRES.NUM_FIRM = Convert.ToInt32(dr["NUM_FIRM"]);
                    objSAAIC_REPRES.TIP_REPRE = dr["TIP_REPRE"].ToString();
                    objSAAIC_REPRES.RUTA_ARCHVAL = dr["RUTA_ARCHVAL"].ToString();
                    objSAAIC_REPRES.CVE_ARCHVAL = dr["CVE_ARCHVAL"].ToString();
                    objSAAIC_REPRES.CER_FIRM = dr["CER_FIRM"].ToString();
                    objSAAIC_REPRES.RUTA_ARCHCER = dr["RUTA_ARCHCER"].ToString();
                    objSAAIC_REPRES.IMAG_FIR = dr["IMAG_FIR"].ToString();
                    objSAAIC_REPRES.BAJ_REPR = Convert.ToDateTime(dr["BAJ_REPR"]);
                    objSAAIC_REPRES.BAJ_DESC = dr["BAJ_DESC"].ToString();
                }
                else
                {
                    objSAAIC_REPRES = default;
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

            return objSAAIC_REPRES;
        }

    }
}

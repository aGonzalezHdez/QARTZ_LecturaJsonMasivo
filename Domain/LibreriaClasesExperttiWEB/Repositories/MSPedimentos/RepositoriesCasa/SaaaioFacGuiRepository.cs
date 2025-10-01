using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class SaaaioFacGuiRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public SaaaioFacGuiRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public SaaioFacGui VerificarExistaHouseEnRemesa(string Num_Refe, string Num_Rem)
        {
            var objSAAIO_FACGUI = new SaaioFacGui();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            try
            {

                cmd.CommandText = "NET_SEARCH_SAAIO_FACGUI_GUIA_EXISTEHOUSE";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = Num_Refe;

                @param = cmd.Parameters.Add("@NUM_REM", SqlDbType.Int, 4);
                @param.Value = Num_Rem;


                // Insertar Parametro de busqueda

                cn.ConnectionString = SConexion;
                cn.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objSAAIO_FACGUI.NUM_REFE = dr["NUM_REFE"].ToString();
                    objSAAIO_FACGUI.CONS_FACT = Convert.ToInt32(dr["CONS_FACT"]);
                    objSAAIO_FACGUI.GUI_MAST = dr["GUI_MAST"].ToString();
                    objSAAIO_FACGUI.GUI_HOUS = dr["GUI_HOUS"].ToString();
                }
                else
                {
                    objSAAIO_FACGUI = default;
                }
                dr.Close();
                cn.Close();
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objSAAIO_FACGUI;
        }

    }
}

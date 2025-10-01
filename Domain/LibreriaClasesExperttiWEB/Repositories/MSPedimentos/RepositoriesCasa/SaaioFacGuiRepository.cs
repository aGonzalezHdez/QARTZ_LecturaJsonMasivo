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
    public class SaaioFacGuiRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public SaaioFacGuiRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }


        public SaaioFacGui VerificarExistaHouseEnRemesa(string numRefe, int numRem)
        {
            SaaioFacGui objGuia = new();

            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_SAAIO_FACGUI_GUIA_EXISTEHOUSE", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = numRefe;
                    cmd.Parameters.Add("@NUM_REM", SqlDbType.Int, 4).Value = numRem;

                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        dr.Read();
                        objGuia.NUM_REFE = dr["NUM_REFE"].ToString();
                        objGuia.CONS_FACT = Convert.ToInt32(dr["CONS_FACT"]);
                        objGuia.GUI_MAST = dr["GUI_MAST"].ToString();
                        objGuia.GUI_HOUS = dr["GUI_HOUS"].ToString();

                    }
                    else
                        objGuia = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objGuia;
        }
    }
}

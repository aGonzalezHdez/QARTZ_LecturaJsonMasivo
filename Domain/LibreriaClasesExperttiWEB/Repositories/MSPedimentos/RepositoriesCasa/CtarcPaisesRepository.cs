using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class CtarcPaisesRepository : ICtarcPaisesRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CtarcPaisesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public CtarcPaises Buscar(string CVE_PAI)
        {
            CtarcPaises objCtarc_Paises = new();

            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_CTARC_PAISES", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CVE_PAI", SqlDbType.VarChar, 3).Value = CVE_PAI;
                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        dr.Read();
                        objCtarc_Paises.CVE_PAI = dr["CVE_PAI"].ToString();
                        objCtarc_Paises.NOM_PAI = dr["NOM_PAI"].ToString();
                        objCtarc_Paises.MON_PAI = dr["MON_PAI"].ToString();
                        objCtarc_Paises.DES_PAI = dr["DES_PAI"].ToString();
                        objCtarc_Paises.APL_TLCS = dr["APL_TLCS"].ToString();
                        objCtarc_Paises.CVE_PAI2 = dr["CVE_PAI2"].ToString();
                        objCtarc_Paises.NOM_CORT = dr["NOM_CORT"].ToString();
                        objCtarc_Paises.APL_ALADI = dr["APL_ALADI"].ToString();
                    }
                    else
                        objCtarc_Paises = null/* TODO Change to default(_) if this is not a reference type */;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objCtarc_Paises;
        }
    }
}

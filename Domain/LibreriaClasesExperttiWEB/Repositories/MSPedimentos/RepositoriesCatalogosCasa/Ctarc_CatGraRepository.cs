using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCatalogosCasa
{
    public class Ctarc_CatGraRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public Ctarc_CatGraRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public Ctarc_CatGra Buscar(string myCatalogo, string myValorBuscado)
        {
            var objCTARC_CATGRA = new Ctarc_CatGra();

            using (var cn = new SqlConnection(SConexion))
            using (var cmd = new SqlCommand("NET_SEARCH_CTARC_CATGRA", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@CVE_1", SqlDbType.VarChar, 4) { Value = myCatalogo });
                cmd.Parameters.Add(new SqlParameter("@CVE_2", SqlDbType.VarChar, 15) { Value = myValorBuscado });

                try
                {
                    cn.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objCTARC_CATGRA.CVE_1 = dr["CVE_1"].ToString();
                            objCTARC_CATGRA.CVE_2 = dr["CVE_2"].ToString();
                            objCTARC_CATGRA.DESCRIP = dr["DESCRIP"].ToString();
                        }
                        else
                        {
                            objCTARC_CATGRA = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return objCTARC_CATGRA;
        }

        public async Task<List<DropDownListDatos>> Buscar_PECA()
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_CTARC_CATGRA_PECA", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    comboList = SqlDataReaderToDropDownList.DropDownList<DropDownListDatos>(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return comboList.ToList();

        }
    }
}

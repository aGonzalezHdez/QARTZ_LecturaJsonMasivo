using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces;
using LibreriaClasesAPIExpertti.Repositories.MSUsuarios.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class SaaioIncremRepository : ISaaioIncremRepository
    {
        public string SConexion { get; set; }
        string ISaaioIncremRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public SaaioIncremRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public SaaioIncrem BuscarMXN(string myNumRefe, string myIncrem)
        {
            SaaioIncrem objSAAIO_INCREM = null;
            using (SqlConnection cn = new SqlConnection(SConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_SEARCH_SAAIO_INCREM_MN", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmd.Parameters.AddWithValue("@NUM_REFE", myNumRefe);
                    cmd.Parameters.AddWithValue("@CVE_INCR", myIncrem);

                    try
                    {
                        cn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                if (dr.Read())
                                {
                                    objSAAIO_INCREM = new SaaioIncrem
                                    {
                                        NUM_REFE = dr["NUM_REFE"].ToString(),
                                        CVE_INCR = dr["CVE_INCR"].ToString(),
                                        IMP_INCR = Convert.ToDecimal(dr["IMP_INCR_MN"]),
                                        MON_INCR = "MXN"
                                    };
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            return objSAAIO_INCREM;
        }
        //inicio
        //atg STI-0000188 23/09/2025
        public DataTable Cargar(string NUM_REFE)
        {
            DataTable dtb = new();
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_LOAD_SAAIO_INCREM", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;

                using SqlDataAdapter dap = new()
                {
                    SelectCommand = cmd
                };

                con.Open();
                dap.Fill(dtb);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + "NET_LOAD_SAAIO_NOINCR");
            }

            return dtb;
        }
        //Fin
        //atg STI-0000188 23/09/2025
    }
}

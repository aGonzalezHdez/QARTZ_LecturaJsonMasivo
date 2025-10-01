using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class SaaioFacCarRepository : ISaaioFacCarRepository
    {
        public string SConexion { get; set; }

        string ISaaioFacCarRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IConfiguration _configuration;
        public SaaioFacCarRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = configuration.GetConnectionString("dbCASAEI")!;
        }
        public int Insertar(SaaioFacCar lsaaio_faccar)
        {
            int id = 0;
         
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_INSERT_SAAIO_FACCAR", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                
                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = string.IsNullOrEmpty(lsaaio_faccar.NUM_REFE) ? DBNull.Value : lsaaio_faccar.NUM_REFE;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = lsaaio_faccar.CONS_FACT;               
                cmd.Parameters.Add("@CVE_INCR", SqlDbType.VarChar, 4).Value = string.IsNullOrEmpty(lsaaio_faccar.CVE_INCR) ? DBNull.Value : lsaaio_faccar.CVE_INCR;                
                cmd.Parameters.Add("@IMP_INCR", SqlDbType.Float, 4).Value = lsaaio_faccar.IMP_INCR;                
                cmd.Parameters.Add("@MON_INCR", SqlDbType.VarChar, 3).Value = string.IsNullOrEmpty(lsaaio_faccar.MON_INCR) ? DBNull.Value : lsaaio_faccar.MON_INCR;                
                cmd.Parameters.Add("@APL_INCR", SqlDbType.VarChar, 1).Value = string.IsNullOrEmpty(lsaaio_faccar.APL_INCR) ? DBNull.Value : lsaaio_faccar.APL_INCR;

                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
           
                con.Open();
                cmd.ExecuteNonQuery();

                id = System.Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
            }
            catch (Exception ex)
            {               
                throw new Exception(ex.Message.ToString() + "NET_INSERT_SAAIO_FACCAR");
            }          
            return id;
        }

        public DataTable CargarIncrementables(string MiReferencia, int MiCons_fact)
        {
            DataTable dtb = new ();
            
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_SEARCH_SAAIO_FACCAR", con);
                
                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = MiReferencia;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = MiCons_fact;
                cmd.CommandType = CommandType.StoredProcedure;

                using SqlDataAdapter dap = new()
                {
                    SelectCommand = cmd,
                };
                
                con.Open();
                dap.Fill(dtb);               

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + "NET_SEARCH_SAAIO_FACCAR");
            }
            return dtb;
        }

        public bool EliminarIncrementables(string NUM_REFE, int CONS_FACT, int CVE_INCR)
        {
            bool Elimino;           
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_DELETE_SAAIO_FACCAR", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                
                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = CONS_FACT;
                cmd.Parameters.Add("@CVE_INCR", SqlDbType.Int, 4).Value = CVE_INCR;
                
                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();
                Elimino = Convert.ToBoolean(cmd.Parameters["@newid_registro"].Value);
               
            }
            catch (Exception ex)
            {               
                throw new Exception(ex.Message.ToString() + "NET_DELETE_SAAIO_FACCAR");
            }
            return Elimino;
        }
    }
}

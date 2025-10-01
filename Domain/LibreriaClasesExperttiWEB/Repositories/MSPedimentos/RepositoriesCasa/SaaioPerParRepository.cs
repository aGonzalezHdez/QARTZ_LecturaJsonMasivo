using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class SaaioPerParRepository : ISaaioPerParRepository
    {
        public string SConexion { get; set; }
        string ISaaioPerParRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;

        public SaaioPerParRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public SaaioPerPar? Buscar(string NUM_REFE, int CONS_FACT, int CONS_PART, int PER_IDEN)
        {           
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_SEARCH_SAAIO_PERPAR", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                   
                cmd.Parameters.Add("@NUM_REFE ", SqlDbType.VarChar, 15).Value = NUM_REFE;
                cmd.Parameters.Add("@CONS_FACT ", SqlDbType.Int, 4).Value = CONS_FACT;
                cmd.Parameters.Add("@CONS_PART ", SqlDbType.Int, 4).Value = CONS_PART;
                cmd.Parameters.Add("@PER_IDEN ", SqlDbType.Int, 4).Value = PER_IDEN;
                    
                con.Open();
                using SqlDataReader dr = cmd.ExecuteReader();
                
                if (dr.Read())
                {
                    return new SaaioPerPar
                    {
                        NUM_REFE = dr["NUM_REFE"]?.ToString(),
                        CONS_PART = Convert.ToInt32(dr["CONS_PART"]),
                        CONS_FACT = Convert.ToInt32(dr["CONS_FACT"]),
                        CVE_PERM = dr["CVE_PERM"]?.ToString(),
                        NUM_PERM = dr["NUM_PERM"]?.ToString(),
                        PER_IDEN = dr["PER_IDEN"]?.ToString(),
                        FIR_ELEC = dr["FIR_ELEC"]?.ToString(),
                        VAL_CDLL = Convert.ToDouble(dr["VAL_CDLL"]),
                        CAN_TARI = Convert.ToDouble(dr["CAN_TARI"]),
                    };                         
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }            
        }

        public int Insertar(SaaioPerPar lsaaio_perpar)
        {
            int id;           

            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_INSERT_SAAIO_PERPAR", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                    cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = string.IsNullOrEmpty(lsaaio_perpar.NUM_REFE) ? DBNull.Value : lsaaio_perpar.NUM_REFE;
                    cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int).Value = lsaaio_perpar.CONS_FACT;
                    cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4).Value = lsaaio_perpar.CONS_PART;
                    cmd.Parameters.Add("@CVE_PERM", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(lsaaio_perpar.CVE_PERM) ? DBNull.Value : lsaaio_perpar.CVE_PERM;
                    cmd.Parameters.Add("@NUM_PERM", SqlDbType.VarChar, 30).Value = string.IsNullOrEmpty(lsaaio_perpar.NUM_PERM) ? DBNull.Value : lsaaio_perpar.NUM_PERM;
                    cmd.Parameters.Add("@PER_IDEN", SqlDbType.Char, 3).Value = string.IsNullOrEmpty(lsaaio_perpar.PER_IDEN) ? DBNull.Value : lsaaio_perpar.PER_IDEN;
                    cmd.Parameters.Add("@FIR_ELEC", SqlDbType.VarChar, 40).Value = string.IsNullOrEmpty(lsaaio_perpar.FIR_ELEC) ? DBNull.Value : lsaaio_perpar.FIR_ELEC;
                    cmd.Parameters.Add("@VAL_CDLL", SqlDbType.Float, 4).Value = lsaaio_perpar.VAL_CDLL;
                    cmd.Parameters.Add("@CAN_TARI", SqlDbType.Float, 4).Value = lsaaio_perpar.CAN_TARI;

                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    con.Open();

                cmd.ExecuteNonQuery();
                if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                    id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                else
                    id = 0;
            }
            catch (Exception ex)
            {             
                throw new Exception(ex.Message.ToString() + "NET_INSERT_SAAIO_PERPAR");
            }          
            return id;
        }

        public int Modificar(SaaioPerPar lsaaio_perpar)
        {
            int id; 
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_UPDATE_SAAIO_PERPAR", con)
                {
                    CommandType = CommandType.StoredProcedure
                };  

                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = lsaaio_perpar.NUM_REFE;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = lsaaio_perpar.CONS_FACT;
                cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4).Value = lsaaio_perpar.CONS_PART;
                cmd.Parameters.Add("@CVE_PERM", SqlDbType.VarChar, 2).Value = lsaaio_perpar.CVE_PERM;
                cmd.Parameters.Add("@NUM_PERM", SqlDbType.VarChar, 30).Value = lsaaio_perpar.NUM_PERM;
                cmd.Parameters.Add("@PER_IDEN", SqlDbType.Char, 3).Value = lsaaio_perpar.PER_IDEN;
                cmd.Parameters.Add("@FIR_ELEC", SqlDbType.VarChar, 40).Value = lsaaio_perpar.FIR_ELEC;
                cmd.Parameters.Add("@VAL_CDLL", SqlDbType.Float, 4).Value = lsaaio_perpar.VAL_CDLL;
                cmd.Parameters.Add("@CAN_TARI", SqlDbType.Float, 4).Value = lsaaio_perpar.CAN_TARI;
                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                con.Open();

                cmd.ExecuteNonQuery();
                if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                    id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                else
                    id = 0;
            }
            catch (Exception ex)
            {             
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_SAAIO_PERPAR");
            }           
            return id;
        }

        public bool EliminarPorID(string NUM_REFE, int CONS_FACT, int CONS_PART, int PER_IDEN)
        {
            bool Elimino = false;         

            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_DELETE_SAAIO_PERPAR", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = CONS_FACT;
                cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4).Value = CONS_PART;
                cmd.Parameters.Add("@PER_IDEN", SqlDbType.Int, 4).Value = PER_IDEN;

                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                              
                con.Open();

                cmd.ExecuteNonQuery();

                if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                    Elimino = Convert.ToBoolean(cmd.Parameters["@newid_registro"].Value);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString() + "NET_DELETE_SAAIO_PERPAR");
            }
            return Elimino;
        }

        public DataTable CargarPermisosDePartida(string NUM_REFE, int CONS_FACT, int Mynum_Part)
        {
            DataTable dtb = new ();
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_LOAD_SAAIO_PERPAR_POR_PARTIDA", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = CONS_FACT;
                cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4).Value = Mynum_Part;

                using SqlDataAdapter dap = new()
                {
                    SelectCommand = cmd,
                };
                con.Open();
                dap.Fill(dtb);                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dtb;
        }
    }
}

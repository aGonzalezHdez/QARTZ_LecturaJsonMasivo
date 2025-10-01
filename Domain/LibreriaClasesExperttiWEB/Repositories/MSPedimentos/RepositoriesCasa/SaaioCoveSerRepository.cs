using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;


namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class SaaioCoveSerRepository : ISaaioCoveSerRepository
    {
        public string SConexion { get; set; }

        string ISaaioCoveSerRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;

        public SaaioCoveSerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI");
        }

        public List<SaaioCoveSer>? CargarSeries(string NUM_REFE, int CONS_FACT, int CONS_PART)
        {
            List<SaaioCoveSer>? lstSaaioCoveSer = new ();
     
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_SEARCH_SAAIO_COVESER_GRID", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                
                cmd.Parameters.Add("@NUM_REFE ", SqlDbType.VarChar, 25).Value = NUM_REFE;
                cmd.Parameters.Add("@CONS_FACT ", SqlDbType.Int, 4).Value = CONS_FACT;
                cmd.Parameters.Add("@CONS_PART ", SqlDbType.Int, 4).Value = CONS_PART;

                con.Open();

                using SqlDataReader dr = cmd.ExecuteReader();
               
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        SaaioCoveSer objSaaioCoveSer = new()
                        {
                            NUM_REFE = dr["NUM_REFE"].ToString(),
                            CONS_FACT = Convert.ToInt32(dr["CONS_FACT"]),
                            CONS_PART = Convert.ToInt32(dr["CONS_PART"]),
                            CONS_SERI = Convert.ToInt32(dr["CONS_SERI"]),
                            NUM_PART = dr["NUM_PART"].ToString(),
                            MAR_MERC = dr["MAR_MERC"].ToString(),
                            SUB_MODE = dr["SUB_MODE"].ToString(),
                            NUM_SERI = dr["NUM_SERI"].ToString()
                        };

                        lstSaaioCoveSer.Add(objSaaioCoveSer);
                    }
                }
                else
                    lstSaaioCoveSer = null;
             
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lstSaaioCoveSer;
        }

        public DataTable Cargar(string NUM_REFE)
        {
            DataTable dtb = new();
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_SEARCH_SAAIO_COVESER_NUM_REFE", con)
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
                throw new Exception(ex.Message.ToString() + "NET_SEARCH_SAAIO_COVESER_NUM_REFE");
            }

            return dtb;
        }      

        public int Insertar(SaaioCoveSer lsaaio_coveser)
        {
            int id;           

            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_INSERT_SAAIO_COVESER", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = lsaaio_coveser.NUM_REFE;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = lsaaio_coveser.CONS_FACT;
                cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4).Value = lsaaio_coveser.CONS_PART;
                cmd.Parameters.Add("@CONS_SERI", SqlDbType.Int, 4).Value = lsaaio_coveser.CONS_SERI;
                cmd.Parameters.Add("@NUM_PART", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(lsaaio_coveser.NUM_PART) ? DBNull.Value : lsaaio_coveser.NUM_PART;
                cmd.Parameters.Add("@MAR_MERC", SqlDbType.VarChar, 100).Value = string.IsNullOrEmpty(lsaaio_coveser.MAR_MERC) ? DBNull.Value : lsaaio_coveser.MAR_MERC;
                cmd.Parameters.Add("@SUB_MODE", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(lsaaio_coveser.SUB_MODE) ? DBNull.Value : lsaaio_coveser.SUB_MODE;
                cmd.Parameters.Add("@NUM_SERI", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(lsaaio_coveser.NUM_SERI) ? DBNull.Value : lsaaio_coveser.NUM_SERI;

                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                con.Open();

                cmd.ExecuteNonQuery();
                if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) > 0)
                    id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                else
                    id = 0;
                
            }
            catch (Exception ex)
            {              
                throw new Exception(ex.Message.ToString() + "NET_INSERT_SAAIO_COVESER");
            }            
            return id;
        }

        public int Modificar(SaaioCoveSer lsaaio_coveser)
        {
            int id;        

            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_UPDATE_SAAIO_COVESER", con)
                { 
                    CommandType = CommandType.StoredProcedure 
                }; 
                
                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = lsaaio_coveser.NUM_REFE;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = lsaaio_coveser.CONS_FACT;
                cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4).Value =  lsaaio_coveser.CONS_PART;
                cmd.Parameters.Add("@CONS_SERI", SqlDbType.Int, 4).Value = lsaaio_coveser.CONS_SERI;
                cmd.Parameters.Add("@NUM_PART", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(lsaaio_coveser.NUM_PART) ? DBNull.Value : lsaaio_coveser.NUM_PART;
                cmd.Parameters.Add("@MAR_MERC", SqlDbType.VarChar, 100).Value = string.IsNullOrEmpty(lsaaio_coveser.MAR_MERC) ? DBNull.Value : lsaaio_coveser.MAR_MERC;
                cmd.Parameters.Add("@SUB_MODE", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(lsaaio_coveser.SUB_MODE) ? DBNull.Value : lsaaio_coveser.SUB_MODE;
                cmd.Parameters.Add("@NUM_SERI", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(lsaaio_coveser.NUM_SERI) ? DBNull.Value : lsaaio_coveser.NUM_SERI;

                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                con.Open();

                cmd.ExecuteNonQuery();
                id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                               
            }
            catch (Exception ex)
            {              
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_SAAIO_COVESER");
            }         
            return id;
        }

        public SaaioCoveSer? Buscar(string NUM_REFE, int CONS_FACT, int CONS_PART, int CONS_SERI)
        {
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_SEARCH_SAAIO_COVESER", con)
                {
                    CommandType = CommandType.StoredProcedure
                };  
                
                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int).Value = CONS_FACT;
                cmd.Parameters.Add("@CONS_PART", SqlDbType.Int).Value = CONS_PART;
                cmd.Parameters.Add("@CONS_SERI", SqlDbType.Int).Value = CONS_SERI;

                con.Open();

                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    return new SaaioCoveSer
                    {
                        NUM_REFE = dr["NUM_REFE"]?.ToString(),
                        CONS_FACT = Convert.ToInt32(dr["CONS_FACT"]),
                        CONS_PART = Convert.ToInt32(dr["CONS_PART"]),
                        CONS_SERI = Convert.ToInt32(dr["CONS_SERI"]),
                        NUM_PART = dr["NUM_PART"]?.ToString(),
                        MAR_MERC = dr["MAR_MERC"]?.ToString(),
                        SUB_MODE = dr["SUB_MODE"]?.ToString(),
                        NUM_SERI = dr["NUM_SERI"]?.ToString()
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }           
        }

        public bool Borrar(string NUM_REFE, int CONS_FACT, int CONS_PART, int CONS_SERI)
        {
            bool id;    

            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_DELETE_SAAIO_COVESER", con)
                {
                    CommandType = CommandType.StoredProcedure
                };               

                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = CONS_FACT;
                cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4).Value = CONS_PART;
                cmd.Parameters.Add("@CONS_SERI", SqlDbType.Int, 4).Value = CONS_SERI;

                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                
                con.Open();

                cmd.ExecuteNonQuery();
                id = Convert.ToBoolean(cmd.Parameters["@newid_registro"].Value);               
            }
            catch (Exception ex)
            {              
                throw new Exception(ex.Message.ToString() + "NET_INSERT_SAAIO_COVESER");
            }       
            return id;
        }

    }
}

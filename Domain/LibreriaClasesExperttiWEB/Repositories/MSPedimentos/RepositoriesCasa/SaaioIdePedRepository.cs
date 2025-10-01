using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces;

using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class SaaioIdePedRepository : ISaaioIdePedRepository
    {
        public string SConexion { get; set; }
        string ISaaioIdePedRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;

        public SaaioIdePedRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<SaaioIdePed> Cargar(string MyNum_Refe)
        {
            List<SaaioIdePed> objLstIdePed = new List<SaaioIdePed>();

            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_SELECT_SAAIO_IDEPED", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 16).Value = MyNum_Refe;

                con.Open();
                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        SaaioIdePed objIdePed = new();

                        objIdePed.NUM_REFE = dr["NUM_REFE"].ToString();
                        objIdePed.NUM_IDE = Convert.ToInt32(dr["NUM_IDE"]);
                        objIdePed.CVE_IDEN = dr["CVE_IDEN"].ToString();
                        objIdePed.COM_IDEN = dr["COM_IDEN"].ToString();
                        objIdePed.COM_IDEN2 = dr["COM_IDEN2"].ToString();
                        objIdePed.COM_IDEN3 = dr["COM_IDEN3"].ToString();
                        objIdePed.NOM_ARCH = dr["NOM_ARCH"].ToString();
                        objIdePed.TIP_ARCH = dr["TIP_ARCH"].ToString();
                        objIdePed.NUM_OPER = dr["NUM_OPER"].ToString();
                        objIdePed.FEC_ENV = (DateTime)dr["FEC_ENV"];
                        objIdePed.FIG_FIRM = dr["FIG_FIRM"].ToString();
                        objIdePed.FOL_TRAMI = dr["FOL_TRAMI"].ToString();

                        objLstIdePed.Add(objIdePed);
                    }
                }
                else
                    objLstIdePed = null;              
            }
            catch (Exception ex)
            {  
                throw new Exception(ex.Message.ToString());
            }
            return objLstIdePed;
        }


        public SaaioIdePed Buscar(string NUM_REFE, string CVE_IDEN)
        {
            SaaioIdePed objSAAIO_IDEPED = new();
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_SEARCH_SAAIO_IDEPED_CVE_IDEN", con)
                {
                    CommandType = CommandType.StoredProcedure
                }; 
                
                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;
                cmd.Parameters.Add("@CVE_IDEN", SqlDbType.VarChar, 2).Value = CVE_IDEN;

                con.Open();
                using SqlDataReader dr = cmd.ExecuteReader();
                
                if (dr.HasRows)
                {
                    dr.Read();
                    objSAAIO_IDEPED.NUM_REFE = dr["NUM_REFE"].ToString();
                    objSAAIO_IDEPED.NUM_IDE = Convert.ToInt32(dr["NUM_IDE"]);
                    objSAAIO_IDEPED.CVE_IDEN = dr["CVE_IDEN"].ToString();
                    objSAAIO_IDEPED.COM_IDEN = dr["COM_IDEN"].ToString();
                    objSAAIO_IDEPED.COM_IDEN2 = dr["COM_IDEN2"].ToString();
                    objSAAIO_IDEPED.COM_IDEN3 = dr["COM_IDEN3"].ToString();
                    objSAAIO_IDEPED.NOM_ARCH = dr["NOM_ARCH"].ToString();
                    objSAAIO_IDEPED.TIP_ARCH = dr["TIP_ARCH"].ToString();
                    objSAAIO_IDEPED.NUM_OPER = dr["NUM_OPER"].ToString();
                    objSAAIO_IDEPED.FEC_ENV = Convert.ToDateTime(dr["FEC_ENV"]);
                    objSAAIO_IDEPED.FIG_FIRM = dr["FIG_FIRM"].ToString();
                    objSAAIO_IDEPED.FOL_TRAMI = dr["FOL_TRAMI"].ToString();
                }
                else
                {
                    objSAAIO_IDEPED = null;
                }                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objSAAIO_IDEPED;
        }
        public int Insertar(SaaioIdePed lsaaio_ideped)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_INSERT_SAAIO_IDEPED";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // ,@NUM_REFE  varchar
            @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
            @param.Value = lsaaio_ideped.NUM_REFE == null ? DBNull.Value : lsaaio_ideped.NUM_REFE;

            // ,@NUM_IDE  int
            @param = cmd.Parameters.Add("@NUM_IDE", SqlDbType.Int, 4);
            @param.Value = lsaaio_ideped.NUM_IDE == null ? DBNull.Value : lsaaio_ideped.NUM_IDE;

            // ,@CVE_IDEN  varchar
            @param = cmd.Parameters.Add("@CVE_IDEN", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_ideped.CVE_IDEN == null ? DBNull.Value : lsaaio_ideped.CVE_IDEN;

            // ,@COM_IDEN  varchar
            @param = cmd.Parameters.Add("@COM_IDEN", SqlDbType.VarChar, 30);
            @param.Value = lsaaio_ideped.COM_IDEN == null ? DBNull.Value : lsaaio_ideped.COM_IDEN;

            // ,@COM_IDEN2  varchar
            @param = cmd.Parameters.Add("@COM_IDEN2", SqlDbType.VarChar, 30);
            @param.Value = lsaaio_ideped.COM_IDEN2 == null ? DBNull.Value : lsaaio_ideped.COM_IDEN2;

            // ,@COM_IDEN3  varchar
            @param = cmd.Parameters.Add("@COM_IDEN3", SqlDbType.VarChar, 30);
            @param.Value = lsaaio_ideped.COM_IDEN3 == null ? DBNull.Value : lsaaio_ideped.COM_IDEN3;

            // ,@NOM_ARCH  varchar
            @param = cmd.Parameters.Add("@NOM_ARCH", SqlDbType.VarChar, 180);
            @param.Value = lsaaio_ideped.NOM_ARCH == null ? DBNull.Value : lsaaio_ideped.NOM_ARCH;

            // ,@TIP_ARCH  varchar
            @param = cmd.Parameters.Add("@TIP_ARCH", SqlDbType.VarChar, 3);
            @param.Value = lsaaio_ideped.TIP_ARCH == null ? DBNull.Value : lsaaio_ideped.TIP_ARCH;

            // ,@NUM_OPER  varchar
            @param = cmd.Parameters.Add("@NUM_OPER", SqlDbType.VarChar, 30);
            @param.Value = lsaaio_ideped.NUM_OPER == null ? DBNull.Value : lsaaio_ideped.NUM_OPER;

            // ,@FEC_ENV  datetime
            @param = cmd.Parameters.Add("@FEC_ENV", SqlDbType.DateTime, 4);
            @param.Value = DBNull.Value;

            // ,@FIG_FIRM  char
            @param = cmd.Parameters.Add("@FIG_FIRM", SqlDbType.Char, 1);
            @param.Value = lsaaio_ideped.FIG_FIRM == null ? DBNull.Value : lsaaio_ideped.FIG_FIRM;

            // ,@FOL_TRAMI  varchar
            @param = cmd.Parameters.Add("@FOL_TRAMI", SqlDbType.VarChar, 30);
            @param.Value = lsaaio_ideped.FOL_TRAMI == null ? DBNull.Value : lsaaio_ideped.FOL_TRAMI;


            @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            @param.Direction = ParameterDirection.Output;


            try
            {
                cmd.ExecuteNonQuery();
                if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                {
                    id = (int)cmd.Parameters["@newid_registro"].Value;
                }
                else
                {
                    id = 0;
                }

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
              
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_SAAIO_IDEPED");
            }
            cn.Close();        
            cn.Dispose();
            return id;
        }
        public int Modificar(string MiReferencia, string MiIdentificador, string MiComplemento)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_SAAIO_IDEPED_CR";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // @NUM_REFE  varchar(15)
            @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
            @param.Value = MiReferencia;

            // ,@CVE_IDEN varchar(2)
            @param = cmd.Parameters.Add("@CVE_IDEN", SqlDbType.VarChar, 2);
            @param.Value = MiIdentificador;

            // ,@COM_IDEN VARCHAR(30)
            @param = cmd.Parameters.Add("@COM_IDEN", SqlDbType.VarChar, 30);
            @param.Value = MiComplemento;


            @param = cmd.Parameters.Add("@newid_registro", SqlDbType.VarChar, 15);
            @param.Direction = ParameterDirection.Output;


            try
            {
                cmd.ExecuteNonQuery();
                id = 1;
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();         
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_SAAIO_IDEPED_CR");
            }
            cn.Close();      
            cn.Dispose();
            return id;
        }

        public List<SaaioIdePed> LlenaDataGridViewSaaioIDePedPorReferencia(string numRefe)
        {
            var lista = new List<SaaioIdePed>();

            using (var cn = new SqlConnection(SConexion))
            {
                cn.Open();

                using (var cmd = new SqlCommand("NET_SEARCH_SAAIO_IDEPED", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = numRefe;

                    try
                    {
                        using (var dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var item = new SaaioIdePed
                                    {
                                        NUM_REFE = dr["NUM_REFE"].ToString(),
                                        NUM_IDE = Convert.ToInt32(dr["NUM_IDE"]),
                                        CVE_IDEN = dr["CVE_IDEN"].ToString(),
                                        COM_IDEN = dr["COM_IDEN"].ToString(),
                                        COM_IDEN2 = dr["COM_IDEN2"].ToString(),
                                        COM_IDEN3 = dr["COM_IDEN3"].ToString()
                                    };

                                    lista.Add(item);
                                }
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error en LlenaDataGridViewSaaioIDePedPorReferencia: {ex.Message}");
                    }
                }
            }

            return lista;
        }

        public bool Eliminar(string NUM_REFE, int NUM_IDE)
        {
            bool SiNo;
            try
            {                
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_DELETE_SAAIO_IDEPED_NEW", con);
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                
                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;
                cmd.Parameters.Add("@NUM_IDE", SqlDbType.Int, 4).Value = NUM_IDE;
                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();
                if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                    SiNo = Convert.ToBoolean(cmd.Parameters["@newid_registro"].Value);
                else
                    SiNo = false;

           
            }
            catch (Exception ex)
            {    
                throw new Exception(ex.Message.ToString() + "NET_DELETE_SAAIO_IDEPED_NEW");
            }            
            return SiNo;
        }
    }
}

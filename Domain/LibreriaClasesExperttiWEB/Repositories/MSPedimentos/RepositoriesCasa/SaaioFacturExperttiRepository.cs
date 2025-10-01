using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class SaaioFacturExperttiRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public SaaioFacturExperttiRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<SaaioFactur> Cargar(string NumerodeReferencia)
        {
            var lstSAAIO_FACTUR = new List<SaaioFactur>();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_LOAD_SAAIO_FACTUR_EXPERTTI";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 25);
                @param.Value = NumerodeReferencia;
                // Insertar Parametro de busqueda

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        var objSAAIO_FACTUR = new SaaioFactur();

                        objSAAIO_FACTUR.NUM_REFE = dr["NUM_REFE"].ToString();
                        objSAAIO_FACTUR.CONS_FACT = Convert.ToInt32(dr["CONS_FACT"]);
                        objSAAIO_FACTUR.NUM_FACT = dr["NUM_FACT"].ToString();

                        objSAAIO_FACTUR.FEC_FACT = Convert.ToDateTime(dr["FEC_FACT"]);
                        objSAAIO_FACTUR.ICO_FACT = dr["ICO_FACT"].ToString();
                        objSAAIO_FACTUR.VAL_DLLS = Convert.ToDouble(dr["VAL_DLLS"]);
                        objSAAIO_FACTUR.VAL_EXTR = Convert.ToDouble(dr["VAL_EXTR"]);
                        objSAAIO_FACTUR.TIP_MOFA = dr["TIP_MOFA"].ToString();
                        objSAAIO_FACTUR.CVE_PROV = dr["CVE_PROV"].ToString();
                        objSAAIO_FACTUR.EQU_DLLS = Convert.ToDouble(dr["EQU_DLLS"]);
                        objSAAIO_FACTUR.DAT_VEHI = dr["DAT_VEHI"].ToString();
                        objSAAIO_FACTUR.CAN_FISC = dr["CAN_FISC"].ToString();
                        objSAAIO_FACTUR.NUM_REM = Convert.ToInt32(dr["NUM_REM"]);
                        objSAAIO_FACTUR.MON_FACT = dr["MON_FACT"].ToString();
                        objSAAIO_FACTUR.NUM_PEDI = dr["NUM_PEDI"].ToString();
                        objSAAIO_FACTUR.PES_BRUT = Convert.ToDouble(dr["PES_BRUT"]);
                        objSAAIO_FACTUR.CAN_BULT = Convert.ToDouble(dr["CAN_BULT"]);
                        objSAAIO_FACTUR.CVE_REFIS = dr["CVE_REFIS"].ToString();
                        objSAAIO_FACTUR.NUM_CONT = dr["NUM_CONT"].ToString();
                        objSAAIO_FACTUR.PESO_GRANEL = Convert.ToDouble(dr["PESO_GRANEL"]);
                        objSAAIO_FACTUR.CANT_NIU = Convert.ToInt32(dr["CANT_NIU"]);
                        objSAAIO_FACTUR.NUM_IDU = dr["NUM_IDU"].ToString();
                        objSAAIO_FACTUR.REL_FACT = dr["REL_FACT"].ToString();
                        objSAAIO_FACTUR.SUB_FACT = dr["SUB_FACT"].ToString();
                        objSAAIO_FACTUR.CER_ORIG = dr["CER_ORIG"].ToString();
                        objSAAIO_FACTUR.EXP_CONF = dr["EXP_CONF"].ToString();
                        objSAAIO_FACTUR.OBS_COVE = dr["OBS_COVE"].ToString();
                        objSAAIO_FACTUR.FOL_COVE = Convert.ToInt32(dr["FOL_COVE"]);
                        objSAAIO_FACTUR.EXIS_VINC = dr["EXIS_VINC"].ToString();
                        objSAAIO_FACTUR.FEC_SALI = Convert.ToDateTime(dr["FEC_SALI"]);
                        objSAAIO_FACTUR.MAR_NUME = dr["MAR_NUME"].ToString();
                        objSAAIO_FACTUR.OBS_COVE2 = dr["OBS_COVE2"].ToString();
                        objSAAIO_FACTUR.NUM_FACT2 = dr["NUM_FACT2"].ToString();
                        objSAAIO_FACTUR.CVE_PROV2 = dr["CVE_PROV2"].ToString();

                        lstSAAIO_FACTUR.Add(objSAAIO_FACTUR);
                    }
                }

                else
                {
                    lstSAAIO_FACTUR = null;
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

            return lstSAAIO_FACTUR;
        }
        public SaaioFactur Buscar(string MyNum_refe, int MyCons_Fact)
        {
            var objSAAIO_FACTUR = new SaaioFactur();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            try
            {
                cmd.CommandText = "NET_SEARCH_SAAIO_FACTUR_EXPERTTI";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @NUM_REFE VARCHAR(15) ,
                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = MyNum_refe;

                // @CONS_FACT INT
                @param = cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4);
                @param.Value = MyCons_Fact;
                // Insertar Parametro de busqueda

                cn.ConnectionString = SConexion;
                cn.Open();

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objSAAIO_FACTUR.NUM_REFE = dr["NUM_REFE"].ToString();
                    objSAAIO_FACTUR.CONS_FACT = Convert.ToInt32(dr["CONS_FACT"]);
                    objSAAIO_FACTUR.NUM_FACT = dr["NUM_FACT"].ToString();
                    objSAAIO_FACTUR.FEC_FACT = Convert.ToDateTime(dr["FEC_FACT"]);
                    objSAAIO_FACTUR.ICO_FACT = dr["ICO_FACT"].ToString();
                    objSAAIO_FACTUR.VAL_DLLS = Convert.ToDouble(dr["VAL_DLLS"]);
                    objSAAIO_FACTUR.VAL_EXTR = Convert.ToDouble(dr["VAL_EXTR"]);
                    objSAAIO_FACTUR.TIP_MOFA = dr["TIP_MOFA"].ToString();
                    objSAAIO_FACTUR.CVE_PROV = dr["CVE_PROV"].ToString();
                    objSAAIO_FACTUR.EQU_DLLS = Convert.ToDouble(dr["EQU_DLLS"]);
                    objSAAIO_FACTUR.DAT_VEHI = dr["DAT_VEHI"].ToString();
                    objSAAIO_FACTUR.CAN_FISC = dr["CAN_FISC"].ToString();
                    objSAAIO_FACTUR.NUM_REM = Convert.ToInt32(dr["NUM_REM"]);
                    objSAAIO_FACTUR.MON_FACT = dr["MON_FACT"].ToString();
                    objSAAIO_FACTUR.NUM_PEDI = dr["NUM_PEDI"].ToString();
                    objSAAIO_FACTUR.PES_BRUT = Convert.ToDouble(dr["PES_BRUT"]);
                    objSAAIO_FACTUR.CAN_BULT = Convert.ToDouble(dr["CAN_BULT"]);
                    objSAAIO_FACTUR.CVE_REFIS = dr["CVE_REFIS"].ToString();
                    objSAAIO_FACTUR.NUM_CONT = dr["NUM_CONT"].ToString();
                    objSAAIO_FACTUR.PESO_GRANEL = Convert.ToDouble(dr["PESO_GRANEL"]);
                    objSAAIO_FACTUR.CANT_NIU = Convert.ToInt32(dr["CANT_NIU"]);
                    objSAAIO_FACTUR.NUM_IDU = dr["NUM_IDU"].ToString();
                    objSAAIO_FACTUR.REL_FACT = dr["REL_FACT"].ToString();
                    objSAAIO_FACTUR.SUB_FACT = dr["SUB_FACT"].ToString();
                    objSAAIO_FACTUR.CER_ORIG = dr["CER_ORIG"].ToString();
                    objSAAIO_FACTUR.EXP_CONF = dr["EXP_CONF"].ToString();
                    objSAAIO_FACTUR.OBS_COVE = dr["OBS_COVE"].ToString();
                    objSAAIO_FACTUR.FOL_COVE = Convert.ToInt32(dr["FOL_COVE"]);
                    objSAAIO_FACTUR.EXIS_VINC = dr["EXIS_VINC"].ToString();
                    objSAAIO_FACTUR.FEC_SALI = Convert.ToDateTime(dr["FEC_SALI"]);
                    objSAAIO_FACTUR.MAR_NUME = dr["MAR_NUME"].ToString();
                    objSAAIO_FACTUR.OBS_COVE2 = dr["OBS_COVE2"].ToString();
                    objSAAIO_FACTUR.NUM_FACT2 = dr["NUM_FACT2"].ToString();
                    objSAAIO_FACTUR.CVE_PROV2 = dr["CVE_PROV2"].ToString();
                    objSAAIO_FACTUR.Ultimo = dr["Ultimo"].ToString();
                    objSAAIO_FACTUR.Primero = dr["Primero"].ToString();
                    objSAAIO_FACTUR.Siguiente = dr["Siguiente"].ToString();
                    objSAAIO_FACTUR.Anterior = dr["Anterior"].ToString();
                    objSAAIO_FACTUR.CONSFACTCASA = Convert.ToInt32(dr["CONSFACTCASA"]);
                }
                else
                {
                    objSAAIO_FACTUR = default;
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

            return objSAAIO_FACTUR;
        }

        public SaaioFactur Buscar(string NUM_REFE, string NUM_FACT)
        {
            var objSAAIO_FACTUR = new SaaioFactur();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            try
            {


                cmd.CommandText = "NET_SEARCH_SAAIO_FACTUR_NUM_FACT_EXPERTTI";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @NUM_FACT VARCHAR(15) ,
                @param = cmd.Parameters.Add("@NUM_FACT", SqlDbType.VarChar, 36);
                @param.Value = NUM_FACT;

                // @NUM_REFE VARCHAR(15) ,
                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = NUM_REFE;



                cn.ConnectionString = SConexion;
                cn.Open();

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objSAAIO_FACTUR.NUM_REFE = dr["NUM_REFE"].ToString();
                    objSAAIO_FACTUR.CONS_FACT = Convert.ToInt32(dr["CONS_FACT"]);
                    objSAAIO_FACTUR.NUM_FACT = dr["NUM_FACT"].ToString();
                    objSAAIO_FACTUR.FEC_FACT = Convert.ToDateTime(dr["FEC_FACT"]);
                    objSAAIO_FACTUR.ICO_FACT = dr["ICO_FACT"].ToString();
                    objSAAIO_FACTUR.VAL_DLLS = Convert.ToDouble(dr["VAL_DLLS"]);
                    objSAAIO_FACTUR.VAL_EXTR = Convert.ToDouble(dr["VAL_EXTR"]);
                    objSAAIO_FACTUR.TIP_MOFA = dr["TIP_MOFA"].ToString();
                    objSAAIO_FACTUR.CVE_PROV = dr["CVE_PROV"].ToString();
                    objSAAIO_FACTUR.EQU_DLLS = Convert.ToDouble(dr["EQU_DLLS"]);
                    objSAAIO_FACTUR.DAT_VEHI = dr["DAT_VEHI"].ToString();
                    objSAAIO_FACTUR.CAN_FISC = dr["CAN_FISC"].ToString();
                    objSAAIO_FACTUR.NUM_REM = Convert.ToInt32(dr["NUM_REM"]);
                    objSAAIO_FACTUR.MON_FACT = dr["MON_FACT"].ToString();
                    objSAAIO_FACTUR.NUM_PEDI = dr["NUM_PEDI"].ToString();
                    objSAAIO_FACTUR.PES_BRUT = Convert.ToDouble(dr["PES_BRUT"]);
                    objSAAIO_FACTUR.CAN_BULT = Convert.ToDouble(dr["CAN_BULT"]);
                    objSAAIO_FACTUR.CVE_REFIS = dr["CVE_REFIS"].ToString();
                    objSAAIO_FACTUR.NUM_CONT = dr["NUM_CONT"].ToString();
                    objSAAIO_FACTUR.PESO_GRANEL = Convert.ToDouble(dr["PESO_GRANEL"]);
                    objSAAIO_FACTUR.CANT_NIU = Convert.ToInt32(dr["CANT_NIU"]);
                    objSAAIO_FACTUR.NUM_IDU = dr["NUM_IDU"].ToString();
                    objSAAIO_FACTUR.REL_FACT = dr["REL_FACT"].ToString();
                    objSAAIO_FACTUR.SUB_FACT = dr["SUB_FACT"].ToString();
                    objSAAIO_FACTUR.CER_ORIG = dr["CER_ORIG"].ToString();
                    objSAAIO_FACTUR.EXP_CONF = dr["EXP_CONF"].ToString();
                    objSAAIO_FACTUR.OBS_COVE = dr["OBS_COVE"].ToString();
                    objSAAIO_FACTUR.FOL_COVE = Convert.ToInt32(dr["FOL_COVE"]);
                    objSAAIO_FACTUR.EXIS_VINC = dr["EXIS_VINC"].ToString();
                    objSAAIO_FACTUR.FEC_SALI = Convert.ToDateTime(dr["FEC_SALI"]);
                    objSAAIO_FACTUR.MAR_NUME = dr["MAR_NUME"].ToString();
                    objSAAIO_FACTUR.OBS_COVE2 = dr["OBS_COVE2"].ToString();
                    objSAAIO_FACTUR.NUM_FACT2 = dr["NUM_FACT2"].ToString();
                    objSAAIO_FACTUR.CVE_PROV2 = dr["CVE_PROV2"].ToString();
                }

                else
                {
                    objSAAIO_FACTUR = default;
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

            return objSAAIO_FACTUR;
        }

        public int Insertar(SaaioFactur lsaaio_factur, int IdUsuario)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            try
            {


                cmd.CommandText = "NET_INSERT_SAAIO_FACTUR_EXPERTTI";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                // ,@NUM_REFE  varchar
                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = lsaaio_factur.NUM_REFE;

                // ,@NUM_FACT  varchar
                @param = cmd.Parameters.Add("@NUM_FACT", SqlDbType.VarChar, 15);
                @param.Value = lsaaio_factur.NUM_FACT;

                // ,@FEC_FACT  datetime
                @param = cmd.Parameters.Add("@FEC_FACT", SqlDbType.DateTime, 4);
                @param.Value = lsaaio_factur.FEC_FACT;

                // ,@ICO_FACT  varchar
                @param = cmd.Parameters.Add("@ICO_FACT", SqlDbType.VarChar, 3);
                @param.Value = lsaaio_factur.ICO_FACT;

                // ,@VAL_DLLS  float
                @param = cmd.Parameters.Add("@VAL_DLLS", SqlDbType.Float, 4);
                @param.Value = lsaaio_factur.VAL_DLLS;

                // ,@VAL_EXTR  float
                @param = cmd.Parameters.Add("@VAL_EXTR", SqlDbType.Float, 4);
                @param.Value = lsaaio_factur.VAL_EXTR;


                // ,@CVE_PROV  varchar
                @param = cmd.Parameters.Add("@CVE_PROV", SqlDbType.VarChar, 6);
                @param.Value = lsaaio_factur.CVE_PROV;

                // ,@EQU_DLLS  float
                @param = cmd.Parameters.Add("@EQU_DLLS", SqlDbType.Float, 4);
                @param.Value = lsaaio_factur.EQU_DLLS;

                // ,@MON_FACT  varchar
                @param = cmd.Parameters.Add("@MON_FACT", SqlDbType.VarChar, 3);
                @param.Value = lsaaio_factur.MON_FACT;


                // ,@SUB_FACT  char
                @param = cmd.Parameters.Add("@SUB_FACT", SqlDbType.Char, 1);
                @param.Value = lsaaio_factur.SUB_FACT;

                // ,@CER_ORIG  char
                @param = cmd.Parameters.Add("@CER_ORIG", SqlDbType.Char, 1);
                @param.Value = lsaaio_factur.CER_ORIG;

                // ,@OBS_COVE  varchar
                @param = cmd.Parameters.Add("@OBS_COVE", SqlDbType.VarChar, 250);
                @param.Value = lsaaio_factur.OBS_COVE;

                // ,@NUM_FACT2  varchar
                @param = cmd.Parameters.Add("@NUM_FACT2", SqlDbType.VarChar, 36);
                @param.Value = lsaaio_factur.NUM_FACT2;

                // ,@CVE_PROV2  varchar
                @param = cmd.Parameters.Add("@CVE_PROV2", SqlDbType.VarChar, 6);
                @param.Value = lsaaio_factur.CVE_PROV2;

                @param = cmd.Parameters.Add("@ID_USUARIO", SqlDbType.Int, 4);
                @param.Value = IdUsuario;

                @param = cmd.Parameters.Add("@ORIGEN", SqlDbType.VarChar, 10);
                @param.Value = "PREVIOS";

                @param = cmd.Parameters.Add("@FACT_ORIG", SqlDbType.Bit);
                @param.Value = lsaaio_factur.FACT_ORIG == null ? DBNull.Value : lsaaio_factur.FACT_ORIG;

                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;

                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.ExecuteNonQuery();
                id = (int)cmd.Parameters["@newid_registro"].Value;
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_INSERT_SAAIO_FACTUR");
            }

            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }

        public int Modificar(SaaioFactur lsaaio_factur, int IdUsuario)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            try
            {


                cmd.CommandText = "NET_UPDATE_SAAIO_FACTUR_EXPERTTI";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // ,@NUM_REFE  varchar
                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = lsaaio_factur.NUM_REFE;

                // ,@CONS_FACT  int
                @param = cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4);
                @param.Value = lsaaio_factur.CONS_FACT;

                // ,@NUM_FACT  varchar
                @param = cmd.Parameters.Add("@NUM_FACT", SqlDbType.VarChar, 15);
                @param.Value = lsaaio_factur.NUM_FACT;

                // ,@FEC_FACT  datetime
                @param = cmd.Parameters.Add("@FEC_FACT", SqlDbType.DateTime, 4);
                @param.Value = lsaaio_factur.FEC_FACT;

                // ,@ICO_FACT  varchar
                @param = cmd.Parameters.Add("@ICO_FACT", SqlDbType.VarChar, 3);
                @param.Value = lsaaio_factur.ICO_FACT;

                // ,@VAL_DLLS  float
                @param = cmd.Parameters.Add("@VAL_DLLS", SqlDbType.Float, 4);
                @param.Value = lsaaio_factur.VAL_DLLS;

                // ,@VAL_EXTR  float
                @param = cmd.Parameters.Add("@VAL_EXTR", SqlDbType.Float, 4);
                @param.Value = lsaaio_factur.VAL_EXTR;

                // ,@TIP_MOFA  char
                @param = cmd.Parameters.Add("@TIP_MOFA", SqlDbType.Char, 1);
                @param.Value = lsaaio_factur.TIP_MOFA;

                // ,@CVE_PROV  varchar
                @param = cmd.Parameters.Add("@CVE_PROV", SqlDbType.VarChar, 6);
                @param.Value = lsaaio_factur.CVE_PROV;

                // ,@EQU_DLLS  float
                @param = cmd.Parameters.Add("@EQU_DLLS", SqlDbType.Float, 4);
                @param.Value = lsaaio_factur.EQU_DLLS;

                // ,@DAT_VEHI  varchar
                @param = cmd.Parameters.Add("@DAT_VEHI", SqlDbType.VarChar, 250);
                @param.Value = lsaaio_factur.DAT_VEHI;

                // ,@CAN_FISC  varchar
                @param = cmd.Parameters.Add("@CAN_FISC", SqlDbType.VarChar, 120);
                @param.Value = lsaaio_factur.CAN_FISC;

                // ,@NUM_REM  int
                @param = cmd.Parameters.Add("@NUM_REM", SqlDbType.Int, 4);
                @param.Value = lsaaio_factur.NUM_REM;

                // ,@MON_FACT  varchar
                @param = cmd.Parameters.Add("@MON_FACT", SqlDbType.VarChar, 3);
                @param.Value = lsaaio_factur.MON_FACT;

                // ,@NUM_PEDI  varchar
                @param = cmd.Parameters.Add("@NUM_PEDI", SqlDbType.VarChar, 17);
                @param.Value = lsaaio_factur.NUM_PEDI;

                // ,@PES_BRUT  numeric
                @param = cmd.Parameters.Add("@PES_BRUT", SqlDbType.Decimal, 12);
                @param.Value = lsaaio_factur.PES_BRUT;

                // ,@CAN_BULT  numeric
                @param = cmd.Parameters.Add("@CAN_BULT", SqlDbType.Decimal, 12);
                @param.Value = lsaaio_factur.CAN_BULT;

                // ,@CVE_REFIS  varchar
                @param = cmd.Parameters.Add("@CVE_REFIS", SqlDbType.VarChar, 3);
                @param.Value = lsaaio_factur.CVE_REFIS;

                // ,@NUM_CONT  varchar
                @param = cmd.Parameters.Add("@NUM_CONT", SqlDbType.VarChar, 13);
                @param.Value = lsaaio_factur.NUM_CONT;

                // ,@PESO_GRANEL  numeric
                @param = cmd.Parameters.Add("@PESO_GRANEL", SqlDbType.Decimal, 12);
                @param.Value = lsaaio_factur.PESO_GRANEL;

                // ,@CANT_NIU  int
                @param = cmd.Parameters.Add("@CANT_NIU", SqlDbType.Int, 4);
                @param.Value = lsaaio_factur.CANT_NIU;

                // ,@NUM_IDU  varchar
                @param = cmd.Parameters.Add("@NUM_IDU", SqlDbType.VarChar, 13);
                @param.Value = lsaaio_factur.NUM_IDU;

                // ,@REL_FACT  char
                @param = cmd.Parameters.Add("@REL_FACT", SqlDbType.Char, 1);
                @param.Value = lsaaio_factur.REL_FACT;

                // ,@SUB_FACT  char
                @param = cmd.Parameters.Add("@SUB_FACT", SqlDbType.Char, 1);
                @param.Value = lsaaio_factur.SUB_FACT;

                // ,@CER_ORIG  char
                @param = cmd.Parameters.Add("@CER_ORIG", SqlDbType.Char, 1);
                @param.Value = lsaaio_factur.CER_ORIG;

                // ,@EXP_CONF  varchar
                @param = cmd.Parameters.Add("@EXP_CONF", SqlDbType.VarChar, 50);
                @param.Value = lsaaio_factur.EXP_CONF;

                // ,@OBS_COVE  varchar
                @param = cmd.Parameters.Add("@OBS_COVE", SqlDbType.VarChar, 250);
                @param.Value = lsaaio_factur.OBS_COVE;

                // ,@FOL_COVE  int
                @param = cmd.Parameters.Add("@FOL_COVE", SqlDbType.Int, 4);
                @param.Value = lsaaio_factur.FOL_COVE;

                // ,@EXIS_VINC  char
                @param = cmd.Parameters.Add("@EXIS_VINC", SqlDbType.Char, 1);
                @param.Value = lsaaio_factur.EXIS_VINC;

                // ,@FEC_SALI  datetime
                @param = cmd.Parameters.Add("@FEC_SALI", SqlDbType.DateTime, 4);
                @param.Value = lsaaio_factur.FEC_SALI;

                // ,@MAR_NUME  varchar
                @param = cmd.Parameters.Add("@MAR_NUME", SqlDbType.VarChar, 150);
                @param.Value = lsaaio_factur.MAR_NUME;

                // ,@OBS_COVE2  varchar
                @param = cmd.Parameters.Add("@OBS_COVE2", SqlDbType.VarChar, 250);
                @param.Value = lsaaio_factur.OBS_COVE2;

                // ,@NUM_FACT2  varchar
                @param = cmd.Parameters.Add("@NUM_FACT2", SqlDbType.VarChar, 36);
                @param.Value = lsaaio_factur.NUM_FACT2;

                // ,@CVE_PROV2  varchar
                @param = cmd.Parameters.Add("@CVE_PROV2", SqlDbType.VarChar, 6);
                @param.Value = lsaaio_factur.CVE_PROV2;

                @param = cmd.Parameters.Add("@ID_USUARIO", SqlDbType.Int, 4);
                @param.Value = IdUsuario;

                @param = cmd.Parameters.Add("@ORIGEN", SqlDbType.VarChar, 10);
                @param.Value = "PREVIOS";

                @param = cmd.Parameters.Add("@FACT_ORIG", SqlDbType.Bit);
                @param.Value = lsaaio_factur.FACT_ORIG == null ? DBNull.Value : lsaaio_factur.FACT_ORIG;

                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;

                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.ExecuteNonQuery();
                id = (int)cmd.Parameters["@newid_registro"].Value;
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_INSERT_SAAIO_PEDIME");
            }

            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }

        public int Modificar(string NumerodeReferencia, string CveProveedor)

        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {

                cmd.CommandText = "NET_UPDATE_SAAIO_FACTUR_PROVED_EXPERTTI";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // ,@NUM_REFE  varchar
                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = NumerodeReferencia;

                // ,@CVE_PROV  varchar    
                @param = cmd.Parameters.Add("@CVE_PRO", SqlDbType.VarChar, 6);
                @param.Value = CveProveedor;

                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;

                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.ExecuteNonQuery();
                id = (int)cmd.Parameters["@newid_registro"].Value;
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_INSERT_SAAIO_PEDIME");
            }

            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }

        public bool BorraFacturaDelCasa(string MyNum_Refe, int MyCons_Fact)
        {
            bool BorraFacturaDelCasaRet = default;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            try
            {

                cmd.CommandText = "NET_DELETE_SAAIO_FACTUR_EXPERTTI";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                // ,@NUM_REFE  varchar
                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = MyNum_Refe;

                // ,@CONS_FACT  int
                @param = cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4);
                @param.Value = MyCons_Fact;

                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;

                cn.ConnectionString = SConexion;
                cn.Open();


                cmd.ExecuteNonQuery();

                BorraFacturaDelCasaRet = (bool)cmd.Parameters["@newid_registro"].Value;
                cmd.Parameters.Clear();
            }

            catch (Exception ex)
            {
                cmd.Parameters.Clear();
                BorraFacturaDelCasaRet = false;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString());

            }

            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return BorraFacturaDelCasaRet;


        }

        public TotalDeFacturaParaCOVE TotalDeFactura(string MyNum_Refe, int MyCons_fact)
        {

            var ObjTotalDeFacturaParaCove = new TotalDeFacturaParaCOVE();

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            try
            {
                cmd.CommandText = "NET_LOAD_TOTALDEFACTURA_EXPERTTI";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @NUM_REFE VARCHAR(15) ,
                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = MyNum_Refe;

                // @CONS_FACT INT
                @param = cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4);
                @param.Value = MyCons_fact;

                // Insertar Parametro de busqueda

                cn.ConnectionString = SConexion;
                cn.Open();


                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        ObjTotalDeFacturaParaCove.TotalValorFactura = Convert.ToDouble(dr["TotalValorFactura"]);
                        ObjTotalDeFacturaParaCove.TotalValorPartidasFactura = Convert.ToDouble(dr["TotalValorPartidasFactura"]);
                        ObjTotalDeFacturaParaCove.DiferenciaFactura = dr["DiferenciaFactura"].ToString();
                        ObjTotalDeFacturaParaCove.TotalPartidasFactura = dr["TotalPartidasFactura"].ToString();
                        ObjTotalDeFacturaParaCove.TotalPeso = dr["TotalPeso"].ToString();
                    }
                }


                else
                {
                    ObjTotalDeFacturaParaCove = default;
                }
                dr.Close();
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString());
            }

            return ObjTotalDeFacturaParaCove;
        }

        public DataTable CargarFacturas(string MyNum_Refe)
        {

            var dtb = new DataTable();
            SqlParameter @param;


            try
            {

                using (var cn = new SqlConnection(SConexion))
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();


                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_LOAD_TOTALDEFACTURAS_EXPERTTI";

                    // @NUM_REFE VARCHAR(15)
                    @param = dap.SelectCommand.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                    @param.Value = MyNum_Refe;


                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;

                    dap.Fill(dtb);

                    cn.Close();
                    // SqlConnection.ClearPool(cn)
                    cn.Dispose();

                }
            }



            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dtb;
        }

        public DataTable CargarFacturasPrevios(string MyNum_Refe)
        {

            var dtb = new DataTable();
            SqlParameter @param;


            try
            {

                using (var cn = new SqlConnection(SConexion))
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();


                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_LOAD_FACTURAS_PREVIOS";

                    // @NUM_REFE VARCHAR(15)
                    @param = dap.SelectCommand.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                    @param.Value = MyNum_Refe;


                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;

                    dap.Fill(dtb);

                    cn.Close();
                    // SqlConnection.ClearPool(cn)
                    cn.Dispose();

                }
            }



            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dtb;
        }

        public DataTable VerTodasLasFacturas(string MyNum_Refe)
        {


            var dtb = new DataTable();
            SqlParameter @param;


            try
            {

                using (var cn = new SqlConnection(SConexion))
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();


                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_LOAD_TODAS_LAS_FACTURAS_EXPERTTI";

                    // @NUM_REFE VARCHAR(15)
                    @param = dap.SelectCommand.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                    @param.Value = MyNum_Refe;


                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;

                    dap.Fill(dtb);

                    cn.Close();
                    // SqlConnection.ClearPool(cn)
                    cn.Dispose();

                }
            }



            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dtb;
        }

        public int EXTRAE_MAX_CONS_FACT(string MyNum_Refe)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            try
            {

                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_EXTRAE_MAX_CONS_FACT_EXPERTTI";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // ,@NUM_REFE  varchar
                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = MyNum_Refe;


                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;



                cmd.ExecuteNonQuery();
                id = (int)cmd.Parameters["@newid_registro"].Value;
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_EXTRAE_MAX_SAAIO_FACTUR");
            }

            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }

        /// <summary>
        /// Modifica la vinculacion de todas las partidas
        /// </summary>
        /// <param name="IDReferencia">Id de la referencia</param>
        /// <param name="ConsFact">Constante de la factura</param>
        /// <param name="MyConnectionString"></param>
        /// <returns></returns>
        /// <remarks>Modifica la vinculacion de las partidas, segun lo que diga el proveedor
        /// si el proveedor dice 1 todas las partidas con 0 la vuelve 1 partidas de factura y de pedimento 
        /// no incluye la exportacion </remarks>
        public bool ModificarVinculacion(int IDReferencia, int ConsFact)
        {
            int Modifico;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;



            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_UPDATE_VINCULACION_EXPERTTI";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // ,@IDReferencia  varchar
                @param = cmd.Parameters.Add("@IDReferencia", SqlDbType.Int, 4);
                @param.Value = IDReferencia;

                // ,@CONS_FACT  int
                @param = cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4);
                @param.Value = ConsFact;

                cmd.ExecuteNonQuery();
                Modifico = Convert.ToInt32(true);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                Modifico = Convert.ToInt32(false);
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_UPDATE_VINCULACION");
            }

            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return Convert.ToBoolean(Modifico);
        }




        public DataTable BuscarValordeTransaccion(string NumRefe)
        {

            var dtb = new DataTable();
            SqlParameter @param;

            using (var cn = new SqlConnection(SConexion))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_LOAD_FACTURAS_VALORDETRANSACCION_EXPERTTI";


                    // @IDReferencia INT
                    @param = dap.SelectCommand.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 25);
                    @param.Value = NumRefe;


                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;

                    dap.Fill(dtb);

                    cn.Close();
                    cn.Dispose();
                }

                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();

                    throw new Exception(ex.Message.ToString() + "NET_LOAD_FACTURAS_VALORDETRANSACCION");
                }

            }

            return dtb;
        }


        public string BuscaProveedorParaReferencia(string MyNum_Refe)
        {
            string BuscaProveedorParaReferenciaRet = default;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            try
            {

                cmd.CommandText = "NET_SEARCH_RELACIONDEPROVEEDORESCONREFERENCIAS";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                // ,@NumeroDeReferencia VARCHAR(15)
                @param = cmd.Parameters.Add("@NumeroDeReferencia", SqlDbType.VarChar, 15);
                @param.Value = MyNum_Refe;

                // ,@ClaveDeProveedor VARCHAR(6) OUTPUT 
                @param = cmd.Parameters.Add("@ClaveDeProveedor", SqlDbType.VarChar, 6);
                @param.Direction = ParameterDirection.Output;

                cn.ConnectionString = SConexion;
                cn.Open();


                cmd.ExecuteNonQuery();

                BuscaProveedorParaReferenciaRet = Convert.ToString(cmd.Parameters["@ClaveDeProveedor"].Value);
                cmd.Parameters.Clear();
            }

            catch (Exception ex)
            {
                cmd.Parameters.Clear();
                BuscaProveedorParaReferenciaRet = "";
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString());

            }

            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return BuscaProveedorParaReferenciaRet;


        }


        public int Validada(string NumerodeReferencia, int ConsFact, int CONSFACTCASA)


        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {

                cmd.CommandText = "NET_UPDATE_SAAIO_FACTUR_EXPERTTI_Validada";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // ,@NUM_REFE  varchar
                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = NumerodeReferencia;

                @param = cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4);
                @param.Value = ConsFact;

                @param = cmd.Parameters.Add("@Validada", SqlDbType.Bit);
                @param.Value = 1;

                @param = cmd.Parameters.Add("@CONSFACTCASA", SqlDbType.Int, 4);
                @param.Value = CONSFACTCASA;

                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;

                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.ExecuteNonQuery();
                id = (int)cmd.Parameters["@newid_registro"].Value;
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_UPDATE_SAAIO_FACTUR_EXPERTTI_Validada");
            }

            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }


        public int InsertarNew(SaaioFactur lsaaio_factur)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_INSERT_SAAIO_FACTUR_EXPERTTI_NEW";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // ,@NUM_REFE  varchar
            @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
            @param.Value = lsaaio_factur.NUM_REFE == null ? DBNull.Value : lsaaio_factur.NUM_REFE;

            // ,@NUM_FACT  varchar
            @param = cmd.Parameters.Add("@NUM_FACT", SqlDbType.VarChar, 15);
            @param.Value = lsaaio_factur.NUM_FACT == null ? DBNull.Value : lsaaio_factur.NUM_FACT;

            // ,@FEC_FACT  datetime
            @param = cmd.Parameters.Add("@FEC_FACT", SqlDbType.DateTime, 4);
            @param.Value = lsaaio_factur.FEC_FACT == null ? DBNull.Value : lsaaio_factur.FEC_FACT;

            // ,@ICO_FACT  varchar
            @param = cmd.Parameters.Add("@ICO_FACT", SqlDbType.VarChar, 3);
            @param.Value = lsaaio_factur.ICO_FACT == null ? DBNull.Value : lsaaio_factur.ICO_FACT;

            // ,@VAL_DLLS  float
            @param = cmd.Parameters.Add("@VAL_DLLS", SqlDbType.Float, 4);
            @param.Value = lsaaio_factur.VAL_DLLS == null ? DBNull.Value : lsaaio_factur.VAL_DLLS;

            // ,@VAL_EXTR  float
            @param = cmd.Parameters.Add("@VAL_EXTR", SqlDbType.Float, 4);
            @param.Value = lsaaio_factur.VAL_EXTR == null ? DBNull.Value : lsaaio_factur.VAL_EXTR;

            // ,@TIP_MOFA  char
            @param = cmd.Parameters.Add("@TIP_MOFA", SqlDbType.Char, 1);
            @param.Value = lsaaio_factur.TIP_MOFA == null ? DBNull.Value : lsaaio_factur.TIP_MOFA;

            // ,@CVE_PROV  varchar
            @param = cmd.Parameters.Add("@CVE_PROV", SqlDbType.VarChar, 6);
            @param.Value = lsaaio_factur.CVE_PROV == null ? DBNull.Value : lsaaio_factur.CVE_PROV;

            // ,@EQU_DLLS  float
            @param = cmd.Parameters.Add("@EQU_DLLS", SqlDbType.Float, 4);
            @param.Value = lsaaio_factur.EQU_DLLS == null ? DBNull.Value : lsaaio_factur.EQU_DLLS;

            // ,@DAT_VEHI  varchar
            @param = cmd.Parameters.Add("@DAT_VEHI", SqlDbType.VarChar, 250);
            @param.Value = lsaaio_factur.DAT_VEHI == null ? DBNull.Value : lsaaio_factur.DAT_VEHI;

            // ,@CAN_FISC  varchar
            @param = cmd.Parameters.Add("@CAN_FISC", SqlDbType.VarChar, 120);
            @param.Value = lsaaio_factur.CAN_FISC == null ? DBNull.Value : lsaaio_factur.CAN_FISC;

            // ,@NUM_REM  int
            @param = cmd.Parameters.Add("@NUM_REM", SqlDbType.Int, 4);
            @param.Value = lsaaio_factur.NUM_REM == null ? DBNull.Value : lsaaio_factur.NUM_REM;

            // ,@MON_FACT  varchar
            @param = cmd.Parameters.Add("@MON_FACT", SqlDbType.VarChar, 3);
            @param.Value = lsaaio_factur.MON_FACT == null ? DBNull.Value : lsaaio_factur.MON_FACT;

            // ,@NUM_PEDI  varchar
            @param = cmd.Parameters.Add("@NUM_PEDI", SqlDbType.VarChar, 17);
            @param.Value = lsaaio_factur.NUM_PEDI == null ? DBNull.Value : lsaaio_factur.NUM_PEDI;

            // ,@PES_BRUT  float
            @param = cmd.Parameters.Add("@PES_BRUT", SqlDbType.Float, 4);
            @param.Value = lsaaio_factur.PES_BRUT == null ? DBNull.Value : lsaaio_factur.PES_BRUT;

            // ,@CAN_BULT  float
            @param = cmd.Parameters.Add("@CAN_BULT", SqlDbType.Float, 4);
            @param.Value = lsaaio_factur.CAN_BULT == null ? DBNull.Value : lsaaio_factur.CAN_BULT;

            // ,@CVE_REFIS  varchar
            @param = cmd.Parameters.Add("@CVE_REFIS", SqlDbType.VarChar, 3);
            @param.Value = lsaaio_factur.CVE_REFIS == null ? DBNull.Value : lsaaio_factur.CVE_REFIS;

            // ,@NUM_CONT  varchar
            @param = cmd.Parameters.Add("@NUM_CONT", SqlDbType.VarChar, 13);
            @param.Value = lsaaio_factur.NUM_CONT == null ? DBNull.Value : lsaaio_factur.NUM_CONT;

            // ,@PESO_GRANEL  float
            @param = cmd.Parameters.Add("@PESO_GRANEL", SqlDbType.Float, 4);
            @param.Value = lsaaio_factur.PESO_GRANEL == null ? DBNull.Value : lsaaio_factur.PESO_GRANEL;

            // ,@CANT_NIU  int
            @param = cmd.Parameters.Add("@CANT_NIU", SqlDbType.Int, 4);
            @param.Value = lsaaio_factur.CANT_NIU == null ? DBNull.Value : lsaaio_factur.CANT_NIU;

            // ,@NUM_IDU  varchar
            @param = cmd.Parameters.Add("@NUM_IDU", SqlDbType.VarChar, 13);
            @param.Value = lsaaio_factur.NUM_IDU == null ? DBNull.Value : lsaaio_factur.NUM_IDU;

            // ,@REL_FACT  char
            @param = cmd.Parameters.Add("@REL_FACT", SqlDbType.Char, 1);
            @param.Value = lsaaio_factur.REL_FACT == null ? DBNull.Value : lsaaio_factur.REL_FACT;

            // ,@SUB_FACT  char
            @param = cmd.Parameters.Add("@SUB_FACT", SqlDbType.Char, 1);
            @param.Value = lsaaio_factur.SUB_FACT == null ? DBNull.Value : lsaaio_factur.SUB_FACT;

            // ,@CER_ORIG  char
            @param = cmd.Parameters.Add("@CER_ORIG", SqlDbType.Char, 1);
            @param.Value = lsaaio_factur.CER_ORIG == null ? DBNull.Value : lsaaio_factur.CER_ORIG;

            // ,@EXP_CONF  varchar
            @param = cmd.Parameters.Add("@EXP_CONF", SqlDbType.VarChar, 50);
            @param.Value = lsaaio_factur.EXP_CONF == null ? DBNull.Value : lsaaio_factur.EXP_CONF;

            // ,@OBS_COVE  varchar
            @param = cmd.Parameters.Add("@OBS_COVE", SqlDbType.VarChar, 250);
            @param.Value = lsaaio_factur.OBS_COVE == null ? DBNull.Value : lsaaio_factur.OBS_COVE;

            // ,@FOL_COVE  int
            @param = cmd.Parameters.Add("@FOL_COVE", SqlDbType.Int, 4);
            @param.Value = lsaaio_factur.FOL_COVE == null ? DBNull.Value : lsaaio_factur.FOL_COVE;

            // ,@EXIS_VINC  char
            @param = cmd.Parameters.Add("@EXIS_VINC", SqlDbType.Char, 1);
            @param.Value = lsaaio_factur.EXIS_VINC == null ? DBNull.Value : lsaaio_factur.EXIS_VINC;

            // ,@FEC_SALI  datetime
            @param = cmd.Parameters.Add("@FEC_SALI", SqlDbType.DateTime, 4);
            // param.Value = "01/01/1900"
            // param.Value = If(IsNothing(lsaaio_factur.FEC_SALI), DBNull.Value, lsaaio_factur.FEC_SALI)
            @param.Value = DBNull.Value;

            // ,@MAR_NUME  varchar
            @param = cmd.Parameters.Add("@MAR_NUME", SqlDbType.VarChar, 150);
            @param.Value = lsaaio_factur.MAR_NUME == null ? DBNull.Value : lsaaio_factur.MAR_NUME;

            // ,@OBS_COVE2  varchar
            @param = cmd.Parameters.Add("@OBS_COVE2", SqlDbType.VarChar, 250);
            @param.Value = lsaaio_factur.OBS_COVE2 == null ? DBNull.Value : lsaaio_factur.OBS_COVE2;

            // ,@NUM_FACT2  varchar
            @param = cmd.Parameters.Add("@NUM_FACT2", SqlDbType.VarChar, 36);
            @param.Value = lsaaio_factur.NUM_FACT2 == null ? DBNull.Value : lsaaio_factur.NUM_FACT2;

            // ,@CVE_PROV2  varchar
            @param = cmd.Parameters.Add("@CVE_PROV2", SqlDbType.VarChar, 6);
            @param.Value = lsaaio_factur.CVE_PROV2 == null ? DBNull.Value : lsaaio_factur.CVE_PROV2;


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
                throw new Exception(ex.Message.ToString() + "NET_INSERT_SAAIO_FACTUR_NEW");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }

        public DataTable CargarFacturasyPartidas(string MyNum_Refe)
        {

            var dtb = new DataTable();
            SqlParameter @param;


            try
            {

                using (var cn = new SqlConnection(SConexion))
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();


                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_LOAD_PARTIDASDEREFERENCIA";

                    // @NUM_REFE VARCHAR(15)
                    @param = dap.SelectCommand.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                    @param.Value = MyNum_Refe;


                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;

                    dap.Fill(dtb);

                    cn.Close();
                    // SqlConnection.ClearPool(cn)
                    cn.Dispose();

                }
            }



            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dtb;
        }
        public int NET_SABER_SI_LA_FACTURA_YA_EXISTE(string MyNum_Refe, string MyNum_fact)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                cmd.CommandText = "NET_SABER_SI_LA_SAAIO_FACTUR_EXPERTTI_YA_EXISTE";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                // @NUM_REFE VARCHAR(15)
                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = MyNum_Refe;

                // ,@NUM_FACT VARCHAR(15)
                @param = cmd.Parameters.Add("@NUM_FACT", SqlDbType.VarChar, 15);
                @param.Value = MyNum_fact;


                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;

                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.ExecuteNonQuery();
                id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_SABER_SI_LA_FACTURA_YA_EXISTE");
            }
            cn.Close();
            cn.Dispose();
            return id;

        }
        /// <summary>
        /// Importa la informacion de sistema expertti a sistema casa
        /// </summary>
        /// <param name="NUM_REFE"></param>
        /// <param name="MyConnectionString"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool ImportarPrecaptura(string NUM_REFE)
        {
            bool Correcto;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_IMPORT_PRECAPTURA";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // ,@NUM_REFE  varchar
            @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
            @param.Value = NUM_REFE;


            try
            {
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                Correcto = true;
            }
            catch (Exception ex)
            {
                Correcto = false;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_SAAIO_FACTUR_NEW");
            }
            cn.Close();
            cn.Dispose();
            return Correcto;
        }

        /// <summary>
        /// Exporta Informacion del sistema casa a expertti
        /// </summary>
        /// <param name="NUM_REFE"></param>
        /// <param name="MyConnectionString"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool ExportarPrecaptura(string NUM_REFE)
        {
            bool Correcto;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_EXPORT_PRECAPTURA";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // ,@NUM_REFE  varchar
            @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
            @param.Value = NUM_REFE;


            try
            {
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                Correcto = true;
            }
            catch (Exception ex)
            {
                Correcto = false;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString());
            }
            cn.Close();
            cn.Dispose();
            return Correcto;
        }

        public int ValidaCoveFac(string NUM_REFE, string MyConnectionString)
        {
            int existe;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            try
            {
                cn.ConnectionString = MyConnectionString;
                cn.Open();

                cmd.CommandText = "NET_VALIDATE_EXIST_COVE_FAC";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = NUM_REFE;

                @param = cmd.Parameters.Add("@SUCCESS", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;


                cmd.ExecuteNonQuery();
                existe = (int)cmd.Parameters["@SUCCESS"].Value;
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                existe = 0;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_VALIDATE_EXIST_COVE_FAC");
            }

            cn.Close();
            cn.Dispose();
            return existe;

        }

        public bool NET_DUPLICA_SAAIO_FACPAR(string MyNum_Refe, int MyCons_fact, int MyCons_Part, int Cuantas, int CopiaPartidas, int CopiaIdePart, int CopiaIdePerm, int CopiaCoveSer, string MyConnectionString)
        {


            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {

                cmd.CommandText = "NET_DUPLICA_SAAIO_FACPART";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                // @NUM_REFE VARCHAR(15)
                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = MyNum_Refe;

                // ,@CONS_FACT INT
                @param = cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4);
                @param.Value = MyCons_fact;

                // ,@CONS_PART INT
                @param = cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4);
                @param.Value = MyCons_Part;

                // ,@Cuantas INT 
                @param = cmd.Parameters.Add("@Cuantas", SqlDbType.Int, 4);
                @param.Value = Cuantas;

                // @CopiaIdePart
                @param = cmd.Parameters.Add("@CopiaIdePart", SqlDbType.Int, 4);
                @param.Value = CopiaIdePart;

                // @CopiaIdePerm
                @param = cmd.Parameters.Add("@CopiaIdePerm", SqlDbType.Int, 4);
                @param.Value = CopiaIdePerm;

                // @CopiaCoveSer
                @param = cmd.Parameters.Add("@CopiaCoveSer", SqlDbType.Int, 4);
                @param.Value = CopiaCoveSer;



                cn.ConnectionString = MyConnectionString;
                cn.Open();

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {

                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString());
            }
            cn.Close();
            cn.Dispose();
            return true;

        }

        public bool NET_DUPLICA_SAAIO_FACPART_EXPERTTI_PREVIOS(string MyNum_Refe, int MyCons_fact, int MyCons_Part, int Cuantas, int CopiaPartidas, int CopiaIdePart, int CopiaIdePerm, int CopiaCoveSer, string MyConnectionString)
        {


            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {

                cmd.CommandText = "NET_DUPLICA_SAAIO_FACPART_EXPERTTI_PREVIOS";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                // @NUM_REFE VARCHAR(15)
                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = MyNum_Refe;

                // ,@CONS_FACT INT
                @param = cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4);
                @param.Value = MyCons_fact;

                // ,@CONS_PART INT
                @param = cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4);
                @param.Value = MyCons_Part;

                // ,@Cuantas INT 
                @param = cmd.Parameters.Add("@Cuantas", SqlDbType.Int, 4);
                @param.Value = Cuantas;

                // @CopiaIdePart
                @param = cmd.Parameters.Add("@CopiaIdePart", SqlDbType.Int, 4);
                @param.Value = CopiaIdePart;

                // @CopiaIdePerm
                @param = cmd.Parameters.Add("@CopiaIdePerm", SqlDbType.Int, 4);
                @param.Value = CopiaIdePerm;

                // @CopiaCoveSer
                @param = cmd.Parameters.Add("@CopiaCoveSer", SqlDbType.Int, 4);
                @param.Value = CopiaCoveSer;



                cn.ConnectionString = MyConnectionString;
                cn.Open();

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {

                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString());
            }
            cn.Close();
            cn.Dispose();
            return true;

        }

        public bool CambioNumeroFactura(SaaioFactur saaiofactur, string myConnectionString)
        {

            SqlDataReader dataReader;
            string query = string.Empty;
            query += "SELECT ISNULL(NUM_FACT,'') AS NUM_FACT,ISNULL(NUM_FACT2,'') AS NUM_FACT2 FROM SAAIO_FACTUR_EXPERTTI WHERE NUM_REFE = @NUM_REFE AND CONS_FACT = @CONS_FACT;";

            var numeroFactura1 = default(string);
            var numeroFactura2 = default(string);
            using (var con = new SqlConnection(myConnectionString))
            {
                using (var com = new SqlCommand())
                {
                    com.Connection = con;
                    com.CommandType = CommandType.Text;
                    com.CommandText = query;
                    com.Parameters.AddWithValue("@NUM_REFE", saaiofactur.NUM_REFE);
                    com.Parameters.AddWithValue("@CONS_FACT", saaiofactur.CONS_FACT);

                    try
                    {
                        con.Open();
                        dataReader = com.ExecuteReader();

                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                numeroFactura1 = dataReader["NUM_FACT"].ToString();
                                numeroFactura2 = dataReader["NUM_FACT2"].ToString();
                            }
                        }
                        else
                        {

                            throw new Exception("No se encontro factura con consecutivo " + saaiofactur.CONS_FACT + " para la referencia " + saaiofactur.NUM_REFE);
                        }

                        dataReader.Close();
                        con.Close();
                        con.Dispose();
                        com.Parameters.Clear();
                    }

                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message.ToString());
                        dataReader.Close();
                        con.Close();
                        con.Dispose();
                        com.Parameters.Clear();

                    }

                    bool cambio = false;

                    if (numeroFactura1 != saaiofactur.NUM_FACT)
                    {
                        cambio = true;
                    }

                    if (numeroFactura2 != saaiofactur.NUM_FACT2)
                    {
                        if (numeroFactura2.Contains(saaiofactur.NUM_FACT2))
                        {
                            saaiofactur.NUM_FACT2 = numeroFactura2;
                        }
                    }

                    return cambio;
                }
            }
        }
    }

}
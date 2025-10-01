using DocumentFormat.OpenXml.Presentation;
using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Data.SqlClient;


namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class SaaioPedimeRepository:ISaaioPedimeRepository
    {
        public string SConexion { get; set; }
        string ISaaioPedimeRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public SaaioPedimeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }


        public int ModificarPagoElectronicoPECE(
    string MiReferencia,
    DateTime DiaPago,
    string MiFirmaDePago,
    string MiOperacion,
    string MiHoraDePago,
    string MiImpuestos,
    string MiTrnLine,
    string MiArchivo,
    int MiIDUsuario)
        {
            int id;

            using (SqlConnection cn = new SqlConnection(SConexion))
            using (SqlCommand cmd = new SqlCommand("NET_UPDATE_SAAIO_PEDIME_PAGO_PEDE", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters
                cmd.Parameters.AddWithValue("@NUM_REFE", MiReferencia);
                cmd.Parameters.AddWithValue("@DIA_PAGO", DiaPago);
                cmd.Parameters.AddWithValue("@FIR_PAGO", MiFirmaDePago);
                cmd.Parameters.AddWithValue("@NUM_OPER", MiOperacion);
                cmd.Parameters.AddWithValue("@HOR_PAGO", MiHoraDePago);

                // Assuming 'objHelp.EliminaComas' is a method that returns a cleaned string
                Helper objHelp = new Helper();
                cmd.Parameters.AddWithValue("@PAG_IMP", objHelp.EliminaComas(MiImpuestos));

                cmd.Parameters.AddWithValue("@TRN_LCAPT", MiTrnLine);
                cmd.Parameters.AddWithValue("@ARCH_CPAGO", MiArchivo);
                cmd.Parameters.AddWithValue("@IdUsuario", MiIDUsuario);

                SqlParameter outputParam = new SqlParameter
                {
                    ParameterName = "@newid_registro",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    id = (int)cmd.Parameters["@newid_registro"].Value;
                }
                catch (Exception ex)
                {
                    id = 0;
                    throw new Exception(ex.Message + " NET_UPDATE_SAAIO_PEDIME_PAGO_PEDE");
                }
            }

            return id;
        }
        public SaaioPedime Buscar(string NumRefe)
        {
            SaaioPedime objSaaioPedime = null;


            try
            {
                using SqlConnection cn = new(SConexion);
                using (SqlCommand cmd = new("NET_SEARCH_SAAIO_PEDIME", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NumRefe;

                    cn.ConnectionString = SConexion;
                    cn.Open();

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        objSaaioPedime = new SaaioPedime();
                        dr.Read();
                        objSaaioPedime.NUM_REFE = dr["NUM_REFE"] == DBNull.Value ? string.Empty : dr["NUM_REFE"].ToString()!;
                        objSaaioPedime.CVE_IMPO = dr["CVE_IMPO"] == DBNull.Value ? string.Empty : dr["CVE_IMPO"].ToString()!;
                        objSaaioPedime.IMP_EXPO = dr["IMP_EXPO"].ToString();
                        objSaaioPedime.TIP_PEDI = dr["TIP_PEDI"].ToString();
                        objSaaioPedime.ADU_DESP = dr["ADU_DESP"].ToString();
                        objSaaioPedime.PAT_AGEN = dr["PAT_AGEN"].ToString();
                        objSaaioPedime.NUM_PEDI = dr["NUM_PEDI"].ToString();
                        objSaaioPedime.ADU_ENTR = dr["ADU_ENTR"].ToString();
                        objSaaioPedime.FEC_ENTR = Convert.ToDateTime(dr["FEC_ENTR"]);
                        objSaaioPedime.TIP_CAMB = Convert.ToDouble(dr["TIP_CAMB"]);
                        objSaaioPedime.CVE_PEDI = dr["CVE_PEDI"].ToString();
                        objSaaioPedime.REG_ADUA = dr["REG_ADUA"].ToString();
                        objSaaioPedime.AUT_REGI = dr["AUT_REGI"].ToString();
                        objSaaioPedime.CVE_ALMA = dr["CVE_ALMA"].ToString();
                        objSaaioPedime.FEC_ESPE = Convert.ToDateTime(dr["FEC_ESPE"]);
                        objSaaioPedime.DES_ORIG = Convert.ToInt32(dr["DES_ORIG"]);
                        objSaaioPedime.MAR_NUME = dr["MAR_NUME"].ToString();
                        objSaaioPedime.PES_BRUT = Convert.ToDouble(dr["PES_BRUT"]);
                        objSaaioPedime.MTR_ENTR = dr["MTR_ENTR"].ToString();
                        objSaaioPedime.MTR_ARRI = dr["MTR_ARRI"].ToString();
                        objSaaioPedime.MTR_SALI = dr["MTR_SALI"].ToString();
                        objSaaioPedime.MON_VASE = Convert.ToDouble(dr["MON_VASE"]);
                        objSaaioPedime.TIP_MOVA = dr["TIP_MOVA"].ToString();
                        objSaaioPedime.VAL_DLLS = Convert.ToDouble(dr["VAL_DLLS"]);
                        objSaaioPedime.VAL_COME = Convert.ToDouble(dr["VAL_COME"]);
                        objSaaioPedime.TOT_INCR = Convert.ToDouble(dr["TOT_INCR"]);
                        objSaaioPedime.TOT_DEDU = Convert.ToDouble(dr["TOT_DEDU"]);
                        objSaaioPedime.VAL_NORM = Convert.ToDouble(dr["VAL_NORM"]);
                        objSaaioPedime.FAC_AJUS = Convert.ToDouble(dr["FAC_AJUS"]);
                        objSaaioPedime.CVE_CAPT = dr["CVE_CAPT"].ToString();
                        objSaaioPedime.FEC_PAGO = Convert.ToDateTime(dr["FEC_PAGO"]);
                        objSaaioPedime.CVE_REPR = dr["CVE_REPR"].ToString();
                        objSaaioPedime.TAS_DTA = Convert.ToDouble(dr["TAS_DTA"]);
                        objSaaioPedime.TTA_DTA = dr["TTA_DTA"].ToString();
                        objSaaioPedime.PAR_2DTA = Convert.ToDouble(dr["PAR_2DTA"]);
                        objSaaioPedime.FAC_ACTU = Convert.ToDouble(dr["FAC_ACTU"]);
                        objSaaioPedime.NUM_FRAC = Convert.ToInt32(dr["NUM_FRAC"]);
                        objSaaioPedime.TOT_EFEC = Convert.ToDouble(dr["TOT_EFEC"]);
                        objSaaioPedime.TOT_OTRO = Convert.ToDouble(dr["TOT_OTRO"]);
                        objSaaioPedime.FIR_ELEC = dr["FIR_ELEC"].ToString();
                        objSaaioPedime.NUM_CAND = dr["NUM_CAND"].ToString();
                        objSaaioPedime.TOT_VEHI = Convert.ToInt32(dr["TOT_VEHI"]);
                        objSaaioPedime.NUM_REFEO = dr["NUM_REFEO"].ToString();
                        objSaaioPedime.PAT_AGENO = dr["PAT_AGENO"].ToString();
                        objSaaioPedime.NUM_PEDIO = dr["NUM_PEDIO"].ToString();
                        objSaaioPedime.FEC_PAGOO = Convert.ToDateTime(dr["FEC_PAGOO"]);
                        objSaaioPedime.CVE_PEDIO = dr["CVE_PEDIO"].ToString();
                        objSaaioPedime.ADU_DESPO = dr["ADU_DESPO"].ToString();
                        objSaaioPedime.CVE_BANC = dr["CVE_BANC"].ToString();
                        objSaaioPedime.NUM_CAJA = dr["NUM_CAJA"].ToString();
                        objSaaioPedime.DIA_PAGO = Convert.ToDateTime(dr["DIA_PAGO"]);
                        objSaaioPedime.HOR_PAGO = dr["HOR_PAGO"].ToString();
                        objSaaioPedime.TOT_PAGO = Convert.ToDouble(dr["TOT_PAGO"]);
                        objSaaioPedime.TIP_ACTU = dr["TIP_ACTU"].ToString();
                        objSaaioPedime.CVE_REFIS = dr["CVE_REFIS"].ToString();
                        objSaaioPedime.CAN_BULT = Convert.ToDouble(dr["CAN_BULT"]);
                        objSaaioPedime.FIR_REME = dr["FIR_REME"].ToString();
                        objSaaioPedime.SEC_DESP = dr["SEC_DESP"].ToString();
                        objSaaioPedime.ADU_TRAN = dr["ADU_TRAN"].ToString();
                        objSaaioPedime.COP_REFE = dr["COP_REFE"].ToString();
                        objSaaioPedime.CAN_BULTSUBD = Convert.ToDouble(dr["CAN_BULTSUBD"]);
                        objSaaioPedime.FIR_PAGO = dr["FIR_PAGO"].ToString();
                        objSaaioPedime.NUM_OPER = dr["NUM_OPER"].ToString();
                        objSaaioPedime.REG_ENTRADA = dr["REG_ENTRADA"].ToString();
                        objSaaioPedime.CVE_CNTA = dr["CVE_CNTA"].ToString();
                        objSaaioPedime.EMP_FACT = dr["EMP_FACT"].ToString();

                    }

                }



            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objSaaioPedime;
        }
        public int ModificarPagoElectronico(SaaioPedime lPedime, int IdUsuario)
        {
            int id;

            using (SqlConnection cn = new SqlConnection(SConexion))
            using (SqlCommand cmd = new SqlCommand("NET_UPDATE_SAAIO_PEDIME_PAGO", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters
                cmd.Parameters.AddWithValue("@NUM_REFE", lPedime.NUM_REFE);
                cmd.Parameters.AddWithValue("@NUM_CAJA", lPedime.NUM_CAJA);
                cmd.Parameters.AddWithValue("@DIA_PAGO", lPedime.DIA_PAGO);
                cmd.Parameters.AddWithValue("@FIR_PAGO", lPedime.FIR_PAGO);
                cmd.Parameters.AddWithValue("@NUM_OPER", lPedime.NUM_OPER);
                cmd.Parameters.AddWithValue("@HOR_PAGO", lPedime.HOR_PAGO);
                cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);

                SqlParameter outputParam = new SqlParameter
                {
                    ParameterName = "@newid_registro",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    id = (int)cmd.Parameters["@newid_registro"].Value;
                }
                catch (Exception ex)
                {
                    id = 0;
                    throw new Exception(ex.Message + " NET_UPDATE_SAAIO_PEDIME_PAGO");
                }
            }

            return id;
        }
        public int Insertar(SaaioPedime lsaaio_pedime)
        {

            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_INSERT_SAAIO_PEDIME";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // ,@NUM_REFE  varchar
            @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
            @param.Value = lsaaio_pedime.NUM_REFE == null ? DBNull.Value : lsaaio_pedime.NUM_REFE;

            // ,@CVE_IMPO  varchar
            @param = cmd.Parameters.Add("@CVE_IMPO", SqlDbType.VarChar, 6);
            @param.Value = lsaaio_pedime.CVE_IMPO == null ? DBNull.Value : lsaaio_pedime.CVE_IMPO;

            // ,@IMP_EXPO  char
            @param = cmd.Parameters.Add("@IMP_EXPO", SqlDbType.Char, 1);
            @param.Value = lsaaio_pedime.IMP_EXPO == null ? DBNull.Value : lsaaio_pedime.IMP_EXPO;

            // ,@TIP_PEDI  varchar
            @param = cmd.Parameters.Add("@TIP_PEDI", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.TIP_PEDI == null ? DBNull.Value : lsaaio_pedime.TIP_PEDI;

            // ,@ADU_DESP  varchar
            @param = cmd.Parameters.Add("@ADU_DESP", SqlDbType.VarChar, 3);
            @param.Value = lsaaio_pedime.ADU_DESP == null ? DBNull.Value : lsaaio_pedime.ADU_DESP;

            // ,@PAT_AGEN  varchar
            @param = cmd.Parameters.Add("@PAT_AGEN", SqlDbType.VarChar, 4);
            @param.Value = lsaaio_pedime.PAT_AGEN == null ? DBNull.Value : lsaaio_pedime.PAT_AGEN;

            // ,@NUM_PEDI  varchar
            @param = cmd.Parameters.Add("@NUM_PEDI", SqlDbType.VarChar, 30);
            @param.Value = lsaaio_pedime.NUM_PEDI == null ? DBNull.Value : lsaaio_pedime.NUM_PEDI;

            // ,@ADU_ENTR  varchar
            @param = cmd.Parameters.Add("@ADU_ENTR", SqlDbType.VarChar, 3);
            @param.Value = lsaaio_pedime.ADU_ENTR == null ? DBNull.Value : lsaaio_pedime.ADU_ENTR;

            // ,@FEC_ENTR  datetime
            @param = cmd.Parameters.Add("@FEC_ENTR", SqlDbType.DateTime, 10);
            @param.Value = lsaaio_pedime.FEC_ENTR;

            // ,@TIP_CAMB  float
            @param = cmd.Parameters.Add("@TIP_CAMB", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.TIP_CAMB == null ? DBNull.Value : lsaaio_pedime.TIP_CAMB;

            // ,@CVE_PEDI  varchar
            @param = cmd.Parameters.Add("@CVE_PEDI", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.CVE_PEDI == null ? DBNull.Value : lsaaio_pedime.CVE_PEDI;

            // ,@REG_ADUA  varchar
            @param = cmd.Parameters.Add("@REG_ADUA", SqlDbType.VarChar, 3);
            @param.Value = lsaaio_pedime.REG_ADUA == null ? DBNull.Value : lsaaio_pedime.REG_ADUA;

            // ,@AUT_REGI  varchar
            @param = cmd.Parameters.Add("@AUT_REGI", SqlDbType.VarChar, 20);
            @param.Value = lsaaio_pedime.AUT_REGI == null ? DBNull.Value : lsaaio_pedime.AUT_REGI;

            // ,@CVE_ALMA  varchar
            @param = cmd.Parameters.Add("@CVE_ALMA", SqlDbType.VarChar, 4);
            @param.Value = lsaaio_pedime.CVE_ALMA == null ? DBNull.Value : lsaaio_pedime.CVE_ALMA;

            // ,@FEC_ESPE  datetime
            @param = cmd.Parameters.Add("@FEC_ESPE", SqlDbType.DateTime, 4);
            @param.Value = DBNull.Value;

            // 
            // param.Value = If(IsNothing(lsaaio_pedime.FEC_ESPE), DBNull.Value, lsaaio_pedime.FEC_ESPE)
            // param.Value = If(lsaaio_pedime.FEC_ESPE = "01/01/1900", DBNull.Value, lsaaio_pedime.FEC_ESPE)


            // ,@DES_ORIG  int
            @param = cmd.Parameters.Add("@DES_ORIG", SqlDbType.Int, 4);
            @param.Value = lsaaio_pedime.DES_ORIG == null ? DBNull.Value : lsaaio_pedime.DES_ORIG;

            // ,@MAR_NUME  varchar
            @param = cmd.Parameters.Add("@MAR_NUME", SqlDbType.VarChar, 150);
            @param.Value = lsaaio_pedime.MAR_NUME == null ? DBNull.Value : lsaaio_pedime.MAR_NUME;

            // ,@PES_BRUT  float
            @param = cmd.Parameters.Add("@PES_BRUT", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.PES_BRUT == null ? DBNull.Value : lsaaio_pedime.PES_BRUT;

            // ,@MTR_ENTR  varchar
            @param = cmd.Parameters.Add("@MTR_ENTR", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.MTR_ENTR == null ? DBNull.Value : lsaaio_pedime.MTR_ENTR;

            // ''''''''''''''''''''''''''''''''
            // ,@MTR_ARRI  varchar
            @param = cmd.Parameters.Add("@MTR_ARRI", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.MTR_ARRI == null ? DBNull.Value : lsaaio_pedime.MTR_ARRI;

            // ,@MTR_SALI  varchar
            @param = cmd.Parameters.Add("@MTR_SALI", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.MTR_SALI == null ? DBNull.Value : lsaaio_pedime.MTR_SALI;

            // ,@MON_VASE  numeric
            @param = cmd.Parameters.Add("@MON_VASE", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.MON_VASE == null ? DBNull.Value : lsaaio_pedime.MON_VASE;

            // ,@TIP_MOVA  varchar
            @param = cmd.Parameters.Add("@TIP_MOVA", SqlDbType.VarChar, 3);
            @param.Value = lsaaio_pedime.TIP_MOVA;

            // ,@VAL_DLLS  numeric
            @param = cmd.Parameters.Add("@VAL_DLLS", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.VAL_DLLS == null ? DBNull.Value : lsaaio_pedime.VAL_DLLS;

            // ,@VAL_COME  numeric
            @param = cmd.Parameters.Add("@VAL_COME", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.VAL_COME == null ? DBNull.Value : lsaaio_pedime.VAL_COME;

            // ,@TOT_INCR  numeric
            @param = cmd.Parameters.Add("@TOT_INCR", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.TOT_INCR == null ? DBNull.Value : lsaaio_pedime.TOT_INCR;

            // ,@TOT_DEDU  numeric
            @param = cmd.Parameters.Add("@TOT_DEDU", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.TOT_DEDU == null ? DBNull.Value : lsaaio_pedime.TOT_DEDU;

            // ,@VAL_NORM  numeric
            @param = cmd.Parameters.Add("@VAL_NORM", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.VAL_NORM == null ? DBNull.Value : lsaaio_pedime.VAL_NORM;

            // ,@FAC_AJUS  numeric
            @param = cmd.Parameters.Add("@FAC_AJUS", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.FAC_AJUS == null ? DBNull.Value : lsaaio_pedime.FAC_AJUS;

            // ,@AUT_OBSE  varbinary
            @param = cmd.Parameters.Add("@AUT_OBSE", SqlDbType.VarBinary, -1);
            @param.Value = lsaaio_pedime.AUT_OBSE == null ? DBNull.Value : lsaaio_pedime.AUT_OBSE;

            // ,@CVE_CAPT  varchar
            @param = cmd.Parameters.Add("@CVE_CAPT", SqlDbType.VarChar, 8);
            @param.Value = lsaaio_pedime.CVE_CAPT == null ? DBNull.Value : lsaaio_pedime.CVE_CAPT;

            // ,@FEC_PAGO  datetime
            @param = cmd.Parameters.Add("@FEC_PAGO", SqlDbType.DateTime, 4);
            @param.Value = DBNull.Value;

            // param.Value = If(lsaaio_pedime.FEC_PAGO = "01/01/1900", DBNull.Value, lsaaio_pedime.FEC_PAGO)

            // ,@CVE_REPR  varchar
            @param = cmd.Parameters.Add("@CVE_REPR", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.CVE_REPR == null ? DBNull.Value : lsaaio_pedime.CVE_REPR;

            // ,@TAS_DTA  numeric
            @param = cmd.Parameters.Add("@TAS_DTA", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.TAS_DTA == null ? DBNull.Value : lsaaio_pedime.TAS_DTA;

            // ,@TTA_DTA  char
            @param = cmd.Parameters.Add("@TTA_DTA", SqlDbType.Char, 1);
            @param.Value = lsaaio_pedime.TTA_DTA == null ? DBNull.Value : lsaaio_pedime.TTA_DTA;

            // ,@PAR_2DTA  numeric
            @param = cmd.Parameters.Add("@PAR_2DTA", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.PAR_2DTA == null ? DBNull.Value : lsaaio_pedime.PAR_2DTA;

            // ,@FAC_ACTU  numeric
            @param = cmd.Parameters.Add("@FAC_ACTU", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.FAC_ACTU == null ? DBNull.Value : lsaaio_pedime.FAC_ACTU;

            // ,@NUM_FRAC  int
            @param = cmd.Parameters.Add("@NUM_FRAC", SqlDbType.Int, 4);
            @param.Value = lsaaio_pedime.NUM_FRAC == null ? DBNull.Value : lsaaio_pedime.NUM_FRAC;

            // ,@TOT_EFEC  numeric
            @param = cmd.Parameters.Add("@TOT_EFEC", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.TOT_EFEC == null ? DBNull.Value : lsaaio_pedime.TOT_EFEC;

            // ,@TOT_OTRO  numeric
            @param = cmd.Parameters.Add("@TOT_OTRO", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.TOT_OTRO == null ? DBNull.Value : lsaaio_pedime.TOT_OTRO;

            // ,@FIR_ELEC  varchar
            @param = cmd.Parameters.Add("@FIR_ELEC", SqlDbType.VarChar, 8);
            @param.Value = lsaaio_pedime.FIR_ELEC == null ? DBNull.Value : lsaaio_pedime.FIR_ELEC;

            // ,@NUM_CAND  varchar
            @param = cmd.Parameters.Add("@NUM_CAND", SqlDbType.VarChar, 70);
            @param.Value = lsaaio_pedime.NUM_CAND == null ? DBNull.Value : lsaaio_pedime.NUM_CAND;

            // ,@TOT_VEHI  int
            @param = cmd.Parameters.Add("@TOT_VEHI", SqlDbType.Int, 4);
            @param.Value = lsaaio_pedime.TOT_VEHI == null ? DBNull.Value : lsaaio_pedime.TOT_VEHI;

            // ,@NUM_REFEO  varchar
            @param = cmd.Parameters.Add("@NUM_REFEO", SqlDbType.VarChar, 15);
            @param.Value = lsaaio_pedime.NUM_REFEO == null ? DBNull.Value : lsaaio_pedime.NUM_REFEO;

            // ,@PAT_AGENO  varchar
            @param = cmd.Parameters.Add("@PAT_AGENO", SqlDbType.VarChar, 4);
            @param.Value = lsaaio_pedime.PAT_AGENO == null ? DBNull.Value : lsaaio_pedime.PAT_AGENO;

            // ,@NUM_PEDIO  varchar
            @param = cmd.Parameters.Add("@NUM_PEDIO", SqlDbType.VarChar, 15);
            @param.Value = lsaaio_pedime.NUM_PEDIO == null ? DBNull.Value : lsaaio_pedime.NUM_PEDIO;

            // ,@FEC_PAGOO  datetime
            @param = cmd.Parameters.Add("@FEC_PAGOO", SqlDbType.DateTime, 4);
            @param.Value = DBNull.Value;

            // ,@CVE_PEDIO  varchar
            @param = cmd.Parameters.Add("@CVE_PEDIO", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.CVE_PEDIO == null ? DBNull.Value : lsaaio_pedime.CVE_PEDIO;

            // ,@ADU_DESPO  varchar
            @param = cmd.Parameters.Add("@ADU_DESPO", SqlDbType.VarChar, 3);
            @param.Value = lsaaio_pedime.ADU_DESPO == null ? DBNull.Value : lsaaio_pedime.ADU_DESPO;

            // ,@CVE_BANC  varchar
            @param = cmd.Parameters.Add("@CVE_BANC", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.CVE_BANC == null ? DBNull.Value : lsaaio_pedime.CVE_BANC;

            // ,@NUM_CAJA  varchar
            @param = cmd.Parameters.Add("@NUM_CAJA", SqlDbType.VarChar, 3);
            @param.Value = lsaaio_pedime.NUM_CAJA == null ? DBNull.Value : lsaaio_pedime.NUM_CAJA;

            // ,@DIA_PAGO  datetime
            @param = cmd.Parameters.Add("@DIA_PAGO", SqlDbType.DateTime, 4);
            @param.Value = DBNull.Value;

            // ,@HOR_PAGO  varchar
            @param = cmd.Parameters.Add("@HOR_PAGO", SqlDbType.VarChar, 6);
            @param.Value = lsaaio_pedime.HOR_PAGO == null ? DBNull.Value : lsaaio_pedime.HOR_PAGO;

            // ,@TOT_PAGO  float
            @param = cmd.Parameters.Add("@TOT_PAGO", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.TOT_PAGO == null ? DBNull.Value : lsaaio_pedime.TOT_PAGO;

            // ,@TIP_ACTU  varchar
            @param = cmd.Parameters.Add("@TIP_ACTU", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.TIP_ACTU == null ? DBNull.Value : lsaaio_pedime.TIP_ACTU;

            // ,@CVE_REFIS  varchar
            @param = cmd.Parameters.Add("@CVE_REFIS", SqlDbType.VarChar, 4);
            @param.Value = lsaaio_pedime.CVE_REFIS == null ? DBNull.Value : lsaaio_pedime.CVE_REFIS;

            // ,@CAN_BULT  numeric
            @param = cmd.Parameters.Add("@CAN_BULT", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.CAN_BULT == null ? DBNull.Value : lsaaio_pedime.CAN_BULT;

            // ,@FIR_REME  varchar
            @param = cmd.Parameters.Add("@FIR_REME", SqlDbType.VarChar, 8);
            @param.Value = lsaaio_pedime.FIR_REME == null ? DBNull.Value : lsaaio_pedime.FIR_REME;

            // ,@SEC_DESP  char
            @param = cmd.Parameters.Add("@SEC_DESP", SqlDbType.Char, 1);
            @param.Value = lsaaio_pedime.SEC_DESP == null ? DBNull.Value : lsaaio_pedime.SEC_DESP;

            // ,@ADU_TRAN  varchar
            @param = cmd.Parameters.Add("@ADU_TRAN", SqlDbType.VarChar, 3);
            @param.Value = lsaaio_pedime.ADU_TRAN == null ? DBNull.Value : lsaaio_pedime.ADU_TRAN;

            // ,@COP_REFE  varchar
            @param = cmd.Parameters.Add("@COP_REFE", SqlDbType.VarChar, 15);
            @param.Value = lsaaio_pedime.COP_REFE == null ? DBNull.Value : lsaaio_pedime.COP_REFE;

            // ,@CAN_BULTSUBD  numeric
            @param = cmd.Parameters.Add("@CAN_BULTSUBD", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.CAN_BULTSUBD == null ? DBNull.Value : lsaaio_pedime.CAN_BULTSUBD;

            // ,@FIR_PAGO  varchar
            @param = cmd.Parameters.Add("@FIR_PAGO", SqlDbType.VarChar, 10);
            @param.Value = lsaaio_pedime.FIR_PAGO == null ? DBNull.Value : lsaaio_pedime.FIR_PAGO;

            // ,@NUM_OPER  varchar
            @param = cmd.Parameters.Add("@NUM_OPER", SqlDbType.VarChar, 10);
            @param.Value = lsaaio_pedime.NUM_OPER == null ? DBNull.Value : lsaaio_pedime.NUM_OPER;

            // ,@REG_ENTRADA  varchar
            @param = cmd.Parameters.Add("@REG_ENTRADA", SqlDbType.VarChar, 15);
            @param.Value = lsaaio_pedime.REG_ENTRADA == null ? DBNull.Value : lsaaio_pedime.REG_ENTRADA;

            // ,@CVE_CNTA  varchar
            @param = cmd.Parameters.Add("@CVE_CNTA", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.CVE_CNTA == null ? DBNull.Value : lsaaio_pedime.CVE_CNTA;

            // ,@EMP_FACT  varchar
            @param = cmd.Parameters.Add("@EMP_FACT", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.EMP_FACT == null ? DBNull.Value : lsaaio_pedime.EMP_FACT;

            @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            @param.Direction = ParameterDirection.Output;

            try
            {
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
                throw new Exception(ex.Message.ToString() + "NET_INSERT_SAAIO_PEDIME");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }
        public int ModificarFirmaElectronicaPECE(string numRefe, string firElec, string lineaCA,
                                             string archivo, ref double impuestos, int idUsuario)
        {
            int id = 0;

            using (SqlConnection cn = new SqlConnection(SConexion))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("NET_UPDATE_SAAIO_PEDIME_FIR_ELEC_PECE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@NUM_REFE", SqlDbType.VarChar, 15)
                        {
                            Value = numRefe
                        });

                        cmd.Parameters.Add(new SqlParameter("@FIR_ELEC", SqlDbType.VarChar, 8)
                        {
                            Value = firElec
                        });

                        cmd.Parameters.Add(new SqlParameter("@PAG_LCAP", SqlDbType.VarChar, 30)
                        {
                            Value = lineaCA
                        });

                        cmd.Parameters.Add(new SqlParameter("@ARCH_LCAPT", SqlDbType.VarChar, 30)
                        {
                            Value = archivo
                        });

                        cmd.Parameters.Add(new SqlParameter("@PAG_IMP", SqlDbType.Float)
                        {
                            Value = impuestos
                        });

                        cmd.Parameters.Add(new SqlParameter("@IdUsuario", SqlDbType.Int)
                        {
                            Value = idUsuario
                        });

                        SqlParameter outputParam = new SqlParameter("@newid_registro", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        cn.Open();
                        cmd.ExecuteNonQuery();

                        // Retrieve the output parameter value
                        id = (int)outputParam.Value;
                    }
                }
                catch (Exception ex)
                {
                    id = 0;
                    throw new Exception(ex.Message + " NET_UPDATE_SAAIO_PEDIME_FIR_ELEC_PECE");
                }
            }

            return id;
        }
        public SaaioPedime Buscar(string NUM_PEDI, string PAT_AGEN, string ADU_DESP)
        {
            var objSAAIO_PEDIME = new SaaioPedime();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_SAAIO_PEDIME_PEDIMENTO_NEW";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            @param = cmd.Parameters.Add("@NUM_PEDI", SqlDbType.VarChar, 7);
            @param.Value = NUM_PEDI;

            @param = cmd.Parameters.Add("@PAT_AGEN", SqlDbType.VarChar, 4);
            @param.Value = PAT_AGEN;

            @param = cmd.Parameters.Add("@ADU_DESP", SqlDbType.VarChar, 3);
            @param.Value = ADU_DESP;

            // Insertar Parametro de busqueda

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    var ObjValor = new Helper();

                    objSAAIO_PEDIME.NUM_REFE = dr["NUM_REFE"] == DBNull.Value ? string.Empty : dr["NUM_REFE"].ToString()!;
                    objSAAIO_PEDIME.CVE_IMPO = dr["CVE_IMPO"] == DBNull.Value ? string.Empty : dr["CVE_IMPO"].ToString()!;
                    objSAAIO_PEDIME.IMP_EXPO = dr["IMP_EXPO"].ToString();
                    objSAAIO_PEDIME.TIP_PEDI = dr["TIP_PEDI"].ToString();
                    objSAAIO_PEDIME.ADU_DESP = dr["ADU_DESP"].ToString();
                    objSAAIO_PEDIME.PAT_AGEN = dr["PAT_AGEN"].ToString();
                    objSAAIO_PEDIME.NUM_PEDI = dr["NUM_PEDI"].ToString();
                    objSAAIO_PEDIME.ADU_ENTR = dr["ADU_ENTR"].ToString();
                    objSAAIO_PEDIME.FEC_ENTR = Convert.ToDateTime(dr["FEC_ENTR"]);
                    objSAAIO_PEDIME.TIP_CAMB = Convert.ToDouble(dr["TIP_CAMB"]);
                    objSAAIO_PEDIME.CVE_PEDI = dr["CVE_PEDI"].ToString();
                    objSAAIO_PEDIME.REG_ADUA = dr["REG_ADUA"].ToString();
                    objSAAIO_PEDIME.AUT_REGI = dr["AUT_REGI"].ToString();
                    objSAAIO_PEDIME.CVE_ALMA = dr["CVE_ALMA"].ToString();
                    objSAAIO_PEDIME.FEC_ESPE = Convert.ToDateTime(dr["FEC_ESPE"]);
                    objSAAIO_PEDIME.DES_ORIG = Convert.ToInt32(dr["DES_ORIG"]);
                    objSAAIO_PEDIME.MAR_NUME = dr["MAR_NUME"].ToString();
                    objSAAIO_PEDIME.PES_BRUT = Convert.ToDouble(dr["PES_BRUT"]);
                    objSAAIO_PEDIME.MTR_ENTR = dr["MTR_ENTR"].ToString();
                    objSAAIO_PEDIME.MTR_ARRI = dr["MTR_ARRI"].ToString();
                    objSAAIO_PEDIME.MTR_SALI = dr["MTR_SALI"].ToString();
                    objSAAIO_PEDIME.MON_VASE = Convert.ToDouble(dr["MON_VASE"]);
                    objSAAIO_PEDIME.TIP_MOVA = dr["TIP_MOVA"].ToString();
                    objSAAIO_PEDIME.VAL_DLLS = Convert.ToDouble(dr["VAL_DLLS"]);
                    objSAAIO_PEDIME.VAL_COME = Convert.ToDouble(dr["VAL_COME"]);
                    objSAAIO_PEDIME.TOT_INCR = Convert.ToDouble(dr["TOT_INCR"]);
                    objSAAIO_PEDIME.TOT_DEDU = Convert.ToDouble(dr["TOT_DEDU"]);
                    objSAAIO_PEDIME.VAL_NORM = Convert.ToDouble(dr["VAL_NORM"]);
                    objSAAIO_PEDIME.FAC_AJUS = Convert.ToDouble(dr["FAC_AJUS"]);
                    objSAAIO_PEDIME.CVE_CAPT = dr["CVE_CAPT"].ToString();
                    objSAAIO_PEDIME.FEC_PAGO = Convert.ToDateTime(dr["FEC_PAGO"]);
                    objSAAIO_PEDIME.CVE_REPR = dr["CVE_REPR"].ToString();
                    objSAAIO_PEDIME.TAS_DTA = Convert.ToDouble(dr["TAS_DTA"]);
                    objSAAIO_PEDIME.TTA_DTA = dr["TTA_DTA"].ToString();
                    objSAAIO_PEDIME.PAR_2DTA = Convert.ToDouble(dr["PAR_2DTA"]);
                    objSAAIO_PEDIME.FAC_ACTU = Convert.ToDouble(dr["FAC_ACTU"]);
                    objSAAIO_PEDIME.NUM_FRAC = Convert.ToInt32(dr["NUM_FRAC"]);
                    objSAAIO_PEDIME.TOT_EFEC = Convert.ToDouble(dr["TOT_EFEC"]);
                    objSAAIO_PEDIME.TOT_OTRO = Convert.ToDouble(dr["TOT_OTRO"]);
                    objSAAIO_PEDIME.FIR_ELEC = dr["FIR_ELEC"].ToString();
                    objSAAIO_PEDIME.NUM_CAND = dr["NUM_CAND"].ToString();
                    objSAAIO_PEDIME.TOT_VEHI = Convert.ToInt32(dr["TOT_VEHI"]);
                    objSAAIO_PEDIME.NUM_REFEO = dr["NUM_REFEO"].ToString();
                    objSAAIO_PEDIME.PAT_AGENO = dr["PAT_AGENO"].ToString();
                    objSAAIO_PEDIME.NUM_PEDIO = dr["NUM_PEDIO"].ToString();
                    objSAAIO_PEDIME.FEC_PAGOO = Convert.ToDateTime(dr["FEC_PAGOO"]);
                    objSAAIO_PEDIME.CVE_PEDIO = dr["CVE_PEDIO"].ToString();
                    objSAAIO_PEDIME.ADU_DESPO = dr["ADU_DESPO"].ToString();
                    objSAAIO_PEDIME.CVE_BANC = dr["CVE_BANC"].ToString();
                    objSAAIO_PEDIME.NUM_CAJA = dr["NUM_CAJA"].ToString();
                    objSAAIO_PEDIME.DIA_PAGO = Convert.ToDateTime(dr["DIA_PAGO"]);
                    objSAAIO_PEDIME.HOR_PAGO = dr["HOR_PAGO"].ToString();
                    objSAAIO_PEDIME.TOT_PAGO = Convert.ToDouble(dr["TOT_PAGO"]);
                    objSAAIO_PEDIME.TIP_ACTU = dr["TIP_ACTU"].ToString();
                    objSAAIO_PEDIME.CVE_REFIS = dr["CVE_REFIS"].ToString();
                    objSAAIO_PEDIME.CAN_BULT = Convert.ToDouble(dr["CAN_BULT"]);
                    objSAAIO_PEDIME.FIR_REME = dr["FIR_REME"].ToString();
                    objSAAIO_PEDIME.SEC_DESP = dr["SEC_DESP"].ToString();
                    objSAAIO_PEDIME.ADU_TRAN = dr["ADU_TRAN"].ToString();
                    objSAAIO_PEDIME.COP_REFE = dr["COP_REFE"].ToString();
                    objSAAIO_PEDIME.CAN_BULTSUBD = Convert.ToDouble(dr["CAN_BULTSUBD"]);
                    objSAAIO_PEDIME.FIR_PAGO = dr["FIR_PAGO"].ToString();
                    objSAAIO_PEDIME.NUM_OPER = dr["NUM_OPER"].ToString();
                    objSAAIO_PEDIME.REG_ENTRADA = dr["REG_ENTRADA"].ToString();
                    objSAAIO_PEDIME.CVE_CNTA = dr["CVE_CNTA"].ToString();
                    objSAAIO_PEDIME.EMP_FACT = dr["EMP_FACT"].ToString();
                }
                else
                {
                    objSAAIO_PEDIME = default;
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

            return objSAAIO_PEDIME;
        }
        public int Modificar(SaaioPedime lsaaio_pedime)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            string MySql;


            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_SAAIO_PEDIME";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            MySql = "NET_UPDATE_SAAIO_PEDIME ";

            // ,@NUM_REFE  varchar
            @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
            @param.Value = lsaaio_pedime.NUM_REFE;
            MySql = MySql + "'" + lsaaio_pedime.NUM_REFE + "','";

            // ,@CVE_IMPO  varchar
            @param = cmd.Parameters.Add("@CVE_IMPO", SqlDbType.VarChar, 6);
            @param.Value = lsaaio_pedime.CVE_IMPO;
            MySql = MySql + lsaaio_pedime.CVE_IMPO + "','";

            // ,@IMP_EXPO  char
            @param = cmd.Parameters.Add("@IMP_EXPO", SqlDbType.Char, 1);
            @param.Value = lsaaio_pedime.IMP_EXPO;
            MySql = MySql + lsaaio_pedime.IMP_EXPO + "','";

            // ,@TIP_PEDI  varchar
            @param = cmd.Parameters.Add("@TIP_PEDI", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.TIP_PEDI == null ? DBNull.Value : lsaaio_pedime.TIP_PEDI;
            MySql = MySql + (lsaaio_pedime.TIP_PEDI == null ? DBNull.Value : lsaaio_pedime.TIP_PEDI) + "','";


            // ,@ADU_DESP  varchar
            @param = cmd.Parameters.Add("@ADU_DESP", SqlDbType.VarChar, 3);
            @param.Value = lsaaio_pedime.ADU_DESP == null ? DBNull.Value : lsaaio_pedime.ADU_DESP;
            MySql = MySql + (lsaaio_pedime.ADU_DESP == null ? DBNull.Value : lsaaio_pedime.ADU_DESP) + "','";

            // ,@PAT_AGEN  varchar
            @param = cmd.Parameters.Add("@PAT_AGEN", SqlDbType.VarChar, 4);
            @param.Value = lsaaio_pedime.PAT_AGEN == null ? DBNull.Value : lsaaio_pedime.PAT_AGEN;
            MySql = MySql + (lsaaio_pedime.PAT_AGEN == null ? DBNull.Value : lsaaio_pedime.PAT_AGEN) + "','";

            // ,@NUM_PEDI  varchar
            @param = cmd.Parameters.Add("@NUM_PEDI", SqlDbType.VarChar, 30);
            @param.Value = lsaaio_pedime.NUM_PEDI == null ? DBNull.Value : lsaaio_pedime.NUM_PEDI;
            MySql = MySql + (lsaaio_pedime.NUM_PEDI == null ? DBNull.Value : lsaaio_pedime.NUM_PEDI) + "','";

            // ,@ADU_ENTR  varchar
            @param = cmd.Parameters.Add("@ADU_ENTR", SqlDbType.VarChar, 3);
            @param.Value = lsaaio_pedime.ADU_ENTR == null ? DBNull.Value : lsaaio_pedime.ADU_ENTR;
            MySql = MySql + (lsaaio_pedime.ADU_ENTR == null ? DBNull.Value : lsaaio_pedime.ADU_ENTR) + "','";

            // ,@FEC_ENTR  datetime
            @param = cmd.Parameters.Add("@FEC_ENTR", SqlDbType.DateTime, 4);
            @param.Value = lsaaio_pedime.FEC_ENTR; // DBNull.Value '
            MySql = MySql + lsaaio_pedime.FEC_ENTR + "','";


            // ,@TIP_CAMB  float
            @param = cmd.Parameters.Add("@TIP_CAMB", SqlDbType.Float);
            @param.Value = lsaaio_pedime.TIP_CAMB;
            MySql = MySql + lsaaio_pedime.TIP_CAMB + "','";

            // ,@CVE_PEDI  varchar
            @param = cmd.Parameters.Add("@CVE_PEDI", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.CVE_PEDI == null ? DBNull.Value : lsaaio_pedime.CVE_PEDI;
            MySql = MySql + (lsaaio_pedime.CVE_PEDI == null ? DBNull.Value : lsaaio_pedime.CVE_PEDI) + "','";


            // ,@REG_ADUA  varchar
            @param = cmd.Parameters.Add("@REG_ADUA", SqlDbType.VarChar, 3);
            @param.Value = lsaaio_pedime.REG_ADUA == null ? DBNull.Value : lsaaio_pedime.REG_ADUA;
            MySql = MySql + (lsaaio_pedime.REG_ADUA == null ? DBNull.Value : lsaaio_pedime.REG_ADUA) + "','";


            // ,@AUT_REGI  varchar
            @param = cmd.Parameters.Add("@AUT_REGI", SqlDbType.VarChar, 20);
            @param.Value = lsaaio_pedime.AUT_REGI == null ? DBNull.Value : lsaaio_pedime.AUT_REGI;
            MySql = MySql + (lsaaio_pedime.AUT_REGI == null ? DBNull.Value : lsaaio_pedime.AUT_REGI) + "','";

            // ,@CVE_ALMA  varchar
            @param = cmd.Parameters.Add("@CVE_ALMA", SqlDbType.VarChar, 4);
            @param.Value = lsaaio_pedime.CVE_ALMA == null ? DBNull.Value : lsaaio_pedime.CVE_ALMA;
            MySql = MySql + (lsaaio_pedime.CVE_ALMA == null ? DBNull.Value : lsaaio_pedime.CVE_ALMA) + "','";


            // ,@FEC_ESPE  datetime
            @param = cmd.Parameters.Add("@FEC_ESPE", SqlDbType.DateTime, 4);
            @param.Value = DBNull.Value;
            MySql = MySql + DBNull.Value + "','";


            // ,@DES_ORIG  int
            @param = cmd.Parameters.Add("@DES_ORIG", SqlDbType.Int, 4);
            @param.Value = lsaaio_pedime.DES_ORIG;
            MySql = MySql + lsaaio_pedime.DES_ORIG + "','";

            // ,@MAR_NUME  varchar
            @param = cmd.Parameters.Add("@MAR_NUME", SqlDbType.VarChar, 150);
            @param.Value = lsaaio_pedime.MAR_NUME == null ? DBNull.Value : lsaaio_pedime.MAR_NUME;
            MySql = MySql + (lsaaio_pedime.MAR_NUME == null ? DBNull.Value : lsaaio_pedime.MAR_NUME) + "','";

            // ,@PES_BRUT  float
            @param = cmd.Parameters.Add("@PES_BRUT", SqlDbType.Float);
            @param.Value = lsaaio_pedime.PES_BRUT;
            MySql = MySql + lsaaio_pedime.PES_BRUT + "','";


            // ,@MTR_ENTR  varchar
            @param = cmd.Parameters.Add("@MTR_ENTR", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.MTR_ENTR == null ? DBNull.Value : lsaaio_pedime.MTR_ENTR;
            MySql = MySql + (lsaaio_pedime.MTR_ENTR == null ? DBNull.Value : lsaaio_pedime.MTR_ENTR) + "','";

            // ,@MTR_ARRI  varchar
            @param = cmd.Parameters.Add("@MTR_ARRI", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.MTR_ARRI == null ? DBNull.Value : lsaaio_pedime.MTR_ARRI;
            MySql = MySql + (lsaaio_pedime.MTR_ARRI == null ? DBNull.Value : lsaaio_pedime.MTR_ARRI) + "','";

            // ,@MTR_SALI  varchar
            @param = cmd.Parameters.Add("@MTR_SALI", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.MTR_SALI == null ? DBNull.Value : lsaaio_pedime.MTR_SALI;
            MySql = MySql + (lsaaio_pedime.MTR_SALI == null ? DBNull.Value : lsaaio_pedime.MTR_SALI) + "','";


            // ,@MON_VASE  float
            @param = cmd.Parameters.Add("@MON_VASE", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.MON_VASE;
            MySql = MySql + lsaaio_pedime.MON_VASE + "','";

            // ,@TIP_MOVA  varchar
            @param = cmd.Parameters.Add("@TIP_MOVA", SqlDbType.VarChar, 3);
            @param.Value = lsaaio_pedime.TIP_MOVA == null ? DBNull.Value : lsaaio_pedime.TIP_MOVA;
            MySql = MySql + (lsaaio_pedime.TIP_MOVA == null ? DBNull.Value : lsaaio_pedime.TIP_MOVA) + "','";

            // ,@VAL_DLLS  float
            @param = cmd.Parameters.Add("@VAL_DLLS", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.VAL_DLLS;
            MySql = MySql + lsaaio_pedime.VAL_DLLS + "','";


            // ,@VAL_COME  float
            @param = cmd.Parameters.Add("@VAL_COME", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.VAL_COME;
            MySql = MySql + lsaaio_pedime.VAL_COME + "','";


            // ,@TOT_INCR  float
            @param = cmd.Parameters.Add("@TOT_INCR", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.TOT_INCR;
            MySql = MySql + lsaaio_pedime.TOT_INCR + "','";

            // ,@TOT_DEDU  float
            @param = cmd.Parameters.Add("@TOT_DEDU", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.TOT_DEDU;
            MySql = MySql + lsaaio_pedime.TOT_DEDU + "','";

            // ,@VAL_NORM  float
            @param = cmd.Parameters.Add("@VAL_NORM", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.VAL_NORM;
            MySql = MySql + lsaaio_pedime.VAL_NORM + "','";

            // ,@FAC_AJUS  float
            @param = cmd.Parameters.Add("@FAC_AJUS", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.FAC_AJUS;
            MySql = MySql + lsaaio_pedime.FAC_AJUS + "','";

            // ',@AUT_OBSE  ntext
            // param = cmd.Parameters.Add("@AUT_OBSE", SqlDbType.NText, 1073741823)
            // param.Value = If(IsNothing(lsaaio_pedime.AUT_OBSE), DBNull.Value, lsaaio_pedime.AUT_OBSE)


            // ,@CVE_CAPT  varchar
            @param = cmd.Parameters.Add("@CVE_CAPT", SqlDbType.VarChar, 8);
            @param.Value = lsaaio_pedime.CVE_CAPT == null ? DBNull.Value : lsaaio_pedime.CVE_CAPT;
            MySql = MySql + (lsaaio_pedime.CVE_CAPT == null ? DBNull.Value : lsaaio_pedime.CVE_CAPT) + "','";

            // ,@FEC_PAGO  datetime
            @param = cmd.Parameters.Add("@FEC_PAGO", SqlDbType.DateTime, 4);
            @param.Value = DBNull.Value; // If(IsNothing(lsaaio_pedime.FEC_PAGO), DBNull.Value, lsaaio_pedime.FEC_PAGO)
            MySql = MySql + DBNull.Value + "','";

            // DBNull.Value

            // ,@CVE_REPR  varchar
            @param = cmd.Parameters.Add("@CVE_REPR", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.CVE_REPR == null ? DBNull.Value : lsaaio_pedime.CVE_REPR;
            MySql = MySql + (lsaaio_pedime.CVE_REPR == null ? DBNull.Value : lsaaio_pedime.CVE_REPR) + "','";

            // ,@TAS_DTA  float
            @param = cmd.Parameters.Add("@TAS_DTA", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.TAS_DTA;
            MySql = MySql + lsaaio_pedime.TAS_DTA + "','";

            // ,@TTA_DTA  char
            @param = cmd.Parameters.Add("@TTA_DTA", SqlDbType.Char, 1);
            @param.Value = lsaaio_pedime.TTA_DTA == null ? DBNull.Value : lsaaio_pedime.TTA_DTA;
            MySql = MySql + (lsaaio_pedime.TTA_DTA == null ? DBNull.Value : lsaaio_pedime.TTA_DTA) + "','";


            // ,@PAR_2DTA  float
            @param = cmd.Parameters.Add("@PAR_2DTA", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.PAR_2DTA;
            MySql = MySql + lsaaio_pedime.PAR_2DTA + "','";


            // ,@FAC_ACTU  float
            @param = cmd.Parameters.Add("@FAC_ACTU", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.FAC_ACTU;
            MySql = MySql + lsaaio_pedime.FAC_ACTU + "','";


            // ,@NUM_FRAC  int
            @param = cmd.Parameters.Add("@NUM_FRAC", SqlDbType.Int, 4);
            @param.Value = lsaaio_pedime.NUM_FRAC;
            MySql = MySql + lsaaio_pedime.NUM_FRAC + "','";

            // ,@TOT_EFEC  float
            @param = cmd.Parameters.Add("@TOT_EFEC", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.TOT_EFEC;
            MySql = MySql + lsaaio_pedime.TOT_EFEC + "','";


            // ,@TOT_OTRO  float
            @param = cmd.Parameters.Add("@TOT_OTRO", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.TOT_OTRO;
            MySql = MySql + lsaaio_pedime.TOT_OTRO + "','";


            // ,@FIR_ELEC  varchar
            @param = cmd.Parameters.Add("@FIR_ELEC", SqlDbType.VarChar, 8);
            @param.Value = lsaaio_pedime.FIR_ELEC == null ? DBNull.Value : lsaaio_pedime.FIR_ELEC;
            MySql = MySql + (lsaaio_pedime.FIR_ELEC == null ? DBNull.Value : lsaaio_pedime.FIR_ELEC) + "','";

            // ,@NUM_CAND  varchar
            @param = cmd.Parameters.Add("@NUM_CAND", SqlDbType.VarChar, 70);
            @param.Value = lsaaio_pedime.NUM_CAND == null ? DBNull.Value : lsaaio_pedime.NUM_CAND;
            MySql = MySql + (lsaaio_pedime.NUM_CAND == null ? DBNull.Value : lsaaio_pedime.NUM_CAND) + "','";


            // ,@TOT_VEHI  int
            @param = cmd.Parameters.Add("@TOT_VEHI", SqlDbType.Int, 4);
            @param.Value = lsaaio_pedime.TOT_VEHI;
            MySql = MySql + lsaaio_pedime.TOT_VEHI + "','";


            // ,@NUM_REFEO  varchar
            @param = cmd.Parameters.Add("@NUM_REFEO", SqlDbType.VarChar, 15);
            @param.Value = lsaaio_pedime.NUM_REFEO == null ? DBNull.Value : lsaaio_pedime.NUM_REFEO;
            MySql = MySql + (lsaaio_pedime.NUM_REFEO == null ? DBNull.Value : lsaaio_pedime.NUM_REFEO) + "','";


            // ,@PAT_AGENO  varchar
            @param = cmd.Parameters.Add("@PAT_AGENO", SqlDbType.VarChar, 4);
            @param.Value = lsaaio_pedime.PAT_AGENO == null ? DBNull.Value : lsaaio_pedime.PAT_AGENO;
            MySql = MySql + (lsaaio_pedime.PAT_AGENO == null ? DBNull.Value : lsaaio_pedime.PAT_AGENO) + "','";


            // ,@NUM_PEDIO  varchar
            @param = cmd.Parameters.Add("@NUM_PEDIO", SqlDbType.VarChar, 15);
            @param.Value = lsaaio_pedime.NUM_PEDIO == null ? DBNull.Value : lsaaio_pedime.NUM_PEDIO;
            MySql = MySql + (lsaaio_pedime.NUM_PEDIO == null ? DBNull.Value : lsaaio_pedime.NUM_PEDIO) + "','";

            // ,@FEC_PAGOO  datetime
            @param = cmd.Parameters.Add("@FEC_PAGOO", SqlDbType.DateTime, 4);
            @param.Value = lsaaio_pedime.FEC_PAGOO == null ? DBNull.Value : lsaaio_pedime.FEC_PAGOO;
            MySql = MySql + DBNull.Value + "','";


            // ,@CVE_PEDIO  varchar
            @param = cmd.Parameters.Add("@CVE_PEDIO", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.CVE_PEDIO == null ? DBNull.Value : lsaaio_pedime.CVE_PEDIO;
            MySql = MySql + (lsaaio_pedime.CVE_PEDIO == null ? DBNull.Value : lsaaio_pedime.CVE_PEDIO) + "','";


            // ,@ADU_DESPO  varchar
            @param = cmd.Parameters.Add("@ADU_DESPO", SqlDbType.VarChar, 3);
            @param.Value = lsaaio_pedime.ADU_DESPO == null ? DBNull.Value : lsaaio_pedime.ADU_DESPO;
            MySql = MySql + (lsaaio_pedime.ADU_DESPO == null ? DBNull.Value : lsaaio_pedime.ADU_DESPO) + "','";


            // ,@CVE_BANC  varchar
            @param = cmd.Parameters.Add("@CVE_BANC", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.CVE_BANC == null ? DBNull.Value : lsaaio_pedime.CVE_BANC;
            MySql = MySql + (lsaaio_pedime.CVE_BANC == null ? DBNull.Value : lsaaio_pedime.CVE_BANC) + "','";

            // ,@NUM_CAJA  varchar
            @param = cmd.Parameters.Add("@NUM_CAJA", SqlDbType.VarChar, 3);
            @param.Value = lsaaio_pedime.NUM_CAJA == null ? DBNull.Value : lsaaio_pedime.NUM_CAJA;
            MySql = MySql + (lsaaio_pedime.NUM_CAJA == null ? DBNull.Value : lsaaio_pedime.NUM_CAJA) + "','";

            // ,@DIA_PAGO  datetime
            @param = cmd.Parameters.Add("@DIA_PAGO", SqlDbType.DateTime, 4);
            @param.Value = DBNull.Value;
            MySql = MySql + DBNull.Value + "','";


            // ,@HOR_PAGO  varchar
            @param = cmd.Parameters.Add("@HOR_PAGO", SqlDbType.VarChar, 6);
            @param.Value = lsaaio_pedime.HOR_PAGO == null ? DBNull.Value : lsaaio_pedime.HOR_PAGO;
            MySql = MySql + (lsaaio_pedime.HOR_PAGO == null ? DBNull.Value : lsaaio_pedime.HOR_PAGO) + "','";



            // ,@TOT_PAGO  float
            @param = cmd.Parameters.Add("@TOT_PAGO", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.TOT_PAGO == null ? DBNull.Value : lsaaio_pedime.TOT_PAGO;
            MySql = MySql + (lsaaio_pedime.TOT_PAGO == null ? DBNull.Value : lsaaio_pedime.TOT_PAGO) + "','";

            // lsaaio_pedime.TOT_PAGO

            // ,@TIP_ACTU  varchar
            @param = cmd.Parameters.Add("@TIP_ACTU", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.TIP_ACTU == null ? DBNull.Value : lsaaio_pedime.TIP_ACTU;
            MySql = MySql + (lsaaio_pedime.TIP_ACTU == null ? DBNull.Value : lsaaio_pedime.TIP_ACTU) + "','";



            // ,@CVE_REFIS  varchar
            @param = cmd.Parameters.Add("@CVE_REFIS", SqlDbType.VarChar, 4);
            @param.Value = lsaaio_pedime.CVE_REFIS;
            @param.Value = lsaaio_pedime.CVE_REFIS == null ? DBNull.Value : lsaaio_pedime.CVE_REFIS;
            MySql = MySql + (lsaaio_pedime.CVE_REFIS == null ? DBNull.Value : lsaaio_pedime.CVE_REFIS) + "','";



            // ,@CAN_BULT  float
            @param = cmd.Parameters.Add("@CAN_BULT", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.CAN_BULT;
            MySql = MySql + lsaaio_pedime.CAN_BULT + "','";

            // ,@FIR_REME  varchar
            @param = cmd.Parameters.Add("@FIR_REME", SqlDbType.VarChar, 8);
            @param.Value = lsaaio_pedime.FIR_REME == null ? DBNull.Value : lsaaio_pedime.FIR_REME;
            MySql = MySql + (lsaaio_pedime.FIR_REME == null ? DBNull.Value : lsaaio_pedime.FIR_REME) + "','";


            // ,@SEC_DESP  char
            @param = cmd.Parameters.Add("@SEC_DESP", SqlDbType.Char, 1);
            @param.Value = lsaaio_pedime.SEC_DESP == null ? DBNull.Value : lsaaio_pedime.SEC_DESP;
            MySql = MySql + (lsaaio_pedime.SEC_DESP == null ? DBNull.Value : lsaaio_pedime.SEC_DESP) + "','";



            // ,@ADU_TRAN  varchar
            @param = cmd.Parameters.Add("@ADU_TRAN", SqlDbType.VarChar, 3);
            @param.Value = lsaaio_pedime.ADU_TRAN == null ? DBNull.Value : lsaaio_pedime.ADU_TRAN;
            MySql = MySql + (lsaaio_pedime.ADU_TRAN == null ? DBNull.Value : lsaaio_pedime.ADU_TRAN) + "','";

            // ,@COP_REFE  varchar

            @param = cmd.Parameters.Add("@COP_REFE", SqlDbType.VarChar, 15);
            @param.Value = lsaaio_pedime.COP_REFE == null ? DBNull.Value : lsaaio_pedime.COP_REFE;
            MySql = MySql + (lsaaio_pedime.COP_REFE == null ? DBNull.Value : lsaaio_pedime.COP_REFE) + "','";



            // ,@CAN_BULTSUBD  float
            @param = cmd.Parameters.Add("@CAN_BULTSUBD", SqlDbType.Float, 4);
            @param.Value = lsaaio_pedime.CAN_BULTSUBD;
            MySql = MySql + lsaaio_pedime.CAN_BULTSUBD + "','";


            // ,@FIR_PAGO  varchar
            @param = cmd.Parameters.Add("@FIR_PAGO", SqlDbType.VarChar, 10);
            @param.Value = lsaaio_pedime.FIR_PAGO == null ? DBNull.Value : lsaaio_pedime.FIR_PAGO;
            MySql = MySql + (lsaaio_pedime.FIR_PAGO == null ? DBNull.Value : lsaaio_pedime.FIR_PAGO) + "','";

            // ,@NUM_OPER  varchar
            @param = cmd.Parameters.Add("@NUM_OPER", SqlDbType.VarChar, 10);
            @param.Value = lsaaio_pedime.NUM_OPER == null ? DBNull.Value : lsaaio_pedime.NUM_OPER;
            MySql = MySql + (lsaaio_pedime.NUM_OPER == null ? DBNull.Value : lsaaio_pedime.NUM_OPER) + "','";


            // ,@REG_ENTRADA  varchar
            @param = cmd.Parameters.Add("@REG_ENTRADA", SqlDbType.VarChar, 15);
            @param.Value = lsaaio_pedime.REG_ENTRADA == null ? DBNull.Value : lsaaio_pedime.REG_ENTRADA;
            MySql = MySql + (lsaaio_pedime.REG_ENTRADA == null ? DBNull.Value : lsaaio_pedime.REG_ENTRADA) + "','";


            // ,@CVE_CNTA  varchar
            @param = cmd.Parameters.Add("@CVE_CNTA", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.CVE_CNTA == null ? DBNull.Value : lsaaio_pedime.CVE_CNTA;
            MySql = MySql + (lsaaio_pedime.REG_ENTRADA == null ? DBNull.Value : lsaaio_pedime.REG_ENTRADA) + "','";


            // ,@EMP_FACT  varchar
            @param = cmd.Parameters.Add("@EMP_FACT", SqlDbType.VarChar, 2);
            @param.Value = lsaaio_pedime.EMP_FACT == null ? DBNull.Value : lsaaio_pedime.EMP_FACT;
            MySql = MySql + (lsaaio_pedime.EMP_FACT == null ? DBNull.Value : lsaaio_pedime.EMP_FACT) + "','";


            @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            @param.Direction = ParameterDirection.Output;
            MySql = MySql + ParameterDirection.Output + "'";


            try
            {
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
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_SAAIO_PEDIME");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }

        public int NETSABERSIEXISTEFIRELEC(string MyNum_Refe)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;

            try
            {


                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_SABER_SI_EXISTE_FIR_ELEC";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                // @NUM_REFE VARCHAR(15)
                param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                param.Value = MyNum_Refe;

                param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                param.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NETSABERSIEXISTEFIRELEC");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;

        }
        public int ModificarFirmaElectronica(string numRefe, string firElec, int idUsuario)
        {
            int id;
            using (SqlConnection cn = new SqlConnection(SConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_UPDATE_SAAIO_PEDIME_FIR_ELEC", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmd.Parameters.Add(new SqlParameter("@NUM_REFE", SqlDbType.VarChar, 15)).Value = numRefe;
                    cmd.Parameters.Add(new SqlParameter("@FIR_ELEC", SqlDbType.VarChar, 8)).Value = firElec;
                    cmd.Parameters.Add(new SqlParameter("@IdUsuario", SqlDbType.Int, 4)).Value = idUsuario;
                    SqlParameter outputParam = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                    outputParam.Direction = ParameterDirection.Output;

                    try
                    {
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        id = (int)cmd.Parameters["@newid_registro"].Value;
                        cmd.Parameters.Clear();
                    }
                    catch (Exception ex)
                    {
                        id = 0;
                        throw new Exception(ex.Message + "NET_UPDATE_SAAIO_PEDIME_FIR_ELEC");
                    }
                }
            }
            return id;
        }

        public int ModificarNEW(SaaioPedime lsaaio_pedime)
        {
            int id = 0;
            using (SqlConnection cn = new SqlConnection(SConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_UPDATE_SAAIO_PEDIME_NEW", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        cn.Open();

                        cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = lsaaio_pedime.NUM_REFE;
                        cmd.Parameters.Add("@CVE_IMPO", SqlDbType.VarChar, 6).Value = lsaaio_pedime.CVE_IMPO;
                        cmd.Parameters.Add("@IMP_EXPO", SqlDbType.Char, 1).Value = lsaaio_pedime.IMP_EXPO;
                        cmd.Parameters.Add("@TIP_PEDI", SqlDbType.VarChar, 2).Value = (object?)lsaaio_pedime.TIP_PEDI ?? DBNull.Value;
                        cmd.Parameters.Add("@ADU_DESP", SqlDbType.VarChar, 3).Value = (object?)lsaaio_pedime.ADU_DESP ?? DBNull.Value;
                        cmd.Parameters.Add("@PAT_AGEN", SqlDbType.VarChar, 4).Value = (object?)lsaaio_pedime.PAT_AGEN ?? DBNull.Value;
                        cmd.Parameters.Add("@NUM_PEDI", SqlDbType.VarChar, 30).Value = (object?)lsaaio_pedime.NUM_PEDI ?? DBNull.Value;
                        cmd.Parameters.Add("@ADU_ENTR", SqlDbType.VarChar, 3).Value = (object?)lsaaio_pedime.ADU_ENTR ?? DBNull.Value;
                        cmd.Parameters.Add("@FEC_ENTR", SqlDbType.DateTime).Value = lsaaio_pedime.FEC_ENTR;
                        cmd.Parameters.Add("@TIP_CAMB", SqlDbType.Float).Value = lsaaio_pedime.TIP_CAMB;
                        cmd.Parameters.Add("@CVE_PEDI", SqlDbType.VarChar, 2).Value = (object?)lsaaio_pedime.CVE_PEDI ?? DBNull.Value;
                        cmd.Parameters.Add("@REG_ADUA", SqlDbType.VarChar, 3).Value = (object?)lsaaio_pedime.REG_ADUA ?? DBNull.Value;
                        cmd.Parameters.Add("@AUT_REGI", SqlDbType.VarChar, 20).Value = (object?)lsaaio_pedime.AUT_REGI ?? DBNull.Value;
                        cmd.Parameters.Add("@CVE_ALMA", SqlDbType.VarChar, 4).Value = (object?)lsaaio_pedime.CVE_ALMA ?? DBNull.Value;
                        cmd.Parameters.Add("@FEC_ESPE", SqlDbType.DateTime).Value = (object?)lsaaio_pedime.FEC_ESPE ?? DBNull.Value;
                        cmd.Parameters.Add("@DES_ORIG", SqlDbType.Int).Value = lsaaio_pedime.DES_ORIG;
                        cmd.Parameters.Add("@MAR_NUME", SqlDbType.VarChar, 150).Value = (object?)lsaaio_pedime.MAR_NUME ?? DBNull.Value;
                        cmd.Parameters.Add("@PES_BRUT", SqlDbType.Float).Value = lsaaio_pedime.PES_BRUT;
                        cmd.Parameters.Add("@MTR_ENTR", SqlDbType.VarChar, 2).Value = (object?)lsaaio_pedime.MTR_ENTR ?? DBNull.Value;
                        cmd.Parameters.Add("@MTR_ARRI", SqlDbType.VarChar, 2).Value = (object?)lsaaio_pedime.MTR_ARRI ?? DBNull.Value;
                        cmd.Parameters.Add("@MTR_SALI", SqlDbType.VarChar, 2).Value = (object?)lsaaio_pedime.MTR_SALI ?? DBNull.Value;
                        cmd.Parameters.Add("@MON_VASE", SqlDbType.Float).Value = lsaaio_pedime.MON_VASE;
                        cmd.Parameters.Add("@TIP_MOVA", SqlDbType.VarChar, 3).Value = (object?)lsaaio_pedime.TIP_MOVA ?? DBNull.Value;
                        cmd.Parameters.Add("@VAL_DLLS", SqlDbType.Float).Value = lsaaio_pedime.VAL_DLLS;
                        cmd.Parameters.Add("@VAL_COME", SqlDbType.Float).Value = lsaaio_pedime.VAL_COME;
                        cmd.Parameters.Add("@TOT_INCR", SqlDbType.Float).Value = lsaaio_pedime.TOT_INCR;
                        cmd.Parameters.Add("@TOT_DEDU", SqlDbType.Float).Value = lsaaio_pedime.TOT_DEDU;
                        cmd.Parameters.Add("@VAL_NORM", SqlDbType.Float).Value = lsaaio_pedime.VAL_NORM;
                        cmd.Parameters.Add("@FAC_AJUS", SqlDbType.Float).Value = lsaaio_pedime.FAC_AJUS;
                        cmd.Parameters.Add("@AUT_OBSE", SqlDbType.NText).Value = (object?)lsaaio_pedime.AUT_OBSE ?? DBNull.Value;
                        cmd.Parameters.Add("@CVE_CAPT", SqlDbType.VarChar, 8).Value = (object?)lsaaio_pedime.CVE_CAPT ?? DBNull.Value;
                        cmd.Parameters.Add("@FEC_PAGO", SqlDbType.DateTime).Value = (object?)lsaaio_pedime.FEC_PAGO ?? DBNull.Value;
                        cmd.Parameters.Add("@CVE_REPR", SqlDbType.VarChar, 2).Value = (object?)lsaaio_pedime.CVE_REPR ?? DBNull.Value;
                        cmd.Parameters.Add("@TAS_DTA", SqlDbType.Float).Value = lsaaio_pedime.TAS_DTA;
                        cmd.Parameters.Add("@TTA_DTA", SqlDbType.Char, 1).Value = (object?)lsaaio_pedime.TTA_DTA ?? DBNull.Value;
                        cmd.Parameters.Add("@PAR_2DTA", SqlDbType.Float).Value = lsaaio_pedime.PAR_2DTA;
                        cmd.Parameters.Add("@FAC_ACTU", SqlDbType.Float).Value = lsaaio_pedime.FAC_ACTU;
                        cmd.Parameters.Add("@NUM_FRAC", SqlDbType.Int).Value = lsaaio_pedime.NUM_FRAC;
                        cmd.Parameters.Add("@TOT_EFEC", SqlDbType.Float).Value = lsaaio_pedime.TOT_EFEC;
                        cmd.Parameters.Add("@TOT_OTRO", SqlDbType.Float).Value = lsaaio_pedime.TOT_OTRO;
                        cmd.Parameters.Add("@FIR_ELEC", SqlDbType.VarChar, 8).Value = (object?)lsaaio_pedime.FIR_ELEC ?? DBNull.Value;
                        cmd.Parameters.Add("@NUM_CAND", SqlDbType.VarChar, 70).Value = (object?)lsaaio_pedime.NUM_CAND ?? DBNull.Value;
                        cmd.Parameters.Add("@TOT_VEHI", SqlDbType.Int).Value = lsaaio_pedime.TOT_VEHI;
                        cmd.Parameters.Add("@NUM_REFEO", SqlDbType.VarChar, 15).Value = (object?)lsaaio_pedime.NUM_REFEO ?? DBNull.Value;
                        cmd.Parameters.Add("@PAT_AGENO", SqlDbType.VarChar, 4).Value = (object?)lsaaio_pedime.PAT_AGENO ?? DBNull.Value;
                        cmd.Parameters.Add("@NUM_PEDIO", SqlDbType.VarChar, 15).Value = (object?)lsaaio_pedime.NUM_PEDIO ?? DBNull.Value;
                        cmd.Parameters.Add("@FEC_PAGOO", SqlDbType.DateTime).Value = (object?)lsaaio_pedime.FEC_PAGOO ?? DBNull.Value;
                        cmd.Parameters.Add("@CVE_PEDIO", SqlDbType.VarChar, 2).Value = (object?)lsaaio_pedime.CVE_PEDIO ?? DBNull.Value;
                        cmd.Parameters.Add("@ADU_DESPO", SqlDbType.VarChar, 3).Value = (object?)lsaaio_pedime.ADU_DESPO ?? DBNull.Value;
                        cmd.Parameters.Add("@CVE_BANC", SqlDbType.VarChar, 2).Value = (object?)lsaaio_pedime.CVE_BANC ?? DBNull.Value;
                        cmd.Parameters.Add("@NUM_CAJA", SqlDbType.VarChar, 3).Value = (object?)lsaaio_pedime.NUM_CAJA ?? DBNull.Value;
                        cmd.Parameters.Add("@DIA_PAGO", SqlDbType.DateTime).Value = (object?)lsaaio_pedime.DIA_PAGO ?? DBNull.Value;
                        cmd.Parameters.Add("@HOR_PAGO", SqlDbType.VarChar, 6).Value = (object?)lsaaio_pedime.HOR_PAGO ?? DBNull.Value;
                        cmd.Parameters.Add("@TOT_PAGO", SqlDbType.Float).Value = (object?)lsaaio_pedime.TOT_PAGO ?? DBNull.Value;
                        cmd.Parameters.Add("@TIP_ACTU", SqlDbType.VarChar, 2).Value = (object?)lsaaio_pedime.TIP_ACTU ?? DBNull.Value;
                        cmd.Parameters.Add("@CVE_REFIS", SqlDbType.VarChar, 4).Value = (object?)lsaaio_pedime.CVE_REFIS ?? DBNull.Value;
                        cmd.Parameters.Add("@CAN_BULT", SqlDbType.Float).Value = lsaaio_pedime.CAN_BULT;
                        cmd.Parameters.Add("@FIR_REME", SqlDbType.VarChar, 8).Value = (object?)lsaaio_pedime.FIR_REME ?? DBNull.Value;
                        cmd.Parameters.Add("@SEC_DESP", SqlDbType.Char, 1).Value = (object?)lsaaio_pedime.SEC_DESP ?? DBNull.Value;
                        cmd.Parameters.Add("@ADU_TRAN", SqlDbType.VarChar, 3).Value = (object?)lsaaio_pedime.ADU_TRAN ?? DBNull.Value;
                        cmd.Parameters.Add("@COP_REFE", SqlDbType.VarChar, 15).Value = (object?)lsaaio_pedime.COP_REFE ?? DBNull.Value;
                        cmd.Parameters.Add("@CAN_BULTSUBD", SqlDbType.Float).Value = lsaaio_pedime.CAN_BULTSUBD;
                        cmd.Parameters.Add("@FIR_PAGO", SqlDbType.VarChar, 10).Value = (object?)lsaaio_pedime.FIR_PAGO ?? DBNull.Value;
                        cmd.Parameters.Add("@NUM_OPER", SqlDbType.VarChar, 10).Value = (object?)lsaaio_pedime.NUM_OPER ?? DBNull.Value;
                        cmd.Parameters.Add("@REG_ENTRADA", SqlDbType.VarChar, 15).Value = (object?)lsaaio_pedime.REG_ENTRADA ?? DBNull.Value;
                        cmd.Parameters.Add("@CVE_CNTA", SqlDbType.VarChar, 2).Value = (object?)lsaaio_pedime.CVE_CNTA ?? DBNull.Value;
                        cmd.Parameters.Add("@EMP_FACT", SqlDbType.VarChar, 2).Value = (object?)lsaaio_pedime.EMP_FACT ?? DBNull.Value;

                        // Output
                        SqlParameter outputIdParam = cmd.Parameters.Add("@newid_registro", SqlDbType.Int);
                        outputIdParam.Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        id = (outputIdParam.Value != DBNull.Value) ? Convert.ToInt32(outputIdParam.Value) : 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + "Error en NET_UPDATE_SAAIO_PEDIME_NEW");
                    }
                }
            }
            return id;
        }

        public bool esRelacion(SaaioPedime objpedi, int numRem)
        {
            bool Relacion = false;
            try
            {
              if (objpedi.TIP_PEDI == null) 
                {
                    Relacion = false;
                }
                else
                {
                    if (objpedi.TIP_PEDI.ToUpper() == "C")
                    {
                        if (objpedi.FIR_REME == null)
                        {
                            throw new Exception("Es necesaria la firma de remesa antes de enviar a COVE, ya que su pedimento es un consolidado");
                        }

                    }
                    SaaioFacGui objFacGui = new SaaioFacGui();
                    SaaioFacGuiRepository objFacGuiD = new(_configuration);
                    objFacGui = objFacGuiD.VerificarExistaHouseEnRemesa(objpedi.NUM_REFE, numRem);

                    if (objFacGui == null) {
                        throw new Exception("No se puede enviar a cove la remesa, por que no se ha integrado una guia House");
                    }
                    Relacion = true;
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.Trim()); 
            }

          


                return Relacion;
        }


        }
}

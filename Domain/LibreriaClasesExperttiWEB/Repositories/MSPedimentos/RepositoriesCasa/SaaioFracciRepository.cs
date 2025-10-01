using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class SaaioFracciRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public SaaioFracciRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public void UpdateFormasDePago(string Referencia, string FpADV, string FpIVA, int MiIdCliente)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;



            cmd.CommandText = "NET_UPDATE_SAAIO_FRACCI_FORMAS_DE_PAGO";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            var objHelp = new Helper();
            // ,@NUM_REFE  varchar

            @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
            @param.Value = Referencia;

            // ,@FPA_ADV1  varchar(2)
            @param = cmd.Parameters.Add("@FPA_ADV1", SqlDbType.VarChar, 2);
            @param.Value = FpADV;

            // ,@FPA_IVA1  varchar(2)
            @param = cmd.Parameters.Add("@FPA_IVA1", SqlDbType.VarChar, 2);
            @param.Value = FpIVA;

            // ,@IDCliente INT
            @param = cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4);
            @param.Value = MiIdCliente;




            try
            {

                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_SAAIO_FRACCI_FORMAS_DE_PAGO");
            }
            cn.Close();
            cn.Dispose();

        }

        public List<SaaioFracci> Cargar(string numeroReferencia)
        {
            List<SaaioFracci> lstSaaioFracci = new List<SaaioFracci>();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;

            try
            {
                cmd.CommandText = "NET_SEARCH_SAAIO_FRACCI";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                // @NUM_REFE  varchar(15)
                param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                param.Value = numeroReferencia;
                cn.ConnectionString = SConexion;
                cn.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        SaaioFracci objSaaioFracci = new SaaioFracci
                        {
                            NUM_REFE = dr["NUM_REFE"].ToString(),
                            NUM_PART = Convert.ToDouble(dr["NUM_PART"]),
                            SUB_PART = dr["SUB_PART"].ToString(),
                            FRACCION = dr["FRACCION"].ToString(),
                            MON_FACT = Convert.ToDouble(dr["MON_FACT"]),
                            TIP_MONE = dr["TIP_MONE"].ToString(),
                            UNI_TARI = dr["UNI_TARI"].ToString(),
                            UNI_FACT = dr["UNI_FACT"].ToString(),
                            DES_MERC = dr["DES_MERC"].ToString(),
                            PAI_ORIG = dr["PAI_ORIG"].ToString(),
                            PAI_VEND = dr["PAI_VEND"].ToString(),
                            EFE_ORIG = dr["EFE_ORIG"].ToString(),
                            EFE_DEST = dr["EFE_DEST"].ToString(),
                            EFE_COMP = dr["EFE_COMP"].ToString(),
                            EFE_VEND = dr["EFE_VEND"].ToString(),
                            TIP_MERC = dr["TIP_MERC"].ToString(),
                            USO_CARA = dr["USO_CARA"].ToString(),
                            CVE_VALO = dr["CVE_VALO"].ToString(),
                            CVE_VINC = dr["CVE_VINC"].ToString(),
                            CAS_TLCS = dr["CAS_TLCS"].ToString(),
                            ADVAL = Convert.ToDouble(dr["ADVAL"]),
                            POR_ADV1 = Convert.ToDouble(dr["POR_ADV1"]),
                            POR_ADV2 = Convert.ToDouble(dr["POR_ADV2"]),
                            FPA_ADV1 = dr["FPA_ADV1"].ToString(),
                            FPA_ADV2 = dr["FPA_ADV2"].ToString(),
                            ARA_ESPE = Convert.ToDouble(dr["ARA_ESPE"]),
                            CON_AZUC = Convert.ToDouble(dr["CON_AZUC"]),
                            MON_SUNT = Convert.ToDouble(dr["MON_SUNT"]),
                            MON_DTAP = Convert.ToDouble(dr["MON_DTAP"]),
                            PORC_IVA = Convert.ToDouble(dr["PORC_IVA"]),
                            POR_IVA1 = Convert.ToDouble(dr["POR_IVA1"]),
                            POR_IVA2 = Convert.ToDouble(dr["POR_IVA2"]),
                            FPA_IVA1 = dr["FPA_IVA1"].ToString(),
                            FPA_IVA2 = dr["FPA_IVA2"].ToString(),
                            PRE_DETA = Convert.ToDouble(dr["PRE_DETA"]),
                            NUM_UNID = Convert.ToDouble(dr["NUM_UNID"]),
                            CAL_IEPS = dr["CAL_IEPS"].ToString(),
                            MON_IEPS = Convert.ToDouble(dr["MON_IEPS"]),
                            FPA_IEPS = dr["FPA_IEPS"].ToString(),
                            CAL_ISAN = dr["CAL_ISAN"].ToString(),
                            FPA_ISAN = dr["FPA_ISAN"].ToString(),
                            CAL_COMP = dr["CAL_COMP"].ToString(),
                            MON_COM1 = Convert.ToDouble(dr["MON_COM1"]),
                            MON_COM2 = Convert.ToDouble(dr["MON_COM2"]),
                            TIP_MCO1 = dr["TIP_MCO1"].ToString(),
                            TIP_MCO2 = dr["TIP_MCO2"].ToString(),
                            FPA_COM1 = dr["FPA_COM1"].ToString(),
                            FPA_COM2 = dr["FPA_COM2"].ToString(),
                            IMP_GARA = Convert.ToDouble(dr["IMP_GARA"]),
                            VAL_AGRE = Convert.ToDouble(dr["VAL_AGRE"]),
                            MAR_MERC = dr["MAR_MERC"].ToString(),
                            MOD_MERC = dr["MOD_MERC"].ToString(),
                            COD_PROD = dr["COD_PROD"].ToString(),
                            VIN_VEHI = dr["VIN_VEHI"].ToString(),
                            KMT_VEHI = Convert.ToInt32(dr["KMT_VEHI"]),
                            VAL_NORF = Convert.ToDouble(dr["VAL_NORF"]),
                            VAL_COMF = Convert.ToDouble(dr["VAL_COMF"]),
                            FAC_PROR = Convert.ToDouble(dr["FAC_PROR"]),
                            FPA_ESPE = dr["FPA_ESPE"].ToString(),
                            CAS_PREF = dr["CAS_PREF"].ToString(),
                            COM_PREF = dr["COM_PREF"].ToString(),
                            MON_ESPE = Convert.ToDouble(dr["MON_ESPE"]),
                            POR_COMP = Convert.ToDouble(dr["POR_COMP"]),
                            CAN_FACT = Convert.ToDouble(dr["CAN_FACT"]),
                            CAN_TARI = Convert.ToDouble(dr["CAN_TARI"]),
                            COM_TLC = dr["COM_TLC"].ToString(),
                            DECR_TLC = dr["DECR_TLC"].ToString(),
                            PRE_ESTI = Convert.ToDouble(dr["PRE_ESTI"]),
                            UNI_ESTI = Convert.ToDouble(dr["UNI_ESTI"]),
                            VAL_RETORNO = Convert.ToDouble(dr["VAL_RETORNO"]),
                            FEC_TLC = dr["FEC_TLC"].ToString(),
                        };
                        lstSaaioFracci.Add(objSaaioFracci);
                    }
                }
                dr.Close();
                cn.Close();
                cn.Dispose();
                return lstSaaioFracci;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message.ToString() + "Cargar SaaioFracci");
            }
        }


        public void SaberSiEsTextil(string Referencia, int IDReferencia)
        {         
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_SABER_SI_ES_TEXTIL", con)
                {
                    CommandType = CommandType.StoredProcedure
                };  

                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = Referencia;
                cmd.Parameters.Add("@IDReferencia", SqlDbType.Int, 4).Value = IDReferencia;
              
                con.Open();

                cmd.ExecuteNonQuery();              

            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message.ToString() + "NET_SABER_SI_ES_TEXTIL");
            }
           
        }


    }
}

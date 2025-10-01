using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class SaaioFacturRepository : ISaaioFacturRepository
    {
        public string SConexion { get; set; }

        string ISaaioFacturRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;

        public SqlConnection con;

        public SaaioFacturRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int Insertar(SaaioFactur lsaaio_factur)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            try
            {

                cmd.CommandText = "NET_INSERT_SAAIO_FACTUR";
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
        public string NET_SABER_SI_EXISTE_GUIA_EN_FACTURA_DE_REMESA(string MyNum_Refe, int MyNum_Rem)
        {
            string FacturasQueFaltan;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            try
            {

                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_SABER_SI_EXISTE_GUIA_EN_FACTURA_DE_REMESA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = MyNum_Refe;

                // , @NUM_REM INT
                @param = cmd.Parameters.Add("@NUM_REM", SqlDbType.Int, 4);
                @param.Value = MyNum_Rem;

                // , @FacturaSinGuia VARCHAR(MAX) OUTPUT 
                @param = cmd.Parameters.Add("@FacturaSinGuia", SqlDbType.VarChar, 8000);
                @param.Direction = ParameterDirection.Output;



                cmd.ExecuteNonQuery();
                FacturasQueFaltan = cmd.Parameters["@FacturaSinGuia"].Value == null ? "" : cmd.Parameters["@FacturaSinGuia"].Value.ToString();

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                FacturasQueFaltan = "";
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_SABER_SI_EXISTE_GUIA_EN_FACTURA_DE_REMESA");
            }

            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return FacturasQueFaltan;
        }

        public List<SaaioFactur> CargarRemesa(string MyNum_refe, int NumRem)
        {
            var lstSAAIO_FACTUR = new List<SaaioFactur>();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            try
            {


                cmd.CommandText = "NET_LOAD_SAAIO_FACTUR_NUM_REM";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @NUM_REFE VARCHAR(15) ,
                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = MyNum_refe;

                // @CONS_FACT INT
                @param = cmd.Parameters.Add("@NUM_REM", SqlDbType.Int, 4);
                @param.Value = NumRem;
                // Insertar Parametro de busqueda

                cn.ConnectionString = SConexion;
                cn.Open();

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

        public int ModificarRelacion(string NumerodeReferencia, int ConsFact, bool Relacion)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                cmd.CommandText = "NET_UPDATE_SAAIO_FACTUR_RELACION";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // ,@NUM_REFE  varchar
                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = NumerodeReferencia;

                // ,@CVE_PROV  varchar    
                @param = cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4);
                @param.Value = ConsFact;

                @param = cmd.Parameters.Add("@REL_FACT", SqlDbType.Int, 4);
                @param.Value = Relacion;

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
                throw new Exception(ex.Message.ToString() + " NET_UPDATE_SAAIO_FACTUR_RELACION");
            }

            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
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
                    dap.SelectCommand.CommandText = "NET_LOAD_FACTURAS_VALORDETRANSACCION";


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
        public TotalDeFacturaParaCOVE TotalDeFactura(string MyNum_Refe, int MyCons_fact)
        {

            var ObjTotalDeFacturaParaCove = new TotalDeFacturaParaCOVE();

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            try
            {
                cmd.CommandText = "NET_LOAD_TOTALDEFACTURA";
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
                cmd.CommandText = "NET_UPDATE_VINCULACION";
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
        public int InsertarNew(SaaioFactur lsaaio_factur)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_INSERT_SAAIO_FACTUR_NEW";
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
                    dap.SelectCommand.CommandText = "NET_LOAD_TOTALDEFACTURAS";

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
                    dap.SelectCommand.CommandText = "NET_LOAD_TODAS_LAS_FACTURAS";

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
                cmd.CommandText = "NET_EXTRAE_MAX_CONS_FACT";
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
        public bool BorraFacturaDelCasa(string MyNum_Refe, int MyCons_Fact)
        {
            bool BorraFacturaDelCasaRet = default;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            try
            {

                cmd.CommandText = "NET_DELETE_SAAIO_FACTUR";
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

        public int Modificar(SaaioFactur lsaaio_factur)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            try
            {


                cmd.CommandText = "NET_UPDATE_SAAIO_FACTUR";
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
                @param = cmd.Parameters.Add("@PES_BRUT", SqlDbType.Float, 12);
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

                cmd.CommandText = "NET_UPDATE_SAAIO_FACTUR_PROVED";
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
        public SaaioFactur Buscar(string NUM_REFE, string NUM_FACT)
        {
            var objSAAIO_FACTUR = new SaaioFactur();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            try
            {
                cmd.CommandText = "NET_SEARCH_SAAIO_FACTUR_NUM_FACT";
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

        public SaaioFactur Buscar(string MyNum_refe, int MyCons_Fact)
        {
            var objSAAIO_FACTUR = new SaaioFactur();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            try
            {

                cmd.CommandText = "NET_SEARCH_SAAIO_FACTUR";
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

                cmd.CommandText = "NET_LOAD_SAAIO_FACTUR";
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

        public int ModificarFactur(string NumerodeReferencia, string CveProveedor)
        {
            int id;
            try
            {
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_UPDATE_SAAIO_FACTUR_PROVED", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NumerodeReferencia;
                    cmd.Parameters.Add("@CVE_PRO", SqlDbType.VarChar, 6).Value = CveProveedor;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return id;
        }

        public int NET_SABER_CUANTOS_COVE_TIENE_LAREMESA(string MyNum_Refe, int MyNum_Rem)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            try
            {

                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_SABER_CUANTOS_COVE_TIENE_LAREMESA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = MyNum_Refe;

                // , @NUM_REM INT
                @param = cmd.Parameters.Add("@NUM_REM", SqlDbType.Int, 4);
                @param.Value = MyNum_Rem;

                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;



                cmd.ExecuteNonQuery();
                id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {

                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_SABER_CUANTOS_COVE_TIENE_LAREMESA");
            }

            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }

    }
}

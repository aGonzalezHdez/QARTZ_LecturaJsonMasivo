using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class CtracClientRepository : ICtracClientRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CtracClientRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public CtracClient Buscar(string MyCve_Imp)
        {
            var objCTRAC_CLIENT = new CtracClient();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CTRAC_CLIENT";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            // @CVE_IMP VARCHAR(6)
            @param = cmd.Parameters.Add("@CVE_IMP", SqlDbType.VarChar, 6);
            @param.Value = MyCve_Imp;
            // Insertar Parametro de busqueda

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objCTRAC_CLIENT.CVE_IMP = dr["CVE_IMP"].ToString().Trim();
                    objCTRAC_CLIENT.NOM_IMP = dr["NOM_IMP"].ToString().Trim();
                    objCTRAC_CLIENT.DIR_IMP = dr["DIR_IMP"].ToString().Trim();
                    objCTRAC_CLIENT.POB_IMP = dr["POB_IMP"].ToString().Trim();
                    objCTRAC_CLIENT.CP_IMP = dr["CP_IMP"].ToString().Trim();
                    objCTRAC_CLIENT.RFC_IMP = dr["RFC_IMP"].ToString().Trim();
                    objCTRAC_CLIENT.CUR_IMP = dr["CUR_IMP"].ToString().Trim();
                    objCTRAC_CLIENT.NOI_IMP = dr["NOI_IMP"].ToString().Trim();
                    objCTRAC_CLIENT.NOE_IMP = dr["NOE_IMP"].ToString().Trim();
                    objCTRAC_CLIENT.EFE_IMP = dr["EFE_IMP"].ToString().Trim();
                    objCTRAC_CLIENT.PAI_IMP = dr["PAI_IMP"].ToString().Trim();
                    objCTRAC_CLIENT.EFE_DESI = dr["EFE_DESI"].ToString().Trim();
                    objCTRAC_CLIENT.TEL_IMP = dr["TEL_IMP"].ToString().Trim();
                    objCTRAC_CLIENT.FAX_IMP = dr["FAX_IMP"].ToString().Trim();
                    objCTRAC_CLIENT.ATN_IMP = dr["ATN_IMP"].ToString().Trim();
                    objCTRAC_CLIENT.CVE_COMI = dr["CVE_COMI"].ToString().Trim();
                    objCTRAC_CLIENT.FIR_TRAD = dr["FIR_TRAD"].ToString().Trim();
                    objCTRAC_CLIENT.PUE_FIRM = dr["PUE_FIRM"].ToString().Trim();
                    objCTRAC_CLIENT.PAG_ELEC = dr["PAG_ELEC"].ToString().Trim();
                    objCTRAC_CLIENT.COM_ENOM = dr["COM_ENOM"].ToString().Trim();
                    objCTRAC_CLIENT.BAJ_IMP = Convert.ToDateTime(dr["BAJ_IMP"]);
                    objCTRAC_CLIENT.DES_ORIG = Convert.ToInt32(dr["DES_ORIG"]);
                    objCTRAC_CLIENT.CVE_CTABAN = dr["CVE_CTABAN"].ToString().Trim();
                    objCTRAC_CLIENT.VCOM_VAGRE = dr["VCOM_VAGRE"].ToString().Trim();
                    objCTRAC_CLIENT.MOD_FACPAR = dr["MOD_FACPAR"].ToString().Trim();
                    objCTRAC_CLIENT.RUT_IMAG = dr["RUT_IMAG"].ToString().Trim();
                    objCTRAC_CLIENT.CVE_EJEC = dr["CVE_EJEC"].ToString().Trim();
                    objCTRAC_CLIENT.APE_PATE = dr["APE_PATE"].ToString().Trim();
                    objCTRAC_CLIENT.APE_MATE = dr["APE_MATE"].ToString().Trim();
                    objCTRAC_CLIENT.COL_IMP = dr["COL_IMP"].ToString().Trim();
                    objCTRAC_CLIENT.LOC_IMP = dr["LOC_IMP"].ToString().Trim();
                    objCTRAC_CLIENT.REFE_IMP = dr["REFE_IMP"].ToString().Trim();
                    objCTRAC_CLIENT.NOM_COVE = dr["NOM_COVE"].ToString().Trim();
                    objCTRAC_CLIENT.MUN_COVE = dr["MUN_COVE"].ToString().Trim();
                    objCTRAC_CLIENT.CONV_BANC = dr["CONV_BANC"].ToString().Trim();
                    objCTRAC_CLIENT.BAJ_DESC = dr["BAJ_DESC"].ToString().Trim();
                    objCTRAC_CLIENT.ATN_IMP2 = dr["ATN_IMP2"].ToString().Trim();
                    objCTRAC_CLIENT.FEC_ALTA = Convert.ToDateTime(dr["FEC_ALTA"]);
                    objCTRAC_CLIENT.DAT_ADIC = dr["DAT_ADIC"].ToString().Trim();
                    objCTRAC_CLIENT.RUTA_FPPI = dr["RUTA_FPPI"].ToString().Trim();
                }
                else
                {
                    objCTRAC_CLIENT = default;
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

            return objCTRAC_CLIENT;
        }

        public CtracClient BuscarClientePorReferencia(string NumeroDeReferencia)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NumeroDeReferencia))
                    throw new ArgumentException("El número de referencia no puede estar vacío.");

                var objPedimeD = new SaaioPedimeRepository(_configuration);
                var objPedime = objPedimeD.Buscar(NumeroDeReferencia);

                if (objPedime == null)
                    throw new InvalidOperationException($"No existe el pedimento para la referencia '{NumeroDeReferencia}' en el sistema CASA.");

                var objClientD = new CtracClientRepository(_configuration);
                var ctracClient = objClientD.Buscar(objPedime.CVE_IMPO);

                if (ctracClient == null)
                    throw new InvalidOperationException($"No existe el cliente con clave '{objPedime.CVE_IMPO}' en el sistema CASA.");

                return ctracClient;
            }
            catch (ArgumentException ex)
            {
                throw new Exception("Error de validación: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("Error de operación: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error inesperado al buscar el importador: " + ex.Message);
            }
        }

    }
}

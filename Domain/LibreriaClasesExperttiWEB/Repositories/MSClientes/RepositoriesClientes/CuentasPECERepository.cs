using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class CuentasPECERepository : ICuentasPECERepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CuentasPECERepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<CuentasdelCliente> CuentasdelCliente(int IdCliente)
        {
            List<CuentasdelCliente> list = new();
            {
                try
                {
                    using (con = new(SConexion))
                    using (SqlCommand cmd = new("NET_LOAD_CUENTAS_PECE_POR_IDCLIENTE", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4).Value = IdCliente;

                        using SqlDataReader reader = cmd.ExecuteReader();
                        list = SqlDataReaderToList.DataReaderMapToList<CuentasdelCliente>(reader);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }
            return list;
        }

        public List<Saaic_Ctaban> BuscarPorCuenta(string NumerodeCuenta)
        {
            List<Saaic_Ctaban> listSaaic_Ctaban = new();

            try
            {
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_SEARCH_SAAIC_CTABAN_DUPLICADAS_NUMCTA", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NUM_CTA", SqlDbType.VarChar, 20).Value = NumerodeCuenta;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Saaic_Ctaban objSaaic_Ctaban = new();
                            objSaaic_Ctaban.CVE_CTA = dr["CVE_CTA"].ToString();
                            objSaaic_Ctaban.NUM_CTA = dr["NUM_CTA"].ToString();
                            objSaaic_Ctaban.DES_CTA = dr["DES_CTA"].ToString();
                            objSaaic_Ctaban.ATN_SUC = dr["ATN_SUC"].ToString();
                            objSaaic_Ctaban.CVE_BAN = dr["CVE_BAN"].ToString();
                            objSaaic_Ctaban.CTA_M3 = dr["CTA_M3"].ToString();
                            objSaaic_Ctaban.CTA_CENT = dr["CTA_CENT"].ToString();
                            objSaaic_Ctaban.IMP_MAXP = dr["IMP_MAXP"] == DBNull.Value ? null : (float?)Convert.ToDecimal(dr["IMP_MAXP"]);
                            objSaaic_Ctaban.CVE_IMP = dr["CVE_IMP"].ToString();
                            objSaaic_Ctaban.LOGIN = dr["LOGIN"].ToString();
                            objSaaic_Ctaban.PAT_AA = dr["PAT_AA"].ToString();
                            objSaaic_Ctaban.CVE_ADUA = dr["CVE_ADUA"].ToString();
                            objSaaic_Ctaban.FEC_BAJA = dr["FEC_BAJA"] == DBNull.Value ? null : Convert.ToDateTime(dr["FEC_BAJA"]);
                            objSaaic_Ctaban.CVE_PECE = dr["CVE_PECE"].ToString();

                            listSaaic_Ctaban.Add(objSaaic_Ctaban);
                        }
                    }
                    else
                        listSaaic_Ctaban = null/* TODO Change to default(_) if this is not a reference type */;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listSaaic_Ctaban;
        }


        public List<Saaic_Ctaban> BuscarPorIdentificador(string Identificador)
        {
            List<Saaic_Ctaban> listSaaic_Ctaban = new();

            try
            {
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_SEARCH_SAAIC_CTABAN_DUPLICADAS_CTA_CENT", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CTA_CENT", SqlDbType.VarChar, 20).Value = Identificador;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Saaic_Ctaban objSaaic_Ctaban = new();
                            objSaaic_Ctaban.CVE_CTA = dr["CVE_CTA"].ToString();
                            objSaaic_Ctaban.NUM_CTA = dr["NUM_CTA"].ToString();
                            objSaaic_Ctaban.DES_CTA = dr["DES_CTA"].ToString();
                            objSaaic_Ctaban.ATN_SUC = dr["ATN_SUC"].ToString();
                            objSaaic_Ctaban.CVE_BAN = dr["CVE_BAN"].ToString();
                            objSaaic_Ctaban.CTA_M3 = dr["CTA_M3"].ToString();
                            objSaaic_Ctaban.CTA_CENT = dr["CTA_CENT"].ToString();
                            objSaaic_Ctaban.IMP_MAXP = dr["IMP_MAXP"] == DBNull.Value ? null : (float?)Convert.ToDecimal(dr["IMP_MAXP"]);
                            objSaaic_Ctaban.CVE_IMP = dr["CVE_IMP"].ToString();
                            objSaaic_Ctaban.LOGIN = dr["LOGIN"].ToString();
                            objSaaic_Ctaban.PAT_AA = dr["PAT_AA"].ToString();
                            objSaaic_Ctaban.CVE_ADUA = dr["CVE_ADUA"].ToString();
                            objSaaic_Ctaban.FEC_BAJA = dr["FEC_BAJA"] == DBNull.Value ? null : Convert.ToDateTime(dr["FEC_BAJA"]);
                            objSaaic_Ctaban.CVE_PECE = dr["CVE_PECE"].ToString();

                            listSaaic_Ctaban.Add(objSaaic_Ctaban);
                        }
                    }
                    else
                        listSaaic_Ctaban = null/* TODO Change to default(_) if this is not a reference type */;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listSaaic_Ctaban;
        }


        public List<Saaic_Ctaban> BuscarPorCuentaDuplicada(string NumerodeCuenta, string Patente, string Aduana)
        {
            List<Saaic_Ctaban> listSaaic_Ctaban = new();
            try
            {
                using (con = new(SConexion))

                using (var cmd = new SqlCommand("NET_SEARCH_SAAIC_CTABAN_DUPLICADAS_NUMCTA_ADUANA_PATENTE", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NUM_CTA", SqlDbType.VarChar, 20).Value = NumerodeCuenta;
                    cmd.Parameters.Add("@PAT_AA", SqlDbType.VarChar, 4).Value = Patente ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@CVE_ADUA", SqlDbType.VarChar, 3).Value = Aduana ?? (object)DBNull.Value;
                    //cmd.Parameters.Add("@CVE_IMP", SqlDbType.VarChar, 250).Value = ClaveCliente;
                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Saaic_Ctaban objSaaic_Ctaban = new();
                            objSaaic_Ctaban.CVE_CTA = dr["CVE_CTA"].ToString();
                            objSaaic_Ctaban.NUM_CTA = dr["NUM_CTA"].ToString();
                            objSaaic_Ctaban.DES_CTA = dr["DES_CTA"].ToString();
                            objSaaic_Ctaban.ATN_SUC = dr["ATN_SUC"].ToString();
                            objSaaic_Ctaban.CVE_BAN = dr["CVE_BAN"].ToString();
                            objSaaic_Ctaban.CTA_M3 = dr["CTA_M3"].ToString();
                            objSaaic_Ctaban.CTA_CENT = dr["CTA_CENT"].ToString();
                            objSaaic_Ctaban.IMP_MAXP = dr["IMP_MAXP"] == DBNull.Value ? null : (float?)Convert.ToDecimal(dr["IMP_MAXP"]);
                            objSaaic_Ctaban.CVE_IMP = dr["CVE_IMP"].ToString();
                            objSaaic_Ctaban.LOGIN = dr["LOGIN"].ToString();
                            objSaaic_Ctaban.PAT_AA = dr["PAT_AA"].ToString();
                            objSaaic_Ctaban.CVE_ADUA = dr["CVE_ADUA"].ToString();
                            objSaaic_Ctaban.FEC_BAJA = dr["FEC_BAJA"] == DBNull.Value ? null : Convert.ToDateTime(dr["FEC_BAJA"]);
                            objSaaic_Ctaban.CVE_PECE = dr["CVE_PECE"].ToString();

                            listSaaic_Ctaban.Add(objSaaic_Ctaban);
                        }
                    }
                    else
                        listSaaic_Ctaban = null/* TODO Change to default(_) if this is not a reference type */;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listSaaic_Ctaban;
        }

        public string InsertarCuenta(CuentasPECE objCuentasPECE)
        {
            int CtaPropia = 0;
            CuentasPECERepository objCuentasPECERepository = new(_configuration);
            string Error = objCuentasPECERepository.ValidacionesComponentes(objCuentasPECE);

            if (Error != "Ok")
            {
                return Error;
            }

            if (objCuentasPECE.CVE_ADUA == "" || objCuentasPECE.PAT_AA == "")
            {
                List<Saaic_Ctaban> lstSaaic_Ctaban = objCuentasPECERepository.BuscarPorCuenta(objCuentasPECE.NUM_CTA);
                if (lstSaaic_Ctaban != null)
                {
                    foreach (Saaic_Ctaban item in lstSaaic_Ctaban)
                    {
                        if (item.CVE_IMP == objCuentasPECE.ClaveCliente && item.NUM_CTA == objCuentasPECE.NUM_CTA)
                            EliminarRegistros(objCuentasPECE.ClaveCliente, objCuentasPECE.NUM_CTA);
                    }
                }
            }

            objCuentasPECE.ALC_IMP = "1";/* En esta pantalla siempre va ser 1 (únicamente)*/
            string CVE_CTA = Insertar(objCuentasPECE);
            InsertarRelacionClientePECE(CVE_CTA, objCuentasPECE.ClaveCliente, CtaPropia, objCuentasPECE.IdOficina, objCuentasPECE.IDDatosDeEmpresa, objCuentasPECE.IdUsuario);

            return CVE_CTA;
        }

        public string Insertar(CuentasPECE objCuentasPECE)
        {
            string CVE_CTA = string.Empty;
            try
            {
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_CASAEI_INSERT_CUENTA_PECE", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NUM_CTA", SqlDbType.VarChar, 20).Value = objCuentasPECE.NUM_CTA;
                    cmd.Parameters.Add("@DES_CTA", SqlDbType.VarChar, 40).Value = objCuentasPECE.DES_CTA;
                    cmd.Parameters.Add("@CVE_BAN", SqlDbType.VarChar, 6).Value = objCuentasPECE.CVE_BAN;
                    cmd.Parameters.Add("@CTA_CENT", SqlDbType.VarChar, 5).Value = objCuentasPECE.CTA_CENT;
                    cmd.Parameters.Add("@PAT_AA", SqlDbType.VarChar, 4).Value = objCuentasPECE.PAT_AA;
                    cmd.Parameters.Add("@CVE_ADUA", SqlDbType.VarChar, 3).Value = objCuentasPECE.CVE_ADUA;
                    cmd.Parameters.Add("@IMP_EXPO", SqlDbType.VarChar, 1).Value = objCuentasPECE.IMP_EXPO;
                    cmd.Parameters.Add("@ALC_IMP", SqlDbType.VarChar, 1).Value = objCuentasPECE.ALC_IMP;

                    cmd.Parameters.Add("@newid_registro", SqlDbType.VarChar, 2).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToString(cmd.Parameters["@newid_registro"].Value) != "")
                        {
                            CVE_CTA = Convert.ToString(cmd.Parameters["@newid_registro"].Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return CVE_CTA;
        }

        public string ModificarCuenta(CuentasPECE objCuentasPECE)
        {
            int CtaPropia = 0;
            string CVE_CTA = objCuentasPECE.CVE_CTA;
            CuentasPECERepository objCuentasPECERepository = new(_configuration);
            string Error = objCuentasPECERepository.ValidacionesComponentes(objCuentasPECE);

            if (Error != "Ok")
            {
                return Error;
            }

            if (objCuentasPECE.CVE_ADUA == "" || objCuentasPECE.PAT_AA == "")
            {
                List<Saaic_Ctaban> lstSaaic_Ctaban = objCuentasPECERepository.BuscarPorCuenta(objCuentasPECE.NUM_CTA);
                if (lstSaaic_Ctaban != null)
                {
                    foreach (Saaic_Ctaban item in lstSaaic_Ctaban)
                    {
                        if (item.CVE_IMP == objCuentasPECE.ClaveCliente && item.NUM_CTA == objCuentasPECE.NUM_CTA)

                            EliminarRegistros(objCuentasPECE.ClaveCliente, objCuentasPECE.NUM_CTA);
                    }
                }
            }

            objCuentasPECE.ALC_IMP = "1";/* En esta pantalla siempre va ser 1 (únicamente)*/
            BorrarRelacionClientePECE(CVE_CTA);
            CambiaRPECA(objCuentasPECE);
            InsertarRelacionClientePECE(CVE_CTA, objCuentasPECE.ClaveCliente, CtaPropia, objCuentasPECE.IdOficina, objCuentasPECE.IDDatosDeEmpresa, objCuentasPECE.IdUsuario);
            return CVE_CTA;
        }

        public int CambiaRPECA(CuentasPECE objCuentasPECE)
        {
            int id;

            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_UPDATE_PECA", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CVE_CTA", SqlDbType.VarChar, 2).Value = objCuentasPECE.CVE_CTA;
                    cmd.Parameters.Add("@NUM_CTA", SqlDbType.VarChar, 20).Value = objCuentasPECE.NUM_CTA;
                    cmd.Parameters.Add("@DES_CTA", SqlDbType.VarChar, 40).Value = objCuentasPECE.DES_CTA;
                    cmd.Parameters.Add("@CVE_BAN", SqlDbType.VarChar, 6).Value = objCuentasPECE.CVE_BAN;
                    cmd.Parameters.Add("@CTA_CENT", SqlDbType.VarChar, 5).Value = objCuentasPECE.CTA_CENT;
                    cmd.Parameters.Add("@PAT_AA", SqlDbType.VarChar, 4).Value = objCuentasPECE.PAT_AA;
                    cmd.Parameters.Add("@CVE_ADUA", SqlDbType.VarChar, 3).Value = objCuentasPECE.CVE_ADUA;
                    cmd.Parameters.Add("@IMP_EXPO", SqlDbType.VarChar, 1).Value = objCuentasPECE.IMP_EXPO;
                    cmd.Parameters.Add("@ALC_IMP", SqlDbType.VarChar, 1).Value = objCuentasPECE.ALC_IMP;

                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        {
                            id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                        }
                        else
                        {
                            id = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return id;
        }

        public CuentasPECE BuscarDetalleCveCtaPece(string CveCta)
        {
            CuentasPECE objCuentasPECE = new();
            {
                try
                {
                    using (con = new(SConexion))
                    using (SqlCommand cmd = new("NET_SEARCH_DETALLECTA_PECA", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CVE_CTA", SqlDbType.VarChar, 5).Value = CveCta;

                        using SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            dr.Read();
                            objCuentasPECE.CVE_CTA = dr["CVE_CTA"].ToString();
                            objCuentasPECE.IMP_EXPO = dr["IMP_EXPO"].ToString();
                            objCuentasPECE.ALC_IMP = dr["ALC_IMP"].ToString();
                            objCuentasPECE.NUM_CTA = dr["NUM_CTA"].ToString();
                            objCuentasPECE.DES_CTA = dr["DES_CTA"].ToString();
                            objCuentasPECE.CVE_BAN = dr["CVE_BAN"].ToString();
                            objCuentasPECE.CTA_CENT = dr["CTA_CENT"].ToString();
                            objCuentasPECE.PAT_AA = dr["PAT_AA"].ToString(); /*dr["PAT_AA"] == DBNull.Value ? null : dr["PAT_AA"].ToString();  */
                            objCuentasPECE.CVE_ADUA = dr["CVE_ADUA"].ToString(); /*dr["CVE_ADUA"] == DBNull.Value ? null : dr["CVE_ADUA"].ToString();*/
                            objCuentasPECE.CtaPropia = Convert.ToBoolean(dr["CtaPropia"]);
                            objCuentasPECE.FechaDeAlta = Convert.ToDateTime(dr["FechaDeAlta"]);
                            objCuentasPECE.UsuarioAlta = dr["UsuarioAlta"].ToString();
                            objCuentasPECE.IdOficina = Convert.ToInt32(dr["IDOficina"]);
                        }
                        else
                        {
                            objCuentasPECE = default;
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }
            return objCuentasPECE;
        }

        public List<CargarRelClientesCuentasPECA> CargarRelClientesCuentasPECA(string CveCta)
        {
            List<CargarRelClientesCuentasPECA> list = new();
            {
                try
                {
                    using (con = new(SConexion))
                    using (SqlCommand cmd = new("NET_LOAD_SAAIC_CTABAND2", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CVE_CTA", SqlDbType.VarChar, 5).Value = CveCta;
                        using SqlDataReader reader = cmd.ExecuteReader();
                        list = SqlDataReaderToList.DataReaderMapToList<CargarRelClientesCuentasPECA>(reader);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }
            return list;
        }

        public int BorrarRelacionClientePECE(string CveCta)
        {
            int id = 0;
            {
                try
                {
                    using (con = new(SConexion))
                    using (SqlCommand cmd = new("NET_DELETE_RELCLIENTEPECA", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CVE_CTA", SqlDbType.VarChar, 5).Value = CveCta;
                        cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                        using (cmd.ExecuteReader())
                        {
                            if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                            {
                                id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }
            return id;
        }

        public int InsertarRelacionClientePECE(string CVE_CTA, string CVE_IMP, int CTAPROPIA, int IDOFICINA, int IDDATOSDEEMPRESA, int IdUsuario)
        {
            int id = 0;
            try

            {
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_INSERT_RELCLIENTEPECA_NEW", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CVE_CTA", SqlDbType.VarChar, 2).Value = CVE_CTA;
                    cmd.Parameters.Add("@CVE_IMP", SqlDbType.VarChar, 20).Value = CVE_IMP;
                    cmd.Parameters.Add("@CTAPROPIA", SqlDbType.Int, 4).Value = CTAPROPIA;
                    cmd.Parameters.Add("@IDOFICINA", SqlDbType.Int, 4).Value = IDOFICINA;
                    cmd.Parameters.Add("@IDDATOSDEEMPRESA", SqlDbType.Int, 4).Value = IDDATOSDEEMPRESA;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = IdUsuario;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        {

                            id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return id;
        }

        public int EliminarRegistros(string Clave, string NUM_CTA)
        {
            int id;

            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_CASAEI_DELETE_RELCLIENTEPECA", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CVE_IMP", SqlDbType.VarChar, 6).Value = Clave;
                    cmd.Parameters.Add("@NUM_CTA", SqlDbType.VarChar, 20).Value = NUM_CTA;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        {
                            id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                        }
                        else
                        {
                            id = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
            return id;
        }

        public string ValidacionesComponentes(CuentasPECE objCuentasPECE)
        {
            string Error = "Ok";

            ////Validar Número de cuenta HAY UN END POINT
            //List<Saaic_Ctaban> listSaaic_Ctaban = BuscarPorCuentaDuplicada(objCuentasPECE.NUM_CTA, objCuentasPECE.PAT_AA, objCuentasPECE.CVE_ADUA);
            //if (listSaaic_Ctaban != null)
            //{
            //    return Error = $"El número de cuenta" + objCuentasPECE.NUM_CTA + " ya esta asociado a la Aduana " + objCuentasPECE.CVE_ADUA + " y Patente " + objCuentasPECE.PAT_AA + ".";

            //}

            //solo en el caso que venga en nulo, el uevo objeto ya 

            if (objCuentasPECE.CVE_BAN != "08") /*Banorte*/
            {
                if (objCuentasPECE.CTA_CENT == "")
                {
                    return Error = "Imposible continuar si el identificador PECE";
                }
            }

            if (objCuentasPECE.DES_CTA == "")
            {
                return Error = "Imposible continuar sin una Descripción";
            }

            return Error;
        }
    }
}

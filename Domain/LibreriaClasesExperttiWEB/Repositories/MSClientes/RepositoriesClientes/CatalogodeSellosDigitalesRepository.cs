using System.Data;
using System.Data.SqlClient;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class CatalogodeSellosDigitalesRepository : ICatalogodeSellosDigitalesRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogodeSellosDigitalesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public bool ProbarCredenciales(string Usuario, string PasswordWS)
        {
            bool Respuesta = false;

            wsVentanillaUnica.ComprobanteValorElectronico[] objComprobantes = new wsVentanillaUnica.ComprobanteValorElectronico[0];

            wsVentanillaUnica.ReceptorClient objClienteWs = new wsVentanillaUnica.ReceptorClient();
            objClienteWs.ClientCredentials.UserName.UserName = Usuario;
            objClienteWs.ClientCredentials.UserName.Password = PasswordWS;

            try
            {
                wsVentanillaUnica.Acuse Acuse = new wsVentanillaUnica.Acuse();

                wsVentanillaUnica.RecibirCoveRequest x = new wsVentanillaUnica.RecibirCoveRequest();
                x.solicitarRecibirCoveServicio = objComprobantes;

                var y = objClienteWs.RecibirCoveAsync(x.solicitarRecibirCoveServicio);



                Respuesta = true;

            }
            catch (Exception)
            {
                Respuesta = false;
            }



            return Respuesta;
        }

        public int Insertar(CatalogodeSellosDigitales objCatalogodeSellosDigitales)
        {
            int id;

            try
            {
                ValidarObjeto(objCatalogodeSellosDigitales, true);
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_INSERT_CATALOGODESELLOSDIGITALES_WEB", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UsuarioWebService", SqlDbType.VarChar, 13).Value = objCatalogodeSellosDigitales.UsuarioWebService;
                    cmd.Parameters.Add("@PasswordWebService", SqlDbType.VarChar, int.MaxValue).Value = objCatalogodeSellosDigitales.PasswordWebService;
                    cmd.Parameters.Add("@ArchivoKey", SqlDbType.VarBinary).Value = objCatalogodeSellosDigitales.ArchivoKey;
                    cmd.Parameters.Add("@ArchivoCer", SqlDbType.VarBinary).Value = objCatalogodeSellosDigitales.ArchivoCer;
                    cmd.Parameters.Add("@PasswordSello", SqlDbType.VarChar, int.MaxValue).Value = objCatalogodeSellosDigitales.PasswordSello;
                    cmd.Parameters.Add("@CertificadoBase64", SqlDbType.VarChar, int.MaxValue).Value = Convert.ToBase64String(objCatalogodeSellosDigitales.ArchivoCer);
                    cmd.Parameters.Add("@OpenSSL", SqlDbType.Bit, 4).Value = objCatalogodeSellosDigitales.OpenSSL;
                    cmd.Parameters.Add("@NumeroDeSerie", SqlDbType.VarChar, 20).Value = objCatalogodeSellosDigitales.NumeroDeSerie;
                    cmd.Parameters.Add("@RFCConsulta", SqlDbType.VarChar, int.MaxValue).Value = objCatalogodeSellosDigitales.RFCConsulta;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, int.MaxValue).Value = objCatalogodeSellosDigitales.Email;
                    cmd.Parameters.Add("@FechaExpedicion", SqlDbType.DateTime).Value = objCatalogodeSellosDigitales.FechaExpedicion;
                    cmd.Parameters.Add("@FechaVigencia", SqlDbType.DateTime).Value = objCatalogodeSellosDigitales.FechaVigencia;
                    cmd.Parameters.Add("@Algoritmo", SqlDbType.VarChar, 15).Value = objCatalogodeSellosDigitales.Algoritmo;
                    cmd.Parameters.Add("@SellaMensajeria", SqlDbType.Bit).Value = objCatalogodeSellosDigitales.SellaMensajeria;
                    cmd.Parameters.Add("@CiecUsuario", SqlDbType.VarChar, 15).Value = objCatalogodeSellosDigitales.CiecUsuario;
                    cmd.Parameters.Add("@CiecPassword", SqlDbType.VarChar, 15).Value = objCatalogodeSellosDigitales.CiecPassword;
                    cmd.Parameters.Add("@UsuarioAVC", SqlDbType.VarChar, 100).Value = objCatalogodeSellosDigitales.UsuarioAVC;
                    cmd.Parameters.Add("@PasswordAVC", SqlDbType.VarChar, 100).Value = objCatalogodeSellosDigitales.PasswordAVC;
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

        public int Modificar(CatalogodeSellosDigitales objCatalogodeSellosDigitales)
        {
            int id;

            try
            {
                ValidarObjeto(objCatalogodeSellosDigitales, false);

                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_UPDATE_CatalogodeSellosDigitales_WEB", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdSelloDigital", SqlDbType.Int, 4).Value = objCatalogodeSellosDigitales.IdSelloDigital;
                    cmd.Parameters.Add("@UsuarioWebService", SqlDbType.VarChar, 13).Value = objCatalogodeSellosDigitales.UsuarioWebService;
                    cmd.Parameters.Add("@PasswordWebService", SqlDbType.VarChar, int.MaxValue).Value = objCatalogodeSellosDigitales.PasswordWebService;
                    cmd.Parameters.Add("@ArchivoKey", SqlDbType.VarBinary).Value = objCatalogodeSellosDigitales.ArchivoKey.Length == 0 ? null : objCatalogodeSellosDigitales.ArchivoKey;
                    cmd.Parameters.Add("@ArchivoCer", SqlDbType.VarBinary).Value = objCatalogodeSellosDigitales.ArchivoCer.Length == 0 ? null : objCatalogodeSellosDigitales.ArchivoCer;
                    cmd.Parameters.Add("@PasswordSello", SqlDbType.VarChar, int.MaxValue).Value = objCatalogodeSellosDigitales.PasswordSello;
                    cmd.Parameters.Add("@CertificadoBase64", SqlDbType.VarChar, int.MaxValue).Value = objCatalogodeSellosDigitales.ArchivoKey.Length == 0 ? null : Convert.ToBase64String(objCatalogodeSellosDigitales.ArchivoCer);
                    cmd.Parameters.Add("@OpenSSL", SqlDbType.Bit, 4).Value = objCatalogodeSellosDigitales.OpenSSL;
                    cmd.Parameters.Add("@NumeroDeSerie", SqlDbType.VarChar, 20).Value = objCatalogodeSellosDigitales.NumeroDeSerie;
                    cmd.Parameters.Add("@RFCConsulta", SqlDbType.VarChar, int.MaxValue).Value = objCatalogodeSellosDigitales.RFCConsulta;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, int.MaxValue).Value = objCatalogodeSellosDigitales.Email;
                    cmd.Parameters.Add("@FechaExpedicion", SqlDbType.DateTime).Value = objCatalogodeSellosDigitales.FechaExpedicion;
                    cmd.Parameters.Add("@FechaVigencia", SqlDbType.DateTime).Value = objCatalogodeSellosDigitales.FechaVigencia;
                    cmd.Parameters.Add("@Algoritmo", SqlDbType.VarChar, 15).Value = objCatalogodeSellosDigitales.Algoritmo;
                    cmd.Parameters.Add("@SellaMensajeria", SqlDbType.Bit).Value = objCatalogodeSellosDigitales.SellaMensajeria;
                    cmd.Parameters.Add("@CiecUsuario", SqlDbType.VarChar, 15).Value = objCatalogodeSellosDigitales.CiecUsuario;
                    cmd.Parameters.Add("@CiecPassword", SqlDbType.VarChar, 15).Value = objCatalogodeSellosDigitales.CiecPassword;
                    cmd.Parameters.Add("@UsuarioAVC", SqlDbType.VarChar, 100).Value = objCatalogodeSellosDigitales.UsuarioAVC;
                    cmd.Parameters.Add("@PasswordAVC", SqlDbType.VarChar, 100).Value = objCatalogodeSellosDigitales.PasswordAVC;
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

        public CatalogodeSellosDigitales Buscar(string UsuarioWebService)
        {
            CatalogodeSellosDigitales objSellos = new CatalogodeSellosDigitales();
            ScrambleNET scrambleNET = new ScrambleNET();
            try
            {
                using (con = new SqlConnection(SConexion))

                using (SqlCommand cmd = new("NET_SEARCH_CatalogodeSellosDigitales", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RFC", SqlDbType.VarChar, 13).Value = UsuarioWebService;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objSellos.IdSelloDigital = Convert.ToInt32(dr["IdSelloDigital"]);
                        objSellos.UsuarioWebService = dr["UsuarioWebService"].ToString();
                        objSellos.PasswordWebService = dr["PasswordWebService"].ToString();
                        objSellos.ArchivoKey = (byte[])dr["ArchivoKey"];
                        objSellos.ArchivoCer = (byte[])dr["ArchivoCer"];
                        objSellos.PasswordSello = scrambleNET.Scramble(dr["PasswordSello"].ToString());
                        objSellos.CertificadoBase64 = dr["CertificadoBase64"].ToString();
                        objSellos.Certificado = Convert.FromBase64String(dr["CertificadoBase64"].ToString());
                        objSellos.OpenSSL = Convert.ToBoolean(dr["OpenSSL"]);
                        objSellos.NumeroDeSerie = dr["NumeroDeSerie"].ToString();
                        objSellos.RFCConsulta = dr["RFCConsulta"].ToString();
                        objSellos.Email = dr["Email"].ToString();
                        objSellos.FechaExpedicion = Convert.ToDateTime(dr["FechaExpedicion"]);
                        objSellos.FechaVigencia = Convert.ToDateTime(dr["FechaVigencia"]);
                        objSellos.Algoritmo = dr["Algoritmo"].ToString();
                        objSellos.SellaMensajeria = Convert.ToBoolean(dr["SellaMensajeria"]);
                        objSellos.CiecUsuario = scrambleNET.Scramble(dr["CiecUsuario"].ToString());
                        objSellos.CiecPassword = scrambleNET.Scramble(dr["CiecPassword"].ToString());
                        objSellos.TokenAVC = dr["TokenAVC"].ToString();
                        objSellos.FechaToken = Convert.ToDateTime(dr["FechaToken"]);
                        objSellos.VigenciaAVC = Convert.ToDateTime(dr["VigenciaAVC"]);
                        objSellos.UsuarioAVC = dr["UsuarioAVC"].ToString();
                        objSellos.PasswordAVC = dr["PasswordAVC"].ToString();
                        objSellos.TokenAVCVencido = Convert.ToBoolean(dr["TokenAVCVencido"]);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objSellos;
        }

        public void ValidarObjeto(CatalogodeSellosDigitales objCatalogodeSellosDigitales, bool esInsertar)
        {
            if (objCatalogodeSellosDigitales.UsuarioWebService == null)
            {
                throw new Exception("Es necesario ingresar el Usuario Web Service");
            }

            if (objCatalogodeSellosDigitales.PasswordWebService == null)
            {
                throw new Exception("Es necesario ingresar el Password Web Service");
            }

            if (esInsertar == true)
            {
                if (objCatalogodeSellosDigitales.ArchivoKey == null)
                {
                    throw new Exception("Es necesario ingresar el Archivo Key");
                }

                if (objCatalogodeSellosDigitales.ArchivoCer == null)
                {
                    throw new Exception("Es necesario ingresar el Archivo Cer");
                }

            }
            if (objCatalogodeSellosDigitales.PasswordSello == null)
            {
                throw new Exception("Es necesario ingresar el Password del Sello");
            }

            if (objCatalogodeSellosDigitales.CertificadoBase64 == null)
            {
                throw new Exception("Es necesario ingresar el Certificado Base 64");
            }

            if (objCatalogodeSellosDigitales.NumeroDeSerie == null)
            {
                throw new Exception("Es necesario ingresar el Numero De Serie");
            }
            if (objCatalogodeSellosDigitales.RFCConsulta == null)
            {
                throw new Exception("Es necesario ingresar los RFC de Consulta");
            }
            if (objCatalogodeSellosDigitales.Email == null)
            {
                throw new Exception("Es necesario ingresar los e-mails");
            }

            if (objCatalogodeSellosDigitales.Algoritmo == null)
            {
                throw new Exception("Es necesario ingresar el Algoritmo");
            }
        }

        public ValidaCertificado ValidarCer(string NormbreArchivo, string sUsuarioWS, bool rdoClientes)
        {
            string txtArchivoCer = string.Empty;
            string txtFechaExpedicion = string.Empty;
            string txtFechaVigencia = string.Empty;
            string txtNoCertificado = string.Empty;
            string txtAlgoritmo = string.Empty;
            ValidaCertificado validaCertificado = new ValidaCertificado();

            try
            {
                txtArchivoCer = NormbreArchivo;
                string FileCerName = sUsuarioWS.Trim() + ".cer";
                char[] MyCharArray;
                string[] MiArray = new string[] { txtArchivoCer.Trim() };
                var objHelp = new Helper();
                var objCerticado = new Certificado();
                objCerticado = Helper.Main2(MiArray);

                txtFechaExpedicion = objCerticado.NotBefore;
                txtFechaVigencia = objCerticado.NotAfter;
                txtNoCertificado = "";
                MyCharArray = objCerticado.SerialNumber.ToCharArray();
                int i;
                var loopTo = Strings.Len(objCerticado.SerialNumber) - 1;
                for (i = 0; i <= loopTo; i += 2)
                {
                    //string HextoDecimal = System.Convert.ToString(Conversion.Val("&H30"));
                    string HextoDecimal = Convert.ToString(Conversion.Val("&H" + MyCharArray[i].ToString() + MyCharArray[i + 1].ToString()));
                    int intValue = int.Parse(HextoDecimal);

                    txtNoCertificado += Strings.Chr(intValue);
                }
                //txtNoCertificado += "&H" + Strings.Chr( MyCharArray[i] + MyCharArray[i + 1]);
                switch (objCerticado.SignatureAlgorithm.Trim().ToUpper())
                {
                    case "SHA1RSA":
                        {
                            txtAlgoritmo = "SHA-1";
                            break;
                        }
                    case "SHA256RSA":
                        {
                            txtAlgoritmo = "SHA256";
                            break;
                        }

                    default:
                        {
                            txtAlgoritmo = objCerticado.SignatureAlgorithm.Trim();
                            break;
                        }
                }
                validaCertificado.FechaExpedicion = txtFechaExpedicion;
                validaCertificado.FechaVigencia = txtFechaVigencia;
                validaCertificado.NoCertificado = txtNoCertificado;
                validaCertificado.Algoritmo = txtAlgoritmo;


                //if (rdoClientes)
                //{
                if (!objHelp.ValidaCertificadoVsCliente(objCerticado.Sujeto, sUsuarioWS))
                {
                    validaCertificado.FechaExpedicion = "";
                    validaCertificado.FechaVigencia = "";
                    validaCertificado.NoCertificado = "";
                    validaCertificado.Algoritmo = "";


                }
                //}

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return validaCertificado;
        }


        public void ValidaPasswordKey(string ArchivoBase64, string RFC, string Password)
        {
            try
            {
                UbicaciondeArchivos objUbicaciondeArchivos = new UbicaciondeArchivos();
                UbicaciondeArchivosRepository objUbicaciondeArchivosD = new UbicaciondeArchivosRepository(_configuration);

                objUbicaciondeArchivos = objUbicaciondeArchivosD.Buscar(180);

                string FileName = objUbicaciondeArchivos.Ubicacion + RFC + ".key";

                File.WriteAllBytes(FileName, Convert.FromBase64String(ArchivoBase64));

                try
                {
                    Chilkat.PrivateKey pkey = new Chilkat.PrivateKey();
                    string pkeyXml;

                    bool success;
                    Chilkat.Rsa rsa = new Chilkat.Rsa();

                    success = rsa.UnlockComponent("EHLECT.CB1112023_3H2GfVbS4R2j");
                    if (success != true)
                        throw new ArgumentException("Fallo la autenticación del Producto Chilkat, esto puede deberse a expiración de la licencia");

                    pkey.LoadPkcs8EncryptedFile(FileName, Password);
                    pkeyXml = pkey.GetXml();

                    if (pkeyXml == null)
                        throw new ArgumentException("Error en el password del Key");

                    File.Delete(FileName);
                }
                catch (Exception)
                {
                    File.Delete(FileName);
                    throw new ArgumentException("Error en el password del Key");
                }


            }
            catch (Exception Err)
            {

                throw new ArgumentException(Err.Message.ToString().Trim());
            }

        }
        public CatalogodeSellosDigitales BuscarMensajeria()
        {
            var objCatalogodeSellosDigitales = new CatalogodeSellosDigitales();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();

            SqlDataReader dr;
            var NewEncripta = new ScrambleNET();

            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CATALOGODESELLOSDIGITALES_MENSAJERIA";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objCatalogodeSellosDigitales.IdSelloDigital = Convert.ToInt32(dr["IdSelloDigital"]);
                    objCatalogodeSellosDigitales.ArchivoCer = Convert.FromBase64String(dr["ArchivoCer"].ToString());
                    objCatalogodeSellosDigitales.ArchivoKey = Convert.FromBase64String(dr["ArchivoKey"].ToString());
                    objCatalogodeSellosDigitales.UsuarioWebService = dr["UsuarioWebService"].ToString();
                    objCatalogodeSellosDigitales.PasswordWebService = dr["PasswordWebService"].ToString();
                    objCatalogodeSellosDigitales.PasswordSello = NewEncripta.Scramble(dr["PasswordSello"].ToString());
                    objCatalogodeSellosDigitales.CertificadoBase64 = dr["CertificadoBase64"].ToString().ToString();
                    objCatalogodeSellosDigitales.OpenSSL = Convert.ToBoolean(dr["OpenSSL"]);
                    objCatalogodeSellosDigitales.NumeroDeSerie = dr["NumeroDeSerie"].ToString();
                    objCatalogodeSellosDigitales.RFCConsulta = dr["RFCConsulta"].ToString();
                    objCatalogodeSellosDigitales.Email = dr["Email"].ToString();
                    objCatalogodeSellosDigitales.FechaExpedicion = Convert.ToDateTime(dr["FechaExpedicion"]);
                    objCatalogodeSellosDigitales.FechaVigencia = Convert.ToDateTime(dr["FechaVigencia"]);
                    objCatalogodeSellosDigitales.Algoritmo = dr["Algoritmo"].ToString();
                }
                else
                {
                    objCatalogodeSellosDigitales = default;
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

            return objCatalogodeSellosDigitales;
        }


        public CatalogodeSellosDigitales BuscarporPatente(string Patente)
        {
            CatalogodeSellosDigitales objSellos = new CatalogodeSellosDigitales();
            ScrambleNET scrambleNET = new ScrambleNET();
            try
            {
                using (con = new SqlConnection(SConexion))

                using (SqlCommand cmd = new("NET_LOAD_USUARIOANAM", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Patente", SqlDbType.VarChar, 4).Value = Patente;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objSellos.IdSelloDigital = Convert.ToInt32(dr["IdSelloDigital"]);
                        objSellos.UsuarioWebService = dr["UsuarioWebService"].ToString();
                        objSellos.PasswordWebService = dr["PasswordWebService"].ToString();
                        objSellos.ArchivoKey = (byte[])dr["ArchivoKey"];
                        objSellos.ArchivoCer = (byte[])dr["ArchivoCer"];
                        objSellos.PasswordSello = scrambleNET.Scramble(dr["PasswordSello"].ToString());
                        objSellos.CertificadoBase64 = dr["CertificadoBase64"].ToString();
                        objSellos.OpenSSL = Convert.ToBoolean(dr["OpenSSL"]);
                        objSellos.NumeroDeSerie = dr["NumeroDeSerie"].ToString();
                        objSellos.RFCConsulta = dr["RFCConsulta"].ToString();
                        objSellos.Email = dr["Email"].ToString();
                        objSellos.FechaExpedicion = Convert.ToDateTime(dr["FechaExpedicion"]);
                        objSellos.FechaVigencia = Convert.ToDateTime(dr["FechaVigencia"]);
                        objSellos.Algoritmo = dr["Algoritmo"].ToString();
                        objSellos.SellaMensajeria = Convert.ToBoolean(dr["SellaMensajeria"]);
                        objSellos.CiecUsuario = scrambleNET.Scramble(dr["CiecUsuario"].ToString());
                        objSellos.CiecPassword = scrambleNET.Scramble(dr["CiecPassword"].ToString());
                        objSellos.TokenAVC = dr["TokenAVC"].ToString();
                        objSellos.FechaToken = Convert.ToDateTime(dr["FechaToken"]);
                        objSellos.VigenciaAVC = Convert.ToDateTime(dr["VigenciaAVC"]);
                        objSellos.UsuarioAVC = dr["UsuarioAVC"].ToString();
                        objSellos.PasswordAVC = dr["PasswordAVC"].ToString();
                        objSellos.UsuarioANAM = scrambleNET.Scramble(dr["UsuarioANAM"].ToString());
                        objSellos.passwordAnam = scrambleNET.Scramble(dr["passwordAnam"].ToString());
                        objSellos.CURP_CONSULTA = dr["CURP_CONSULTA"].ToString();
                    }
                    else
                    {
                        objSellos = null!;
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objSellos;
        }
    }
}

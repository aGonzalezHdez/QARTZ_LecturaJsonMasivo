using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using Chilkat;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using StringBuilder = System.Text.StringBuilder;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Utilities.Helper
{
    public class Helper
    {
        //[SecurityPermission(SecurityAction.LinkDemand, Unrestricted = true)]
        public static Certificado Main2(string[] args)
        {
            var objCertificado = new Certificado();
            // Test for correct number of arguments.
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: CertInfo <filename>");
                return objCertificado;
            }
            try
            {
                //var x509 = new X509Certificate2();
                //X509Certificate2 x509 = new X509Certificate2();
                var x509 = new X509Certificate2(File.ReadAllBytes(args[0]));

                //byte[] rawData = ReadFile(args[0]);
                //x509.Import(rawData);
                // CertificadoHex = Bytes_To_String2(rawData, Size)


                objCertificado.Sujeto = x509.Subject;
                objCertificado.Issuer = x509.Issuer;
                objCertificado.Version = x509.Version.ToString();
                objCertificado.NotBefore = x509.NotBefore.ToString();
                objCertificado.NotAfter = x509.NotAfter.ToString();
                objCertificado.Thumbprint = x509.Thumbprint;
                objCertificado.SerialNumber = x509.SerialNumber;
                objCertificado.FriendlyName = x509.FriendlyName;
                objCertificado.EncodedKeyValue = x509.PublicKey.EncodedKeyValue.Format(true);
                objCertificado.Certificate = x509.ToString(true);
                objCertificado.SignatureAlgorithm = x509.SignatureAlgorithm.FriendlyName;
            }


            catch (DirectoryNotFoundException dnfExcept)
            {
                throw new Exception("Error: no se pudo encontrar el directorio especificado.");
            }
            catch (IOException ioExpcept)
            {
                throw new Exception("Error: No se pudo acceder a un archivo en el directorio.");
            }
            catch (NullReferenceException nrExcept)
            {
                throw new Exception("El archivo debe ser un archivo .cer. El programa no tiene acceso a ese tipo de archivo.");

            }

            return objCertificado;
        }

        public string PADL(string cadena, int longitud, string relleno, bool esNumero)
        {
            if (cadena == null) cadena = "";
            if (relleno == null || relleno == "") relleno = " ";

            if (esNumero)
            {
                int pnPosicion = cadena.IndexOf(".");
                if (pnPosicion == -1)
                {
                    cadena += ".00";
                }
                else
                {
                    if ((cadena.Trim().Length - pnPosicion) == 1)
                    {
                        cadena += "0";
                    }
                }
            }

            int lenCadena = cadena.Length;
            if (lenCadena != longitud)
            {
                // Generar el relleno necesario
                string padding = "";
                for (int i = 2; i <= (longitud - lenCadena); i++)
                {
                    padding += relleno.Substring(0, 1);
                }

                cadena = padding + cadena;
            }

            return cadena;
        }


        public string EliminaComas(string s)
        {
            StringBuilder straux = new StringBuilder();

            foreach (char c in s)
            {
                if (c != ',')
                {
                    straux.Append(c);
                }
            }

            return straux.ToString();
        }

        public async Task<bool> CalcularImpuestos(string Referencia, string MyConnectionString)
        {
            bool Calculo = false;



            var Cierre = new ProcessStartInfo();
            try
            {

                Cierre.FileName = @"C:\CASAWIN\CSAAIWIN-SQL\CMDCierreSaai.exe";
                Cierre.Arguments = " Referencia=" + Referencia.Trim();
                Cierre.UseShellExecute = true;
                Cierre.WindowStyle = ProcessWindowStyle.Hidden;
                var proc = Process.Start(Cierre);
                proc.WaitForExit();
                proc.Dispose();

                Calculo = true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return Calculo;
        }
        public bool DownLoadFTP(string RemotePath, string FilePath, string User, string Pass)
        {
            try
            {
                bool result = false;
                var ftp = (FtpWebRequest)WebRequest.Create(RemotePath);
                // Fijo las credenciales, usuario y contraseña
                ftp.Credentials = new NetworkCredential(User, Pass);
                // Declaro que la conexión no se cierra tras terminar con la solicitud
                ftp.KeepAlive = false;
                ftp.UsePassive = true;
                // ftp.Timeout = 600000;

                ftp.UseBinary = true;
                // Defino la acción de descarga
                ftp.Method = WebRequestMethods.Ftp.DownloadFile;

                // Tomo la respuesta del Servidor Ftp
                using (var response = (FtpWebResponse)ftp.GetResponse())
                {
                    // Asocio la respuesta al stream local
                    using (var responseStream = response.GetResponseStream())
                    {
                        // Bucle que lee y escribe en el fichero
                        var filePermission = new FileIOPermission(FileIOPermissionAccess.Write, FilePath);
                        using (var fileStream = new FileStream(FilePath, FileMode.Create))
                        {
                            var buffer = new byte[2048];
                            int bytesRead;
                            while ((bytesRead = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                fileStream.Write(buffer, 0, bytesRead);
                            }
                        }
                        // Cierro las variables
                        responseStream.Close();
                    }
                    // Cierro la variable
                    response.Close();
                }
                // Retorno verdadero
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        public async Task<bool> DownLoadFTPAsync(string remotePath, string filePath, string user, string pass)
        {
            try
            {
                bool result = false;

                var ftpRequest = (FtpWebRequest)WebRequest.Create(remotePath);
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.KeepAlive = false;
                ftpRequest.UsePassive = true;
                ftpRequest.UseBinary = true;
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                string nombreArchivo = Path.GetFileName(filePath);

                using (var response = (FtpWebResponse)await ftpRequest.GetResponseAsync())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream == null)
                        {
                            throw new InvalidOperationException("Error al obtener el flujo de respuesta del servidor FTP.");
                        }

                        var buffer = new byte[2048];
                        int bytesRead;

                        using (var fileStream = new FileStream(filePath, FileMode.Create, System.IO.FileAccess.Write, FileShare.None, buffer.Length, useAsync: true))
                        {
                            while ((bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                await fileStream.WriteAsync(buffer, 0, bytesRead);
                            }
                        }
                    }
                }

                result = true;
                return result;
            }
            catch (WebException ex)
            {
                if (ex.Response is FtpWebResponse ftpResponse && ftpResponse.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    throw new FileNotFoundException($"El archivo '{Path.GetFileName(filePath)}' no se encuentra en el servidor FTP.");
                }
                throw new Exception("Error de conexión con el servidor FTP: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al descargar el archivo: " + ex.Message);
            }

        }


        public bool ValidaCertificadoVsCliente(string Sujeto, string RfcCliente)
        {
            bool ValidaCertificadoVsClienteRet = default;

            try
            {
                foreach (Match m in Regex.Matches(Sujeto, "^.*$", RegexOptions.Multiline))
                {
                    // Carga a un arreglo de "strings" los campos de la línea leída separados por "coma"
                    string[] Arreglo = Regex.Split(m.ToString(), ",");
                    int i;
                    int pnPosicion;
                    string RfcCert;
                    string TipoSello;

                    var loopTo = Arreglo.Length - 1;
                    for (i = 0; i <= loopTo; i++)
                    {

                        bool exitFor = false;
                        switch (i)
                        {

                            case 0:
                                {
                                    pnPosicion = Strings.InStr(Arreglo[i], "=");
                                    TipoSello = Strings.Mid(Arreglo[i], pnPosicion + 1);

                                    TipoSello = EliminaComillas(TipoSello);
                                    if (Strings.Trim(TipoSello) == "COVE" | Strings.Trim(TipoSello) == "VUCEM")
                                    {
                                        ValidaCertificadoVsClienteRet = true;
                                    }

                                    else
                                    {
                                        ValidaCertificadoVsClienteRet = false;
                                        throw new Exception("ERROR CRITICO, El sello no es exclusivo para COVE o VUCEM.");
                                    }

                                    break;
                                }

                            case 1:
                                {
                                    break;
                                }
                            case 2:
                                {
                                    pnPosicion = Strings.InStr(Arreglo[i], "=");
                                    RfcCert = Strings.Mid(Arreglo[i], pnPosicion + 1, 13);
                                    if ((RfcCliente.Trim() ?? "") != (RfcCert.Trim() ?? ""))
                                    {
                                        ValidaCertificadoVsClienteRet = false;
                                        throw new Exception("ERROR CRITICO, El sello no corresponde al cliente, favor de verificar.");
                                        exitFor = true;
                                        break;
                                    }
                                    else
                                    {
                                        ValidaCertificadoVsClienteRet = true;
                                        exitFor = true;
                                        break;
                                    }
                                }
                        }

                        if (exitFor)
                        {
                            break;

                        }

                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
            return ValidaCertificadoVsClienteRet;
        }

        public string EliminaComillas(string s)
        {
            string EliminaComillasRet = default;

            string straux;
            long i;
            int ExistenComillas = 0;

            straux = "";
            ExistenComillas = Strings.InStr(s, "\"");
            if (ExistenComillas != 0)
            {

                var loopTo = (long)Strings.Len(s);
                for (i = 1L; i <= loopTo; i++)
                {
                    if (Strings.Mid(s, (int)i, 1) != "\"")
                    {
                        straux = straux + Strings.Mid(s, (int)i, 1);
                    }
                }
            }
            else
            {

                straux = s;

            }

            EliminaComillasRet = Strings.Trim(straux);
            return EliminaComillasRet;

        }
        public List<int> Parsear(string Valor)
        {
            List<int> lstValores = new List<int>();
            if (Valor.Trim() == "")
            {
                lstValores.Clear();
                return lstValores;
            }

            if (Valor.Trim() == "0")
            {
                lstValores.Clear();
                return lstValores;
            }

            string[] Arreglo = null;
            foreach (Match m in Regex.Matches(Valor, "^.*$", RegexOptions.Multiline))
                // Carga a un arreglo de "strings" los campos de la línea leída separados por "coma"
                Arreglo = Regex.Split(m.ToString(), ",");

            foreach (string item in Arreglo)
                lstValores.Add(Convert.ToInt32(item));
            return lstValores;
        }
        public void ConvertirByteaArchivo(object BLOBObject, string Ubicacion)
        {
            try
            {
                File.WriteAllBytes(Ubicacion, (byte[])BLOBObject);
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible descargar el archivo");
            }
        }
        public byte[] ReadFileString(string Cadena)
        {
            int size;
            size = Strings.Len(Cadena);

            var data = new byte[size + 1];
            data = Encoding.ASCII.GetBytes(Cadena.ToCharArray());

            return data;
        }

        public byte[] GenerarFirmaCadenaByte(CatalogodeSellosDigitales lSelloDigital, string CadenaOriginal)
        {
            byte[] firmaByte = null;

            string CadenaOriginalEnHexa = string.Empty;


            if (lSelloDigital.Equals(default))
            {
                throw new Exception("Imposible generar cadenar original, usted debera revisar todos los datos de sus factura y de las partidas de factura antes de continuar");
            }

            if (CadenaOriginal.Length == 0)
            {
                throw new Exception("Imposible generar cadenar original, usted debera revisar todos los datos de sus factura y de las partidas de factura antes de continuar");
            }

            if (CadenaOriginal.Length == 0)
            {
                throw new Exception("Imposible generar cadenar original, usted debera revisar todos los datos de sus factura y de las partidas de factura antes de continuar");
            }
            string PathKey = string.Empty;
            try
            {
                var pkey = new Chilkat.PrivateKey();
                var NewEncripta = new ScrambleNET();

                string MisDocumentos = string.Empty;
                MisDocumentos = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

                string PathSellos = MisDocumentos + @"\ExperttiTmp\";

                if (!Directory.Exists(PathSellos))
                {
                    Directory.CreateDirectory(PathSellos);
                }

                PathKey = PathSellos + lSelloDigital.UsuarioWebService + ".Key";

                var objHelp = new Helper();
                objHelp.ConvertirByteaArchivo(lSelloDigital.ArchivoKey, PathKey);


                // Load the private key from an RSA PEM file:

                pkey.LoadPkcs8EncryptedFile(PathKey.Trim(), lSelloDigital.PasswordSello);



                bool success;

                string pkeyXml;
                // Get the private key in XML format:
                pkeyXml = pkey.GetXml();
                // Con el RFC de TetraaPak pkeyXML regresa vacía


                var rsa = new Chilkat.Rsa();

                // Any string argument automatically begins the 30-day trial.
                // success = rsa.UnlockComponent("GRUPOERSA_EYXykNaw4ZnG")
                success = rsa.UnlockComponent("EHLECT.CB1112023_3H2GfVbS4R2j");
                if (success != true)
                {
                    throw new Exception("RSA component unlock failed");
                    // MsgBox("RSA component unlock failed")

                }

                // Import the private key into the RSA component:
                success = rsa.ImportPrivateKey(pkeyXml);
                if (success != true)
                {
                    throw new Exception(rsa.LastErrorText);

                    // MsgBox(rsa.LastErrorText)

                }

                rsa.EncodingMode = "hex";
                rsa.LittleEndian = false;


                CadenaOriginalEnHexa = rsa.SignStringENC(CadenaOriginal, lSelloDigital.Algoritmo);

                byte[] cadenaOriginalbyte = null;

                cadenaOriginalbyte = ReadFileString(CadenaOriginal);
                firmaByte = rsa.SignBytes(cadenaOriginalbyte, lSelloDigital.Algoritmo);
            }

            catch (Exception ex)
            {
                File.Delete(PathKey);
                throw new Exception("Error :" + ex.ToString());
            }

            File.Delete(PathKey);
            return firmaByte;
        }
        public byte[] GenerarFirmaCadenaByte(CatalogodeSellosDigitales lSelloDigital, string pMisDocumentos, string CadenaOriginal)
        {
            byte[] firmaByte = null;

            string CadenaOriginalEnHexa = string.Empty;


            if (lSelloDigital.Equals(default))
            {
                throw new Exception("Imposible generar cadenar original, usted debera revisar todos los datos de sus factura y de las partidas de factura antes de continuar");
            }

            if (CadenaOriginal.Length == 0)
            {
                throw new Exception("Imposible generar cadenar original, usted debera revisar todos los datos de sus factura y de las partidas de factura antes de continuar");
            }

            if (CadenaOriginal.Length == 0)
            {
                throw new Exception("Imposible generar cadenar original, usted debera revisar todos los datos de sus factura y de las partidas de factura antes de continuar");
            }
            string PathKey = string.Empty;
            try
            {
                var pkey = new Chilkat.PrivateKey();
                var NewEncripta = new ScrambleNET();



                string PathSellos = pMisDocumentos + @"\";

                if (!Directory.Exists(PathSellos))
                {
                    Directory.CreateDirectory(PathSellos);
                }

                PathKey = PathSellos + lSelloDigital.UsuarioWebService + ".Key";

                var objHelp = new Helper();
                objHelp.ConvertirByteaArchivo(lSelloDigital.ArchivoKey, PathKey);


                // Load the private key from an RSA PEM file:

                pkey.LoadPkcs8EncryptedFile(PathKey.Trim(), lSelloDigital.PasswordSello);



                bool success;

                string pkeyXml;
                // Get the private key in XML format:
                pkeyXml = pkey.GetXml();

                var rsa = new Chilkat.Rsa();

                // Any string argument automatically begins the 30-day trial.
                // success = rsa.UnlockComponent("GRUPOERSA_EYXykNaw4ZnG")
                success = rsa.UnlockComponent("EHLECT.CB1112023_3H2GfVbS4R2j");
                if (success != true)
                {
                    throw new Exception("RSA component unlock failed");
                    // MsgBox("RSA component unlock failed")

                }

                // Import the private key into the RSA component:
                success = rsa.ImportPrivateKey(pkeyXml);
                if (success != true)
                {
                    throw new Exception(rsa.LastErrorText);

                    // MsgBox(rsa.LastErrorText)

                }

                rsa.EncodingMode = "hex";
                rsa.LittleEndian = false;



                CadenaOriginalEnHexa = rsa.SignStringENC(CadenaOriginal, lSelloDigital.Algoritmo);


                byte[] cadenaOriginalbyte = null;

                cadenaOriginalbyte = ReadFileString(CadenaOriginal);
                firmaByte = rsa.SignBytes(cadenaOriginalbyte, lSelloDigital.Algoritmo);
            }




            catch (Exception ex)
            {
                File.Delete(PathKey);
                throw new Exception("Error :" + ex.ToString());
            }

            File.Delete(PathKey);
            return firmaByte;
        }
        public bool UnLoadFtp2(string Miurl, string MiUsuario, string MiPassword, string MiArchivo)
        {

            string miUri = Miurl; // Es la dirección de destino incluyendo en nombre del archivo con su extencion ftp://ftp.midominio.com/carpeta/fichero.jpg"
            FtpWebRequest miRequest = (FtpWebRequest)WebRequest.Create(miUri);
            miRequest.Credentials = new NetworkCredential(MiUsuario, MiPassword);
            miRequest.Method = WebRequestMethods.Ftp.UploadFile;
            try
            {
                byte[] bFile = File.ReadAllBytes(MiArchivo); // Es la Ruta del archivo que intento subir al ftp
                System.IO.Stream miStream = miRequest.GetRequestStream();
                miStream.Write(bFile, 0, bFile.Length);
                miStream.Close();
                miStream.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ". El Archivo no pudo ser enviado.");
                return false;
            }



        }

        public void UnloadFtp(string RemotePath, string FilePath, string lUsuarioftp, string lPasswrodftp)
        {
            FileStream fs = new(RemotePath, FileMode.Open, System.IO.FileAccess.Read, FileShare.Read);
            using (fs)
            {
                //FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(RemotePath);
                //ftp.Credentials = new NetworkCredential(lUsuarioftp, lPasswrodftp);
                //ftp.KeepAlive = false;
                //ftp.Method = WebRequestMethods.Ftp.UploadFile;
                //ftp.UseBinary = false;
                //ftp.ContentLength = fs.Length;
                //ftp.Proxy = null;
                //fs.Position = 0;


                //Int16 buffLength = 2048;
                //byte[] buff = new byte[buffLength + 1];
                //int contentLen;
                //Stream strm = ftp.GetRequestStream();
                //using ((strm))
                //{
                //    contentLen = fs.Read(buff, 0, buffLength);
                //    while ((contentLen != 0))
                //    {
                //        strm.Write(buff, 0, contentLen);
                //        contentLen = fs.Read(buff, 0, buffLength);
                //    }
                //}
            }
        }

        public string ConvertirGrupoaCadena(int IdGrupo)
        {
            string cadena = string.Empty;

            //    List<CatalogoDeGruposdeCorreo> lstGpoCorreo = new List<CatalogoDeGruposdeCorreo>();
            //    CatalogoGruposdeCorreoData objGpoCorreoD = new CatalogoGruposdeCorreoData();
            //    try
            //    {
            //        lstGpoCorreo = objGpoCorreoD.Cargar(IdGrupo);


            //        foreach (CatalogoDeGruposdeCorreo itemEmail in lstGpoCorreo)
            //            cadena = cadena + itemEmail.Email.Trim() + ";";
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new ArgumentException("ConvertirGrupoaCadena :" + ex.Message);
            //    }

            return cadena;
        }

    }
}

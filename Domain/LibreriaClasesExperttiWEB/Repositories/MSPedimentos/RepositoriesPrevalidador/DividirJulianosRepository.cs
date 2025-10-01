using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using LibreriaClasesAPIExpertti.Services.S3;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPrevalidador
{
    public class DividirJulianosRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;


        public DividirJulianosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public string MyReadFile(string myPath)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(myPath);
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
        }
        public string ExtraerFechaDePago(string contenido)
        {
            string posicion;
            int miTipo;
            string miFecha = "";

            foreach (Match m in Regex.Matches(contenido, "^.*$", RegexOptions.Multiline))
            {
                string[] arreglo = Regex.Split(m.ToString(), "^");
                posicion = Mid(arreglo[1], 1, 3);

                switch (posicion)
                {
                    case "701":
                        miFecha = Mid(arreglo[1], 25, 8);
                        return miFecha;

                    case "506":
                        miTipo = Convert.ToInt32(Mid(arreglo[1], 13, 1));
                        if (miTipo == 2)
                        {
                            miFecha = Mid(arreglo[1], 15, 8);
                            return miFecha;
                        }
                        break;

                    case "801":
                        return miFecha;
                }
            }
            return miFecha;
        }

        // Helper method for Mid equivalent in C#
        private string Mid(string input, int startIndex, int length)
        {
            if (startIndex <= 0 || startIndex > input.Length)
            {
                return "";
            }

            if (startIndex + length - 1 > input.Length)
            {
                return input.Substring(startIndex - 1);
            }

            return input.Substring(startIndex - 1, length);
        }

        public void CreaNuevoArchivoDePagoElectronicoTipoA(string newFile, string miRegistro, string miPatente, string miPedimento)
        {
            string nombreDeArchivo = "";

            if (!File.Exists(newFile))
            {
                nombreDeArchivo = Path.GetFileName(newFile);

                using (FileStream strStreamErr = File.OpenWrite(newFile))
                using (StreamWriter strStreamWriterErr = new StreamWriter(strStreamErr, System.Text.Encoding.ASCII))
                {
                    strStreamWriterErr.Write(miRegistro);
                }

                Insertar(nombreDeArchivo, "A", miPatente, miPedimento);
            }
            else
            {
                nombreDeArchivo = Path.GetFileName(newFile);
                Insertar(nombreDeArchivo, "A", miPatente, miPedimento);
            }
        }

        public void CreaNuevoArchivoDePagoElectronicoTipoE(string newFile, string miRegistro, string miPatente, string miPedimento)
        {
            string nombreDeArchivo = "";
            if (!File.Exists(newFile))
            {
                nombreDeArchivo = Path.GetFileName(newFile);
                using (FileStream strStreamErr = File.OpenWrite(newFile))
                using (StreamWriter strStreamWriterErr = new StreamWriter(strStreamErr, System.Text.Encoding.ASCII))
                {
                    strStreamWriterErr.Write(miRegistro);
                }
                Insertar(nombreDeArchivo, "E", miPatente, miPedimento);
            }
            else
            {
                nombreDeArchivo = Path.GetFileName(newFile);
                Insertar(nombreDeArchivo, "E", miPatente, miPedimento);
            }
        }

        public void LeerArchivoDePECA_E(string fileName, string lPathLocal)
        {
            Regex re = new Regex(@"^10.{2}(?<Aduana>.{2}).{0}(?<Patente>.{0,4}).{0}(?<Pedimento>.{7}).{4}(?<Rfc>.{13}).+?(?=^10|(?-m)$)",
                                  RegexOptions.Multiline | RegexOptions.Singleline);

            MatchCollection mc;
            string pedimento = "";
            string patente = "";
            string aduana = "";
            string rfc = "";
            string todoElRegistro = "";
            string nombreDeArchivo = "";
            string extencionDeArchivo = "";
            string myReferencia = "";

            // Get file name and extension
            nombreDeArchivo = Path.GetFileNameWithoutExtension(fileName);
            nombreDeArchivo = nombreDeArchivo.Substring(5, 3);  // Equivalent to VB's Mid function
            extencionDeArchivo = Path.GetExtension(fileName);

            // Read the content of the file
            string contenidoDelFicheroOriginal = MyReadFile(fileName);

            // Perform regex matching
            mc = re.Matches(contenidoDelFicheroOriginal);

            // Process each match
            for (int i = 0; i < mc.Count; i++)
            {
                todoElRegistro = mc[i].ToString();
                patente = mc[i].Groups["Patente"].ToString();
                pedimento = mc[i].Groups["Pedimento"].ToString();
                rfc = mc[i].Groups["Rfc"].ToString();
                aduana = mc[i].Groups["Aduana"].ToString();

                // Fetch reference
                myReferencia = NET_BUSCA_REFERENCIA(patente, pedimento, aduana);

                // Create new file
                CreaNuevoArchivoDePagoElectronicoTipoE(
                    Path.Combine(lPathLocal, $"E{aduana}-{patente}-{pedimento}-{nombreDeArchivo}{extencionDeArchivo}"),
                    todoElRegistro, patente, pedimento);
            }
        }
        public string ReadFileText(string path)
        {
            string content;
            using (StreamReader sr = new StreamReader(path))
            {
                content = sr.ReadToEnd();
            }
            return content;
        }
        public async Task<string> BuscarFirmaDePagoElectronicoAsync(string fileName, string miPatente, string miPedimento, string miRegexA,
                                                            string pathDestino, bool activaPECA, CatalogoDeUsuarios gObjUsuario)
        {
            string vBuscarFirmaDePagoElectronico = pathDestino;

            try
            {
                string miRegex = "^.*$";
                bool existio = false;

                foreach (Match m in Regex.Matches(ReadFileText(fileName), miRegex, RegexOptions.Multiline))
                {
                    string[] arreglo = Regex.Split(m.ToString(), "^");
                    string nombreJuliano = Path.GetFileName(fileName);
                    nombreJuliano = nombreJuliano.Substring(5, 3); // Equivalent to Mid in VB
                    string extencionDeArchivo = Path.GetExtension(fileName);
                    existio = false;

                    for (int i = 0; i < 2; i++)
                    {
                        if (arreglo[i].Length > 80)
                        {
                            string todoElRegistro = arreglo[i];
                            string miNombreDeArchivo = "";
                            string aduana = "", diaPago = "", horPago = "", firPago = "", numOper = "", impuestos = "", transicion = "", referencia = "", fechaCorta = "", fechaLarga = "";
                            string lmipedimento = "", lmiPatente = "", lRutaDestino = "";

                            aduana = arreglo[i].Substring(25, 2);
                            lmipedimento = arreglo[i].Substring(36, 7);
                            lmiPatente = arreglo[i].Substring(28, 8);
                            fechaCorta = arreglo[i].Substring(75, 8);
                            fechaLarga = fechaCorta.Substring(0, 2) + "/" + fechaCorta.Substring(2, 2) + "/" + fechaCorta.Substring(4, 4) + " " + arreglo[i].Substring(83, 8);

                            diaPago = fechaLarga;
                            firPago = fechaCorta.Substring(6, 2) + arreglo[i].Substring(83, 8);
                            numOper = arreglo[i].Substring(91, 14);
                            horPago = arreglo[i].Substring(83, 8);
                            impuestos = arreglo[i].Substring(61, 14);
                            transicion = arreglo[i].Substring(105, 20);
                            referencia = NET_BUSCA_REFERENCIA(lmiPatente.Trim(), lmipedimento.Trim(), aduana);

                            lRutaDestino = Path.Combine(pathDestino, lmiPatente.Trim(), fechaCorta.Substring(2, 4), fechaCorta.Substring(0, 2));
                            if (!Directory.Exists(lRutaDestino))
                            {
                                Directory.CreateDirectory(lRutaDestino);
                            }

                            if (miPatente.Trim() == lmiPatente.Trim() && miPedimento.Trim() == lmipedimento.Trim())
                            {
                                string newFile = Path.Combine(lRutaDestino, $"A{aduana}-{lmiPatente.Trim()}-{lmipedimento.Trim()}-{nombreJuliano}{extencionDeArchivo}");
                                CreaNuevoArchivoDePagoElectronicoTipoA(newFile, todoElRegistro, lmiPatente.Trim(), lmipedimento.Trim());
                                miNombreDeArchivo = $"A{aduana}-{lmiPatente.Trim()}-{lmipedimento.Trim()}-{nombreJuliano}{extencionDeArchivo}";

                                InsertarBitacorDeJulianos(referencia, aduana, lmiPatente.Trim(), lmipedimento.Trim(), fechaLarga, lRutaDestino, 0, 0, 0, 1, "", "", "", miNombreDeArchivo);

                                vBuscarFirmaDePagoElectronico = lRutaDestino;

                                try
                                {
                                    int id = await SubiraS3Async(referencia, Path.Combine(lRutaDestino, miNombreDeArchivo), gObjUsuario);
                                    if (id > 0)
                                    {
                                        ModificarBitacorDeJulianos(referencia, id, 4);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Handle the exception (you may log it or rethrow it)
                                }

                                existio = true;
                                break;
                            }
                            else
                            {
                                existio = false;
                            }
                        }
                    }

                    if (existio)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (you may log it or rethrow it)
            }

            return vBuscarFirmaDePagoElectronico;
        }

        public async Task<string> BuscarFirmaDePagoElectronico(string FileName, string MiPatente, string MiPedimento, string MiRegexA, string PathDestino, bool ActivaPECA, CatalogoDeUsuarios GObjUsuario)
        {
            string vBuscarFirmaDePagoElectronico = PathDestino;

            try
            {
                string MiRegex = "^.*$";
                bool Existio = false;

                foreach (Match m in Regex.Matches(ReadFileText(FileName), MiRegex, RegexOptions.Multiline))
                {
                    string[] Arreglo = Regex.Split(m.ToString(), "^");
                    string NombreJuliano = Path.GetFileName(FileName);
                    NombreJuliano = NombreJuliano.Substring(5, 3);
                    string ExtencionDeArchivo = Path.GetExtension(FileName);
                    Existio = false;

                    for (int i = 0; i <= 1; i++)
                    {
                        if (Arreglo[i].Length > 80)
                        {
                            string TodoElRegistro = Arreglo[i];
                            string MiNombreDeArchivo = "";
                            string Aduana = Arreglo[i].Substring(25, 2);
                            string lMipedimento = Arreglo[i].Substring(36, 7);
                            string lMiPatente = Arreglo[i].Substring(28, 8);
                            string FechaCorta = Arreglo[i].Substring(75, 8);
                            string FechaLarga = FechaCorta.Substring(0, 2) + "/" + FechaCorta.Substring(2, 2) + "/" + FechaCorta.Substring(4, 4) + " " + Arreglo[i].Substring(83, 8);
                            string DIA_PAGO = FechaLarga;
                            string FIR_PAGO = FechaCorta.Substring(6, 2) + Arreglo[i].Substring(83, 8);
                            string NUM_OPER = Arreglo[i].Substring(91, 14);
                            string HOR_PAGO = Arreglo[i].Substring(83, 8);
                            string IMPUESTOS = Arreglo[i].Substring(61, 14);
                            string TRANSICION = Arreglo[i].Substring(105, 20);
                            string Referencia = NET_BUSCA_REFERENCIA(lMiPatente.Trim(), lMipedimento.Trim(), Aduana);
                            string lRutaDestino = Path.Combine(PathDestino, lMiPatente.Trim(), FechaCorta.Substring(2, 6), FechaCorta.Substring(0, 2));

                            if (!Directory.Exists(lRutaDestino.Trim()))
                            {
                                Directory.CreateDirectory(lRutaDestino);
                            }

                            if (MiPatente.Trim() == lMiPatente.Trim() && MiPedimento.Trim() == lMipedimento.Trim())
                            {
                                CreaNuevoArchivoDePagoElectronicoTipoA(Path.Combine(lRutaDestino, "A" + Aduana + "-" + lMiPatente.Trim() + "-" + lMipedimento.Trim() + "-" + NombreJuliano + ExtencionDeArchivo), TodoElRegistro, lMiPatente.Trim(), lMipedimento.Trim());
                                MiNombreDeArchivo = "A" + Aduana + "-" + lMiPatente.Trim() + "-" + lMipedimento.Trim() + "-" + NombreJuliano + ExtencionDeArchivo;

                                InsertarBitacorDeJulianos(Referencia, Aduana, lMiPatente.Trim(), lMipedimento.Trim(), FechaLarga, lRutaDestino, 0, 0, 0, 1, "", "", "", MiNombreDeArchivo);

                                vBuscarFirmaDePagoElectronico = lRutaDestino;

                                try
                                {
                                    int ID = await SubiraS3Async(Referencia, Path.Combine(lRutaDestino, MiNombreDeArchivo), GObjUsuario);
                                    if (ID > 0)
                                    {
                                        ModificarBitacorDeJulianos(Referencia, ID, 4);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Manejo de la excepción
                                }

                                Existio = true;
                                break;
                            }
                            else
                            {
                                Existio = false;
                            }
                        }
                    }

                    if (Existio) break;
                }
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
            }

            return vBuscarFirmaDePagoElectronico;
        }

        public async Task LeerArchivosDePagoCentralizadoA(string fileName, string lPathLocal, bool activaPECA, CatalogoDeUsuarios gObjUsuario)
        {
            if (activaPECA)
            {
                var re = new Regex(@"^30.{0}(?<Aduana>.{2}).{0}(?<Patente>.{0,4}).{0}(?<Pedimento>.{7}).{0}(?<Rfc>.{13}).{22}(?<Fecha>.{8}).+?(?=^30|(?-m)$)", RegexOptions.Multiline | RegexOptions.Singleline);
                var mc = re.Matches(MyReadFile(fileName));
                string nombreDeArchivo = Path.GetFileNameWithoutExtension(fileName).Substring(5, 3);
                string extencionDeArchivo = Path.GetExtension(fileName);

                for (int i = 0; i < mc.Count; i++)
                {
                    string todoElRegistro = mc[i].ToString();
                    string patente = mc[i].Groups["Patente"].ToString();
                    string pedimento = mc[i].Groups["Pedimento"].ToString();
                    string rfc = mc[i].Groups["Rfc"].ToString().Trim();
                    string aduana = mc[i].Groups["Aduana"].ToString();
                    string miFecha = mc[i].Groups["Fecha"].ToString();

                    if (miFecha == "00000000")
                        continue;

                    string nuevoPath = Path.Combine(lPathLocal, patente, miFecha.Substring(2, 6), miFecha.Substring(0, 2));
                    if (!Directory.Exists(nuevoPath))
                        Directory.CreateDirectory(nuevoPath);

                    string myReferencia = NET_BUSCA_REFERENCIA(patente, pedimento, aduana);
                    string fechaDePago = $"{miFecha.Substring(0, 2)}/{miFecha.Substring(2, 2)}/{miFecha.Substring(4, 4)}";

                    string documentoA = Path.Combine(nuevoPath, $"A{aduana}-{patente}-{pedimento}-{nombreDeArchivo}{extencionDeArchivo}");
                    CreaNuevoArchivoDePagoElectronicoTipoA(documentoA, todoElRegistro, patente, pedimento);
                    InsertarBitacorDeJulianos(myReferencia, aduana, patente, pedimento, fechaDePago, nuevoPath, 0, 0, 0, 1, "", "", "", documentoA);

                    try
                    {
                        int id = await SubiraS3Async(myReferencia, documentoA, gObjUsuario);
                        if (id > 0)
                        {
                            ModificarBitacorDeJulianos(myReferencia, id, 4);
                        }
                    }
                    catch (Exception)
                    {
                        // Handle the exception as needed
                    }

                    string soloRuta = Path.GetDirectoryName(fileName) + "\\";
                    string soloNombre = Path.GetFileName(fileName).ToUpper().Replace("A", "E");
                    string archivoE = Path.Combine(soloRuta, soloNombre);
                    LeerArchivoDePECA_E(archivoE, nuevoPath);
                }
            }
            else
            {
                string miRegexE = @"^4{0}(?<BANCO>.{5}).{0}(?<LINEA>.{0,20}).{0}(?<ADUANA>.{0,3}).{0}(?<PATENTE>.{0,8}).{0}(?<PEDIMENTO>.{0,7}).{0}(?<RFC>.{0,13}).{0}(?<ID>.{0,5}).{0}(?<IMPUESTOS>.{0,14})";
                var re = new Regex(miRegexE, RegexOptions.Multiline | RegexOptions.Singleline);
                var mc = re.Matches(MyReadFile(fileName));

                string nombreDeArchivo = Path.GetFileNameWithoutExtension(fileName).Substring(5, 3);
                string extencionDeArchivo = Path.GetExtension(fileName);
                string fechaLarga = DateTime.Now.ToString("dd/MM/yyyy");

                for (int i = 0; i < mc.Count; i++)
                {
                    string todoElRegistro = mc[i].ToString().Trim();
                    if (todoElRegistro.Length >= 75)
                    {
                        string patente = mc[i].Groups["PATENTE"].ToString().Trim();
                        string pedimento = mc[i].Groups["PEDIMENTO"].ToString().Trim();
                        string rfc = mc[i].Groups["RFC"].ToString();
                        string aduana = mc[i].Groups["ADUANA"].ToString().Substring(0, 2);

                        string myReferencia = NET_BUSCA_REFERENCIA(patente, pedimento, aduana);
                        string lRutaDestino = await BuscarFirmaDePagoElectronico(fileName, patente, pedimento, "", lPathLocal, activaPECA, gObjUsuario);

                        if (!string.IsNullOrEmpty(lRutaDestino))
                        {
                            string nuevoArchivo = Path.Combine(lRutaDestino, $"E{aduana}-{patente}-{pedimento}-{nombreDeArchivo}{extencionDeArchivo}");
                            string miNombreDeArchivo = $"E{aduana}-{patente}-{pedimento}-{nombreDeArchivo}{extencionDeArchivo}";

                            CreaNuevoArchivoDePagoElectronicoTipoE(nuevoArchivo, todoElRegistro, patente, pedimento);
                            InsertarBitacorDeJulianos(myReferencia, aduana, patente, pedimento, fechaLarga, lRutaDestino, 0, 0, 1, 0, "", "", miNombreDeArchivo, "");

                            try
                            {
                                int id = await SubiraS3Async(myReferencia, nuevoArchivo.Trim(), gObjUsuario);
                                if (id > 0)
                                {
                                    ModificarBitacorDeJulianos(myReferencia, id, 3);
                                }
                            }
                            catch (Exception)
                            {
                                // Handle the exception as needed
                            }
                        }
                    }
                }
            }
        }

        public string NET_BUSCA_REFERENCIA(string patente, string pedimento, string aduana)
        {
            string regreso = "";
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;

            try
            {
                // Set up the command for the stored procedure
                cmd.CommandText = "NET_SEARCH_SAAIO_PEDIME_PEDIMENTO_Y_ADUANA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters to the command
                param = cmd.Parameters.Add("@NUM_PEDI", SqlDbType.VarChar, 7);
                param.Value = pedimento;

                param = cmd.Parameters.Add("@PAT_AGEN", SqlDbType.VarChar, 4);
                param.Value = patente;

                param = cmd.Parameters.Add("@ADU_DESP", SqlDbType.VarChar, 3);
                param.Value = aduana;

                param = cmd.Parameters.Add("@newid_registro", SqlDbType.VarChar, 15);
                param.Direction = ParameterDirection.Output;

                // Open the connection
                cn.ConnectionString = sConexion;
                cn.Open();

                // Execute the command
                cmd.ExecuteNonQuery();

                // Retrieve the output parameter value
                regreso = cmd.Parameters["@newid_registro"].Value.ToString();

                // Clear the parameters
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                regreso = "";
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message + " NET_SEARCH_SAAIO_PEDIME_PEDIMENTO_Y_ADUANA");
            }
            finally
            {
                // Close and dispose of the connection
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                cn.Dispose();
            }

            return regreso;
        }

        public void CreaNuevoArchivoJuliano(string newFile, string miPatente, string pedimento, string miRegistro)
        {
            string nombreDeArchivo = Path.GetFileName(newFile);

            // Check if the file does not exist
            if (!File.Exists(newFile))
            {
                using (FileStream strStreamErr = File.OpenWrite(newFile))
                {
                    using (StreamWriter strStreamWriterErr = new StreamWriter(strStreamErr, Encoding.ASCII))
                    {
                        strStreamWriterErr.Write(miRegistro);
                    }
                }

                // Insert the new file details into the database
                Insertar(nombreDeArchivo, "M", miPatente, pedimento);
            }
            else
            {
                // If the file already exists, perform the insertion
                Insertar(nombreDeArchivo, "M", miPatente, pedimento);
            }
        }
        public string PADL(string cadena, int longitud, string relleno, bool numero)
        {
            int lenCadena;
            int pnPosicion;

            // If the number flag is true, process decimal points
            if (numero)
            {
                pnPosicion = cadena.IndexOf(".");

                // Append ".00" if no decimal point found
                if (pnPosicion == -1)
                {
                    cadena = cadena + ".00";
                }
                else
                {
                    // Add "0" to the end if there's only one decimal digit
                    if (cadena.Trim().Length - pnPosicion == 1)
                    {
                        cadena = cadena + "0";
                    }
                }
            }

            lenCadena = cadena.Length;

            // Adjust the length of the string if it doesn't match the desired length
            if (lenCadena != longitud)
            {
                for (int i = 2; i <= (longitud - lenCadena); i++)
                {
                    relleno = relleno + relleno.Substring(0, 1);
                }
                relleno = relleno + cadena;
                cadena = relleno;
            }

            return cadena;
        }

        public bool MySearchFirmaElectronica(string fileName, string patente, string pedimento, string newFileErr, int regexOption)
        {
            bool MySearchFirmaElectronica = false;
            string miRegex = "";

            // Set the regex pattern based on the option provided
            if (regexOption == 1)
            {
                miRegex = @"^F\n{0}(?<PEDIMENTO>.{7}).{0}(?<FIRMA>.{0,8}).{9}(?<LINEA>.{0,20}).{0}(?<IMPUESTOS>.{0,12})\n";
            }
            else
            {
                miRegex = @"^F(?<PEDIMENTO>\d{7})(?<FIRMA>.*?)$";
            }

            // Construct the .err file name
            string fileNameSinExt = Path.GetDirectoryName(fileName) + "\\" + Path.GetFileNameWithoutExtension(fileName) + ".err";

            // Initialize the regex with options
            Regex re = new Regex(miRegex, RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection mc;
            string contenidoDelFicheroOriginal;
            string todoElRegistro;
            string firmaElectronica = "";

            // Read the contents of the .err file
            contenidoDelFicheroOriginal = MyReadFile(fileNameSinExt);

            // Find matches using the regex
            mc = re.Matches(contenidoDelFicheroOriginal);

            // Loop through matches and search for the pedimento
            for (int i = 0; i < mc.Count; i++)
            {
                todoElRegistro = mc[i].ToString();
                if (mc[i].Groups["PEDIMENTO"].ToString() == PADL(pedimento, 7, "0", false))
                {
                    firmaElectronica = mc[i].Groups["FIRMA"].ToString();

                    // Call the function to create a new .err file
                    CREA_NUEVO_ARCHIVO_ERR(newFileErr, pedimento, patente, firmaElectronica, todoElRegistro);

                    MySearchFirmaElectronica = true;
                    break;
                }
            }

            return MySearchFirmaElectronica;
        }
        public int Insertar(string miArchivo, string miTipo, string miPatente, string miPedimento)
        {
            int id = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;

            try
            {
                // Open the connection
                cn.ConnectionString = sConexion;
                cn.Open();

                // Set up the command for the stored procedure
                cmd.CommandText = "NET_INSERT_ARCHIVOSDEVALIDACIONYPAGO";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters
                param = cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 50);
                param.Value = miArchivo;

                param = cmd.Parameters.Add("@Tipo", SqlDbType.VarChar, 1);
                param.Value = miTipo;

                param = cmd.Parameters.Add("@Patente", SqlDbType.VarChar, 4);
                param.Value = miPatente;

                param = cmd.Parameters.Add("@Pedimento", SqlDbType.VarChar, 7);
                param.Value = miPedimento;

                param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                param.Direction = ParameterDirection.Output;

                // Execute the command
                cmd.ExecuteNonQuery();

                // Check the output parameter
                if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) == 1)
                {
                    id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                }
                else
                {
                    id = 0;
                }

                // Clear the parameters
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message + " NET_INSERT_BITACORADECLIENTES");
            }
            finally
            {
                // Ensure the connection is closed and disposed
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                cn.Dispose();
            }

            return id;
        }
        public void CREA_NUEVO_ARCHIVO_ERR(string newFileErr, string pedimento, string miPatente, string firmaElectronica, string todoElRegistro)
        {
            string nombreDeArchivo = "";

            // Check if the file does not exist
            if (!File.Exists(newFileErr))
            {
                FileStream strStreamErr = null;
                StreamWriter strStreamWriterErr = null;

                try
                {
                    // Get the filename and create the file stream and writer
                    nombreDeArchivo = Path.GetFileName(newFileErr);
                    strStreamErr = File.OpenWrite(newFileErr);
                    strStreamWriterErr = new StreamWriter(strStreamErr, Encoding.ASCII);

                    // Write the entire record to the file
                    strStreamWriterErr.Write(todoElRegistro);
                }
                finally
                {
                    // Ensure the writer and stream are closed and disposed
                    if (strStreamWriterErr != null)
                    {
                        strStreamWriterErr.Close();
                        strStreamWriterErr.Dispose();
                    }

                    if (strStreamErr != null)
                    {
                        strStreamErr.Close();
                        strStreamErr.Dispose();
                    }
                }

                // Insert the record into the database
                Insertar(nombreDeArchivo, "M", miPatente, pedimento);
            }
            else
            {
                // If the file already exists, just perform the insertion
                nombreDeArchivo = Path.GetFileName(newFileErr);
                Insertar(nombreDeArchivo, "M", miPatente, pedimento);
            }
        }
        public int InsertarBitacorDeJulianos(
        string miReferencia,
        string miAduana,
        string miPatente,
        string miPedimento,
        string miFechaDePago,
        string miRutaFisica,
        int archivoM,
        int archivoErr,
        int archivoE,
        int archivoA,
        string nombreM,
        string nombreErr,
        string nombreE,
        string nombreA)
        {
            int id = 0;
            string sql = "";

            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_INSERT_CASAEI_BITACORADEJULIANOS_INDIVIDUALES", cn))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        cmd.Parameters.Add(new SqlParameter("@Referencia", SqlDbType.VarChar, 15)).Value = miReferencia;
                        cmd.Parameters.Add(new SqlParameter("@Anio", SqlDbType.VarChar, 2)).Value = miFechaDePago.Substring(8, 2);
                        cmd.Parameters.Add(new SqlParameter("@Aduana", SqlDbType.VarChar, 2)).Value = miAduana.Substring(0, 2);
                        cmd.Parameters.Add(new SqlParameter("@Patente", SqlDbType.VarChar, 4)).Value = miPatente;
                        cmd.Parameters.Add(new SqlParameter("@Pedimento", SqlDbType.VarChar, 7)).Value = miPedimento;
                        cmd.Parameters.Add(new SqlParameter("@FechaDePago", SqlDbType.Date)).Value = DateTime.Parse(miFechaDePago);
                        cmd.Parameters.Add(new SqlParameter("@RutaFisica", SqlDbType.VarChar, 100)).Value = miRutaFisica;
                        cmd.Parameters.Add(new SqlParameter("@ArchivoM", SqlDbType.Int)).Value = archivoM;
                        cmd.Parameters.Add(new SqlParameter("@ArchivoErr", SqlDbType.Int)).Value = archivoErr;
                        cmd.Parameters.Add(new SqlParameter("@ArchivoE", SqlDbType.Int)).Value = archivoE;
                        cmd.Parameters.Add(new SqlParameter("@ArchivoA", SqlDbType.Int)).Value = archivoA;
                        cmd.Parameters.Add(new SqlParameter("@NombreM", SqlDbType.VarChar, 30)).Value = nombreM;
                        cmd.Parameters.Add(new SqlParameter("@NombreErr", SqlDbType.VarChar, 30)).Value = nombreErr;
                        cmd.Parameters.Add(new SqlParameter("@NombreE", SqlDbType.VarChar, 30)).Value = nombreE;
                        cmd.Parameters.Add(new SqlParameter("@NombreA", SqlDbType.VarChar, 30)).Value = nombreA;

                        sql = $"NET_INSERT_CASAEI_BITACORADEJULIANOS_INDIVIDUALES '{miReferencia}','" +
                              $"{miFechaDePago.Substring(8, 2)}','" +
                              $"{miAduana.Substring(0, 2)}','" +
                              $"{miPatente}','" +
                              $"{miPedimento}','" +
                              $"{miFechaDePago}','" +
                              $"{miRutaFisica}','" +
                              $"{archivoM},{archivoErr},{archivoE},{archivoA}','" +
                              $"{nombreM}','" +
                              $"{nombreErr}','" +
                              $"{nombreE}','" +
                              $"{nombreA}'";

                        cn.Open();
                        cmd.ExecuteNonQuery();
                        id = 1;
                    }
                    catch (Exception ex)
                    {
                        id = 0;
                        throw new Exception($"{ex.Message} {sql} {miRutaFisica} {miReferencia}");
                    }
                }
            }

            return id;
        }
        public async Task<int> SubiraS3Async(string myReferencia, string documento, CatalogoDeUsuarios gObjUsuario)
        {
            int id = 0;

            try
            {
                string vRespuesta = string.Empty;
                Referencias objRefe = null;
                ReferenciasRepository objRefeD = new ReferenciasRepository(_configuration);
                objRefe = objRefeD.Buscar(myReferencia.Trim());

                if (objRefe != null)
                {
                    
                    CentralizarS3sp objCEntralizaS3 = new CentralizarS3sp(_configuration);
                    id = await objCEntralizaS3.SubirDocumentosporGuia(
                        1167,
                        documento,
                        objRefe.IDReferencia,
                        0,
                        Path.GetFileNameWithoutExtension(documento),
                        gObjUsuario,
                        "grupoei.documentos"
                    );
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                id = 0;
            }

            return id;
        }
        public int ModificarBitacorDeJulianos(string miReferencia, int idDocumento, int tipo)
        {
            int id = 0;

            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_UPDATE_CASAEI_BITACORADEJULIANOS_INDIVIDUALES", cn))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        cmd.Parameters.Add(new SqlParameter("@Referencia", SqlDbType.VarChar, 15)).Value = miReferencia;
                        cmd.Parameters.Add(new SqlParameter("@idDocumento", SqlDbType.Int)).Value = idDocumento;
                        cmd.Parameters.Add(new SqlParameter("@tipo", SqlDbType.Int)).Value = tipo;

                        cn.Open();
                        cmd.ExecuteNonQuery();
                        id = 1;
                    }
                    catch (Exception ex)
                    {
                        id = 0;
                        throw new Exception(ex.Message);
                    }
                }
            }

            return id;
        }

        public async Task<bool> ParseaJulianoAsync(string fileName, string lPathLocal, CatalogoDeUsuarios gObjUsuario)
        {
            try
            {
                Regex re = new Regex(@"^500\|.{0,2}\|(?<PATENTE>.{0,4})\|(?<PEDIMENTO>.{0,7})\|(?<ADUANA>.{0,3})\|.*?^501\|.{0,4}\|.{0,7}\|.{0,3}\|.{0,1}\|.{0,3}\|.{0,3}\|.{0,18}\|(?<RFC>.{0,13})\|.*?^800\|.*?$",
                                     RegexOptions.Multiline | RegexOptions.Singleline);
                MatchCollection mc;
                string pedimento = "", patente = "", aduana = "", rfc = "", todoElRegistro = "";
                string nombreDeArchivo = Path.GetFileNameWithoutExtension(fileName);
                nombreDeArchivo = nombreDeArchivo.Substring(nombreDeArchivo.Length - 3, 3);
                string extencionDeArchivo = Path.GetExtension(fileName);
                string contenidoDelFicheroOriginal = MyReadFile(fileName);
                mc = re.Matches(contenidoDelFicheroOriginal);

                for (int i = 0; i < mc.Count; i++)
                {
                    try
                    {
                        todoElRegistro = mc[i].ToString();
                        patente = mc[i].Groups["PATENTE"].ToString();
                        pedimento = mc[i].Groups["PEDIMENTO"].ToString();
                        rfc = mc[i].Groups["RFC"].ToString().Trim();
                        aduana = mc[i].Groups["ADUANA"].ToString();
                        string miFecha = ExtraerFechaDePago(todoElRegistro);
                        string nuevoPath = Path.Combine(lPathLocal, $"{patente}\\{miFecha.Substring(2, 6)}\\{miFecha.Substring(0, 2)}\\");

                        if (!Directory.Exists(nuevoPath))
                        {
                            Directory.CreateDirectory(nuevoPath);
                        }

                        string myReferencia = NET_BUSCA_REFERENCIA(patente, pedimento, aduana);
                        string newErr = Path.Combine(nuevoPath, $"M{aduana}-{patente}-{pedimento}-{nombreDeArchivo}.err");
                        string fechaDePago = $"{miFecha.Substring(0, 2)}/{miFecha.Substring(2, 2)}/{miFecha.Substring(4, 4)}";
                        int regex = DateTime.Parse(fechaDePago) < DateTime.Parse("25/11/2019") ? 2 : 1;

                        if (MySearchFirmaElectronica(fileName, patente, pedimento, newErr, regex))
                        {
                            string documentoErr = $"M{aduana}-{patente}-{pedimento}-{nombreDeArchivo}.err";
                            int id = 0;

                            InsertarBitacorDeJulianos(myReferencia, aduana, patente, pedimento, fechaDePago, nuevoPath, 0, 1, 0, 0, "", documentoErr, "", "");

                            try
                            {
                                id = await SubiraS3Async(myReferencia, Path.Combine(nuevoPath, documentoErr), gObjUsuario);
                                if (id > 0)
                                {
                                    ModificarBitacorDeJulianos(myReferencia, id, 2);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error al subir {documentoErr} a S3: {ex.Message}");
                            }

                            string documentoJuliano = $"M{aduana}-{patente}-{pedimento}-{nombreDeArchivo}{extencionDeArchivo}";
                            CreaNuevoArchivoJuliano(Path.Combine(nuevoPath, documentoJuliano), patente, pedimento, todoElRegistro);
                            InsertarBitacorDeJulianos(myReferencia, aduana, patente, pedimento, fechaDePago, nuevoPath, 1, 0, 0, 0, documentoJuliano, "", "", "");

                            try
                            {
                                id = await SubiraS3Async(myReferencia, Path.Combine(nuevoPath, documentoJuliano), gObjUsuario);
                                if (id > 0)
                                {
                                    ModificarBitacorDeJulianos(myReferencia, id, 1);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error al subir {documentoJuliano} a S3: {ex.Message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al procesar registro {i}: {ex.Message}");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ParseaJulianoAsync: {ex.Message}");
                return false;
            }
        }


    }
}

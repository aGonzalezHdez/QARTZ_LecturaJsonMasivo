using LibreriaClasesAPIExpertti.Entities.EntitiesJCJF;
using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesTurnado;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesJCJF;
using LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using NPOI.OpenXmlFormats.Dml;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF
{
    public class CargadeManifiestosRepository : ICargadeManifiestosRepository
    {

        public string SConexion { get; set; }
        string ICargadeManifiestosRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;  
        public CargadeManifiestosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public ManifiestosErrores CargarManifiestoDHL(ManifiestoFile objManifiestoDHLFile)
        {
            ManifiestosErrores objCustomAlertsErrores = new();
            string s = "";

            UbicaciondeArchivosRepository objUbicacionDeArchivosRepository = new(_configuration);
            string NombreArchivo = objManifiestoDHLFile.NombreArchivo;

            UbicaciondeArchivos objRuta = objUbicacionDeArchivosRepository.Buscar(241);

            if (Directory.Exists(objRuta.Ubicacion) == false)
            {
                Directory.CreateDirectory(objRuta.Ubicacion);
            }

            //objRuta.Ubicacion = @"D:\atgomez\";

            NombreArchivo = objRuta.Ubicacion + NombreArchivo;
            Byte[] bytes = Convert.FromBase64String(objManifiestoDHLFile.ArchivoBase64);
            System.IO.File.WriteAllBytes(NombreArchivo, bytes);

            using (var sr = new StreamReader(NombreArchivo))
            {
                s = sr.ReadToEnd();
                sr.Close();
            }

            objCustomAlertsErrores = CargarManifiestoExtendido(objManifiestoDHLFile, s);

            return objCustomAlertsErrores;
        }

        public ManifiestosErrores CargarManifiestoExtendido(ManifiestoFile objManifiestoDHLFile, string s)
        {
            ManifiestosErrores objCustomAlertsErrores = new();
            List<string> lstErrores = new();
            int txtTotalError;
            int txtFilas;
            //int txtProcesadas = 0;
            int vContador = 0;
            Helper helper = new();

            DataTable dtCustomAlerts = new();
            dtCustomAlerts.Columns.Add("GuiaHouse", typeof(string));
            dtCustomAlerts.Columns.Add("Piezas", typeof(int));
            dtCustomAlerts.Columns.Add("PesoTotal", typeof(decimal));
            dtCustomAlerts.Columns.Add("ValorEnDolares", typeof(decimal));
            dtCustomAlerts.Columns.Add("OrigenIata", typeof(string));
            dtCustomAlerts.Columns.Add("DestinoIata", typeof(string));
            dtCustomAlerts.Columns.Add("Remitente", typeof(string));
            dtCustomAlerts.Columns.Add("Cliente", typeof(string));
            dtCustomAlerts.Columns.Add("Descripcion", typeof(string));
            dtCustomAlerts.Columns.Add("GuiaMaster", typeof(string));
            dtCustomAlerts.Columns.Add("FechaDeEntrada", typeof(string));
            dtCustomAlerts.Columns.Add("PID", typeof(string));
            dtCustomAlerts.Columns.Add("IdUsuario", typeof(int));

            int vTotalColumnas = 0;
            foreach (Match m in Regex.Matches(s, "^.*$", RegexOptions.Multiline))
            {
                string[] vArreglo = Regex.Split(m.ToString(), char.ConvertFromUtf32(9));
                vTotalColumnas = vArreglo.Length;
                break;
            }

            try
            {
                foreach (Match m in Regex.Matches(s, "^.*$", RegexOptions.Multiline))
                {
                    // Carga a un arreglo de "strings" los campos de la línea leída separados por "tabulador"
                    vContador++;
                    string[] vArreglo = Regex.Split(m.ToString(), Char.ConvertFromUtf32(9));

                    try
                    {
                        if (vArreglo.Length == 1)
                            break;

                        DataRow dataRow = dtCustomAlerts.NewRow();
                        dataRow["GuiaHouse"] = vArreglo[0].Trim();
                        dataRow["Piezas"] = Convert.ToInt32(vArreglo[1]);
                        dataRow["PesoTotal"] = Convert.ToDecimal(vArreglo[2]);
                        dataRow["ValorEnDolares"] = Convert.ToDecimal(vArreglo[3]);
                        dataRow["OrigenIata"] = helper.EliminaComillas(vArreglo[4]);
                        dataRow["DestinoIata"] = helper.EliminaComillas(vArreglo[5]);
                        dataRow["Remitente"] = helper.EliminaComillas(vArreglo[6]);
                        dataRow["Cliente"] = helper.EliminaComillas(vArreglo[7]);
                        dataRow["Descripcion"] = helper.EliminaComillas(vArreglo[8]);
                        dataRow["GuiaMaster"] = vArreglo[9].Trim();
                        //dataRow["FechaDeEntrada"] = DateTime.ParseExact(vArreglo[10], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //dataRow["FechaDeEntrada"] = Convert.ToDateTime(vArreglo[10]).ToString("dd/MM/yyyy");
                        dataRow["FechaDeEntrada"] = (object)(vArreglo[10]);
                        if (vTotalColumnas == 11)
                        {
                            dataRow["PID"] = null;
                        }
                        else if (vTotalColumnas == 12)
                        {
                            dataRow["PID"] = vArreglo[11].Trim();
                        }
                        dataRow["IdUsuario"] = objManifiestoDHLFile.IdUsuario;
                        dtCustomAlerts.Rows.Add(dataRow);

                    }
                    catch (Exception ex)
                    {
                        lstErrores.Add("Fila " + vContador + ": " + ex.Message);
                    }
                }
                Errores Errores = new();

                if (dtCustomAlerts.Rows.Count != 0)
                {
                    Errores = DT_InsertarManifiestoExtendido_TableType(dtCustomAlerts, objManifiestoDHLFile.IdOficina, objManifiestoDHLFile.NombreArchivo);
                }

                //txtFilas = Regex.Matches(s, "^.*$", RegexOptions.Multiline).Count;
                txtFilas = dtCustomAlerts.Rows.Count + lstErrores.Count;
                txtTotalError = txtFilas - Errores.txtProcesadas;
                if (Errores.Error != "")
                {
                    lstErrores.Add("Error al insertar en Expertit: " + Errores.Error);
                }
                objCustomAlertsErrores.txtTotalError = txtTotalError;
                objCustomAlertsErrores.txtFilas = txtFilas;
                objCustomAlertsErrores.txtProcesadas = Errores.txtProcesadas;
                objCustomAlertsErrores.lstErrores = lstErrores;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objCustomAlertsErrores;
        }

        public class Errores
        {
            public int txtProcesadas { get; set; }
            public string Error { get; set; }
        }

        public Errores DT_InsertarManifiestoExtendido_TableType(DataTable dt, int IdOficina, string NombreArchivo)
        {
            Errores Errores = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_CASAEI_INSERT_MANIFIESTOEXTENDIDO_TableType", con))
                {
                    con.Open();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@dt", SqlDbType.Structured)).Value = dt;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = IdOficina;
                    cmd.Parameters.Add("@NombreArchivo", SqlDbType.VarChar, 250).Value = NombreArchivo;
                    cmd.Parameters.Add("@TotalProcesadas", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Error", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.ExecuteScalar();

                    Errores.txtProcesadas = Convert.ToInt32(cmd.Parameters["@TotalProcesadas"].Value);
                    Errores.Error = cmd.Parameters["@Error"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return Errores;
        }

        public ManifiestosErrores CargarManifiestoFedex(ManifiestoFile objManifiestoFedexFile)
        {
            ValidarObjeto(objManifiestoFedexFile);
            ManifiestosErrores objCustomAlertsErrores = new();

            string contenidoManifiesto = "";

            UbicaciondeArchivosRepository objUbicacionDeArchivosRepository = new(_configuration);
            string NombreArchivo = objManifiestoFedexFile.NombreArchivo;

            UbicaciondeArchivos objRuta = objUbicacionDeArchivosRepository.Buscar(241);
            if (Directory.Exists(objRuta.Ubicacion) == false)
            {
                Directory.CreateDirectory(objRuta.Ubicacion);
            }

            //objRuta.Ubicacion = @"D:\atgomez\";

            NombreArchivo = objRuta.Ubicacion + NombreArchivo;
            Byte[] bytes = Convert.FromBase64String(objManifiestoFedexFile.ArchivoBase64);
            System.IO.File.WriteAllBytes(NombreArchivo, bytes);

            using (var sr = new StreamReader(NombreArchivo))
            {
                contenidoManifiesto = sr.ReadToEnd();
                sr.Close();
            }

            int vTotalColumnas = 0;
            foreach (Match m in Regex.Matches(contenidoManifiesto, "^.*$", RegexOptions.Multiline))
            {
                string[] vArreglo = Regex.Split(m.ToString(), Char.ConvertFromUtf32(9));
                vTotalColumnas = vArreglo.Length;
                break;
            }

            if (objManifiestoFedexFile.idManifiesto == 3)
            {
                if (vTotalColumnas != 41) throw new Exception("El número de columnas no es válido"); 
                objCustomAlertsErrores = CargarManifiestoFedex(objManifiestoFedexFile, contenidoManifiesto, true);
            }else if (objManifiestoFedexFile.idManifiesto == 2)
            {
                if (vTotalColumnas == 41) throw new Exception("El número de columnas no es válido");
                objCustomAlertsErrores = CargarManifiestoFedex(objManifiestoFedexFile, contenidoManifiesto, false);
            }          

            return objCustomAlertsErrores;

        }

        public ManifiestosErrores CargarManifiestoFedex(ManifiestoFile objManifiestoFedexFile, string contenidoManifiesto, bool esFacturacion)
        {
            Publicas publicas = new();
            List<string> lstErrores = new();
            int vContador = 0;
            int vProcesadas = 0;
            int IdCustomAlert = 0;
            bool inicioProcesamiento = false;

            DataTable dt = crearTablaManifiestoFedex(esFacturacion);
            DataTable dtBabys = crearTablaManifiestoFedexBaby();

            foreach (Match m in Regex.Matches(contenidoManifiesto, "^.*$", RegexOptions.Multiline))
            {
                vContador++;
                string linea = m.ToString();

                if (!inicioProcesamiento)
                {
                    if (linea.Contains("AB/HDR:"))
                        inicioProcesamiento = true;
                    else
                        continue;
                }

                if (string.IsNullOrWhiteSpace(linea))
                    break;

                try
                {
                    if (linea.Trim().StartsWith("CRNS"))
                    {
                        ProcesarCRNS(linea, dtBabys, vContador, IdCustomAlert, publicas);
                        continue;
                    }

                    string[] campos = Regex.Split(linea, Char.ConvertFromUtf32(9));
                    if (campos.Length <= 1)
                        continue;

                    string guiaHouse = publicas.EliminaComillas(campos[0].Trim());
                    if (!Regex.IsMatch(guiaHouse, @"\d"))
                        continue;

                    DataRow dataRow = dt.NewRow();
                    dataRow["Fila"] = vContador;

                    try
                    {
                        AsignarCamposPrincipales(dataRow, campos, vContador, objManifiestoFedexFile, esFacturacion, guiaHouse, IdCustomAlert.ToString());
                        dt.Rows.Add(dataRow);
                        vProcesadas++;
                    }
                    catch (Exception ex)
                    {
                        lstErrores.Add($"Fila {vContador}: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    lstErrores.Add($"Fila {vContador}: {ex.Message}");
                }
            }

            int totalErrores = lstErrores.Count;
            int totalFilas = vContador;
            int totalProcesadas = totalFilas - totalErrores;

            if (dt.Rows.Count > 0)
            {
                try
                {
                    DT_InsertarManifiestoFedex(dt, dtBabys, objManifiestoFedexFile.IdOficina, objManifiestoFedexFile.NombreArchivo, esFacturacion);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " Falla la inserción en la tabla");
                }
            }

            return new ManifiestosErrores
            {
                txtTotalError = totalErrores,
                txtFilas = totalFilas,
                txtProcesadas = totalProcesadas,
                lstErrores = lstErrores
            };
        }

        private void ProcesarCRNS(string linea, DataTable dtBabys, int fila, int idCustomAlert, Publicas publicas)
        {
            string[] partes = linea.Split('-');

            foreach (var parte in partes)
            {
                string guia = publicas.EliminaEnter(publicas.EliminaComillas(parte.Replace("CRNS", "").Replace(":", "")));

                if (!string.IsNullOrWhiteSpace(guia))
                {
                    DataRow row = dtBabys.NewRow();
                    row["Fila"] = fila;
                    row["IdCustomAlert"] = idCustomAlert;
                    row["GuiaBaby"] = guia.Trim();
                    dtBabys.Rows.Add(row);
                }
            }
        }



        void AsignarCamposPrincipales(DataRow dataRow, string[] vArreglo, int fila, ManifiestoFile obj, bool esFacturacion, string guiaHouse,string IdCustomAlert)
        {
           
                AsignarCampo(dataRow, "GuiaHouse", () => guiaHouse, fila);
                AsignarCampo(dataRow, "Piezas", () => vArreglo[34] == "NCV" ? 1 : vArreglo[34], fila);
                AsignarCampo(dataRow, "PesoTotal", () => CalcularPeso(vArreglo[35]), fila);
                AsignarCampo(dataRow, "ValorEnDolares", () => 0, fila);
                AsignarCampo(dataRow, "OrigenIata", () => vArreglo[1], fila, true);
                AsignarCampo(dataRow, "DestinoIata", () => vArreglo[2], fila, true);
                AsignarCampo(dataRow, "Remitente", () => vArreglo[18], fila, true);
                AsignarCampo(dataRow, "Cliente", () => vArreglo[9], fila, true);
                AsignarCampo(dataRow, "Descripcion", () => vArreglo[39].Trim(), fila, true);
                AsignarCampo(dataRow, "GuiaMaster", () => obj.TxtGuiaMaster, fila);
                AsignarCampo(dataRow, "FechaDeEntrada", () => Information.IsDate(vArreglo[5]) ? vArreglo[5] : DateTime.Now.ToString("dd/MM/yyyy"), fila);
                AsignarCampo(dataRow, "IdUsuario", () => obj.IdUsuario, fila);
                AsignarCampo(dataRow, "ValorMe", () => vArreglo[37] == "NCV" ? 1 : decimal.Parse(vArreglo[37]), fila);
                AsignarCampo(dataRow, "Moneda", () => vArreglo[36], fila, true);
                AsignarCampo(dataRow, "Fletes", () => string.IsNullOrWhiteSpace(vArreglo[38]) || vArreglo[38] == "NCV" ? 0 : decimal.Parse(vArreglo[38]), fila);
                AsignarCampo(dataRow, "IDDatosDeEmpresa", () => obj.IDDatosDeEmpresa, fila);
                AsignarCampo(dataRow, "RFCCliente", () => vArreglo[30], fila);
                AsignarCampo(dataRow, "IdCustomAlert", () => IdCustomAlert, fila);

                // Cliente
                AsignarCampo(dataRow, "ClienteCuenta", () => vArreglo[6].Trim(), fila, true);
                AsignarCampo(dataRow, "ClienteTelefono", () => vArreglo[7].Trim(), fila, true);
                AsignarCampo(dataRow, "ClienteNombre", () => vArreglo[8].Trim(), fila, true);
                AsignarCampo(dataRow, "ClienteContacto", () => vArreglo[9].Trim(), fila, true);
                AsignarCampo(dataRow, "ClienteDireccion", () => vArreglo[10].Trim(), fila, true);
                AsignarCampo(dataRow, "ClienteDireccion2", () => vArreglo[11].Trim(), fila, true);
                AsignarCampo(dataRow, "ClienteCiudad", () => vArreglo[12].Trim(), fila, true);
                AsignarCampo(dataRow, "ClienteSTPV", () => vArreglo[13].Trim(), fila, true);
                AsignarCampo(dataRow, "ClienteEntidadFederativa", () => vArreglo[14].Trim(), fila, true);
                AsignarCampo(dataRow, "ClienteCodigoPostal", () => vArreglo[15].Trim(), fila, true);

                // Proveedor
                AsignarCampo(dataRow, "ProveedorCuenta", () => vArreglo[16].Trim(), fila, true);
                AsignarCampo(dataRow, "ProveedorTelefono", () => vArreglo[17].Trim(), fila, true);
                AsignarCampo(dataRow, "ProveedorNombre", () => vArreglo[18].Trim(), fila, true);
                AsignarCampo(dataRow, "ProveedorContacto", () => vArreglo[19].Trim(), fila, true);
                AsignarCampo(dataRow, "ProveedorDireccion", () => vArreglo[20].Trim(), fila, true);
                AsignarCampo(dataRow, "ProveedorDireccion2", () => vArreglo[21].Trim(), fila, true);
                AsignarCampo(dataRow, "ProveedorCiudad", () => vArreglo[22].Trim(), fila, true);
                AsignarCampo(dataRow, "ProveedorSTPV", () => vArreglo[23].Trim(), fila, true);
                AsignarCampo(dataRow, "ProveedorEntidadFederativa", () => vArreglo[24].Trim(), fila, true);
                AsignarCampo(dataRow, "ProveedorCodigoPostal", () => vArreglo[25].Trim(), fila, true);

                // Países
                AsignarCampo(dataRow, "OrigenCountry", () => vArreglo[3], fila, true);
                AsignarCampo(dataRow, "DestinoCountry", () => vArreglo[4], fila, true);

                // Opcional
                if (esFacturacion)
                    AsignarCampo(dataRow, "FacturacionFx", () => vArreglo[40].Trim(), fila, true);
            
            
        }


        void AsignarCampo(DataRow row, string columna, Func<object> obtenerValor, int fila, bool sinCaractesEspeciales = false)
        {
            Publicas publicas = new();
            try
            {
                object valor = obtenerValor();

                if (sinCaractesEspeciales && valor is string strValor)
                {
                    valor = publicas.EliminaComillas(strValor);
                //valor = publicas.QuitarEnter(strValor);
                }    

                row[columna] = valor;
            }
            catch (FormatException)
            {
                throw new Exception($"Error en columna '{columna}', fila {fila}. Detalle: El formato de la celda no es correcto");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en columna '{columna}', fila {fila}. Detalle: {ex.Message}");
            }
        }



        private DataTable crearTablaManifiestoFedexBaby()
        {
            DataTable dtBabys = new();
            dtBabys.Columns.Add("Fila", typeof(int));
            dtBabys.Columns.Add("IdCustomAlert", typeof(int));
            dtBabys.Columns.Add("GuiaBaby", typeof(string));
            return dtBabys; 
        }
        private DataTable crearTablaManifiestoFedex(bool esFacturacion)
        {
            DataTable dt = new();
            dt.Columns.Add("Fila", typeof(int));
            dt.Columns.Add("GuiaHouse", typeof(string));
            dt.Columns.Add("ValorEnDolares", typeof(decimal));
            dt.Columns.Add("PesoTotal", typeof(decimal));
            dt.Columns.Add("Descripcion", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("OrigenIata", typeof(string));
            dt.Columns.Add("DestinoIata", typeof(string));
            dt.Columns.Add("GuiaMaster", typeof(string));
            dt.Columns.Add("FechaDeEntrada", typeof(string));
            dt.Columns.Add("IdUsuario", typeof(int));
            dt.Columns.Add("Remitente", typeof(string));
            dt.Columns.Add("Piezas", typeof(int));
            dt.Columns.Add("ValorMe", typeof(decimal));
            dt.Columns.Add("Moneda", typeof(string));
            dt.Columns.Add("Fletes", typeof(decimal));
            dt.Columns.Add("RFCCliente", typeof(string));
            dt.Columns.Add("IDDatosDeEmpresa", typeof(int));
            dt.Columns.Add("idCustomAlert", typeof(int));
            dt.Columns.Add("ClienteCuenta", typeof(string));
            dt.Columns.Add("ClienteTelefono", typeof(string));
            dt.Columns.Add("ClienteNombre", typeof(string));
            dt.Columns.Add("ClienteContacto", typeof(string));
            dt.Columns.Add("ClienteDireccion", typeof(string));
            dt.Columns.Add("ClienteDireccion2", typeof(string));
            dt.Columns.Add("ClienteCiudad", typeof(string));
            dt.Columns.Add("ClienteSTPV", typeof(string));
            dt.Columns.Add("ClienteEntidadFederativa", typeof(string));
            dt.Columns.Add("ClienteCodigoPostal", typeof(string));
            dt.Columns.Add("ProveedorCuenta", typeof(string));
            dt.Columns.Add("ProveedorTelefono", typeof(string));
            dt.Columns.Add("ProveedorNombre", typeof(string));
            dt.Columns.Add("ProveedorContacto", typeof(string));
            dt.Columns.Add("ProveedorDireccion", typeof(string));
            dt.Columns.Add("ProveedorDireccion2", typeof(string));
            dt.Columns.Add("ProveedorCiudad", typeof(string));
            dt.Columns.Add("ProveedorSTPV", typeof(string));
            dt.Columns.Add("ProveedorEntidadFederativa", typeof(string));
            dt.Columns.Add("ProveedorCodigoPostal", typeof(string));
            dt.Columns.Add("OrigenCountry", typeof(string));
            dt.Columns.Add("DestinoCountry", typeof(string));

            if (esFacturacion)
                dt.Columns.Add("FacturacionFx", typeof(string));
            return dt;
        }

        public int DT_InsertarManifiestoFedex(DataTable dt, DataTable dtBabys, int IdOficina, string NombreArchivo, bool esFacturacion)
        {
            int TotalProcesadas = 0;
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new(esFacturacion
                    ? "NET_CASAEI_INSERT_MANIFIESTOFEDEXFacturacion_TableType"
                    : "NET_CASAEI_INSERT_MANIFIESTOFEDEX_TableType", con);

                con.Open();
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@dt", SqlDbType.Structured) { Value = dt });
                cmd.Parameters.Add(new SqlParameter("@dtBabys", SqlDbType.Structured) { Value = dtBabys });
                cmd.Parameters.Add(new SqlParameter("@IdOficina", SqlDbType.Int) { Value = IdOficina });
                cmd.Parameters.Add(new SqlParameter("@NombreArchivo", SqlDbType.VarChar, 250) { Value = NombreArchivo });
                cmd.Parameters.Add(new SqlParameter("@TotalProcesadas", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                });

                cmd.ExecuteScalar();
                TotalProcesadas = Convert.ToInt32(cmd.Parameters["@TotalProcesadas"].Value);
            }
            catch (Exception ex)
            {
                string nombreSP = esFacturacion
                    ? "NET_CASAEI_INSERT_MANIFIESTOFEDEXFacturacion_TableType"
                    : "NET_CASAEI_INSERT_MANIFIESTOFEDEX_TableType";
                throw new Exception($"{ex.Message} - Error al ejecutar SP: {nombreSP}");
            }

            return TotalProcesadas;
        }


        public ManifiestosErrores CargarManifiestoPieceIDs(ManifiestoFile objManifiestoFile)
        {
            //PieceIDsCarga pieceIDsCarga = new PieceIDsCarga();
            ManifiestosErrores customAlertsErrores = new ManifiestosErrores();
            List<string> lstErrores = new();
            int txtTotalError = 0;
            int txtFilas = 0;
            int Procesadas = 0;
            int vContador = 0;

            DataTable table = new DataTable();
            string Valija = string.Empty;
            string Contenedor = string.Empty;
            string GuiaMaster = string.Empty;
            string GuiaHouse = string.Empty;
            string PID = string.Empty;
            string PWeight = string.Empty;
            int IdUsuario = objManifiestoFile.IdUsuario;
            string valor_celda = "string.Empty";
            int drColumn = 0;
            int drRow = 0;
            string Archivo = string.Empty;
            int Estatus = 0;
            int noOfRow = 0;
            int iCuentaArchivoCargado = 0;

            UbicaciondeArchivosRepository objUbicacionDeArchivosRepository = new(_configuration);
            string NombreArchivo = objManifiestoFile.NombreArchivo;

            iCuentaArchivoCargado = CuentaCargaBitacoraPieceIds(NombreArchivo, objManifiestoFile.IdOficina);
            if (iCuentaArchivoCargado > 0)
            {
                lstErrores.Add("Archivo procesado con anterioridad");
            }
            else
            {



                UbicaciondeArchivos objRuta = objUbicacionDeArchivosRepository.Buscar(242);


                if (Directory.Exists(objRuta.Ubicacion) == false)
                {
                    Directory.CreateDirectory(objRuta.Ubicacion);
                }

                Archivo = objRuta.Ubicacion + NombreArchivo;
                Byte[] bytes = Convert.FromBase64String(objManifiestoFile.ArchivoBase64);
                System.IO.File.WriteAllBytes(Archivo, bytes);
                FileInfo newFile = new FileInfo(Archivo);

                try
                {
                    if (newFile.Exists == false)
                    {
                        lstErrores.Add("No se encontró el Archivo");
                    }
                    else
                    {
                        //if you want to read data from a excel file use this 
                        //using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                        using (var stream = newFile.OpenRead())
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                            ExcelPackage package = new ExcelPackage();
                            package.Load(stream);
                            if (package.Workbook.Worksheets.Count > 0)
                            {
                                using (ExcelWorksheet workSheet = package.Workbook.Worksheets.First())
                                {
                                    int noOfCol = workSheet.Dimension.End.Column;
                                    noOfRow = workSheet.Dimension.End.Row;
                                    int rowIndex = 1;

                                    //for (int c = 1; c <= noOfCol; c++)
                                    //{
                                    //    table.Columns.Add(workSheet.Cells[rowIndex, c].Text);
                                    //}
                                    for (int c = 0; c < 8; c++)
                                    {
                                        switch (c)
                                        {
                                            case 0:
                                                table.Columns.Add("Valija");
                                                break;
                                            case 1:
                                                table.Columns.Add("Contenedor");
                                                break;
                                            case 2:
                                                table.Columns.Add("GuiaMaster");
                                                break;
                                            case 3:
                                                table.Columns.Add("GuiaHouse");
                                                break;
                                            case 4:
                                                table.Columns.Add("PID");
                                                break;
                                            case 5:
                                                table.Columns.Add("PWeight");
                                                break;
                                            case 7:
                                                table.Columns.Add("IdUsuario");
                                                break;

                                        }
                                    }
                                    rowIndex = 2;
                                    //DataRow dr = table.NewRow();
                                    for (int r = rowIndex; r < noOfRow; r++)
                                    {
                                        //DataRow dr = table.NewRow();
                                        drColumn = 1;
                                        drRow = 1;
                                        //for (int c = 1; c <= noOfCol; c++)
                                        //{
                                        valor_celda = workSheet.Cells[r, 1].Value.ToString().Trim();
                                        //if (c == 1)
                                        //{
                                        if (valor_celda == "H")
                                        {
                                            Valija = workSheet.Cells[r, 2].Value.ToString().Trim();
                                            //dr[0] = Valija;
                                            Contenedor = workSheet.Cells[r, 4].Value.ToString().Trim();
                                            //dr[1] = Contenedor;
                                        }
                                        if (valor_celda == "A")
                                        {
                                            GuiaMaster = workSheet.Cells[r, 4].Value.ToString().Trim();
                                            //dr[2] = GuiaMaster;
                                            GuiaHouse = workSheet.Cells[r, 5].Value.ToString().Trim();
                                            //dr[3] = GuiaHouse;
                                        }
                                        if (valor_celda == "P")
                                        {
                                            DataRow dr = table.NewRow();
                                            dr[0] = Valija;
                                            dr[1] = Contenedor;
                                            dr[2] = GuiaMaster;
                                            dr[3] = GuiaHouse;
                                            PID = workSheet.Cells[r, 2].Value.ToString().Trim();
                                            dr[4] = PID;
                                            bool lenPID = PID.Contains(".");
                                            if (workSheet.Cells[r, 4].Value == null)
                                            {
                                                PWeight = "0";
                                            }
                                            else
                                            {
                                                PWeight = workSheet.Cells[r, 4].Value.ToString().Trim();
                                            }

                                            dr[5] = PWeight;
                                            dr[6] = IdUsuario.ToString();
                                            if (!lenPID)
                                            {
                                                table.Rows.Add(dr);
                                            }

                                        }

                                        //}
                                        //dr[c - 1] = workSheet.Cells[r, c].Value;
                                        //}
                                        //table.Rows.Add(dr);
                                    }

                                    if (table.Rows.Count != 0)
                                    {
                                        Procesadas = InsertarPieceIDs_TableType(table, objManifiestoFile.IdOficina, IdUsuario, NombreArchivo);
                                        if (Procesadas > 0)
                                        {
                                            Estatus = 1;
                                        }
                                    }



                                }
                            }
                            //else
                            //{

                            //}
                            customAlertsErrores.txtTotalError = 0;
                            customAlertsErrores.txtFilas = noOfRow;
                            customAlertsErrores.txtProcesadas = Procesadas;

                        }
                    }



                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }
            customAlertsErrores.lstErrores = lstErrores;
            return customAlertsErrores;
        }

        public int InsertarPieceIDs_TableType(DataTable dt, int IdOficina, int IdUsuario, string NombreArchivo)
        {
            int TotalProcesadas = 0;
            string Error = "";
            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_INSERT_PIECEID_MASIVO", con))
                {
                    con.Open();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@dt", SqlDbType.Structured)).Value = dt;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = IdOficina;
                    cmd.Parameters.Add("@NombreArchivo", SqlDbType.VarChar, 250).Value = NombreArchivo;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = IdUsuario;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Error", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.ExecuteScalar();

                    TotalProcesadas = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    Error = cmd.Parameters["@Error"].Value.ToString();
                    if (TotalProcesadas == 0)
                    {
                        if(Error.Trim() != "")
                        {
                            throw new Exception(Error);
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return TotalProcesadas;
        }

        public int CuentaCargaBitacoraPieceIds(string NombreArchivo, int IDOficina)
        {
            int vContar;

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_COUNT_CARGAPIECEIDBITACORA", con))
                {
                    con.Open();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@NombreArchivo", SqlDbType.VarChar, 250).Value = NombreArchivo;
                    cmd.Parameters.Add("@IDOficina", SqlDbType.Int).Value = IDOficina;


                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        dr.Read();
                        vContar = Convert.ToInt32(dr["Cuantos"]);
                    }
                    else
                        vContar = 0;
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return vContar;
        }

        public ManifiestosErrores CargarSafety(ManifiestoFile objSafety)
        {
            ManifiestosErrores objCustomAlertsErrores = new();
            DataTable dtSafety = new();      
            List<string> lstErrores = new();
            //int txtFilas;
            int cantidadFilas;
            int txtTotalError;
            //int txtProcesadas = 0;
            int txtProcesadas;

            UbicaciondeArchivosRepository objUbicacionDeArchivosRepository = new(_configuration);
            string NombreArchivo = objSafety.NombreArchivo;

            try
            {
                UbicaciondeArchivos objRuta = objUbicacionDeArchivosRepository.Buscar(241);
                if (Directory.Exists(objRuta.Ubicacion) == false)
                {
                    Directory.CreateDirectory(objRuta.Ubicacion);
                }

                NombreArchivo = objRuta.Ubicacion + NombreArchivo;
                Byte[] bytes = Convert.FromBase64String(objSafety.ArchivoBase64);
                System.IO.File.WriteAllBytes(NombreArchivo, bytes);

                if (File.Exists(NombreArchivo))
                {
                    IWorkbook? MiExcel = null;
                    if (Path.GetExtension(NombreArchivo) != ".xlsx")
                    {
                        throw new Exception("Debe ser un archivo Excel(.xlsx)");
                    }

                    MiExcel = new XSSFWorkbook(NombreArchivo);
                    MiExcel.Close();
                    ISheet HojaExcel = MiExcel.GetSheetAt(0);

                    //txtFilas = HojaExcel.LastRowNum;
                    cantidadFilas = HojaExcel.LastRowNum;

                    dtSafety.Columns.Add("origin", typeof(string));
                    dtSafety.Columns.Add("master_guide", typeof(string));
                    dtSafety.Columns.Add("guide", typeof(string));
                    dtSafety.Columns.Add("packages", typeof(string));
                    dtSafety.Columns.Add("weight", typeof(string));
                    dtSafety.Columns.Add("customs_value", typeof(string));
                    dtSafety.Columns.Add("origin2", typeof(string));
                    dtSafety.Columns.Add("destination", typeof(string));
                    dtSafety.Columns.Add("shipper", typeof(string));
                    dtSafety.Columns.Add("consignee", typeof(string));
                    dtSafety.Columns.Add("description", typeof(string));
                    dtSafety.Columns.Add("suggestion", typeof(string));

                    for (int i = 1; i <= cantidadFilas; i++)
                    //for (int i = 1; i <= txtFilas; i++)
                    {
                        IRow fila = HojaExcel.GetRow(i);

                        if (fila != null)
                        {
                            try
                            {
                                // Validar cada celda antes de agregarla al DataTable
                                ICell celda1 = fila.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                if (celda1.CellType == CellType.Blank)
                                {
                                    throw new Exception($"La celda {1 + 1} no contiene un valor válido.");
                                }

                                ICell celda2 = fila.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                if (celda2.CellType == CellType.Blank)
                                {
                                    throw new Exception($"La celda {2 + 1} no contiene un valor válido.");
                                }
                                ICell celda11 = fila.GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                                if (celda11.CellType == CellType.Blank)
                                {
                                    throw new Exception($"La celda {11 + 1} no contiene un valor válido.");
                                }

                                // Agregar fila al DataTable
                                dtSafety.Rows.Add(
                                    fila.GetCell(0).ToString().Trim(),
                                    fila.GetCell(1).ToString().Trim(),
                                    fila.GetCell(2).ToString().Trim(),
                                    fila.GetCell(3).ToString().Trim(),
                                    fila.GetCell(4).ToString().Trim(),
                                    fila.GetCell(5).ToString().Trim(),
                                    fila.GetCell(6).ToString().Trim(),
                                    fila.GetCell(7).ToString().Trim(),
                                    fila.GetCell(8).ToString().Trim(),
                                    fila.GetCell(9).ToString().Trim(),
                                    fila.GetCell(10).ToString().Trim(),
                                    fila.GetCell(11).ToString().Trim());
                            }
                            catch (Exception ex)
                            {
                                lstErrores.Add($"Error al procesar la fila {i + 1}: {ex.Message}");
                            }
                        }
                    }

                    //if (dtSafety.Rows.Count != 0)
                    //{ 
                    //txtProcesadas = DT_InsertarSafety_TableType(dtSafety);
                    //}

                    //txtTotalError = txtFilas - txtProcesadas;

                    //objCustomAlertsErrores.txtTotalError = txtTotalError;
                    //objCustomAlertsErrores.txtFilas = txtFilas;
                    //objCustomAlertsErrores.txtProcesadas = txtProcesadas;
                    //objCustomAlertsErrores.lstErrores = lstErrores;

                    txtTotalError = lstErrores.Count;
                    txtProcesadas = cantidadFilas - txtTotalError;

                    if (dtSafety.Rows.Count != 0)
                    {
                        if (DT_InsertarSafety_TableType(dtSafety))
                        {
                            objCustomAlertsErrores.txtTotalError = txtTotalError;
                            objCustomAlertsErrores.txtFilas = cantidadFilas;
                            objCustomAlertsErrores.txtProcesadas = txtProcesadas;
                            objCustomAlertsErrores.lstErrores = lstErrores;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objCustomAlertsErrores;
        }

        //public int DT_InsertarSafety_TableType(DataTable dtSafety)
        public bool DT_InsertarSafety_TableType(DataTable dtSafety)
        {
            //int TotalProcesadas = 0;
            //string? Error = "";
            bool Procesadas = false;
            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_CASAEI_INSERT_SAFETY_TableType", con))
                {
                    con.Open();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Tabla", SqlDbType.Structured)).Value = dtSafety;

                    //cmd.Parameters.Add("@TotalProcesadas", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add("@Error", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    //cmd.ExecuteScalar();

                    //TotalProcesadas = Convert.ToInt32(cmd.Parameters["@TotalProcesadas"].Value);

                    //Error = cmd.Parameters["@Error"].Value.ToString();
                    //if (TotalProcesadas == 0)
                    //{
                    //    throw new Exception(Error);
                    //}
                    cmd.Parameters.Add("@Update", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@Update"].Value) != -1)
                        {
                            if (Convert.ToInt32(cmd.Parameters["@Update"].Value) == 1)
                            {
                                Procesadas = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            //return TotalProcesadas;
            return Procesadas;
        }

        public async Task<ManifiestosErrores> CargarManifiestoJCJFAsync(ManifiestoFile objJCJF)
        {
            ManifiestosErrores objCustomAlertsErrores = new();
            BitacoraJCJFRepository objBitacora = new(_configuration);
           
            try
            {
                ApiJCJFManifiestoRepository objApiManifiestoJCJF = new(_configuration);
                RequestJCJFManifiesto objRequestArchivo = new()
                {
                    Manifiesto = objJCJF.ArchivoBase64,
                    Master = objJCJF.TxtGuiaMaster
                };

                ResposeJCJFManifiesto objResponse = await objApiManifiestoJCJF.RequestCargarManifiestoJCJF(objRequestArchivo);

                if (objResponse.Datos == null)
                {
                    if (objResponse.Error is string errorString) // Verifica si objResponse.Error es de tipo string

                        objCustomAlertsErrores.lstErrores = new List<string> { errorString }; // Convierte la cadena a una lista de cadenas                   
                }
                else
                {
                    objCustomAlertsErrores.txtProcesadas = Convert.ToInt32(objResponse.Datos.RegistrosProcesados);
                }
                objBitacora.InsertarManifieto(objResponse, objJCJF);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objCustomAlertsErrores;
        }

       
      
        private decimal CalcularPeso(string valor)
        {            
                if (valor.Contains("LB"))
                    return Convert.ToDecimal(valor.Replace("LB", "")) * 0.4536M;
                else
                    return Convert.ToDecimal(valor.Replace("K", ""));                      
        }


        public void ValidarObjeto(ManifiestoFile objManifiestoFedexFile)
        {
            if (objManifiestoFedexFile.TxtGuiaMaster == "")
            {
                throw new Exception("Falta escribir la Guía Master");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////

        public ManifiestosErrores CargarManifiestoCMF(ManifiestoFile objManifiestoFile)
        {           
            ManifiestosErrores objCustomAlertsErrores = new();

            string contenidoManifiesto = "";

            UbicaciondeArchivosRepository objUbicacionDeArchivosRepository = new(_configuration);
            string NombreArchivo = objManifiestoFile.NombreArchivo;

            UbicaciondeArchivos objRuta = objUbicacionDeArchivosRepository.Buscar(47);
            if (Directory.Exists(objRuta.Ubicacion) == false)
            {
                Directory.CreateDirectory(objRuta.Ubicacion);
            }

            NombreArchivo = objRuta.Ubicacion + NombreArchivo;
            Byte[] bytes = Convert.FromBase64String(objManifiestoFile.ArchivoBase64);
            System.IO.File.WriteAllBytes(NombreArchivo, bytes);

            using (var sr = new StreamReader(NombreArchivo))
            {
                contenidoManifiesto = sr.ReadToEnd();
                sr.Close();
            }           

            objCustomAlertsErrores = CargarManifiestoCMF(objManifiestoFile, contenidoManifiesto);          

            return objCustomAlertsErrores;

        }

        public ManifiestosErrores CargarManifiestoCMF(ManifiestoFile objManifiestoFile, string contenidoManifiesto)
        {
            Publicas publicas = new();
            List<string> lstErrores = new();
            int vContador = 0;
            //int vProcesadas = 0;
            //int IdCustomAlert = 0;
            bool inicioProcesamiento = false;
          
            DataTable dt = CrearTablaCMF();

            //string contenidoManifiesto = "";

            foreach (Match m in Regex.Matches(contenidoManifiesto, "^.*$", RegexOptions.Multiline))
            {
                vContador++;
                string linea = m.ToString();

                if (!inicioProcesamiento)
                {
                    if (linea.Contains("Tipo2"))
                        inicioProcesamiento = true;
                    else
                        continue;
                }

                if (string.IsNullOrWhiteSpace(linea))
                    break;

                try
                {                  
                    
                    string[] campos = Regex.Split(linea, Char.ConvertFromUtf32(9));
                    if (campos.Length <= 1)
                        continue;

                    //string guiaHouse = publicas.EliminaComillas(campos[3].Trim());
                    //if (!Regex.IsMatch(guiaHouse, @"\d"))
                    //    continue;

                    string guiaMaster = publicas.EliminaComillas(campos[4].Trim());
                    if (!Regex.IsMatch(guiaMaster, @"\d"))
                        continue;

                    DataRow dataRow = dt.NewRow();
                    dataRow["Fila"] = vContador;
                    
                    try
                    
                    {
                        AsignarCamposCMF(dataRow, campos, vContador, objManifiestoFile);
                        dt.Rows.Add(dataRow);
                        //vProcesadas++;
                    }
                    catch (Exception ex)
                    {
                        lstErrores.Add($"Fila {vContador}: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    lstErrores.Add($"Fila {vContador}: {ex.Message}");
                }
            }
            int totalErrores = lstErrores.Count;
            int totalFilas = dt.Rows.Count;
            int totalInsertadas = 0;
            int totalEnviadas = totalFilas - totalErrores;
            if (dt.Rows.Count > 0)
            {
                try
                {                 
                    var (TotalInsertadas, ErrorMessage) = DT_InsertarCMF(dt, objManifiestoFile);
                    totalInsertadas = TotalInsertadas;
                    if (ErrorMessage!="")
                    {
                        lstErrores.Add(ErrorMessage);
                    }
                   
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " Falla la inserción en la tabla");
                }
            }

     
            
       
            totalErrores = totalEnviadas - totalInsertadas;
            int totalProcesadas = totalFilas - totalErrores;

            return new ManifiestosErrores
            {
                txtTotalError = totalErrores,
                txtFilas = totalFilas,            
                txtProcesadas = totalProcesadas,
                lstErrores = lstErrores 
            };
        }

        public ( int TotalInsertadas, string ErrorMessage) DT_InsertarCMF(DataTable dt, ManifiestoFile objManifiestoFile)
        {
            bool esImpo = false;

            if (objManifiestoFile.idManifiesto == 7)
            {
                esImpo = false;
            }
            else if (objManifiestoFile.idManifiesto == 8)
            {
                esImpo = true;
            }

            int totalInsertadas;
            string errorMessage;
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new(esImpo
                    ? ""
                    : "NET_CASAEI_INSERT_CMF_TableType", con);

                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@dt", SqlDbType.Structured) { Value = dt });
                cmd.Parameters.Add(new SqlParameter("@IdOficina", SqlDbType.Int) { Value = objManifiestoFile.IdOficina });
                cmd.Parameters.Add(new SqlParameter("@NombreArchivo", SqlDbType.VarChar, 250) { Value = objManifiestoFile.NombreArchivo });
                cmd.Parameters.Add(new SqlParameter("@TotalProcesadas", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                });

                cmd.Parameters.Add(new SqlParameter("@ErrorMessage", SqlDbType.VarChar, 1000)
                {
                    Direction = ParameterDirection.Output
                });

                con.Open();
                cmd.ExecuteScalar();
                totalInsertadas = Convert.ToInt32(cmd.Parameters["@TotalProcesadas"].Value);
                errorMessage = (string)cmd.Parameters["@ErrorMessage"].Value;
            }
            catch (Exception ex)
            {
                string nombreSP = esImpo
                    ? ""
                    : "NET_CASAEI_INSERT_CMF_TableType";
                throw new Exception($"{ex.Message} - Error al ejecutar SP: {nombreSP}");
            }

            return (totalInsertadas, errorMessage);
        }

        private static DataTable CrearTablaCMF()
        {
            DataTable dt = new();

            dt.Columns.Add("Fila", typeof(int));
            dt.Columns.Add("IDDatosDeEmpresa", typeof(int));

            dt.Columns.Add("GuiaHouse", typeof(string)); //4
            dt.Columns.Add("GtwDestino", typeof(string)); //8
            dt.Columns.Add("IataOrigen", typeof(string)); //6
            dt.Columns.Add("IataDestino", typeof(string)); //8
            dt.Columns.Add("TipoEnvio", typeof(string)); //52
            dt.Columns.Add("Descripcion", typeof(string)); //25
            dt.Columns.Add("NoCuenta", typeof(string)); // 53
            dt.Columns.Add("Destinatario", typeof(string)); // 26
            dt.Columns.Add("Direccion1", typeof(string)); //27
            dt.Columns.Add("Direccion2", typeof(string)); //28
            dt.Columns.Add("Direccion3", typeof(string)); //29
            dt.Columns.Add("Ciudad", typeof(string)); // 49
            dt.Columns.Add("CodigoPostal", typeof(string)); // 30
            dt.Columns.Add("Pais", typeof(string)); //33
            dt.Columns.Add("Contacto", typeof(string)); //26           
            dt.Columns.Add("DatosContacto", typeof(string));  //34
            dt.Columns.Add("Proveedor", typeof(string)); //15
            dt.Columns.Add("ProveedorDireccion", typeof(string)); //17         
            dt.Columns.Add("ProveedorCiudad", typeof(string)); //19
            dt.Columns.Add("ProveedorEstado", typeof(string)); //18
            dt.Columns.Add("ProveedorPais", typeof(string)); //7 -23
            dt.Columns.Add("ProveedorCodigoPostal", typeof(string)); //20            
            dt.Columns.Add("ProveedorDatos", typeof(string)); //24
            dt.Columns.Add("Peso", typeof(decimal)); //11
            dt.Columns.Add("PesoVolumetrico", typeof(decimal)); //36
            dt.Columns.Add("Piezas", typeof(int)); //63
            dt.Columns.Add("Incoterm", typeof(string)); //59
            dt.Columns.Add("ServicioDhl", typeof(string)); //57
            dt.Columns.Add("FacturaValor", typeof(decimal)); //13
            dt.Columns.Add("FacturaMoneda", typeof(string)); //14
            dt.Columns.Add("PaisVendedor", typeof(string)); //23
            dt.Columns.Add("PaisComprador", typeof(string)); //33            
            dt.Columns.Add("GuiaMaster", typeof(string));//3         
            dt.Columns.Add("NoCuentaCliente", typeof(string)); //55            
            dt.Columns.Add("Valija", typeof(string)); //65
            dt.Columns.Add("Contenedor", typeof(string)); //66           
            dt.Columns.Add("IdUsuario", typeof(int));
            return dt;
        }

        void AsignarCamposCMF(DataRow dataRow, string[] vArreglo, int fila, ManifiestoFile obj)
        {
            AsignarCampo(dataRow, "IDDatosDeEmpresa", () => obj.IDDatosDeEmpresa, fila);

            AsignarCampo(dataRow, "GuiaHouse", () => vArreglo[4].Trim(), fila, true);
            AsignarCampo(dataRow, "GtwDestino", () => vArreglo[8].Trim(), fila, true);
            AsignarCampo(dataRow, "IataOrigen", () => vArreglo[6].Trim(), fila, true);
            AsignarCampo(dataRow, "IataDestino", () => vArreglo[8].Trim(), fila, true);
            AsignarCampo(dataRow, "TipoEnvio", () => vArreglo[52].Trim(), fila, true);
            AsignarCampo(dataRow, "Descripcion", () => vArreglo[12].Trim(),fila, true);
            AsignarCampo(dataRow, "NoCuenta", () => vArreglo[54].Trim(), fila, true);
            AsignarCampo(dataRow, "Destinatario", () => vArreglo[26].Trim(), fila, true);
            AsignarCampo(dataRow, "Direccion1", () => vArreglo[27].Trim(), fila, true);
            AsignarCampo(dataRow, "Direccion2", () => vArreglo[28].Trim(), fila, true);
            AsignarCampo(dataRow, "Direccion3", () => vArreglo[29].Trim(), fila, true);

            AsignarCampo(dataRow, "Ciudad", () => vArreglo[49].Trim(), fila, true);
            AsignarCampo(dataRow, "CodigoPostal", () => vArreglo[30].Trim(),fila, true);
            AsignarCampo(dataRow, "Pais", () => vArreglo[33].Trim(), fila, true);
            AsignarCampo(dataRow, "Contacto", () => vArreglo[26].Trim(), fila, true);
            AsignarCampo(dataRow, "DatosContacto", () => vArreglo[34].Trim(), fila, true);

            // Proveedor
            AsignarCampo(dataRow, "Proveedor", () => vArreglo[15].Trim(), fila, true);
            AsignarCampo(dataRow, "ProveedorDireccion", () => vArreglo[17].Trim(), fila, true);          
            AsignarCampo(dataRow, "ProveedorCiudad", () => vArreglo[19].Trim(), fila, true);
            AsignarCampo(dataRow, "ProveedorEstado", () => vArreglo[21].Trim(), fila, true);
            AsignarCampo(dataRow, "ProveedorPais", () => vArreglo[23].Trim(), fila, true);
            AsignarCampo(dataRow, "ProveedorCodigoPostal", () => vArreglo[20].Trim(), fila, true);
            AsignarCampo(dataRow, "ProveedorDatos", () => vArreglo[24].Trim(), fila, true);
                        
            AsignarCampo(dataRow, "Peso", () => vArreglo[11].Trim(), fila);
            AsignarCampo(dataRow, "PesoVolumetrico", () => vArreglo[36].Trim(), fila, true);                        
            AsignarCampo(dataRow, "Piezas", () => vArreglo[63].Trim(), fila, true);
            AsignarCampo(dataRow, "Incoterm", () => vArreglo[59].Trim(), fila, true);
            AsignarCampo(dataRow, "ServicioDhl", () => vArreglo[57].Trim(), fila, true);
            AsignarCampo(dataRow, "FacturaValor", () => vArreglo[13].Trim(), fila, true);
            AsignarCampo(dataRow, "FacturaMoneda", () => vArreglo[14].Trim(), fila, true);
            AsignarCampo(dataRow, "PaisVendedor", () => vArreglo[23].Trim(), fila, true);
            AsignarCampo(dataRow, "PaisComprador", () => vArreglo[33].Trim(), fila, true);

            AsignarCampo(dataRow, "GuiaMaster", () => vArreglo[3].Trim(), fila, true);

            AsignarCampo(dataRow, "NoCuentaCliente", () => vArreglo[55].Trim(), fila, true);
            AsignarCampo(dataRow, "Valija", () => vArreglo[65].Trim(), fila, true);
            AsignarCampo(dataRow, "Contenedor", () => vArreglo[66].Trim(), fila, true);

           
            AsignarCampo(dataRow, "IdUsuario", () => obj.IdUsuario, fila);
        }

        ///////////////////////////////////////////////////////////////////////////////



    }
}

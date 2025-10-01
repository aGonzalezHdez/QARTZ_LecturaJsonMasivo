using LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.Safe;
using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesSafe
{
    public class WebApiSafeRepository : IWebApiSafeRepository
    {
        public string SConexion { get; set; }
        string IWebApiSafeRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public WebApiSafeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public string? Msg { get; set; }
        Referencias? objRefe = new();

        public RespuestaJuliano GenerarJulianoSafe(JulianoSiemens objJulianoSiemens)
        {
            int IdReferencia = objJulianoSiemens.IdReferencia;

            RespuestaJuliano objrespuestaJuliano = new();
            try
            {
                objRefe = ValidarReferencias(IdReferencia);

                if (objRefe == null)
                {
                    objrespuestaJuliano.bError = true;
                    objrespuestaJuliano.Error = "Las referencia no existe en el sistema expertti.";
                }
                else
                {
                    SaaioPedimeRepository objSaaioPedimeRepository = new(_configuration);
                    SaaioPedime objSaaioPedime = objSaaioPedimeRepository.Buscar(objRefe.NumeroDeReferencia);

                    if (objSaaioPedime is not null)
                    {
                        DataTable dtb = ToDataTable(new List<int> { IdReferencia });

                        using (SqlConnection con = new(SConexion))
                        using (SqlCommand cmd = new("juliano.NET_INSERT_Juliano_Siemens", con))
                        {
                            con.Open();

                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@Aduana", SqlDbType.VarChar, 3).Value = objRefe.AduanaDespacho;
                            cmd.Parameters.Add("@Patente", SqlDbType.VarChar, 4).Value = objRefe.Patente;
                            cmd.Parameters.Add("@Movimiento", SqlDbType.Int, 4).Value = 1;
                            cmd.Parameters.Add("@Prevalidador", SqlDbType.VarChar, 3).Value = objJulianoSiemens.CvePrev;
                            cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4).Value = objRefe.Operacion;
                            cmd.Parameters.Add("@idOficina", SqlDbType.Int, 4).Value = objRefe.IdOficina;
                            cmd.Parameters.Add("@idUsuario", SqlDbType.Int, 4).Value = objJulianoSiemens.IdUsuario;
                            cmd.Parameters.Add("@Representante", SqlDbType.VarChar, 2).Value = objSaaioPedime.CVE_REPR;
                            cmd.Parameters.Add("@Tabla", SqlDbType.Structured).Value = dtb;
                            cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@Archivo", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@ArchivoTMP", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("@RFCAA", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                            cmd.ExecuteReader();


                            if (Convert.ToInt32(cmd.Parameters["@NEWID_REGISTRO"].Value) != -1)
                            {
                                objrespuestaJuliano.idJuliano = Convert.ToInt32(cmd.Parameters["@NEWID_REGISTRO"].Value);
                                objrespuestaJuliano.Archivo = cmd.Parameters["@Archivo"].Value.ToString();
                            }
                        }
                    }
                    else
                    {
                        objrespuestaJuliano.bError = true;
                        objrespuestaJuliano.Error = "La referencia no existe en el sistema de pedimentos CASA.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objrespuestaJuliano;
        }
        public Referencias ValidarReferencias(int IdReferencia)
        {
            Referencias objRefe = new();
            ReferenciasRepository objRefeD = new(_configuration);

            if (IdReferencia != 0)
            {
                objRefe = objRefeD.Buscar(IdReferencia);
            }
            return objRefe;
        }

        public static DataTable ToDataTable(IEnumerable<int> items)
        {
            DataTable dataTable = new("Integers");

            // Añadir una columna para los enteros
            dataTable.Columns.Add("Value", typeof(int));

            foreach (int item in items)
            {
                // Insertar el valor entero en una nueva fila
                DataRow row = dataTable.NewRow();
                row["Value"] = item;
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }

        public List<DropDownListDatos> CargarTiposdeFiltro()
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_CASAEI_TIPOSDEFILTRO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    using SqlDataReader reader = cmd.ExecuteReader();
                    comboList = SqlDataReaderToDropDownList.DropDownList<DropDownListDatos>(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return comboList.ToList();
        }

        public List<DropDownListDatos> CargarJustificaciones()
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_CASAEI_CATALOGODEJUSTIFICACIONES", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    using SqlDataReader reader = cmd.ExecuteReader();
                    comboList = SqlDataReaderToDropDownList.DropDownList<DropDownListDatos>(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return comboList.ToList();
        }

        public async Task<ResponseHallazgos> ObtenerHallazgos(RutaJuliano objRutaJuliano)
        {
            ResponseHallazgos? objResponseHallazgos = new();
            SafeRepository ObjSafeApiRepository = new(_configuration);

            objRefe = ValidarReferencias(objRutaJuliano.IdReferencia);
            Safe ObjSafe = ObjSafeApiRepository.Buscar(objRefe.IdOficina, objRefe.IDCliente);

            try
            {
                if (ObjSafe is not null)
                {
                    string uriObtenerHallazgos = ObjSafe.UriObtenerHallazgos;
                    string usuario = ObjSafe.Usuario;
                    string contraseña = ObjSafe.Token;
                    // Obtener hallazgos
                    RequestHallazgosValidar ObjJsonRequestHallazgos = new();
                    RequestCredenciales ObjCredenciales = new()
                    {
                        Usuario = usuario,
                        Password = contraseña
                    };
                    ObjJsonRequestHallazgos.Credenciales = ObjCredenciales;
                    string ruta = objRutaJuliano.RutaArchivo;
                    string StringBase64 = ConvertFileToBase64(ruta);
                    string NombreArchivo = Path.GetFileName(ruta);

                    RequestArchivo ObjArchivo = new()
                    {
                        rfc = ObjSafe.RFC.Trim(),
                        fileName = NombreArchivo,
                        JulianoBase64 = StringBase64,
                        envioNotificacionSuite = true
                    };

                    ObjJsonRequestHallazgos.Archivo = ObjArchivo;
                    objResponseHallazgos = await RequestHallazgosValidar(ObjJsonRequestHallazgos, uriObtenerHallazgos);

                    if(string.IsNullOrEmpty(objResponseHallazgos.message))
                    {
                        string Error = "";
                        int contadorErrores = 1;

                        if (objResponseHallazgos.success == false)
                        {
                            List<string>? ListErrores = objResponseHallazgos.Errores;
                            foreach (var item in ListErrores)
                            {
                                //Errores = item.ToString();
                                //throw new Exception(Errores);  
                                Error += " " + contadorErrores + ".- "+ item.ToString();
                                contadorErrores++;
                            }
                            objResponseHallazgos.Error = "Api Safe:" + Error;
                            objResponseHallazgos.bError = true;
                        }
                        else
                        {
                            int nivel3 = 3;
                            List<int> valores = new();

                            foreach (var item in objResponseHallazgos.Hallazgos)
                            {
                                if (item.RFC == ObjSafe.RFC.Trim())
                                {
                                    switch (item.Severidad)
                                    {
                                        case 3:
                                            {
                                                int nivel = item.Severidad;
                                                valores.Add(nivel);
                                                break;
                                            }
                                    }
                                }
                            }
                            if (valores.IndexOf(nivel3) >= 0)
                            {
                                objResponseHallazgos.Error = "Nivel de severidad 3: Es necesario volver a generar el juliano ya corregido.";
                                objResponseHallazgos.bError = true;
                            }
                        }
                    }
                    else 
                    {
                        objResponseHallazgos.bError = true;
                        objResponseHallazgos.Error = "Api Safe: " + objResponseHallazgos.message;
                    }
                }
                else
                {
                    objResponseHallazgos.Error = "No existe un usuario y contraseña en SAFE para esta oficina o este cliente";
                    objResponseHallazgos.bError = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objResponseHallazgos;
        }
        //2
        public async Task<ResponseHallazgos> RequestHallazgosValidar(object objObtenerHallazgos, string UriObtenerHallazgos)
        {
            ResponseHallazgos? objResponseHallazgos = new();

            try
            {
                WebApiSafeRepository objApi = new(_configuration);
                await objApi.ResponseHallazgos(objObtenerHallazgos, UriObtenerHallazgos);

                string? respuesta = objApi.Msg;

                if (respuesta == "")
                {
                    objResponseHallazgos.Error = "Api Safe: No se obtiene respuesta de Safe";
                    objResponseHallazgos.bError = true;
                }
                else
                {
                    objResponseHallazgos = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseHallazgos>(respuesta);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message.Trim());
            }

            return objResponseHallazgos;
        }
        //3
        public async Task<bool> ResponseHallazgos(object ObjObtenerHallazgos, string UriObtenerHallazgos)
        {
            using (HttpClient client = new())  // Se crea el Objeto para realizar la petición
            {
                try
                {
                    // Se crea la cabecera y el cuerpo
                    string lUriObtenerHallazgos = UriObtenerHallazgos;
                    client.BaseAddress = new Uri(lUriObtenerHallazgos);
                    string Json = Newtonsoft.Json.JsonConvert.SerializeObject(ObjObtenerHallazgos);
                    var response = await client.PostAsync(client.BaseAddress, new StringContent(Json, Encoding.UTF8, "application/json")); // respuesta 
                    var jHallazgos = response.Content.ReadAsStringAsync().Result;
                    var status = response.StatusCode;
                    if (status == System.Net.HttpStatusCode.Unauthorized)
                    {
                        this.Msg = jHallazgos;
                        return false;
                    }
                    else
                    {
                        status = System.Net.HttpStatusCode.Created;
                        this.Msg = jHallazgos;
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    //MsgBox("Error: " + ex.Message(), MsgBoxStyle.OkOnly);
                    return true;
                }
            }
        }

        public string ConvertFileToBase64(string fileContenido)
        {
            byte[] pdfBytes;
            pdfBytes = System.IO.File.ReadAllBytes(fileContenido);
            // Return fileContenido
            return Convert.ToBase64String(pdfBytes);
        }

        public async Task<ResponseHallazgos> ObtenerHallazgosJuliano(JulianoSiemens objJulianoSiemens)
        {
            ResponseHallazgos objResponseHallazgos = new();

            try
            {
                RespuestaJuliano objrespuestaJuliano = new();
                RutaJuliano objRutaJuliano = new();
                objrespuestaJuliano = GenerarJulianoSafe(objJulianoSiemens);

                if (objrespuestaJuliano.bError == false)
                {
                    objRutaJuliano.RutaArchivo = objrespuestaJuliano.Archivo;
                    objRutaJuliano.IdReferencia = objJulianoSiemens.IdReferencia;
                    objResponseHallazgos = await ObtenerHallazgos(objRutaJuliano);   
                }
                else
                {
                    objResponseHallazgos.bError = objrespuestaJuliano.bError;
                    objResponseHallazgos.Error = objrespuestaJuliano.Error;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objResponseHallazgos;
        }
        //4
        public async Task<ResponseHallazgos> JustificarHallazgos(List<RequestJustificarHallazgosHallazgos> lstJustificarHallazgos, int IdReferencia, int IdUsuario)
        {
            ResponseHallazgos? ObjJustificarHallazgos = new();

            try
            {
                WebApiSafeRepository ObjWebApiSafeRepository = new(_configuration);
                SafeRepository ObjSafeApiRepository = new(_configuration);
                Safe ObjSafeApi = new();

                objRefe = ValidarReferencias(IdReferencia);

                ObjSafeApi = ObjSafeApiRepository.Buscar(objRefe.IdOficina, objRefe.IDCliente);

                if (ObjSafeApi != null)
                {
                    string lUriJustificarHallazgos = ObjSafeApi.UriJustificarHallazgos;
                    string lUsuario = ObjSafeApi.Usuario;
                    string lContraseña = ObjSafeApi.Token;
                    // Justifica hallazgos
                    RequestJustificarHallazgos? ObjJsonRequestHallazgos = new();
                    RequestCredenciales ObjCredenciales = new()
                    {
                        Usuario = lUsuario,
                        Password = lContraseña
                    };

                    ObjJsonRequestHallazgos.Credenciales = ObjCredenciales;
                    ObjJsonRequestHallazgos.Hallazgos = lstJustificarHallazgos;
                    ObjJustificarHallazgos = await RequestJustificarHallazgos(ObjJsonRequestHallazgos, lUriJustificarHallazgos);

                    if (ObjJustificarHallazgos.success == true)
                    {
                        //LLena la chekpoint la base de datos CASAEI
                        ChckPoint(lstJustificarHallazgos, objRefe.IDDatosDeEmpresa, objRefe.IdOficina, objRefe.IDReferencia, IdUsuario);
                    }
                }
                else
                {
                    ObjJustificarHallazgos.bError = true;
                    ObjJustificarHallazgos.Error = "No existe un usuario y contraseña en SAFE para esta oficina o este cliente";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return ObjJustificarHallazgos;
        }
        //5
        public async Task<ResponseHallazgos> RequestJustificarHallazgos(object ObjObtenerHallazgos, string UriObtenerHallazgos)
        {
            ResponseHallazgos? ObjJustificarHallazgos = new();
            WebApiSafeRepository objApi = new(_configuration);

            try
            {
                await objApi.ResponseJustificarHallazgos(ObjObtenerHallazgos, UriObtenerHallazgos);
                string? respuesta = objApi.Msg;

                if (respuesta == "")
                {
                    ObjJustificarHallazgos.bError = true;
                    ObjJustificarHallazgos.Error = "Api Safe: No se obtiene respuesta de Safe";
                }
                else
                {
                    ObjJustificarHallazgos = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseHallazgos>(respuesta);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message.Trim());
            }
            return ObjJustificarHallazgos;
        }
        //6
        public async Task<bool> ResponseJustificarHallazgos(object ObjObtenerHallazgos, string UriObtenerHallazgos)
        {
            using (HttpClient client = new())  // Se crea el Objeto para realizar la petición
            {
                try
                {
                    // Se crea la cabecera y el cuerpo
                    string lUriObtenerHallazgos = UriObtenerHallazgos;
                    client.BaseAddress = new Uri(lUriObtenerHallazgos);
                    string Json = Newtonsoft.Json.JsonConvert.SerializeObject(ObjObtenerHallazgos);
                    var response = await client.PostAsync(client.BaseAddress, new StringContent(Json, Encoding.UTF8, "application/json")); // respuesta 
                    var jHallazgos = response.Content.ReadAsStringAsync().Result;
                    var status = response.StatusCode;
                    if (status == System.Net.HttpStatusCode.Unauthorized)
                    {
                        this.Msg = jHallazgos;
                        return false;
                    }
                    else
                    {
                        status = System.Net.HttpStatusCode.Created;
                        this.Msg = jHallazgos;
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return true;
                }
            }
        }

        public async void ChckPoint(List<RequestJustificarHallazgosHallazgos> ObjJoson, int IDDatosDeEmpresa, int IdOficina, int IDReferencia, int IdUsuario)
        {
            ControldeEventosRepository ctrlEvD = new(_configuration);
            foreach (RequestJustificarHallazgosHallazgos item in ObjJoson)
            {
                try
                {
                    ControldeEventos lEventos = new(521, IDReferencia, IdUsuario, Convert.ToDateTime("01/01/1900"));
                    AsignarGuiasRespuesta objRespuesta = await ctrlEvD.InsertarEvento(lEventos, 521, IdOficina, false, item.MensajeCriterio, IDDatosDeEmpresa);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }
        }


    }
}






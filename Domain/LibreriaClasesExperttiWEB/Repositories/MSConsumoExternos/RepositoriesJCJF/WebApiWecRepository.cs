using LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos;
using LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.JsonWEC;
using LibreriaClasesAPIExpertti.Entities.EntitiesJsonWec;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Text;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesConsultasWsExternos;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesJCJF
{
    public class WebApiWecRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SDTConsultaWecRepository objD;
        public SDTConsultaWecPaletsRepository objPalletD;
        public WebApiWecRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
            objD = new SDTConsultaWecRepository(_configuration);
            objPalletD = new SDTConsultaWecPaletsRepository(_configuration);
        }
        public async Task<SDTConsultaWec> ConsultasWec(string GuiaHouse, int IdUsuario)
        {
            RespuestaConsultaWEC objRespuesta = new RespuestaConsultaWEC();
            SDTConsultaWec obj = new SDTConsultaWec();
            //WebApiWec ws = new WebApiWec();
            objRespuesta = await WSConsultaWec(GuiaHouse);

            int Consecutivo = 0;

            Consecutivo = objD.BuscarConsecutivo(GuiaHouse);

            foreach (var item in objRespuesta.SDTConsultaWec)
            {
                int IdWec = 0;
                obj.Consecutivo = Consecutivo.ToString();
                obj.GuiaHouse = item.House;
                obj.GuiaMaster = item.Master;
                obj.REFI = item.REFI;
                obj.RegistroEntrada = item.RegistroEntrada;
                obj.EntradaAduana = item.EntradaAduana.ToShortDateString();
                obj.AlmacenArribo = item.AlmacenArribo;
                obj.AlmacenNuevo = item.AlmacenNuevo;
                obj.RevalidadaxAgteExterno = (string)Interaction.IIf(item.RevalidadaxAgteExterno == "" || item.RevalidadaxAgteExterno == "0", "false", item.RevalidadaxAgteExterno);
                obj.MercanciaAlertada = (string)Interaction.IIf(item.MercanciaAlertada == "" || item.MercanciaAlertada == "0", "false", "true");
                obj.ClavePedimento = item.ClavePedimento;
                obj.Bultos = (string)Interaction.IIf(item.Bultos == "", 0, item.Bultos);
                obj.Peso = (string)Interaction.IIf(item.Peso == "", 0.0, item.Peso);
                obj.Salida = (string)Interaction.IIf(item.Salida == "" || item.Salida == "0", "false", "true");
                obj.RevalidaOtroAgenteAduanal = (string)Interaction.IIf(item.RevalidaOtroAgenteAduanal == "" || item.RevalidaOtroAgenteAduanal == "0", "false", "true");
                obj.Ubicacion = item.Ubicacion;

                IdWec = objD.Insertar(obj, IdUsuario);

                if (!Information.IsNothing(item.Pallets))
                {
                    foreach (var itemPallet in item.Pallets)
                    {
                        SDTConsultaWecPalets objPallet = new SDTConsultaWecPalets();

                        objPallet.Consecutivo = Consecutivo;
                        objPallet.GuiaHouse = item.House.Trim();
                        objPallet.GuiaMaster = item.Master.Trim();
                        objPallet.Bultos = itemPallet.Bultos;
                        objPallet.Peso = (decimal)itemPallet.Peso;
                        objPallet.Salidas = Convert.ToBoolean(Convert.ToInt32(itemPallet.Salidas));
                        objPallet.Ubicacion = itemPallet.Ubicacion.Trim();
                        objPallet.IdWec = IdWec;
                        objPalletD.Insertar(objPallet);
                    }
                }

                if (item.RevalidadaxAgteExterno.Trim() == "1" | item.RevalidaOtroAgenteAduanal.Trim() == "1")
                    throw new ArgumentException("!!! A T E N C I O N !!! La Guía House: " + GuiaHouse.Trim() + " ya fue revalidada a otro AGENTE ADUANAL. Usted no deberá procesarla por ningún motivo. Expertit cambiará la guía a la patente 9999 con la cual no podra ser validado el pedimento ");
            }


            return obj;
        }

        public string Msg { get; set; }
        public async Task<RevalidacionWECRespuesta> WSRevalidaWec(RevalidadosWEC objParaJson)
        {
            var objRespuesta1 = new RevalidacionWECRespuesta();
            try
            {
                //var objApi = new WebApiWec();
                string vJson = string.Empty;


                vJson = Newtonsoft.Json.JsonConvert.SerializeObject(objParaJson);


                await Cliente("PR_REST_DHL_RevalidacionAtm", vJson);

                string respuesta = Msg;
                //string respuesta = objApi.Msg;

                if (string.IsNullOrEmpty(respuesta))
                {
                    throw new ArgumentException("enviado y sin respuesta de ws Wec");
                }

                RevalidacionWECRespuesta objRespuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<RevalidacionWECRespuesta>(respuesta);

                objRespuesta1 = objRespuesta;
            }


            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message.Trim());
            }

            return objRespuesta1;
        }

        /// <summary>
        ///     ''' Metodo Post para envio de información
        ///     ''' </summary>
        ///     ''' <param name="metodo"></param>
        ///     ''' <param name="Json"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public async Task<bool> Cliente(string metodo, string Json)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://www.wec24.com.mx/WEC_REST/Rest/");

                string url = string.Format(metodo);
                var response = await client.PostAsync(url, new StringContent(Json, Encoding.UTF8, "application/json"));
                var result = response.Content.ReadAsStringAsync().Result;
                var status = response.StatusCode;

                if (status != System.Net.HttpStatusCode.OK)
                {
                    Msg = result;
                    return false;
                }
                else
                {
                    Msg = result;
                    return true;
                }
            }
            catch (Exception er)
            {
                Msg = er.Message;
                return false;
            }
        }
        public async Task<RespuestaConsultaWEC> WSConsultaWec(string pHouse)
        {
            RespuestaConsultaWEC objRespuesta1 = new RespuestaConsultaWEC();
            try
            {
                //WebApiWec objApi = new WebApiWec();
                string vJson = string.Empty;

                GuiaHouse objGuia = new GuiaHouse();
                objGuia.pHouse = pHouse.Trim();

                vJson = Newtonsoft.Json.JsonConvert.SerializeObject(objGuia);


                await Cliente("PR_WS_ConsultaWecGpoEi", vJson);

                string respuesta = Msg;

                if (respuesta == "")
                    throw new ArgumentException("enviado y sin respuesta de ws Wec");


                RespuestaConsultaWEC objRespuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<RespuestaConsultaWEC>(respuesta);

                objRespuesta1 = objRespuesta;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message.Trim());
            }

            return objRespuesta1;
        }

        public async Task<SDTConsultaWec> ConsultasWecAPI(string GuiaHouse, int IdUsuario)
        {
            var objRespuesta = new RespuestaConsultaWEC();
            var obj = new SDTConsultaWec();
            //var ws = new WebApiWec();
            var webApiWecRepository = new WebApiWecRepository(_configuration);
            objRespuesta = await webApiWecRepository.WSConsultaWec(GuiaHouse);

            int Consecutivo = 0;
            var objD = new SDTConsultaWecRepository(_configuration);
            Consecutivo = objD.BuscarConsecutivo(GuiaHouse);

            foreach (var item in objRespuesta.SDTConsultaWec)
            {
                int IdWec = 0;
                obj.Consecutivo = Consecutivo.ToString();
                obj.GuiaHouse = ((dynamic)item).House;
                obj.GuiaMaster = ((dynamic)item).Master;
                obj.REFI = ((dynamic)item).REFI;
                obj.RegistroEntrada = ((dynamic)item).RegistroEntrada;
                obj.EntradaAduana = ((dynamic)item).EntradaAduana;
                obj.AlmacenArribo = ((dynamic)item).AlmacenArribo;
                obj.AlmacenNuevo = ((dynamic)item).AlmacenNuevo;
                obj.RevalidadaxAgteExterno = item.RevalidadaxAgteExterno;
                obj.MercanciaAlertada = item.MercanciaAlertada;
                obj.ClavePedimento = item.ClavePedimento;
                obj.Bultos = item.Bultos;
                obj.Peso = item.Peso;
                obj.Salida = item.Salida;
                obj.RevalidaOtroAgenteAduanal = item.RevalidaOtroAgenteAduanal;
                obj.Ubicacion = ((dynamic)item).Ubicacion;

                IdWec = objD.Insertar(obj, IdUsuario);

                if (!(((dynamic)item).Pallets == null))
                {
                    foreach (var itemPallet in item.Pallets)
                    {
                        var objPallet = new SDTConsultaWecPalets();
                        var objPalletD = new SDTConsultaWecPaletsRepository(_configuration);


                        objPallet.Consecutivo = Consecutivo;
                        objPallet.GuiaHouse = ((dynamic)item).House.Trim;
                        objPallet.GuiaMaster = ((dynamic)item).Master.Trim;
                        objPallet.Bultos = ((dynamic)itemPallet).Bultos;
                        objPallet.Peso = ((dynamic)itemPallet).Peso;
                        objPallet.Salidas = ((dynamic)itemPallet).Salidas;
                        objPallet.Ubicacion = ((dynamic)itemPallet).Ubicacion.Trim;
                        objPallet.IdWec = IdWec;
                        objPalletD.Insertar(objPallet);
                    }
                }

                if (item.RevalidadaxAgteExterno.Trim() == "1" || item.RevalidaOtroAgenteAduanal.Trim() == "1")
                {
                    throw new ArgumentException("!!! A T E N C I O N !!! La Guía House: " + GuiaHouse.Trim() + " ya fue revalidada a otro AGENTE ADUANAL. Usted no deberá procesarla por ningún motivo. Expertit cambiará la guía a la patente 9999 con la cual no podra ser validado el pedimento ");
                }

            }


            return obj;
        }
    }
}

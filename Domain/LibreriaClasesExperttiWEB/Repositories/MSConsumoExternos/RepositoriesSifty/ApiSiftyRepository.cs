using iTextSharp.text;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesCMF;
using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using LibreriaClasesAPIExpertti.Entities.EntitiesSifty;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesTurnado;
using LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesSifty
{
    public class ApiSiftyRepository
    {
        public string Msg = string.Empty;
        public WebApis objAPi = new();
        public IConfiguration _configuration;
        public ApiSiftyRepository(IConfiguration configuration)
        {
            WebApisRepository objAPiD = new(configuration);
            objAPi = objAPiD.Buscar(23);
            _configuration = configuration;
        }
        private  SiftyRespuesta post(string Json)
        {
            SiftyRespuesta objRespuesta=new ();
            try
            {
                string metodo = "import";
               

                System.Net.ServicePointManager.SecurityProtocol= System.Net.SecurityProtocolType.Tls12;
     
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(objAPi.URL );
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", objAPi.Token.Trim());
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                string url = string.Format(metodo);
                // var response = await client.PostAsync (url, new StringContent(Json, Encoding.UTF8, "application/json"));
                var response =  client.PostAsync(url, new StringContent(Json, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();

                // var result = response.Content.ReadAsStringAsync().Result;
                var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var status = response.StatusCode;

                if (status == System.Net.HttpStatusCode.OK)
                {
                    this.Msg = result.ToString();
                    string respuesta = Msg;

                     objRespuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<SiftyRespuesta>(respuesta);
                }
                else
                {
                    objRespuesta = null;
                    this.Msg = result.ToString();

                    string respuesta = Msg;

                    if (respuesta == "")
                        throw new ArgumentException("enviado y sin respuesta de ws sifty");
                  
                }
            }
            catch (Exception er)
            {
                this.Msg = er.Message;
                objRespuesta = null;
            }

            return objRespuesta;
        }

        private async Task<SiftyRespuesta> postAsync(string Json)
        {
            SiftyRespuesta objRespuesta = new();
            try
            {
                string metodo = "import";


                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(objAPi.URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", objAPi.Token.Trim());
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                string url = string.Format(metodo);
                var response = await client.PostAsync (url, new StringContent(Json, Encoding.UTF8, "application/json"));
                //var response = client.PostAsync(url, new StringContent(Json, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();

                var result = response.Content.ReadAsStringAsync().Result;
                //var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var status = response.StatusCode;

                if (status == System.Net.HttpStatusCode.OK)
                {
                    this.Msg = result.ToString();
                    string respuesta = Msg;

                    objRespuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<SiftyRespuesta>(respuesta);
                }
                else
                {
                    objRespuesta = null;
                    this.Msg = result.ToString();

                    string respuesta = Msg;

                    if (respuesta == "")
                        throw new ArgumentException("enviado y sin respuesta de ws sifty");

                }
            }
            catch (Exception er)
            {
                this.Msg = er.Message;
                objRespuesta = null;
            }

            return objRespuesta;
        }


        public bool EnviarSifty(int idCMF)
        {
            bool enviado= false;
            try
            {
                CustomerMasterFile customerMasterFile = new ();
                CustomerMasterFileRepository customerMasterFileD = new (_configuration);
                customerMasterFile = customerMasterFileD.BuscarId(idCMF);

                if (customerMasterFile == null)
                {
                    throw new ArgumentException("La house no existe en CustomerMasterFile");
                }
                if (customerMasterFile.IdCategoria == 5)
                {
                    Siftydata  siftydata = new();
                    siftydata.shipper = customerMasterFile.Proveedor.Trim();
                    siftydata.consignee = customerMasterFile.Destinatario.Trim();
                    siftydata.packages = customerMasterFile.Piezas;
                    siftydata.weight = customerMasterFile.Peso;
                    siftydata.description = customerMasterFile.Descripcion.Trim();
                    siftydata.customs_value = customerMasterFile.ValorDolares;

                    List<Siftydata> lstsiftydata = new ();
                    lstsiftydata.Add(siftydata);

                    SiftyEnvio siftyEnvio = new SiftyEnvio();
                    siftyEnvio.data = lstsiftydata;

                   string vJson = Newtonsoft.Json.JsonConvert.SerializeObject(siftyEnvio);

                   SiftyRespuesta objRespuesta = new ();
                   objRespuesta =post(vJson);

                    foreach (Siftydata item in objRespuesta.predictions)
                    {
                        int NoAbrir = 1;
                        if (item.suggestion.Trim() == "low_risk") { NoAbrir = 3; }
                        else { NoAbrir = 2; }

                        customerMasterFileD.modificarSifty(idCMF, NoAbrir);
                    }
          


                    enviado = true;
                }
                else { enviado=false; }
            }
            catch (Exception)
            {
                enviado = false;
                
            }
        
            
            return enviado;

        }
       

        public bool EnviarSiftyCA(int idCA)
        {
            bool enviado = false;
            try
            {
                CustomsAlerts customsAlerts = new();
                CustomsAlertsRepository customsAlertsD = new(_configuration);
                customsAlerts = customsAlertsD.BuscarPorId(idCA);

                if (customsAlerts == null)
                {
                    throw new ArgumentException("La house no existe en CustomerMasterFile");
                }
                if (customsAlerts.IdCategoria == 5)
                {
                    Siftydata siftydata = new();
                    siftydata.shipper = customsAlerts.Remitente.Trim();
                    siftydata.consignee = customsAlerts.Cliente.Trim();
                    siftydata.packages = customsAlerts.Piezas;
                    siftydata.weight = customsAlerts.PesoTotal;
                    siftydata.description = customsAlerts.Descripcion.Trim();
                    siftydata.customs_value = customsAlerts.ValorEnDolares;

                    List<Siftydata> lstsiftydata = new();
                    lstsiftydata.Add(siftydata);

                    SiftyEnvio siftyEnvio = new SiftyEnvio();
                    siftyEnvio.data = lstsiftydata;

                    string vJson = Newtonsoft.Json.JsonConvert.SerializeObject(siftyEnvio);

                    SiftyRespuesta objRespuesta = new();
                    objRespuesta = post(vJson);

                    foreach (Siftydata item in objRespuesta.predictions)
                    {
                        int NoAbrir = 1;
                        if (item.suggestion.Trim() == "low_risk") { NoAbrir = 3; }
                        else { NoAbrir = 2; }

                        customsAlertsD.modificarSifty(idCA, NoAbrir);
                    }



                    enviado = true;
                }
                else { enviado = false; }
            }
            catch (Exception)
            {
                enviado = false;

            }


            return enviado;

        }

        public async Task<bool> EnviarSiftyAsync(List<CustomsAlerts> lstGuiaHouse)
        {         
            bool enviado = false;
            try
            {
                //CustomsAlerts customsAlerts = new();
                CustomsAlertsRepository customsAlertsD = new(_configuration);
                //customsAlerts = customsAlertsD.BuscarPorId(idCA);

                //if (customsAlerts == null)
                //{
                //    throw new ArgumentException("La house no existe en CustomerMasterFile");
                //}

                foreach (CustomsAlerts itemAlerts in lstGuiaHouse)
                {
                    if (itemAlerts.IdCategoria == 5)
                    {
                        Siftydata siftydata = new();
                        siftydata.shipper = itemAlerts.Remitente.Trim();
                        siftydata.consignee = itemAlerts.Cliente.Trim();
                        siftydata.packages = itemAlerts.Piezas;
                        siftydata.weight = itemAlerts.PesoTotal;
                        siftydata.description = itemAlerts.Descripcion.Trim();
                        siftydata.customs_value = itemAlerts.ValorEnDolares;

                        List<Siftydata> lstsiftydata = new();
                        lstsiftydata.Add(siftydata);

                        SiftyEnvio siftyEnvio = new SiftyEnvio();
                        siftyEnvio.data = lstsiftydata;

                        string vJson = Newtonsoft.Json.JsonConvert.SerializeObject(siftyEnvio);

                        SiftyRespuesta objRespuesta = new();
                        objRespuesta = await postAsync(vJson);

                        foreach (Siftydata item in objRespuesta.predictions)
                        {
                            int NoAbrir = 1;
                            if (item.suggestion.Trim() == "low_risk") { NoAbrir = 3; }
                            else { NoAbrir = 2; }

                            await customsAlertsD.modificarSiftyAsync(itemAlerts.IdCustomAlert, NoAbrir);
                        }
                        enviado = true;
                    }
                    else 
                    { 
                        enviado = false;
                    }
                }
            }
            catch (Exception)
            {
                enviado = false;
            }
            return enviado;
        }
    }
}

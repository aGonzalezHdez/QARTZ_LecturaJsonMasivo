using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesJsonDHL;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesDHL.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSDespachos.RepositoriesSalidas;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.RegularExpressions;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesDHL
{
    public class APIGenerarFac : IAPIGenerarFac
    {

        public IConfiguration _configuration;
        public string gtw = string.Empty;
        public int idReferencia;

        public APIGenerarFac(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string JsonFac(int vidReferencia)
        {
            idReferencia = vidReferencia;
            string vJson = string.Empty;
            int IDDatosDeEmpresa = 1;
            try
            {

                Referencias objRefe = new();
                ReferenciasRepository objRefeD = new(_configuration);
                objRefe = objRefeD.Buscar(idReferencia, 1);
                if (objRefe == null)
                {
                    throw new ArgumentException("No existe id de la referencia");
                }
                idReferencia = objRefe.IDReferencia;

                CatalogoDeOficinas oficinas = new();
                CatalogoDeOficinasRepository oficinasD = new(_configuration);
                oficinas = oficinasD.Buscar(objRefe.IdOficina);
                if (oficinas == null)
                {
                    throw new ArgumentException("No existe id de la referencia");
                }
                gtw = oficinas.GTWFac;

                List<JsonFac> lst = new();


                DataTable dtb = new();
                FacturasReporitory objFac = new(_configuration);
                dtb = objFac.ArchivoFacEnDatatable(idReferencia);

                foreach (DataRow item in dtb.Rows)
                {
                    JsonFac objFacj = new();
                    string referencef = item["NumeroDePedimento"].ToString();
                    string x = referencef.Substring(4, 1);

                    objFacj.reference = "C" + referencef.Substring(4, 7);
                    objFacj.pediment_num = item["NumeroDePedimento"].ToString();
                    objFacj.fecha_entrada = Convert.ToDateTime(item["FechaDeEntrada"]);
                    objFacj.bultos = Convert.ToInt32(item["CantidadDeBultos"]);
                    objFacj.peso = Convert.ToDouble(item["Peso"]);
                    objFacj.guia = item["NumeroDeGuia"].ToString();
                    objFacj.valor_mercan = Convert.ToDouble(item["ValorComercia"]);
                    objFacj.valor_factura = Convert.ToDouble(item["ValorDolares"]);
                    objFacj.valor_aduana = Convert.ToDouble(item["ValorAduana"]);
                    objFacj.descripcion = item["Descripcion"].ToString();
                    objFacj.tipo_cambio = Convert.ToDouble(item["TipoDeCambio"]);
                    objFacj.tipo_pediment = item["Operacion"].ToString();
                    objFacj.clave_pediment = item["ClaveDePedimento"].ToString();
                    objFacj.valor_increm = Convert.ToDouble(item["Incrementables"]);
                    objFacj.fecha_pago_ped = Convert.ToDateTime(item["FechaDePago"]);
                    objFacj.impuestos = Convert.ToDouble(item["TotalDeImpuestos"]);
                    objFacj.servicios = Convert.ToDouble(item["Servicios"]);
                    objFacj.total_factura = Convert.ToDouble(item["TotalFactura"]);
                    objFacj.fkrfc = item["RFC"].ToString();
                    objFacj.razon_social = EliminaAcentos(item["Importador"].ToString());
                    objFacj.direccion = EliminaAcentos(item["Direccion"].ToString());
                    objFacj.no_ext = EliminaAcentos(item["NumeroExt"].ToString());
                    objFacj.no_int = EliminaAcentos(item["NumeroInt"].ToString());
                    objFacj.colonia = EliminaAcentos(item["colonia"].ToString());
                    objFacj.cp = EliminaAcentos(item["CodigoPostal"].ToString());
                    objFacj.work_num = "";
                    objFacj.stat_trx = item["Stat"].ToString();
                    objFacj.date_trx = Convert.ToDateTime(item["Datetrx"]);
                    objFacj.numctl_net = Convert.ToInt32(item["NumctlNet"]);
                    objFacj.calle = item["Calle"].ToString();
                    objFacj.estado = item["Estado"].ToString();
                    objFacj.municipio = item["Municipio"].ToString();

                    lst.Add(objFacj);
                }




                vJson = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Hubo un error para generar el Json:" + ex.Message.Trim());
            }

            return vJson;
        }

        public static string EliminaAcentos(string inputString)
        {
            Regex replace_a_Accents = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex replace_e_Accents = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex replace_i_Accents = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex replace_o_Accents = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex replace_u_Accents = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            inputString = replace_a_Accents.Replace(inputString, "a");
            inputString = replace_e_Accents.Replace(inputString, "e");
            inputString = replace_i_Accents.Replace(inputString, "i");
            inputString = replace_o_Accents.Replace(inputString, "o");
            inputString = replace_u_Accents.Replace(inputString, "u");
            return inputString;
        }


        public JsonRepuestaDHL EnvioporReferencia(int IdReferencia)
        {
            ApiDHL objApi = new(_configuration);
            JsonRepuestaDHL objrep = new();
            string token = objApi.getToken();


            objrep.TokenDHL = token;
            if (objApi.GeneroToken == true)
            {
                string jsonFac = JsonFac(IdReferencia);
                objrep = objApi.postDHL(jsonFac, token, gtw, "PedimentoFormal/");
                objApi.Bitacora(objrep.respuesta, objrep.status, idReferencia, "");
            }

            return objrep;
        }


        public List<string> EnviarPendientes()
        {
            List<string> Respuestas = new List<string>();
            try
            {
                FacturasReporitory obj = new(_configuration);
                List<int> ids = new List<int>();

                APIGenerarFac objAPIFac = new(_configuration);
                ids = obj.FacsPendientes();
                foreach (int idReferencia in ids)
                {
                    JsonRepuestaDHL objrep = new();
                    objrep = objAPIFac.EnvioporReferencia(idReferencia);
                    Respuestas.Add(objrep.respuesta.Trim());
                }
            }
            catch (Exception)
            {
                Respuestas.Clear();
                throw;
            }


            return Respuestas;
        }


        public List<string> EnviarPorDODA(int idDODA)
        {
            List<string> Respuestas = new List<string>();
            try
            {
                FacturasReporitory obj = new(_configuration);
                List<JsonDetalleporDODA> ids = new List<JsonDetalleporDODA>();

                APIGenerarFac objAPIFac = new(_configuration);
                ApiDHLControl objControl = new(_configuration);
                ids = objControl.DetalleporDODAFacs(idDODA);


                foreach (JsonDetalleporDODA objDetalleFac in ids)
                {
                    try
                    {
                        JsonRepuestaDHL objrep = new();
                        objrep = objAPIFac.EnvioporReferencia(objDetalleFac.IdReferencia);
                        Respuestas.Add(objrep.respuesta.Trim());
                    }
                    catch (Exception ex)
                    { 
                        Respuestas.Add(ex.Message.Trim());
                    }
                  
                }
            }
            catch (Exception)
            {
                Respuestas.Clear();
              
            }


            return Respuestas;
        }
    }
}

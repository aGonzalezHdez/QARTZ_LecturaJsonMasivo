using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Data;
using System.Net;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing;
using System.Drawing.Imaging;
using LibreriaClasesAPIExpertti.Services.S3;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using Chilkat;
using wsCentralizar;
using CatalogoDeOficinas = LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos.CatalogoDeOficinas;

namespace LibreriaClasesAPIExpertti.Negocio
{
    public  class Generacion
    {
        public IConfiguration _configuration;
        public PredodaRepository predodaRepository;
        public PredodaUUIDRepository predodaUUIDRepository;
        public CatalogodeSellosDigitalesRepository catalogodeSellosDigitalesRepository;
        public UbicaciondeArchivosRepository ubicacionDeArchivosRepository;
        public dat_DODARepository dat_DODARepository;
        public SaaioContenRepository saaaioContenRepository;
        public RelacionBitacoraRepository relacionBitacoraRepository;
        public CatalogoDeOficinasRepository catalogoDeOficinasRepository;
        public RelaciondeEnvioRepository relaciondeEnvioRepository;
        public CatalogoDeAgentesAduanalesRepository catalogoDeAgentesAduanalesRepository;
        public ReportePDFHelper helperPdf;
        private ent_DODA entDoda;
        public Generacion(IConfiguration configuration)
        {
            _configuration = configuration;
            predodaUUIDRepository = new PredodaUUIDRepository(_configuration);
            predodaRepository = new PredodaRepository(_configuration);
            ubicacionDeArchivosRepository = new UbicaciondeArchivosRepository(_configuration);
            dat_DODARepository = new dat_DODARepository(_configuration);
            saaaioContenRepository = new SaaioContenRepository(_configuration);
            catalogodeSellosDigitalesRepository = new CatalogodeSellosDigitalesRepository(_configuration);
            relacionBitacoraRepository = new RelacionBitacoraRepository(_configuration);
            catalogoDeOficinasRepository = new CatalogoDeOficinasRepository(_configuration);
            relaciondeEnvioRepository = new RelaciondeEnvioRepository(_configuration);
            catalogoDeAgentesAduanalesRepository = new CatalogoDeAgentesAduanalesRepository(_configuration);
            helperPdf = new ReportePDFHelper();

        }
        public ent_DODA GenerarTicket(int idPredoda,int idRelaciondeEnvio,string rfc,int tipo,string pMisDocumentos)
        {
            int IdDoda = 0;

            CIntegracionDODAPITA objUnificado = new CIntegracionDODAPITA();
            string cadenaOriginal = "";
            entDoda = new ent_DODA();
            Predoda predoda = predodaRepository.Buscar(idPredoda);

            CatalogoDeOficinas oficina = catalogoDeOficinasRepository.Buscar(predoda.IdOficina);

            string MisDocumentos = Path.Combine(pMisDocumentos, "DODA");
            if (!Directory.Exists(MisDocumentos))
                Directory.CreateDirectory(MisDocumentos);

            if (idRelaciondeEnvio!=0)
            {
                RelaciondeEnvio relaciondeEnvio = relaciondeEnvioRepository.Buscar(idRelaciondeEnvio);
                objUnificado._Seccion = relaciondeEnvio.AduanaEntrada;
            }
            else
            {
                objUnificado._Seccion = oficina.AduEntr;
            }
            

            
            objUnificado._Placas = predoda.Placas;
            objUnificado._Patente = predoda.Patente;
            objUnificado._Aduana = Convert.ToInt32(predoda.Aduana);
            objUnificado._CAAT = predoda.CAAT==""?"NULO":predoda.CAAT;
            objUnificado._DespachoAduaneroPITA = tipo;
            objUnificado._NumeroGafeteUnicoPITA = tipo==1?predoda.NumeroGafeteUnico:"";
            objUnificado._TipoOperacion = predoda.Operacion.ToString();

            List<PredodaDetalle> predodaDetalle = predodaRepository.DetallePredoda(idPredoda);

            List<DodaRemesa> listaReferencias = new List<DodaRemesa>();
            foreach (var item in predodaDetalle)
            {
                listaReferencias.Add(new DodaRemesa
                {
                    IdReferencia = item.IdReferencia,
                    Remesa = item.Remesa,
                    Cove = item.NumeroDeCOVE
                });
            }

            List<PredodaUUIDs> uuids = predodaUUIDRepository.Cargar(idPredoda);
            List<string> listadouuids = new List<string>();
            foreach (var item in uuids)
            {
                listadouuids.Add(item.UUID);
            }

            objUnificado._ListaIdReferencia = listaReferencias;
            objUnificado.CFDIsCP = listadouuids;

            CatalogodeSellosDigitales objSello = catalogodeSellosDigitalesRepository.Buscar(rfc);//rfc

            if (objSello.IdSelloDigital==0)
            {
                throw new Exception("No existe sello para el agente aduanal :" + rfc);
            }

            UbicaciondeArchivos objUbicacion = ubicacionDeArchivosRepository.Buscar(74);

            objUnificado._Rfc = objSello.CiecUsuario.Trim();

            cadenaOriginal += "||" + (objUnificado._Seccion == "0" || objUnificado._Seccion == "" ? objUnificado._Aduana : objUnificado._Seccion);
            cadenaOriginal += "|" + objUnificado._Patente;
            cadenaOriginal += "|" + objUnificado._ListaIdReferencia.Count() + "|";

            entDoda._N_Pedimentos = objUnificado._ListaIdReferencia.Count();

            //objUnificado._LPedimento = 

            List<CPedimento> ListaPedimentos = new List<CPedimento>();

            int i = 0;
            foreach (var item in objUnificado._ListaIdReferencia)
            {

                var pedimentosRemesas = dat_DODARepository.ObtenerPedimento(item.IdReferencia, item.Remesa);

                foreach (var ped in pedimentosRemesas)
                {
                    if (i > 0)
                        cadenaOriginal += ",";
                    i++;
                    cadenaOriginal += ped._Documento + (ped._Cove == "" ? "" : "-" + ped._Cove) ;

                    if (ped._NumeroRemesa == "0")
                    {
                        ListaPedimentos.Add(new CPedimento()
                        {
                            _Patente = ped._Patente,
                            _Documento = ped._Documento,
                            _NumeroRemesa = "0",
                            _Campo12Apendice = "0"
                        });
                    }
                    else
                    {
                        ListaPedimentos.Add(ped);
                    }

                    
                }
            }
            objUnificado._LPedimento = ListaPedimentos;
            cadenaOriginal += "||" + objUnificado._Placas + "|";
            /*
             *  CONTENEDORES
             */

            List<CContenedores> dtbContenedores = saaaioContenRepository.CargarporPredoda(idPredoda);
            int j = 0;
            foreach (var item in dtbContenedores)
            {
                j++;
                cadenaOriginal += item._ValorContenedor;
                if (j < dtbContenedores.Count())
                    cadenaOriginal += ",";
            }
            objUnificado._LContenedores = dtbContenedores;

            /*
             * FIN CONTENEDORES 
             */
            cadenaOriginal += "|";
            cadenaOriginal += DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"||";

            objUnificado._CadenaOriginal = cadenaOriginal;
            objUnificado._Sellos = objSello;

            List<string> aduanas = dat_DODARepository.ObtenerAduana(Convert.ToInt32(predoda.Aduana));
            if (aduanas.Count()>0)
            {
                objUnificado._NoAduana = aduanas[0];
            }
            try
            {
                string nombre_doda = $"Doda-pre{idPredoda}.xml";
                GenerarXmlAltaDoda(objUnificado, MisDocumentos,nombre_doda);
                string nombre_alta_firmado = $"AltaFirmado-pre{idPredoda}.xml";
                GenerarFirmaXmlEnvolventeAltaDoda(MisDocumentos,nombre_doda,nombre_alta_firmado);
                DataTable tdMensaje = new DataTable();
                DataSet ds_RespuestaAlta = new DataSet();

                string nombre_resultado_alta_firmado = $"RespuestaAltaFirmado-pre{idPredoda}.xml";
                ds_RespuestaAlta = XmlResponse(nombre_alta_firmado, MisDocumentos, nombre_resultado_alta_firmado);

                if (ds_RespuestaAlta.Tables.Contains("mensajes") == false)
                    throw new ArgumentException("No fue posible establecer conexión con el SAT, favor de intentarlo mas tarde");

                DataTable dtRespuesta = ds_RespuestaAlta.Tables[3];
                if (!dtRespuesta.Rows[0].ItemArray[0].ToString().Contains("Su solicitud ha sido recibida satisfactoriamente"))
                {
                    tdMensaje = ds_RespuestaAlta.Tables[3];
                    string mensajes = "";
                    foreach (DataRow m in tdMensaje.Rows)
                    {
                        mensajes += m[0] +"";
                    }
                    throw new Exception("No se pudo generar un número de ticket" + mensajes);
                }
                dtRespuesta = ds_RespuestaAlta.Tables[2];
                entDoda._N_Ticket = dtRespuesta.Rows[0].ItemArray[2].ToString();
                entDoda._UsuarioCiec = objUnificado._Rfc;
                entDoda._Patente = objUnificado._Patente;
                entDoda._NAduana = objUnificado._Aduana;
                entDoda._Seccion = objUnificado._Seccion;
                entDoda._NoAduana = objUnificado._NoAduana;
                entDoda._CadenaOriginal = objUnificado._CadenaOriginal;
                entDoda._NSerieCertificado = objUnificado._Sellos.NumeroDeSerie;
                entDoda._ent_Lista_DODANew = objUnificado;
                entDoda._DespachoAduanero = objUnificado._DespachoAduaneroPITA;
                entDoda._NumeroGafeteUnico = objUnificado._NumeroGafeteUnicoPITA;
                entDoda._N_Integracion = "";
                entDoda._SelloDigitalSAT = "";
                entDoda._CadenaOriginalSAT = "";
                entDoda._NSerieCertificadoSAT = "";
                entDoda._Link = "";
                entDoda.Operacion = int.Parse(objUnificado._TipoOperacion);
                entDoda.Placas = objUnificado._Placas!= null? objUnificado._Placas:"";
                entDoda.CAAT = objUnificado._CAAT!=null? objUnificado._CAAT:"";
                IdDoda = dat_DODARepository.neg_InsertarDODANew(entDoda, 1, idPredoda);
                entDoda._IdDODA = IdDoda;

            }
            catch (Exception ex)
            {

                throw new Exception("WS Doda"+ex.Message);
            }
            return entDoda;
        }
        public void GenerarXmlAltaDoda(CIntegracionDODAPITA ent_ListaDODA, string RutaLocal,string nombre_doda)
        {
            try
            {
                Helper objHelp = new Helper();
                string Firma;

                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    Encoding = Encoding.UTF8
                };

                using (XmlWriter writer = XmlWriter.Create(Path.Combine(RutaLocal, nombre_doda), settings))
                {
                    // Inicia el documento
                    writer.WriteStartDocument();
                    // Inicia Nodo Doda
                    writer.WriteStartElement("doda");
                    writer.WriteElementString("rfc", ent_ListaDODA._Rfc.Trim());
                    writer.WriteElementString("despachoAduanero", ent_ListaDODA._DespachoAduaneroPITA.ToString());

                    if (ent_ListaDODA._DespachoAduaneroPITA == 1)
                    {
                        writer.WriteElementString("numeroGafeteUnico", ent_ListaDODA._NumeroGafeteUnicoPITA);
                    }

                    writer.WriteElementString("aduana", ent_ListaDODA._Aduana.ToString());
                    if (string.IsNullOrEmpty(ent_ListaDODA._Seccion) || ent_ListaDODA._Seccion == "0")
                    {
                        writer.WriteElementString("seccion", ent_ListaDODA._Aduana.ToString());
                    }
                    else
                    {
                        writer.WriteElementString("seccion", ent_ListaDODA._Seccion);
                    }

                    if (!string.IsNullOrEmpty(ent_ListaDODA._CAAT) && ent_ListaDODA._CAAT != "NULO")
                    {
                        writer.WriteElementString("caat", ent_ListaDODA._CAAT);
                        writer.WriteElementString("idTransporte", ent_ListaDODA._Placas);
                    }
                    else
                    {
                        writer.WriteElementString("caat", "NULO");
                    }

                    writer.WriteElementString("fastId", ent_ListaDODA._FastaId);
                    

                    writer.WriteElementString("tipoOperacion", ent_ListaDODA._TipoOperacion);

                    if (ent_ListaDODA._LContenedores.Count()>0)
                    {
                        writer.WriteStartElement("contenedores");
                        foreach (CContenedores val in ent_ListaDODA._LContenedores)
                        {
                            writer.WriteStartElement("contenedor");
                            writer.WriteElementString("valorContenedor", val._ValorContenedor);

                            if (val._LCandados != null)
                            {
                                writer.WriteStartElement("candados");
                                foreach (Ccandado val2 in val._LCandados)
                                {
                                    writer.WriteElementString("candado", val2._Candado);
                                }
                                writer.WriteEndElement();
                            }

                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }

                    if (ent_ListaDODA._LPedimentoAmericano != null)
                    {
                        writer.WriteStartElement("pedimentosAmericanos");
                        foreach (CPedimentoAmericano val in ent_ListaDODA._LPedimentoAmericano)
                        {
                            writer.WriteStartElement("pedimentoAmericano");
                            writer.WriteElementString("tipoPedimentoAmericano", val._TipoPedimentoAmericano);
                            writer.WriteElementString("valorPedimentoAmericano", val._ValorPedimentoAmericano);
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }

                    if (ent_ListaDODA.CFDIsCP != null)
                    {
                        writer.WriteStartElement("cfdisCartaPorte");
                        foreach (string itemCFDI in ent_ListaDODA.CFDIsCP)
                        {
                            writer.WriteElementString("cfdiCartaPorte", itemCFDI.Trim());
                        }
                        writer.WriteEndElement();
                    }

                    // Inicia Nodo de Pedimentos
                    writer.WriteStartElement("pedimentos");
                    foreach (CPedimento item in ent_ListaDODA._LPedimento)
                    {
                        writer.WriteStartElement("pedimento");
                        writer.WriteElementString("patente", item._Patente);
                        writer.WriteElementString("documento", item._Documento);

                        if (item._NumeroRemesa == "0")
                        {
                            writer.WriteElementString("numeroRemesa", "0");
                            writer.WriteElementString("dtaNiu", "");
                            writer.WriteElementString("importeDifDolares", "");
                            writer.WriteElementString("importeEfectivoDolares", "");
                            writer.WriteElementString("campo12Apendice17", "0");
                            writer.WriteElementString("cove", "");
                            writer.WriteElementString("umc", "");
                        }
                        else
                        {
                            writer.WriteElementString("numeroRemesa", item._NumeroRemesa);
                            writer.WriteElementString("dtaNiu", item._dtaNiu);
                            writer.WriteElementString("importeDifDolares", item._ImporteDifDolares);
                            writer.WriteElementString("importeEfectivoDolares", item._ImporteEfectivoDolares);
                            writer.WriteElementString("campo12Apendice17", item._Campo12Apendice);
                            writer.WriteElementString("cove", item._Cove);
                            writer.WriteElementString("umc", item._umc);
                        }

                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    // Inicia Nodo Sellado
                    writer.WriteStartElement("sellado");
                    writer.WriteElementString("cadenaOriginalAA", ent_ListaDODA._CadenaOriginal);
                    Firma =  Convert.ToBase64String(objHelp.GenerarFirmaCadenaByte(ent_ListaDODA._Sellos, RutaLocal, ent_ListaDODA._CadenaOriginal));
                    entDoda._SelloDigital = Firma;
                    writer.WriteElementString("firmado", Firma);
                    writer.WriteElementString("serie", ent_ListaDODA._Sellos.NumeroDeSerie);
                    writer.WriteEndElement();

                    // Cierra Nodos y libera recursos
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("GenerarXmlAltaDoda: " + ex.Message);
            }
        }
        public void GenerarFirmaXmlEnvolventeAltaDoda(string RutaLocal,string nombre_doda,string nombre_firmado)
        {
            RSACryptoServiceProvider key = null;
            XmlTextReader _ruta = null;

            try
            {
                _ruta = new XmlTextReader(RutaLocal + "\\"+nombre_doda);

                CspParameters cspParams = new CspParameters();
                cspParams.KeyContainerName = "XML_DSIG_RSA_KEY";
                key = new RSACryptoServiceProvider(cspParams);
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = false;
                doc.Load(_ruta);
                SignedXml signedXml = new SignedXml(doc);
                signedXml.SigningKey = key;
                signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
                Reference reference = new Reference();
                reference.Uri = "";
                XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                reference.AddTransform(env);
                reference.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
                signedXml.AddReference(reference);
                KeyInfo KeyInfo = new KeyInfo();
                KeyInfo.AddClause(new RSAKeyValue(key));
                signedXml.KeyInfo = KeyInfo;

                signedXml.ComputeSignature();

                // Aquí se agrega la firma al xml
                XmlElement xmlFirmado = signedXml.GetXml();
                doc.DocumentElement.AppendChild(doc.ImportNode(xmlFirmado, true));

                for (int i = 0; i < doc.ChildNodes.Count; i++)
                {
                    XmlNode node = doc.ChildNodes[i];
                    if (node.NodeType == XmlNodeType.XmlDeclaration)
                    {
                        doc.RemoveChild(node);
                    }
                }

                StringWriter escritor = new StringWriter();
                try
                {
                    escritor.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
                    escritor.WriteLine("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:mat=\"http://impl.service.qrws.ce.siat.sat.gob.mx/siatbus/matce\" xmlns:xd=\"http://www.w3.org/2000/09/xmldsig#\">");
                    escritor.WriteLine("<soapenv:Header/>");
                    escritor.WriteLine("<soapenv:Body>");
                    escritor.WriteLine("<mat:altaDoda>");
                    escritor.WriteLine(doc.OuterXml);
                    escritor.WriteLine("</mat:altaDoda>");
                    escritor.WriteLine("</soapenv:Body>");
                    escritor.WriteLine("</soapenv:Envelope>");
                    string xmlFirmados = escritor.ToString();
                    Thread.Sleep(2000);
                    File.WriteAllText(RutaLocal + "\\"+ nombre_firmado, xmlFirmados);
                    Thread.Sleep(5000);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("GenerarXmlFirmado Nodo Signature: " + ex.Message);
                }
                finally
                {
                    escritor.Close();
                    escritor.Dispose();
                    key.Clear();
                    key.Dispose();
                    _ruta.Close();
                    _ruta.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("GenerarXmlFirmado: " + ex.Message);
            }
        }
        public DataSet XmlResponse(string documento, string RutaLocal, string nombre_resultado)
        {
            DataSet ds = new DataSet();
            HttpWebResponse myResponse = null;
            StringWriter escritor = new StringWriter();
            System.IO.Stream responsedata = null;
            StreamReader responsereader = null;
            string urlconfig;

            try
            {
                string xml = string.Empty;
                string pathAlta = RutaLocal + "\\" + documento;
                xml = File.ReadAllText(pathAlta);

                if (_configuration.GetConnectionString("dbCASAEI").Contains("172.25.32.4"))//Ambiente productivo
                {
                    urlconfig = "https://200.57.3.82:3443/Integracion/WebServiceDodaPita";
                }
                else
                {
                    urlconfig = "http://192.168.1.164:8080";
                    throw new Exception("En ambiente desarrollo se necesita simular la respuesta");//Descomentar urlconfig
                    
                }
                

                string response;
                try
                {
                    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(urlconfig);
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    byte[] buffer = encoding.GetBytes(xml);
                    myReq.AllowWriteStreamBuffering = false;
                    myReq.Method = "POST";
                    myReq.ContentType = "text/xml; charset=UTF-8";
                    myReq.ContentLength = buffer.Length;
                    myReq.Headers.Add("SOAPAction", "");
                    ServicePointManager.ServerCertificateValidationCallback = (sender1, certificate, chain, sslPolicyErrors) => true;
                    using (System.IO.Stream post = myReq.GetRequestStream())
                    {
                        post.Write(buffer, 0, buffer.Length);
                    }
                    myResponse = (HttpWebResponse)myReq.GetResponse();
                    responsedata = myResponse.GetResponseStream();
                    responsereader = new StreamReader(responsedata);
                    response = responsereader.ReadToEnd();
                }
                catch (WebException exs)
                {
                    string errorfault = string.Empty;
                    if (exs.Response != null)
                    {
                        XmlDocument docs = new XmlDocument();
                        docs.Load(exs.Response.GetResponseStream());
                        XmlNodeList elemList = docs.GetElementsByTagName("detail");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            errorfault += " " + elemList[i].InnerXml;
                        }
                    }
                    else
                    {
                        errorfault = exs.Message;
                    }
                    throw new ArgumentException(errorfault);
                }

                ds.ReadXml(new XmlTextReader(new StringReader(response)));
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = false;
                doc.Load(new XmlTextReader(new StringReader(response)));
                escritor.WriteLine(doc.OuterXml);
                File.WriteAllText(RutaLocal + "\\"+ nombre_resultado, escritor.ToString());
            }
            catch (Exception ex)
            {
                throw new ArgumentException("XmlResponse: " + ex.Message);
            }
            finally
            {
                if (myResponse != null)
                {
                    myResponse.Close();
                    responsereader.Close();
                    responsedata.Close();
                    responsedata.Dispose();
                    responsereader.Dispose();
                    myResponse.Dispose();
                }
                escritor.Dispose();
            }
            return ds;
        }

        public async Task<DODAResultado> GenerarIntegracion(
            ent_DODA entDoda,
            string PITA,
            int IDDatosDeEmpresa,
            Entities.EntitiesCatalogos.CatalogoDeUsuarios objUsuario,
            string pMisDocumentos)
        {
            var objDODAResultado = new DODAResultado();
            DataSet ds_RespuestaEstatus;
            string pdf;
            
            string MisDocumentos = string.Empty;
            var cldoda = dat_DODARepository;
            var objAA = new CatalogoDeAgentesAduanales();
            var objAAD = catalogoDeAgentesAduanalesRepository;
            string RutaDestinoDoda;
            bool ErrorTicket = false;
            string Acummensajes = " SAT informa:";

            try
            {

                var ruta = ubicacionDeArchivosRepository.Buscar(147);

                MisDocumentos = Path.Combine(pMisDocumentos, "DODA");

                CatalogodeSellosDigitales objSello = catalogodeSellosDigitalesRepository.Buscar(entDoda._UsuarioCiec);//se envia usuarioCiec de doda
                string contrasenia = objSello.CiecPassword;

                string nombre_estatus = $"Estatus-pre{entDoda.IdPredoda}.xml";
                GenerarXmlEstatus(entDoda, MisDocumentos,nombre_estatus);
                string estatus_firmado = $"EstatusFirmado-pre{entDoda.IdPredoda}.xml";
                GenerarFirmaXMLEnvolventeEstatus(MisDocumentos,nombre_estatus, estatus_firmado);

                string respuesta_estatus = $"RespuestaEstatus-pre{entDoda.IdPredoda}.xml";
                ds_RespuestaEstatus = XmlResponse(estatus_firmado, MisDocumentos, respuesta_estatus);
                var dtRespuestaEstatus = ds_RespuestaEstatus.Tables[2];

                if (dtRespuestaEstatus.Rows[0].ItemArray[4].ToString().Contains("INCORRECTO"))
                {
                    DataTable vMensaje;

                    try
                    {
                        vMensaje = ds_RespuestaEstatus.Tables["mensajes"];

                        foreach (DataRow row in vMensaje.Rows)
                        {
                            Acummensajes += row[0].ToString() + " ";
                        }
                    }
                    catch (Exception ex)
                    {
                        Acummensajes = dtRespuestaEstatus.Rows[0].ItemArray[4].ToString();
                    }

                    ErrorTicket = true;
                }

                if (!ErrorTicket)
                {
                    entDoda._N_Integracion = dtRespuestaEstatus.Rows[0].ItemArray[2].ToString();
                    entDoda._CadenaOriginalSAT = dtRespuestaEstatus.Rows[0].ItemArray[6].ToString();

                    string vNSerieCertificadoSAT = string.Empty;
                    if (!string.IsNullOrWhiteSpace(dtRespuestaEstatus.Rows[0].ItemArray[5].ToString().Trim()))
                    {
                        vNSerieCertificadoSAT = dtRespuestaEstatus.Rows[0].ItemArray[5].ToString();
                    }

                    entDoda._NSerieCertificadoSAT = vNSerieCertificadoSAT;
                    entDoda._SelloDigitalSAT = dtRespuestaEstatus.Rows[0].ItemArray[7].ToString();
                    entDoda._FechaEmision = Convert.ToDateTime(dtRespuestaEstatus.Rows[0].ItemArray[8].ToString());

                    string Qr = "https://siat.sat.gob.mx/app/qr/faces/pages/mobile/validadorqr.jsf?D1=16&D2=1&D3=" + dtRespuestaEstatus.Rows[0].ItemArray[2].ToString() + Environment.NewLine;
                    entDoda._Link = Qr;

                    var qrEncoder = new QrEncoder();
                    var qrCode = new QrCode();
                    qrEncoder.TryEncode(Qr, out qrCode);

                    var renderer = new GraphicsRenderer(new FixedCodeSize(600, QuietZoneModules.Zero),Brushes.Black, Brushes.White);
                    var ms = new MemoryStream();
                    renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
                    var imagetemporal = new Bitmap(ms);
                    var image = new System.Drawing.Bitmap(imagetemporal, new Size(500, 500));
                    var converter = new System.Drawing.ImageConverter();
                    entDoda._Img = (byte[])converter.ConvertTo(image, typeof(byte[]));

                    var objUbicacion = new UbicaciondeArchivos();
                    var objUbicacionD = new UbicaciondeArchivosRepository(_configuration);
                    objUbicacion = objUbicacionD.Buscar(74);

                    //string NuevaRuta = objUbicacion.Ubicacion.Trim(); //Para producción
                    string NuevaRuta = Path.Combine(pMisDocumentos, "S3");
                    string RutaArchivo = Path.Combine(NuevaRuta, entDoda._FechaEmision.ToString("yyyy")+ entDoda._FechaEmision.ToString("MM"), entDoda._FechaEmision.ToString("dd"));
                    entDoda._RutaArchivo = RutaArchivo;

                    cldoda.neg_UpdateIntegracion(entDoda, ErrorTicket, Acummensajes);

                    if (entDoda._N_Integracion == "0" || string.IsNullOrWhiteSpace(entDoda._N_Integracion))
                    {
                        throw new ArgumentException(Acummensajes);
                    }

                    /*RutaDestinoDoda = Path.Combine(entDoda._RutaArchivo, entDoda._N_Integracion.ToString() + ".pdf");
                    string AltaFirmadoOrigen = Path.Combine(MisDocumentos, "AltaFirmado.xml");
                    string AltaFirmadoDestino = Path.Combine(entDoda._RutaArchivo, entDoda._N_Integracion.ToString() + "_AltaFirmado.xml");

                    if (!Directory.Exists(Path.GetDirectoryName(AltaFirmadoDestino.Trim())))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(AltaFirmadoDestino.Trim()));
                    }*/
                    var objDoda = new ent_DODA();

                    pdf = Path.Combine(MisDocumentos, $"{entDoda._N_Integracion.ToString()}.pdf");

                    objDODAResultado.Archivo = pdf;
                    objDODAResultado.NoIntegracion = entDoda._N_Integracion.ToString();
                }
                else
                {
                    objDODAResultado.Archivo = string.Empty;
                    objDODAResultado.NoIntegracion = string.Empty;

                    entDoda._N_Integracion = string.Empty;
                    entDoda._CadenaOriginalSAT = string.Empty;
                    entDoda._SelloDigitalSAT = string.Empty;
                    entDoda._NSerieCertificadoSAT = string.Empty;
                    entDoda._Link = string.Empty;
                    entDoda._FechaEmision = DateTime.Now;

                    var qrEncoder = new QrEncoder();
                    var qrCode = new QrCode();
                    qrEncoder.TryEncode("https://siat.sat.gob.mx/app/qr/faces/pages/mobile/validadorqr.jsf?D1=16&D2=1&D3=", out qrCode);

                    var renderer = new GraphicsRenderer(new FixedCodeSize(600, QuietZoneModules.Zero), Brushes.Black, Brushes.White);
                    var ms = new MemoryStream();
                    renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);

                    var imagetemporal = new Bitmap(ms);
                    var image = new Bitmap(imagetemporal, new Size(500, 500));
                    var converter = new ImageConverter();
                    entDoda._Img = (byte[])converter.ConvertTo(image, typeof(byte[]));

                    cldoda.neg_UpdateIntegracion(entDoda, ErrorTicket, Acummensajes);
                    throw new ArgumentException(Acummensajes.Trim());
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(" Respuestas: " + ex.Message);
            }

            return objDODAResultado;
        }
        private void GenerarXmlEstatus(ent_DODA entDODA, string RutaLocal,string nombre_estatus)
        {
            XmlTextWriter writer = null;

            try
            {
                writer = new XmlTextWriter(Path.Combine(RutaLocal, nombre_estatus), Encoding.UTF8)
                {
                    Formatting = Formatting.Indented
                };
                writer.WriteStartDocument();
                writer.WriteStartElement("doda");
                writer.WriteElementString("rfc", entDODA._UsuarioCiec.Trim());
                writer.WriteElementString("ticket", entDODA._N_Ticket);
                writer.WriteStartElement("sellado");
                writer.WriteElementString("cadenaOriginalAA", entDODA._CadenaOriginal);
                writer.WriteElementString("firmado", entDODA._SelloDigital);
                writer.WriteElementString("serie", entDODA._NSerieCertificado);
                writer.WriteEndElement(); // cierre de sellado
                writer.WriteEndElement(); // cierre de doda
                writer.WriteEndDocument();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("GenerarXmlEstatus: " + ex.Message);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                }
            }
        }
        private void GenerarFirmaXMLEnvolventeEstatus(string RutaLocal,string nombre_estatus,string estatus_firmado)
        {
            StringWriter escritor = new StringWriter();
            RSACryptoServiceProvider key;
            XmlTextReader _ruta = null;

            try
            {
                _ruta = new XmlTextReader(Path.Combine(RutaLocal, nombre_estatus));
                CspParameters cspParams = new CspParameters
                {
                    KeyContainerName = "XML_DSIG_RSA_KEY"
                };
                key = new RSACryptoServiceProvider(cspParams);
                XmlDocument doc = new XmlDocument
                {
                    PreserveWhitespace = false
                };
                doc.Load(_ruta);
                SignedXml signedXml = new SignedXml(doc)
                {
                    SigningKey = key
                };
                Reference reference = new Reference
                {
                    Uri = ""
                };
                XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                reference.AddTransform(env);
                reference.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
                signedXml.AddReference(reference);
                KeyInfo keyInfo = new KeyInfo();
                keyInfo.AddClause(new RSAKeyValue(key));
                signedXml.KeyInfo = keyInfo;
                signedXml.ComputeSignature();
                XmlElement xmlFirmado = signedXml.GetXml();
                doc.DocumentElement.AppendChild(doc.ImportNode(xmlFirmado, true));

                foreach (XmlNode node in doc)
                {
                    if (node.NodeType == XmlNodeType.XmlDeclaration)
                    {
                        doc.RemoveChild(node);
                    }
                }

                escritor.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
                escritor.WriteLine("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:mat=\"http://impl.service.qrws.ce.siat.sat.gob.mx/siatbus/matce\" xmlns:xd=\"http://www.w3.org/2000/09/xmldsig#\">");
                escritor.WriteLine("<soapenv:Header/>");
                escritor.WriteLine("<soapenv:Body>");
                escritor.WriteLine("<mat:consultaEstatus>");
                escritor.WriteLine(doc.OuterXml);
                escritor.WriteLine("</mat:consultaEstatus>");
                escritor.WriteLine("</soapenv:Body>");
                escritor.WriteLine("</soapenv:Envelope>");
                string xmlFirmados = escritor.ToString();
                File.WriteAllText(Path.Combine(RutaLocal, estatus_firmado), xmlFirmados);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("GenerarXmlEstatusEnvolvente: " + ex.Message);
            }
            finally
            {
                escritor.Close();
                escritor.Dispose();
                _ruta.Close();
                _ruta.Dispose();
            }
        }

        private XmlDsigXPathTransform CreateXPathTransform(string xPath)
        {
            XmlDsigXPathTransform transform = new XmlDsigXPathTransform();
            XmlDocument doc = new XmlDocument();
            XmlElement elem = doc.CreateElement("XPath");
            elem.InnerText = xPath;
            transform.LoadInnerXml(elem.SelectNodes("."));
            return transform;
        }
        public async Task<string> SincronizarArchivosS3(string MisDocumentos,ent_DODA entDoda, int IDDatosDeEmpresa, Entities.EntitiesCatalogos.CatalogoDeUsuarios objUsuario)
        {
            var objDodaD = dat_DODARepository;
            DataTable dtbReferencias = objDodaD.Cargar(entDoda._IdDODA);

            try
            {
                string altaFirmado = $"AltaFirmado-pre{entDoda.IdPredoda}.xml";
                string respuestaAlta = $"RespuestaAltaFirmado-pre{entDoda.IdPredoda}.xml";
                string estatusFirmado = $"EstatusFirmado-pre{entDoda.IdPredoda}.xml";
                string respuestaEstatusFirmado = $"RespuestaEstatus-pre{entDoda.IdPredoda}.xml";
                foreach (DataRow item in dtbReferencias.Rows)
                {
                    int idReferencia = Convert.ToInt32(item["IDReferencia"]);
                    int ID = 0;

                    var objS3 = new CentralizarS3sp(_configuration);

                    if (File.Exists(Path.Combine(MisDocumentos, altaFirmado)))
                    {
                        ID = await objS3.SubirDocumentosporGuiaIntegracion(1168, Path.Combine(MisDocumentos, altaFirmado), idReferencia, 0, entDoda._N_Integracion.ToString() + "_AltaFirmado", objUsuario, "grupoei.documentos");
                    }

                    if (File.Exists(Path.Combine(MisDocumentos, estatusFirmado)))
                    {

                        ID = await objS3.SubirDocumentosporGuiaIntegracion(1168, Path.Combine(MisDocumentos, estatusFirmado), idReferencia, 0, entDoda._N_Integracion.ToString() + "_EstatusFirmado", objUsuario, "grupoei.documentos");
                    }

                    if (File.Exists(Path.Combine(MisDocumentos, respuestaEstatusFirmado)))
                    {

                        ID = await objS3.SubirDocumentosporGuiaIntegracion(1168, Path.Combine(MisDocumentos, respuestaEstatusFirmado), idReferencia, 0, entDoda._N_Integracion.ToString() + "_RespuestaEstatus", objUsuario, "grupoei.documentos");
                    }

                    if (File.Exists(Path.Combine(MisDocumentos, respuestaAlta)))
                    {

                        ID = await objS3.SubirDocumentosporGuiaIntegracion(1168, Path.Combine(MisDocumentos, respuestaAlta), idReferencia, 0, entDoda._N_Integracion.ToString() + "_RespuestaAlta", objUsuario, "grupoei.documentos");
                    }
                }

                try
                {
                    File.Delete(Path.Combine(MisDocumentos, altaFirmado));
                    File.Delete(Path.Combine(MisDocumentos, estatusFirmado));
                    File.Delete(Path.Combine(MisDocumentos, respuestaEstatusFirmado));
                    File.Delete(Path.Combine(MisDocumentos, respuestaAlta));
                }
                catch (Exception ex) { }
            }
            catch (Exception ex) { }

            var paramList = new List<string> {
                        "N_Integracion="+ entDoda._N_Integracion.ToString()
                    };
            var pdfBytes = await helperPdf.GenerarReportePdf(40, paramList, IDDatosDeEmpresa, _configuration);
            string pdf = Path.Combine(MisDocumentos, $"{entDoda._N_Integracion.ToString()}.pdf");

            await System.IO.File.WriteAllBytesAsync(pdf, pdfBytes);

            //subiendo pdf
            foreach (DataRow item in dtbReferencias.Rows)
            {
                int idReferencia = Convert.ToInt32(item["IDReferencia"]);
                int ID = 0;
                var objS3 = new CentralizarS3sp(_configuration);
                if (File.Exists(Path.Combine(MisDocumentos, $"{entDoda._N_Integracion.ToString()}.pdf")))
                {

                    ID = await objS3.SubirDocumentosporGuiaIntegracion(1163, Path.Combine(MisDocumentos, $"{entDoda._N_Integracion.ToString()}.pdf"), idReferencia, 0, entDoda._N_Integracion.ToString() + entDoda._N_Integracion.ToString(), objUsuario, "grupoei.documentos");
                }
            }
            return "ok";
        }

    }
}

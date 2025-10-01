using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Entities.EntitiesVucem;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesVUCEM;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RespositoriesVentanillaUnica;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesNube;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using wsCentralizar;
using CatalogoDeUsuarios = LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos.CatalogoDeUsuarios;

namespace LibreriaClasesAPIExpertti.Services.VentanillaUnica
{
    public class ComprobanteService
    {

        public string SConexion { get; set; }
        public string SConexionGP { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public ComprobanteService(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
            SConexionGP = _configuration.GetConnectionString("dbCASAEIGP")!;
        }

        public async Task<wsRespuestaCove.RespuestaPeticion> RecuperarCOVENOIA(
    CatalogodeSellosDigitales lSelloDigital,
    string NumeroDeOperacion,
    Referencias Referencias,
    string vArchivo,
    CatalogoDeUsuarios GObjUsuario,
    int IdUsuarioAutoriza,
    string pMisDocumentos)
        {
            var objHelp = new Helper();

            string CadenaOriginal = "|" + NumeroDeOperacion + "|" + lSelloDigital.UsuarioWebService.Trim() + "|";

            byte[] vFirma = objHelp.GenerarFirmaCadenaByte(lSelloDigital, pMisDocumentos, CadenaOriginal);

            var ClienteRC = new wsRespuestaCove.ReceptorClient();
            ClienteRC.ClientCredentials.UserName.UserName = lSelloDigital.UsuarioWebService;
            ClienteRC.ClientCredentials.UserName.Password = lSelloDigital.PasswordWebService;

            var objFirmaElectronicaRC = new wsRespuestaCove.FirmaElectronica
            {
                cadenaOriginal = CadenaOriginal,
                certificado = lSelloDigital.Certificado,
                firma = vFirma
            };

            var SolicitarRespuestaCove = new wsRespuestaCove.SolicitarConsultarRespuestaCoveServicio
            {
                numeroOperacion = NumeroDeOperacion,
                firmaElectronica = objFirmaElectronicaRC
            };

            var RespuestaPeticion = new wsRespuestaCove.RespuestaPeticion();

            try
            {
                RespuestaPeticion = await ClienteRC.ConsultarRespuestaCove(SolicitarRespuestaCove);

                if (RespuestaPeticion.respuestasOperaciones != null)
                {
                    var lstRespuestas = RespuestaPeticion.respuestasOperaciones;

                    int vNumerodeOperacion = Convert.ToInt32(RespuestaPeticion.numeroOperacion);

                    var objUbicacionD = new UbicaciondeArchivosRepository(_configuration);
                    var objUbicacion = objUbicacionD.Buscar(22);

                    if (objUbicacion == null)
                        throw new ArgumentException("No existe ruta para generar XML");

                    string ArchivoAcuse = Path.Combine(objUbicacion.Ubicacion, $"Acuse_{NumeroDeOperacion}.xml");
                    GenerarXmlAcuse(ArchivoAcuse, RespuestaPeticion);

                    foreach (var item in lstRespuestas)
                    {
                        var objFactD = new SaaioFacturRepository(_configuration);
                        var lstFact = objFactD.CargarRemesa(
                            Referencias.NumeroDeReferencia.Trim(),
                            Convert.ToInt32(item.numeroFacturaORelacionFacturas.Trim()));

                        var objFactCoveD = new FacturasCoveRepository(_configuration);
                        FacturasCove objFactCove = null;

                        foreach (var iFactura in lstFact)
                        {
                            objFactCove = objFactCoveD.Buscar(
                                Referencias.IDReferencia,
                                iFactura.CONS_FACT);

                            if (!item.contieneError)
                            {
                                objFactCove.NumeroDeCOVE = item.eDocument;
                                objFactCove.COVE = true;
                                objFactCove.FirmaDigitalOperacion = Convert.ToBase64String(vFirma);
                                objFactCove.CadenaOriginalOperacion = CadenaOriginal;
                                objFactCove.EnviadoSAT = true;
                                objFactCove.numeroAdenda = "";
                                objFactCove.NumeroDeOperacion = Convert.ToInt32(NumeroDeOperacion);

                                objFactCoveD.Modificar(
                                    objFactCove,
                                    GObjUsuario.IdUsuario,
                                    IdUsuarioAutoriza
                                    );
                            }
                        }

                        try
                        {
                            await SubirXMlaS3NOIA(
                                Referencias.IDReferencia,
                                vNumerodeOperacion.ToString(),
                                Convert.ToInt32(item.numeroFacturaORelacionFacturas),
                                objFactCove.IDFacturaCOVE,
                                GObjUsuario);

                            await SubirAcuseXMlaS3NOIA(
                                ArchivoAcuse,
                                Referencias.IDReferencia,
                                Convert.ToInt32(item.numeroFacturaORelacionFacturas),
                                objFactCove.IDFacturaCOVE,
                                GObjUsuario);
                        }
                        catch
                        {
                            // Ignorar errores aquí según el código original
                        }
                    }
                }
                else
                {
                    throw new Exception("Favor de intentar la recepción más tarde");
                }
            }
            catch (Exception ex)
            {
                if (RespuestaPeticion.leyenda == null)
                    throw new Exception(ex.Message);
                else
                    throw new Exception(RespuestaPeticion.leyenda);
            }

            return RespuestaPeticion;
        }
        public async Task SubirXMlaS3NOIA(int IDReferencia, string Numerodeoperacion, int Num_Rem, int Id, CatalogoDeUsuarios GObjUsuario)
        {
            int Result = 0;

            var objUbicacionD = new UbicaciondeArchivosRepository(_configuration);
            var objUbicacion = objUbicacionD.Buscar(22);
            if (objUbicacion == null)
                throw new ArgumentException("No existe ruta para generar XML");

            string vRuta = objUbicacion.Ubicacion.Trim() + "ExperttiTmp\\";
            string ArchivoAcuse = $"{vRuta}{Numerodeoperacion}_{Num_Rem}.xml";

            var objDocumentosporguia = new DocumentosPorGuia();
            var objCentralizar = new CentralizarDocsS3(_configuration);

            Result = await objCentralizar.AgregarDocumentos(
                ArchivoAcuse,
                IDReferencia,
                1148,
                "",
                Id,
                GObjUsuario,
                false,
                ""                
            );

            var objFactCove = new FacturasCoveRepository(_configuration);
            objFactCove.ModificarDocNOIA(IDReferencia, Num_Rem, Result, 2);

            File.Delete(ArchivoAcuse);
        }


        public async Task<wsRespuestaCove.RespuestaPeticion> RecuperarCOVE(
    CatalogodeSellosDigitales lSelloDigital,
    string NumeroDeOperacion,
    Referencias Referencias,
    string vArchivo,
    string pMisDocumentos,
    CatalogoDeUsuarios GObjUsuario,
    int IdUsuarioAutoriza)
        {
            var objHelp = new Helper();

            string CadenaOriginal = $"|{NumeroDeOperacion}|{lSelloDigital.UsuarioWebService.Trim()}|";

            byte[] vFirma = objHelp.GenerarFirmaCadenaByte(lSelloDigital, pMisDocumentos, CadenaOriginal);

            var ClienteRC = new wsRespuestaCove.ReceptorClient();
            ClienteRC.ClientCredentials.UserName.UserName = lSelloDigital.UsuarioWebService;
            ClienteRC.ClientCredentials.UserName.Password = lSelloDigital.PasswordWebService;

            var objFirmaElectronicaRC = new wsRespuestaCove.FirmaElectronica
            {
                cadenaOriginal = CadenaOriginal,
                certificado = lSelloDigital.Certificado,
                firma = vFirma
            };

            var SolicitarRespuestaCove = new wsRespuestaCove.SolicitarConsultarRespuestaCoveServicio
            {
                numeroOperacion = NumeroDeOperacion,
                firmaElectronica = objFirmaElectronicaRC
            };

            var RespuestaPeticion = new wsRespuestaCove.RespuestaPeticion();

            try
            {
                RespuestaPeticion =  ClienteRC.ConsultarRespuestaCoveAsync(SolicitarRespuestaCove).Result.solicitarConsultarRespuestaCoveServicioResponse;

                var objUbicacionD = new UbicaciondeArchivosRepository(_configuration);
                var objUbicacion = objUbicacionD.Buscar(22);

                if (objUbicacion == null)
                    throw new ArgumentException("No existe ruta para generar XML");

                string ArchivoAcuse = Path.Combine(objUbicacion.Ubicacion, $"Acuse_{NumeroDeOperacion}.xml");
                GenerarXmlAcuse(ArchivoAcuse, RespuestaPeticion);

                if (RespuestaPeticion.respuestasOperaciones != null)
                {
                    var lstRespuestas = RespuestaPeticion.respuestasOperaciones;
                    int vNumerodeOperacion = Convert.ToInt32(RespuestaPeticion.numeroOperacion);

                    foreach (var item in lstRespuestas)
                    {
                        var objFactD = new SaaioFacturRepository(_configuration);
                        var objFact = objFactD.Buscar(
                            Referencias.NumeroDeReferencia.Trim(),
                            item.numeroFacturaORelacionFacturas.Trim());

                        var objFactCoveD = new FacturasCoveRepository(_configuration);
                        var objFactCove = objFactCoveD.Buscar(
                            Referencias.IDReferencia,
                            objFact.CONS_FACT);

                        if (objFactCove == null)
                            continue;

                        if (!item.contieneError)
                        {
                            objFactCove.NumeroDeCOVE = item.eDocument;
                            objFactCove.COVE = true;
                            objFactCove.FirmaDigitalOperacion = Convert.ToBase64String(vFirma);
                            objFactCove.CadenaOriginalOperacion = CadenaOriginal;
                            objFactCove.EnviadoSAT = true;
                            objFactCove.numeroAdenda = item.numeroAdenda;
                        }

                        objFactCove.NumeroDeOperacion = Convert.ToInt32(NumeroDeOperacion);
                        objFactCoveD.Modificar(objFactCove, GObjUsuario.IdUsuario, IdUsuarioAutoriza);

                        try
                        {
                            await SubirXMlaS3(
                                Referencias.IDReferencia,
                                vNumerodeOperacion.ToString(),
                                objFact.CONS_FACT,
                                objFactCove.IDFacturaCOVE,
                                GObjUsuario);

                            await SubirAcuseXMlaS3(
                                ArchivoAcuse,
                                Referencias.IDReferencia,
                                objFact.CONS_FACT,
                                objFactCove.IDFacturaCOVE,
                                GObjUsuario);
                        }
                        catch
                        {
                            // Ignorado como en VB
                        }
                    }

                    File.Delete(ArchivoAcuse);
                }
                else
                {
                    throw new Exception("Favor de intentar la recepción mas tarde");
                }
            }
            catch (Exception ex)
            {
                if (RespuestaPeticion.leyenda == null)
                {
                    throw new Exception(ex.Message);
                }
                else
                {
                    throw new Exception("Respuesta de Ventilla Unica: " + RespuestaPeticion.leyenda);
                }
            }

            return RespuestaPeticion;
        }

        private async Task SubirXMlaS3(int IDReferencia, string Numerodeoperacion, int CONS_FACT, int Id, CatalogoDeUsuarios GObjUsuario)
        {
            int Result = 0;

            var objUbicacionD = new UbicaciondeArchivosRepository(_configuration);
            var objUbicacion = objUbicacionD.Buscar(22);

            if (objUbicacion == null)
            {
                throw new ArgumentException("No existe ruta para generar XML");
            }

            string vRuta = objUbicacion.Ubicacion.Trim() + "ExperttiTmp\\";
            string ArchivoAcuse = vRuta + Numerodeoperacion + "_" + CONS_FACT + ".xml";

            var objDocumentosporguia = new DocumentosPorGuia();
            var objCentralizar = new CentralizarDocsS3(_configuration);

            Result = await objCentralizar.AgregarDocumentos(
                ArchivoAcuse,
                IDReferencia,
                1148,
                "",
                Id,
                GObjUsuario,
                false,
                ""
            );

            var objFactCove = new FacturasCoveRepository(_configuration);
            objFactCove.ModificarDocXML(IDReferencia, CONS_FACT, Result);

            File.Delete(ArchivoAcuse);
        }
        private async Task SubirAcuseXMlaS3(string ArchivoAcuse, int IDReferencia, int CONS_FACT, int Id, CatalogoDeUsuarios GObjUsuario)
        {
            int Result = 0;

            var objDocumentosporguia = new DocumentosPorGuia();
            var objCentralizar = new CentralizarDocsS3(_configuration);

            Result = await objCentralizar.AgregarDocumentos(
                ArchivoAcuse,
                IDReferencia,
                1159,
                "",
                Id,
                GObjUsuario,
                false,
                ""
            );

            var objFactCove = new FacturasCoveRepository(_configuration);
            objFactCove.ModificarDocAcuseXML(IDReferencia, CONS_FACT, Result);
        }


        public async Task<wsRespuestaCove.RespuestaPeticion> EnviarCoveNOIA(
    string NumeroDeReferencia,
    int NumRem,
    string edocument,
    bool xml,
    string Ruta,
    int IDDatosDeEmpresa,
    string pMisDocumentos,
    CatalogoDeUsuarios GObjUsuario,
    int IdUsuarioAutoriza)
        {
            var objRespuesta = new wsRespuestaCove.RespuestaPeticion();

            try
            {
                var objRefeD = new ReferenciasRepository(_configuration);
                var objRefe = objRefeD.Buscar(NumeroDeReferencia, IDDatosDeEmpresa);

                if (objRefe == null)
                    throw new Exception("No existe la referencia");

                var objPedimeD = new SaaioPedimeRepository(_configuration);
                var objPedime = objPedimeD.Buscar(objRefe.NumeroDeReferencia);

                if (objPedime.CVE_IMPO == "EI123")
                    throw new Exception("No puede enviar cove con el cliente de prueba");

                var objClienteD = new ClientesRepository(_configuration);
                var objCliente = objClienteD.Buscar(objRefe.IDCliente);

                if (objCliente != null && objCliente.Prospecto)
                    throw new Exception("No puede enviar a cove un cliente prospecto");

                var objComprobanteD = new ComprobanteService(_configuration);
                var Comprobante = objComprobanteD.GenerarComprobanteNoia(
                    NumRem, NumeroDeReferencia, edocument, IDDatosDeEmpresa, pMisDocumentos);

                var Comprobantes = new wsVentanillaUnica.ComprobanteValorElectronicoNoIA[1];
                Comprobantes[0] = Comprobante;

                objRespuesta = await objComprobanteD.GenerarCoveNOIA(
                    Comprobantes,
                    objRefe,
                    false,
                    Ruta,
                    IDDatosDeEmpresa,
                    GObjUsuario,
                    IdUsuarioAutoriza,
                    pMisDocumentos
                );
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return objRespuesta;
        }
        public wsVentanillaUnica.ComprobanteValorElectronicoNoIA GenerarComprobanteNoia(
    int NumRem,
    string NumerodeReferencia,
    string edocument,
    int IDDatosDeEmpresa,
    string pMisDocumentos)
        {
            var objGenerales = new ComponentesGenerales();
            var objDatos = new Datos(_configuration);

            objGenerales = objDatos.DatosGenerales(NumerodeReferencia, IDDatosDeEmpresa);

            var objComprobanteSAT = new wsVentanillaUnica.ComprobanteValorElectronicoNoIA();

            try
            {
                var lstFacturas = new List<SaaioFactur>();
                var objFacturD = new SaaioFacturRepository(_configuration);
                lstFacturas = objFacturD.CargarRemesa(NumerodeReferencia.Trim(), NumRem);

                objComprobanteSAT.numeroRelacionFacturas = NumRem.ToString();

                var Facturas = new wsVentanillaUnica.FacturaNoIA[lstFacturas.Count];
                var lstFacNOIA = new List<wsVentanillaUnica.FacturaNoIA>();

                int i = 0;
                string Observaciones = string.Empty;

                foreach (var itemFactura in lstFacturas)
                {
                    var objDatosComp = new ComponentesdelComprobante();
                    objDatosComp = objDatos.DatosParaELComprobante(objGenerales, itemFactura.CONS_FACT);

                    var objFacturaCove = new wsVentanillaUnica.FacturaNoIA
                    {
                        certificadoOrigen = objDatosComp.CertificadoOrigen,
                        numeroExportadorAutorizado = string.IsNullOrWhiteSpace(objDatosComp.Proveedor.EXP_CONF) ? null : objDatosComp.Proveedor.EXP_CONF.Trim(),
                        subdivision = objDatosComp.Subdivision,
                        emisor = objDatosComp.Emisor,
                        numeroFactura = objDatosComp.Factura.NUM_FACT2,
                        destinatario = objDatosComp.Destiantario,
                        mercancias = objDatosComp.Mercancias
                    };

                    Observaciones += " " + itemFactura.OBS_COVE.Trim();
                    lstFacNOIA.Add(objFacturaCove);
                    Facturas[i] = objFacturaCove;
                    i++;
                }

                Observaciones = Observaciones.Trim();
                objComprobanteSAT.facturas = Facturas;
                objComprobanteSAT.edocument = string.IsNullOrWhiteSpace(edocument) ? null : edocument;
                objComprobanteSAT.tipoFigura = objGenerales.TipodeFigura;
                var tipoOperacion = ((objGenerales.Pedime.IMP_EXPO) == "1") ? wsVentanillaUnica.TipoOperacion.TOCEIMP : wsVentanillaUnica.TipoOperacion.TOCEEXP;
                objComprobanteSAT.tipoOperacion = tipoOperacion;

                var Patentes = new string[1];
                Patentes[0] = objGenerales.Pedime.PAT_AGEN;
                objComprobanteSAT.patenteAduanal = Patentes;

                var nubePrepagoRepository = new NubePrepagoRepository(_configuration);
                var objFecha = nubePrepagoRepository.FechaDelServidor();
                DateTime fechaServidor = new DateTime(Convert.ToInt32(objFecha.anio), Convert.ToInt32(objFecha.mes), Convert.ToInt32(objFecha.dia));
                objComprobanteSAT.fechaExpedicion = fechaServidor;

                objComprobanteSAT.rfcConsulta = objGenerales.RfcConsulta;
                objComprobanteSAT.correoElectronico = objGenerales.Sello.Email;
                objComprobanteSAT.observaciones = Observaciones;

                string vCadenaOriginal;
                var objCadenaOriginal = new CadenaOriginal(_configuration);
                vCadenaOriginal = objCadenaOriginal.GenerarComprobanteNOIA(NumRem, lstFacNOIA, objGenerales, Observaciones);

                objComprobanteSAT.firmaElectronica = objDatos.getFirmaElectronica(objGenerales.Sello, vCadenaOriginal, pMisDocumentos);
                objComprobanteSAT.edocument = string.IsNullOrWhiteSpace(edocument) ? null : edocument;

                // Si no hay edocument, registrar en tabla
                if (string.IsNullOrWhiteSpace(edocument))
                {
                    foreach (var iFact in lstFacturas)
                    {
                        var objFactCove = new FacturasCove
                        {
                            IDReferencia = objGenerales.Referencia.IDReferencia,
                            CONS_FACT = iFact.CONS_FACT,
                            CadenaOriginal = vCadenaOriginal,
                            FirmaDigital = Convert.ToBase64String(objComprobanteSAT.firmaElectronica.firma),
                            NumeroDeCOVE = "",
                            COVE = false,
                            NumeroDeOperacion = 0,
                            FirmaDigitalOperacion = "",
                            CadenaOriginalOperacion = "",
                            EnviadoSAT = false,
                            numeroAdenda = ""
                        };

                        var objFactCoveD = new FacturasCoveRepository(_configuration);
                        objFactCoveD.Insertar(objFactCove);
                    }
                }
            }
            catch (Exception ex)
            {
                objComprobanteSAT = null;
                throw new Exception(ex.Message);
            }

            return objComprobanteSAT;
        }

        private async Task<wsRespuestaCove.RespuestaPeticion> GenerarCoveNOIA(
    wsVentanillaUnica.ComprobanteValorElectronicoNoIA[] Comprobantes,
    Referencias Referencia,
    bool GeneraXml,
    string PathXml,
    int IDDatosDeEmpresa,
    CatalogoDeUsuarios GObjUsuario,
    int IdUsuarioAutoriza,
    string pMisDocumentos)
        {
            string Archivo = PathXml + Referencia.NumeroDeReferencia.Trim() + ".xml";

            var objGenerales = new ComponentesGenerales();
            var objDatos = new Datos(_configuration);

            objGenerales = objDatos.DatosGenerales(Referencia.NumeroDeReferencia, IDDatosDeEmpresa);

            var objPeticion = new wsRespuestaCove.RespuestaPeticion();

            string Mensaje = "imposible enviar comprobante favor de intentar más tarde";

            try
            {                
                var Cliente = new wsVentanillaUnica.ReceptorClient();
                Cliente.ClientCredentials.UserName.UserName = objGenerales.Sello.UsuarioWebService;
                Cliente.ClientCredentials.UserName.Password = objGenerales.Sello.PasswordWebService;
                var Acuse = new wsVentanillaUnica.Acuse();

                try
                {
                    Acuse = Cliente.RecibirRelacionFacturasNoIAAsync(Comprobantes).Result.solicitarRecibirRelacionFacturasNoIAServicioResponse;
                    Mensaje = Acuse.mensajeInformativo.ToString();
                }
                catch (Exception ex)
                {
                    Mensaje = ex.Message;
                }

                foreach (var itemComprobante in Comprobantes)
                {
                    var objFactD = new SaaioFacturRepository(_configuration);
                    List<SaaioFactur> lstFact = objFactD.CargarRemesa(
                        Referencia.NumeroDeReferencia.Trim(),
                        Convert.ToInt32(itemComprobante.numeroRelacionFacturas.ToString())
                    );

                    foreach (var iFact in lstFact)
                    {
                        var objFactCoveD = new FacturasCoveRepository(_configuration);
                        var objFactCove = objFactCoveD.Buscar(
                            Referencia.IDReferencia,
                            iFact.CONS_FACT
                        );

                        objFactCove.NumeroDeOperacion = Convert.ToInt32(Acuse.numeroDeOperacion.ToString());
                        objFactCoveD.Modificar(objFactCove, GObjUsuario.IdUsuario, IdUsuarioAutoriza);
                    }

                    GenerarXmlNOIANuevo(PathXml, itemComprobante, Convert.ToInt32(itemComprobante.numeroRelacionFacturas.ToString()), Acuse.numeroDeOperacion.ToString());
                }

                if (!string.IsNullOrWhiteSpace(Acuse.numeroDeOperacion))
                {
                    Mensaje = "El comprobante fue enviado con éxito y la respuesta está en proceso";
                }

                objPeticion = await RecuperarCOVENOIA(
                    objGenerales.Sello,
                    Acuse.numeroDeOperacion.Trim(),
                    objGenerales.Referencia,
                    Archivo,
                    GObjUsuario,
                    IdUsuarioAutoriza,
                    pMisDocumentos
                );
            }
            catch (Exception)
            {
                throw new Exception(Mensaje);
            }

            return objPeticion;
        }

        private void GenerarXmlNOIANuevo(string Ruta,
                                 wsVentanillaUnica.ComprobanteValorElectronicoNoIA Comprobante,
                                 int RelaciondeFacturas,
                                 string NumerodeOperacion)
        {
            bool GeneraXml = true;
            string Archivo = Ruta + NumerodeOperacion + "_" + RelaciondeFacturas + ".xml";

            TextWriter txtWriter = new StreamWriter(Archivo);
            try
            {
                var FileXml = new System.Xml.Serialization.XmlSerializer(Comprobante.GetType());
                FileXml.Serialize(txtWriter, Comprobante);
            }
            catch
            {
                // Se ignoran excepciones como en el código original
            }

            txtWriter.Flush();
            txtWriter.Close();
            txtWriter.Dispose();
        }


        private void GenerarXmlAcuse(string archivo, wsRespuestaCove.RespuestaPeticion acuse)
        {
            bool generaXml = true;
            
            TextWriter txtWriter = new StreamWriter(archivo);
            try
            {
                var fileXml = new XmlSerializer(acuse.GetType());
                fileXml.Serialize(txtWriter, acuse);
            }
            catch
            {
                // Silenciado como en el original
            }
            finally
            {
                txtWriter.Flush();
                txtWriter.Close();
                txtWriter.Dispose();
            }
        }

        public async Task<wsRespuestaCove.RespuestaPeticion> EnviarCove(
    string numeroDeReferencia,
    int consFact,
    string cove,
    bool xml,
    string ruta,
    int idDatosDeEmpresa,
    bool pGlobal,
    string pMisDocumentos,
    CatalogoDeUsuarios gObjUsuario,
    int idUsuarioAutoriza)
        {
            var objRespuesta = new wsRespuestaCove.RespuestaPeticion();

            var objRefeD = new ReferenciasRepository(_configuration);
            var objRefe = objRefeD.Buscar(numeroDeReferencia, idDatosDeEmpresa);

            if (objRefe == null)
                throw new Exception("No existe la referencia");

            var objClienteD = new ClientesRepository(_configuration);
            var objCliente = objClienteD.Buscar(objRefe.IDCliente);

            if (objCliente != null && objCliente.Prospecto)
                throw new Exception("No puede enviar a cove un clientes prospecto");

            if (string.IsNullOrWhiteSpace(cove))
            {
                var objFactCoveD = new FacturasCoveRepository(_configuration);
                var objFactCove = objFactCoveD.Buscar(objRefe.IDReferencia, consFact);

                if (objFactCove != null)
                    throw new Exception("Ya existe una peticion de Cove");
            }

            var objPedimeD = new SaaioPedimeRepository(_configuration);
            var objPedime = objPedimeD.Buscar(objRefe.NumeroDeReferencia);

            if (objPedime.CVE_IMPO == "EI123")
                throw new Exception("No puede enviar cove con el cliente de prueba");

            var objSaiioFactD = new SaaioFacturRepository(_configuration);

            objSaiioFactD.ModificarVinculacion(objRefe.IDReferencia, consFact);

            var objSaiioFact = objSaiioFactD.Buscar(objRefe.NumeroDeReferencia, consFact);

            if (objSaiioFact == null)
                throw new ArgumentException("Existe un problema en la captura de su factura");

            if (objSaiioFact.CVE_PROV == null)
                throw new ArgumentException("no se guardó adecuadamente el proveedor, favor de modificar la factura");

            if (objSaiioFact.CVE_PROV == "EI123")
                throw new Exception("No puede enviar cove con el proveedor de prueba");

            var objComprobanteD = new ComprobanteService(_configuration);
            var comprobante = objComprobanteD.GenerarComprobante(
                consFact, numeroDeReferencia, cove, idDatosDeEmpresa, pMisDocumentos);

            var comprobantes = new wsVentanillaUnica.ComprobanteValorElectronico[1];
            comprobantes[0] = comprobante;

            objRespuesta = await objComprobanteD.GenerarCove(
                comprobantes,
                objRefe,
                false,
                ruta,
                idDatosDeEmpresa,
                cove.Trim(),
                pMisDocumentos,
                gObjUsuario,
                idUsuarioAutoriza);

            return objRespuesta;
        }

        public async Task<wsRespuestaCove.RespuestaPeticion> GenerarCove(
    wsVentanillaUnica.ComprobanteValorElectronico[] comprobantes,
    Referencias referencia,
    bool generaXml,
    string pathXml,
    int idDatosDeEmpresa,
    string pMisDocumentos,
    string cove,
    CatalogoDeUsuarios gObjUsuario,
    int idUsuarioAutoriza)
        {
            string archivo = pathXml + referencia.NumeroDeReferencia.Trim() + ".xml";
            var objDatos = new Datos(_configuration);
            var objGenerales = objDatos.DatosGenerales(referencia.NumeroDeReferencia, idDatosDeEmpresa);
            var objPeticion = new wsRespuestaCove.RespuestaPeticion();
            string mensaje = "imposible enviar comprobante favor de intentar más tarde";

            try
            {
                var cliente = new wsVentanillaUnica.ReceptorClient();
                cliente.ClientCredentials.UserName.UserName = objGenerales.Sello.UsuarioWebService;
                cliente.ClientCredentials.UserName.Password = objGenerales.Sello.PasswordWebService;
                wsVentanillaUnica.Acuse acuse = new wsVentanillaUnica.Acuse();

                try
                {
                    acuse = cliente.RecibirCoveAsync(comprobantes).Result.solicitarRecibirCoveServicioResponse;
                    mensaje = acuse.mensajeInformativo == null
                        ? "No se recibió una respuesta exitosa de VUCEM"
                        : acuse.mensajeInformativo.Trim();
                }

                catch (Exception ex)
                {
                    mensaje = "No se recibió una respuesta exitosa de VUCEM: " + ex.Message;
                }

                if (acuse == null || comprobantes == null || acuse.numeroDeOperacion == null)
                {
                    throw new ArgumentException("No se recibió una respuesta exitosa de VUCEM");
                }

                foreach (var itemComprobante in comprobantes)
                {
                    var objFactD = new SaaioFacturRepository(_configuration);
                    var objFact = objFactD.Buscar(referencia.NumeroDeReferencia.Trim(), itemComprobante.numeroFacturaOriginal.Trim());
                    var objFactCoveD = new FacturasCoveRepository(_configuration);
                    var objFactCove = objFactCoveD.Buscar(referencia.IDReferencia, objFact.CONS_FACT);
                    objFactCove.NumeroDeOperacion =Convert.ToInt32(acuse.numeroDeOperacion);
                    objFactCoveD.Modificar(objFactCove, gObjUsuario.IdUsuario, idUsuarioAutoriza);
                    GenerarXmlNuevo(pathXml, itemComprobante, objFact.CONS_FACT, acuse.numeroDeOperacion);
                }

                if (!string.IsNullOrWhiteSpace(acuse.numeroDeOperacion))
                {
                    mensaje = "El comprobante fue enviado con éxito y la respuesta está en proceso";
                }

                objPeticion = await RecuperarCOVE(
                    objGenerales.Sello,
                    acuse.numeroDeOperacion.Trim(),
                    objGenerales.Referencia,
                    archivo,
                    pMisDocumentos,
                    gObjUsuario,
                    idUsuarioAutoriza);

            }
            catch (Exception ex)
            {
                throw new Exception(mensaje);
            }

            return objPeticion;
    }

    private void GenerarXmlNuevo(
    string Ruta,wsVentanillaUnica.ComprobanteValorElectronico Comprobante,int ConsFact,string NumerodeOperacion)
        {
            bool GeneraXml = true;
            string Archivo = Ruta + NumerodeOperacion + "_" + ConsFact + ".xml";

            if (GeneraXml)
            {
                TextWriter txtWriter = new StreamWriter(Archivo);
                try
                {
                    var FileXml = new System.Xml.Serialization.XmlSerializer(Comprobante.GetType());
                    FileXml.Serialize(txtWriter, Comprobante);
                }
                catch
                {
                    // Silencia la excepción como en el código original (aunque no es recomendable)
                }

                txtWriter.Flush();
                txtWriter.Close();
                txtWriter.Dispose();
            }
        }


        private async Task SubirAcuseXMlaS3NOIA(string ArchivoAcuse, int IDReferencia, int Num_Rem, int Id, Entities.EntitiesCatalogos.CatalogoDeUsuarios GObjUsuario)
        {
            int Result = 0;

            var objDocumentosporguia = new DocumentosPorGuia();
            var objCentralizar = new CentralizarDocsS3(_configuration);

            Result = await objCentralizar.AgregarDocumentos(
                ArchivoAcuse,
                IDReferencia,
                1159,
                "",
                Id,
                GObjUsuario,
                false,
                ""
            );

            var objFactCove = new FacturasCoveRepository(_configuration);
            objFactCove.ModificarDocNOIA(IDReferencia, Num_Rem, Result, 1);
        }

        private wsVentanillaUnica.ComprobanteValorElectronico GenerarComprobante(
    int ConsFact,
    string NumerodeReferencia,
    string edocument,
    int IDDatosDeEmpresa,
    string pMisDocumentos)
        {
            var objDatos = new Datos(_configuration);
            var objGenerales = objDatos.DatosGenerales(NumerodeReferencia, IDDatosDeEmpresa);
            var objComprobanteSAT = new wsVentanillaUnica.ComprobanteValorElectronico();

            try
            {
                var objDatosComp = objDatos.DatosParaELComprobante(objGenerales, ConsFact);
                var objFacturaCove = new wsVentanillaUnica.FacturaCove();

                objFacturaCove.certificadoOrigen = objDatosComp.CertificadoOrigen;

                if (objDatosComp.Proveedor.EXP_CONF != null)
                {
                    objFacturaCove.numeroExportadorAutorizado =
                        string.IsNullOrWhiteSpace(objDatosComp.Proveedor.EXP_CONF) ? null : objDatosComp.Proveedor.EXP_CONF.Trim();
                }
                else
                {
                    objFacturaCove.numeroExportadorAutorizado = null;
                }

                objFacturaCove.subdivision = objDatosComp.Subdivision;

                objComprobanteSAT.numeroFacturaOriginal = objDatosComp.Factura.NUM_FACT2;
                objComprobanteSAT.emisor = objDatosComp.Emisor;
                objComprobanteSAT.destinatario = objDatosComp.Destiantario;
                objComprobanteSAT.mercancias = objDatosComp.Mercancias;
                objComprobanteSAT.edocument = string.IsNullOrEmpty(edocument) ? null : edocument;
                objComprobanteSAT.factura = objFacturaCove;
                objComprobanteSAT.tipoFigura = objGenerales.TipodeFigura;                
                var tipoOperacion = ((objGenerales.Pedime.IMP_EXPO) == "1") ? wsVentanillaUnica.TipoOperacion.TOCEIMP : wsVentanillaUnica.TipoOperacion.TOCEEXP;
                objComprobanteSAT.tipoOperacion = tipoOperacion;
                string[] Patentes = new string[1];
                Patentes[0] = objGenerales.Pedime.PAT_AGEN;
                objComprobanteSAT.patenteAduanal = Patentes;

                objComprobanteSAT.fechaExpedicion = Convert.ToDateTime(objDatosComp.Factura.FEC_FACT);

                objComprobanteSAT.observaciones = string.IsNullOrWhiteSpace(objDatosComp.Factura.OBS_COVE)
                    ? null
                    : objDatosComp.Factura.OBS_COVE.Trim() + objDatosComp.Factura.OBS_COVE2.Trim();

                objComprobanteSAT.rfcConsulta = objGenerales.RfcConsulta;
                objComprobanteSAT.correoElectronico = objGenerales.Sello.Email;

                var objCadenaOriginal = new CadenaOriginal(_configuration);
                string vCadenaOriginal = objCadenaOriginal.GenerarComprobante(objDatosComp, objGenerales);

                objComprobanteSAT.firmaElectronica = objDatos.getFirmaElectronica(objGenerales.Sello, vCadenaOriginal, pMisDocumentos);
                objComprobanteSAT.edocument = string.IsNullOrEmpty(edocument) ? null : edocument;

                var objFactCove = new FacturasCove
                {
                    IDReferencia = objGenerales.Referencia.IDReferencia,
                    CONS_FACT = ConsFact,
                    CadenaOriginal = vCadenaOriginal,
                    FirmaDigital = Convert.ToBase64String(objComprobanteSAT.firmaElectronica.firma),
                    NumeroDeCOVE = edocument,
                    COVE = false,
                    NumeroDeOperacion = 0,
                    FirmaDigitalOperacion = "",
                    CadenaOriginalOperacion = "",
                    EnviadoSAT = false,
                    numeroAdenda = ""
                };

                var objFactCoveD = new FacturasCoveRepository(_configuration);
                objFactCoveD.Insertar(objFactCove);
            }
            catch (Exception ex)
            {
                objComprobanteSAT = null;
                throw new Exception(ex.Message);
            }

            return objComprobanteSAT;
        }



    }
}

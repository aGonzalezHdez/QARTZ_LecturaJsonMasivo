using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Entities.EntitiesVucem;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesVUCEM.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesVUCEM
{
    public class COVENOIARepository : ICOVENOIARepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public COVENOIARepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public async Task<CoveResponse> GenerarCOVEAsync(CoveRequest coveRequest)
        {
            CoveResponse objCoveResponse = new CoveResponse();
            try
            {

                    Referencias objReferencias = new Referencias();
                    ReferenciasRepository objReferenciasD = new(_configuration);
                    objReferencias = objReferenciasD.Buscar(coveRequest.idReferencia);

                    if (objReferencias == null)
                    {
                        throw new Exception("No existe el idReferencia que se solicito");
                    }

                    SaaioPedime objPedi = new SaaioPedime();
                    SaaioPedimeRepository objPediD = new SaaioPedimeRepository(_configuration);
                    objPedi = objPediD.Buscar(objReferencias.NumeroDeReferencia);

                    if (objPedi == null)
                    {
                        throw new Exception("No existe el número de referencia que se solicito:" + objReferencias.NumeroDeReferencia);
                    }

                    if (objPedi.CVE_IMPO == "EI123")
                    {
                        throw new Exception("Error; No puede enviar cove con el cliente de prueba");
                    }

                    if (objPedi.CVE_REPR == null)
                    {
                        throw new Exception("Error; Es necesario, seleccionar el representante legal (pedimento)");
                    }
                    string eDocument = string.Empty;

                    if (coveRequest.Adenda == true)
                    {
                        SaaioCove objSaaioCOVE = new SaaioCove();
                        SaaioCoveRepository objSaaioCOVED = new SaaioCoveRepository(_configuration);
                        //objSaaioCOVE = objSaaioCOVE.Buscar(objPedi.NUM_REFE, coveRequest.consFact);

                        if (objSaaioCOVE == null)
                        {
                            throw new Exception("Para realizar una adenda, es necesario tener un número de COVE");
                        }

                        if (objSaaioCOVE.E_DOCUMENT == null)
                        {
                            throw new Exception("Para realizar una adenda, es necesario tener un número de COVE");
                        }

                        if (objSaaioCOVE.E_DOCUMENT.Trim() == "")
                        {
                            throw new Exception("Para realizar una adenda, es necesario tener un número de COVE");

                        }

                        eDocument = objSaaioCOVE.E_DOCUMENT;
                    }
                    SaaioFacturRepository objFactD = new(_configuration);
                    SaaioFactur objFact = new();
                    objFact = objFactD.Buscar(objPedi.NUM_REFE, coveRequest.consFact);

                    bool Relacion = objPediD.esRelacion(objPedi, objFact.NUM_REM);


                    objFactD.ModificarRelacion(objPedi.NUM_REFE, coveRequest.consFact, Relacion);



                    if (objFact.MON_FACT == "MXP")
                    {
                        if (objFact.EQU_DLLS == 1.00000000)
                        {
                            throw new Exception("Error; Cuando la moneda es MXP, la equivalencia no puede ser 1.00000000");
                        }
                    }

                    wsRespuestaCove.RespuestaPeticion respuestaPeticion = new();

                    respuestaPeticion = await EnviarCOVENOIA(objReferencias, objPedi, objFact, eDocument, coveRequest.idUsuario, coveRequest.idUsuarioAutorizaAdenda);

            }
            catch (Exception ex)
            {

                throw new Exception (ex.Message.Trim());
            }
    
            return objCoveResponse;
        }

        /// <summary>
        ///  Envía COVE de remesas.
        /// </summary>
        /// <param name="objRefe">objeto referencia</param>
        /// <param name="objPedi">objeto pedimento</param>
        /// <param name="objFact">objeto factura de pedimento</param>
        /// <param name="eDocument">edoucment</param>
        /// <param name="idUsuario">id usuario</param>
        /// <param name="idUsuarioAutorizaAdenda">usuario que autoriza adenda</param>
        /// <returns> Respuesta del cove</returns>
        /// <exception cref="Exception">Manejo de excepciones</exception>
        /// <remarks>18/08/2025 Vanessa Báez:  </remarks>
        private async Task<wsRespuestaCove.RespuestaPeticion> EnviarCOVENOIA(Referencias objRefe, SaaioPedime objPedi, SaaioFactur objFact, string eDocument, int idUsuario, int idUsuarioAutorizaAdenda)
        {
            wsRespuestaCove.RespuestaPeticion objRespuesta = new();
            try
            {
                UbicaciondeArchivosRepository objUbicaciondeArchivos = new(_configuration);
                string MisDocumentos = objUbicaciondeArchivos.fMisDocumentos(idUsuario);



                ComponentesGenerales objGenerales = new ComponentesGenerales();
                Datos objDatos = new(_configuration);
                objGenerales = objDatos.DatosGenerales(objRefe.NumeroDeReferencia, objRefe.IDDatosDeEmpresa);



                wsVentanillaUnica.ComprobanteValorElectronicoNoIA[] Comprobantes = new wsVentanillaUnica.ComprobanteValorElectronicoNoIA[1];

                wsVentanillaUnica.ComprobanteValorElectronicoNoIA Comprobante = new wsVentanillaUnica.ComprobanteValorElectronicoNoIA();

                Comprobante = GenerarComprobanteNoia(objRefe, objPedi, objFact, objGenerales, eDocument, idUsuario);
                Comprobantes[0] = Comprobante;


                wsRespuestaCove.RespuestaPeticion objPeticion = new();

                string Mensaje = "imposible enviar comprobante favor de intentar mas tarde";

                wsVentanillaUnica.ReceptorClient cliente = new();
                cliente.ClientCredentials.UserName.UserName = objGenerales.Sello.UsuarioWebService;
                cliente.ClientCredentials.UserName.Password = objGenerales.Sello.PasswordWebService;


                wsVentanillaUnica.Acuse Acuse = new wsVentanillaUnica.Acuse();
                wsVentanillaUnica.RecibirCoveRequest x = new wsVentanillaUnica.RecibirCoveRequest();
               // x.solicitarRecibirCoveServicio =    ;

                //var y = cliente.RecibirRelacionFacturasNoIAAsync(x.solicitarRecibirCoveServicio);

                //acuse = await cliente.RecibirRelacionFacturasNoIAAsync(Comprobantes);

                foreach (wsVentanillaUnica.ComprobanteValorElectronicoNoIA itemComprobante in Comprobantes)
                {
                    List<SaaioFactur> lstFact = new List<SaaioFactur>();
                    SaaioFacturRepository saaioFacturRepository = new(_configuration);
                    lstFact = saaioFacturRepository.CargarRemesa(objRefe.NumeroDeReferencia, objFact.NUM_REM);

                    //foreach (SaaioFactur iFact in lstFact)
                    //{
                    //    FacturasCove facturasCove = new FacturasCove();
                    //    FacturasCoveRepository facturasCoveRepository = new(_configuration);
                    //    facturasCove = facturasCoveRepository.Buscar(objRefe.IDReferencia, iFact.CONS_FACT);

                    //    facturasCove.NumeroDeOperacion = Convert.ToInt32(acuse.solicitarRecibirRelacionFacturasNoIAServicioResponse.numeroDeOperacion);
                    //    facturasCoveRepository.Modificar(facturasCove, idUsuario, idUsuarioAutorizaAdenda);
                    //}



                }



            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.Trim());
            }

            return objRespuesta;
        }

        private wsVentanillaUnica.ComprobanteValorElectronicoNoIA GenerarComprobanteNoia(Referencias objRefe, SaaioPedime objPedi, SaaioFactur objFact, ComponentesGenerales objGenerales, string eDocument, int idUsuario)
        {

            wsVentanillaUnica.ComprobanteValorElectronicoNoIA objComprobanteSAT = new();
            try
            {
                UbicaciondeArchivosRepository objUbicaciondeArchivos = new(_configuration);
                string MisDocumentos = objUbicaciondeArchivos.fMisDocumentos(idUsuario);


                List<SaaioFactur> lstFacturas = new List<SaaioFactur>();
                SaaioFacturRepository objFacturD = new SaaioFacturRepository(_configuration);
                lstFacturas = objFacturD.CargarRemesa(objRefe.NumeroDeReferencia, objFact.NUM_REM);

                wsVentanillaUnica.FacturaNoIA[] facturaNoIAs = new wsVentanillaUnica.FacturaNoIA[lstFacturas.Count];

                List<wsVentanillaUnica.FacturaNoIA> lstFacNOIA = new List<wsVentanillaUnica.FacturaNoIA>();

                int i = 0;
                string Observaciones = string.Empty;

                Datos objDatos = new(_configuration);

                foreach (SaaioFactur itemFactura in lstFacturas)
                {
                    ComponentesdelComprobante objDatosComp = new();
                    objDatosComp = objDatos.DatosParaELComprobante(objGenerales, itemFactura.CONS_FACT);

                    wsVentanillaUnica.FacturaNoIA facturaNoIA = new();
                    facturaNoIA.certificadoOrigen = objDatosComp.CertificadoOrigen;
                    facturaNoIA.numeroExportadorAutorizado = string.IsNullOrEmpty(objDatosComp.Proveedor.EXP_CONF) ? "" : objDatosComp.Proveedor.EXP_CONF;
                    facturaNoIA.subdivision = objDatosComp.Subdivision;
                    facturaNoIA.emisor = objDatosComp.Emisor;
                    facturaNoIA.numeroFactura = objDatosComp.Factura.NUM_FACT2;
                    facturaNoIA.destinatario = objDatosComp.Destiantario;
                    facturaNoIA.mercancias = objDatosComp.Mercancias;
                    Observaciones = Observaciones + " " + itemFactura.OBS_COVE.Trim();

                    lstFacNOIA.Add(facturaNoIA);

                    facturaNoIAs[i] = facturaNoIA;

                    i += 1;

                }

                Observaciones = Observaciones.Trim();

                objComprobanteSAT.facturas = facturaNoIAs;
                objComprobanteSAT.edocument = string.IsNullOrEmpty(eDocument) ? null : eDocument;
                objComprobanteSAT.tipoFigura = objGenerales.TipodeFigura;
                objComprobanteSAT.tipoOperacion = (wsVentanillaUnica.TipoOperacion)(objGenerales.Pedime.IMP_EXPO == "1" ? 0 : 1);

                string[] patentes = new string[1];
                patentes[0] = objGenerales.Pedime.PAT_AGEN;

                objComprobanteSAT.patenteAduanal = patentes;

                FechadelServidor fechadelServidor = new FechadelServidor();
                FechadelServidorRepository fechadelServidorRepository = new(_configuration);
                fechadelServidor = fechadelServidorRepository.Buscar();

                objComprobanteSAT.fechaExpedicion = Convert.ToDateTime(fechadelServidor.Fechayhora);
                objComprobanteSAT.rfcConsulta = objGenerales.RfcConsulta;
                objComprobanteSAT.correoElectronico = objGenerales.Sello.Email;
                objComprobanteSAT.observaciones = Observaciones;

                string vCadenaOriginal = string.Empty;
                CadenaOriginal objCadenaOriginal = new CadenaOriginal(_configuration);

                vCadenaOriginal = objCadenaOriginal.GenerarComprobanteNOIA(objFact.NUM_REM, lstFacNOIA, objGenerales, Observaciones);

                objComprobanteSAT.firmaElectronica = objDatos.getFirmaElectronica(objGenerales.Sello, vCadenaOriginal, MisDocumentos);

                if (eDocument.IsNullOrEmpty())
                {
                    FacturasCove facturasCove = new FacturasCove();
                    FacturasCoveRepository facturasCoveRepository = new(_configuration);

                    facturasCove.IDReferencia = objRefe.IDReferencia;
                    facturasCove.CONS_FACT = objFact.CONS_FACT;
                    facturasCove.CadenaOriginal = vCadenaOriginal;
                    facturasCove.FirmaDigital = Convert.ToBase64String(objComprobanteSAT.firmaElectronica.firma);
                    facturasCove.NumeroDeCOVE = string.Empty;
                    facturasCove.COVE = false;
                    facturasCove.NumeroDeOperacion = 0;
                    facturasCove.FirmaDigitalOperacion = string.Empty;
                    facturasCove.CadenaOriginalOperacion = string.Empty;
                    facturasCove.EnviadoSAT = false;
                    facturasCove.numeroAdenda = string.Empty;

                    facturasCoveRepository.Insertar(facturasCove);
                }


            }
            catch (Exception ex)
            {
                objComprobanteSAT = null;
                throw new Exception("Error al generar comprobante NOIA;" + ex.Message.Trim());

            }

            return objComprobanteSAT;

        }




    }
}

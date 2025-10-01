using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesVUCEM;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesNube;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesVucem
{
    public class CadenaOriginal
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn = null!;

        public CadenaOriginal(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public string GenerarComprobante(ComponentesdelComprobante objDatosComp, ComponentesGenerales objGenerales)
        {
            string vCadena = string.Empty;
            var objDatos = new Datos(_configuration);

            try
            {
                string CadenaComprobante = string.Empty;

                CadenaComprobante = objGenerales.tipoOperacion + "|";
                CadenaComprobante += objDatosComp.Factura.NUM_FACT2.Trim() + "|";
                CadenaComprobante += "0|";
                CadenaComprobante += objDatosComp.Factura.FEC_FACT.ToString("yyyy-MM-dd") + "|";
                CadenaComprobante += objGenerales.TipodeFigura + "|";
                CadenaComprobante += (!string.IsNullOrEmpty(objDatosComp.Factura.OBS_COVE.Trim()) ?
                    objDatosComp.Factura.OBS_COVE.Trim() + objDatosComp.Factura.OBS_COVE2.Trim() + "|" : "");

                if (objGenerales.RfcConsulta != null)
                {
                    foreach (string itemRFc in objGenerales.RfcConsulta)
                    {
                        CadenaComprobante += itemRFc.Trim() + "|";
                    }
                }

                CadenaComprobante += objGenerales.Pedime.PAT_AGEN.Trim() + "|";

                string CadenaFacturaCove = objDatosComp.Subdivision + "|";
                CadenaFacturaCove += objDatosComp.CertificadoOrigen + "|";
                CadenaFacturaCove += objDatosComp.Proveedor.EXP_CONF != null && objDatosComp.Proveedor.EXP_CONF.Trim() != ""
                    ? objDatosComp.Proveedor.EXP_CONF + "|"
                    : "";

                // Emisor
                string CadenaEmisor = string.Empty;
                if (objDatosComp.Emisor != null)
                {
                    var em = objDatosComp.Emisor;
                    CadenaEmisor += em.tipoIdentificador + "|";
                    CadenaEmisor += em.identificacion + "|";
                    CadenaEmisor += (em.apellidoPaterno ?? "") + "|";
                    CadenaEmisor += (em.apellidoMaterno ?? "") + "|";
                    CadenaEmisor += (em.nombre ?? "") + "|";

                    if (em.domicilio != null)
                    {
                        var dom = em.domicilio;
                        CadenaEmisor += (dom.calle ?? "") + "|";
                        CadenaEmisor += (dom.numeroExterior ?? "") + "|";
                        CadenaEmisor += (dom.numeroInterior ?? "") + "|";
                        CadenaEmisor += (dom.colonia ?? "") + "|";
                        CadenaEmisor += (dom.localidad ?? "") + "|";
                        CadenaEmisor += (dom.municipio ?? "") + "|";
                        CadenaEmisor += (dom.entidadFederativa ?? "") + "|";
                        CadenaEmisor += (dom.pais ?? "") + "|";
                        CadenaEmisor += (dom.codigoPostal ?? "") + "|";
                    }
                }

                // Destinatario
                string CadenaDestinatario = string.Empty;
                if (objDatosComp.Destiantario != null)
                {
                    var dest = objDatosComp.Destiantario;
                    CadenaDestinatario += dest.tipoIdentificador + "|";
                    CadenaDestinatario += dest.identificacion + "|";
                    CadenaDestinatario += (dest.apellidoPaterno ?? "") + "|";
                    CadenaDestinatario += (dest.apellidoMaterno ?? "") + "|";
                    CadenaDestinatario += (dest.nombre ?? "") + "|";

                    if (dest.domicilio != null)
                    {
                        var dom = dest.domicilio;
                        CadenaDestinatario += (dom.calle ?? "") + "|";
                        CadenaDestinatario += (dom.numeroExterior ?? "") + "|";
                        CadenaDestinatario += (dom.numeroInterior ?? "") + "|";
                        CadenaDestinatario += (dom.colonia ?? "") + "|";
                        CadenaDestinatario += (dom.localidad ?? "") + "|";
                        CadenaDestinatario += (dom.municipio ?? "") + "|";
                        CadenaDestinatario += (dom.entidadFederativa ?? "") + "|";
                        CadenaDestinatario += (dom.pais ?? "") + "|";
                        CadenaDestinatario += (dom.codigoPostal ?? "") + "|";
                    }
                }

                string vMercancias = string.Empty;
                if (objDatosComp.Mercancias != null)
                {
                    foreach (var item in objDatosComp.Mercancias)
                    {
                        string vMercancia = item.descripcionGenerica + "|";
                        vMercancia += item.claveUnidadMedida + "|";
                        vMercancia += item.cantidad.ToString("0.000") + "|";
                        vMercancia += item.tipoMoneda + "|";
                        vMercancia += item.valorUnitario.ToString("0.000000") + "|";
                        vMercancia += item.valorTotal.ToString("0.0000") + "|";
                        vMercancia += item.valorDolares.ToString("0.0000") + "|";

                        if (item.descripcionesEspecificas != null)
                        {
                            foreach (var desc in item.descripcionesEspecificas)
                            {
                                vMercancia += (desc.marca ?? "") + "|";
                                vMercancia += (desc.modelo ?? "") + "|";
                                vMercancia += (desc.subModelo ?? "") + "|";
                                vMercancia += (desc.numeroSerie ?? "") + "|";
                            }
                        }

                        vMercancias += vMercancia;
                    }
                }

                vCadena = "|" + CadenaComprobante + CadenaFacturaCove + CadenaEmisor + CadenaDestinatario + vMercancias;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return vCadena;
        }

        public string GenerarComprobanteNOIA(int NumReme, List<wsVentanillaUnica.FacturaNoIA> lstFacNOIA, ComponentesGenerales objGenerales, string Observaciones)
        {
            // Datos del comprobante|[Factura NO IA]*
            string vCadena = string.Empty;
            var nubePrepagoRepository = new NubePrepagoRepository(_configuration);

            Fecha objFecha = nubePrepagoRepository.FechaDelServidor();
            DateTime fechaServidor = Convert.ToDateTime($"{objFecha.dia}/{objFecha.mes}/{objFecha.anio}");
            Datos objDatos = new Datos(_configuration);

            try
            {
                // Datos del comprobante
                string CadenaComprobante = string.Empty;
                CadenaComprobante = objGenerales.tipoOperacion + "|";
                CadenaComprobante += NumReme.ToString() + "|";
                CadenaComprobante += "1|";
                CadenaComprobante += fechaServidor.ToString("yyyy-MM-dd") + "|";
                CadenaComprobante += objGenerales.TipodeFigura + "|";
                CadenaComprobante += Observaciones + "|";

                if (objGenerales.RfcConsulta != null)
                {
                    foreach (string itemRFc in objGenerales.RfcConsulta)
                    {
                        CadenaComprobante += itemRFc.Trim() + "|";
                    }
                }

                CadenaComprobante += objGenerales.Pedime.PAT_AGEN.Trim() + "|";

                string CadenaFacturas = string.Empty;

                foreach (var iFactura in lstFacNOIA)
                {
                    string CadenaEmisor = string.Empty;

                    if (iFactura.emisor != null)
                    {
                        CadenaEmisor = iFactura.emisor.tipoIdentificador + "|";
                        CadenaEmisor += iFactura.emisor.identificacion + "|";
                        CadenaEmisor += (iFactura.emisor.apellidoPaterno ?? "") + "|";
                        CadenaEmisor += (iFactura.emisor.apellidoMaterno ?? "") + "|";
                        CadenaEmisor += (iFactura.emisor.nombre ?? "") + "|";

                        if (iFactura.emisor.domicilio != null)
                        {
                            CadenaEmisor += (iFactura.emisor.domicilio.calle ?? "") + "|";
                            CadenaEmisor += (iFactura.emisor.domicilio.numeroExterior ?? "") + "|";
                            CadenaEmisor += (iFactura.emisor.domicilio.numeroInterior ?? "") + "|";
                            CadenaEmisor += (iFactura.emisor.domicilio.colonia ?? "") + "|";
                            CadenaEmisor += (iFactura.emisor.domicilio.localidad ?? "") + "|";
                            CadenaEmisor += (iFactura.emisor.domicilio.municipio ?? "") + "|";
                            CadenaEmisor += (iFactura.emisor.domicilio.entidadFederativa ?? "") + "|";
                            CadenaEmisor += (iFactura.emisor.domicilio.pais ?? "") + "|";
                            CadenaEmisor += (iFactura.emisor.domicilio.codigoPostal ?? "") + "|";
                        }
                    }

                    string CadenaDestinatario = string.Empty;

                    if (iFactura.destinatario != null)
                    {
                        CadenaDestinatario = iFactura.destinatario.tipoIdentificador + "|";
                        CadenaDestinatario += iFactura.destinatario.identificacion + "|";
                        CadenaDestinatario += (iFactura.destinatario.apellidoPaterno ?? "") + "|";
                        CadenaDestinatario += (iFactura.destinatario.apellidoMaterno ?? "") + "|";
                        CadenaDestinatario += (iFactura.destinatario.nombre ?? "") + "|";

                        if (iFactura.destinatario.domicilio != null)
                        {
                            CadenaDestinatario += (iFactura.destinatario.domicilio.calle ?? "") + "|";
                            CadenaDestinatario += (iFactura.destinatario.domicilio.numeroExterior ?? "") + "|";
                            CadenaDestinatario += (iFactura.destinatario.domicilio.numeroInterior ?? "") + "|";
                            CadenaDestinatario += (iFactura.destinatario.domicilio.colonia ?? "") + "|";
                            CadenaDestinatario += (iFactura.destinatario.domicilio.localidad ?? "") + "|";
                            CadenaDestinatario += (iFactura.destinatario.domicilio.municipio ?? "") + "|";
                            CadenaDestinatario += (iFactura.destinatario.domicilio.entidadFederativa ?? "") + "|";
                            CadenaDestinatario += (iFactura.destinatario.domicilio.pais ?? "") + "|";
                            CadenaDestinatario += (iFactura.destinatario.domicilio.codigoPostal ?? "") + "|";
                        }
                    }

                    string vMercancias = string.Empty;

                    if (iFactura.mercancias != null)
                    {
                        foreach (var itemMercancia in iFactura.mercancias)
                        {
                            string vMercancia = string.Empty;
                            vMercancia = itemMercancia.descripcionGenerica + "|";
                            vMercancia += itemMercancia.claveUnidadMedida + "|";
                            vMercancia += itemMercancia.cantidad.ToString("0.000") + "|";
                            vMercancia += itemMercancia.tipoMoneda + "|";
                            vMercancia += itemMercancia.valorUnitario.ToString("0.000000") + "|";
                            vMercancia += itemMercancia.valorTotal.ToString("0.0000") + "|";
                            vMercancia += itemMercancia.valorDolares.ToString("0.0000") + "|";

                            if (itemMercancia.descripcionesEspecificas != null)
                            {
                                foreach (var itemDescripcion in itemMercancia.descripcionesEspecificas)
                                {
                                    vMercancia += (itemDescripcion.marca ?? "") + "|";
                                    vMercancia += (itemDescripcion.modelo ?? "") + "|";
                                    vMercancia += (itemDescripcion.subModelo ?? "") + "|";
                                    vMercancia += (itemDescripcion.numeroSerie ?? "") + "|";
                                }
                            }

                            vMercancias += vMercancia;
                        }
                    }

                    string CadenaFac = string.Empty;
                    CadenaFac += iFactura.numeroFactura.Trim() + "|";
                    CadenaFac += iFactura.subdivision.ToString() + "|";
                    CadenaFac += iFactura.certificadoOrigen.ToString() + "|";

                    string expAutorizado = iFactura.numeroExportadorAutorizado != null ? iFactura.numeroExportadorAutorizado + "|" : "";

                    CadenaFacturas += CadenaFac + CadenaEmisor + CadenaDestinatario + vMercancias;
                }

                vCadena = "|" + CadenaComprobante + CadenaFacturas;
            }
            catch (Exception ex)
            {
                vCadena = string.Empty;
                throw new Exception(ex.Message);
            }

            return vCadena;
        }

    }
}

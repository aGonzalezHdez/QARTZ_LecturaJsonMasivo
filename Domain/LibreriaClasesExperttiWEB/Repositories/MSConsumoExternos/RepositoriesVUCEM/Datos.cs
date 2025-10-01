namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesVUCEM
{
    using System;
    using System.Collections.Generic;
    using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
    using LibreriaClasesAPIExpertti.Entities.EntitiesControlSalidas;
    using LibreriaClasesAPIExpertti.Utilities.Helper;
    using System.Text.RegularExpressions;
    using Microsoft.VisualBasic; // Install-Package Microsoft.VisualBasic
    using System.Data.SqlClient;
    using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
    using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes;
    using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
    using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
    using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
    using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
    using Microsoft.Extensions.Configuration;
    using LibreriaClasesAPIExpertti.Entities.EntitiesVucem;
    using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesCasa;
    using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPedimentos;

    public partial class Datos
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public Datos(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public wsVentanillaUnica.Destinatario getDestinatario(int operacion, CtracClient cliente, CtracProved proveedor, string rfcMensajeria)
        {
            var destinatario = new wsVentanillaUnica.Destinatario();
            var domicilio = new wsVentanillaUnica.Domicilio();

            var genericosData = new CatalogoDeRFCsGenericosRepository(_configuration);
            var generico = genericosData.Buscar(cliente.RFC_IMP);
            bool genericoMensajeria = generico != null;

            if (operacion == 1)
            {
                domicilio.calle = string.IsNullOrEmpty(cliente.DIR_IMP) ? null : cliente.DIR_IMP.Trim();
                domicilio.codigoPostal = string.IsNullOrEmpty(cliente.CP_IMP) ? null : cliente.CP_IMP.Trim();
                domicilio.colonia = string.IsNullOrEmpty(cliente.COL_IMP) ? null : cliente.COL_IMP.Trim();
                domicilio.entidadFederativa = string.IsNullOrEmpty(cliente.EFE_IMP) ? null : cliente.EFE_IMP.Trim();
                domicilio.localidad = string.IsNullOrEmpty(cliente.LOC_IMP) ? null : cliente.LOC_IMP.Trim();
                domicilio.municipio = string.IsNullOrEmpty(cliente.MUN_COVE) ? null : cliente.MUN_COVE.Trim();
                domicilio.numeroExterior = string.IsNullOrEmpty(cliente.NOE_IMP) ? null : cliente.NOE_IMP.Trim();
                domicilio.numeroInterior = string.IsNullOrEmpty(cliente.NOI_IMP) ? null : cliente.NOI_IMP.Trim();
                domicilio.pais = string.IsNullOrEmpty(cliente.PAI_IMP) ? null : cliente.PAI_IMP.Trim();

                destinatario.nombre = string.IsNullOrEmpty(cliente.NOM_IMP) ? null : cliente.NOM_IMP.Trim();
                destinatario.apellidoPaterno = string.IsNullOrEmpty(cliente.APE_PATE) ? null : cliente.APE_PATE.Trim();
                destinatario.apellidoMaterno = string.IsNullOrEmpty(cliente.APE_MATE) ? null : cliente.APE_MATE.Trim();

                if (!string.IsNullOrWhiteSpace(cliente.RFC_IMP))
                {
                    if (genericoMensajeria)
                    {
                        destinatario.tipoIdentificador = 3;
                        destinatario.identificacion = cliente.RFC_IMP.Trim();
                        destinatario.nombre = cliente.NOM_IMP.Trim();
                    }
                    else
                    {
                        destinatario.tipoIdentificador = 1;
                        destinatario.identificacion = cliente.RFC_IMP.Trim();
                    }
                }
                else
                {
                    destinatario.tipoIdentificador = 2;
                    destinatario.identificacion = cliente.CUR_IMP.Trim();
                    destinatario.nombre = cliente.NOM_IMP.Trim();
                }

                destinatario.domicilio = domicilio;
            }
            else
            {
                destinatario.tipoIdentificador = proveedor.TAX_PRO == "SIN TAX ID" ? 3 : 0;
                
                var entidadData = new CtarcEntFedRepository(_configuration);
                var entidad = entidadData.Buscar(proveedor.PAI_PRO, proveedor.EFE_PRO.Trim());
                var entidadNombre = entidad != null ? entidad.NOM_EFED.Trim() : null;

                domicilio.calle = string.IsNullOrEmpty(proveedor.DIR_PRO) ? null : proveedor.DIR_PRO.Trim();
                domicilio.codigoPostal = string.IsNullOrEmpty(proveedor.ZIP_PRO) ? null : proveedor.ZIP_PRO.Trim();
                domicilio.colonia = string.IsNullOrEmpty(proveedor.COL_PRO) ? null : proveedor.COL_PRO.Trim();
                domicilio.entidadFederativa = string.IsNullOrEmpty(entidadNombre) ? null : entidadNombre;
                domicilio.localidad = string.IsNullOrEmpty(proveedor.LOC_PRO) ? null : proveedor.LOC_PRO.Trim();
                domicilio.municipio = string.IsNullOrEmpty(proveedor.MUN_COVE) ? null : proveedor.MUN_COVE.Trim();
                domicilio.numeroExterior = string.IsNullOrEmpty(proveedor.NOE_PRO) ? null : proveedor.NOE_PRO.Trim();
                domicilio.numeroInterior = string.IsNullOrEmpty(proveedor.NOI_PRO) ? null : proveedor.NOI_PRO.Trim();
                domicilio.pais = string.IsNullOrEmpty(proveedor.PAI_PRO) ? null : proveedor.PAI_PRO.Trim();

                destinatario.nombre = string.IsNullOrEmpty(proveedor.NOM_PRO) ? null : proveedor.NOM_PRO.Trim();
                destinatario.apellidoPaterno = string.IsNullOrEmpty(proveedor.APE_PATE) ? null : proveedor.APE_PATE.Trim();
                destinatario.apellidoMaterno = string.IsNullOrEmpty(proveedor.APE_MATE) ? null : proveedor.APE_MATE.Trim();
                destinatario.identificacion = string.IsNullOrEmpty(proveedor.TAX_PRO) ? null : proveedor.TAX_PRO.Trim();

                destinatario.domicilio = domicilio;
            }

            switch (destinatario.tipoIdentificador)
            {
                case 1:
                    if (generico == null)
                    {
                        destinatario.domicilio = null;
                        destinatario.nombre = null;
                        destinatario.apellidoPaterno = null;
                        destinatario.apellidoMaterno = null;
                    }
                    break;
                case 2:
                    destinatario.apellidoPaterno = null;
                    destinatario.apellidoMaterno = null;
                    break;
            }

            return destinatario;
        }

        public wsVentanillaUnica.FirmaElectronica getFirmaElectronica(CatalogodeSellosDigitales objSello, string CadenaOriginal, string pMisDocumentos)
        {
            var objFirmaElectronica = new wsVentanillaUnica.FirmaElectronica();
            var objHelp = new Helper();

            objFirmaElectronica.cadenaOriginal = CadenaOriginal;
            objFirmaElectronica.certificado = Convert.FromBase64String(objSello.CertificadoBase64);
            objFirmaElectronica.firma = objHelp.GenerarFirmaCadenaByte(objSello, pMisDocumentos, CadenaOriginal);

            return objFirmaElectronica;
        }


        public CatalogodeSellosDigitales getSello(SaaioPedime objPedime, CtracClient objClient, int TipoDeFigura)
        {
            var objSello = new CatalogodeSellosDigitales();
            var objSelloD = new CatalogodeSellosDigitalesRepository(_configuration);
            try
            {
                if (objPedime.CVE_REPR == null)
                {
                    throw new ArgumentException("No existe el representante legal, favor de cerrar de nuevo el pedimento");
                }

                if (objPedime.PAT_AGEN.Trim() == "3547")
                {
                    objPedime.CVE_REPR = "03";
                }


                var objRepresentante = new SaaicRepres();
                var objRepresentanteD = new SaaicRepresRepository(_configuration);
                objRepresentante = objRepresentanteD.Buscar(objPedime.CVE_REPR.Trim());
                if (objRepresentante == null)
                {
                    throw new ArgumentException("No existe el representante legal, favor de cerrar de nuevo el pedimento");
                }

                string RFCSello = string.Empty;
                var objSelloMensajeria = new CatalogodeSellosDigitales();
                // si el RFC es EDM sella con el que tenga activo en la tabla de Sellos para mensajeria
                if (objClient.RFC_IMP == "EDM930614781")
                {

                    objSelloMensajeria = objSelloD.BuscarMensajeria();

                    if (!(objSelloMensajeria == null))
                    {
                        RFCSello = objSelloMensajeria.UsuarioWebService.Trim();
                        objSello.RFCConsulta = objSelloMensajeria.RFCConsulta.Trim();
                    }

                    else
                    {
                        var objSelloRep = new CatalogodeSellosDigitales();
                        objSelloRep = objSelloD.Buscar(RFCSello);
                        if (objSelloRep == null)
                        {
                            var objAA = new CatalogoDeAgentesAduanales();
                            var objAAD = new CatalogoDeAgentesAduanalesRepository(_configuration);
                            objAA = objAAD.BuscarRFC(objRepresentante.RFC_REP.Trim());
                            if (!(objAA == null))
                            {
                                RFCSello = objAA.Rfc;
                            }
                            else
                            {
                                RFCSello = "";
                            }
                        }
                        else
                        {
                            RFCSello = objRepresentante.RFC_REP.Trim();
                        }
                    }
                }
                else if (TipoDeFigura == 1 | TipoDeFigura == 3) // si el cliente no es generico 
                {
                    var objSelloRep = new CatalogodeSellosDigitales();
                    objSelloRep = objSelloD.Buscar(RFCSello);
                    if (objSelloRep == null)
                    {
                        var objAA = new CatalogoDeAgentesAduanales();
                        var objAAD = new CatalogoDeAgentesAduanalesRepository(_configuration);
                        objAA = objAAD.BuscarRFC(objRepresentante.RFC_REP.Trim());
                        if (!(objAA == null))
                        {
                            RFCSello = objAA.Rfc;
                        }
                        else
                        {
                            RFCSello = "";
                        }
                    }

                    else
                    {
                        RFCSello = objRepresentante.RFC_REP.Trim();
                    }
                }
                else if (objClient.RFC_IMP != "")
                {
                    RFCSello = objClient.RFC_IMP.Trim();


                }

                if (string.IsNullOrEmpty(RFCSello))
                {
                    throw new Exception("No fue posible definir quien sella esta operación");
                }

                objSello = objSelloD.Buscar(RFCSello);
                if (objClient.RFC_IMP == "EDM930614781")
                {
                    if (objSelloMensajeria == null)
                    {
                        objSello.RFCConsulta = objSello.RFCConsulta;
                    }
                    else
                    {
                        objSello.RFCConsulta = objSelloMensajeria.RFCConsulta.Trim();
                    }
                }

                if (TipoDeFigura == 1)
                {
                    if (objClient.RFC_IMP != "EDM930614781")
                    {
                        var objGenericos = new CatalogoDeRFCsGenericos();
                        var objGenericosD = new CatalogoDeRFCsGenericosRepository(_configuration);
                        objGenericos = objGenericosD.Buscar(objClient.RFC_IMP.Trim());
                        if (objGenericos == null)
                        {
                            if (objClient.RFC_IMP.Trim() != "")
                            {
                                objSello.RFCConsulta = objClient.RFC_IMP.Trim();
                            }
                        }
                    }
                }
                if (TipoDeFigura == 4 | TipoDeFigura == 5)
                {
                    var objAA = new CatalogoDeAgentesAduanales();
                    var objAAD = new CatalogoDeAgentesAduanalesRepository(_configuration);
                    objAA = objAAD.BuscarRFC(objRepresentante.RFC_REP.Trim());
                    if (!(objAA == null))
                    {
                        // objSello.RFCConsulta = objSello.RFCConsulta & ";" & objAA.Rfc.Trim()  No me funciono la cadena original apar los digitalizados
                        objSello.RFCConsulta = objAA.Rfc.Trim();
                    }
                }


                if (objSello == null)
                {
                    throw new Exception("No existen configuracion sellos para este cliente");
                }
            }

            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }

            return objSello;
        }


        public int getTipoFigura(Clientes objClienteExpertti, int Operacion)
        {
            var tipoFigura = default(int);

            switch (objClienteExpertti.TipoDeFigura)
            {
                case 1:
                    {
                        tipoFigura = 1;
                        break;
                    }

                case 4:
                    {
                        switch (Operacion)
                        {
                            case 1:
                                {
                                    tipoFigura = 5;
                                    break;
                                }
                            case 2:
                                {
                                    tipoFigura = 4;
                                    break;
                                }

                            default:
                                {
                                    break;
                                }
                        }

                        break;
                    }

                case 5:
                    {
                        switch (Operacion)
                        {
                            case 1:
                                {
                                    tipoFigura = 5;
                                    break;
                                }
                            case 2:
                                {
                                    tipoFigura = 4;
                                    break;
                                }

                            default:
                                {
                                    break;
                                }
                        }

                        break;
                    }

            }

            return tipoFigura;
        }


        public ComponentesGenerales DatosGenerales(string NumerodeReferencia, int IDDatosDeEmpresa)

        {

            var objComprobante = new ComponentesGenerales();

            var objReferencias = new Referencias();
            var objReferenciasD = new ReferenciasRepository(_configuration);
            objReferencias = objReferenciasD.Buscar(NumerodeReferencia, IDDatosDeEmpresa);
            if (objReferencias == null)
            {
                throw new Exception("No existe la referencia en el sistema Expertti");
            }

            var objPedime = new SaaioPedime();
            var objPedimeD = new SaaioPedimeRepository(_configuration);
            objPedime = objPedimeD.Buscar(objReferencias.NumeroDeReferencia);
            if (objPedime == null)
            {
                throw new Exception("No existe encabezado del pedimento en sistema CASA");
            }


            var objClient = new CtracClient();
            var objClientD = new CtracClientRepository(_configuration);
            objClient = objClientD.Buscar(objPedime.CVE_IMPO);
            if (objClient == null)
            {
                throw new Exception("No existe el cliente en el sistema Casa");
            }


            // Traigo el tipo de figura
            var objClienteExpertti = new Clientes();
            var objClientExperttiD = new ClientesRepository(_configuration);
            objClienteExpertti = objClientExperttiD.Buscar(objReferencias.IDCliente);
            if (objClienteExpertti == null)
            {
                throw new Exception("No existe el cliente en el sistema Expertti");
            }



            var objSello = new CatalogodeSellosDigitales();
            objSello = getSello(objPedime, objClient, objClienteExpertti.TipoDeFigura);
            if (objSello == null)
            {
                throw new Exception("No existen datos para sellar el Comprobante");
            }

            var objDatos = new Datos(_configuration);
            string[] ArregloRFCs = null;
            foreach (Match m in Regex.Matches(objSello.RFCConsulta, "^.*$", RegexOptions.Multiline))
                // Carga a un arreglo de "strings" los campos de la línea leída separados por "coma"
                ArregloRFCs = Regex.Split(m.ToString(), ";");


            objComprobante.tipoOperacion = objPedime.IMP_EXPO == "1" ? "TOCE.IMP" : "TOCE.EXP";
            objComprobante.Referencia = objReferencias;
            objComprobante.Pedime = objPedime;
            objComprobante.ClienteCasa = objClient;
            objComprobante.ClienteExpertti = objClienteExpertti;
            objComprobante.TipodeFigura = objDatos.getTipoFigura(objClienteExpertti, objReferencias.Operacion);

            objComprobante.RfcConsulta = ArregloRFCs;
            objComprobante.Sello = objSello;


            return objComprobante;

        }

        public ComponentesdelComprobante DatosParaELComprobante(ComponentesGenerales objGenerales, int ConsFact)
        {
            ComponentesdelComprobante objComprobante = new ComponentesdelComprobante();

            try
            {
                SaaioFactur objFactura = new SaaioFactur();
                SaaioFacturRepository objFacturaD = new SaaioFacturRepository(_configuration);
                objFactura = objFacturaD.Buscar(objGenerales.Referencia.NumeroDeReferencia, ConsFact);

                if (objFactura == null)
                    throw new Exception("No existe la factura en el sistema Casa");

                CtracProved objProveedor = new CtracProved();
                CtracProvedRepository objProveedorD = new CtracProvedRepository(_configuration);
                CtracDestinRepository objDestinatarioD = new CtracDestinRepository(_configuration);
                ConsolFacturas objConsolFacturas = new ConsolFacturas();
                ConsolFacturasRepository objConsolFacturasD = new ConsolFacturasRepository(_configuration);

                objConsolFacturas = objConsolFacturasD.Buscar(objGenerales.Referencia.NumeroDeReferencia.Trim(), ConsFact);

                switch (objGenerales.Referencia.Operacion)
                {
                    case 1:
                        objProveedor = objProveedorD.Buscar(objFactura.CVE_PROV);
                        if (objProveedor == null)
                            throw new Exception("No existe el proveedor en el sistema Casa");
                        break;

                    case 2:
                        string Clave;
                        if (objFactura.CVE_PROV2 != null)
                        {
                            Clave = string.IsNullOrEmpty(objFactura.CVE_PROV2) ? objFactura.CVE_PROV : objFactura.CVE_PROV2;
                        }
                        else
                        {
                            Clave = objFactura.CVE_PROV;
                        }

                        objProveedor = objDestinatarioD.Buscar(Clave);
                        if (objProveedor == null)
                            throw new Exception("No existe el Destinatario/Comprador en el sistema Casa");
                        break;
                }

                if (objConsolFacturas == null)
                {
                    if (objProveedor.EFE_PRO.ToUpper() == "DF")
                        objProveedor.EFE_PRO = "CDMX";
                }

                List<SaaioFacpar> lstPartidas = new List<SaaioFacpar>();
                SaaioFacparRepository objpartidaD = new SaaioFacparRepository(_configuration);
                lstPartidas = objpartidaD.Cargar(objGenerales.Referencia.NumeroDeReferencia, ConsFact);

                if (lstPartidas == null)
                    throw new Exception("No existe la mercancia en el sistema Expertti, para la factura No." + objFactura.NUM_FACT);

                Datos objDatos = new Datos(_configuration);

                objComprobante.Factura = objFactura;
                objComprobante.Mercancias = objDatos.getMercancias(lstPartidas, objFactura);

                CatalogodeSellosDigitales objSello = new CatalogodeSellosDigitales();
                var objSelloD = new CatalogodeSellosDigitalesRepository(_configuration);
                objSello = objSelloD.BuscarMensajeria();
                string RfcMensajeria = string.Empty;
                if (objSello != null)
                {
                    RfcMensajeria = objSello.UsuarioWebService.Trim();
                }

                if (objConsolFacturas == null)
                {
                    objComprobante.Emisor = objDatos.getEmisor(objGenerales.Referencia.Operacion, objGenerales.ClienteCasa, objProveedor);
                    objComprobante.Destiantario = objDatos.getDestinatario(objGenerales.Referencia.Operacion, objGenerales.ClienteCasa, objProveedor, RfcMensajeria);
                }
                else
                {
                    Clientes objCliente = new Clientes();
                    ClientesRepository objClienteD = new ClientesRepository(_configuration);
                    objCliente = objClienteD.Buscar(objConsolFacturas.IdCliente);

                    if (objCliente == null)
                        throw new ArgumentException("es Necesario capturar el destinatario  de la factura");

                    CtracClient objCtracClient = new CtracClient();
                    CtracClientRepository objCtracClientD = new CtracClientRepository(_configuration);
                    objCtracClient = objCtracClientD.Buscar(objCliente.Clave);

                    if (objCtracClient == null)
                        throw new ArgumentException("No existe cliente asociado, pedimento global");

                    objComprobante.Destiantario = objDatos.getDestinatario(objGenerales.Referencia.Operacion, objCtracClient, objProveedor, RfcMensajeria);
                    objComprobante.Emisor = objDatos.getEmisor(objGenerales.Referencia.Operacion, objCtracClient, objProveedor);
                }

                objComprobante.Proveedor = objProveedor;
                objComprobante.Subdivision = objFactura.SUB_FACT == "N" ? 0 : 1;
                objComprobante.CertificadoOrigen = objFactura.CER_ORIG == "N" ? 0 : 1;
            }
            catch (Exception ex)
            {
                objComprobante = null;
                throw new Exception(ex.Message.Trim());
            }
            return objComprobante;
        }

        public wsVentanillaUnica.Mercancia[] getMercancias(List<SaaioFacpar> lstPartidas, SaaioFactur lFacturas)
        {
            var lstMercancias = new wsVentanillaUnica.Mercancia[lstPartidas.Count];

            try
            {
                int contadorMerc = 0;
                foreach (var itemMercancia in lstPartidas)
                {
                    var objMercancia = new wsVentanillaUnica.Mercancia();

                    var objUnidadD = new CtarcUnidadRepository(_configuration);
                    CtarcUnidad objUnidad;

                    try
                    {
                        objUnidad = objUnidadD.Buscar(Convert.ToInt32(itemMercancia.UNI_COVE));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Se produjo un error al localizar la unidad de medida para la partida No.{itemMercancia.NUM_PART}");
                    }

                    if (objUnidad == null)
                        throw new Exception($"No existe unidad de medida para la partida No.{itemMercancia.NUM_PART}");

                    var objMonedasCoveD = new CatalogodeMonedasCOVERepository(_configuration);
                    var objMonedasCove = objMonedasCoveD.Buscar(itemMercancia.TIP_MONE);
                    if (objMonedasCove != null)
                    {
                        itemMercancia.TIP_MONE = objMonedasCove.OMA.Trim();
                    }

                    objMercancia.descripcionGenerica = itemMercancia.DESC_COVE.Trim();
                    objMercancia.claveUnidadMedida = objUnidad.CVE_COVE.Trim();
                    objMercancia.cantidad = Convert.ToDecimal(itemMercancia.CANT_COVE.ToString("0.000"));
                    objMercancia.valorUnitario = Convert.ToDecimal(((itemMercancia.MON_FACT + itemMercancia.VAL_AGRE) / itemMercancia.CANT_COVE).ToString("0.000000"));
                    objMercancia.valorTotal = Convert.ToDecimal((itemMercancia.MON_FACT + itemMercancia.VAL_AGRE).ToString("0.0000"));

                    if (itemMercancia.TIP_MONE == "MXN" && lFacturas.EQU_DLLS == 1)
                    {
                        throw new Exception($"A T E N C I O N... Cuando la moneda de la factura son pesos mexicanos, la equivalencia no puede ser 1, favor de verificar {itemMercancia.NUM_PART}");
                    }

                    objMercancia.valorDolares = Convert.ToDecimal(((itemMercancia.MON_FACT + itemMercancia.VAL_AGRE) * lFacturas.EQU_DLLS).ToString("0.0000"));
                    objMercancia.tipoMoneda = itemMercancia.TIP_MONE.Trim();
                    
                    wsVentanillaUnica.DescripcionMercancia[] descEspc;

                    var objSerieD = new MSComunesExpertti.RepositoriesCasa.SaaioCoveSerRepository(_configuration);
                    var lstSeries = objSerieD.CargarSeries(itemMercancia.NUM_REFE.Trim(), itemMercancia.CONS_FACT, itemMercancia.CONS_PART);

                    if (lstSeries == null || lstSeries.Count == 0)
                    {
                        descEspc = null;
                    }
                    else
                    {
                        descEspc = new wsVentanillaUnica.DescripcionMercancia[lstSeries.Count];
                        int contador = 0;

                        foreach (var itemSerie in lstSeries)
                        {
                            var objDescEspc = new wsVentanillaUnica.DescripcionMercancia
                            {
                                numeroSerie = string.IsNullOrWhiteSpace(itemSerie.NUM_SERI) ? null : itemSerie.NUM_SERI.Trim(),
                                modelo = string.IsNullOrWhiteSpace(itemSerie.NUM_PART) ? null : itemSerie.NUM_PART.Trim(),
                                marca = string.IsNullOrWhiteSpace(itemSerie.MAR_MERC) ? null : itemSerie.MAR_MERC.Trim(),
                                subModelo = string.IsNullOrWhiteSpace(itemSerie.SUB_MODE) ? null : itemSerie.SUB_MODE.Trim()
                            };

                            descEspc[contador++] = objDescEspc;
                        }
                    }

                    objMercancia.descripcionesEspecificas = descEspc;
                    lstMercancias[contadorMerc++] = objMercancia;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return lstMercancias;
        }


        public wsVentanillaUnica.Emisor getEmisor(int lOperacion, CtracClient lCliente, CtracProved lProveedor)
        {
            var objEmisor = new wsVentanillaUnica.Emisor();
            var objDomicilioEmisor = new wsVentanillaUnica.Domicilio();

            if (lOperacion == 1)
            {
                objEmisor.tipoIdentificador = (lProveedor.TAX_PRO == "SIN TAX ID") ? 3 : 0;

                var ctarEntFedData = new CtarcEntFedRepository(_configuration);
                var ctarEntidad = ctarEntFedData.Buscar(lProveedor.PAI_PRO, lProveedor.EFE_PRO.Trim());

                string entidadFederativa = ctarEntidad != null ? ctarEntidad.NOM_EFED.Trim() : null;

                objDomicilioEmisor.calle = string.IsNullOrWhiteSpace(lProveedor.DIR_PRO) ? null : lProveedor.DIR_PRO.Trim();
                objDomicilioEmisor.codigoPostal = string.IsNullOrWhiteSpace(lProveedor.ZIP_PRO) ? null : lProveedor.ZIP_PRO.Trim();
                objDomicilioEmisor.colonia = string.IsNullOrWhiteSpace(lProveedor.COL_PRO) ? null : lProveedor.COL_PRO.Trim();
                objDomicilioEmisor.entidadFederativa = string.IsNullOrWhiteSpace(entidadFederativa) ? null : entidadFederativa;
                objDomicilioEmisor.localidad = string.IsNullOrWhiteSpace(lProveedor.LOC_PRO) ? null : lProveedor.LOC_PRO.Trim();
                objDomicilioEmisor.municipio = string.IsNullOrWhiteSpace(lProveedor.MUN_COVE) ? null : lProveedor.MUN_COVE.Trim();
                objDomicilioEmisor.numeroExterior = string.IsNullOrWhiteSpace(lProveedor.NOE_PRO) ? null : lProveedor.NOE_PRO.Trim();
                objDomicilioEmisor.numeroInterior = string.IsNullOrWhiteSpace(lProveedor.NOI_PRO) ? null : lProveedor.NOI_PRO.Trim();
                objDomicilioEmisor.pais = string.IsNullOrWhiteSpace(lProveedor.PAI_PRO) ? null : lProveedor.PAI_PRO.Trim();

                objEmisor.nombre = string.IsNullOrWhiteSpace(lProveedor.NOM_PRO) ? null : lProveedor.NOM_PRO.Trim();
                objEmisor.apellidoPaterno = string.IsNullOrWhiteSpace(lProveedor.APE_PATE) ? null : lProveedor.APE_PATE.Trim();
                objEmisor.apellidoMaterno = string.IsNullOrWhiteSpace(lProveedor.APE_MATE) ? null : lProveedor.APE_MATE.Trim();
                objEmisor.identificacion = string.IsNullOrWhiteSpace(lProveedor.TAX_PRO) ? null : lProveedor.TAX_PRO.Trim();

                objEmisor.domicilio = objDomicilioEmisor;
            }
            else
            {
                objDomicilioEmisor.calle = string.IsNullOrWhiteSpace(lCliente.DIR_IMP) ? null : lCliente.DIR_IMP.Trim();
                objDomicilioEmisor.codigoPostal = string.IsNullOrWhiteSpace(lCliente.CP_IMP) ? null : lCliente.CP_IMP.Trim();
                objDomicilioEmisor.colonia = string.IsNullOrWhiteSpace(lCliente.COL_IMP) ? null : lCliente.COL_IMP.Trim();
                objDomicilioEmisor.entidadFederativa = string.IsNullOrWhiteSpace(lCliente.EFE_IMP) ? null : lCliente.EFE_IMP.Trim();
                objDomicilioEmisor.localidad = string.IsNullOrWhiteSpace(lCliente.LOC_IMP) ? null : lCliente.LOC_IMP.Trim();
                objDomicilioEmisor.municipio = string.IsNullOrWhiteSpace(lCliente.MUN_COVE) ? null : lCliente.MUN_COVE.Trim();
                objDomicilioEmisor.numeroExterior = string.IsNullOrWhiteSpace(lCliente.NOE_IMP) ? null : lCliente.NOE_IMP.Trim();
                objDomicilioEmisor.numeroInterior = string.IsNullOrWhiteSpace(lCliente.NOI_IMP) ? null : lCliente.NOI_IMP.Trim();
                objDomicilioEmisor.pais = string.IsNullOrWhiteSpace(lCliente.PAI_IMP) ? null : lCliente.PAI_IMP.Trim();

                objEmisor.nombre = string.IsNullOrWhiteSpace(lCliente.NOM_IMP) ? null : lCliente.NOM_IMP.Trim();
                objEmisor.apellidoPaterno = string.IsNullOrWhiteSpace(lCliente.APE_PATE) ? null : lCliente.APE_PATE.Trim();
                objEmisor.apellidoMaterno = string.IsNullOrWhiteSpace(lCliente.APE_MATE) ? null : lCliente.APE_MATE.Trim();

                var objGenericosD = new CatalogoDeRFCsGenericosRepository(_configuration);
                var objGenericos = objGenericosD.Buscar(lCliente.RFC_IMP.Trim());

                var objClientMensD = new CatalogoDeClientesDeDeMensajeriaRepository(_configuration);
                var objClientMens = objClientMensD.Buscar(Convert.ToInt32(lCliente.CVE_IMP));

                string rfcCase = lCliente.RFC_IMP.Trim();
                if (objClientMens != null)
                {
                    rfcCase = "XAXX010101000";
                }

                if (string.IsNullOrWhiteSpace(rfcCase))
                {
                    objEmisor.tipoIdentificador = 0;
                    objEmisor.identificacion = lCliente.CUR_IMP.Trim();
                }
                else if (rfcCase == "XAXX010101000")
                {
                    objEmisor.tipoIdentificador = 3;
                    objEmisor.identificacion = !string.IsNullOrWhiteSpace(lCliente.RFC_IMP) ? lCliente.RFC_IMP.Trim() : lCliente.CUR_IMP;
                }
                else if (objGenericos != null)
                {
                    objEmisor.tipoIdentificador = 3;
                    objEmisor.identificacion = lCliente.RFC_IMP.Trim();
                }
                else
                {
                    objEmisor.tipoIdentificador = 1;
                    objEmisor.identificacion = lCliente.RFC_IMP.Trim();
                }

                objEmisor.domicilio = objDomicilioEmisor;
            }

            return objEmisor;
        }



    }
}

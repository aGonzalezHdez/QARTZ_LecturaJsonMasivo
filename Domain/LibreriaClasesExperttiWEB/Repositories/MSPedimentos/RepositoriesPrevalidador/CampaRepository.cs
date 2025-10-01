using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wsAirBus;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPrevalidador
{
    public class CampaRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn = null!;

        public CampaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public bool ExisteEnRelacionFacturasFrcciones(string estaReferencia)
        {
            bool existeEnRelacion = false;

            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.existeEnRelacionFacturasFrcciones", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmd.Parameters.AddWithValue("@NUM_REFE", estaReferencia);
                    SqlParameter returnParam = new SqlParameter("@Existe", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    cmd.Parameters.Add(returnParam);

                    // Open connection and execute the command
                    cn.Open();
                    cmd.ExecuteScalar();

                    // Retrieve the return value
                    existeEnRelacion = (bool)cmd.Parameters["@Existe"].Value;
                }
            }

            return existeEnRelacion;
        }
        public wsAirBus.Encabezado LlenarEncabezado(SaaioPedime objPedime)
        {
            wsAirBus.Encabezado encabezado = new wsAirBus.Encabezado();

            try
            {
                encabezado.Pedimento = objPedime.FEC_PAGO.ToString().Substring(8, 2) + "-" +
                                       objPedime.ADU_ENTR.ToString() + "-" +
                                       objPedime.PAT_AGEN.ToString() + "-" +
                                       objPedime.NUM_PEDI.ToString();

                encabezado.ClaveOperacion = Convert.ToInt32(objPedime.IMP_EXPO);
                encabezado.ClaveDocumento = objPedime.CVE_PEDI.ToString();
                encabezado.ClaveDestino = Convert.ToInt32(objPedime.DES_ORIG);
                encabezado.TipoCambio = Convert.ToDecimal(objPedime.TIP_CAMB);
                encabezado.PesoBruto = Convert.ToDecimal(objPedime.PES_BRUT);
                encabezado.Patente = Convert.ToInt32(objPedime.PAT_AGEN.ToString());
                encabezado.ClaveAduEntradaSalida = objPedime.ADU_ENTR.ToString();
                encabezado.ClaveMedTransSalida = Convert.ToInt32(objPedime.MTR_SALI);
                encabezado.ClaveMedTransArribo = Convert.ToInt32(objPedime.MTR_ARRI);
                encabezado.ClaveMedTransEntrada = Convert.ToInt32(objPedime.MTR_ENTR);
                encabezado.ValorDolares = Convert.ToDecimal(objPedime.VAL_DLLS);
                encabezado.ValorAduanalTotal = Convert.ToDecimal(objPedime.VAL_NORM);
                encabezado.ValorComercialTotal = Convert.ToDecimal(objPedime.VAL_COME);
                encabezado.Aduana = Convert.ToInt32(objPedime.ADU_ENTR);
                encabezado.Regimen = objPedime.REG_ADUA.ToString();
                encabezado.ValorSeguros = Convert.ToInt32(objPedime.MON_VASE);

                encabezado.Marcas = objPedime.MAR_NUME == null ? "" : objPedime.MAR_NUME.ToString();
                encabezado.Numeros = 0;
                encabezado.Referencia = objPedime.NUM_REFE;
                encabezado.Acuse = objPedime.FIR_ELEC;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in LlenarEncabezado: " + ex.Message);
            }

            return encabezado;
        }
        public wsAirBus.Rectificaciones LlenarRectificaciones(SaaioPedime objPedime)
        {
            wsAirBus.Rectificaciones objRectificaciones = new wsAirBus.Rectificaciones();

            try
            {
                if (objPedime.TIP_PEDI == "R1")
                {
                    objRectificaciones.FechaPago = Convert.ToDateTime(objPedime.FEC_PAGOO.ToString("dd/MM/yyyy"));
                    objRectificaciones.PedimentoOriginal = objPedime.FEC_PAGOO.ToString().Substring(8, 2) + "-" +
                                                           objPedime.ADU_ENTR.ToString() + "-" +
                                                           objPedime.PAT_AGENO.ToString() + "-" +
                                                           objPedime.NUM_PEDIO.ToString();
                    objRectificaciones.Referencia = objPedime.NUM_REFEO;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in LlenarRectificaciones: " + ex.Message);
            }

            return objRectificaciones;
        }

        //pendiente
        //public ImportadorExportador LlenarImportadorExportador(string referencia, int bultos)
        //{
        //    ImportadorExportador objImportadorExportador = new ImportadorExportador();
        //    SaaioIncremRepository objIncremD = new SaaioIncremRepository(_configuration);
        //    SaaioIncrem objIncrem;

        //    try
        //    {
        //        // Seguros
        //        objIncrem = objIncremD.BuscarMXN(referencia, "2");
        //        objImportadorExportador.Seguros = objIncrem == null ? 0 : Convert.ToDecimal(objIncrem.IMP_INCR);

        //        // Fletes
        //        objIncrem = objIncremD.BuscarMXN(referencia, "1");
        //        objImportadorExportador.Fletes = objIncrem == null ? 0 : Convert.ToDecimal(objIncrem.IMP_INCR);

        //        // Embalajes
        //        objIncrem = objIncremD.BuscarMXN(referencia, "3");
        //        objImportadorExportador.Embalajes = objIncrem == null ? 0 : Convert.ToDecimal(objIncrem.IMP_INCR);

        //        // Otros Incrementables
        //        objIncrem = objIncremD.BuscarMXN(referencia, "4");
        //        objImportadorExportador.Incrementables = objIncrem == null ? 0 : Convert.ToDecimal(objIncrem.IMP_INCR);

        //        // Bultos
        //        objImportadorExportador.Bultos = bultos;

        //        // Fechas
        //        List<FechasAirbus> lstObjFechasAirbus = new FechasAirbusRepository(_configuration).Cargar(referencia);
        //        if (lstObjFechasAirbus != null && lstObjFechasAirbus.Count > 0)
        //        {
        //            wsAirBus.FechaImpExp[] objArrayFechaImpExp = new wsAirBus.FechaImpExp[lstObjFechasAirbus.Count];
        //            int apuFecha = 0;

        //            foreach (FechasAirbus objFechasAirbus in lstObjFechasAirbus)
        //            {
        //                wsAirBus.FechaImpExp objFechaImpExp = new wsAirBus.FechaImpExp
        //                {
        //                    ClaveTipo = objFechasAirbus.ClaveFecha,
        //                    Fecha = objFechasAirbus.Fecha.ToString("dd/MM/yyyy")
        //                };

        //                objArrayFechaImpExp[apuFecha] = objFechaImpExp;
        //                apuFecha++;
        //            }

        //            objImportadorExportador.FechaImpExp = objArrayFechaImpExp;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error en LlenarImportadorExportador: " + ex.Message);
        //    }

        //    return objImportadorExportador;
        //}
        public void RegistraEnvio(string estaReferencia, string respuestaXML, bool referenciaEnviada)
        {
            using (SqlConnection cn = new SqlConnection(sConexion))
            using (SqlCommand cmd = new SqlCommand("NET_INSERT_BITACORAENVIOCAMPA", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@NUMREFE", SqlDbType.VarChar, 15)
                {
                    Value = estaReferencia
                });

                cmd.Parameters.Add(new SqlParameter("@RESPUESTA_XML", SqlDbType.VarChar, 2000)
                {
                    Value = respuestaXML
                });

                cmd.Parameters.Add(new SqlParameter("@ENVIADA", SqlDbType.Bit)
                {
                    Value = referenciaEnviada
                });

                SqlParameter outputParam = new SqlParameter("@newid_registro", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error in RegistraEnvio: " + ex.Message);
                }
                // The connection and command are disposed automatically at the end of the using block
            }
        }

        //pendiente
        //public wsAirBus.TNPCL[] LlenarTNPCL(string referencia)
        //{
        //    SaaioContPedData objContPedD = new SaaioContPedData();
        //    List<SaaioContPed> objLstContPed = new List<SaaioContPed>();
        //    wsAirBus.TNPCL[] objArrayAirbusContPed;
        //    int iContPed = 0;

        //    ContribucionesAirbusData objContAirbusD = new ContribucionesAirbusData();
        //    List<ContribucionesAirbus> lstObjContAirbus = new List<ContribucionesAirbus>();

        //    try
        //    {
        //        lstObjContAirbus = objContAirbusD.Cargar(referencia);

        //        if (lstObjContAirbus != null)
        //        {
        //            objArrayAirbusContPed = new wsAirBus.TNPCL[lstObjContAirbus.Count];
        //            int apuContAirbus = 0;

        //            foreach (ContribucionesAirbus objContAirbus in lstObjContAirbus)
        //            {
        //                if (objContAirbus.Clave != "98" && objContAirbus.Clave != "99")
        //                {
        //                    wsAirBus.TNPCL objAirbusContPed = new wsAirBus.TNPCL
        //                    {
        //                        ClaveContribucion = Convert.ToInt32(objContAirbus.Clave.ToString()),
        //                        ClaveFPago = Convert.ToInt32(objContAirbus.FormaDePago.ToString()),
        //                        Importe = objContAirbus.Clave == "15"
        //                            ? Convert.ToDecimal(objContAirbus.Importe) - Convert.ToDecimal(objContAirbus.Importe - objContAirbus.TasaAplicable)
        //                            : Convert.ToDecimal(objContAirbus.Importe),
        //                        TasaAplicable = Convert.ToDecimal(objContAirbus.TasaAplicable),
        //                        ClaveTipoTasa = Convert.ToInt32(objContAirbus.ClaveTipoTasa.ToString())
        //                    };

        //                    objArrayAirbusContPed[apuContAirbus] = objAirbusContPed;
        //                    apuContAirbus++;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            objArrayAirbusContPed = new wsAirBus.TNPCL[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error en LlenarTNPCL: " + ex.Message);
        //    }

        //    return objArrayAirbusContPed;
        //}

        //pendiente
        //public wsAirBus.Facturas[] LlenarFacturas(string referencia, int impExpo, string rfcCliente, string misDocumentos)
        //{
        //    wsAirBus.Facturas[] arrayAirbusFacturas;
        //    try
        //    {
        //        // Initialize the data access objects
        //        var objFacturasD = new SaaioFacturRepository(_configuration);
        //        var objLstFacturas = objFacturasD.Cargar(referencia);

        //        // Check if the list is null or empty
        //        if (objLstFacturas == null || objLstFacturas.Count() == 0)
        //        {
        //            throw new ArgumentException($"La Referencia: {referencia} no tiene Facturas.");
        //        }

        //        // Initialize the array with the size of the list
        //        arrayAirbusFacturas = new wsAirBus.Facturas[objLstFacturas.Count];

        //        int iFactur = 0;
        //        foreach (var objFactura in objLstFacturas)
        //        {
        //            var objUnaFactura = new wsAirBus.Facturas
        //            {
        //                FechaFactura = objFactura.FEC_FACT, // Date - OK
        //                Factura = objFactura.NUM_FACT2.ToString(), // Invoice Number - OK
        //                ClaveMoneda = objFactura.MON_FACT.ToString(), // Currency - OK
        //                ValorDolares = Convert.ToDecimal(objFactura.VAL_DLLS.ToString("#############.####")), // Dollar Value
        //                ValorMonedaExt = Convert.ToDecimal(objFactura.VAL_EXTR), // Foreign Currency Value
        //                FactorMoneda = Convert.ToDecimal(objFactura.EQU_DLLS),
        //                ProveedorComprador = LlenarProveedorComprador(impExpo, objFactura.CVE_PROV),
        //                ClaveFactura = objFactura.ICO_FACT.ToString()
        //            };

        //            var objCoveD = new SaaioCoveData();
        //            var objCove = objCoveD.Buscar(referencia, objFactura.CONS_FACT);
        //            objUnaFactura.COVE = objCove == null ? "" : objCove.E_DOCUMENT.Trim();

        //            objUnaFactura.partidas = LlenarPartidas(referencia, objFactura.CONS_FACT, rfcCliente, misDocumentos);

        //            arrayAirbusFacturas[iFactur] = objUnaFactura;
        //            iFactur++;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error en llenarFacturas: {referencia} {ex.Message}");
        //    }

        //    return arrayAirbusFacturas;
        //}

        //pendiente
        //public string LlenarProveedorComprador(int IMP_EXPO, string CVE_PROV)
        //{
        //    string lProveedorComprador = string.Empty;

        //    try
        //    {
        //        if (IMP_EXPO == 1)
        //        {
        //            CtracProvedData objProveedD = new CtracProvedData();
        //            CtracProved objProveed = new CtracProved();
        //            objProveed = objProveedD.Buscar(CVE_PROV);

        //            if (objProveed == null) // NOMBRE, DENOMINACIÓN O RAZÓN SOCIAL
        //            {
        //                lProveedorComprador = "";
        //            }
        //            else
        //            {
        //                lProveedorComprador = objProveed.NOM_PRO.ToString();
        //            }
        //        }
        //        else
        //        {
        //            CtracDestinData objProveedD = new CtracDestinData();
        //            CtracProved objProveed = new CtracProved();
        //            objProveed = objProveedD.Buscar(CVE_PROV);

        //            if (objProveed == null) // NOMBRE, DENOMINACIÓN O RAZÓN SOCIAL
        //            {
        //                lProveedorComprador = "";
        //            }
        //            else
        //            {
        //                lProveedorComprador = objProveed.NOM_PRO.ToString();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error en LlenarProveedorComprador: " + ex.Message);
        //    }

        //    return lProveedorComprador;
        //}

        public wsAirBus.Guias[] LlenarGuias(string referencia)
        {
            wsAirBus.Guias[] arrayAirbusGuiasEmbarque;

            try
            {
                var objGuiasD = new SaaioGuiasRepository(_configuration);
                var objLstGuias = objGuiasD.CargarTodas(referencia);

                if (objLstGuias == null || objLstGuias.Count() == 0)
                {
                    return new wsAirBus.Guias[0]; // Return an empty array if no guias are found
                }

                arrayAirbusGuiasEmbarque = new wsAirBus.Guias[objLstGuias.Count()];
                int iGuias = 0;

                foreach (var objGuias in objLstGuias)
                {
                    var objUnasGuias = new wsAirBus.Guias
                    {
                        GuiaManifiesto = objGuias.GUIA, // Guide Number/ID
                        TipoGuia = objGuias.IDE_MH
                    };

                    arrayAirbusGuiasEmbarque[iGuias] = objUnasGuias;
                    iGuias++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en llenarGuias: {ex.Message}");
            }

            return arrayAirbusGuiasEmbarque;
        }
        public bool LlenarCompensaciones(string Referencia, ref wsAirBus.Compensaciones[] wsAirBusCompensaciones)
        {
            wsAirBus.Compensaciones[] objArrayAirbusCompensaciones = new wsAirBus.Compensaciones[0];

            SaaioCompenRepository objCommpenD = new SaaioCompenRepository(_configuration);
            List<SaaioCompen> objLstCompen = new List<SaaioCompen>();
            int iCompen = 0;
            bool regresaValor = false;

            try
            {
                objLstCompen = objCommpenD.Buscar(Referencia);
                regresaValor = objLstCompen != null;

                if (objLstCompen != null)
                {
                    objArrayAirbusCompensaciones = new wsAirBus.Compensaciones[objLstCompen.Count];

                    foreach (SaaioCompen objUnaCompen in objLstCompen)
                    {
                        wsAirBus.Compensaciones objUnaCompensacion = new wsAirBus.Compensaciones();

                        objUnaCompensacion.AduOriginal_Clave = objUnaCompen.ADU_ORIG.ToString();
                        objUnaCompensacion.FechapagoOriginal = objUnaCompen.FEC_PAGOO;
                        objUnaCompensacion.Gravamen_Clave = Convert.ToInt32(objUnaCompen.CVE_IMPU.ToString());
                        objUnaCompensacion.IdPedimento = objUnaCompen.NUM_PEDIO.ToString();
                        objUnaCompensacion.Monto = Convert.ToInt32(objUnaCompen.TOT_IMPU);
                        objUnaCompensacion.PatenteOriginal = Convert.ToInt32(objUnaCompen.PAT_ORIG.ToString());

                        objArrayAirbusCompensaciones[iCompen] = objUnaCompensacion;
                        iCompen++;
                    }

                    wsAirBusCompensaciones = objArrayAirbusCompensaciones;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en LlenarCompensaciones: " + ex.Message);
            }

            return regresaValor;
        }

        ////Pendiente
        //public void EnviaEstaReferencia(string estaReferencia, string pMisDocumentos)
        //{

        //    wsAirBus.Pedimento objPedimento = new wsAirBus.Pedimento();

        //    SaaioPedimeRepository objPedimeD = new SaaioPedimeRepository(_configuration);
        //    SaaioPedime objPedime = new SaaioPedime();

        //    ReferenciasRepository objReferenciasD = new ReferenciasRepository(_configuration);
        //    Referencias objReferencias = new Referencias();

        //    ClientesRepository objClientesD = new ClientesRepository(_configuration);
        //    Clientes objClientes = new Clientes();

        //    wsAirBus.Compensaciones[] wsAirBusCompensaciones;

        //    bool referenciaEnviable = ExisteEnRelacionFacturasFrcciones(estaReferencia);

        //    string RFCCliente;

        //    if (referenciaEnviable)
        //    {
        //        // Enviarla
        //        objReferencias = objReferenciasD.Buscar(estaReferencia);
        //        objClientes = objClientesD.Buscar(objReferencias.IDCliente);
        //        RFCCliente = objClientes.RFC;

        //        objPedime = objPedimeD.Buscar(estaReferencia);
        //        objPedimento.Encabezado = LlenarEncabezado(objPedime);
        //        objPedimento.Rectificaciones = LlenarRectificaciones(objPedime);
        //        objPedimento.ImportadorExportador = LlenarImportadorExportador(estaReferencia, Convert.ToInt32(objPedime.CAN_BULT));
        //        objPedimento.TNPCL = LlenarTNPCL(estaReferencia);
        //        objPedimento.Facturas = LlenarFacturas(estaReferencia, Convert.ToInt32(objPedime.IMP_EXPO), RFCCliente, pMisDocumentos);
        //        objPedimento.Guias = LlenarGuias(estaReferencia);
        //        objPedimento.Identificadores_Pedimento = LlenarIdentificadores_Pedimento(estaReferencia);

        //        if (LlenarCompensaciones(estaReferencia,  wsAirBusCompensaciones))
        //        {
        //            objPedimento.Compensaciones = wsAirBusCompensaciones;
        //        }

        //        objPedimento.Observaciones_Pedimento = !string.IsNullOrEmpty(objPedime.AUT_OBSE)
        //            ? LlenarObservaciones_Pedimento(objPedime.AUT_OBSE.Trim())
        //            : LlenarObservaciones_Pedimento("");

        //        objPedimento.ContCarroFerrocarriloNumEconodeVehiculo = LlenarContenedoresFFCC(estaReferencia, objPedime.NUM_PEDI);

        //        SaveXML($"{pMisDocumentos}\\ExperttiTmp\\CAMPA\\{RFCCliente.Trim()}\\{estaReferencia}.Xml", objPedimento, objPedimento.GetType());

        //        // Call web service
        //        var objAirbusMetodos = new ServicioCommerceSoapClient();
        //        var objArrayPedimentoEnviar = new wsAirBus.Pedimento[1];

        //        objReferencias = objReferenciasD.Buscar(estaReferencia);

        //        DatosDeLaEmpresaData objEmpresasD = new DatosDeLaEmpresaData();
        //        DatosDeLaEmpresa objEmpresas = objEmpresasD.Buscar(objReferencias.IDDatosDeEmpresa);

        //        objArrayPedimentoEnviar[0] = objPedimento;
        //        string respuestaXML = objAirbusMetodos.SetPedimentos(objArrayPedimentoEnviar, objEmpresas.RFC.ToUpper(), RFCCliente);

        //        RegistraEnvio(estaReferencia, respuestaXML, referenciaEnviable);

        //    }
        //    else
        //    {
        //        // Register it properly
        //        RegistraEnvio(estaReferencia, "", referenciaEnviable);
        //    }
        //}

        public void EnviaDespuesDelPago(ref string misPedimentos, int miIdUsuario, string pMisDocumentos)
        {
            using (SqlConnection cn = new SqlConnection())
            using (SqlCommand cmd = new SqlCommand())
            {
                SqlDataReader dr = null;

                try
                {
                    // Set up the stored procedure
                    cmd.CommandText = "NET_SABER_SI_ENVIA_A_WS_CAMPA";
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmd.Parameters.Add(new SqlParameter("@ArrayPedimentos", SqlDbType.VarChar) { Value = misPedimentos });
                    cmd.Parameters.Add(new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = miIdUsuario });

                    cn.ConnectionString = sConexion;
                    cn.Open();

                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            //pendiente:
                            //EnviaEstaReferencia(dr["Referencia"].ToString(), pMisDocumentos);
                            //fin pendiente:
                        }
                    }

                    dr.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    cmd.Parameters.Clear();

                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                }
            }
        }

    }
}

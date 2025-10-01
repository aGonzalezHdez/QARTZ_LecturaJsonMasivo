//using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;

//namespace LibreriaClasesAPIExpertti.Services.Turnado
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Data.SqlClient;
//    using System.Data;
//    using System.Globalization;
//    using System.IO;
//    using System.Linq;
//    using System.Threading.Tasks;
//    using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
//    using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
//    using LibreriaClasesAPIExpertti.Entities.EntitiesControlSalidas;
//    using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
//    using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
//    using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
//    //using API_MSConsumoExternos.Controllers.ControllersWSExternos.ApiWec;
//    //using API_MSConsumoExternos.Controllers..wsExternos.JCJF;
//    using wsVentanillaUnica;
//    using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
//    using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
//    using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesOperaciones;
//    using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesTurnado;
//    using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
//    using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas;
//    using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesNube;
//    using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
//    using wsCentralizar;
//    using Microsoft.VisualBasic;
//    using CatalogoDeUsuarios = CatalogoDeUsuarios;
//    //using Chilkat;
//    using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
//    using LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos;
//    using LibreriaClasesAPIExpertti.Entities.EntitiesTurnadoImpo;
//    using LibreriaClasesAPIExpertti.Entities.EntitiesWs;
//    using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes;
//    using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
//    using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
//    using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
//    using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPedimentos;
//    using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesDocumentosPorGuia;
//    using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales;
//    using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
//    using Microsoft.Extensions.Configuration;
//    using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesJCJF;
//    using LibreriaClasesAPIExpertti.Entities.EntitiesTrazabilidad;
//    using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
//    using LibreriaClasesAPIExpertti.Entities.EntitiesCMF;
//    using LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF;
//    using LibreriaClasesAPIExpertti.Entities.EntitiesGlobal;
//    using LibreriaClasesAPIExpertti.Repositories.MSGlobal;
//    using LibreriaClasesAPIExpertti.Entities.EntitiesDespacho;

//    public partial class TurnadoImportacion
//    {
//        public IConfiguration _configuration;
//        public string SConexion { get; set; }
//        public string MyConnectionStringGP { get; set; }

//        public TurnadoImportacion(IConfiguration configuration)
//        {
//            _configuration = configuration;
//            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
//            MyConnectionStringGP = _configuration.GetConnectionString("dbCASAEIGP")!;
//        }
//        public async Task<TurnadoImpoR> Procesar(string GuiaHouse, int IdUsuario, int IDDatosDeEmpresa, int IdOficina, int IdDepartamento, string Guia2d, bool rdbUnitarias, bool rdbPartidas, bool rdbMiscelaneas, bool rdbVolumen, bool rdbTransito, bool chkPreAlertas, bool chkDiasAnteriores, bool chkInformaciones, bool validaNoManisfestado, bool validaEncargoCliente, int avisarDiffAgenteAdu, bool chkbSubdivision, int IDGrupodeTrabajoSelected, int validaNubePrepago, CatalogoDeUsuarios ObjUsuario, bool RevDsicrepancia, bool chkFis, List<string> piecesIds, bool reasignar, bool escaneoCompleto, int turnadoParcialOAsignacion, bool validarReferencia, int avisarPrevioOConsolidado)
//        {
//            TurnadoImpoR RespuestaImpo = new TurnadoImpoR();
//            string MensajeCompuesto = string.Empty;
//            bool solicitarAlmacen = false;
//            try
//            {
//                reasignar = GetAutoFunctionalityValue(reasignar, IdOficina);

//                int IDGrupodeTrabajo = IDGrupodeTrabajoSelected;
//                int vidCliente = 0;
//                int vidMasterConsol = 0;
//                int IDReferencia;
//                string GuiaHouseOriginal;

//                var guiaParser = new Guia2dParser(_configuration);
//                var guia2Data = new Guia2dData();

//                var oficinaData = new CatalogoDeOficinasRepository(_configuration);
//                var oficinaObj = oficinaData.Buscar(IdOficina);

//                var objPieceIData = new PieceIdRepository(_configuration);
//                var registrarPieceId = false;
//                var PieceIdIndividual = "";

//                if (IDDatosDeEmpresa == 1)
//                {
//                    var funcionalidadesData = new ActivarFuncionalidadesRepository(_configuration);
//                    var funcionalidades = funcionalidadesData.BuscarPorOficina("ESCANEO POR PIECE ID", IdOficina);
//                    if (funcionalidades != null)
//                    {
//                        if (funcionalidades.Activo)
//                        {
//                            var pieceIdData = new PieceIdRepository(_configuration);
//                            var pieceObj = pieceIdData.BuscarReferenciaPorPieceId(GuiaHouse);

//                            registrarPieceId = true;
//                            pieceIdData.ValidaPieceIdDuplicado(pieceObj.PieceID);
//                            PieceIdIndividual = pieceObj.PieceID;
//                            GuiaHouse = pieceObj.NumeroDeGuia;
//                        }
//                    }



//                    GuiaHouse = Strings.Mid(GuiaHouse.Trim(), Strings.Len(GuiaHouse.Trim()) - 9, 10);

//                    Validaciones objVal = new Validaciones();
//                    if (objVal.ValidaGuias(GuiaHouse.Trim(), "H") == false)
//                    {
//                        RespuestaImpo.TipoDeRespuesta = TipoDeRespuesta.TR_Error;
//                        RespuestaImpo.Mensaje = "El número de referencia no es válido para DHL";
//                        return RespuestaImpo;
//                    }
//                }

//                GuiaHouseOriginal = GuiaHouse;

//                if (IDDatosDeEmpresa == 2)
//                {
//                    if (Guia2d != "")
//                    {
//                        guia2Data = guiaParser.getData(Guia2d);
//                        try
//                        {
//                            var guiaHouse2d = new CatalogoDeUsuarioData2();
//                            guiaHouse2d.InsertGuia2d(GuiaHouse, guia2Data.GuiaMaster, Guia2d, guia2Data.Numero, guia2Data.Peso, guia2Data.Descripcion, guia2Data.Valor, guia2Data.ClavePaisDestino, guia2Data.ClientName, guia2Data.Address);
//                        }

//                        catch (Exception ex)
//                        {
//                        }

//                        if (!IsNothing(guia2Data.GuiaMaster))
//                            GuiaHouse = guia2Data.GuiaMaster;
//                    }

//                    CustomsAlertsBabys objCustomAlertsBAbyD = new CustomsAlertsBabys();


//                    CustomsAlerts objCustomAlertsBabyExiste = new CustomsAlerts();
//                    string GuiaBabyHouse = "";
//                    objCustomAlertsBabyExiste = objCustomAlertsBAbyD.BuscarPorGuiaBaby(GuiaHouse.Trim());
//                    if (!IsNothing(objCustomAlertsBabyExiste))
//                    {
//                        if (!IsNothing(objCustomAlertsBabyExiste.GuiaMaster))
//                            GuiaHouse = objCustomAlertsBabyExiste.GuiaMaster;
//                    }

//                    if (Strings.Len(GuiaHouse.Trim()) == 10)
//                    {
//                        if (ValidaGuias(GuiaHouse.Trim(), "H") == true)
//                            throw new ArgumentException("La guia que esta procesando no es de FedEx");
//                    }
//                    CustomsAlerts objCA = new CustomsAlerts();
//                    CustomsAlertsRepository objCAD = new CustomsAlertsRepository(_configuration);
//                    objCA = objCAD.BuscarPorGuiaHouse(GuiaHouse.Trim(), IDDatosDeEmpresa);
//                    if (IsNothing(objCA))
//                    {
//                        // If MessageBox.Show("El número de guia no se encuentra manifestado, ¿desea continuar?", "Alerta", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
//                        if (validaNoManisfestado == true)
//                        {
//                            BitacoraGeneral objBit = new BitacoraGeneral();
//                            BitacoraGeneralRepository objBitD = new BitacoraGeneralRepository(_configuration);

//                            objBit.Descripcion = "Guia House sin manifiesto :" + GuiaHouse.ToUpper();
//                            objBit.IdUsuario = IdUsuario;
//                            objBit.IdReferencia = 0;
//                            objBit.Modulo = "frmTurnadodeGuias";

//                            objBitD.Insertar(objBit);
//                        }
//                        else
//                        {
//                            RespuestaImpo.AvisarNoManifestado = true;
//                            RespuestaImpo.Mensaje = "El número de guía no se encuentra manifestado, ¿desea continuar?";
//                            return RespuestaImpo;
//                        }
//                    }

//                    if (oficinaObj.OperacionDefault == 1)
//                    {
//                        CatalogoDeClientesExpertti objCliente = new CatalogoDeClientesExpertti();
//                        CatalogoDeClientesExperttiRepository objClienteD = new CatalogoDeClientesExperttiRepository(_configuration);
//                        objCliente = objClienteD.Buscar(objCA.IdCliente);
//                        if (!IsNothing(objCliente))
//                        {
//                            CatalogodeEncargoConferidoRepository objEncargoConferidoD = new CatalogodeEncargoConferidoRepository(_configuration);
//                            if (objEncargoConferidoD.ExisteEncargo(objCA.IdCliente, oficinaObj.PatenteDefault) == false)
//                            {
//                                // If MessageBox.Show("No existe encargo conferido para el cliente " & objCliente.Nombre.Trim()& ", ¿desea continuar?", "Alerta", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.No Then
//                                // Exit Sub
//                                // End If

//                                if (validaEncargoCliente == false)
//                                {
//                                    RespuestaImpo.AvisarEncargoCliente = true;
//                                    RespuestaImpo.Mensaje = "No existe encargo conferido para el cliente " + objCliente.Nombre.Trim() + ", ¿desea continuar?";
//                                    return RespuestaImpo;
//                                }
//                            }
//                        }
//                    }
//                }

//                IDGrupodeTrabajo = IDGrupodeTrabajoSelected;

//                GruposdeTrabajoRepository objGrupoD = new GruposdeTrabajoRepository(_configuration);

//                GuiasQueNoseDebenLiberar objGuiasqueNoseDebenLiberar = new GuiasQueNoseDebenLiberar();
//                GuiasqueNoseDebenLiberarRepository objGuiasqueNoseDebenLiberarD = new GuiasqueNoseDebenLiberarRepository(_configuration);

//                objGuiasqueNoseDebenLiberar = objGuiasqueNoseDebenLiberarD.Buscar(GuiaHouse);

//                if (!IsNothing(objGuiasqueNoseDebenLiberar))
//                {
//                    if (objGuiasqueNoseDebenLiberar.TodaslasAreas == true)
//                        throw new Exception("Esta guía no debe ser liberada, por ninguna Area :" + objGuiasqueNoseDebenLiberar.Motivo.Trim());
//                    else if (objGuiasqueNoseDebenLiberar.IDDepartamento == 9)
//                        throw new Exception("Esta guía No debe ser liberada, por el departamento de Clasificación: " + objGuiasqueNoseDebenLiberar.Motivo.Trim());
//                }


//                DetalleDeRelacionEnTransito objTransito = new DetalleDeRelacionEnTransito();
//                EnvioEnTransitoRepository objTransitoD = new EnvioEnTransitoRepository(_configuration);
//                objTransito = objTransitoD.Buscar(GuiaHouse);

//                if (!IsNothing(objTransito))
//                {
//                    if (objTransito.IdUsuarioScaneaLlegada == 0)
//                        throw new ArgumentException("Se detectó guía " + GuiaHouse + " en tránsito, por favor ingrese el escaneo de la llegada primero.");
//                }




//                int IdCategoria;
//                double valorEnDolares;
//                // Tomar decisiones en base al customalerts

//                CustomsAlertsRepository objCustomsAlertsD = new CustomsAlertsRepository(_configuration);

//                CustomerMasterFile objCMF = new CustomerMasterFile();
//                CustomerMasterFileRepository objCMFD = new CustomerMasterFileRepository(_configuration);
//                objCMF = objCMFD.Buscar(GuiaHouse);

//                CustomsAlerts objCustomsAlerts = new CustomsAlerts();
//                objCustomsAlerts = objCustomsAlertsD.BuscarPorGuiaHouse(GuiaHouse, IDDatosDeEmpresa);

//                string GuiaMasterCMF = string.Empty;
//                CatalogodeMaster objMasterCMF = new CatalogodeMaster();
//                CatalogodeMasterRepository objMasterD = new CatalogodeMasterRepository(_configuration);

//                bool validaPieceIds = false;
//                int numberPieceIds = 0;
//                bool multiPieza = false;
//                bool piezaindividual = false;

//                if (IsNothing(objCMF))
//                {
//                    if (!IsNothing(objCustomsAlerts))
//                    {
//                        vidCliente = objCustomsAlerts.IdCliente;
//                        IdCategoria = objCustomsAlerts.IdCategoria;
//                        valorEnDolares = objCustomsAlerts.ValorEnDolares;

//                        GuiaMasterCMF = Mid(objCustomsAlerts.GuiaMaster.Trim, 1, 3) + Mid(objCustomsAlerts.GuiaMaster.Trim, 4, 8);
//                        if (IDDatosDeEmpresa == 1)
//                        {
//                            if (objCustomsAlerts.Piezas > 1)
//                            {
//                                numberPieceIds = objCustomsAlerts.Piezas;
//                                validaPieceIds = true;
//                                multiPieza = true;
//                            }
//                            else if (objCustomsAlerts.Piezas == 1)
//                                piezaindividual = true;
//                        }

//                        objMasterCMF = objMasterD.Buscar(GuiaMasterCMF);

//                        if (!IsNothing(objTransito))
//                        {
//                            if (IdOficina != objTransito.IdOficinaLlegada)
//                            {
//                                var oficinaTemp = oficinaData.Buscar(objTransito.IdOficinaLlegada);
//                                throw new ArgumentException("La oficina de llegada de transito (" + oficinaTemp.nombre + ") no coincide con la oficina del usuario (" + oficinaObj.nombre + "), favor de verificar");
//                            }
//                        }
//                        else if (!IsNothing(objMasterCMF))
//                        {
//                            if (objMasterCMF.IdOficina != IdOficina & multiPieza == false)
//                            {
//                                var oficinaTemp = oficinaData.Buscar(objMasterCMF.IdOficina);
//                                throw new ArgumentException("La oficina de la master (" + oficinaTemp.nombre + ") no coincide con la oficina del usuario (" + oficinaObj.nombre + "), favor de verificar");
//                            }
//                        }
//                    }
//                    else
//                    {
//                        vidCliente = 17169;
//                        IdCategoria = 0;
//                    }
//                }
//                else
//                {
//                    vidCliente = objCMF.IdCliente;
//                    IdCategoria = objCMF.IdCategoria;
//                    valorEnDolares = objCMF.ValorDolares;

//                    GuiaMasterCMF = Mid(objCMF.GuiaMaster.Trim, 1, 3) + Mid(objCMF.GuiaMaster.Trim, 4, 8);

//                    if (IDDatosDeEmpresa == 1)
//                    {
//                        if (objCMF.Piezas > 1)
//                        {
//                            numberPieceIds = objCMF.Piezas;
//                            validaPieceIds = true;
//                            multiPieza = true;
//                        }
//                        else if (objCMF.Piezas == 1)
//                            piezaindividual = true;
//                    }

//                    // ivbm 20-01-2023                
//                    objMasterCMF = objMasterD.Buscar(GuiaMasterCMF);

//                    if (!IsNothing(objTransito))
//                    {
//                        if (IdOficina != objTransito.IdOficinaLlegada)
//                            throw new ArgumentException("La oficina de la de llegada de transito no coincide con la oficina del usuario, favor de verificar");
//                    }
//                    else if (!IsNothing(objMasterCMF))
//                    {
//                        if (objMasterCMF.IdOficina != IdOficina & multiPieza == false)
//                            throw new ArgumentException("La oficina de la master no coincide con la oficina del usuario, favor de verificar");
//                    }
//                }

//                if (IdCategoria == 9)
//                    throw new ArgumentException("La referencia " + GuiaHouse + " tiene categoría HOSPITAL, favor de verificar");

//                Referencias objReferencia1 = new Referencias();
//                ReferenciasRepository objReferenciaD1 = new ReferenciasRepository();
//                if (IDDatosDeEmpresa == 1)
//                    objReferencia1 = objReferenciaD1.BuscarUltimaReferencia(GuiaHouse, IDDatosDeEmpresa);
//                else
//                    objReferencia1 = objReferenciaD1.Buscar(GuiaHouse, IDDatosDeEmpresa);


//                ControldeEventosRepository controldeEventosData = new ControldeEventosRepository(_configuration);

//                Referencias objReferencia1New = null/* TODO Change to default(_) if this is not a reference type */;
//                if (validarReferencia)
//                {
//                    if (!IsNothing(objReferencia1))
//                    {
//                        SaaioPedime objPedime = new SaaioPedime();
//                        SaaioPedimeData objPedimeD = new SaaioPedimeData();
//                        ReferenciasRepository objReferenciaData = new ReferenciasRepository();

//                        objPedime = objPedimeD.Buscar(objReferencia1.NumeroDeReferencia.Trim);


//                        if (!IsNothing(objPedime))
//                        {
//                            if (!IsNothing(objPedime.FEC_PAGO))
//                            {
//                                DateTime fechaPago = Convert.ToDateTime(objPedime.FEC_PAGO);
//                                var fechaActual = DateTime.Now();

//                                int DifMeses = DateDiff("m", fechaPago, fechaActual);

//                                if (DifMeses >= 2)
//                                    // GUIA DUPLIACADA
//                                    objReferencia1New = CrearReferenciaParaOficinaCMF(GuiaHouse, IDDatosDeEmpresa, vidCliente, IdOficina, objReferencia1, objReferenciaD1, IdUsuario, IDGrupodeTrabajo, chkbSubdivision, "D");
//                            }
//                        }

//                        if (IsNothing(objReferencia1New))
//                        {
//                            if (!IsNothing(objReferencia1.FechaApertura))
//                            {
//                                DateTime fechaApertura = Convert.ToDateTime(objReferencia1.FechaApertura);
//                                var fechaActual = DateTime.Now();

//                                int DifMeses = DateDiff("m", fechaApertura, fechaActual);

//                                if (DifMeses >= 2)
//                                    // GUIA DUPLIACADA
//                                    objReferencia1New = CrearReferenciaParaOficinaCMF(GuiaHouse, IDDatosDeEmpresa, vidCliente, IdOficina, objReferencia1, objReferenciaD1, IdUsuario, IDGrupodeTrabajo, chkbSubdivision, "D");
//                            }
//                        }

//                        if (IsNothing(objReferencia1New))
//                        {

//                            // Si no ha arribado, cambiamos oficina
//                            List<int> listPre2 = new List<int>();
//                            listPre2.Add(644); // GUIA TURNADA A ASIGNACIONES
//                            ControldeEventosRepository objEventoD2 = new ControldeEventosRepository(_configuration);
//                            Checkpoint checkpointData2 = new Checkpoint();
//                            if (!objEventoD2.ExistePrecedencia(objReferencia1.IDReferencia, listPre2))
//                            {
//                                CatalogoDeOficinas objOficina = new CatalogoDeOficinas();
//                                CatalogoDeOficinasData objOficinaD = new CatalogoDeOficinasData();

//                                objOficina = objOficinaD.Buscar(IdOficina);
//                                objReferencia1.IdOficina = IdOficina;
//                                objReferencia1.AduanaDespacho = objOficina.AduDesp;
//                                objReferencia1.AduanaEntrada = objOficina.AduEntr;

//                                objReferenciaD1.Modificar(objReferencia1);
//                            }

//                            if (IdOficina != objReferencia1.IdOficina)
//                            {
//                                if (multiPieza)
//                                {
//                                    PieceIData pieceIDData = new PieceIData();
//                                    var numberPieceIdsTotal = pieceIDData.GetNumeroPieceIdsTotal(GuiaHouse);
//                                    if (numberPieceIdsTotal <= numberPieceIds)
//                                    {
//                                        // Multipieza
//                                        var prefijoOficina = "D";
//                                        if (IdOficina == 2 | IdOficina == 24)
//                                            prefijoOficina = "S";
//                                        objReferencia1New = CrearReferenciaParaOficinaCMF(GuiaHouse, IDDatosDeEmpresa, vidCliente, IdOficina, objReferencia1, objReferenciaD1, IdUsuario, IDGrupodeTrabajo, chkbSubdivision, prefijoOficina);
//                                    }
//                                }
//                            }
//                        }

//                        if (IsNothing(objReferencia1New))
//                        {
//                            var departamentoActual = controldeEventosData.BuscaDepartamentoActual(objReferencia1.IDReferencia, IdOficina);

//                            if (!departamentoActual == 77 & !departamentoActual == 0 & !departamentoActual == 51 & !departamentoActual == 9 & !departamentoActual == 54 & !departamentoActual == 72)
//                            {
//                                // NO ESTA EN
//                                // 77 ASIGNACIONES
//                                // 0 NINGUN DEPARTAMENTO
//                                // 51 NOTI SIN PREVIO
//                                // 9 CLASI
//                                // 9 AGENTE ADUANAL
//                                if (multiPieza)
//                                    objReferencia1New = CrearReferenciaParaOficinaCMF(GuiaHouse, IDDatosDeEmpresa, vidCliente, IdOficina, objReferencia1, objReferenciaD1, IdUsuario, IDGrupodeTrabajo, chkbSubdivision, "S");
//                            }
//                        }
//                    }
//                    else
//                        objReferencia1New = CrearReferenciaParaOficinaCMF(GuiaHouse, IDDatosDeEmpresa, vidCliente, IdOficina, objReferencia1, objReferenciaD1, IdUsuario, IDGrupodeTrabajo, chkbSubdivision, "S");
//                    RespuestaImpo.ReferenciaValidada = true;
//                }

//                if (!IsNothing(objReferencia1New))
//                    objReferencia1 = objReferencia1New;
//                IDReferencia = objReferencia1.IDReferencia;
//                GuiaHouse = objReferencia1.NumeroDeReferencia;

//                ControldeEventosRepository objEventoD = new ControldeEventosRepository(_configuration);
//                var departamentoPrevio = controldeEventosData.BuscaDepartamentoActual(objReferencia1.IDReferencia, IdOficina);


//                // Buscar CB entonces cambiar categoria 3
//                List<int> listPre = new List<int>();
//                listPre.Add(590); // Global turna a Formal CB
//                Checkpoint checkpointData = new Checkpoint();
//                if (objEventoD.ExistePrecedencia(objReferencia1.IDReferencia, listPre))
//                {
//                    objCustomsAlertsD.ModificarporIDCategoria3(Strings.Mid(GuiaHouse, Strings.Len(GuiaHouse) - 9, 10));
//                    IdCategoria = 3;
//                }


//                // __________FAST MORNING__________________
//                if (!IsNothing(objReferencia1))
//                {
//                    IDReferencia = objReferencia1.IDReferencia;

//                    if (objReferencia1.IDDatosDeEmpresa == 1)
//                    {
//                        CatalogoDeOficinas objOficina = new CatalogoDeOficinas();
//                        CatalogoDeOficinasData objOficinaD = new CatalogoDeOficinasData();

//                        if (!IsNothing(objTransito))
//                        {
//                            objOficina = objOficinaD.Buscar(objTransito.IdOficinaLlegada);

//                            if (IsNothing(objOficina))
//                                throw new ArgumentException("La oficina de la llegada de transito no es correcta, favor de verificar antes de seguir con el proceso");

//                            if (!IsNothing(objMasterCMF))
//                                objReferencia1.IDMasterConsol = objMasterCMF.IDMasterConsol;

//                            if (objReferencia1.IdOficina != objTransito.IdOficinaLlegada)
//                            {
//                                // TODO REGENERAR EL ARCHIVO DE PROFORMA
//                                NubePrepago nube = new NubePrepago();
//                                nube.RegenerarProformaInsertar(objReferencia1.IDReferencia, IdUsuario, DateTime.Now.ToString());
//                            }

//                            objReferencia1.IdOficina = objTransito.IdOficinaLlegada;
//                            objReferencia1.AduanaDespacho = objOficina.AduDesp;
//                            // objReferencia1.AduanaEntrada = objOficina.AduEntr

//                            objReferenciaD1.Modificar(objReferencia1);

//                            SaaioPedime objPedime = new SaaioPedime();
//                            SaaioPedimeData objPedimeD = new SaaioPedimeData();
//                            objPedime = objPedimeD.Buscar(objReferencia1.NumeroDeReferencia);
//                            if (!IsNothing(objPedime))
//                            {
//                                objPedime.MTR_ENTR = "4";
//                                objPedime.MTR_ARRI = "7";
//                                objPedime.MTR_SALI = "7";
//                                objPedime.ADU_DESP = objOficina.AduDesp;
//                                // objPedime.ADU_ENTR = objOficina.AduEntr
//                                objPedime.CVE_REPR = objOficina.CveMant;
//                                objPedimeD.Modificar(objPedime);
//                            }
//                        }
//                        else if (!IsNothing(objMasterCMF))
//                        {
//                            objOficina = objOficinaD.Buscar(objMasterCMF.IdOficina);
//                            if (IsNothing(objOficina))
//                                throw new ArgumentException("La oficina de la master no es correcta, favor de verificar antes de seguir con el proceso");

//                            if (multiPieza == false)
//                            {
//                                if (objReferencia1.IdOficina != objMasterCMF.IdOficina)
//                                {
//                                    // TODO REGENERAR EL ARCHIVO DE PROFORMA
//                                    NubePrepago nube = new NubePrepago();
//                                    nube.RegenerarProformaInsertar(objReferencia1.IDReferencia, IdUsuario, DateTime.Now.ToString());
//                                }

//                                objReferencia1.IDMasterConsol = objMasterCMF.IDMasterConsol;
//                                objReferencia1.IdOficina = objMasterCMF.IdOficina;
//                                objReferencia1.AduanaDespacho = objOficina.AduDesp;
//                                objReferencia1.AduanaEntrada = objOficina.AduEntr;

//                                objReferenciaD1.Modificar(objReferencia1);

//                                SaaioPedime objPedime = new SaaioPedime();
//                                SaaioPedimeData objPedimeD = new SaaioPedimeData();
//                                objPedime = objPedimeD.Buscar(objReferencia1.NumeroDeReferencia);
//                                if (!IsNothing(objPedime))
//                                {
//                                    objPedime.ADU_DESP = objOficina.AduDesp;
//                                    objPedime.ADU_ENTR = objOficina.AduEntr;
//                                    objPedimeD.Modificar(objPedime);
//                                }
//                            }
//                        }
//                    }
//                    // ---------------IVBM 24-01-2023

//                    if (chkFis == true)
//                    {
//                        objReferenciaD1.ModificarEscaladas(GuiaHouse, 6);
//                        chkFis = false;
//                    }


//                    int departamentoActual = 0;
//                    int departamentoDestino = 0;

//                    // 'GUIA PRECAPTURADA PARA VALIDACIÓN Y PAGO'
//                    if (objEventoD.BuscaSiExisteIDCheckpoint(524, objReferencia1.IDReferencia))
//                    {
//                        // 'CLASIFICACIÓN DETECTA INCONSISTENCIA EN MERCANCÍA SE CANCELA PRECAPTURA'
//                        if (objEventoD.BuscaSiExisteIDCheckpoint(525, objReferencia1.IDReferencia) == false)
//                        {
//                            // Clasificacion calzado chino
//                            if (IsNothing(objCMF))
//                            {
//                                if (objCustomsAlerts.IdTipodePedimento == 21)
//                                    throw new Exception($"La referencia {objReferencia1.NumeroDeReferencia} corresponde a un pedimento de CALZADO, posiblemente de origen chino.");
//                            }
//                            else if (objCMF.idTipodePedimento == 21)
//                                throw new Exception($"La referencia {objReferencia1.NumeroDeReferencia} corresponde a un pedimento de CALZADO, posiblemente de origen chino.");

//                            if (validaNubePrepago == 0)
//                            {
//                                RespuestaImpo.AvisarNubePrepago = true;
//                                RespuestaImpo.Mensaje = "¿Se detectó guía tipo Nube prepago quieres procesar?";
//                                return RespuestaImpo;
//                            }

//                            if (validaNubePrepago == 1)
//                            {
//                                var nubePrepago = new NubePrepago();

//                                CatalogodeMaster objGuiaMaster;

//                                // VALIDACIONES

//                                // VALIDAR QUE NO ESTE EN OTRO DEPARTAMENTO

//                                if (departamentoActual == 0)
//                                    departamentoActual = IdDepartamento;
//                                if (departamentoActual == 54)
//                                    Precaptura(objReferencia1, IdUsuario, IdOficina, IDDatosDeEmpresa, 0);

//                                if (departamentoActual > 0)
//                                {
//                                    CatalogodeCheckPoints objCatCheck = new CatalogodeCheckPoints();
//                                    CatalogodeCheckPointsData objCatCheckD = new CatalogodeCheckPointsData();
//                                    objCatCheck = objCatCheckD.BuscarPorDepto(526, IdOficina, departamentoActual);

//                                    CatalogoDepartamentos objDep = new CatalogoDepartamentos();
//                                    CatalogoDepartamentosData objDepD = new CatalogoDepartamentosData();
//                                    objDep = objDepD.Buscar(departamentoActual);

//                                    if (IsNothing(objCatCheck))
//                                        throw new Exception("La referencia está asignada al departamento " + Constants.vbCrLf + objDep.NombreDepartamento.Trim()+ " y el checkpoint es de otro departamento");
//                                }

//                                // VALIDACION DE GUIA DUPLICADA Fecha no exceda los dos meses
//                                if (!IsNothing(objCMF))
//                                {
//                                    var mesesDiff = DateDiff(DateInterval.Month, objCMF.FechaAlta, DateTime.Now);
//                                    if (mesesDiff > 2)
//                                        throw new ArgumentException("Guía con más de dos meses de existencia, revisar si se trata de una guía duplicada.");
//                                }
//                                if (!IsNothing(objCustomsAlerts))
//                                {
//                                    var mesesDiff = DateDiff(DateInterval.Month, objCustomsAlerts.FechaDeAlta, DateTime.Now);
//                                    if (mesesDiff > 2)
//                                        throw new ArgumentException("Guía con más de dos meses de existencia, revisar si se trata de una guía duplicada.");
//                                }

//                                SaaioPedime objPedimeFastMorning = new SaaioPedime();
//                                SaaioPedimeData objPedimeDFastMorning = new SaaioPedimeData();
//                                objPedimeFastMorning = objPedimeDFastMorning.Buscar(objReferencia1.NumeroDeReferencia);

//                                if (!IsNothing(objPedimeFastMorning))
//                                {
//                                    if (objPedimeFastMorning.FIR_ELEC != "")
//                                        throw new ArgumentException("Referencia " + objReferencia1.NumeroDeReferencia + " ya tiene firma electrónica.");
//                                }

//                                // MASTER 
//                                var saaoi_guia = new SaaioGuiasData();

//                                var guiaMaster = saaoi_guia.CargarMaster(objReferencia1.NumeroDeReferencia);

//                                // VALIDA MISMA MASTER
//                                if (!IsNothing(guiaMaster))
//                                {
//                                    if (guiaMaster.Count > 1)
//                                    {
//                                        var GuiaGuion = guiaMaster.Find(p => p.GUIA.Contains("-"));
//                                        if (!IsNothing(GuiaGuion))
//                                        {
//                                            var GuiaSin = GuiaGuion.GUIA.Replace("-", "");
//                                            foreach (SaaioGuias item in guiaMaster)
//                                            {
//                                                if (item.GUIA == GuiaSin)
//                                                    saaoi_guia.EliminarGuia(item.NUM_REFE, item.GUIA, item.IDE_MH);
//                                            }
//                                        }
//                                    }
//                                }

//                                var existeMasterEnWec = false;
//                                WebApiWec objws = new WebApiWec();
//                                var obsCheck = "";

//                                var resultSTD;
//                                if (objReferencia1.IdOficina == 2)
//                                    resultSTD = await objws.ConsultasWec(objReferencia1.NumeroDeReferencia, IdUsuario);


//                                try
//                                {
//                                    if (!Information.IsNothing(resultSTD))
//                                    {
//                                        if (!Information.IsNothing(resultSTD.GuiaMaster))
//                                        {
//                                            existeMasterEnWec = true;

//                                            var existeMasterEnBD = false;

//                                            if (resultSTD.Bultos == 1)
//                                            {
//                                                if (!IsNothing(guiaMaster))
//                                                {
//                                                    foreach (SaaioGuias item in guiaMaster)
//                                                    {
//                                                        var GuiaSin = item.GUIA.Replace("-", "");
//                                                        if ((item.GUIA == resultSTD.GuiaMaster || GuiaSin == resultSTD.GuiaMaster))
//                                                            existeMasterEnBD = true;
//                                                        else
//                                                            saaoi_guia.EliminarGuia(item.NUM_REFE, item.GUIA, item.IDE_MH);
//                                                    }
//                                                }
//                                            }

//                                            if (existeMasterEnBD == false)
//                                                CrearMasterPorWEC(resultSTD, objReferencia1.NumeroDeReferencia);
//                                        }
//                                    }
//                                    else if (objReferencia1.IdOficina == 2)
//                                        obsCheck = "SIN RESPUESTA DE WEC PARA SABER CR";



//                                    if (existeMasterEnWec == false)
//                                    {
//                                        // CREAMOS MASTER APARTIR DE CUSTOM ALERT
//                                        if (!IsNothing(objCustomsAlerts))
//                                        {
//                                            var existeMasterEnBD = false;

//                                            if (objCustomsAlerts.Piezas == 1)
//                                            {
//                                                if (!IsNothing(guiaMaster))
//                                                {
//                                                    foreach (SaaioGuias item in guiaMaster)
//                                                    {
//                                                        var GuiaSin = item.GUIA.Replace("-", "");
//                                                        if ((item.GUIA == objCustomsAlerts.GuiaMaster || GuiaSin == objCustomsAlerts.GuiaMaster))
//                                                            existeMasterEnBD = true;
//                                                        else
//                                                            saaoi_guia.EliminarGuia(item.NUM_REFE, item.GUIA, item.IDE_MH);
//                                                    }
//                                                }
//                                            }

//                                            if (existeMasterEnBD == false)
//                                                CrearMasterPorCustomAlert(objCustomsAlerts, objReferencia1.NumeroDeReferencia);
//                                        }
//                                    }
//                                }

//                                catch (Exception ex)
//                                {
//                                }

//                                guiaMaster = saaoi_guia.CargarMaster(objReferencia1.NumeroDeReferencia);

//                                if (!IsNothing(guiaMaster))
//                                {
//                                    if (guiaMaster.Count > 1)
//                                        // TODO INCORPORAR PANTALLA DE SELECCION DE MASTER
//                                        throw new ArgumentException("Existe mas de una master declarada, por favor procesar desde Expertti.");
//                                }
//                                else
//                                    // TODO INCORPORAR PANTALLA DE CREACION DE MASTER
//                                    throw new ArgumentException("No se detecto guía master, por favor procesar desde Expertti.");


//                                // ASIGNAMOS MASTER SI NO TIENE
//                                if (objReferencia1.IDMasterConsol == 0)
//                                {
//                                    CatalogodeMasterData objGuiaMasterD = new CatalogodeMasterData();
//                                    if (!IsNothing(objCustomsAlerts))
//                                        objGuiaMaster = objGuiaMasterD.Buscar(objCustomsAlerts.GuiaMaster.Trim);
//                                    else if (!IsNothing(guiaMaster))
//                                        objGuiaMaster = objGuiaMasterD.Buscar(guiaMaster.First.GUIA.Trim);


//                                    if (!IsNothing(objGuiaMaster))
//                                    {
//                                        objReferencia1.IDMasterConsol = objGuiaMaster.IDMasterConsol;
//                                        objReferenciaD1.Modificar(objReferencia1);
//                                    }
//                                }


//                                var guiaHouseDos = saaoi_guia.BuscarGuia(objReferencia1.NumeroDeReferencia, "H");

//                                if (IsNothing(guiaHouseDos))
//                                    // TODO INCORPORAR PANTALLA DE CORRECION DE GUIA HOUSE
//                                    throw new ArgumentException("No se detecto guía house, por favor procesar desde Expertti.");


//                                if (objReferencia1.IDMasterConsol == 0)
//                                    // TODO - Pantalla para agregar master a mano.
//                                    throw new ArgumentException("Se debe asignar una master a la referencia, por favor procesar desde Expertti.");
//                                else
//                                    objGuiaMaster = objMasterD.Buscar(objReferencia1.IDMasterConsol);

//                                var envioCOVE = false;

//                                if (!IsNothing(objPedimeFastMorning))
//                                {
//                                    if (!IsNothing(objGuiaMaster))
//                                    {
//                                        if (objPedimeFastMorning.FEC_ENTR != objGuiaMaster.FechaArriboUnitarias)
//                                        {
//                                            // Actualizamos fecha de entrada y tipo de cambio en saio pedime
//                                            CtarcTipCam objTipodeCambio = new CtarcTipCam();
//                                            CtarcTipCamData objTipodeCambioD = new CtarcTipCamData();
//                                            objTipodeCambio = objTipodeCambioD.Buscar(objGuiaMaster.FechaArriboUnitarias);
//                                            if (!IsNothing(objTipodeCambio))
//                                            {
//                                                if (objTipodeCambio.TIP_CAM == null/* TODO Change to default(_) if this is not a reference type */ )
//                                                    // TODO - Mostrar alerta.
//                                                    // MessageBox.Show("El tipo de cambio no se ha dado de alta en Expertti")
//                                                    throw new ArgumentException("El tipo de cambio no se ha dado de alta en Expertti");
//                                                else
//                                                    objPedimeFastMorning.TIP_CAMB = objTipodeCambio.TIP_CAM;
//                                            }

//                                            objPedimeFastMorning.FEC_ENTR = objGuiaMaster.FechaArriboUnitarias;
//                                            objPedimeDFastMorning.Modificar(objPedimeFastMorning);

//                                            // Eliminar COVE
//                                            EliminarCove(objReferencia1, ObjUsuario);

//                                            // Actualizar equivaliencias  STORE DE DANIEL
//                                            SaaioCoveSerExperttiData saaioCoveSerExperttiData = new SaaioCoveSerExperttiData();
//                                            saaioCoveSerExperttiData.ActualizaFechaEntrada(objReferencia1.NumeroDeReferencia, objPedimeFastMorning.FEC_ENTR);

//                                            envioCOVE = true;
//                                        }
//                                    }
//                                }



//                                SaaioIdePed objSaaioIdePed = new SaaioIdePed();
//                                SaaioIdePedData objSaaioIdePedD = new SaaioIdePedData();
//                                objSaaioIdePed = objSaaioIdePedD.Buscar(objReferencia1.NumeroDeReferencia, "CR");

//                                if (IsNothing(objSaaioIdePed))
//                                {
//                                    try
//                                    {
//                                        SaaioIdePed objIdePed = new SaaioIdePed();
//                                        objIdePed.NUM_REFE = objReferencia1.NumeroDeReferencia;
//                                        objIdePed.CVE_IDEN = "CR";
//                                        if (!Information.IsNothing(resultSTD))
//                                        {
//                                            if (!Information.IsNothing(resultSTD.GuiaMaster))
//                                                objIdePed.COM_IDEN = "263";
//                                            else
//                                                objIdePed.COM_IDEN = "12";
//                                        }
//                                        else
//                                        {
//                                            if (objReferencia1.IdOficina == 2)
//                                                objIdePed.COM_IDEN = "12";
//                                            if (objReferencia1.IdOficina == 24)
//                                                objIdePed.COM_IDEN = "296";
//                                        }
//                                        objSaaioIdePedD.Insertar(objIdePed);
//                                    }
//                                    catch (Exception ex)
//                                    {
//                                        // MessageBox.Show(ex.Message)
//                                        throw new ArgumentException("No se pudo generar identificador CR: " + ex.Message);
//                                    }
//                                }
//                                else
//                                    objSaaioIdePedD.Modificar(objReferencia1.NumeroDeReferencia, "CR", objSaaioIdePed.COM_IDEN);

//                                objSaaioIdePed = objSaaioIdePedD.Buscar(objReferencia1.NumeroDeReferencia, "CR");
//                                if (IsNothing(objSaaioIdePed))
//                                    throw new ArgumentException("No se encontró identificador CR");

//                                if (!IsNothing(objSaaioIdePed))
//                                {
//                                    DocumentosPorGuiaData objDocumentosPorGuiaD = new DocumentosPorGuiaData();

//                                    var sinonimosData = new SinonimosdeRiesgoData();
//                                    var sinonimoTextilCalzado = false;

//                                    if (!IsNothing(objCMF))
//                                        sinonimoTextilCalzado = sinonimosData.ExisteSinonimoDeRiesgoTextilesZapatos(objCMF.Descripcion);
//                                    else if (!IsNothing(objCustomsAlerts))
//                                        sinonimoTextilCalzado = sinonimosData.ExisteSinonimoDeRiesgoTextilesZapatos(objCustomsAlerts.Descripcion);




//                                    UbicacionDeArchivos objUbicacion = new UbicacionDeArchivos();
//                                    UbicacionDeArchivosData objUbicacionD = new UbicacionDeArchivosData();
//                                    objUbicacion = objUbicacionD.Buscar(121);
//                                    if (IsNothing(objUbicacion))
//                                        throw new ArgumentException("No existe ubicacion de archivos Id. 121, MisDOcumentos");

//                                    var pMisDocumentos = objUbicacion.Ubicacion + @"\" + ObjUsuario.Usuario.Trim()+ @"\ExperttiTmp\";

//                                    if (Directory.Exists(pMisDocumentos) == false)
//                                        Directory.CreateDirectory(Directory.Exists(pMisDocumentos));



//                                    if (sinonimoTextilCalzado)
//                                    {
//                                        // ENVIAR FACTURA COMERCIAL
//                                        if (Convert.ToBoolean(objDocumentosPorGuiaD.ExisteUltimoDocumentoSubido(IDReferencia, 14, 1)))
//                                        {
//                                            var documentoavucem = objDocumentosPorGuiaD.Buscar(IDReferencia, 14); // Ya me regresa el ultimo consecutivo

//                                            if (documentoavucem.idDocumento == 0)
//                                                throw new ArgumentException("No se encontro factura para enviar a VUCEM");


//                                            DigitalizadosVucem objDigitalizados = new DigitalizadosVucem();
//                                            DigitalizadosVucemData objDigitalizadosD = new DigitalizadosVucemData();

//                                            // Factura
//                                            objDigitalizados = objDigitalizadosD.Buscar(IDReferencia, 68);
//                                            if (IsNothing(objDigitalizados))
//                                            {
//                                                Thread thFactura = new Thread(() => nubePrepago.GeneraEDocument(documentoavucem.idDocumento, objReferencia1.IDReferencia, false, objReferencia1, ObjUsuario, pMisDocumentos));
//                                                thFactura.Start();
//                                            }
//                                        }
//                                    }

//                                    // CR 12 NO ESTAN EN WEC
//                                    // CR 263 ESTAN EN WEC

//                                    if (registrarPieceId & PieceIdIndividual != "")
//                                    {
//                                        string ultimosDiezDigitos;
//                                        ultimosDiezDigitos = GuiaHouse.Substring(GuiaHouse.Length - 10);
//                                        objPieceIData.ImpoInsertPieceID(ultimosDiezDigitos, PieceIdIndividual, IdOficina, IdUsuario, objReferencia1.IDReferencia);
//                                    }



//                                    if (objSaaioIdePed.COM_IDEN == "263")
//                                    {
//                                        DocumentosPorGuia objDocumentosPorGuia = new DocumentosPorGuia();
//                                        if (!Convert.ToBoolean(objDocumentosPorGuiaD.ExisteUltimoDocumentoSubido(IDReferencia, 1166, 1)))
//                                        {
//                                            var resultRevalidaWec = await nubePrepago.RevalidaWec(objReferencia1, pMisDocumentos, IdUsuario, IDDatosDeEmpresa, IdDepartamento, IdOficinaGP, ObjUsuario);
//                                            if (resultRevalidaWec == 0)
//                                            {
//                                                // Throw New ArgumentException("No fue posible revalidar intente de nuevo")

//                                                // ARRIBO DE FAST MORNING - INFORMATIVO
//                                                ControldeEventos objEventoCoSalida = new ControldeEventos(535, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                                var objRespuestaFastMorning = objEventoD.InsertarEvento(objEventoCoSalida, IdDepartamento, IdOficina, false, obsCheck, IDDatosDeEmpresa);

//                                                // GUIA ARRIBADA PARA SALIDAS'
//                                                ControldeEventos objEventoCoSalida2 = new ControldeEventos(527, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                                var objRespuestaSalidas = objEventoD.InsertarEvento(objEventoCoSalida2, IdDepartamento, IdOficina, false, "No fue posible revalidar la guia en WEC", IDDatosDeEmpresa);



//                                                RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "Turnada correctamente para " + Constants.vbCrLf + "SALIDAS";
//                                                RespuestaImpo.BackColor = 0;
//                                                return RespuestaImpo;
//                                            }
//                                        }


//                                        // 1047 - GUIA HOUSE SELLADA
//                                        // 1166 - GUIA HOUSE SELLADA WEC
//                                        if ((Convert.ToBoolean(objDocumentosPorGuiaD.ExisteUltimoDocumentoSubido(IDReferencia, 1166, 1))) | (Convert.ToBoolean(objDocumentosPorGuiaD.ExisteUltimoDocumentoSubido(IDReferencia, 1047, 1))))
//                                        {
//                                            var documentoavucem = objDocumentosPorGuiaD.Buscar(IDReferencia, 1166);
//                                            if (documentoavucem.idDocumento == 0)
//                                                documentoavucem = objDocumentosPorGuiaD.Buscar(IDReferencia, 1047);

//                                            if (documentoavucem.idDocumento == 0)
//                                            {
//                                                // Throw New ArgumentException("No se encontro documento para enviar a VUCEM")
//                                                // ARRIBO DE FAST MORNING - INFORMATIVO
//                                                ControldeEventos objEventoCoSalida2 = new ControldeEventos(535, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                                var objRespuestaFastMorning2 = objEventoD.InsertarEvento(objEventoCoSalida2, IdDepartamento, IdOficina, false, obsCheck, IDDatosDeEmpresa);

//                                                // GUIA ARRIBADA PARA SALIDAS'
//                                                ControldeEventos objEventoCoSalida3 = new ControldeEventos(527, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                                var objRespuestaSalidas = objEventoD.InsertarEvento(objEventoCoSalida3, IdDepartamento, IdOficina, false, "No se encontro documento GUIA HOUSE para enviar a VUCEM", IDDatosDeEmpresa);

//                                                RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "Turnada correctamente para " + Constants.vbCrLf + "SALIDAS";
//                                                RespuestaImpo.BackColor = 0;
//                                                return RespuestaImpo;
//                                            }


//                                            DigitalizadosVucem objDigitalizados = new DigitalizadosVucem();
//                                            DigitalizadosVucemData objDigitalizadosD = new DigitalizadosVucemData();

//                                            // ARRIBO DE FAST MORNING - INFORMATIVO
//                                            ControldeEventos objEventoCoSalida = new ControldeEventos(535, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                            var objRespuestaFastMorning = objEventoD.InsertarEvento(objEventoCoSalida, IdDepartamento, IdOficina, false, obsCheck, IDDatosDeEmpresa);

//                                            // Guia aerea 
//                                            objDigitalizados = objDigitalizadosD.Buscar(IDReferencia, 30);
//                                            // If IsNothing(objDigitalizados) Then
//                                            Thread thGuia = new Thread(() => nubePrepago.GeneraEDocumentAll(documentoavucem.idTipoDocumento, objReferencia1.IDReferencia, true, objReferencia1, ObjUsuario, pMisDocumentos, envioCOVE));
//                                            thGuia.Start();
//                                            // End If




//                                            RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "en espera de eDocument para " + Constants.vbCrLf + "CONTROL DE PAGO ELECTRONICO";
//                                            RespuestaImpo.BackColor = 0;
//                                            return RespuestaImpo;
//                                        }
//                                        else
//                                        {


//                                            // Throw New ArgumentException("No se encontro guía sellada")
//                                            // ARRIBO DE FAST MORNING - INFORMATIVO
//                                            ControldeEventos objEventoCoSalida2 = new ControldeEventos(535, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                            var objRespuestaFastMorning2 = objEventoD.InsertarEvento(objEventoCoSalida2, IdDepartamento, IdOficina, false, obsCheck, IDDatosDeEmpresa);

//                                            // GUIA ARRIBADA PARA SALIDAS'
//                                            ControldeEventos objEventoCoSalida3 = new ControldeEventos(527, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                            var objRespuestaSalidas = objEventoD.InsertarEvento(objEventoCoSalida3, IdDepartamento, IdOficina, false, "No se encontro guía sellada", IDDatosDeEmpresa);

//                                            RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "Turnada correctamente para " + Constants.vbCrLf + "SALIDAS";
//                                            RespuestaImpo.BackColor = 0;
//                                            return RespuestaImpo;
//                                        }
//                                    }
//                                    else
//                                    {
//                                        if (IsNothing(objGuiaMaster))
//                                            // TODO IMPLEMENTAR PANTALLA DE ALTA DE GUIA MASTER
//                                            throw new ArgumentException("No se encontro guía master, por favor procesar desde Expertti.");
//                                        if (objGuiaMaster.ImagenMasterizacion == false)
//                                            // TODO IMPLEMENTAR PANTALLA DE ALTA DE GUIA MASTER
//                                            throw new ArgumentException("No existe sello de master, por favor procesar desde Expertti.");
//                                        if (objGuiaMaster.ImagenRevalidacion == false)

//                                            // TODO IMPLEMENTAR PANTALLA DE ALTA DE GUIA MASTER
//                                            throw new ArgumentException("No existe sello de Revalidación, por favor procesar desde Expertti.");


//                                        // Dim objDocumentosPorGuiaD As New DocumentosPorGuiaData()
//                                        if (!Convert.ToBoolean(objDocumentosPorGuiaD.ExisteUltimoDocumentoSubido(IDReferencia, 1047, 1)))
//                                        {
//                                            try
//                                            {

//                                                // Dim objDocs As New VentanillaUnica.CentralizarDocs
//                                                bool x = false;
//                                                // x = Await (objDocs.sellarGuia(IDReferencia, 0, pMisDocumentos, GObjUsuario))
//                                                // NEW SELLAMOS CON TODAS LAS MASTER DE SAAI_GUIAS Y CATALOGOGODEMASTER
//                                                x = await (nubePrepago.sellarGuia(IDReferencia, 0, pMisDocumentos, ObjUsuarioGP));
//                                            }
//                                            catch (Exception ex)
//                                            {
//                                                // Throw New ArgumentException("No fue posible sellar la Guía " & ex.Message.Trim)
//                                                // ARRIBO DE FAST MORNING - INFORMATIVO
//                                                ControldeEventos objEventoCoSalida = new ControldeEventos(535, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                                var objRespuestaFastMorning = objEventoD.InsertarEvento(objEventoCoSalida, IdDepartamento, IdOficina, false, obsCheck, IDDatosDeEmpresa);

//                                                // GUIA ARRIBADA PARA SALIDAS'
//                                                ControldeEventos objEventoCoSalida2 = new ControldeEventos(527, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                                var objRespuestaSalidas = objEventoD.InsertarEvento(objEventoCoSalida2, IdDepartamento, IdOficina, false, "No fue posible sellar la guia: " + ex.Message, IDDatosDeEmpresa);

//                                                RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "Turnada correctamente para " + Constants.vbCrLf + "SALIDAS";
//                                                RespuestaImpo.BackColor = 0;
//                                                return RespuestaImpo;
//                                            }
//                                        }

//                                        // 1047 - GUIA HOUSE SELLADA
//                                        // 1166 - GUIA HOUSE SELLADA WEC
//                                        if ((Convert.ToBoolean(objDocumentosPorGuiaD.ExisteUltimoDocumentoSubido(IDReferencia, 1166, 1))) | (Convert.ToBoolean(objDocumentosPorGuiaD.ExisteUltimoDocumentoSubido(IDReferencia, 1047, 1))))
//                                        {
//                                            var documentoavucem = objDocumentosPorGuiaD.Buscar(IDReferencia, 1166);
//                                            if (documentoavucem.idDocumento == 0)
//                                                documentoavucem = objDocumentosPorGuiaD.Buscar(IDReferencia, 1047);

//                                            if (documentoavucem.idDocumento == 0)
//                                                throw new ArgumentException("No se encontro documento para enviar a VUCEM");

//                                            DigitalizadosVucem objDigitalizados = new DigitalizadosVucem();
//                                            DigitalizadosVucemData objDigitalizadosD = new DigitalizadosVucemData();

//                                            // ARRIBO DE FAST MORNING - INFORMATIVO
//                                            ControldeEventos objEventoCoSalida = new ControldeEventos(535, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                            var objRespuestaFastMorning = objEventoD.InsertarEvento(objEventoCoSalida, IdDepartamento, IdOficina, false, obsCheck, IDDatosDeEmpresa);

//                                            Thread thGuia = new Thread(() => nubePrepago.GeneraEDocumentAll(documentoavucem.idTipoDocumento, objReferencia1.IDReferencia, true, objReferencia1, ObjUsuario, pMisDocumentos, envioCOVE));
//                                            thGuia.Start();


//                                            RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "en espera de eDocument para " + Constants.vbCrLf + "CONTROL DE PAGO ELECTRONICO";
//                                            RespuestaImpo.BackColor = 0;
//                                            return RespuestaImpo;
//                                        }
//                                        else
//                                        {
//                                            // Throw New ArgumentException("No se encontro guía sellada")
//                                            // ARRIBO DE FAST MORNING - INFORMATIVO
//                                            ControldeEventos objEventoCoSalida = new ControldeEventos(535, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                            var objRespuestaFastMorning = objEventoD.InsertarEvento(objEventoCoSalida, IdDepartamento, IdOficina, false, obsCheck, IDDatosDeEmpresa);

//                                            // GUIA ARRIBADA PARA SALIDAS'
//                                            ControldeEventos objEventoCoSalida2 = new ControldeEventos(527, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                            var objRespuestaSalidas = objEventoD.InsertarEvento(objEventoCoSalida2, IdDepartamento, IdOficina, false, "No se encontro guía sellada", IDDatosDeEmpresa);

//                                            RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "Turnada correctamente para " + Constants.vbCrLf + "SALIDAS";
//                                            RespuestaImpo.BackColor = 0;
//                                            return RespuestaImpo;
//                                        }
//                                    }
//                                }
//                            }
//                            else
//                            {
//                                // CLASIFICACIÓN DETECTA INCONSISTENCIA EN MERCANCÍA SE CANCELA PRECAPTURA'
//                                ControldeEventos objEventos525 = new ControldeEventos(525, objReferencia1.IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                objEventoD.InsertarEvento(objEventos525, 54, IdOficina, false, "", IDDatosDeEmpresa);
//                            }
//                        }
//                    }
//                }
//                // __________FAST MORNING__________________



//                // VALIDACIONES
//                // ____________________________________________________________________________________________
//                // BUSCA SI EXISTE LA GUIA EN LA TABLA CONSOLANEXOS, SI EXISTE MANDA EXEPCION Y TERMINA 
//                ConsolAnexos objConsolAnexos = new ConsolAnexos();
//                ConsolAnexosData objConsolAnexosD = new ConsolAnexosData();
//                objConsolAnexos = objConsolAnexosD.Buscar(GuiaHouse, IDDatosDeEmpresa);

//                if (IsNothing(objConsolAnexos) == false & IdCategoria != 5)
//                    throw new Exception("Este Numero de Guia esta en Pedimento de Consolidado, Debe ser Eliminada");

//                // BUSCA SI LA GUIA YA EXISTE EN LA TABLA DE REFERENCIAS, SI ES SUBDIVISION PONE S AL INICIO DE LA REFERENCIA 
//                if (chkbSubdivision == true)
//                    GuiaHouse = "S" + GuiaHouse;

//                SaaioPedime objPedi = new SaaioPedime();
//                SaaioPedimeData objPediD = new SaaioPedimeData();
//                objPedi = objPediD.Buscar(GuiaHouse.Trim());
//                if (!IsNothing(objPedi))
//                {
//                    if (!IsNothing(objPedi.FIR_ELEC))
//                    {
//                        if (objPedi.FIR_ELEC.Trim()!= "")
//                            throw new ArgumentException("La Referencia " + GuiaHouse.Trim() + " ya tiene Firma Electronica");
//                    }
//                }
//                // ____________________________________________________________________________________________



//                if (validaPieceIds)
//                {
//                    if (piecesIds.Count == 0)
//                    {
//                        RespuestaImpo.SolicitarPieceId = true;
//                        RespuestaImpo.TotalPieceId = numberPieceIds;
//                        RespuestaImpo.Mensaje = "Se detectó AWB con " + numberPieceIds + " piezas, favor de ingresar los Piece Ids";
//                        return RespuestaImpo;
//                    }

//                    // INSERTAR LOS PIECE IDS
//                    string ultimosDiezDigitos;
//                    ultimosDiezDigitos = GuiaHouse.Substring(GuiaHouse.Length - 10);

//                    piecesIds.ForEach(pieceID =>
//                    {
//                        objPieceIData.ImpoInsertPieceID(ultimosDiezDigitos, pieceID, IdOficina, IdUsuario, objReferencia1.IDReferencia);
//                    });
//                }
//                if (piezaindividual & registrarPieceId & PieceIdIndividual != "")
//                {
//                    string ultimosDiezDigitos;
//                    ultimosDiezDigitos = GuiaHouse.Substring(GuiaHouse.Length - 10);
//                    objPieceIData.ImpoInsertPieceID(ultimosDiezDigitos, PieceIdIndividual, IdOficina, IdUsuario, objReferencia1.IDReferencia);
//                }

//                if (departamentoPrevio == 54)
//                {
//                    Precaptura(objReferencia1, IdUsuario, IdOficina, IDDatosDeEmpresa, departamentoPrevio);
//                    departamentoPrevio = controldeEventosData.BuscaDepartamentoActual(objReferencia1.IDReferencia, IdOficina);
//                }

//                CartadeInstruccionesData cartadeInstruccionesData = new CartadeInstruccionesData();
//                if (vidCliente != 0)
//                    RespuestaImpo.CARTADEINSTRUCCIONES = cartadeInstruccionesData.LoadDvgCartadeInstruccionesForCambioValor(vidCliente, IdOficina);

//                if ((departamentoPrevio == 51 | departamentoPrevio == 18) & multiPieza & piecesIds.Count == numberPieceIds)
//                {
//                    var sacarDeNoti = false;
//                    // Sacar si tiene carta de instrucciones 
//                    if (RespuestaImpo.CARTADEINSTRUCCIONES != null & RespuestaImpo.CARTADEINSTRUCCIONES.Rows.Count > 0)
//                        sacarDeNoti = true;
//                    // Sacar si es menor a 1000
//                    if (valorEnDolares <= 1000)
//                        sacarDeNoti = true;

//                    if (sacarDeNoti)
//                    {
//                        // Se completa referencia, sacamos de noti sin previo 
//                        NotiSinPrevio(objReferencia1, IdUsuario, IdOficina, IDDatosDeEmpresa, departamentoPrevio);
//                        departamentoPrevio = controldeEventosData.BuscaDepartamentoActual(objReferencia1.IDReferencia, IdOficina);
//                    }
//                }

//                if (departamentoPrevio > 0 & departamentoPrevio != 77 & departamentoPrevio != 9)
//                {
//                    CatalogoDepartamentos objDep = new CatalogoDepartamentos();
//                    CatalogoDepartamentosData objDepD = new CatalogoDepartamentosData();
//                    objDep = objDepD.Buscar(departamentoPrevio);

//                    // TODO si el departamento actual es 72 AA y arriba o se completa una referencia con multipiezas, notificamos al agente aduanal

//                    if (multiPieza)
//                    {
//                        if (departamentoPrevio == 72)
//                        {
//                            var idUsuarioAsignado = 0;
//                            CatalogoDeUsuarios objUsuarioAsignado;
//                            CatalogoDeUsuariosData objUsuarioData = new CatalogoDeUsuariosData();
//                            AsignaciondeGuiasData asignacionGuiasData = new AsignaciondeGuiasData();
//                            idUsuarioAsignado = asignacionGuiasData.GetAgenteAduanal(IDReferencia);
//                            if (idUsuarioAsignado > 0)
//                            {
//                                objUsuarioAsignado = objUsuarioData.BuscarPorId(idUsuarioAsignado);
//                                Correo correo = new Correo();
//                                var objOficina = new CatalogoDeOficinasData().Buscar(IdOficina);

//                                string mensaje = "Arribo parcial de la referencia " + GuiaHouse + " " + piecesIds.Count + "/" + numberPieceIds + ", " + string.Join(",", piecesIds.ToArray());
//                                if (piecesIds.Count < numberPieceIds)
//                                    correo.Enviar("Arribo parcial de la referencia " + GuiaHouse + " " + piecesIds.Count + "/" + numberPieceIds, objUsuarioAsignado.Email, mensaje, "HTML");
//                                else
//                                {
//                                    mensaje = "La referencia " + GuiaHouse + " con " + numberPieceIds + " piezas  se completo en " + objOficina.nombre;
//                                    correo.Enviar("La referencia " + GuiaHouse + " se completo en " + objOficina.nombre, objUsuarioAsignado.Email, mensaje, "HTML");
//                                }
//                            }
//                        }
//                    }


//                    throw new Exception("La referencia " + objReferencia1.NumeroDeReferencia + " está asignada al departamento " + Constants.vbCrLf + objDep.NombreDepartamento.Trim()+ " y el checkpoint es de otro departamento");
//                }


//                // _________SUBDIVICIONES__________________
//                var pieceIdsString = objPieceIData.GetPieceIDsByReference(IDReferencia);

//                MensajeCompuesto = pieceIdsString;

//                if (multiPieza)
//                {
//                    if (piecesIds.Count == numberPieceIds & !escaneoCompleto)
//                    {
//                        RespuestaImpo.EscaneoCompleto = true;


//                        if (departamentoPrevio != 77)
//                        {
//                            if (PiezasEnMismaOficina(GuiaHouseOriginal))
//                            {
//                                RespuestaImpo.Mensaje = "Guía " + GuiaHouse + " turnada a asignaciones";
//                                insertarCheckpoint(644, IDReferencia, IdUsuario, IdDepartamento, IdOficina, pieceIdsString, IDDatosDeEmpresa);
//                            }
//                            else
//                            {
//                                RespuestaImpo.Mensaje = "Guía " + GuiaHouse + " turnada a asignaciones";
//                                pieceIdsString = pieceIdsString + " " + PieceIdsOficinaGrupo(GuiaHouseOriginal);
//                                insertarCheckpoint(644, IDReferencia, IdUsuario, IdDepartamento, IdOficina, pieceIdsString, IDDatosDeEmpresa);
//                            }
//                        }
//                    }
//                    else if (avisarDiffAgenteAdu == 0 & avisarPrevioOConsolidado == 0)
//                    {
//                        if (turnadoParcialOAsignacion == 0)
//                        {
//                            ControldeEventosRepository controldeDeEventosDataRes = new ControldeEventosRepository(_configuration);
//                            RespuestaImpo.Eventos = controldeDeEventosDataRes.ObtenerUltimosEventos(GuiaHouse, IDDatosDeEmpresa, 3);
//                            RespuestaImpo.Mensaje = "Guía " + GuiaHouse + " turnada a asignaciones";
//                            if (departamentoPrevio == 77)
//                                return RespuestaImpo;
//                        }
//                        // ------------Inicio no se completo------------'
//                        // If turnadoParcialOAsignacion = 0 Then
//                        // RespuestaImpo.NotifyTurnadoParcialOAsignacion = True
//                        // Dim faltantes = numberPieceIds - piecesIds.Count
//                        // RespuestaImpo.Mensaje = "Faltan " + faltantes.ToString + " Piece Id de " + numberPieceIds.ToString + " ¿Qué desea hacer?"
//                        // Return RespuestaImpo
//                        // End If
//                        // If turnadoParcialOAsignacion = 2 Then

//                        // Dim controldeDeEventosDataRes As New ControldeEventosRepository(_configuration)
//                        // RespuestaImpo.Eventos = controldeDeEventosDataRes.ObtenerUltimosEventos(GuiaHouse, IDDatosDeEmpresa, 3)
//                        // RespuestaImpo.Mensaje = "Guía " + GuiaHouse + " turnada a asignaciones"
//                        // If departamentoPrevio = 77 Then
//                        // Return RespuestaImpo
//                        // End If

//                        // insertarCheckpoint(644, IDReferencia, IdUsuario, IdDepartamento, IdOficina, pieceIdsString, IDDatosDeEmpresa)

//                        // Return RespuestaImpo
//                        // ElseIf turnadoParcialOAsignacion = 1 Then
//                        // MensajeCompuesto = "Turnado parcial (" + piecesIds.Count.ToString + "/" + numberPieceIds.ToString + "): " + pieceIdsString
//                        // End If
//                        // -------------Fin no se completo------------'
//                        if (departamentoPrevio != 77)
//                        {
//                            if (PiezasEnMismaOficina(GuiaHouseOriginal))
//                            {
//                                RespuestaImpo.Mensaje = "Guía " + GuiaHouse + " turnada a asignaciones";
//                                insertarCheckpoint(644, IDReferencia, IdUsuario, IdDepartamento, IdOficina, pieceIdsString, IDDatosDeEmpresa);
//                            }
//                            else
//                            {
//                                RespuestaImpo.Mensaje = "Guía " + GuiaHouse + " turnada a asignaciones";
//                                pieceIdsString = pieceIdsString + " " + PieceIdsOficinaGrupo(GuiaHouseOriginal);
//                                insertarCheckpoint(644, IDReferencia, IdUsuario, IdDepartamento, IdOficina, pieceIdsString, IDDatosDeEmpresa);
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    RespuestaImpo.Mensaje = "Guía " + GuiaHouse + " turnada a asignaciones";
//                    if (!departamentoPrevio == 77)
//                        insertarCheckpoint(644, IDReferencia, IdUsuario, IdDepartamento, IdOficina, "", IDDatosDeEmpresa);
//                }
//                // _________Turnar a clasificacion_________


//                var objTgrupo = new GruposdeTrabajo();
//                CatalogoDeUsuarios GObjUsuario = new CatalogoDeUsuarios();
//                CatalogoDeUsuariosData objUsuariosD = new CatalogoDeUsuariosData();
//                ControldeEventos objTEventos123 = new ControldeEventos();
//                AsignarGuiasRespuesta objTResp = new AsignarGuiasRespuesta();
//                ControldeEventosRepository objTEventosD = new ControldeEventosRepository(_configuration);
//                AsignarGuias objTAsignar = new AsignarGuias();
//                int idTEvento;
//                GObjUsuario = objUsuariosD.Buscar(IdUsuario);
//                if (!IsNothing(objTransito))
//                {
//                    if (!IsNothing(objTransito.IdOficinaLlegada))
//                    {
//                        int idRiel = 0;
//                        if (!IsNothing(objCMF))
//                            idRiel = objCMF.IdRiel;
//                        // -- 1 FRM1, 6 FRM2, 8 SIN RIEL
//                        List<int> rieles = new List<int>();
//                        rieles.Add(1);
//                        rieles.Add(6);
//                        rieles.Add(8);

//                        if (rieles.Contains(idRiel))
//                        {
//                            // TODO: ASIGNACIÓN TURNA A CLASI PARA PREVIO
//                            objTgrupo = objGrupoD.Buscar(IDGrupodeTrabajo);
//                            objTEventos123 = new ControldeEventos(665, objReferencia1.IDReferencia, GObjUsuario.IdUsuario, DateTime.Parse("01/01/1900"));
//                            objTResp = objTEventosD.InsertarEvento(objTEventos123, IdDepartamento, GObjUsuario.IdOficina, false, MensajeCompuesto, GObjUsuario.IDDatosDeEmpresa);
//                            idTEvento = objTResp.IdEvento;
//                            if (reasignar)
//                                objTAsignar.ReasignarGuia(GObjUsuario.IdUsuario, objTAsignar.getUsuarioDeGrupo(IDGrupodeTrabajo), objReferencia1.IDReferencia);
//                            // ENVIAR CORREO
//                            if (objTEventosD.SendMailTurnado(objReferencia1.NumeroDeReferencia, objTransito.IdOficinaSalida, objTransito.IdOficinaLlegada, objReferencia1.IDReferencia))
//                            {
//                                RespuestaImpo.BackColor = 0;
//                                RespuestaImpo.Mensaje = "Guía:" + " " + objReferencia1.NumeroDeReferencia + " " + "Turnada Correctamente," + Constants.vbCrLf + " al grupo" + " " + objTgrupo.Nombre.Trim()+ "";
//                                return RespuestaImpo;
//                            }
//                        }
//                    }

//                    // TODO: Esta logica ya no aplica quitar
//                    bool IsSubdivision = false;
//                    if (!chkbSubdivision)
//                    {
//                        IsSubdivision = objPieceIData.IsSubdivision(GuiaHouse.Trim());
//                        if (IsSubdivision)
//                        {
//                            RespuestaImpo.NotifyIsSubdivision = true;
//                            RespuestaImpo.Mensaje = "Se detecto Piece Id sin validar en Previo ¿Se trata de una subdivisión?";
//                            return RespuestaImpo;
//                        }
//                    }
//                }


//                // _________SUBDIVICIONES__________________

//                PrioridadSafranData prioridadData = new PrioridadSafranData();
//                PrioridadSafran prioridadSafran;

//                prioridadSafran = prioridadData.Buscar(GuiaHouse.Trim());

//                if (!IsNothing(prioridadSafran))
//                    RespuestaImpo.Prioridad = "AWB " + GuiaHouse.Trim() + "  con prioridad tipo " + prioridadSafran.Prioridad;

//                GruposdeTrabajo objGrupoTop = new GruposdeTrabajo();
//                objGrupoTop = objGrupoD.BuscarTop(IdOficina, 9);

//                bool AsignarTurnaClasiprevio = false;

//                if (IdOficina == 2 | IdOficina == 24)
//                {
//                    if (multiPieza)
//                    {
//                        if (piecesIds.Count == numberPieceIds)
//                        {
//                            // Se completa referencia CHECKPOINT REFERENCIA CON BULTOS COMPLETOS
//                            ControldeEventos objEventos692 = new ControldeEventos(692, objReferencia1.IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                            objEventoD.InsertarEvento(objEventos692, IdDepartamento, IdOficina, IDDatosDeEmpresa);
//                        }
//                        else
//                        {
//                            var quedarEnAsignaciones = true;

//                            // Permitimos turnar a Agente Aduanal incompletas
//                            if (IdCategoria == 1)
//                                quedarEnAsignaciones = false;

//                            if (IdCategoria == 10)
//                                quedarEnAsignaciones = false;

//                            if (quedarEnAsignaciones)
//                            {
//                                ControldeEventosRepository controldeDeEventosDataRes = new ControldeEventosRepository(_configuration);
//                                RespuestaImpo.BackColor = 0;
//                                RespuestaImpo.Eventos = controldeDeEventosDataRes.ObtenerUltimosEventos(GuiaHouse, IDDatosDeEmpresa, 3);
//                                RespuestaImpo.Mensaje = "Guía " + GuiaHouse.Trim() + " turnada a asignaciones";
//                                return RespuestaImpo;
//                            }
//                        }
//                    }
//                }


//                switch (IdCategoria)
//                {
//                    case 0 // Sin Identificar
//                   :
//                        {
//                            throw new ArgumentException("ESTA CATEGORÍA NO ESTA DEFINIDA, FAVOR DE TURNAR MANUALMENTE, REFERENCIA " + GuiaHouse + " SE QUEDA EN POOL DE ASIGNACIONES");
//                            break;
//                        }

//                    case 1:
//                        {
//                            if (avisarDiffAgenteAdu == 0)
//                            {
//                                RespuestaImpo.AvisarDiffAgenteAdu = true;
//                                RespuestaImpo.Mensaje = "Guía que pertenece a otro Agente Aduanal";
//                                return RespuestaImpo;
//                            }

//                            if (avisarDiffAgenteAdu == 1)
//                            {
//                                objCustomsAlertsD.ModificarporIDCategoria3(Strings.Mid(GuiaHouse, Strings.Len(GuiaHouse) - 9, 10));
//                                RespuestaImpo.Mensaje = "Cambio a categoria 3"; // REALIZAR PREVIO
//                                RespuestaImpo.BackColor = 4;
//                                var preRespuesta = Procesar(GuiaHouse, IdUsuario, IDDatosDeEmpresa, IdOficina, IdDepartamento, Guia2d, rdbUnitarias, rdbPartidas, rdbMiscelaneas, rdbVolumen, rdbTransito, chkPreAlertas, chkDiasAnteriores, chkInformaciones, validaNoManisfestado, validaEncargoCliente, avisarDiffAgenteAdu, chkbSubdivision, IDGrupodeTrabajoSelected, validaNubePrepago, ObjUsuario, RevDsicrepanciaGP, chkFis, piecesIds, reasignar, escaneoCompleto, turnadoParcialOAsignacion, validarReferencia, avisarPrevioOConsolidado).GetAwaiter().GetResult;
//                                return preRespuesta;
//                            }
//                            else
//                            {
//                                IDReferencia = objReferencia1.IDReferencia;

//                                AsignarGuiasRespuesta objRespuesta = new AsignarGuiasRespuesta();
//                                ControldeEventosRepository objEventosDa = new ControldeEventosRepository(_configuration);
//                                RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "Turnada correctamente para AGENTE ADUANAL";
//                                RespuestaImpo.BackColor = 4;

//                                if (multiPieza)
//                                {
//                                    if (piecesIds.Count < numberPieceIds)
//                                        MensajeCompuesto = "Turnado parcial (" + piecesIds.Count.ToString() + "/" + numberPieceIds.ToString() + "): " + pieceIdsString;
//                                }


//                                // CHECK: ASIGNACION TURNA GUÍA PARA AGENTE ADUANAL
//                                ControldeEventos objEventoATA = new ControldeEventos(660, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                objRespuesta = objEventosDa.InsertarEvento(objEventoATA, IdDepartamento, IdOficina, false, MensajeCompuesto, IDDatosDeEmpresa);
//                                ControldeEventosRepository controldeDeEventosDataRes = new ControldeEventosRepository(_configuration);
//                                RespuestaImpo.Eventos = controldeDeEventosDataRes.ObtenerUltimosEventos(GuiaHouse, IDDatosDeEmpresa, 3);
//                                // TODO notificar a aa via email

//                                if (multiPieza)
//                                {
//                                    var idUsuarioAsignado = objRespuesta.IdUsuarioAsignado;
//                                    if (idUsuarioAsignado > 0)
//                                    {
//                                        CatalogoDeUsuarios objUsuarioAsignado;
//                                        CatalogoDeUsuariosData objUsuarioData = new CatalogoDeUsuariosData();
//                                        objUsuarioAsignado = objUsuarioData.BuscarPorId(idUsuarioAsignado);

//                                        Correo correo = new Correo();
//                                        var objOficina = new CatalogoDeOficinasData().Buscar(IdOficina);

//                                        string mensaje = "Arribo parcial de la referencia " + GuiaHouse + " " + piecesIds.Count + "/" + numberPieceIds + ", " + string.Join(",", piecesIds.ToArray());
//                                        if (piecesIds.Count < numberPieceIds)
//                                            correo.Enviar("Arribo parcial de la referencia " + GuiaHouse + " " + piecesIds.Count + "/" + numberPieceIds, objUsuarioAsignado.Email, mensaje, "HTML");
//                                        else
//                                        {
//                                            mensaje = "La referencia " + GuiaHouse + " con " + numberPieceIds + " piezas  se completo en " + objOficina.nombre;
//                                            correo.Enviar("La referencia " + GuiaHouse + " se completo en " + objOficina.nombre, objUsuarioAsignado.Email, mensaje, "HTML");
//                                        }
//                                    }
//                                }

//                                return RespuestaImpo;
//                            }

//                            break;
//                        }

//                    case 10:
//                        {
//                            if (avisarDiffAgenteAdu == 0)
//                            {
//                                RespuestaImpo.AvisarDiffAgenteAdu = true;
//                                RespuestaImpo.Mensaje = "Guía que pertenece a otro Agente Aduanal";
//                                return RespuestaImpo;
//                            }

//                            if (avisarDiffAgenteAdu == 1)
//                            {
//                                objCustomsAlertsD.ModificarporIDCategoria3(Strings.Mid(GuiaHouse, Strings.Len(GuiaHouse) - 9, 10));
//                                RespuestaImpo.Mensaje = "Cambio a categoria 3"; // REALIZAR PREVIO
//                                RespuestaImpo.BackColor = 4;
//                                var preRespuesta = Procesar(GuiaHouse, IdUsuario, IDDatosDeEmpresa, IdOficina, IdDepartamento, Guia2d, rdbUnitarias, rdbPartidas, rdbMiscelaneas, rdbVolumen, rdbTransito, chkPreAlertas, chkDiasAnteriores, chkInformaciones, validaNoManisfestado, validaEncargoCliente, avisarDiffAgenteAdu, chkbSubdivision, IDGrupodeTrabajoSelected, validaNubePrepago, ObjUsuario, RevDsicrepanciaGP, chkFis, piecesIds, reasignar, escaneoCompleto, turnadoParcialOAsignacion, validarReferencia, avisarPrevioOConsolidado).GetAwaiter().GetResult;
//                                return preRespuesta;
//                            }
//                            else
//                            {
//                                IDReferencia = objReferencia1.IDReferencia;

//                                AsignarGuiasRespuesta objRespuesta = new AsignarGuiasRespuesta();
//                                ControldeEventosRepository objEventosDa = new ControldeEventosRepository(_configuration);

//                                RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "Turnada correctamente para AGENTE ADUANAL";
//                                RespuestaImpo.BackColor = 4;

//                                // CHECK: ASIGNACION TURNA GUÍA PARA AGENTE ADUANAL

//                                ControldeEventos objEventoATA = new ControldeEventos(661, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                objRespuesta = objEventosDa.InsertarEvento(objEventoATA, IdDepartamento, IdOficina, false, MensajeCompuesto, IDDatosDeEmpresa);

//                                ControldeEventosRepository controldeDeEventosDataRes = new ControldeEventosRepository(_configuration);
//                                RespuestaImpo.Eventos = controldeDeEventosDataRes.ObtenerUltimosEventos(GuiaHouse, IDDatosDeEmpresa, 3);

//                                // TODO notificar a aa via email
//                                if (multiPieza)
//                                {
//                                    var idUsuarioAsignado = objRespuesta.IdUsuarioAsignado;
//                                    if (idUsuarioAsignado > 0)
//                                    {
//                                        CatalogoDeUsuarios objUsuarioAsignado;
//                                        CatalogoDeUsuariosData objUsuarioData = new CatalogoDeUsuariosData();
//                                        objUsuarioAsignado = objUsuarioData.BuscarPorId(idUsuarioAsignado);
//                                        Correo correo = new Correo();
//                                        var objOficina = new CatalogoDeOficinasData().Buscar(IdOficina);

//                                        string mensaje = "Arribo parcial de la referencia " + GuiaHouse + " " + piecesIds.Count + "/" + numberPieceIds + ", " + string.Join(",", piecesIds.ToArray());
//                                        if (piecesIds.Count < numberPieceIds)
//                                            correo.Enviar("Arribo parcial de la referencia " + GuiaHouse + " " + piecesIds.Count + "/" + numberPieceIds, objUsuarioAsignado.Email, mensaje, "HTML");
//                                        else
//                                        {
//                                            mensaje = "La referencia " + GuiaHouse + " con " + numberPieceIds + " piezas  se completo en " + objOficina.nombre;
//                                            correo.Enviar("La referencia " + GuiaHouse + " se completo en " + objOficina.nombre, objUsuarioAsignado.Email, mensaje, "HTML");
//                                        }
//                                    }
//                                }

//                                return RespuestaImpo;
//                            }

//                            break;
//                        }

//                    case 2 // Clientes Top
//             :
//                        {
//                            if (!IsNothing(objGrupoTop))
//                                IDGrupodeTrabajo = objGrupoTop.IdGrupo;


//                            // CHECK: ASIGNACIÓN TURNA A CLASI PARA PREVIO
//                            AsignarGuiasRespuesta objRespuesta = new AsignarGuiasRespuesta();
//                            ControldeEventosRepository objEventosDa = new ControldeEventosRepository(_configuration);
//                            if (validaPieceIds)
//                            {
//                                if (piecesIds.Count < numberPieceIds)
//                                    MensajeCompuesto = "Turnado parcial (" + piecesIds.Count.ToString() + "/" + numberPieceIds.ToString() + "): " + pieceIdsString;


//                                if (oficinaObj.IdOficina == 2)
//                                {
//                                    if (piecesIds.Count == numberPieceIds & PiezasEnMismaOficina(GuiaHouseOriginal) & PiezasConMismaFecha(GuiaHouseOriginal))
//                                        MensajeCompuesto = "Guía Completada favor de solicitar piece ids para previo " + pieceIdsString;
//                                    else if (piecesIds.Count == numberPieceIds & !PiezasEnMismaOficina(GuiaHouseOriginal))
//                                        MensajeCompuesto = "Guía Completada en otra oficina favor de solicitar piece ids para previo";
//                                }
//                            }

//                            ControldeEventos objEventoATA = new ControldeEventos(665, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                            objRespuesta = objEventosDa.InsertarEvento(objEventoATA, IdDepartamento, IdOficina, false, MensajeCompuesto, IDDatosDeEmpresa);

//                            AsignarTurnaClasiprevio = true;
//                            break;
//                        }

//                    case 3:
//                    case 11 // Realizar Previo
//             :
//                        {
//                            if (reasignar)
//                            {
//                                if (!IsNothing(objGrupoTop))
//                                {
//                                    if (IDGrupodeTrabajoSelected == objGrupoTop.IdGrupo)
//                                        throw new ArgumentException("No se debe asignar a equipo top, la instruccion es realizar previo");
//                                }
//                            }


//                            // CHECK: ASIGNACIÓN TURNA A CLASI PARA PREVIO
//                            AsignarGuiasRespuesta objRespuesta = new AsignarGuiasRespuesta();
//                            ControldeEventosRepository objEventosDa = new ControldeEventosRepository(_configuration);


//                            if (validaPieceIds)
//                            {
//                                if (piecesIds.Count < numberPieceIds)
//                                    MensajeCompuesto = "Turnado parcial (" + piecesIds.Count.ToString() + "/" + numberPieceIds.ToString() + "): " + pieceIdsString;

//                                if (oficinaObj.IdOficina == 2)
//                                {
//                                    if (piecesIds.Count == numberPieceIds & PiezasEnMismaOficina(GuiaHouseOriginal) & PiezasConMismaFecha(GuiaHouseOriginal))
//                                        MensajeCompuesto = "Guía Completada favor de solicitar piece ids para previo " + pieceIdsString;
//                                    else if (piecesIds.Count == numberPieceIds & !PiezasEnMismaOficina(GuiaHouseOriginal))
//                                        MensajeCompuesto = "Guía Completada en otra oficina favor de solicitar piece ids para previo";
//                                }
//                            }

//                            ControldeEventos objEventoATA = new ControldeEventos(665, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                            objRespuesta = objEventosDa.InsertarEvento(objEventoATA, IdDepartamento, IdOficina, false, MensajeCompuesto, IDDatosDeEmpresa);

//                            AsignarTurnaClasiprevio = true;
//                            break;
//                        }

//                    case 4 // Notificar antes de Realizar Previo  
//             :
//                        {
//                            if (avisarDiffAgenteAdu == 0)
//                            {
//                                RespuestaImpo.Mensaje = "La guía deberá ser notificada antes de realizar el previo";
//                                RespuestaImpo.AvisarDiffAgenteAdu = true;
//                                return RespuestaImpo;
//                            }

//                            if (avisarDiffAgenteAdu == 1)
//                            {
//                                objCustomsAlertsD.ModificarporIDCategoria3(Strings.Mid(GuiaHouse, Strings.Len(GuiaHouse) - 9, 10));

//                                RespuestaImpo.Mensaje = "Cambio a categoria 3"; // REALIZAR PREVIO
//                                RespuestaImpo.BackColor = 4;
//                                var preRespuesta = Procesar(GuiaHouse, IdUsuario, IDDatosDeEmpresa, IdOficina, IdDepartamento, Guia2d, rdbUnitarias, rdbPartidas, rdbMiscelaneas, rdbVolumen, rdbTransito, chkPreAlertas, chkDiasAnteriores, chkInformaciones, validaNoManisfestado, validaEncargoCliente, avisarDiffAgenteAdu, chkbSubdivision, IDGrupodeTrabajoSelected, validaNubePrepago, ObjUsuario, RevDsicrepanciaGP, chkFis, piecesIds, reasignar, escaneoCompleto, turnadoParcialOAsignacion, validarReferencia, avisarPrevioOConsolidado).GetAwaiter().GetResult;
//                                return preRespuesta;
//                            }
//                            else
//                            {
//                                IDReferencia = objReferencia1.IDReferencia;

//                                AsignarGuiasRespuesta objRespuestaSA = new AsignarGuiasRespuesta();
//                                ControldeEventosRepository objEventosDaSA = new ControldeEventosRepository(_configuration);
//                                ControldeEventos objEventoPSA = new ControldeEventos(662, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                objRespuestaSA = objEventosDaSA.InsertarEvento(objEventoPSA, IdDepartamento, IdOficina, false, MensajeCompuesto, IDDatosDeEmpresa);

//                                RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "Turnada correctamente para NOTIFICACION SIN PREVIO";
//                                RespuestaImpo.BackColor = 4;

//                                ControldeEventosRepository controldeDeEventosDataRes = new ControldeEventosRepository(_configuration);
//                                RespuestaImpo.Eventos = controldeDeEventosDataRes.ObtenerUltimosEventos(GuiaHouse, IDDatosDeEmpresa, 3);
//                                return RespuestaImpo;
//                            }

//                            break;
//                        }

//                    case 5 // Consolidado
//             :
//                        {
//                            var isGlobalMayor = false;
//                            if (IDDatosDeEmpresa == 2)
//                            {
//                                if (IdOficina == 21)
//                                {
//                                    if (!IsNothing(objCustomsAlerts))
//                                    {
//                                        var IdTipodePedimento = objCustomsAlerts.IdTipodePedimento; // ID 18,19
//                                        List<int> listIdPedimentos = new List<int>{18, 19};

//                                        if (listIdPedimentos.Contains(IdTipodePedimento))
//                                        {
//                                            isGlobalMayor = true;
//                                            vidCliente = 9307822; // FEDERAL EXPRESS



//                                            CatalogodeMasterData objGuiaMasterD = new CatalogodeMasterData();
//                                            var objGuiaMaster = objGuiaMasterD.Buscar(objCustomsAlerts.GuiaMaster.Trim);

//                                            if (!IsNothing(objGuiaMaster))
//                                            {
//                                                objReferencia1.IDMasterConsol = objGuiaMaster.IDMasterConsol;
//                                                vidMasterConsol = objGuiaMaster.IDMasterConsol;
//                                            }
//                                            ReferenciasRepository ReferenciasRepository = new ReferenciasRepository();
//                                            objReferencia1.IDCliente = 9307822; // FEDERAL EXPRESS

//                                            ReferenciasRepository.Modificar(objReferencia1);

//                                            SaaioPedime objModificaSaaioPedime = new SaaioPedime();
//                                            SaaioPedime objPedime = new SaaioPedime();
//                                            SaaioPedimeData objPedimeD = new SaaioPedimeData();
//                                            objPedime = objPedimeD.Buscar(GuiaHouse);

//                                            SaaioPedime objSaaioPedime = new SaaioPedime();


//                                            GObjUsuario = objUsuariosD.Buscar(IdUsuario);


//                                            objSaaioPedime.NUM_REFE = GuiaHouse;
//                                            objSaaioPedime.ADU_DESP = GObjUsuario.Oficina.AduDesp;
//                                            objSaaioPedime.ADU_ENTR = GObjUsuario.Oficina.AduEntr;


//                                            FechadelServidor objFechaServ = new FechadelServidor();
//                                            FechadelServidorData objFechaServD = new FechadelServidorData();
//                                            objFechaServ = objFechaServD.Buscar(MyConnectionString);


//                                            DateTime dFecha;
//                                            dFecha = DateTime.ParseExact(objFechaServ.Fecha, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None);

//                                            objSaaioPedime.FEC_ENTR = dFecha;

//                                            CtarcTipCam objTipodeCambio = new CtarcTipCam();
//                                            CtarcTipCamData objTipodeCambioD = new CtarcTipCamData();
//                                            objTipodeCambio = objTipodeCambioD.Buscar(DateTime.Now);
//                                            if (!IsNothing(objTipodeCambio))
//                                            {
//                                                if (objTipodeCambio.TIP_CAM == null/* TODO Change to default(_) if this is not a reference type */ )
//                                                    throw new ArgumentException("El tipo de cambio no se ha dado de alta en Expertti");
//                                                else
//                                                    objSaaioPedime.TIP_CAMB = objTipodeCambio.TIP_CAM;
//                                            }


//                                            objSaaioPedime.CVE_PEDI = "T1";
//                                            objSaaioPedime.DES_ORIG = GObjUsuario.Oficina.DesOrig;

//                                            // Importacion
//                                            objSaaioPedime.MTR_ENTR = GObjUsuario.Oficina.MtrEntrImp;
//                                            objSaaioPedime.MTR_ARRI = GObjUsuario.Oficina.MtrArriImp;
//                                            objSaaioPedime.MTR_SALI = GObjUsuario.Oficina.MtrSaliImp;
//                                            objSaaioPedime.REG_ADUA = "IMD";


//                                            objSaaioPedime.MON_VASE = "0";

//                                            objSaaioPedime.SEC_DESP = GObjUsuario.Oficina.SecDesp;
//                                            objSaaioPedime.CVE_CAPT = "";
//                                            Clientes objClient = new Clientes();
//                                            ClientesData objClientD = new ClientesData();

//                                            objClient = objClientD.Buscar(vidCliente);
//                                            if (!IsNothing(objClient))
//                                                objSaaioPedime.CVE_IMPO = objClient.Clave;




//                                            objSaaioPedime.IMP_EXPO = 1;
//                                            objSaaioPedime.PAT_AGEN = GObjUsuario.Oficina.PatenteDefault;
//                                            objSaaioPedime.PES_BRUT = objCustomsAlerts.PesoTotal;
//                                            objSaaioPedime.CAN_BULT = objCustomsAlerts.Piezas;
//                                            objSaaioPedime.TOT_VEHI = 1;
//                                            objSaaioPedime.TIP_MOVA = "USD";

//                                            CatalogodeAgentesAduanales objCatalogodeAgentesAduanales = new CatalogodeAgentesAduanales();
//                                            CatalogodeAgentesAduanalesData objCatalogodeAgentesAduanalesD = new CatalogodeAgentesAduanalesData();

//                                            objCatalogodeAgentesAduanales = objCatalogodeAgentesAduanalesD.Buscar(GObjUsuario.Oficina.PatenteDefault);

//                                            if (!IsNothing(objCatalogodeAgentesAduanales))
//                                            {
//                                                objSaaioPedime.CVE_REPR = objCatalogodeAgentesAduanales.ClavedeRepresentante;
//                                                objSaaioPedime.EMP_FACT = objCatalogodeAgentesAduanales.EmpresaFactura;
//                                            }

//                                            if (IsNothing(objPedime))
//                                                objPedimeD.Insertar(objSaaioPedime);
//                                            else
//                                                objPedimeD.Modificar(objSaaioPedime);

//                                            SaaioGuias objInsertaGuiaM = new SaaioGuias();
//                                            SaaioGuiasData objInsertaGuiaHoMD = new SaaioGuiasData();

//                                            var ExistGuiaHouse = objInsertaGuiaHoMD.BuscarGuia(objReferencia1.NumeroDeReferencia, "H");

//                                            if (IsNothing(ExistGuiaHouse))
//                                            {
//                                                // INSERTA LA GUIA HOUSE
//                                                SaaioGuias LlenarobjInsertaGuiaH = new SaaioGuias();

//                                                LlenarobjInsertaGuiaH.NUM_REFE = GuiaHouse;
//                                                LlenarobjInsertaGuiaH.GUIA = GuiaHouse;
//                                                LlenarobjInsertaGuiaH.IDE_MH = "H";
//                                                LlenarobjInsertaGuiaH.CONS_GUIA = "1";
//                                                objInsertaGuiaHoMD.Insertar(LlenarobjInsertaGuiaH);
//                                            }



//                                            var ExistGuiaMaster = objInsertaGuiaHoMD.Buscar(objReferencia1.NumeroDeReferencia, "M");

//                                            if (IsNothing(ExistGuiaMaster))
//                                            {
//                                                // INSERTA LA GUIA MASTER
//                                                if (!IsNothing(objGuiaMaster))
//                                                {
//                                                    SaaioGuias LlenarobjInsertaGuiaM = new SaaioGuias();

//                                                    LlenarobjInsertaGuiaM.NUM_REFE = GuiaHouse;
//                                                    LlenarobjInsertaGuiaM.GUIA = objGuiaMaster.GuiaMaster;
//                                                    LlenarobjInsertaGuiaM.IDE_MH = "M";
//                                                    LlenarobjInsertaGuiaM.CONS_GUIA = "1";
//                                                    objInsertaGuiaM.NUM_REFE = objInsertaGuiaHoMD.Insertar(LlenarobjInsertaGuiaM);
//                                                }
//                                            }
//                                        }
//                                    }
//                                }
//                            }

//                            if (isGlobalMayor == false)
//                            {
//                                // If avisarPrevioOConsolidado = 0 Then
//                                // RespuestaImpo.AvisarPrevioOConsolidado = True
//                                // RespuestaImpo.Mensaje = "Se detecta guía para consolidado, seleccione opción"
//                                // Return RespuestaImpo

//                                // End If

//                                // If avisarPrevioOConsolidado = 1 Then

//                                // 'CHECK: ASIGNACIÓN TURNA A CLASI PARA PREVIO
//                                // Dim objRespuesta As New AsignarGuiasRespuesta
//                                // Dim objEventosDa As New ControldeEventosRepository

//                                // Dim objEventoATA As New ControldeEventos(665, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"))
//                                // objRespuesta = objEventosDa.InsertarEvento(objEventoATA, IdDepartamento, IdOficina, False, MensajeCompuesto, IDDatosDeEmpresa)

//                                // AsignarTurnaClasiprevio = True

//                                // ElseIf avisarPrevioOConsolidado = 2 Then

//                                // CHECK: ASIGNACIÓN TURNA AL ÁREA DE CONSOLIDADO
//                                AsignarGuiasRespuesta objRespuesta = new AsignarGuiasRespuesta();
//                                ControldeEventosRepository objEventosDa = new ControldeEventosRepository(_configuration);
//                                ControldeEventosRepository controldeDeEventosDataRes = new ControldeEventosRepository(_configuration);
//                                ControldeEventos objEventoATA = new ControldeEventos(663, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                objRespuesta = objEventosDa.InsertarEvento(objEventoATA, IdDepartamento, IdOficina, false, MensajeCompuesto, IDDatosDeEmpresa);
//                                RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "Turnada correctamente para ÁREA DE CONSOLIDADO";
//                                RespuestaImpo.Eventos = controldeDeEventosDataRes.ObtenerUltimosEventos(GuiaHouse, IDDatosDeEmpresa, 3);
//                                RespuestaImpo.BackColor = 4;
//                                return RespuestaImpo;
//                            }

//                            break;
//                        }
//                }

//                if (AsignarTurnaClasiprevio)
//                {
//                    if (validaPieceIds)
//                    {
//                        if (piecesIds.Count == numberPieceIds & PiezasEnMismaOficina(GuiaHouseOriginal) & PiezasConMismaFecha(GuiaHouseOriginal))
//                        {
//                            if (oficinaObj.IdOficina == 24)
//                            {
//                                // PREVIO SOLICITADO A ALMACEN
//                                string obsAlmacen = "";
//                                try
//                                {
//                                    wsJCJFSolicitarPrevio objJCJF = new wsJCJFSolicitarPrevio();
//                                    string Respuesta = "";
//                                    Respuesta = await objJCJF.fSolicitarPrevioAsync(GuiaHouseOriginal);
//                                    obsAlmacen = "WebAPi JCJF: " + Respuesta;
//                                }
//                                catch (Exception ex)
//                                {
//                                    obsAlmacen = "WebAPi JCJF: " + ex.Message.Trim();
//                                }

//                                AsignarGuiasRespuesta objRespuesta = new AsignarGuiasRespuesta();
//                                ControldeEventosRepository objEventosDa = new ControldeEventosRepository(_configuration);
//                                ControldeEventos objEventoCo = new ControldeEventos(294, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                objRespuesta = objEventosDa.InsertarEvento(objEventoCo, IdDepartamento, IdOficina, false, obsAlmacen, IDDatosDeEmpresa);
//                            }
//                        }
//                        else if (piecesIds.Count == numberPieceIds & !PiezasEnMismaOficina(GuiaHouseOriginal))
//                        {
//                            if (oficinaObj.IdOficina == 24)
//                            {
//                                // PREVIO SOLICITADO A ALMACEN

//                                string obsAlmacen = "Guía Completata en otra oficina";
//                                try
//                                {
//                                    wsJCJFSolicitarPrevio objJCJF = new wsJCJFSolicitarPrevio();
//                                    string Respuesta = "";
//                                    Respuesta = await objJCJF.fSolicitarPrevioAsync(GuiaHouseOriginal);
//                                    obsAlmacen = obsAlmacen + " WebAPi JCJF: " + Respuesta;
//                                }
//                                catch (Exception ex)
//                                {
//                                    obsAlmacen = obsAlmacen + " WebAPi JCJF: " + ex.Message.Trim();
//                                }

//                                AsignarGuiasRespuesta objRespuesta = new AsignarGuiasRespuesta();
//                                ControldeEventosRepository objEventosDa = new ControldeEventosRepository(_configuration);
//                                ControldeEventos objEventoCo = new ControldeEventos(294, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                objRespuesta = objEventosDa.InsertarEvento(objEventoCo, IdDepartamento, IdOficina, false, obsAlmacen, IDDatosDeEmpresa);
//                                RespuestaImpo.Mensaje = "PREVIO SOLICITADO A ALMACEN";
//                            }
//                        }
//                    }

//                    ControldeEventosRepository controldeDeEventosDataRes = new ControldeEventosRepository(_configuration);
//                    RespuestaImpo.Eventos = controldeDeEventosDataRes.ObtenerUltimosEventos(GuiaHouse, IDDatosDeEmpresa, 3);
//                }

//                // BUSCA EN TABLA CATALOGO DE COTIZADORES POR CLIENTE Y SE TREA EL IDUSUARIO,
//                // SI SE TIENE CLIENTE IDENTIFICADO, BSUCA SI TIENE CLASIFICADOR ASIGNADO 

//                CatalogoDeCotizadoresPorCliente objCotizadoresporCliente = new CatalogoDeCotizadoresPorCliente();
//                CatalogoDeCotizadoresPorClienteData objCotizadoresporClienteD = new CatalogoDeCotizadoresPorClienteData();
//                objCotizadoresporCliente = objCotizadoresporClienteD.Buscar(vidCliente, IdOficina);

//                if (!IsNothing(objCotizadoresporCliente))
//                    IDGrupodeTrabajo = objGrupoD.BuscarporGrupo(objCotizadoresporCliente.IdUsuario);


//                if (objEventoD.BuscaSiExisteIDCheckpoint(8, IDReferencia))
//                {
//                    rdbUnitarias = true;
//                    rdbPartidas = false;
//                    rdbMiscelaneas = false;
//                    rdbVolumen = false;
//                    rdbTransito = false;
//                }
//                else if (objEventoD.BuscaSiExisteIDCheckpoint(9, IDReferencia))
//                {
//                    rdbPartidas = true;
//                    rdbUnitarias = false;
//                    rdbMiscelaneas = false;
//                    rdbVolumen = false;
//                    rdbTransito = false;
//                }
//                else if (objEventoD.BuscaSiExisteIDCheckpoint(16, IDReferencia))
//                {
//                    rdbMiscelaneas = true;
//                    rdbPartidas = false;
//                    rdbUnitarias = false;
//                    rdbVolumen = false;
//                    rdbTransito = false;
//                }
//                else if (objEventoD.BuscaSiExisteIDCheckpoint(163, IDReferencia))
//                {
//                    rdbVolumen = true;
//                    rdbMiscelaneas = false;
//                    rdbPartidas = false;
//                    rdbUnitarias = false;
//                    rdbTransito = false;
//                }
//                else if (objEventoD.BuscaSiExisteIDCheckpoint(528, IDReferencia))
//                {
//                    rdbTransito = true;
//                    rdbVolumen = false;
//                    rdbMiscelaneas = false;
//                    rdbPartidas = false;
//                    rdbUnitarias = false;
//                }


//                // Si existe Check prealerta.- se la asigno al mismo clasificador que tenia asignado
//                if (objEventoD.BuscaSiExisteIDCheckpoint(181, objReferencia1.IDReferencia))
//                    chkPreAlertas = true;

//                // Si existe Check Información.- se la asigno al mismo clasificador que tenia asignado
//                if (objEventoD.BuscaSiExisteIDCheckpoint(12, objReferencia1.IDReferencia))
//                {
//                    chkPreAlertas = true;
//                    if (objReferencia1.IDGrupo != 0)
//                    {
//                        objTgrupo = objGrupoD.Buscar(objReferencia1.IDGrupo);
//                        if (!IsNothing(objTgrupo))
//                        {
//                            if (objTgrupo.IdOficina == IdOficina)
//                                IDGrupodeTrabajo = objReferencia1.IDGrupo;
//                        }
//                    }
//                }

//                // Si existe Check Dias Anteriores.- se la asigno al mismo clasificador que tenia asignado
//                if (objEventoD.BuscaSiExisteIDCheckpoint(196, objReferencia1.IDReferencia))
//                {
//                    chkDiasAnteriores = true;
//                    if (objReferencia1.IDGrupo != 0)
//                    {
//                        objTgrupo = objGrupoD.Buscar(objReferencia1.IDGrupo);
//                        if (!IsNothing(objTgrupo))
//                        {
//                            if (objTgrupo.IdOficina == IdOficina)
//                                IDGrupodeTrabajo = objReferencia1.IDGrupo;
//                        }
//                    }
//                }

//                Referencias objReferencia = new Referencias();
//                objReferencia = objReferencia1;

//                Referencias objReferenciaNew = new Referencias();

//                objReferenciaNew = LlenaObjReferencia(objReferencia, GuiaHouse, IDGrupodeTrabajo, vidCliente, chkbSubdivision, oficinaObj, 1);
//                if (vidMasterConsol != 0)
//                    objReferenciaNew.IDMasterConsol = vidMasterConsol;

//                vidCliente = objReferencia.IDCliente;
//                objReferenciaNew.IDGrupo = IDGrupodeTrabajo;
//                objReferenciaNew.ReferenciaDestinatario = "";

//                objReferenciaD1.Modificar(objReferenciaNew);

//                // Eventos

//                if (RevDsicrepancia)
//                {
//                    ControldeEventosRepository objEventosDa = new ControldeEventosRepository(_configuration);

//                    // Checkpoint GUIA REVISADA POR GLOBAL
//                    ControldeEventos lEventos = new ControldeEventos(580, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                    objEventosDa.InsertarEventoPocket(lEventos, IdDepartamento, IdOficina, false, "", IDDatosDeEmpresa);
//                    Thread.Sleep(1000);
//                    // Checkpoint GLOBAL TURNA A FORMAL
//                    ControldeEventos lEventosDos = new ControldeEventos(587, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                    objEventosDa.InsertarEventoPocket(lEventosDos, IdDepartamento, IdOficina, false, "", IDDatosDeEmpresa);
//                }

//                ControldeEventosRepository objEventosD = new ControldeEventosRepository(_configuration);

//                int idEvento;
//                // Dim objEventos123 As New ControldeEventos(123, objReferencia.IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"))
//                // Dim objResp As New AsignarGuiasRespuesta
//                // objResp = objEventosD.InsertarEvento(objEventos123, 9, IdOficina, False, MensajeCompuesto, IDDatosDeEmpresa)
//                // idEvento = objResp.IdEvento

//                AsignarGuias objAsignar = new AsignarGuias();
//                AsignarGuiasRespuesta reasigno = new AsignarGuiasRespuesta();
//                if (reasignar)
//                    reasigno = objAsignar.ReasignarGuia(IdUsuario, objAsignar.getUsuarioDeGrupo(IDGrupodeTrabajo), objReferencia.IDReferencia);

//                // idCheckPoint = DeterminarCheckPoint()

//                int idCheckPoint = 0;
//                if (rdbUnitarias == true)
//                    idCheckPoint = 8;

//                if (rdbPartidas == true)
//                    idCheckPoint = 9;

//                if (rdbMiscelaneas == true)
//                    idCheckPoint = 16;

//                if (rdbVolumen == true)
//                    idCheckPoint = 163;

//                if (rdbTransito == true)
//                    idCheckPoint = 528;

//                // If chkPreAlertas.Checked = True Then
//                // idCheckPoint = 181
//                // End If

//                // If chkInformaciones.Checked = True Then
//                // idCheckPoint = 12
//                // End If

//                ControldeEventos objEventos = new ControldeEventos(idCheckPoint, objReferencia.IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                idEvento = objEventosD.InsertarEvento(objEventos, IdDepartamento, IdOficina, IDDatosDeEmpresa);


//                if (chkPreAlertas == true)
//                {
//                    idCheckPoint = 181;
//                    ControldeEventos objEventos181 = new ControldeEventos(idCheckPoint, objReferencia.IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                    idEvento = objEventosD.InsertarEvento(objEventos181, IdDepartamento, IdOficina, IDDatosDeEmpresa);
//                }

//                if (chkInformaciones == true)
//                {
//                    idCheckPoint = 12;
//                    ControldeEventos objEventos12 = new ControldeEventos(idCheckPoint, objReferencia.IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                    idEvento = objEventosD.InsertarEvento(objEventos12, IdDepartamento, IdOficina, IDDatosDeEmpresa);
//                }

//                if (chkDiasAnteriores == true)
//                {
//                    idCheckPoint = 196;
//                    ControldeEventos objEventos196 = new ControldeEventos(idCheckPoint, objReferencia.IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                    idEvento = objEventosD.InsertarEvento(objEventos196, IdDepartamento, IdOficina, IDDatosDeEmpresa);
//                }

//                // Fin Eventos
//                GruposdeTrabajo objGrupo = new GruposdeTrabajo();
//                if (reasignar)
//                    objGrupo = objGrupoD.Buscar(IDGrupodeTrabajo);
//                else
//                {
//                    IDGrupodeTrabajo = objEventosD.BuscaGrupoClasi(objReferencia.IDReferencia);
//                    objGrupo = objGrupoD.Buscar(IDGrupodeTrabajo);
//                }

//                if (IsNothing(objGrupo))
//                    throw new ArgumentException("No existe el grupo asignado ");

//                // GuiaHouseWec = txtReferencia.Text.Trim

//                RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "Turnada Correctamente, al grupo" + " " + objGrupo.Nombre.Trim()+ "";
//                RespuestaImpo.BackColor = 0;
//                ControldeEventosRepository controldeDeEventosData = new ControldeEventosRepository(_configuration);

//                RespuestaImpo.Eventos = controldeDeEventosData.ObtenerUltimosEventos(GuiaHouse, IDDatosDeEmpresa, 3);
//            }

//            catch (Exception ex)
//            {
//                RespuestaImpo.Mensaje = ex.Message;
//                RespuestaImpo.TipoDeRespuesta = TipoDeRespuesta.TR_Excepcion;
//            }

//            return RespuestaImpo;
//        }
//        public async void insertarCheckpoint(int IdCheckpoint, int IDReferencia, int IdUsuario, int IdDepartamento, int IdOficina, string MensajeCompuesto, int IDDatosDeEmpresa)
//        {
//            // Insertar previo solicitado a almacen
//            AsignarGuiasRespuesta objRespuestaSA = new AsignarGuiasRespuesta();
//            ControldeEventosRepository objEventosDaSA = new ControldeEventosRepository(_configuration);
//            ControldeEventos objEventoPSA = new ControldeEventos(IdCheckpoint, IDReferencia, IdUsuario,DateTime.Parse("01/01/1900"));
//             objRespuestaSA = await objEventosDaSA.InsertarEvento(objEventoPSA, IdDepartamento, IdOficina, false, MensajeCompuesto, IDDatosDeEmpresa);
//        }
//        public bool PiezasEnMismaOficina(string NumeroReferncia)
//        {
//            bool result = false;
//            DataTable data = GetPiecesIdsByGuiaHouse(NumeroReferncia);
//            int cont = 0;
//            List<string> oficinasArribo = new List<string>();

//            foreach (DataRow row in data.Rows)
//            {
//                object oficinaArribo = row["OficinaArribo"];
//                if (oficinaArribo != DBNull.Value && oficinaArribo != null)
//                    oficinasArribo.Add(oficinaArribo.ToString());
//            }

//            if (oficinasArribo.Distinct().Count() == 1)
//                result = true;

//            return result;
//        }
//        public bool PiezasConMismaFecha(string NumeroReferncia)
//        {
//            bool result = false;
//            DataTable data = GetPiecesIdsByGuiaHouse(NumeroReferncia);
//            List<DateTime> fechasArribo = new List<DateTime>();

//            foreach (DataRow row in data.Rows)
//            {
//                object fechaArribo = row["FechaArribo"];
//                if (fechaArribo != DBNull.Value && fechaArribo != null)
//                {
//                    DateTime fechaSinHoras = System.Convert.ToDateTime(fechaArribo).Date;
//                    fechasArribo.Add(fechaSinHoras);
//                }
//            }

//            if (fechasArribo.Distinct().Count() > 1)
//                result = true;

//            return result;
//        }

//        public Referencias CrearReferenciaParaOficinaCMF(string GuiaHouse, int IDDatosDeEmpresa, int IdCliente, int IdOficina, Referencias objReferencia1, ReferenciasRepository objReferenciaD1, int IdUsuario, int IDGrupodeTrabajo, bool chkbSubdivision, string prefijo)
//        {
//            CatalogoDeOficinasRepository oficinaData = new CatalogoDeOficinasRepository(_configuration);
//            var oficinaObj = oficinaData.Buscar(IdOficina);

//            Referencias objReferenciaNewFlujo = new Referencias();
//            ReferenciasRepository objReferenciaData = new ReferenciasRepository(_configuration);
//            var numDuplicas = objReferenciaData.BuscarDuplicados(GuiaHouse, IDDatosDeEmpresa);
//            if (numDuplicas > 1)
//                prefijo = prefijo + (numDuplicas - 1);
//            else if (numDuplicas == 1)
//                prefijo = prefijo;
//            else
//                prefijo = "";

//            var newGuiaHouse = prefijo + GuiaHouse;
//            objReferenciaNewFlujo = LlenaObjReferencia(newGuiaHouse, IdUsuario, IDGrupodeTrabajo, IdCliente, chkbSubdivision, oficinaObj, 1);
//            var IDReferencia = objReferenciaD1.Insertar(objReferenciaNewFlujo, IDDatosDeEmpresa);
//            if (objReferencia1!=null)
//                objReferenciaData.RegistarControlReferencias(objReferencia1.IDReferencia, IDReferencia);
//            objReferencia1 = objReferenciaD1.Buscar(IDReferencia, IDDatosDeEmpresa);

//            return objReferencia1;
//        }
//        public string PieceIdsOficinaGrupo(string GuiaHouse)
//        {
//            PieceIData pieceidData = new PieceIData();
//            DataTable data = pieceidData.PieceIdsOficinaGrupo(GuiaHouse);
//            var mensaje = string.Empty;
//            foreach (DataRow row in data.Rows)
//                mensaje = mensaje + (Convert.ToInt32(row("Piezas")).ToString()) + " pieza(s) en oficina " + row("OficinaArribo") + ". ";
//            return mensaje;
//        }
//        public bool ValidaGuias(string No_Guia, string Identificador)
//        {
//            string numero;
//            int Digito_calculado;
//            string Digito_verificador;

//            switch (Identificador)
//            {
//                case "H":
//                    {
//                        if (Strings.Len(No_Guia) < 10)
//                            return false;

//                        if (Strings.Len(No_Guia) > 10)
//                            No_Guia = Strings.Right(No_Guia, 10);

//                        numero = Strings.Mid(No_Guia, 1, 9);
//                        Digito_verificador = Strings.Mid(No_Guia, 10, 1);
//                        Digito_calculado = System.Convert.ToInt32(numero) % 7;

//                        if (System.Convert.ToInt32(Digito_verificador) == Digito_calculado)
//                            return true;
//                        else
//                            return false;
//                        break;
//                    }

//                case "M":
//                    {
//                        return true;
//                    }

//                default:
//                    {
//                        return false;
//                    }
//            }
//        }




//        public void EliminarCove(Referencias objRefe, CatalogoDeUsuarios GObjUsuario)
//        {
//            try
//            {
//                Factura SaaioFacturData = new Factura(objRefe.CapturaenCasa);
//                SaaioFactur SAAIO_FACTUR = new SaaioFactur();

//                var MyCons_Fact = SaaioFacturData.EXTRAE_MAX_CONS_FACT(objRefe.NumeroDeReferencia);

//                SAAIO_FACTUR = SaaioFacturData.Buscar(objRefe.NumeroDeReferencia, MyCons_Fact);



//                FacturasCove objFactCove = new FacturasCove();
//                FacturasCoveData objFactCoveD = new FacturasCoveData();
//                objFactCove = objFactCoveD.Buscar(objRefe.IDReferencia, MyCons_Fact);

//                dat_DODA objDodaD = new dat_DODA();
//                ent_DODA objDoda = new ent_DODA();
//                objDoda = objDodaD.BuscarRemesa(objRefe.IDReferencia, SAAIO_FACTUR.NUM_REM);
//                if (!IsNothing(objDoda))
//                    throw new ArgumentException("El Cove" + objFactCove.NumeroDeCOVE.Trim()+ "se encuentra en el número de integración " + objDoda._N_Integracion + " por lo cual no se puede eliminar");


//                SaaioPedime objPedime = new SaaioPedime();
//                SaaioPedimeData objPedimeD = new SaaioPedimeData();
//                objPedime = objPedimeD.Buscar(objRefe.NumeroDeReferencia);
//                if (!IsNothing(objPedime))
//                {
//                    if (objPedime.FIR_ELEC == "")
//                    {
//                        var nubePrepago = new NubePrepago();
//                        bool Relacion = nubePrepago.esRelacion(objPedime, objRefe.NumeroDeReferencia, SAAIO_FACTUR.NUM_REM, GObjUsuario);

//                        int id;

//                        if (Relacion)
//                            id = objFactCoveD.EliminarRemesa(objRefe.IDReferencia, Convert.ToInt32(SAAIO_FACTUR.NUM_REM));
//                        else
//                            id = objFactCoveD.Eliminar(objRefe.IDReferencia, MyCons_Fact);
//                    }
//                    else
//                        throw new Exception("Imposible eliminar un COVE cuando el pedimento ya esta validado");
//                }
//            }

//            catch (Exception ex)
//            {
//            }
//        }

//        public Referencias LlenaObjReferencia(string Referencia, int IdUsuario, int lIdGrupo, int IdCliente, bool subDivision, Entities.EntitiesCatalogos.CatalogoDeOficinas Oficina, int Operacion)
//        {
//            Referencias objGuardaReferencia = new Referencias();
//            objGuardaReferencia.IDReferencia = 0;
//            objGuardaReferencia.NumeroDeReferencia = Referencia;
//            objGuardaReferencia.FechaApertura = DateTime.Now;

//            objGuardaReferencia.AduanaEntrada = Oficina.AduEntr;
//            objGuardaReferencia.AduanaDespacho = Oficina.AduDesp;
//            objGuardaReferencia.Patente = Oficina.PatenteDefault;
//            if (IdCliente == 0)
//                objGuardaReferencia.IDCliente = 17169;
//            else
//                objGuardaReferencia.IDCliente = IdCliente;
//            objGuardaReferencia.Operacion = Operacion;
//            objGuardaReferencia.IdDuenoDeLaReferencia = IdUsuario;
//            objGuardaReferencia.Subdivision = subDivision;
//            objGuardaReferencia.IdOficina = Oficina.IdOficina;
//            objGuardaReferencia.PendientePorRectificar = false;
//            objGuardaReferencia.IdClienteDestinatario = 0;
//            objGuardaReferencia.ReferenciaDestinatario = "";
//            objGuardaReferencia.IDGrupo = lIdGrupo;


//            return objGuardaReferencia;
//        }


//        public Referencias LlenaObjReferencia(Referencias objRefe, string Referencia, int lIdGrupo, int IdCliente, bool subDivision, Entities.EntitiesCatalogos.CatalogoDeOficinas Oficina, int Operacion)
//        {
//            objRefe.NumeroDeReferencia = Referencia;
//            objRefe.FechaApertura = DateTime.Now;

//            objRefe.AduanaEntrada = Oficina.AduEntr;
//            objRefe.AduanaDespacho = Oficina.AduDesp;
//            objRefe.Patente = Oficina.PatenteDefault;
//            if (IdCliente == 0)
//                objRefe.IDCliente = 17169;
//            else
//                objRefe.IDCliente = IdCliente;
//            objRefe.Operacion = Operacion;
//            objRefe.IdDuenoDeLaReferencia = 1;
//            objRefe.Subdivision = subDivision;
//            objRefe.IdOficina = Oficina.IdOficina;
//            objRefe.PendientePorRectificar = false;
//            objRefe.IdClienteDestinatario = 0;
//            objRefe.ReferenciaDestinatario = "";
//            objRefe.IDGrupo = lIdGrupo;


//            return objRefe;
//        }

//        public void Precaptura(Referencias objReferencia, int IdUsuario, int IdOficina, int IDDatosDeEmpresa, int DeptoPrevio)
//        {
//            int departamentoActual = DeptoPrevio;

//            if (departamentoActual == 0)
//            {
//                ControldeEventosRepository objControldeEventosD = new ControldeEventosRepository(_configuration);
//                departamentoActual = objControldeEventosD.BuscaDepartamentoActual(objReferencia.IDReferencia, IdOficina);
//            }

//            if (departamentoActual == 54)
//            {
//                ControldeEventosRepository objEventoD = new ControldeEventosRepository(_configuration);
//                // Busca  436	PRECAPTURA RECIBE GUÍA DE CMF
//                if (objEventoD.BuscaSiExisteIDCheckpoint(436, objReferencia.IDReferencia))
//                {
//                    // 487	PRIC	PRECAPTURA INICIA CAPTURA
//                    if (objEventoD.BuscaSiExisteIDCheckpoint(487, objReferencia.IDReferencia) == false)
//                    {
//                        // 486	PRDS	PRECAPTURA DESACTIVADA
//                        if (objEventoD.BuscaSiExisteIDCheckpoint(485, objReferencia.IDReferencia) == false)
//                        {
//                            ControldeEventos objEventos486 = new ControldeEventos(486, objReferencia.IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                            objEventoD.InsertarEvento(objEventos486, 54, IdOficina, IDDatosDeEmpresa);
//                        }
//                    }
//                    else
//                        // si tiene termino de precaptura cancelo precaptura
//                        if (objEventoD.BuscaSiExisteIDCheckpoint(485, objReferencia.IDReferencia) == true)
//                    {
//                        ControldeEventos objEventos486 = new ControldeEventos(486, objReferencia.IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                        objEventoD.InsertarEvento(objEventos486, 54, IdOficina, IDDatosDeEmpresa);
//                    }
//                }
//            }
//        }

//        public void NotiSinPrevio(Referencias objReferencia, int IdUsuario, int IdOficina, int IDDatosDeEmpresa, int DeptoPrevio)
//        {
//            int departamentoActual = DeptoPrevio;

//            if (departamentoActual == 0)
//            {
//                ControldeEventosRepository objControldeEventosD = new ControldeEventosRepository(_configuration);
//                departamentoActual = objControldeEventosD.BuscaDepartamentoActual(objReferencia.IDReferencia, IdOficina);
//            }




//            if ((departamentoActual == 51 | departamentoActual == 18))
//            {
//                AsignaciondeGuias objAsignacionNoti = new AsignaciondeGuias();
//                AsignaciondeGuiasData objAsignacionD = new AsignaciondeGuiasData();

//                objAsignacionNoti = objAsignacionD.BuscarUltimoAsignado(objReferencia.IDReferencia, departamentoActual, IdOficina);

//                if (departamentoActual == 51)
//                {
//                    // CANCELAR NOTI SIN PREVIO
//                    ControldeEventosRepository objEventoD = new ControldeEventosRepository(_configuration);
//                    ControldeEventos objEventos486 = new ControldeEventos(691, objReferencia.IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                    objEventoD.InsertarEvento(objEventos486, 51, IdOficina, IDDatosDeEmpresa);
//                }
//                if (departamentoActual == 18)
//                {
//                    // CANCELAR NOTIFICACION
//                    ControldeEventosRepository objEventoD = new ControldeEventosRepository(_configuration);
//                    ControldeEventos objEventos486 = new ControldeEventos(732, objReferencia.IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                    objEventoD.InsertarEvento(objEventos486, 18, IdOficina, IDDatosDeEmpresa);
//                }

//                try
//                {
//                    // ENVIO DE EMAIL
//                    if (!IsNothing(objAsignacionNoti))
//                        BultosCompletosNotificacion(objAsignacionNoti.idUsuarioAsignado, objReferencia.IDReferencia);
//                }
//                catch (Exception ex)
//                {
//                }
//            }
//        }
//        public DataTable LlenardgvCartadeInstrucciones(int IdCliente, int IdOfinina)
//        {
//            DataTable dtb = new DataTable();
//            CartadeInstruccionesData objloadCartadeInstruccionesD = new CartadeInstruccionesData();

//            try
//            {
//                dtb = objloadCartadeInstruccionesD.LoadDvgCartadeInstruccionesForCambioValor(IdCliente, IdOfinina);
//            }
//            catch (Exception ex)
//            {
//            }

//            return dtb;
//        }
//        public DataTable GetPiecesIdsByGuiaHouse(string NumeroDeReferencia)
//        {
//            DataTable dtb = new DataTable();
//            PieceIData objPieceIData = new PieceIData();

//            try
//            {
//                dtb = objPieceIData.GetPiecesIdsByGuiaHouse(NumeroDeReferencia);
//            }
//            catch (Exception ex)
//            {
//            }

//            return dtb;
//        }

//        private void CrearMasterPorCustomAlert(CustomsAlerts objCustomsAlerts, string NumeroDeReferencia)
//        {
//            var masteraCrear = objCustomsAlerts.GuiaMaster;
//            if (masteraCrear.Length <= 11)
//                masteraCrear = Mid(masteraCrear, 1, 3) + "-" + Mid(masteraCrear, 4, 8);

//            SaaioGuias objSaaioGuias = new SaaioGuias();
//            SaaioGuiasData objSaaioGuiasD = new SaaioGuiasData();

//            objSaaioGuias.GUIA = masteraCrear;
//            objSaaioGuias.IDE_MH = "M";
//            objSaaioGuias.NUM_REFE = NumeroDeReferencia;
//            objSaaioGuias.CONS_GUIA = 0;

//            objSaaioGuiasD.Insertar(objSaaioGuias);
//        }

//        private void CrearMasterPorWEC(Entidades.SDTConsultaWec respuestaWec, string NumeroDeReferencia)
//        {
//            var masteraCrear = respuestaWec.GuiaMaster;
//            if (masteraCrear.Length <= 11)
//                masteraCrear = Mid(masteraCrear, 1, 3) + "-" + Mid(masteraCrear, 4, 8);

//            SaaioGuias objSaaioGuias = new SaaioGuias();
//            SaaioGuiasData objSaaioGuiasD = new SaaioGuiasData();

//            objSaaioGuias.GUIA = masteraCrear;
//            objSaaioGuias.IDE_MH = "M";
//            objSaaioGuias.NUM_REFE = NumeroDeReferencia;
//            objSaaioGuias.CONS_GUIA = 0;

//            objSaaioGuiasD.Insertar(objSaaioGuias);
//        }

//        public DataTable GetGuiasMaster(int Operacion, int IdOficina, int IdDatosEmpresa)
//        {
//            DataTable dtb = new DataTable();
//            CatalogodeMasterData objCatalogoMasterData = new CatalogodeMasterData();

//            try
//            {
//                dtb = objCatalogoMasterData.GetGuiasMaster(Operacion, IdOficina, IdDatosEmpresa);
//            }
//            catch (Exception ex)
//            {
//            }

//            return dtb;
//        }
//        public DataTable GetReportControlAsignacionAifa(string GuiaMaster)
//        {
//            DataTable dtb = new DataTable();
//            CatalogodeMasterData objCatalogoMasterData = new CatalogodeMasterData();

//            try
//            {
//                dtb = objCatalogoMasterData.GetReportControlAsignacionAifa(GuiaMaster);
//            }
//            catch (Exception ex)
//            {
//            }

//            return dtb;
//        }

//        public async Task<TurnadoImpoR> ProcesarRespaldo(string GuiaHouse, int IdUsuario, int IDDatosDeEmpresa, int IdOficina, int IdDepartamento, string Guia2d, bool rdbUnitarias, bool rdbPartidas, bool rdbMiscelaneas, bool rdbVolumen, bool rdbTransito, bool chkPreAlertas, bool chkDiasAnteriores, bool chkInformaciones, bool validaNoManisfestado, bool validaEncargoCliente, int avisarDiffAgenteAdu, bool chkbSubdivision, int IDGrupodeTrabajoSelected, int validaNubePrepago, CatalogoDeUsuarios ObjUsuario, bool RevDsicrepanciaGP, bool chkFis, List<string> piecesIds, bool reasignar)
//        {
//            TurnadoImpoR RespuestaImpo = new TurnadoImpoR();
//            try
//            {
//                reasignar = GetAutoFunctionalityValue(reasignar, IdOficina);

//                int IDGrupodeTrabajo = IDGrupodeTrabajoSelected;
//                int vidCliente = 0;
//                int vidMasterConsol = 0;
//                int IDReferencia;

//                var guiaParser = new Guia2dParser();
//                var guia2Data = new Guia2dData();

//                var oficinaData = new CatalogoDeOficinasData();
//                var oficinaObj = oficinaData.Buscar(IdOficina);

//                if (IDDatosDeEmpresa == 1)
//                {
//                    var funcionalidadesData = new ActivarFuncionalidadesData();
//                    var funcionalidades = funcionalidadesData.BuscarPorOficina("ESCANEO POR PIECE ID", IdOficina);
//                    if (!IsNothing(funcionalidades))
//                    {
//                        if (funcionalidades.Activo)
//                        {
//                            var pieceIdData = new PieceIData();
//                            var pieceObj = pieceIdData.BuscarReferenciaPorPieceId(GuiaHouse);

//                            pieceIdData.ValidaPieceIdDuplicado(pieceObj.PieceID);

//                            GuiaHouse = pieceObj.NumeroDeGuia;
//                        }
//                    }



//                    GuiaHouse = Strings.Mid(GuiaHouse.Trim(), Strings.Len(GuiaHouse.Trim()) - 9, 10);

//                    Validaciones objVal = new Validaciones();
//                    if (objVal.ValidaGuias(GuiaHouse.Trim(), "H") == false)
//                    {
//                        RespuestaImpo.TipoDeRespuesta = TipoDeRespuesta.TR_Error;
//                        RespuestaImpo.Mensaje = "El número de referencia no es válido para DHL";
//                        return RespuestaImpo;
//                    }
//                }

//                if (IDDatosDeEmpresa == 2)
//                {
//                    if (Guia2d != "")
//                    {
//                        guia2Data = guiaParser.getData(Guia2d);
//                        try
//                        {
//                            var guiaHouse2d = new CatalogoDeUsuarioData2();
//                            // guiaHouse2d.InsertGuia2d(GuiaHouse, Guia2d, guia2Data.Numero, guia2Data.Peso)
//                            guiaHouse2d.InsertGuia2d(GuiaHouse, guia2Data.GuiaMaster, Guia2d, guia2Data.Numero, guia2Data.Peso, guia2Data.Descripcion, guia2Data.Valor, guia2Data.ClavePaisDestino, guia2Data.ClientName, guia2Data.Address);
//                        }

//                        catch (Exception ex)
//                        {
//                        }

//                        if (!IsNothing(guia2Data.GuiaMaster))
//                            GuiaHouse = guia2Data.GuiaMaster;
//                    }

//                    CustomAlertBaby objCustomAlertsBAbyD = new CustomAlertBaby();


//                    CustomsAlerts objCustomAlertsBabyExiste = new CustomsAlerts();
//                    string GuiaBabyHouse = "";
//                    objCustomAlertsBabyExiste = objCustomAlertsBAbyD.BuscarPorGuiaBaby(GuiaHouse.Trim());
//                    if (!IsNothing(objCustomAlertsBabyExiste))
//                    {
//                        if (!IsNothing(objCustomAlertsBabyExiste.GuiaMaster))
//                            GuiaHouse = objCustomAlertsBabyExiste.GuiaMaster;
//                    }

//                    if (Strings.Len(GuiaHouse.Trim()) == 10)
//                    {
//                        if (ValidaGuias(GuiaHouse.Trim(), "H") == true)
//                            throw new ArgumentException("La guia que esta procesando no es de FedEx");
//                    }
//                    CustomsAlerts objCA = new CustomsAlerts();
//                    CustomsAlertsData objCAD = new CustomsAlertsData();
//                    objCA = objCAD.BuscarPorGuiaHouse(GuiaHouse.Trim(), IDDatosDeEmpresa);
//                    if (IsNothing(objCA))
//                    {
//                        // If MessageBox.Show("El número de guia no se encuentra manifestado, ¿desea continuar?", "Alerta", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
//                        if (validaNoManisfestado == true)
//                        {
//                            BitacoraGeneral objBit = new BitacoraGeneral();
//                            BitacoraGeneralData objBitD = new BitacoraGeneralData();

//                            objBit.Descripcion = "Guia House sin manifiesto :" + GuiaHouse.ToUpper();
//                            objBit.IdUsuario = IdUsuario;
//                            objBit.IdReferencia = 0;
//                            objBit.Modulo = "frmTurnadodeGuias";

//                            objBitD.Insertar(objBit);
//                        }
//                        else
//                        {
//                            RespuestaImpo.AvisarNoManifestado = true;
//                            RespuestaImpo.Mensaje = "El número de guía no se encuentra manifestado, ¿desea continuar?";
//                            return RespuestaImpo;
//                        }
//                    }

//                    if (oficinaObj.OperacionDefault == 1)
//                    {
//                        CatalogoDeClientesExpertti objCliente = new CatalogoDeClientesExpertti();
//                        CatalogoDeClientesExperttiData objClienteD = new CatalogoDeClientesExperttiData();
//                        objCliente = objClienteD.Buscar(objCA.IdCliente);
//                        if (!IsNothing(objCliente))
//                        {
//                            CatalogodeEncargoConferidoData objEncargoConferidoD = new CatalogodeEncargoConferidoData();
//                            if (objEncargoConferidoD.ExisteEncargo(objCA.IdCliente, oficinaObj.PatenteDefault) == false)
//                            {
//                                // If MessageBox.Show("No existe encargo conferido para el cliente " & objCliente.Nombre.Trim()& ", ¿desea continuar?", "Alerta", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.No Then
//                                // Exit Sub
//                                // End If

//                                if (validaEncargoCliente == false)
//                                {
//                                    RespuestaImpo.AvisarEncargoCliente = true;
//                                    RespuestaImpo.Mensaje = "No existe encargo conferido para el cliente " + objCliente.Nombre.Trim()+ ", ¿desea continuar?";
//                                    return RespuestaImpo;
//                                }
//                            }
//                        }
//                    }
//                }

//                IDGrupodeTrabajo = IDGrupodeTrabajoSelected;

//                GruposdeTrabajoData objGrupoD = new GruposdeTrabajoData();

//                GuiasQueNoseDebenLiberar objGuiasqueNoseDebenLiberar = new GuiasQueNoseDebenLiberar();
//                GuiasqueNoseDebenLiberarData objGuiasqueNoseDebenLiberarD = new GuiasqueNoseDebenLiberarData();

//                objGuiasqueNoseDebenLiberar = objGuiasqueNoseDebenLiberarD.Buscar(GuiaHouse);

//                if (!IsNothing(objGuiasqueNoseDebenLiberar))
//                {
//                    if (objGuiasqueNoseDebenLiberar.TodaslasAreas == true)
//                        throw new Exception("Esta guía no debe ser liberada, por ninguna Area :" + objGuiasqueNoseDebenLiberar.Motivo.Trim);
//                    else if (objGuiasqueNoseDebenLiberar.IDDepartamento == 9)
//                        throw new Exception("Esta guía No debe ser liberada, por el departamento de Clasificación:" + objGuiasqueNoseDebenLiberar.Motivo.Trim);
//                }


//                DetalleDeRelacionEnTransito objTransito = new DetalleDeRelacionEnTransito();
//                EnvioEnTransitoData objTransitoD = new EnvioEnTransitoData();
//                objTransito = objTransitoD.Buscar(GuiaHouse);

//                if (!IsNothing(objTransito))
//                {
//                    if (objTransito.IdUsuarioScaneaLlegada == 0)
//                        throw new ArgumentException("Se detectó guía " + GuiaHouse + " en tránsito, por favor ingrese el escaneo de la llegada primero.");
//                }





//                int IdCategoria;
//                // Tomar decisiones en base al customalerts

//                CustomsAlertsData objCustomsAlertsD = new CustomsAlertsData();

//                CustomerMasterFile objCMF = new CustomerMasterFile();
//                CustomerMasterFileData objCMFD = new CustomerMasterFileData();
//                objCMF = objCMFD.Buscar(GuiaHouse);

//                CustomsAlerts objCustomsAlerts = new CustomsAlerts();
//                objCustomsAlerts = objCustomsAlertsD.BuscarPorGuiaHouse(GuiaHouse, IDDatosDeEmpresa);

//                string GuiaMasterCMF = string.Empty;
//                CatalogodeMaster objMasterCMF = new CatalogodeMaster();
//                CatalogodeMasterData objMasterD = new CatalogodeMasterData();

//                bool validaPieceIds = false;
//                int numberPieceIds = 0;

//                if (IsNothing(objCMF))
//                {
//                    if (!IsNothing(objCustomsAlerts))
//                    {
//                        vidCliente = objCustomsAlerts.IdCliente;
//                        IdCategoria = objCustomsAlerts.IdCategoria;

//                        GuiaMasterCMF = Mid(objCustomsAlerts.GuiaMaster.Trim, 1, 3) + Mid(objCustomsAlerts.GuiaMaster.Trim, 4, 8);
//                        if (IDDatosDeEmpresa == 1 | IDDatosDeEmpresa == 2)
//                        {
//                            if (objCustomsAlerts.Piezas > 1)
//                            {
//                                numberPieceIds = objCustomsAlerts.Piezas;
//                                validaPieceIds = true;
//                            }
//                        }

//                        objMasterCMF = objMasterD.Buscar(GuiaMasterCMF);

//                        if (!IsNothing(objTransito))
//                        {
//                            if (IdOficina != objTransito.IdOficinaLlegada)
//                            {
//                                var oficinaTemp = oficinaData.Buscar(objTransito.IdOficinaLlegada);
//                                throw new ArgumentException("La oficina de llegada de transito (" + oficinaTemp.nombre + ") no coincide con la oficina del usuario (" + oficinaObj.nombre + "), favor de verificar");
//                            }
//                        }
//                        else if (!IsNothing(objMasterCMF))
//                        {
//                            if (objMasterCMF.IdOficina != IdOficina)
//                            {
//                                var oficinaTemp = oficinaData.Buscar(objMasterCMF.IdOficina);
//                                throw new ArgumentException("La oficina de la master (" + oficinaTemp.nombre + ") no coincide con la oficina del usuario (" + oficinaObj.nombre + "), favor de verificar");
//                            }
//                        }
//                    }
//                    else
//                    {
//                        vidCliente = 17169;
//                        IdCategoria = 0;
//                    }
//                }
//                else
//                {
//                    vidCliente = objCMF.IdCliente;
//                    IdCategoria = objCMF.IdCategoria;


//                    GuiaMasterCMF = Mid(objCMF.GuiaMaster.Trim, 1, 3) + Mid(objCMF.GuiaMaster.Trim, 4, 8);

//                    // 2024-05-17'
//                    // If IDDatosDeEmpresa = 1 Then
//                    // If objCMF.Piezas > 1 Then
//                    // numberPieceIds = objCMF.Piezas
//                    // validaPieceIds = True
//                    // End If
//                    // End If

//                    if (IDDatosDeEmpresa == 1)
//                    {
//                        if (objCMF.Piezas > 1)
//                        {
//                            numberPieceIds = objCMF.Piezas;
//                            validaPieceIds = true;
//                        }
//                    }

//                    if (IDDatosDeEmpresa == 2)
//                    {
//                        if (objCustomsAlerts.Piezas > 1)
//                        {
//                            numberPieceIds = objCustomsAlerts.Piezas;
//                            validaPieceIds = true;
//                        }
//                    }
//                    // ivbm 20-01-2023                
//                    objMasterCMF = objMasterD.Buscar(GuiaMasterCMF);

//                    if (!IsNothing(objTransito))
//                    {
//                        if (IdOficina != objTransito.IdOficinaLlegada)
//                            throw new ArgumentException("La oficina de la de llegada de transito no coincide con la oficina del usuario, favor de verificar");
//                    }
//                    else if (!IsNothing(objMasterCMF))
//                    {
//                        if (objMasterCMF.IdOficina != IdOficina)
//                            throw new ArgumentException("La oficina de la master no coincide con la oficina del usuario, favor de verificar");
//                    }
//                }

//                Referencias objReferencia1 = new Referencias();
//                ReferenciasRepository objReferenciaD1 = new ReferenciasRepository();
//                objReferencia1 = objReferenciaD1.Buscar(GuiaHouse, IDDatosDeEmpresa);
//                // If Not IsNothing(objReferencia1) Then
//                // IDReferencia = objReferencia1.IDReferencia
//                // End If





//                // __________FAST MORNING__________________
//                if (!IsNothing(objReferencia1))
//                {
//                    IDReferencia = objReferencia1.IDReferencia;

//                    if (objReferencia1.IDDatosDeEmpresa == 1)
//                    {
//                        CatalogoDeOficinas objOficina = new CatalogoDeOficinas();
//                        CatalogoDeOficinasData objOficinaD = new CatalogoDeOficinasData();

//                        if (!IsNothing(objTransito))
//                        {
//                            objOficina = objOficinaD.Buscar(objTransito.IdOficinaLlegada);

//                            if (IsNothing(objOficina))
//                                throw new ArgumentException("La oficina de la llegada de transito no es correcta, favor de verificar antes de seguir con el proceso");

//                            if (!IsNothing(objMasterCMF))
//                                objReferencia1.IDMasterConsol = objMasterCMF.IDMasterConsol;

//                            if (objReferencia1.IdOficina != objTransito.IdOficinaLlegada)
//                            {
//                                // TODO REGENERAR EL ARCHIVO DE PROFORMA
//                                NubePrepago nube = new NubePrepago();
//                                nube.RegenerarProformaInsertar(objReferencia1.IDReferencia, IdUsuario, DateTime.Now.ToString());
//                            }

//                            objReferencia1.IdOficina = objTransito.IdOficinaLlegada;
//                            objReferencia1.AduanaDespacho = objOficina.AduDesp;
//                            // objReferencia1.AduanaEntrada = objOficina.AduEntr

//                            objReferenciaD1.Modificar(objReferencia1);

//                            SaaioPedime objPedime = new SaaioPedime();
//                            SaaioPedimeData objPedimeD = new SaaioPedimeData();
//                            objPedime = objPedimeD.Buscar(objReferencia1.NumeroDeReferencia);
//                            if (!IsNothing(objPedime))
//                            {
//                                objPedime.MTR_ENTR = "4";
//                                objPedime.MTR_ARRI = "7";
//                                objPedime.MTR_SALI = "7";
//                                objPedime.ADU_DESP = objOficina.AduDesp;
//                                // objPedime.ADU_ENTR = objOficina.AduEntr
//                                objPedime.CVE_REPR = objOficina.CveMant;
//                                objPedimeD.Modificar(objPedime);
//                            }
//                        }
//                        else if (!IsNothing(objMasterCMF))
//                        {
//                            objOficina = objOficinaD.Buscar(objMasterCMF.IdOficina);
//                            if (IsNothing(objOficina))
//                                throw new ArgumentException("La oficina de la master no es correcta, favor de verificar antes de seguir con el proceso");

//                            if (objReferencia1.IdOficina != objMasterCMF.IdOficina)
//                            {
//                                // TODO REGENERAR EL ARCHIVO DE PROFORMA
//                                NubePrepago nube = new NubePrepago();
//                                nube.RegenerarProformaInsertar(objReferencia1.IDReferencia, IdUsuario, DateTime.Now.ToString());
//                            }

//                            objReferencia1.IDMasterConsol = objMasterCMF.IDMasterConsol;
//                            objReferencia1.IdOficina = objMasterCMF.IdOficina;
//                            objReferencia1.AduanaDespacho = objOficina.AduDesp;
//                            objReferencia1.AduanaEntrada = objOficina.AduEntr;

//                            objReferenciaD1.Modificar(objReferencia1);

//                            SaaioPedime objPedime = new SaaioPedime();
//                            SaaioPedimeData objPedimeD = new SaaioPedimeData();
//                            objPedime = objPedimeD.Buscar(objReferencia1.NumeroDeReferencia);
//                            if (!IsNothing(objPedime))
//                            {
//                                objPedime.ADU_DESP = objOficina.AduDesp;
//                                objPedime.ADU_ENTR = objOficina.AduEntr;
//                                objPedimeD.Modificar(objPedime);
//                            }
//                        }
//                    }
//                    // ---------------IVBM 24-01-2023

//                    if (chkFis == true)
//                    {
//                        objReferenciaD1.ModificarEscaladas(GuiaHouse, 6);
//                        chkFis = false;
//                    }

//                    ControldeEventosRepository objEventoD = new ControldeEventosRepository(_configuration);

//                    // 'GUIA PRECAPTURADA PARA VALIDACIÓN Y PAGO'
//                    if (objEventoD.BuscaSiExisteIDCheckpoint(524, objReferencia1.IDReferencia))
//                    {
//                        // 'CLASIFICACIÓN DETECTA INCONSISTENCIA EN MERCANCÍA SE CANCELA PRECAPTURA'
//                        if (objEventoD.BuscaSiExisteIDCheckpoint(525, objReferencia1.IDReferencia) == false)
//                        {

//                            // Clasificacion calzado chino
//                            if (IsNothing(objCMF))
//                            {
//                                if (objCustomsAlerts.IdTipodePedimento == 21)
//                                    throw new Exception($"La referencia {objReferencia1.NumeroDeReferencia} corresponde a un pedimento de CALZADO, posiblemente de origen chino.");
//                            }
//                            else if (objCMF.idTipodePedimento == 21)
//                                throw new Exception($"La referencia {objReferencia1.NumeroDeReferencia} corresponde a un pedimento de CALZADO, posiblemente de origen chino.");




//                            if (validaNubePrepago == 0)
//                            {
//                                RespuestaImpo.AvisarNubePrepago = true;
//                                RespuestaImpo.Mensaje = "¿Se detectó guía tipo Nube prepago quieres procesar?";
//                                return RespuestaImpo;
//                            }

//                            if (validaNubePrepago == 1)
//                            {
//                                var nubePrepago = new NubePrepago();

//                                CatalogodeMaster objGuiaMaster;

//                                // VALIDACIONES

//                                // VALIDAR QUE NO ESTE EN OTRO DEPARTAMENTO
//                                int departamentoActual = 0;
//                                int departamentoDestino = 0;
//                                ControldeEventosRepository controldeEventosData = new ControldeEventosRepository(_configuration);
//                                departamentoActual = controldeEventosData.BuscaDepartamentoActual(objReferencia1.IDReferencia, IdOficina);
//                                if (departamentoActual == 0)
//                                    departamentoActual = IdDepartamento;
//                                if (departamentoActual == 54)
//                                    Precaptura(objReferencia1, IdUsuario, IdOficina, IDDatosDeEmpresa, 0);

//                                if (departamentoActual > 0)
//                                {
//                                    CatalogodeCheckPoints objCatCheck = new CatalogodeCheckPoints();
//                                    CatalogodeCheckPointsData objCatCheckD = new CatalogodeCheckPointsData();
//                                    objCatCheck = objCatCheckD.BuscarPorDepto(526, IdOficina, departamentoActual);

//                                    CatalogoDepartamentos objDep = new CatalogoDepartamentos();
//                                    CatalogoDepartamentosData objDepD = new CatalogoDepartamentosData();
//                                    objDep = objDepD.Buscar(departamentoActual);

//                                    if (IsNothing(objCatCheck))
//                                        throw new Exception("La referencia está asignada al departamento " + Constants.vbCrLf + objDep.NombreDepartamento.Trim()+ " y el checkpoint es de otro departamento");
//                                }

//                                // VALIDACION DE GUIA DUPLICADA Fecha no exceda los dos meses
//                                if (!IsNothing(objCMF))
//                                {
//                                    var mesesDiff = DateDiff(DateInterval.Month, objCMF.FechaAlta, DateTime.Now);
//                                    if (mesesDiff > 2)
//                                        throw new ArgumentException("Guía con más de dos meses de existencia, revisar si se trata de una guía duplicada.");
//                                }
//                                if (!IsNothing(objCustomsAlerts))
//                                {
//                                    var mesesDiff = DateDiff(DateInterval.Month, objCustomsAlerts.FechaDeAlta, DateTime.Now);
//                                    if (mesesDiff > 2)
//                                        throw new ArgumentException("Guía con más de dos meses de existencia, revisar si se trata de una guía duplicada.");
//                                }

//                                SaaioPedime objPedimeFastMorning = new SaaioPedime();
//                                SaaioPedimeData objPedimeDFastMorning = new SaaioPedimeData();
//                                objPedimeFastMorning = objPedimeDFastMorning.Buscar(objReferencia1.NumeroDeReferencia);

//                                if (!IsNothing(objPedimeFastMorning))
//                                {
//                                    if (objPedimeFastMorning.FIR_ELEC != "")
//                                        throw new ArgumentException("Referencia " + objReferencia1.NumeroDeReferencia + " ya tiene firma electrónica.");
//                                }

//                                // MASTER 
//                                var saaoi_guia = new SaaioGuiasData();

//                                var guiaMaster = saaoi_guia.CargarMaster(objReferencia1.NumeroDeReferencia);

//                                // VALIDA MISMA MASTER
//                                if (!IsNothing(guiaMaster))
//                                {
//                                    if (guiaMaster.Count > 1)
//                                    {
//                                        var GuiaGuion = guiaMaster.Find(p => p.GUIA.Contains("-"));
//                                        if (!IsNothing(GuiaGuion))
//                                        {
//                                            var GuiaSin = GuiaGuion.GUIA.Replace("-", "");
//                                            foreach (SaaioGuias item in guiaMaster)
//                                            {
//                                                if (item.GUIA == GuiaSin)
//                                                    saaoi_guia.EliminarGuia(item.NUM_REFE, item.GUIA, item.IDE_MH);
//                                            }
//                                        }
//                                    }
//                                }

//                                var existeMasterEnWec = false;
//                                WebApiWec objws = new WebApiWec();
//                                var obsCheck = "";

//                                var resultSTD;
//                                if (objReferencia1.IdOficina == 2)
//                                    resultSTD = await objws.ConsultasWec(objReferencia1.NumeroDeReferencia, IdUsuario);


//                                try
//                                {
//                                    if (!Information.IsNothing(resultSTD))
//                                    {
//                                        if (!Information.IsNothing(resultSTD.GuiaMaster))
//                                        {
//                                            existeMasterEnWec = true;

//                                            var existeMasterEnBD = false;

//                                            if (resultSTD.Bultos == 1)
//                                            {
//                                                if (!IsNothing(guiaMaster))
//                                                {
//                                                    foreach (SaaioGuias item in guiaMaster)
//                                                    {
//                                                        var GuiaSin = item.GUIA.Replace("-", "");
//                                                        if ((item.GUIA == resultSTD.GuiaMaster || GuiaSin == resultSTD.GuiaMaster))
//                                                            existeMasterEnBD = true;
//                                                        else
//                                                            saaoi_guia.EliminarGuia(item.NUM_REFE, item.GUIA, item.IDE_MH);
//                                                    }
//                                                }
//                                            }

//                                            if (existeMasterEnBD == false)
//                                                CrearMasterPorWEC(resultSTD, objReferencia1.NumeroDeReferencia);
//                                        }
//                                    }
//                                    else if (objReferencia1.IdOficina == 2)
//                                        obsCheck = "SIN RESPUESTA DE WEC PARA SABER CR";



//                                    if (existeMasterEnWec == false)
//                                    {
//                                        // CREAMOS MASTER APARTIR DE CUSTOM ALERT
//                                        if (!IsNothing(objCustomsAlerts))
//                                        {
//                                            var existeMasterEnBD = false;

//                                            if (objCustomsAlerts.Piezas == 1)
//                                            {
//                                                if (!IsNothing(guiaMaster))
//                                                {
//                                                    foreach (SaaioGuias item in guiaMaster)
//                                                    {
//                                                        var GuiaSin = item.GUIA.Replace("-", "");
//                                                        if ((item.GUIA == objCustomsAlerts.GuiaMaster || GuiaSin == objCustomsAlerts.GuiaMaster))
//                                                            existeMasterEnBD = true;
//                                                        else
//                                                            saaoi_guia.EliminarGuia(item.NUM_REFE, item.GUIA, item.IDE_MH);
//                                                    }
//                                                }
//                                            }

//                                            if (existeMasterEnBD == false)
//                                                CrearMasterPorCustomAlert(objCustomsAlerts, objReferencia1.NumeroDeReferencia);
//                                        }
//                                    }
//                                }

//                                catch (Exception ex)
//                                {
//                                }

//                                guiaMaster = saaoi_guia.CargarMaster(objReferencia1.NumeroDeReferencia);

//                                if (!IsNothing(guiaMaster))
//                                {
//                                    if (guiaMaster.Count > 1)
//                                        // TODO INCORPORAR PANTALLA DE SELECCION DE MASTER
//                                        throw new ArgumentException("Existe mas de una master declarada, por favor procesar desde Expertti.");
//                                }
//                                else
//                                    // TODO INCORPORAR PANTALLA DE CREACION DE MASTER
//                                    throw new ArgumentException("No se detecto guía master, por favor procesar desde Expertti.");


//                                // ASIGNAMOS MASTER SI NO TIENE
//                                if (objReferencia1.IDMasterConsol == 0)
//                                {
//                                    CatalogodeMasterData objGuiaMasterD = new CatalogodeMasterData();
//                                    if (!IsNothing(objCustomsAlerts))
//                                        objGuiaMaster = objGuiaMasterD.Buscar(objCustomsAlerts.GuiaMaster.Trim);
//                                    else if (!IsNothing(guiaMaster))
//                                        objGuiaMaster = objGuiaMasterD.Buscar(guiaMaster.First.GUIA.Trim);


//                                    if (!IsNothing(objGuiaMaster))
//                                    {
//                                        objReferencia1.IDMasterConsol = objGuiaMaster.IDMasterConsol;
//                                        objReferenciaD1.Modificar(objReferencia1);
//                                    }
//                                }


//                                var guiaHouseDos = saaoi_guia.BuscarGuia(objReferencia1.NumeroDeReferencia, "H");

//                                if (IsNothing(guiaHouseDos))
//                                    // TODO INCORPORAR PANTALLA DE CORRECION DE GUIA HOUSE
//                                    throw new ArgumentException("No se detecto guía house, por favor procesar desde Expertti.");


//                                if (objReferencia1.IDMasterConsol == 0)
//                                    // TODO - Pantalla para agregar master a mano.
//                                    throw new ArgumentException("Se debe asignar una master a la referencia, por favor procesar desde Expertti.");
//                                else
//                                    objGuiaMaster = objMasterD.Buscar(objReferencia1.IDMasterConsol);

//                                var envioCOVE = false;

//                                if (!IsNothing(objPedimeFastMorning))
//                                {
//                                    if (!IsNothing(objGuiaMaster))
//                                    {
//                                        if (objPedimeFastMorning.FEC_ENTR != objGuiaMaster.FechaArriboUnitarias)
//                                        {
//                                            // Actualizamos fecha de entrada y tipo de cambio en saio pedime
//                                            CtarcTipCam objTipodeCambio = new CtarcTipCam();
//                                            CtarcTipCamData objTipodeCambioD = new CtarcTipCamData();
//                                            objTipodeCambio = objTipodeCambioD.Buscar(objGuiaMaster.FechaArriboUnitarias);
//                                            if (!IsNothing(objTipodeCambio))
//                                            {
//                                                if (objTipodeCambio.TIP_CAM == null/* TODO Change to default(_) if this is not a reference type */ )
//                                                    // TODO - Mostrar alerta.
//                                                    // MessageBox.Show("El tipo de cambio no se ha dado de alta en Expertti")
//                                                    throw new ArgumentException("El tipo de cambio no se ha dado de alta en Expertti");
//                                                else
//                                                    objPedimeFastMorning.TIP_CAMB = objTipodeCambio.TIP_CAM;
//                                            }

//                                            objPedimeFastMorning.FEC_ENTR = objGuiaMaster.FechaArriboUnitarias;
//                                            objPedimeDFastMorning.Modificar(objPedimeFastMorning);

//                                            // Eliminar COVE
//                                            EliminarCove(objReferencia1, ObjUsuario);

//                                            // Actualizar equivaliencias  STORE DE DANIEL
//                                            SaaioCoveSerExperttiData saaioCoveSerExperttiData = new SaaioCoveSerExperttiData();
//                                            saaioCoveSerExperttiData.ActualizaFechaEntrada(objReferencia1.NumeroDeReferencia, objPedimeFastMorning.FEC_ENTR);

//                                            envioCOVE = true;
//                                        }
//                                    }
//                                }



//                                SaaioIdePed objSaaioIdePed = new SaaioIdePed();
//                                SaaioIdePedData objSaaioIdePedD = new SaaioIdePedData();
//                                objSaaioIdePed = objSaaioIdePedD.Buscar(objReferencia1.NumeroDeReferencia, "CR");

//                                if (IsNothing(objSaaioIdePed))
//                                {
//                                    try
//                                    {
//                                        SaaioIdePed objIdePed = new SaaioIdePed();
//                                        objIdePed.NUM_REFE = objReferencia1.NumeroDeReferencia;
//                                        objIdePed.CVE_IDEN = "CR";
//                                        if (!Information.IsNothing(resultSTD))
//                                        {
//                                            if (!Information.IsNothing(resultSTD.GuiaMaster))
//                                                objIdePed.COM_IDEN = "263";
//                                            else
//                                                objIdePed.COM_IDEN = "12";
//                                        }
//                                        else
//                                        {
//                                            if (objReferencia1.IdOficina == 2)
//                                                objIdePed.COM_IDEN = "12";
//                                            if (objReferencia1.IdOficina == 24)
//                                                objIdePed.COM_IDEN = "296";
//                                        }
//                                        objSaaioIdePedD.Insertar(objIdePed);
//                                    }
//                                    catch (Exception ex)
//                                    {
//                                        // MessageBox.Show(ex.Message)
//                                        throw new ArgumentException("No se pudo generar identificador CR: " + ex.Message);
//                                    }
//                                }
//                                else
//                                    objSaaioIdePedD.Modificar(objReferencia1.NumeroDeReferencia, "CR", objSaaioIdePed.COM_IDEN);

//                                objSaaioIdePed = objSaaioIdePedD.Buscar(objReferencia1.NumeroDeReferencia, "CR");
//                                if (IsNothing(objSaaioIdePed))
//                                    throw new ArgumentException("No se encontró identificador CR");

//                                if (!IsNothing(objSaaioIdePed))
//                                {
//                                    DocumentosPorGuiaData objDocumentosPorGuiaD = new DocumentosPorGuiaData();

//                                    var sinonimosData = new SinonimosdeRiesgoData();
//                                    var sinonimoTextilCalzado = false;

//                                    if (!IsNothing(objCMF))
//                                        sinonimoTextilCalzado = sinonimosData.ExisteSinonimoDeRiesgoTextilesZapatos(objCMF.Descripcion);
//                                    else if (!IsNothing(objCustomsAlerts))
//                                        sinonimoTextilCalzado = sinonimosData.ExisteSinonimoDeRiesgoTextilesZapatos(objCustomsAlerts.Descripcion);




//                                    UbicacionDeArchivos objUbicacion = new UbicacionDeArchivos();
//                                    UbicacionDeArchivosData objUbicacionD = new UbicacionDeArchivosData();
//                                    objUbicacion = objUbicacionD.Buscar(121);
//                                    if (IsNothing(objUbicacion))
//                                        throw new ArgumentException("No existe ubicacion de archivos Id. 121, MisDOcumentos");

//                                    var pMisDocumentos = objUbicacion.Ubicacion + @"\" + ObjUsuario.Usuario.Trim()+ @"\ExperttiTmp\";

//                                    if (Directory.Exists(pMisDocumentos) == false)
//                                        Directory.CreateDirectory(Directory.Exists(pMisDocumentos));



//                                    if (sinonimoTextilCalzado)
//                                    {
//                                        // ENVIAR FACTURA COMERCIAL
//                                        if (Convert.ToBoolean(objDocumentosPorGuiaD.ExisteUltimoDocumentoSubido(IDReferencia, 14, 1)))
//                                        {
//                                            var documentoavucem = objDocumentosPorGuiaD.Buscar(IDReferencia, 14); // Ya me regresa el ultimo consecutivo

//                                            if (documentoavucem.idDocumento == 0)
//                                                throw new ArgumentException("No se encontro factura para enviar a VUCEM");


//                                            DigitalizadosVucem objDigitalizados = new DigitalizadosVucem();
//                                            DigitalizadosVucemData objDigitalizadosD = new DigitalizadosVucemData();

//                                            // Factura
//                                            objDigitalizados = objDigitalizadosD.Buscar(IDReferencia, 68);
//                                            if (IsNothing(objDigitalizados))
//                                            {
//                                                Thread thFactura = new Thread(() => nubePrepago.GeneraEDocument(documentoavucem.idDocumento, objReferencia1.IDReferencia, false, objReferencia1, ObjUsuario, pMisDocumentos));
//                                                thFactura.Start();
//                                            }
//                                        }
//                                    }

//                                    // CR 12 NO ESTAN EN WEC
//                                    // CR 263 ESTAN EN WEC

//                                    if (objSaaioIdePed.COM_IDEN == "263")
//                                    {
//                                        DocumentosPorGuia objDocumentosPorGuia = new DocumentosPorGuia();
//                                        if (!Convert.ToBoolean(objDocumentosPorGuiaD.ExisteUltimoDocumentoSubido(IDReferencia, 1166, 1)))
//                                        {
//                                            var resultRevalidaWec = await nubePrepago.RevalidaWec(objReferencia1, pMisDocumentos, IdUsuario, IDDatosDeEmpresa, IdDepartamento, IdOficinaGP, ObjUsuario);
//                                            if (resultRevalidaWec == 0)
//                                            {
//                                                // Throw New ArgumentException("No fue posible revalidar intente de nuevo")

//                                                // ARRIBO DE FAST MORNING - INFORMATIVO
//                                                ControldeEventos objEventoCoSalida = new ControldeEventos(535, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                                var objRespuestaFastMorning = objEventoD.InsertarEvento(objEventoCoSalida, IdDepartamento, IdOficina, false, obsCheck, IDDatosDeEmpresa);

//                                                // GUIA ARRIBADA PARA SALIDAS'
//                                                ControldeEventos objEventoCoSalida2 = new ControldeEventos(527, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                                var objRespuestaSalidas = objEventoD.InsertarEvento(objEventoCoSalida2, IdDepartamento, IdOficina, false, "No fue posible revalidar la guia en WEC", IDDatosDeEmpresa);



//                                                RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "Turnada correctamente para " + Constants.vbCrLf + "SALIDAS";
//                                                RespuestaImpo.BackColor = 0;
//                                                return RespuestaImpo;
//                                            }
//                                        }


//                                        // 1047 - GUIA HOUSE SELLADA
//                                        // 1166 - GUIA HOUSE SELLADA WEC
//                                        if ((Convert.ToBoolean(objDocumentosPorGuiaD.ExisteUltimoDocumentoSubido(IDReferencia, 1166, 1))) | (Convert.ToBoolean(objDocumentosPorGuiaD.ExisteUltimoDocumentoSubido(IDReferencia, 1047, 1))))
//                                        {
//                                            var documentoavucem = objDocumentosPorGuiaD.Buscar(IDReferencia, 1166);
//                                            if (documentoavucem.idDocumento == 0)
//                                                documentoavucem = objDocumentosPorGuiaD.Buscar(IDReferencia, 1047);

//                                            if (documentoavucem.idDocumento == 0)
//                                            {
//                                                // Throw New ArgumentException("No se encontro documento para enviar a VUCEM")
//                                                // ARRIBO DE FAST MORNING - INFORMATIVO
//                                                ControldeEventos objEventoCoSalida2 = new ControldeEventos(535, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                                var objRespuestaFastMorning2 = objEventoD.InsertarEvento(objEventoCoSalida2, IdDepartamento, IdOficina, false, obsCheck, IDDatosDeEmpresa);

//                                                // GUIA ARRIBADA PARA SALIDAS'
//                                                ControldeEventos objEventoCoSalida3 = new ControldeEventos(527, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                                var objRespuestaSalidas = objEventoD.InsertarEvento(objEventoCoSalida3, IdDepartamento, IdOficina, false, "No se encontro documento GUIA HOUSE para enviar a VUCEM", IDDatosDeEmpresa);

//                                                RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "Turnada correctamente para " + Constants.vbCrLf + "SALIDAS";
//                                                RespuestaImpo.BackColor = 0;
//                                                return RespuestaImpo;
//                                            }


//                                            DigitalizadosVucem objDigitalizados = new DigitalizadosVucem();
//                                            DigitalizadosVucemData objDigitalizadosD = new DigitalizadosVucemData();

//                                            // ARRIBO DE FAST MORNING - INFORMATIVO
//                                            ControldeEventos objEventoCoSalida = new ControldeEventos(535, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                            var objRespuestaFastMorning = objEventoD.InsertarEvento(objEventoCoSalida, IdDepartamento, IdOficina, false, obsCheck, IDDatosDeEmpresa);

//                                            // Guia aerea 
//                                            objDigitalizados = objDigitalizadosD.Buscar(IDReferencia, 30);
//                                            // If IsNothing(objDigitalizados) Then
//                                            Thread thGuia = new Thread(() => nubePrepago.GeneraEDocumentAll(documentoavucem.idTipoDocumento, objReferencia1.IDReferencia, true, objReferencia1, ObjUsuario, pMisDocumentos, envioCOVE));
//                                            thGuia.Start();
//                                            // End If




//                                            RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "en espera de eDocument para " + Constants.vbCrLf + "CONTROL DE PAGO ELECTRONICO";
//                                            RespuestaImpo.BackColor = 0;
//                                            return RespuestaImpo;
//                                        }
//                                        else
//                                        {


//                                            // Throw New ArgumentException("No se encontro guía sellada")
//                                            // ARRIBO DE FAST MORNING - INFORMATIVO
//                                            ControldeEventos objEventoCoSalida2 = new ControldeEventos(535, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                            var objRespuestaFastMorning2 = objEventoD.InsertarEvento(objEventoCoSalida2, IdDepartamento, IdOficina, false, obsCheck, IDDatosDeEmpresa);

//                                            // GUIA ARRIBADA PARA SALIDAS'
//                                            ControldeEventos objEventoCoSalida3 = new ControldeEventos(527, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                            var objRespuestaSalidas = objEventoD.InsertarEvento(objEventoCoSalida3, IdDepartamento, IdOficina, false, "No se encontro guía sellada", IDDatosDeEmpresa);

//                                            RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "Turnada correctamente para " + Constants.vbCrLf + "SALIDAS";
//                                            RespuestaImpo.BackColor = 0;
//                                            return RespuestaImpo;
//                                        }
//                                    }
//                                    else
//                                    {
//                                        if (IsNothing(objGuiaMaster))
//                                            // TODO IMPLEMENTAR PANTALLA DE ALTA DE GUIA MASTER
//                                            throw new ArgumentException("No se encontro guía master, por favor procesar desde Expertti.");
//                                        if (objGuiaMaster.ImagenMasterizacion == false)
//                                            // TODO IMPLEMENTAR PANTALLA DE ALTA DE GUIA MASTER
//                                            throw new ArgumentException("No existe sello de master, por favor procesar desde Expertti.");
//                                        if (objGuiaMaster.ImagenRevalidacion == false)

//                                            // TODO IMPLEMENTAR PANTALLA DE ALTA DE GUIA MASTER
//                                            throw new ArgumentException("No existe sello de Revalidación, por favor procesar desde Expertti.");


//                                        // Dim objDocumentosPorGuiaD As New DocumentosPorGuiaData()
//                                        if (!Convert.ToBoolean(objDocumentosPorGuiaD.ExisteUltimoDocumentoSubido(IDReferencia, 1047, 1)))
//                                        {
//                                            try
//                                            {

//                                                // Dim objDocs As New VentanillaUnica.CentralizarDocs
//                                                bool x = false;
//                                                // x = Await (objDocs.sellarGuia(IDReferencia, 0, pMisDocumentos, GObjUsuario))
//                                                // NEW SELLAMOS CON TODAS LAS MASTER DE SAAI_GUIAS Y CATALOGOGODEMASTER
//                                                x = await (nubePrepago.sellarGuia(IDReferencia, 0, pMisDocumentos, ObjUsuarioGP));
//                                            }
//                                            catch (Exception ex)
//                                            {
//                                                // Throw New ArgumentException("No fue posible sellar la Guía " & ex.Message.Trim)
//                                                // ARRIBO DE FAST MORNING - INFORMATIVO
//                                                ControldeEventos objEventoCoSalida = new ControldeEventos(535, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                                var objRespuestaFastMorning = objEventoD.InsertarEvento(objEventoCoSalida, IdDepartamento, IdOficina, false, obsCheck, IDDatosDeEmpresa);

//                                                // GUIA ARRIBADA PARA SALIDAS'
//                                                ControldeEventos objEventoCoSalida2 = new ControldeEventos(527, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                                var objRespuestaSalidas = objEventoD.InsertarEvento(objEventoCoSalida2, IdDepartamento, IdOficina, false, "No fue posible sellar la guia: " + ex.Message, IDDatosDeEmpresa);

//                                                RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "Turnada correctamente para " + Constants.vbCrLf + "SALIDAS";
//                                                RespuestaImpo.BackColor = 0;
//                                                return RespuestaImpo;
//                                            }
//                                        }

//                                        // 1047 - GUIA HOUSE SELLADA
//                                        // 1166 - GUIA HOUSE SELLADA WEC
//                                        if ((Convert.ToBoolean(objDocumentosPorGuiaD.ExisteUltimoDocumentoSubido(IDReferencia, 1166, 1))) | (Convert.ToBoolean(objDocumentosPorGuiaD.ExisteUltimoDocumentoSubido(IDReferencia, 1047, 1))))
//                                        {
//                                            var documentoavucem = objDocumentosPorGuiaD.Buscar(IDReferencia, 1166);
//                                            if (documentoavucem.idDocumento == 0)
//                                                documentoavucem = objDocumentosPorGuiaD.Buscar(IDReferencia, 1047);

//                                            if (documentoavucem.idDocumento == 0)
//                                                throw new ArgumentException("No se encontro documento para enviar a VUCEM");

//                                            DigitalizadosVucem objDigitalizados = new DigitalizadosVucem();
//                                            DigitalizadosVucemData objDigitalizadosD = new DigitalizadosVucemData();

//                                            // ARRIBO DE FAST MORNING - INFORMATIVO
//                                            ControldeEventos objEventoCoSalida = new ControldeEventos(535, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                            var objRespuestaFastMorning = objEventoD.InsertarEvento(objEventoCoSalida, IdDepartamento, IdOficina, false, obsCheck, IDDatosDeEmpresa);

//                                            Thread thGuia = new Thread(() => nubePrepago.GeneraEDocumentAll(documentoavucem.idTipoDocumento, objReferencia1.IDReferencia, true, objReferencia1, ObjUsuario, pMisDocumentos, envioCOVE));
//                                            thGuia.Start();


//                                            RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "en espera de eDocument para " + Constants.vbCrLf + "CONTROL DE PAGO ELECTRONICO";
//                                            RespuestaImpo.BackColor = 0;
//                                            return RespuestaImpo;
//                                        }
//                                        else
//                                        {
//                                            // Throw New ArgumentException("No se encontro guía sellada")
//                                            // ARRIBO DE FAST MORNING - INFORMATIVO
//                                            ControldeEventos objEventoCoSalida = new ControldeEventos(535, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                            var objRespuestaFastMorning = objEventoD.InsertarEvento(objEventoCoSalida, IdDepartamento, IdOficina, false, obsCheck, IDDatosDeEmpresa);

//                                            // GUIA ARRIBADA PARA SALIDAS'
//                                            ControldeEventos objEventoCoSalida2 = new ControldeEventos(527, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                            var objRespuestaSalidas = objEventoD.InsertarEvento(objEventoCoSalida2, IdDepartamento, IdOficina, false, "No se encontro guía sellada", IDDatosDeEmpresa);

//                                            RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "Turnada correctamente para " + Constants.vbCrLf + "SALIDAS";
//                                            RespuestaImpo.BackColor = 0;
//                                            return RespuestaImpo;
//                                        }
//                                    }
//                                }
//                            }
//                            else
//                            {
//                                // CLASIFICACIÓN DETECTA INCONSISTENCIA EN MERCANCÍA SE CANCELA PRECAPTURA'
//                                ControldeEventos objEventos525 = new ControldeEventos(525, objReferencia1.IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                objEventoD.InsertarEvento(objEventos525, 54, IdOficina, false, "", IDDatosDeEmpresa);
//                            }
//                        }
//                    }
//                }
//                // __________FAST MORNING__________________

//                // _________SUBDIVICIONES__________________

//                CatalogoDeUsuarios GObjUsuario = new CatalogoDeUsuarios();
//                CatalogoDeUsuariosData objUsuariosD = new CatalogoDeUsuariosData();
//                GObjUsuario = objUsuariosD.Buscar(IdUsuario);

//                var objPieceIData = new PieceIData();
//                var customAlertBabyData = new CustomAlertBaby();

//                if (validaPieceIds)
//                {
//                    if (piecesIds.Count == 0)
//                    {
//                        var validaPieceIdsMensaje = "Se detectó AWB con " + numberPieceIds + " piezas, favor de ingresar los Piece Ids";
//                        if (IDDatosDeEmpresa == 2)
//                            validaPieceIdsMensaje = "Se detectó Referencia con " + numberPieceIds + " piezas, favor de ingresar las guias hijas";
//                        RespuestaImpo.SolicitarPieceId = true;
//                        RespuestaImpo.TotalPieceId = numberPieceIds;
//                        RespuestaImpo.Mensaje = validaPieceIdsMensaje;
//                        return RespuestaImpo;
//                    }

//                    // INSERTAR LOS PIECE IDS
//                    if (IsNothing(objReferencia1))
//                    {
//                        Referencias objReferenciaNew = new Referencias();
//                        objReferenciaNew = LlenaObjReferencia(GuiaHouse, IdUsuario, IDGrupodeTrabajo, vidCliente, chkbSubdivision, oficinaObj, 1);
//                        IDReferencia = objReferenciaD1.Insertar(objReferenciaNew, GObjUsuario.IDDatosDeEmpresa);
//                        objReferencia1 = objReferenciaD1.Buscar(IDReferencia, GObjUsuario.IDDatosDeEmpresa);
//                    }

//                    // 2024-05-17 fpereda
//                    if (IDDatosDeEmpresa == 1)
//                    {
//                        piecesIds.ForEach(pieceID =>
//                        {
//                            objPieceIData.ImpoInsertPieceID(GuiaHouse, pieceID, IdOficina, IdUsuario, objReferencia1.IDReferencia);
//                        });
//                    }
//                    if (IDDatosDeEmpresa == 2)
//                    {
//                        piecesIds.ForEach(customAlertBaby =>
//                        {
//                            customAlertBabyData.ImpoInsertCustomAlertBaby(objCustomsAlerts.IdCustomAlert, customAlertBaby, IdUsuario, IdOficina, DateTime.Now);
//                        });
//                    }
//                }



//                // _________Turnar a clasificacion_________


//                var objTgrupo = new GruposdeTrabajo();


//                ControldeEventos objTEventos123 = new ControldeEventos();
//                AsignarGuiasRespuesta objTResp = new AsignarGuiasRespuesta();
//                ControldeEventosRepository objTEventosD = new ControldeEventosRepository(_configuration);
//                AsignarGuias objTAsignar = new AsignarGuias();
//                int idTEvento;

//                if (!IsNothing(objTransito))
//                {
//                    if (!IsNothing(objTransito.IdOficinaLlegada))
//                    {
//                        int idRiel = 0;
//                        if (!IsNothing(objCMF))
//                            idRiel = objCMF.IdRiel;
//                        // -- 1 FRM1, 6 FRM2, 8 SIN RIEL
//                        List<int> rieles = new List<int>();
//                        rieles.Add(1);
//                        rieles.Add(6);
//                        rieles.Add(8);

//                        if (rieles.Contains(idRiel))
//                        {
//                            if (IsNothing(objReferencia1))
//                            {
//                                Referencias objReferenciaNew = new Referencias();
//                                objReferenciaNew = LlenaObjReferencia(GuiaHouse, IdUsuario, IDGrupodeTrabajo, vidCliente, chkbSubdivision, oficinaObj, 1);
//                                IDReferencia = objReferenciaD1.Insertar(objReferenciaNew, GObjUsuario.IDDatosDeEmpresa);
//                                objReferencia1 = objReferenciaD1.Buscar(IDReferencia, GObjUsuario.IDDatosDeEmpresa);
//                            }

//                            objTgrupo = objGrupoD.Buscar(IDGrupodeTrabajo);
//                            objTEventos123 = new ControldeEventos(123, objReferencia1.IDReferencia, GObjUsuario.IdUsuario, DateTime.Parse("01/01/1900"));
//                            objTResp = objTEventosD.InsertarEvento(objTEventos123, 9, GObjUsuario.IdOficina, false, "", GObjUsuario.IDDatosDeEmpresa);
//                            idTEvento = objTResp.IdEvento;
//                            if (reasignar)
//                                objTAsignar.ReasignarGuia(GObjUsuario.IdUsuario, objTAsignar.getUsuarioDeGrupo(IDGrupodeTrabajo), objReferencia1.IDReferencia);
//                            // ENVIAR CORREO
//                            if (objTEventosD.SendMailTurnado(objReferencia1.NumeroDeReferencia, objTransito.IdOficinaSalida, objTransito.IdOficinaLlegada, objReferencia1.IDReferencia))
//                            {
//                                RespuestaImpo.BackColor = 0;
//                                RespuestaImpo.Mensaje = "Guía:" + " " + objReferencia1.NumeroDeReferencia + " " + "Turnada Correctamente," + Constants.vbCrLf + " al grupo" + " " + objTgrupo.Nombre.Trim()+ "";
//                                return RespuestaImpo;
//                            }
//                        }
//                    }


//                    bool IsSubdivision = false;
//                    if (!chkbSubdivision)
//                    {
//                        IsSubdivision = objPieceIData.IsSubdivision(GuiaHouse.Trim());
//                        if (IsSubdivision)
//                        {
//                            RespuestaImpo.NotifyIsSubdivision = true;
//                            RespuestaImpo.Mensaje = "Se detecto Piece Id sin validar en Previo ¿Se trata de una subdivisión?";
//                            return RespuestaImpo;
//                        }
//                    }
//                }


//                // _________SUBDIVICIONES__________________

//                PrioridadSafranData prioridadData = new PrioridadSafranData();
//                PrioridadSafran prioridadSafran;

//                prioridadSafran = prioridadData.Buscar(GuiaHouse.Trim());

//                if (!IsNothing(prioridadSafran))
//                    RespuestaImpo.Prioridad = "AWB " + GuiaHouse.Trim() + "  con prioridad tipo " + prioridadSafran.Prioridad;

//                GruposdeTrabajo objGrupoTop = new GruposdeTrabajo();
//                objGrupoTop = objGrupoD.BuscarTop(IdOficina, 9);

//                switch (IdCategoria)
//                {
//                    case 1:
//                    case 10 // No Liberar
//                   :
//                        {
//                            if (avisarDiffAgenteAdu == 0)
//                            {
//                                RespuestaImpo.AvisarDiffAgenteAdu = true;
//                                RespuestaImpo.Mensaje = "Guía que pertenece a otro Agente Aduanal";
//                                return RespuestaImpo;
//                            }

//                            if (avisarDiffAgenteAdu == 1)
//                            {
//                                objCustomsAlertsD.ModificarporIDCategoria3(Strings.Mid(GuiaHouse, Strings.Len(GuiaHouse) - 9, 10));
//                                RespuestaImpo.Mensaje = "Cambio a categoria 3"; // REALIZAR PREVIO
//                                RespuestaImpo.BackColor = 4;
//                            }
//                            else
//                            {
//                                if (IsNothing(objReferencia1))
//                                {
//                                    Referencias objReferenciaNew = new Referencias();
//                                    objReferenciaNew = LlenaObjReferencia(GuiaHouse, IdUsuario, IDGrupodeTrabajo, vidCliente, chkbSubdivision, oficinaObj, 1);

//                                    IDReferencia = objReferenciaD1.Insertar(objReferenciaNew, IDDatosDeEmpresa);
//                                    objReferencia1 = objReferenciaD1.Buscar(IDReferencia, IDDatosDeEmpresa);
//                                }
//                                else
//                                    IDReferencia = objReferencia1.IDReferencia;


//                                RespuestaImpo.Mensaje = "Turnado a Agente aduanal";
//                                RespuestaImpo.BackColor = 4;
//                                // --------------
//                                AsignarGuiasRespuesta objRespuesta = new AsignarGuiasRespuesta();
//                                ControldeEventosRepository objEventosDa = new ControldeEventosRepository(_configuration);
//                                ControldeEventos objEventoAA = new ControldeEventos(389, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                objRespuesta = objEventosDa.InsertarEvento(objEventoAA, IdDepartamento, IdOficina, false, "", IDDatosDeEmpresa);
//                            }


//                            return RespuestaImpo;
//                        }

//                    case 2 // Clientes Top
//                   :
//                        {
//                            if (!IsNothing(objGrupoTop))
//                                IDGrupodeTrabajo = objGrupoTop.IdGrupo;

//                            if (vidCliente != 0)
//                                RespuestaImpo.CARTADEINSTRUCCIONES = LlenardgvCartadeInstrucciones(vidCliente, IdOficina);
//                            break;
//                        }

//                    case 3:
//                    case 11 // Realizar Previo
//             :
//                        {
//                            if (!IsNothing(objGrupoTop))
//                            {
//                                if (IDGrupodeTrabajoSelected == objGrupoTop.IdGrupo)
//                                    throw new ArgumentException("No se debe asignar a equipo top, la instruccion es realizar previo");
//                            }
//                            if (vidCliente != 0)
//                                RespuestaImpo.CARTADEINSTRUCCIONES = LlenardgvCartadeInstrucciones(vidCliente, IdOficina);
//                            break;
//                        }

//                    case 4 // Notificar antes de Realizar Previo  
//             :
//                        {
//                            CartadeInstruccionesData cartadeInstruccionesData = new CartadeInstruccionesData();

//                            // Multipiezas notificar si hace falta

//                            if (avisarDiffAgenteAdu == 0)
//                            {
//                                RespuestaImpo.Mensaje = "La guía deberá ser notificada antes de realizar el previo";
//                                if (vidCliente != 0)
//                                    RespuestaImpo.CARTADEINSTRUCCIONES = cartadeInstruccionesData.LoadDvgCartadeInstruccionesForCambioValor(vidCliente, IdOficina);
//                                RespuestaImpo.AvisarDiffAgenteAdu = true;
//                                return RespuestaImpo;
//                            }

//                            if (avisarDiffAgenteAdu == 1)
//                                objCustomsAlertsD.ModificarporIDCategoria3(Strings.Mid(GuiaHouse, Strings.Len(GuiaHouse) - 9, 10));
//                            else
//                            {
//                                if (IsNothing(objReferencia1))
//                                {
//                                    Referencias objReferenciaNew = new Referencias();
//                                    objReferenciaNew = LlenaObjReferencia(GuiaHouse, IdUsuario, IDGrupodeTrabajo, vidCliente, chkbSubdivision, oficinaObj, 1);

//                                    IDReferencia = objReferenciaD1.Insertar(objReferenciaNew, IDDatosDeEmpresa);
//                                    objReferencia1 = objReferenciaD1.Buscar(IDReferencia, IDDatosDeEmpresa);
//                                }
//                                else
//                                    IDReferencia = objReferencia1.IDReferencia;


//                                RespuestaImpo.Mensaje = "Notificar antes de realizar previo";
//                                RespuestaImpo.BackColor = 4;
//                                // --------------
//                                AsignarGuiasRespuesta objRespuesta = new AsignarGuiasRespuesta();
//                                ControldeEventosRepository objEventosDa = new ControldeEventosRepository(_configuration);
//                                ControldeEventos objEventoCo = new ControldeEventos(132, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                objRespuesta = objEventosDa.InsertarEvento(objEventoCo, IdDepartamento, IdOficina, false, "", IDDatosDeEmpresa);
//                                return RespuestaImpo;
//                            }

//                            break;
//                        }

//                    case 5 // Consolidado
//             :
//                        {
//                            var isGlobalMayor = false;
//                            if (IDDatosDeEmpresa == 2)
//                            {
//                                if (IdOficina == 21)
//                                {
//                                    if (!IsNothing(objCustomsAlerts))
//                                    {
//                                        var IdTipodePedimento = objCustomsAlerts.IdTipodePedimento; // ID 18,19
//                                        List<int> listIdPedimentos = new List<int>{18,19};

//                                        if (listIdPedimentos.Contains(IdTipodePedimento))
//                                        {
//                                            isGlobalMayor = true;
//                                            vidCliente = 9307822; // FEDERAL EXPRESS



//                                            CatalogodeMasterData objGuiaMasterD = new CatalogodeMasterData();
//                                            var objGuiaMaster = objGuiaMasterD.Buscar(objCustomsAlerts.GuiaMaster.Trim);

//                                            if (IsNothing(objReferencia1))
//                                            {
//                                                Referencias objReferenciaNew = new Referencias();
//                                                objReferenciaNew = LlenaObjReferencia(GuiaHouse, IdUsuario, IDGrupodeTrabajo, vidCliente, chkbSubdivision, oficinaObj, 1);
//                                                if (!IsNothing(objGuiaMaster))
//                                                {
//                                                    objReferenciaNew.IDMasterConsol = objGuiaMaster.IDMasterConsol;
//                                                    vidMasterConsol = objGuiaMaster.IDMasterConsol;
//                                                }
//                                                IDReferencia = objReferenciaD1.Insertar(objReferenciaNew, IDDatosDeEmpresa);
//                                                objReferencia1 = objReferenciaD1.Buscar(IDReferencia, IDDatosDeEmpresa);
//                                            }
//                                            else
//                                            {
//                                                if (!IsNothing(objGuiaMaster))
//                                                {
//                                                    objReferencia1.IDMasterConsol = objGuiaMaster.IDMasterConsol;
//                                                    vidMasterConsol = objGuiaMaster.IDMasterConsol;
//                                                }
//                                                ReferenciasRepository ReferenciasRepository = new ReferenciasRepository();
//                                                objReferencia1.IDCliente = 9307822; // FEDERAL EXPRESS

//                                                ReferenciasRepository.Modificar(objReferencia1);
//                                            }

//                                            SaaioPedime objModificaSaaioPedime = new SaaioPedime();
//                                            SaaioPedime objPedime = new SaaioPedime();
//                                            SaaioPedimeData objPedimeD = new SaaioPedimeData();
//                                            objPedime = objPedimeD.Buscar(GuiaHouse);

//                                            SaaioPedime objSaaioPedime = new SaaioPedime();


//                                            GObjUsuario = objUsuariosD.Buscar(IdUsuario);


//                                            objSaaioPedime.NUM_REFE = GuiaHouse;
//                                            objSaaioPedime.ADU_DESP = GObjUsuario.Oficina.AduDesp;
//                                            objSaaioPedime.ADU_ENTR = GObjUsuario.Oficina.AduEntr;


//                                            FechadelServidor objFechaServ = new FechadelServidor();
//                                            FechadelServidorData objFechaServD = new FechadelServidorData();
//                                            objFechaServ = objFechaServD.Buscar(MyConnectionString);


//                                            DateTime dFecha;
//                                            dFecha = DateTime.ParseExact(objFechaServ.Fecha, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None);

//                                            objSaaioPedime.FEC_ENTR = dFecha;

//                                            CtarcTipCam objTipodeCambio = new CtarcTipCam();
//                                            CtarcTipCamData objTipodeCambioD = new CtarcTipCamData();
//                                            objTipodeCambio = objTipodeCambioD.Buscar(DateTime.Now);
//                                            if (!IsNothing(objTipodeCambio))
//                                            {
//                                                if (objTipodeCambio.TIP_CAM == null/* TODO Change to default(_) if this is not a reference type */ )
//                                                    throw new ArgumentException("El tipo de cambio no se ha dado de alta en Expertti");
//                                                else
//                                                    objSaaioPedime.TIP_CAMB = objTipodeCambio.TIP_CAM;
//                                            }


//                                            objSaaioPedime.CVE_PEDI = "T1";
//                                            objSaaioPedime.DES_ORIG = GObjUsuario.Oficina.DesOrig;

//                                            // Importacion
//                                            objSaaioPedime.MTR_ENTR = GObjUsuario.Oficina.MtrEntrImp;
//                                            objSaaioPedime.MTR_ARRI = GObjUsuario.Oficina.MtrArriImp;
//                                            objSaaioPedime.MTR_SALI = GObjUsuario.Oficina.MtrSaliImp;
//                                            objSaaioPedime.REG_ADUA = "IMD";


//                                            objSaaioPedime.MON_VASE = "0";

//                                            objSaaioPedime.SEC_DESP = GObjUsuario.Oficina.SecDesp;
//                                            objSaaioPedime.CVE_CAPT = "";
//                                            Clientes objClient = new Clientes();
//                                            ClientesData objClientD = new ClientesData();

//                                            objClient = objClientD.Buscar(vidCliente);
//                                            if (!IsNothing(objClient))
//                                                objSaaioPedime.CVE_IMPO = objClient.Clave;




//                                            objSaaioPedime.IMP_EXPO = 1;
//                                            objSaaioPedime.PAT_AGEN = GObjUsuario.Oficina.PatenteDefault;
//                                            objSaaioPedime.PES_BRUT = objCustomsAlerts.PesoTotal;
//                                            objSaaioPedime.CAN_BULT = objCustomsAlerts.Piezas;
//                                            objSaaioPedime.TOT_VEHI = 1;
//                                            objSaaioPedime.TIP_MOVA = "USD";

//                                            CatalogodeAgentesAduanales objCatalogodeAgentesAduanales = new CatalogodeAgentesAduanales();
//                                            CatalogodeAgentesAduanalesData objCatalogodeAgentesAduanalesD = new CatalogodeAgentesAduanalesData();

//                                            objCatalogodeAgentesAduanales = objCatalogodeAgentesAduanalesD.Buscar(GObjUsuario.Oficina.PatenteDefault);

//                                            if (!IsNothing(objCatalogodeAgentesAduanales))
//                                            {
//                                                objSaaioPedime.CVE_REPR = objCatalogodeAgentesAduanales.ClavedeRepresentante;
//                                                objSaaioPedime.EMP_FACT = objCatalogodeAgentesAduanales.EmpresaFactura;
//                                            }

//                                            if (IsNothing(objPedime))
//                                                objPedimeD.Insertar(objSaaioPedime);
//                                            else
//                                                objPedimeD.Modificar(objSaaioPedime);

//                                            SaaioGuias objInsertaGuiaM = new SaaioGuias();
//                                            SaaioGuiasData objInsertaGuiaHoMD = new SaaioGuiasData();

//                                            var ExistGuiaHouse = objInsertaGuiaHoMD.BuscarGuia(objReferencia1.NumeroDeReferencia, "H");

//                                            if (IsNothing(ExistGuiaHouse))
//                                            {
//                                                // INSERTA LA GUIA HOUSE
//                                                SaaioGuias LlenarobjInsertaGuiaH = new SaaioGuias();

//                                                LlenarobjInsertaGuiaH.NUM_REFE = GuiaHouse;
//                                                LlenarobjInsertaGuiaH.GUIA = GuiaHouse;
//                                                LlenarobjInsertaGuiaH.IDE_MH = "H";
//                                                LlenarobjInsertaGuiaH.CONS_GUIA = "1";
//                                                objInsertaGuiaHoMD.Insertar(LlenarobjInsertaGuiaH);
//                                            }



//                                            var ExistGuiaMaster = objInsertaGuiaHoMD.Buscar(objReferencia1.NumeroDeReferencia, "M");

//                                            if (IsNothing(ExistGuiaMaster))
//                                            {
//                                                // INSERTA LA GUIA MASTER
//                                                if (!IsNothing(objGuiaMaster))
//                                                {
//                                                    SaaioGuias LlenarobjInsertaGuiaM = new SaaioGuias();

//                                                    LlenarobjInsertaGuiaM.NUM_REFE = GuiaHouse;
//                                                    LlenarobjInsertaGuiaM.GUIA = objGuiaMaster.GuiaMaster;
//                                                    LlenarobjInsertaGuiaM.IDE_MH = "M";
//                                                    LlenarobjInsertaGuiaM.CONS_GUIA = "1";
//                                                    objInsertaGuiaM.NUM_REFE = objInsertaGuiaHoMD.Insertar(LlenarobjInsertaGuiaM);
//                                                }
//                                            }
//                                        }
//                                    }
//                                }
//                            }

//                            if (isGlobalMayor == false)
//                            {
//                                if (avisarDiffAgenteAdu == 0)
//                                {
//                                    RespuestaImpo.AvisarDiffAgenteAdu = true;
//                                    RespuestaImpo.Mensaje = "La Guía deberá ser procesada por consolidado";
//                                    return RespuestaImpo;
//                                }

//                                if (avisarDiffAgenteAdu == 1)
//                                    objCustomsAlertsD.ModificarporIDCategoria3(Strings.Mid(GuiaHouse, Strings.Len(GuiaHouse) - 9, 10));
//                                else
//                                {
//                                    if (IsNothing(objReferencia1))
//                                    {
//                                        Referencias objReferenciaNew = new Referencias();
//                                        objReferenciaNew = LlenaObjReferencia(GuiaHouse, IdUsuario, IDGrupodeTrabajo, vidCliente, chkbSubdivision, oficinaObj, 1);

//                                        IDReferencia = objReferenciaD1.Insertar(objReferenciaNew, IDDatosDeEmpresa);
//                                        objReferencia1 = objReferenciaD1.Buscar(IDReferencia, IDDatosDeEmpresa);
//                                    }
//                                    else
//                                        IDReferencia = objReferencia1.IDReferencia;


//                                    RespuestaImpo.Mensaje = "Guía " + GuiaHouse + " Turnada a consolidado ";
//                                    RespuestaImpo.BackColor = 4;
//                                    // --------------
//                                    AsignarGuiasRespuesta objRespuesta = new AsignarGuiasRespuesta();
//                                    ControldeEventosRepository objEventosDa = new ControldeEventosRepository(_configuration);
//                                    ControldeEventos objEventoCo = new ControldeEventos(19, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                                    objRespuesta = objEventosDa.InsertarEvento(objEventoCo, IdDepartamento, IdOficina, false, "", IDDatosDeEmpresa);
//                                    return RespuestaImpo;
//                                }
//                            }

//                            break;
//                        }
//                }

//                // BUSCA EN TABLA CATALOGO DE COTIZADORES POR CLIENTE Y SE TREA EL IDUSUARIO,
//                // SI SE TIENE CLIENTE IDENTIFICADO, BSUCA SI TIENE CLASIFICADOR ASIGNADO 

//                CatalogoDeCotizadoresPorCliente objCotizadoresporCliente = new CatalogoDeCotizadoresPorCliente();
//                CatalogoDeCotizadoresPorClienteData objCotizadoresporClienteD = new CatalogoDeCotizadoresPorClienteData();
//                objCotizadoresporCliente = objCotizadoresporClienteD.Buscar(vidCliente, IdOficina);

//                if (!IsNothing(objCotizadoresporCliente))
//                    IDGrupodeTrabajo = objGrupoD.BuscarporGrupo(objCotizadoresporCliente.IdUsuario);


//                // BUSCA SI EXISTE LA GUIA EN LA TABLA CONSOLANEXOS, SI EXISTE MANDA EXEPCION Y TERMINA 

//                ConsolAnexos objConsolAnexos = new ConsolAnexos();
//                ConsolAnexosData objConsolAnexosD = new ConsolAnexosData();
//                objConsolAnexos = objConsolAnexosD.Buscar(GuiaHouse, IDDatosDeEmpresa);

//                if (IsNothing(objConsolAnexos) == false)
//                    throw new Exception("Este Numero de Guia esta en Pedimento de Consolidado, Debe ser Eliminada");

//                // BUSCA SI LA GUIA YA EXISTE EN LA TABLA DE REFERENCIAS, SI ES SUBDIVISION PONE S AL INICIO DE LA REFERENCIA 
//                if (chkbSubdivision == true)
//                    GuiaHouse = "S" + GuiaHouse;

//                SaaioPedime objPedi = new SaaioPedime();
//                SaaioPedimeData objPediD = new SaaioPedimeData();
//                objPedi = objPediD.Buscar(GuiaHouse.Trim());
//                if (!IsNothing(objPedi))
//                {
//                    if (!IsNothing(objPedi.FIR_ELEC))
//                    {
//                        if (objPedi.FIR_ELEC.Trim()!= "")
//                            throw new ArgumentException("La Referencia " + GuiaHouse.Trim() + " ya tiene Firma Electronica");
//                    }
//                }

//                Referencias objReferencia = new Referencias();
//                ReferenciasRepository objReferenciaD = new ReferenciasRepository();
//                objReferencia = objReferenciaD.Buscar(GuiaHouse.Trim(), IDDatosDeEmpresa);


//                if (IsNothing(objReferencia))
//                {
//                    Referencias objReferenciaNew = new Referencias();
//                    objReferenciaNew = LlenaObjReferencia(GuiaHouse, IdUsuario, IDGrupodeTrabajo, vidCliente, chkbSubdivision, oficinaObj, 1);
//                    if (vidMasterConsol != 0)
//                        objReferenciaNew.IDMasterConsol = vidMasterConsol;

//                    IDReferencia = objReferenciaD.Insertar(objReferenciaNew, IDDatosDeEmpresa);

//                    objReferencia = objReferenciaD.Buscar(IDReferencia, IDDatosDeEmpresa);

//                    if (RevDsicrepancia)
//                    {
//                        ControldeEventosRepository objEventosDa = new ControldeEventosRepository(_configuration);

//                        // Checkpoint GUIA REVISADA POR GLOBAL
//                        ControldeEventos lEventos = new ControldeEventos(580, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                        objEventosDa.InsertarEventoPocket(lEventos, IdDepartamento, IdOficina, false, "", IDDatosDeEmpresa);

//                        // Checkpoint GLOBAL TURNA A FORMAL
//                        ControldeEventos lEventosDos = new ControldeEventos(587, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                        objEventosDa.InsertarEventoPocket(lEventosDos, IdDepartamento, IdOficina, false, "", IDDatosDeEmpresa);
//                    }
//                }
//                else
//                {
//                    Precaptura(objReferencia, IdUsuario, IdOficina, IDDatosDeEmpresa, 0);

//                    Referencias objReferenciaNew = new Referencias();

//                    objReferenciaNew = LlenaObjReferencia(objReferencia, GuiaHouse, IDGrupodeTrabajo, vidCliente, chkbSubdivision, oficinaObj, 1);
//                    if (vidMasterConsol != 0)
//                        objReferenciaNew.IDMasterConsol = vidMasterConsol;
//                    objReferenciaD.Modificar(objReferenciaNew);

//                    // buscarCheckPointsBultos(objReferencia.IDReferencia)
//                    ControldeEventosRepository objEventoD = new ControldeEventosRepository(_configuration);

//                    if (objEventoD.BuscaSiExisteIDCheckpoint(8, IDReferencia))
//                    {
//                        rdbUnitarias = true;
//                        rdbPartidas = false;
//                        rdbMiscelaneas = false;
//                        rdbVolumen = false;
//                        rdbTransito = false;
//                    }
//                    else if (objEventoD.BuscaSiExisteIDCheckpoint(9, IDReferencia))
//                    {
//                        rdbPartidas = true;
//                        rdbUnitarias = false;
//                        rdbMiscelaneas = false;
//                        rdbVolumen = false;
//                        rdbTransito = false;
//                    }
//                    else if (objEventoD.BuscaSiExisteIDCheckpoint(16, IDReferencia))
//                    {
//                        rdbMiscelaneas = true;
//                        rdbPartidas = false;
//                        rdbUnitarias = false;
//                        rdbVolumen = false;
//                        rdbTransito = false;
//                    }
//                    else if (objEventoD.BuscaSiExisteIDCheckpoint(163, IDReferencia))
//                    {
//                        rdbVolumen = true;
//                        rdbMiscelaneas = false;
//                        rdbPartidas = false;
//                        rdbUnitarias = false;
//                        rdbTransito = false;
//                    }
//                    else if (objEventoD.BuscaSiExisteIDCheckpoint(528, IDReferencia))
//                    {
//                        rdbTransito = true;
//                        rdbVolumen = false;
//                        rdbMiscelaneas = false;
//                        rdbPartidas = false;
//                        rdbUnitarias = false;
//                    }


//                    // Si existe Check prealerta.- se la asigno al mismo clasificador que tenia asignado


//                    if (objEventoD.BuscaSiExisteIDCheckpoint(181, objReferencia.IDReferencia))
//                        chkPreAlertas = true;

//                    // Si existe Check Información.- se la asigno al mismo clasificador que tenia asignado
//                    if (objEventoD.BuscaSiExisteIDCheckpoint(12, objReferencia.IDReferencia))
//                    {
//                        chkPreAlertas = true;
//                        if (objReferencia.IDGrupo != 0)
//                        {
//                            objTgrupo = objGrupoD.Buscar(objReferencia.IDGrupo);
//                            if (!IsNothing(objTgrupo))
//                            {
//                                if (objTgrupo.IdOficina == IdOficina)
//                                    IDGrupodeTrabajo = objReferencia.IDGrupo;
//                            }
//                        }
//                    }

//                    // Si existe Check Dias Anteriores.- se la asigno al mismo clasificador que tenia asignado
//                    if (objEventoD.BuscaSiExisteIDCheckpoint(196, objReferencia.IDReferencia))
//                    {
//                        chkDiasAnteriores = true;
//                        if (objReferencia.IDGrupo != 0)
//                        {
//                            objTgrupo = objGrupoD.Buscar(objReferencia.IDGrupo);
//                            if (!IsNothing(objTgrupo))
//                            {
//                                if (objTgrupo.IdOficina == IdOficina)
//                                    IDGrupodeTrabajo = objReferencia.IDGrupo;
//                            }
//                        }
//                    }

//                    // Dim objAsignar As New AsignarGuias
//                    // objAsignar.ReasignarGuia(GObjUsuario.IdUsuario, objAsignar.getUsuarioDeGrupo(IDGrupodeTrabajo), objReferencia.IDReferencia)
//                    vidCliente = objReferencia.IDCliente;
//                    objReferencia.IDGrupo = IDGrupodeTrabajo;
//                    objReferencia.ReferenciaDestinatario = "";
//                    objReferenciaD.Modificar(objReferencia);

//                    RespuestaImpo.CARTADEINSTRUCCIONES = LlenardgvCartadeInstrucciones(vidCliente, IdOficina);
//                }


//                // Eventos

//                if (RevDsicrepancia)
//                {
//                    ControldeEventosRepository objEventosDa = new ControldeEventosRepository(_configuration);

//                    // Checkpoint GUIA REVISADA POR GLOBAL
//                    ControldeEventos lEventos = new ControldeEventos(580, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                    objEventosDa.InsertarEventoPocket(lEventos, IdDepartamento, IdOficina, false, "", IDDatosDeEmpresa);
//                    Thread.Sleep(1000);
//                    // Checkpoint GLOBAL TURNA A FORMAL
//                    ControldeEventos lEventosDos = new ControldeEventos(587, IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                    objEventosDa.InsertarEventoPocket(lEventosDos, IdDepartamento, IdOficina, false, "", IDDatosDeEmpresa);
//                }

//                ControldeEventosRepository objEventosD = new ControldeEventosRepository(_configuration);

//                int idEvento;
//                ControldeEventos objEventos123 = new ControldeEventos(123, objReferencia.IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                AsignarGuiasRespuesta objResp = new AsignarGuiasRespuesta();
//                objResp = objEventosD.InsertarEvento(objEventos123, 9, IdOficina, false, "", IDDatosDeEmpresa);
//                idEvento = objResp.IdEvento;

//                AsignarGuias objAsignar = new AsignarGuias();
//                AsignarGuiasRespuesta reasigno = new AsignarGuiasRespuesta();
//                if (reasignar)
//                    reasigno = objAsignar.ReasignarGuia(IdUsuario, objAsignar.getUsuarioDeGrupo(IDGrupodeTrabajo), objReferencia.IDReferencia);

//                // idCheckPoint = DeterminarCheckPoint()

//                int idCheckPoint = 0;
//                if (rdbUnitarias == true)
//                    idCheckPoint = 8;

//                if (rdbPartidas == true)
//                    idCheckPoint = 9;

//                if (rdbMiscelaneas == true)
//                    idCheckPoint = 16;

//                if (rdbVolumen == true)
//                    idCheckPoint = 163;

//                if (rdbTransito == true)
//                    idCheckPoint = 528;

//                // If chkPreAlertas.Checked = True Then
//                // idCheckPoint = 181
//                // End If

//                // If chkInformaciones.Checked = True Then
//                // idCheckPoint = 12
//                // End If

//                ControldeEventos objEventos = new ControldeEventos(idCheckPoint, objReferencia.IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                idEvento = objEventosD.InsertarEvento(objEventos, IdDepartamento, IdOficina, IDDatosDeEmpresa);


//                if (chkPreAlertas == true)
//                {
//                    idCheckPoint = 181;
//                    ControldeEventos objEventos181 = new ControldeEventos(idCheckPoint, objReferencia.IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                    idEvento = objEventosD.InsertarEvento(objEventos181, IdDepartamento, IdOficina, IDDatosDeEmpresa);
//                }

//                if (chkInformaciones == true)
//                {
//                    idCheckPoint = 12;
//                    ControldeEventos objEventos12 = new ControldeEventos(idCheckPoint, objReferencia.IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                    idEvento = objEventosD.InsertarEvento(objEventos12, IdDepartamento, IdOficina, IDDatosDeEmpresa);
//                }

//                if (chkDiasAnteriores == true)
//                {
//                    idCheckPoint = 196;
//                    ControldeEventos objEventos196 = new ControldeEventos(idCheckPoint, objReferencia.IDReferencia, IdUsuario, DateTime.Parse("01/01/1900"));
//                    idEvento = objEventosD.InsertarEvento(objEventos196, IdDepartamento, IdOficina, IDDatosDeEmpresa);
//                }

//                // Fin Eventos

//                GruposdeTrabajo objGrupo = new GruposdeTrabajo();
//                objGrupo = objGrupoD.Buscar(IDGrupodeTrabajo);

//                if (IsNothing(objGrupo))
//                    throw new ArgumentException("No existe el grupo asignado ");
//                // GuiaHouseWec = txtReferencia.Text.Trim

//                RespuestaImpo.Mensaje = "Guía:" + " " + GuiaHouse + " " + "Turnada Correctamente, al grupo" + " " + objGrupo.Nombre.Trim()+ "";
//                RespuestaImpo.BackColor = 0;
//                ControldeEventosRepository controldeDeEventosData = new ControldeEventosRepository(_configuration);

//                RespuestaImpo.Eventos = controldeDeEventosData.ObtenerUltimosEventos(GuiaHouse, IDDatosDeEmpresa, 3);
//            }

//            catch (Exception ex)
//            {
//                RespuestaImpo.Mensaje = ex.Message;
//                RespuestaImpo.TipoDeRespuesta = TipoDeRespuesta.TR_Excepcion;
//            }

//            return RespuestaImpo;
//        }
//        public void BultosCompletosNotificacion(int idUsuario, int referencia)
//        {
//            int id;
//            SqlConnection cn = new SqlConnection();
//            SqlCommand cmd = new SqlCommand();
//            SqlParameter param;


//            cn.ConnectionString = sc;
//            cn.Open();
//            cmd.CommandText = "NET_EMAIL_NOTI_BULTOS_COMPLETOS";
//            cmd.Connection = cn;
//            cmd.CommandType = CommandType.StoredProcedure;


//            // ,@IdUsuario  int
//            param = cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4);
//            param.Value = idUsuario;

//            // ,@Referencia  varchar(15)
//            param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
//            param.Value = referencia;

//            try
//            {
//                cmd.ExecuteNonQuery();

//                cmd.Parameters.Clear();
//            }
//            catch (Exception ex)
//            {
//                id = 0;
//                cn.Close();
//                // SqlConnection.ClearPool(cn)
//                cn.Dispose();
//            }
//            cn.Close();
//            // SqlConnection.ClearPool(cn)
//            cn.Dispose();
//        }

//        public bool GetAutoFunctionalityValue(bool reasignar, int idOficina)
//        {
//            ActivarFuncionalidadesRepository activarFuncionalidadesData = new ActivarFuncionalidadesRepository(_configuration);
//            var funcionalidad = activarFuncionalidadesData.BuscarPorOficina("ASIGNACION AUT IMPO", idOficina);

//            if (funcionalidad == null)
//                return reasignar;

//            return funcionalidad.Activo;
//        }
//    }
//}

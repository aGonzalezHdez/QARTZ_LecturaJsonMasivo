using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesNube;
using LibreriaClasesAPIExpertti.Services.Turnado;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesVUCEM;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RespositoriesVentanillaUnica;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPedimentos
{
    public class ControlDeVerificacionRepository
    {
        private readonly string _conexion;
        private readonly IConfiguration _configuration;

        public ControlDeVerificacionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _conexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public async Task<ProcesarCheckPointResponse> ProcesarCheckPoint(ProcesarCheckPointRequest procesarCheckPointRequest)
        {
            ProcesarCheckPointResponse response = new();

            try
            {
                if (procesarCheckPointRequest == null)
                    throw new ArgumentNullException(nameof(procesarCheckPointRequest), "El objeto de solicitud no puede ser nulo");

                if (procesarCheckPointRequest.SoloAsignar)
                {
                    if (string.IsNullOrWhiteSpace(procesarCheckPointRequest.NumeroDeReferencia))
                        throw new ArgumentException("El número de referencia es obligatorio para la asignación de pedimento");
                    if (procesarCheckPointRequest.IdUsuario <= 0)
                        throw new ArgumentException("El ID del usuario debe ser mayor que cero para la asignación de pedimento");
                    AsignarPedimento(procesarCheckPointRequest, out string pedimento, out int disponibles);
                }

                var referenciaRepo = new ReferenciasRepository(_configuration);
                var referencia = referenciaRepo.Buscar(procesarCheckPointRequest.NumeroDeReferencia);
                if (referencia == null)
                    throw new ArgumentException("La referencia no existe o no se pudo encontrar");

                var usersRepo = new CatalogoDeUsuariosRepository(_configuration);
                var usuario = usersRepo.BuscarPorId(procesarCheckPointRequest.IdUsuario);
                if (usuario == null)
                    throw new ArgumentException("El usuario no existe o no tiene permisos para realizar esta operación");


                var checkPointRepo = new CatalogodeCheckPointsRepository(_configuration);
                var objCheck = checkPointRepo.BuscarId(procesarCheckPointRequest.IdCheckPoint, usuario.IdOficina, referencia.IDReferencia);
                if (objCheck == null)
                    throw new ArgumentException("No se encontró el CheckPoint seleccionado.");


                Guardar(
                    objCheck,
                    objCheck.IDCheckPoint,
                    referencia.IDReferencia,
                    procesarCheckPointRequest.Observacion.Trim(),
                    procesarCheckPointRequest.Extraordinario,
                    null,
                    procesarCheckPointRequest.IdUsuario,
                    usuario.IdDepartamento,
                    procesarCheckPointRequest.IdUsuario,
                    procesarCheckPointRequest.IdDatosDeEmpresa
                );

                GuardarSaaioPedime(
                    procesarCheckPointRequest.NumeroDeReferencia,
                    new SaaioPedime
                    {
                        NUM_REFE = procesarCheckPointRequest.NumeroDeReferencia,
                        AUT_OBSE = procesarCheckPointRequest.Observacion.Trim()
                    },
                    procesarCheckPointRequest.Observacion.Trim()
                );

                SaberSiEsTextil(referencia.NumeroDeReferencia, referencia.IDReferencia);

                if (objCheck.IdDepartamentoDestino == 24)
                {
                    NubePrepagoRepository nubePrepagoRepository = new NubePrepagoRepository(_configuration);

                    var SaaioFacturData = new Factura(referencia.CapturaenCasa);
                    var MyCons_Fact = SaaioFacturData.EXTRAE_MAX_CONS_FACT(referencia.NumeroDeReferencia);

                    var SAAIO_FACTUR = new SaaioFactur();

                    SAAIO_FACTUR = SaaioFacturData.Buscar(referencia.NumeroDeReferencia, MyCons_Fact);

                    try
                    {
                        await nubePrepagoRepository.EnviarCove(referencia, MyCons_Fact, SAAIO_FACTUR, procesarCheckPointRequest.pMisDocumentos, usuario);
                    }
                    catch (Exception ex)
                    {
                        response.Mensaje = ex.Message;
                        response.Exitoso = false;
                        return response;
                    }

                    response.Errores = await EnviarDocumentAsync(
                        referencia.IDReferencia,
                        usuario,
                        procesarCheckPointRequest.pMisDocumentos
                    );

                }

            }
            catch (Exception ex)
            {
                response.Mensaje = ex.Message;
                response.Exitoso = false;
                return response;
            }

            return response;
        }

        private void SaberSiEsTextil(string referencia, int idReferencia)
        {
            SaaioFracciRepository saaioFracciRepository = new SaaioFracciRepository(_configuration);
            saaioFracciRepository.SaberSiEsTextil(referencia, idReferencia);
        }

        public void AsignarPedimento(ProcesarCheckPointRequest objRequest, out string pedimento, out int disponibles)
        {
            pedimento = string.Empty;
            disponibles = 0;

            // Buscar Referencia
            var referenciaRepo = new ReferenciasRepository(_configuration);
            var referencia = referenciaRepo.Buscar(objRequest.NumeroDeReferencia);
            if (referencia == null)
                throw new ArgumentException("Favor de buscar la referencia");

            // Buscar usuario

            var usuarioRepo = new CatalogoDeUsuariosRepository(_configuration);
            var GObjUsuario = usuarioRepo.BuscarPorId(objRequest.IdUsuario);
            if (GObjUsuario == null)
                throw new ArgumentException("El usuario no existe o no tiene permisos para realizar esta operación");

            if (objRequest.SoloAsignar)
            {
                var eventosRepo = new ControldeEventosRepository(_configuration);

                var evento = new ControldeEventos
                {
                    IDCheckPoint = 64, // Asignación de pedimento
                    IDReferencia = referencia.IDReferencia,
                    IDUsuario = objRequest.IdUsuario,
                    FechaEvento = DateTime.Parse("1900-01-01")
                };

                eventosRepo.InsertarEvento(
                    evento,
                    GObjUsuario.IdDepartamento,
                    referencia.IdOficina,
                    objRequest.IdDatosDeEmpresa
                ).GetAwaiter().GetResult();
            }

            var pedimeRepo = new SaaioPedimeRepository(_configuration);
            var pedime = pedimeRepo.Buscar(objRequest.NumeroDeReferencia);
            if (pedime != null)
            {
                var ctrlPedRepo = new ControldePedimentosRepository(_configuration);
                var ctrlPed = new ControldePedimentos
                {
                    Patente = pedime.PAT_AGEN,
                    IdOficina = GObjUsuario.IdOficina,
                    IdReferencia = referencia.IDReferencia,
                    IdUsuario = GObjUsuario.IdUsuario
                };

                // Validación IMP_EXPO
                if (pedime.IMP_EXPO == "1")
                {
                    var idePedRepo = new SaaioIdePedRepository(_configuration);
                    var listaIdePed = idePedRepo.LlenaDataGridViewSaaioIDePedPorReferencia(pedime.NUM_REFE);

                    if (listaIdePed != null)
                    {
                        int cuentaCR = listaIdePed.Count(x => x.CVE_IDEN == "CR");

                        if (cuentaCR > 1)
                            throw new ArgumentException("Imposible asignar número de pedimento, la proforma cuenta con más de un CR");
                    }
                }

                // Asignar pedimento
                var resultado = ctrlPedRepo.AsignarPedimento(
                    ctrlPed,
                    objRequest.Extraordinario,
                    pedime.ADU_DESP
                ).GetAwaiter().GetResult();

                if (resultado != null)
                {
                    pedimento = resultado.Pedimento;
                    disponibles = resultado.Disponibles;
                }
            }
        }

        public void Guardar(CatalogodeCheckPoints objCheck, int idCheckPoint, int idReferencia,
                         string observaciones, bool servicioExtraordinario,
                         List<int> erroresSeleccionados, int idUsuarioError, int idDepartamento,
                         int idUsuario, int idDatosDeEmpresa)
        {
            if (observaciones.Trim().Length < 10)
                throw new ArgumentException("Se requiere una observación más específica");

            bool seleccionoError = erroresSeleccionados != null && erroresSeleccionados.Count > 0;

            if (objCheck.Reproceso)
            {
                if (!seleccionoError)
                    throw new ArgumentException("Es necesario seleccionar un error");

                if (idUsuarioError == 0)
                    throw new ArgumentException("Es necesario seleccionar al usuario del error");
            }

            // Obtener referencia
            var referenciasRepository = new ReferenciasRepository(_configuration);
            var referencia = referenciasRepository.Buscar(idReferencia);
            if (referencia == null)
                throw new ArgumentException("Favor de buscar la referencia");

            // Insertar evento
            var eventosRepository = new ControldeEventosRepository(_configuration);
            var evento = new ControldeEventos(idCheckPoint, idReferencia, idUsuario, DateTime.Parse("1900-01-01"));

            var respuesta = eventosRepository.InsertarEvento(
                evento,
                idDepartamento,
                referencia.IdOficina,
                servicioExtraordinario,
                observaciones.Trim(),
                idDatosDeEmpresa
            ).GetAwaiter().GetResult();

            int idEvento = respuesta.IdEvento;

            if (idUsuarioError != 0 && seleccionoError)
            {
                foreach (var errorId in erroresSeleccionados)
                {
                    InsertarError(idReferencia, errorId, idUsuarioError, idEvento);
                }
            }

            if (!string.IsNullOrEmpty(respuesta.Pedimento))
            {
                Console.WriteLine($"Pedimento: {respuesta.Pedimento}, Disponibles: {respuesta.Disponibles}");
            }

            if (respuesta.IdUsuarioAsignado != 0)
            {
                var usuariosRepository = new CatalogoDeUsuariosRepository(_configuration);
                var usuarioAsignado = usuariosRepository.BuscarPorId(respuesta.IdUsuarioAsignado);

                if (usuarioAsignado != null)
                {
                    Console.WriteLine($"Usuario Asignado: {usuarioAsignado.Nombre}");
                }
            }
            else
            {
                throw new ArgumentException("No existen usuarios configurados para turnar al área correspondiente");
            }
        }

        private void InsertarError(int idReferencia, int idError, int idUsuarioError, int idEvento)
        {
            Insertar(idReferencia, idError, idUsuarioError, idEvento);
        }

        public int Insertar(int idReferencia, int idError, int idUsuarioError, int idEvento)
        {
            int id = 0;

            using (var cn = new SqlConnection(_conexion))
            {
                using (var cmd = new SqlCommand("NET_INSERT_CONTROLDEVERIFICACION", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdReferencia", SqlDbType.Int).Value = idReferencia;
                    cmd.Parameters.Add("@IdError", SqlDbType.Int).Value = idError;
                    cmd.Parameters.Add("@IdUsuarioError", SqlDbType.Int).Value = idUsuarioError;
                    cmd.Parameters.Add("@idEvento", SqlDbType.Int).Value = idEvento;

                    var outputId = cmd.Parameters.Add("@newid_registro", SqlDbType.Int);
                    outputId.Direction = ParameterDirection.Output;

                    try
                    {
                        cn.Open();
                        cmd.ExecuteNonQuery();

                        if (outputId.Value != DBNull.Value && Convert.ToInt32(outputId.Value) != -1)
                        {
                            id = Convert.ToInt32(outputId.Value);
                        }
                        else
                        {
                            id = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + " | Error en NET_INSERT_CONTROLDEVERIFICACION");
                    }
                }
            }

            return id;
        }

        public void GuardarSaaioPedime(string numeroDeReferencia, SaaioPedime datos, string observaciones)
        {
            try
            {
                var saaioPedimeRepository = new SaaioPedimeRepository(_configuration);

                // Consultamos si ya existe el registro
                var registroExistente = saaioPedimeRepository.Buscar(numeroDeReferencia);

                if (registroExistente != null)
                {
                    registroExistente.NUM_REFE = numeroDeReferencia;
                    registroExistente.AUT_OBSE = observaciones;
                    saaioPedimeRepository.ModificarNEW(registroExistente);
                }
                else
                {
                    throw new Exception($"No se encontró registro SaaioPedime para la referencia: {numeroDeReferencia}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar SaaioPedime: {ex.Message}");
            }
        }

        public async Task<List<string>> EnviarDocumentAsync(int idReferencia, CatalogoDeUsuarios usuario, string misDocumentos)
        {
            var errores = new List<string>();

            try
            {
                var digitalizadosVucemRepository = new DigitalizadosVucemRepository(_configuration);
                var documentos = digitalizadosVucemRepository.CargarParaVerificacion(idReferencia);

                var digitalizacion = new Digitalizacion(_configuration);

                var turnaPago = false;

                foreach (var doc in documentos)
                {
                    var eDocuments = await digitalizacion.Digitalizar(doc.IdDigitalizadosVucem, usuario, misDocumentos);

                    if (eDocuments != null)
                    {
                        if (eDocuments.Err)
                        {
                            errores.Add($"Archivo: {doc.Archivo}");
                            foreach (var error in eDocuments.Errores)
                            {
                                errores.Add($"  - {error}");
                            }
                        }
                        else
                        {
                            errores.Add($"Archivo: {doc.Archivo}");
                            errores.Add("  - La operación está en proceso");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errores.Add($"Excepción: {ex.Message}");
            }

            return errores;
        }
    }
}

using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesRobot;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesRobot.Constantes;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPrevalidador;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesRobot
{
    public class RobotRecuperarJuliano:IRobotRecuperarJuliuano
    {

        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public RobotRecuperarJuliano(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public void recuperarJuliano()
        {
            RobotPagoRepository robotPagoRepository = new(_configuration);
            DataTable idsOficina = robotPagoRepository.GetIdsOficina();
            if (idsOficina.Rows.Count > 0)
            {
                foreach (DataRow dr in idsOficina.Rows)
                {
                    var idOficina = Convert.ToInt32(dr["idOficina"]);
                    var AduanaDespacho = Convert.ToInt32(dr["AduanaDespacho"].ToString());
                    var Prevalidador = (dr["Prevalidador"]).ToString();
                    var Patente = (dr["Patente"].ToString());
                    process(idOficina, AduanaDespacho, Prevalidador, Patente);
                }
            }
        }


        public void process(int idOficina, int Aduana, string Prevalidador, string Patente)
        {
            RobotPagoRepository robotPagoRepository = new(_configuration);
            var timeDurarion = getConfigMaxDuration(idOficina);
            var maxItems = getConfigMaxNumberItems(idOficina);

            DataTable pendingRecords = robotPagoRepository.GetRobotPagoListByStatus("VALIDACIÓN ENVIADO", maxItems, idOficina, Aduana, Prevalidador, Patente);
            if (pendingRecords.Rows.Count > 0)
            {
                foreach (DataRow dr in pendingRecords.Rows)
                {
                    var idRobotPago = Convert.ToInt32(dr["iDNetRobotPago"].ToString());
                    var idReferencia = Convert.ToInt32(dr["IdReferencia"].ToString());
                    var numeroReferencia = dr["NumeroDeReferencia"].ToString();
                    var idDatosDeEmpresa = Convert.ToInt32(dr["IDDatosDeEmpresa"].ToString());
                    var idUsuario = Convert.ToInt32(dr["IdUsuario"].ToString());
                    var idOficinaDR = Convert.ToInt32(dr["IdOficina"].ToString());
                    var idValidado = Convert.ToInt32(dr["IdValidado"].ToString());
                    var IntentosEjecucion = Convert.ToInt32(dr["IntentosEjecucion"].ToString());


                    if (ConstantesRobotPago.numItemsRecuperarJuliano == maxItems || ConstantesRobotPago.InitTimeRecuperarJuliano == timeDurarion)
                    {
                        break;
                    }
                    else
                    {
                        ConstantesRobotPago.listPendientesRecuperarJuliano.Add(new RobotPago
                        {
                            IdRobotPago = idRobotPago,
                            NumeroDeReferencia = numeroReferencia,
                            IDDatosDeEmpresa = idDatosDeEmpresa,
                            IdReferencia = idReferencia,
                            Patente = Patente,
                            Prevalidador = Prevalidador,
                            AduanaDespacho = Aduana.ToString(),
                            IdUsuario = idUsuario,
                            IdOficina = idOficinaDR,
                            IdValidado = idValidado,
                            IntentosEjecucion = IntentosEjecucion,
                        });
                        ConstantesRobotPago.numItemsRecuperarJuliano++;
                    }
                }
                foreach (RobotPago item in ConstantesRobotPago.listPendientesRecuperarJuliano)
                {
                    recuperarJuliano(item);
                }

            }
        }

        private void recuperarJuliano(RobotPago robotPago)
        {
            RobotPagoRepository robotPagoRepository = new(_configuration);
            try
            {
                CatalogoDeUsuarios GObjUsuario = new();
                CatalogoDeUsuariosRepository GObjUsuarioRepository = new(_configuration);
                GObjUsuario = GObjUsuarioRepository.BuscarPorId(robotPago.IdUsuario);
                ControldeValidacionRepository controldeValidacion = new(_configuration);
                if (GObjUsuario != null)
                {
                    bool isCorrect = controldeValidacion.RecuperarValidacion(robotPago.IdValidado, robotPago.IdUsuario).GetAwaiter().GetResult();

                    if (isCorrect)
                    {
                        _ = robotPagoRepository.RobotPagoUpdateStatus(robotPago.IdRobotPago, "VALIDADO", "");
                        //EnviarCorreo
                        robotPagoRepository.SendEmail(robotPago.IdReferencia, true);
                    }
                    else
                    {

                        if (robotPago.IntentosEjecucion < 3)
                        {

                            robotPago.IntentosEjecucion++;
                            _ = robotPagoRepository.RobotPagoUpdateExecutionAttemps(robotPago.IdRobotPago, robotPago.IntentosEjecucion);
                        }
                        else
                        {
                            _ = robotPagoRepository.RobotPagoUpdateStatus(robotPago.IdRobotPago, "ERROR", "No fue posible completar el proceso. Se alcanzó el número máximo de intentos (3).");
                            robotPagoRepository.SendEmail(robotPago.IdReferencia, false);
                            //Turnar a validacion y pago
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _ = robotPagoRepository.RobotPagoUpdateStatus(robotPago.IdRobotPago, "ERROR", ex.Message);
                throw;
            }
        }

        int getConfigMaxDuration(int idOficina)
        {
            var robotPago = new RobotPagoRepository(_configuration);
            DataTable configRobotPagoDuration = robotPago.GetRobotConfigByClave("CHANGE_STATUS_DURATION", idOficina);
            var timeDuration = 0;

            if (configRobotPagoDuration.Rows.Count > 0)
            {
                foreach (DataRow dr in configRobotPagoDuration.Rows)
                {
                    timeDuration = Convert.ToInt32(dr["Valor"].ToString());
                }
            }
            return timeDuration;
        }

        int getConfigMaxNumberItems(int idOficina)
        {
            var robotPago = new RobotPagoRepository(_configuration);
            DataTable configRobotPagoDuration = robotPago.GetRobotConfigByClave("CHANGE_STATUS_NUMBER_MAX_ITEMS", idOficina);
            var maxItems = 0;

            if (configRobotPagoDuration.Rows.Count > 0)
            {
                foreach (DataRow dr in configRobotPagoDuration.Rows)
                {
                    maxItems = Convert.ToInt32(dr["Valor"].ToString());
                }
            }
            return maxItems;
        }

        public void reset()
        {
            ConstantesRobotPago.InitTime = 0;
            ConstantesRobotPago.numItems = 0;
            ConstantesRobotPago.pedimentosPendientes = new List<string>();
            ConstantesRobotPago.listPendientes = new List<RobotPago>();
        }
    }
}

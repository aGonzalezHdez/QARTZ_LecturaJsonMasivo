using LibreriaClasesAPIExpertti.Entities.EntitiesRobot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesRobot.Constantes
{
    public static class ConstantesRobotPago
    {
        #region RobotPagoCreaJuliano
        public static int InitTime = 0;
        public static int numItems = 0;
        public static List<String> pedimentosPendientes = new List<string>();
        public static List<RobotPago> listPendientes = new List<RobotPago>();
        #endregion

        #region RobotPagoRecuperarJuliano
        public static int InitTimeRecuperarJuliano = 0;
        public static int numItemsRecuperarJuliano = 0;
        public static List<String> pedimentosPendientesRecuperarJuliano = new List<string>();
        public static List<RobotPago> listPendientesRecuperarJuliano = new List<RobotPago>();
        #endregion
    }
}

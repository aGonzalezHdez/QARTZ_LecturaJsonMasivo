namespace LibreriaClasesAPIExpertti.Entities.EntitiesRobot
{
    using System.Collections.Generic;

    public class RobotPago
    {

        public int IdRobotPago;
        public int IdOficina;
        public string Patente;
        public string AduanaDespacho;
        public string Prevalidador;
        public string ClaveMandatario;
        public string Estatus;
        public int IdReferencia;
        public int IdCliente;
        public int IdUsuario;
        public string FechaArribo;
        public string FechaTermino;
        public string Descripcion;
        public string NumeroDeReferencia;
        public int IDDatosDeEmpresa;
        public int IdValidado;
        public int IntentosEjecucion;


        public static bool ValidateStatus(string Status)
        {

            bool result = false;
            var listStatus = new List<string>();
            listStatus.Add("PENDIENTE");
            listStatus.Add("PROCESO");
            listStatus.Add("ERROR");
            listStatus.Add("PAGADO");
            listStatus.Add("VALIDADO");
            listStatus.Add("VALIDACIÓN ENVIADO");
            if (listStatus.Contains(Status.Trim().ToUpper()))
            {
                result = true;
            }
            return result;

        }

    }
}

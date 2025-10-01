namespace LibreriaClasesAPIExpertti.Entities.EntitiesReferencias.Pendientes
{
    public class ReferenciasPendientes
    {
        public int idReferencia { get; set; }

        public string Referencia { get; set; }

        public string Pedimento { get; set; }

        public DateTime? FechaPago { get; set; }

        public int Operacion { get; set; }

        public string Clave { get; set; }

        public string CR { get; set; }

        public string Firma { get; set; }

        public double Efectivo { get; set; }

        public double Otros { get; set; }

        public double Total { get; set; }

        public string Cliente { get; set; }

        public string Representante { get; set; }

        public string FirmaPago { get; set; }


    }
}
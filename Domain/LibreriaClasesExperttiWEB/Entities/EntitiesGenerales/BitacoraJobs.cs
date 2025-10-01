namespace LibreriaClasesAPIExpertti.Entities.EntitiesGenerales
{
    public class BitacoraJobs
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        private string DescripcionField;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public string Descripcion
        {
            get { return DescripcionField; }
            set { DescripcionField = value; }
        }

        private string JobField;
        public string Job
        {
            get { return JobField; }
            set { JobField = value; }
        }

        private string ErrorTField;
        public string ErrorT
        {
            get { return ErrorTField; }
            set { ErrorTField = value; }
        }

        private DateTime FechaField;
        public DateTime Fecha
        {
            get { return FechaField; }
            set { FechaField = value; }
        }
    }
}

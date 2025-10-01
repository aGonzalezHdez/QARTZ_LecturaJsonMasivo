namespace LibreriaClasesAPIExpertti.Entities.EntitiesGenerales
{
    public class DodaRemesa
    {
        private int IdReferenciaField;
        public int IdReferencia
        {
            get
            {
                return IdReferenciaField;
            }
            set
            {
                IdReferenciaField = value;
            }
        }


        private int RemesaField;
        public int Remesa
        {
            get
            {
                return RemesaField;
            }
            set
            {
                RemesaField = value;
            }
        }

        private string CoveField;
        public string Cove
        {
            get
            {
                return CoveField;
            }
            set
            {
                CoveField = value;
            }
        }
    }
}

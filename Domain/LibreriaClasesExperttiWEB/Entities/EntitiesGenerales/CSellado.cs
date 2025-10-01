namespace LibreriaClasesAPIExpertti.Entities.EntitiesGenerales
{
    public class CSellado
    {
        private string CadenaOriginalAA;
        private string Firmado;
        private string Serie;
        public string _CadenaOrigianlAA
        {
            get
            {
                return CadenaOriginalAA;
            }
            set
            {
                CadenaOriginalAA = value;
            }
        }
        public string _Firmado
        {
            get
            {
                return Firmado;
            }
            set
            {
                Firmado = value;
            }
        }
        public string _Serie
        {
            get
            {
                return Serie;
            }
            set
            {
                Serie = value;
            }
        }
    }
}

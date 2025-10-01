namespace LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador
{

    public class CatalogoDeRegex
    {
        private int IdRegexField;
        public int IdRegex
        {
            get
            {
                return IdRegexField;
            }
            set
            {
                IdRegexField = value;
            }
        }


        private string RegexField;
        public string Regex
        {
            get
            {
                return RegexField;
            }
            set
            {
                RegexField = value;
            }
        }


        private string DescripcionField;
        public string Descripcion
        {
            get
            {
                return DescripcionField;
            }
            set
            {
                DescripcionField = value;
            }
        }
    }
}

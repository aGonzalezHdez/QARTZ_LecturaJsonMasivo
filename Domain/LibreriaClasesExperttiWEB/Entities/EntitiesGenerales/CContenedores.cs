namespace LibreriaClasesAPIExpertti.Entities.EntitiesGenerales
{
    public class CContenedores
    {
        private string ValorContenedor;
        private List<Ccandado> LCandados;
        public string _ValorContenedor
        {
            get
            {
                return ValorContenedor;
            }
            set
            {
                ValorContenedor = value;
            }
        }
        public List<Ccandado> _LCandados
        {
            get
            {
                return LCandados;
            }
            set
            {
                LCandados = value;
            }
        }
    }
}

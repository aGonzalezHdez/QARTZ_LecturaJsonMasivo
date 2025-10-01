namespace LibreriaClasesAPIExpertti.Entities.EntitiesGenerales
{
    public class CPedimentoAmericano
    {
        private string TipoPedimentoAmericano;
        private string ValorPedimentoAmericano;
        public string _TipoPedimentoAmericano
        {
            get
            {
                return TipoPedimentoAmericano;
            }
            set
            {
                TipoPedimentoAmericano = value;
            }
        }
        public string _ValorPedimentoAmericano
        {
            get
            {
                return ValorPedimentoAmericano;
            }
            set
            {
                ValorPedimentoAmericano = value;
            }
        }
    }
}

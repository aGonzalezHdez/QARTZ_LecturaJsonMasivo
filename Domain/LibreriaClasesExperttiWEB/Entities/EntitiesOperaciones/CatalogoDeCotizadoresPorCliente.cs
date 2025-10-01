using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones
{
    public class CatalogoDeCotizadoresPorCliente : CatalogoDeUsuarios
    {
        private int IDClienteCotizadorField;
        public int IDClienteCotizador
        {
            get
            {
                return IDClienteCotizadorField;
            }
            set
            {
                IDClienteCotizadorField = value;
            }
        }


        private int IDClienteField;
        public int IDCliente
        {
            get
            {
                return IDClienteField;
            }
            set
            {
                IDClienteField = value;
            }
        }

    }
}

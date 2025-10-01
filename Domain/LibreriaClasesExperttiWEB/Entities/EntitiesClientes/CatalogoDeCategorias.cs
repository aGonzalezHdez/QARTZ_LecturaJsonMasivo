namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class CatalogoDeCategorias
    {
        public int IdCategoria { get; set; }
        public string Descripcion { get; set; } = null!;
        public int IdRiel { get; set; }
        //public int idRielWEC { get; set; }
    }
}
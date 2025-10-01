namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class CatalogoDeRFCsGenericos
    {
        public int IdRfcgenerico { get; set; }
        public string NOMBRE { get; set; } = null!;
        public string RFC { get; set; } = null!;
        public int OrdenDeDespliegue { get; set; }

    }
}
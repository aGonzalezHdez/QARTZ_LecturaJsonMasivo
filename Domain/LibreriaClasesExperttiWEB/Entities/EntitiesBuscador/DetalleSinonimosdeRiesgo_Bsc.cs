namespace LibreriaClasesAPIExpertti.Entities.EntitiesBuscador
{
    public class DetalleSinonimosdeRiesgo_Bsc
    {
        public int idDetalleSR { get; set; }
        public int IdSinonimoRiesgo { get; set; }
        public int idBuscar { get; set; }      

        public string? Sinonimo { get; set; } = null!;

        public string? Categoria { get; set; } = null!;

        public string? Requiere { get; set; } = null!;

        public string? Nombre { get; set; } = null!;


    }
}

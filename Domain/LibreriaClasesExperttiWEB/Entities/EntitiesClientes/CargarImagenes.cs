namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class CargarImagenes
    {
        public int IdImagen { get; set; }

        public string Ubicacion { get; set; }

        public string TipoDeDocumento { get; set; }

        public string Archivo { get; set; }

        public bool S3 { get; set; }

        public string UbicacionAnterior { get; set; }

    }

}
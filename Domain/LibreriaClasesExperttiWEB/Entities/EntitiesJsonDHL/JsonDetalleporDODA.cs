
namespace LibreriaClasesAPIExpertti.Entities.EntitiesJsonDHL
{
    public class JsonDetalleporDODA
    {
        public int IdReferencia { get; set; }

        public string NumeroDeReferencia { get; set; }

        public string GuiaHouse { get; set; }

        public string? transferReceiptId { get; set; }

        public bool? TieneError { get; set; }

        public string? Mensaje { get; set; }

        public DateTime? FechaEnvio { get; set; }

        public string Tipo { get; set; }

        public bool GeneraRecibo { get; set; }

        public string Estatus { get; set; }

        public int idEstadistica { get; set; }

    }
}

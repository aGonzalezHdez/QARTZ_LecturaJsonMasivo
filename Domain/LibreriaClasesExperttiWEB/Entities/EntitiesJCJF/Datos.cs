using Newtonsoft.Json;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesJCJF
{
    public class Datos
    {
        [JsonProperty("idSubidaManifiestoEi")]
        public int? IdSubidaManifiestoEi { get; set; }
        public string? Master { get; set; }
        public string? Nombre { get; set; } = null;
        public bool? Procesado { get; set; }
        public int? RegistrosProcesados { get; set; }
        public DateTime? FechaProcesado { get; set; }
        public DateTime? Creado { get; set; }
    }
}

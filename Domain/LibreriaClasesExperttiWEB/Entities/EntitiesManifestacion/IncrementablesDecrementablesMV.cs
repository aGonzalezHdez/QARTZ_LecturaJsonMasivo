using System.ComponentModel.DataAnnotations;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesManifestacion
{
    public class IncrementablesDecrementablesMV
    {
        [Key]
        public int idIncrementable { get; set; }        
        public string? tipoIncrementable { get; set; }
        public DateTime fechaErogacion { get; set; }     
        public decimal? importe { get; set; }
        public string? tipoMoneda { get; set; }       
        public bool? aCargoImportador { get; set; }
        public bool? incrementable { get; set; }

        public string DescLegal { get; set; }

    }
}

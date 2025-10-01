using System.ComponentModel.DataAnnotations;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesManifestacion
{
    public class IncrementablesDecrementablesMVModificar
    {
        [Key]
        public int idIncrementable { get; set; }     
        public DateTime? fechaErogacion { get; set; }    
        public bool? aCargoImportador { get; set; }
        public bool? incrementable { get; set; }
       
    }
}

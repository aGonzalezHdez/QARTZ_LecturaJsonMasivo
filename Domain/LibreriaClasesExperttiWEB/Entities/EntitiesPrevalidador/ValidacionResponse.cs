using LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador;

namespace API_MSPedimentos.DTO
{
    public class ValidacionResponse
    {
        public List<ArchivoErr> archivoErrs { get; set; }   
        public String archivoK { get; set; }
        public String archivoJuliano { get; set; }
        public String archivoJulianoRuta { get; set; }
    }
}

namespace LibreriaClasesAPIExpertti.Entities.EntitiesUtilities
{
    public class Errores
    {      
        public int TotalArchivos { get; set; }

        public int ArchivosProcesados { get; set; }

        public int ArchivosErrores { get; set; }

        public List<string>? ListaErrores { get; set; } = new();
       
    }
}

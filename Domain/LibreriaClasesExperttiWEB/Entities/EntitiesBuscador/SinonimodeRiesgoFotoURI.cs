namespace LibreriaClasesAPIExpertti.Entities.EntitiesBuscador
{
    public class SinonimodeRiesgoFotoURI
    {
        public int IdSinonimoRiesgo { get; set; }

        public int NoFotos { get; set; }

        public List<FotosUri>? FotosUri { get; set; } = new();

    }


    public class FotosUri
    {
        public string? Link { get; set; }
    }
}

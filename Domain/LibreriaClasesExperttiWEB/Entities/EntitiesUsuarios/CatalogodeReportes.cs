namespace LibreriaClasesAPIExpertti.Entities.EntitiesUsuarios
{
    public class CatalogodeReportes
    {
        public int IDReporte { get; set; }

        public int IDOficina { get; set; }

        public int IDModulo { get; set; }

        public string? Nombre { get; set; }

        public string Script { get; set; }

        public int IDGrupos { get; set; }

        public int IDFTP { get; set; }

        public bool Estatus { get; set; }

        public string Emai { get; set; }

        public string ScriptNew { get; set; }

    }
}

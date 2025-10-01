using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;


namespace LibreriaClasesAPIExpertti.Entities.EntitiesProyectos
{
    public class ActividadesdeDesarrollo
    {
        public int IdProyecto { get; set; }
        public int IdDesarrollador { get; set; }
        public int TipoDePrioridad { get; set; }
        public string Solicitud { get; set; }
        public string App { get; set; }
        public double Estatus { get; set; }
        public string MotivosDeDemora { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaCompromido { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string VersionPublicada { get; set; }
        public string Comentarios { get; set; }
        public int TipoDeActividad { get; set; }
        public int IdEstatus { get; set; }

        public string Folio { get; set; }

        public int idDepartamento { get; set; }

        public CatalogoDeUsuariosCorto UsuarioSolicita { get; set; }
        public CatalogoDeUsuariosCorto UsuarioPruebas { get; set; }
        public List<int>? IdUsuariosInvolucrados { get; set; }
    }
}

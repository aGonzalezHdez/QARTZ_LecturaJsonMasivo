namespace LibreriaClasesAPIExpertti.Entities.EntitiesTrazabilidad
{
    public class AsignaciondeGuias
    {
        public int idAsignacionDeGuias { get; set; }
        public int idReferencia { get; set; }
        public int idDepartamento { get; set; }
        public int idUsuarioAsignado { get; set; }
        public int IdUsuarioAsigna { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public int IdCheckPointSalida { get; set; }
        public int IdOficina { get; set; }
        public string Nombre { get; set; }
        public bool Asigna { get; set; }
    }

}

namespace LibreriaClasesAPIExpertti.Entities.EntitiesReferencias.Pendientes
{
    public class pendientesValidacion
    {
        public int idReferencia { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public int Operacion { get; set; }
        public string Patente { get; set; }
        public string aduanaDespacho { get; set; }
        public string Representante { get; set; }
        public int idUsuario { get; set; }
        public bool Global { get; set; }
        public int IDDatosDeEmpresa { get; set; }
        public int IdOficina { get; set; }
    }

}

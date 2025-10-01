namespace LibreriaClasesAPIExpertti.Entities.EntitiesUsuarios
{
    public class PermisosYReportesporUsuario
    {
        public int IDPermisosyReportes { get; set; }
       
        public string Nombre { get; set; }
      
        public int IDUsuario { get; set; }

        public int IDReporte { get; set; }
    }
}

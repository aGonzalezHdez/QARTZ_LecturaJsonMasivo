/******************************************************************************************************
Usuario Crea: Cubits
Funcionalidad: Insertar tarea de proyectos.
Fecha de Modificación: 2025-09-24
Usuario Modifica: Edward - Cubits
Motivo del Cambio: Se agrega mapeo de usuario realizo cambios en tarea.
******************************************************************************************************/
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesProyectos
{
    public class DetalleActividadesDesarrollo
    {
        public string? Folio { get; set; }
        public string? Solicitud { get; set; }
        public int IdDetalle { get; set; }
        public int IdProyecto { get; set; }
        public string Tarea { get; set; }
        public double Porcentaje { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaTermino { get; set; }
        public int IdEstatus { get; set; }
        public string Estatus { get; set; }
        public string? EstatusColor { get; set; }
        public string Seguimiento { get; set; }
        public CatalogoDeUsuariosCorto Usuario { get; set; }
        public DateTime? FechaCompromiso { get; set; }
        public int TipoDePrioridad { get; set; }
        public int? IdProyectoNuevo { get; set; }

        public int IdUsuarioEvento { get; set; }
        public string Desarrolladores { get; set; }
        public string? Seccion { get; set; }
    }
}

using System.Net;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesS3
{
    public abstract class RespuestaS3Comun
    {
        public HttpStatusCode Estatus { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public string IdSolicitud { get; set; } = string.Empty;
    }

}

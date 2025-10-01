using System.ComponentModel.DataAnnotations;

namespace LibreriaClasesAPIExpertti.Entities.DespachoIntegrado
{
    public class Predoda
    {
        public int IdPredoda { get; set; }
        public string Aduana { get; set; }
        public string Patente { get; set; }
        public int Operacion { get; set; }
        public string Origen { get; set; }
        public string Placas { get; set; }
        public string NumeroGafeteUnico { get; set; }
        public string CAAT { get; set; }
        public bool Cancelado { get; set; }
        public bool Liberado { get; set; }
        public string TipoOperacionId { get; set; }
        public string EstatudId { get; set; }
        public string uuid { get; set; }
        public string uuidRelacionado { get; set; }
        public DateTime fechaCFDI { get; set; }
        public string endPointPdf { get; set; }
        public string procesoId { get; set; }
        public DateTime fechaSolicitud { get; set; }
        public DateTime FechaAlta { get; set; }
        public int Consecutivo { get; set; }
        public string PreDODA { get; set; }
        public int IdOficina { get; set; }
        public int ModalidadCruce { get; set; }
        public string NumerodeTag { get; set; }
        public string tipoDocumentoId { get; set; }
        public string FastId { get; set; }
        public string DatosAdicionales { get; set; }
        public string Candados { get; set; }
        public int IdRelacionBitacora { get; set; }
        public int IdRelaciondeEnvio { get; set; }
        public int IdSalidasConsol { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos
{

    public partial class DigitalizadosVucem
    {

        [Key]
        public int IdDigitalizadosVucem { get; set; }
              
        public int IDDocumento { get; set; }

        public int IDReferencia { get; set; }

        public int Consecutivo { get; set; }

        public string eDocument { get; set; }


        public string numeroDeTramite { get; set; }
        
        public int NoOperacion { get; set; }

        public DateTime FechaEnvioDate { get; set; }

        public DateTime FechaRecibidoDate { get; set; }

        public bool FechaEnvio { get; set; }

        public bool FechaRecibido { get; set; }

        public string RFCSello { get; set; }

        public bool ErrorArchivo { get; set; }

        public bool EnviadoSAT { get; set; }

        public DateTime FechaAlta { get; set; }

        public int NoHojas { get; set; }

        public string Extension { get; set; }

        public string HashDoc { get; set; }

        public string FirmaBase64 { get; set; }

        public int IdDigitalizadosVucemPermiso { get; set; }

        public int idDocumentoS3 { get; set; }

        public int idDocumentoAcuse { get; set; }

        public string? Complemento { get; set; }

        public int? idDocumentoPorGuia { get; set; }

        public int IdTipoDocumento { get; set; }

        public string Archivo { get; set; }

        public string URL { get; set; }  
        
        public string URLviejo { get; set; }

        public string Encriptado { get; set; }


        public string linkS3 { get; set; }

    }
}

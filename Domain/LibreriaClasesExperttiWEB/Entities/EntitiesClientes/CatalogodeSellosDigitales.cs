namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class CatalogodeSellosDigitales
    {
        public int IdSelloDigital { get; set; }

        public string UsuarioWebService { get; set; }

        public string PasswordWebService { get; set; }

        public byte[] ArchivoKey { get; set; }

        public byte[] ArchivoCer { get; set; }

        public string PasswordSello { get; set; }

        public string CertificadoBase64 { get; set; }

        public byte[]? Certificado { get; set; }

        public bool OpenSSL { get; set; }

        public string NumeroDeSerie { get; set; }

        public string RFCConsulta { get; set; }

        public string Email { get; set; }

        public DateTime FechaExpedicion { get; set; }

        public DateTime FechaVigencia { get; set; }

        public string Algoritmo { get; set; }

        public bool SellaMensajeria { get; set; }

        public string? CiecUsuario { get; set; }

        public string? CiecPassword { get; set; }

        public string? TokenAVC { get; set; }

        public DateTime? FechaToken { get; set; }

        public DateTime? VigenciaAVC { get; set; }

        public string? UsuarioAVC { get; set; }

        public string? PasswordAVC { get; set; }

        public bool? TokenAVCVencido { get; set; }

        public string? UsuarioANAM { get; set; }

        public string? passwordAnam { get; set; }

        public string? CURP_CONSULTA { get; set; }

    }

}
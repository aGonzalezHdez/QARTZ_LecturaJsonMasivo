namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class CatalogoDeClientesDeDeMensajeria
    {
        public int IDCliente { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string RFC { get; set; }
        public int IDCapturo { get; set; }
        public bool Activo { get; set; }
        public string Telefono { get; set; }
        public string EmailContacto { get; set; }
        public string Atencion { get; set; }

        public string eMailCFD { get; set; }

        public int TipoDeFigura { get; set; }

        public string EmailPdfCASA { get; set; }

        public DateTime FechaDeAlta { get; set; }

        public bool TipoDePersona { get; set; }

        public bool SoloExportacion { get; set; }

        public bool Prospecto { get; set; }

        public string IdEncriptado { get; set; }

        public DireccionesDeClientes Direcciones { get; set; }

        public int idReferencia { get; set; }
        public string Observaciones { get; set; }

    }
}

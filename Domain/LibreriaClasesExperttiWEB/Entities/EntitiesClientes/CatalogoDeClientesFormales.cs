namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class CatalogoDeClientesFormales
    {
        public int IDCliente { get; set; }
        public string? Clave { get; set; }
        public string? Nombre { get; set; } 
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; } 
        public bool RFCGenerico { get; set; }
        public string? RFC { get; set; }
        public string? CURP { get; set; }
        public int IDCapturo { get; set; }
        public bool Activo { get; set; }
        public string? Telefono { get; set; } 
        public string? EmailContacto { get; set; } 
        public string? Atencion { get; set; } 
        public bool VerificarProductos { get; set; }
        public int IDNotificador { get; set; }
        public int IDNotificadorBK { get; set; }
        public string? eMailCFD { get; set; } 
        public bool VerificaProveedor { get; set; }
        public bool OperacionesRT { get; set; }
        public string? SectorComercial { get; set; } 
        public int TipoDeFigura { get; set; }
        public bool MandanEdocuments { get; set; }
        public bool CambianFacturas { get; set; }
        public bool ForzarSOP { get; set; }
        public bool EnPadronDeIMP { get; set; }
        public string? EmailPdfCASA { get; set; } 
        public DateTime FechaDeAlta { get; set; }
        public string? RfcParaConsulta { get; set; } 
        public string? EmailManifiesto { get; set; } 
        public int IDVendedor { get; set; }
        public int SoloDeGrupoEI { get; set; }
        public DateTime FechaDeInicio { get; set; }
        public bool NoFacturar { get; set; }
        public bool SoloExportacion { get; set; }
        public bool Prospecto { get; set; }
        public int IdUsuarioManif { get; set; }
        public string? IdEncriptado { get; set; }
        public int IdRielWec { get; set; }
        public int IDTipoCliente { get; set; }
        public bool ClienteEspecialDespachoXPO { get; set; }
        public int IDTipoClienteTop { get; set; }
        public bool Pedimento { get; set; }
        public int IdRegimenFiscal { get; set; }
        public string? PasswordWEB { get; set; }
        public string? RegimenCapital { get; set; }
        public bool SoloFacturacion { get; set; }
        public string? idCIF { get; set; }
        public string? Ultimo { get; set; } 
        public string? Primero { get; set; } 
        public string? Siguiente { get; set; } 
        public string? Anterior { get; set; } 
        public string? Observaciones { get; set; }
        public DireccionesDeClientes? DireccionesDeClientes { get; set; }       
    }
}
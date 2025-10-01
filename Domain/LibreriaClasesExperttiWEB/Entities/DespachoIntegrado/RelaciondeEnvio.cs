namespace LibreriaClasesAPIExpertti.Entities.DespachoIntegrado
{
    public class RelaciondeEnvio
    {
        public int IdRelaciondeEnvio { get; set; }
        public string NumerodeEnvio { get; set; }
        public int ClavePedimento { get; set; }
        public DateTime Fecha { get; set; }
        public int IdUsuario { get; set; }
        public int Patente { get; set; }
        public bool Estatus { get; set; }
        public int IdDepartamento { get; set; }
        public string CAAT { get; set; }
        public string Placas { get; set; }
        public int IdOficina { get; set; }
        public DateTime FechaDeImpresion { get; set; }
        public string Integracion { get; set; }
        public string UbicacionDODA { get; set; }
        public int IdBloque { get; set; }
        public int IdVuelo { get; set; }
        public int IdCliente { get; set; }
        public string Cliente { get; set; }
        public bool ServicioExtraordinario { get; set; }
        public string RangoInicial { get; set; }
        public string RangoFinal { get; set; }
        public int TipoContingencia { get; set; }
        public int Operacion { get; set; }
        public int IdRelacionBitacora { get; set; }
        public int IdTramitador { get; set; }
        public string IdEmpTransportista { get; set; }
        public string Tramitador { get; set; }
        public int IdDoda { get; set; }
        public int IdPreDoda { get; set; }
        public int LimiteDePedimentos { get; set; }
        public int TotalDePedimentos { get; set; }
        public string AduanaEntrada { get; set; }
        public string AduanaDespacho { get; set; }
    }
}

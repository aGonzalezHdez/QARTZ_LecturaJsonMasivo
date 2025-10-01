using System.ComponentModel.DataAnnotations;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesManifestacion
{
    public class ManifestaciondeValorVucem
    {
        [Key]
        public int idMV { get; set; }
        public int idReferencia { get; set; }
        public bool? ExisteVinculacion { get; set; }
        public int? MetododeValoracion { get; set; }        
        public decimal? PrecioPagado { get; set; }
        public DateTime pagadoFecha { get; set; }
        public int? PagadoFormadePago { get; set; }
        public string? PagadoMoneda { get; set; }
        public string? pagadoEspecifique { get; set; }
        public decimal? precioPorPagar { get; set; }
        public DateTime porPagarFecha { get; set; }
		public string? porPagarMomento { get; set; }
		public int?  porPagarFormadePago { get; set; }
        public string?  porPagarEspecifique { get; set; }
        public string? porPagarMoneda { get; set; }

        public decimal? compensoPago { get; set; }
        public DateTime  compensoFecha { get; set; }
        public int? compensoFormadePago { get; set; }
        public string? compensoMotivo { get; set; }
        public string? compensoPrestacion { get; set; }
        public string? compensoEspecifique { get; set; }
        public string? RFCSella { get; set; }
        public string? CadenaOriginal { get; set; }
        public string? firmaDigital { get; set; }
        public DateTime? fechaEnvio { get; set; }
        public string? NumerodeOperacion { get; set; }
        public DateTime? fechaRecepcion { get; set; }
        public string? eDocument { get; set; }
        public int? idDocumentoAcuse { get; set; }
        public int? idDocumentoMV { get; set; }
        public bool? Aceptada { get; set; }
        public int? idUsuarioAcepta { get; set; }
        public DateTime? FechaAceptacion { get; set; }
        public string? Observaciones { get; set; }
        public bool? Estatus { get; set; }

    }
}

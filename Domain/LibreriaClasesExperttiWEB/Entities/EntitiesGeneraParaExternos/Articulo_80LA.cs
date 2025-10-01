using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesGeneraParaExternos
{
    public class Articulo_80LA
    {
      
        public string Referencia { get; set; }
        public string GuiaHouse { get; set; }
        public string PaisDeOrigen { get; set; }
        public string DescripcionPedimento { get; set; }
        public string DescripcionEnManifiesto { get; set; }
        public double CantidadUMC { get; set; }
        public int UnidadUMF { get; set; }
        public double ValorEnDolaresEnManifiesto { get; set; }
        public float ValorEnAduana { get; set; }
        public string Patente { get; set; }
        public string Pedimento { get; set; }
        public string AduanaDespacho { get; set; }
        public string AduanaEntrada { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaPago { get; set; }
        public string? Destinatario { get; set; }
        public string? RfcDestinatario { get; set; }
        public string? DomicilioDestinatario { get; set; }
        public string? TelefonoDestinatario { get; set; }
        public string? CorreoDestinatario { get; set; }
        public string? NombreRemitente { get; set; }
        public string? IdentificacionRemitente { get; set; }
        public string? DomicilioRemitente { get; set; }
        public string? TelefonoRemitente { get; set; }
        public string? CorreoRemitente { get; set; }
        public DateTime? FechaDePrevio { get; set; }
        public DateTime? FechaYHoraEnUnidad { get; set; }
        public DateTime? FechayHoraDeModulacion { get; set; }
        public string? Modulo { get; set; }
        public int TipodeOperacion { get; set; }
        public int NoPartida { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesDespacho
{
    public class Doda
    {
        public int IdDODA { get; set; }
        public string N_Ticket { get; set; }
        public string N_Integracion { get; set; }
        public string Patente { get; set; }
        public DateTime FechaEmision { get; set; }
        public int N_Pedimentos { get; set; }
        public int NAduana { get; set; }
        public string NoAduana { get; set; }
        public string CadenaOriginal { get; set; }
        public string NSerieCertificado { get; set; }
        public string SelloDigital { get; set; }
        public string NSerieCertificadoSAT { get; set; }
        public string SelloDigitalSAT { get; set; }
        public string CadenaOriginalSAT { get; set; }
        public string UsuarioCiec { get; set; }
        public string Link { get; set; }
        public string RutaArchivo { get; set; }
        public int DespachoAduanero { get; set; }
        public string NumeroGafeteUnico { get; set; }
        public bool ACTIVO { get; set; }
        public bool ERRORES { get; set; }
        public string DESCRIPCIONERROR { get; set; }
        public string Placas { get; set; }
        public string CAAT { get; set; }
        public string Seccion { get; set; }
        public int IdPredoda { get; set; }
        public string NumerodeTag { get; set; }
        public int tipo_documento_id { get; set; }
        public string Fast_Id { get; set; }
        public string datos_adicionales { get; set; }
        public int ModalidadCruce { get; set; }
        public string Candados { get; set; }
        public string AVC { get; set; }
        public DateTime FechaVigencia { get; set; }
        public string ValidacionAgencia { get; set; }
        public int Operacion { get; set; }
        public bool ModulacionAVC { get; set; }
        public int IDCHECKPOINT { get; set; }
        public DateTime FECHADESPACHO { get; set; }
    }
}

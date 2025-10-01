using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesAnam
{
    
    public class RequestAnam
    {
        [JsonPropertyName("aduana")]
        public string Aduana { get; set; }

        [JsonPropertyName("patente")]
        public string Patente { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("FolioMaster")]
        public string FolioMaster { get; set; }        

        [JsonPropertyName("Consecutivo")]
        public int Consecutivo { get; set; }

        [JsonPropertyName("documentos")]
        public List<Documento> Documentos { get; set; }
    }

    public class Documento
    {
        [JsonPropertyName("tipo_documento")]
        public string TipoDocumento { get; set; }

        [JsonPropertyName("usuario_consulta")]
        public string UsuarioConsulta { get; set; }

        [JsonPropertyName("archivo")]
        public string Archivo { get; set; }

        [JsonPropertyName("medio_transporte")]
        public string MedioTransporte { get; set; }

        //[JsonPropertyName("numero_equipo")]
        //public string NumeroEquipo { get; set; }

        //[JsonPropertyName("id_tren")]
        //public string IdTren { get; set; }

        //[JsonPropertyName("numero_pedimento")]
        //public string NumeroPedimento { get; set; }

        //[JsonPropertyName("parte_2")]
        //public string Parte2 { get; set; }

        //[JsonPropertyName("remesa")]
        //public string Remesa { get; set; }

        [JsonPropertyName("clave_pedimento")]
        public string ClavePedimento { get; set; }

        //[JsonPropertyName("empresa_ferroviaria")]
        //public string EmpresaFerroviaria { get; set; }

        //[JsonPropertyName("documento_transporte")]
        //public string DocumentoTransporte { get; set; }

        //[JsonPropertyName("iniciales")]
        //public string Iniciales { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesAnam
{
    public class ResponseRepadiMasterDocumento
    {
        [JsonPropertyName("folio")]
        public string folio { get; set; }

        [JsonPropertyName("medio_transporte")]
        public string medio_transporte { get; set; }

        [JsonPropertyName("email")]
        public string email { get; set; }

        [JsonPropertyName("aduana")]
        public string aduana { get; set; }

        [JsonPropertyName("numero_aduana")]
        public string numero_aduana { get; set; }

        [JsonPropertyName("tipo_documento")]
        public string tipo_documento { get; set; }

        [JsonPropertyName("patente_consulta")]
        public string patente_consulta { get; set; }

        [JsonPropertyName("usuario_consulta")]
        public string usuario_consulta { get; set; }

        [JsonPropertyName("id_tren")]
        public string id_tren { get; set; }

        [JsonPropertyName("numero_equipo")]
        public string numero_equipo { get; set; }

        [JsonPropertyName("numero_pedimento")]
        public string numero_pedimento { get; set; }

        [JsonPropertyName("parte_2")]
        public string parte_2 { get; set; }

        [JsonPropertyName("remesa")]
        public string remesa { get; set; }

        [JsonPropertyName("clave_pedimento")]
        public string clave_pedimento { get; set; }

        [JsonPropertyName("empresa_ferroviaria")]
        public string empresa_ferroviaria { get; set; }

        [JsonPropertyName("documento_transporte")]
        public string documento_transporte { get; set; }

        [JsonPropertyName("iniciales")]
        public string iniciales { get; set; }

        [JsonPropertyName("archivo")]
        public string archivo { get; set; }
    }

    public class ResponseRepadiMaster
    {
        [JsonPropertyName("folio_master")]
        public string folio_master { get; set; }

        [JsonPropertyName("documentos")]
        public List<ResponseRepadiMasterDocumento> documentos { get; set; }
    }

}

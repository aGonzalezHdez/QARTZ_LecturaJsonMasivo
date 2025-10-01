using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesAnam
{
    public class RequestAnamJson
    {        
            [JsonPropertyName("aduana")]
            public string Aduana { get; set; }

            [JsonPropertyName("patente")]
            public string Patente { get; set; }

            [JsonPropertyName("email")]
            public string Email { get; set; }                  

            [JsonPropertyName("documentos")]
            public List<Documento> Documentos { get; set; }
        
    }
}

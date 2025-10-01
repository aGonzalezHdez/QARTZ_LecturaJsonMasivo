using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesJsonDHL
{
    internal class JsonRespuesta
    {
        public bool success { get; set; }

        public string message { get; set; }

        public List<string> errors { get; set; }

        public int statusCode { get; set; }

        public JsonPayload payload { get; set; }

        public List<JsonerrorsModel> errorsModel { get; set; }

    }
}

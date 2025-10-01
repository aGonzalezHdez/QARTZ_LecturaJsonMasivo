using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesAnam
{
    public class FolioGenerado
    {
        public int No { get; set; }
        public string Folio { get; set; }
    }

    public class ResponseData
    {
        public string Message { get; set; }
        public List<FolioGenerado> folios_generados { get; set; }
    }

    public class FolioStatusResponse
    {
        public int Procesados { get; set; }
        public int Pendiente { get; set; }
        public ResponseData Response { get; set; }
        public string Estatus { get; set; }
    }
}

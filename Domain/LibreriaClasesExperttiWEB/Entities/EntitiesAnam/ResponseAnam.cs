using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesAnam
{
    public class SuccessResponse
    {
        public string message { get; set; }
        public List<FolioInfo> folios_generados { get; set; }
    }

    public class FolioInfo
    {
        public int No { get; set; }
        public string folio { get; set; }
    }

    public class ErrorResponse
    {
        public List<ErrorDetail> Detail { get; set; }
    }

    public class ErrorDetail
    {
        public List<object> Loc { get; set; }
        public string Msg { get; set; }
        public string Type { get; set; }
    }
}

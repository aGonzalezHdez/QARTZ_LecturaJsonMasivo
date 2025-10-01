using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos
{
    public class ApiError
    {
        public bool Success => false;
        public string ErrorType { get; set; }
        public string Message { get; set; }
        public int CustomCode { get; set; }
    }
}

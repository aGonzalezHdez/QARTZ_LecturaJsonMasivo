using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos
{
    public class ValidacionArchivoResult
    {
        public string Archivo { get; set; } = string.Empty;
        public long Tamaño { get; set; }
        public List<string> Validaciones { get; set; } = new();
    }
}

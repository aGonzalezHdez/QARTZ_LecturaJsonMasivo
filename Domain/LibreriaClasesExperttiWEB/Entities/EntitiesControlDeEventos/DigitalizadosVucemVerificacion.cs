using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos
{
    public class DigitalizadosVucemVerificacion
    {
        public int IdDigitalizadosVucem { get; set; }
        public int Id { get; set; }
        public string Documento { get; set; }
        public int NoOperacion { get; set; }
        public string eDocument { get; set; }
        public int numeroDeTramite { get; set; }
        public string Archivo { get; set; }
        public string URL { get; set; }
        public string Encriptado { get; set; }
    }
}

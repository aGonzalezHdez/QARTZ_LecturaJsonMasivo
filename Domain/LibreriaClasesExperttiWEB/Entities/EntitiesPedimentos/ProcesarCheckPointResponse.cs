using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos
{
    public class ProcesarCheckPointResponse
    {
        public string UsuarioAsignado { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public bool Exitoso { get; set; } = false;

        public List<string> Errores { get; set; } = new List<string>();
    }
}

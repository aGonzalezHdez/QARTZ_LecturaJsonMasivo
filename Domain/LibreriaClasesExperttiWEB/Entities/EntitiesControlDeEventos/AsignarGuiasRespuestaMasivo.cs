using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos
{
    public class AsignarGuiasRespuestaMasivo
    {
        public int IdReferencia { get; set; }
        public string Referencia { get; set; }
        public string Patente { get; set; }
        public string Pedimento { get; set; } = null!;
        public string UsuarioAsignado { get; set; }
        public string Resultado { get; set; }


    }
}

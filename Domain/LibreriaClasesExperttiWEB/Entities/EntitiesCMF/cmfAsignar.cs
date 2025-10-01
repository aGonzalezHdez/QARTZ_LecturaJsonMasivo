using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesCMF
{
    public class cmfAsignar
    {
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }
        public int idUsuario { get; set; }
        public bool Nuevos { get; set; }
    
    }
}

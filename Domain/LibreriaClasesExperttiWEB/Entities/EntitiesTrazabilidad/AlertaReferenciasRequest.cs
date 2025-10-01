using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesTrazabilidad
{
    public class AlertaReferenciasRequest
    {
        public string Referencias { get; set; }       // Lista separada por comas
        public string Mensaje { get; set; } // Mensaje del usuario
        public string Fecha { get; set; }       // Fecha en formato YYYY-MM-DD
    }
}

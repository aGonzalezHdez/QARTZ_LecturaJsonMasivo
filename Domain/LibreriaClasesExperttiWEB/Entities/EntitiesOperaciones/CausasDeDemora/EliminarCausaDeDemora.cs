using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones.CausasDeDemora
{
    public class EliminarCausaDeDemora
    {
        public int IdDemoraGuia { get; set; }

        public string Guia { get; set; }

        public string Justificacion { get; set; }

        public int IdUsuario { get; set; }

        public int IdDatosdeEmpresa { get; set; }
    }
}


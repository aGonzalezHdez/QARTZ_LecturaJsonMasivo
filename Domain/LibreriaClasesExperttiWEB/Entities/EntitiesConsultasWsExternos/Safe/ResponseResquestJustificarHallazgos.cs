using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.Safe
{
    public class ResponseResquestJustificarHallazgos
    {
        public string? PedimentoId { get; set; }

        public string? RFC { get; set; }

        public int CriterioId { get; set; }

        public string? Criterio { get; set; }

        public string? MensajeCriterio { get; set; }        

        public string? Justificacion { get; set; }

        public int ErrorId { get; set; }

        public int Severidad { get; set; }
       
    }
}

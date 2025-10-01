using LibreriaClasesAPIExpertti.Entities.EntitiesWs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesEquialencia
{
    public partial class EquivalenciaR : BaseRespuestaWS
    {

        private bool CargadoXmlField;
        public bool CargadoXml
        {
            get
            {
                return CargadoXmlField;
            }
            set
            {
                CargadoXmlField = value;
            }
        }
    }
}

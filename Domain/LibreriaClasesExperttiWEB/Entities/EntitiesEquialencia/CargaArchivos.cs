using LibreriaClasesAPIExpertti.Entities.EntitiesWs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesEquialencia
{
    public class CargaArchivos : BaseRespuestaWS
    {
        private string NombreField;
        public string Nombre
        {
            get
            {
                return NombreField;
            }
            set
            {
                NombreField = value;
            }
        }

        private int NumRegistrosField;
        public int NumRegistros
        {
            get
            {
                return NumRegistrosField;
            }
            set
            {
                NumRegistrosField = value;
            }
        }
    }
}

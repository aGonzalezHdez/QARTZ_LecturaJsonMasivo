using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesDespacho
{
    public class DetalleSalidasConsol
    {
        private int idDetalleSalidasConsol;
        public int IDDetalleSalidasConsol
        {
            get { return idDetalleSalidasConsol; }
            set { idDetalleSalidasConsol = value; }
        }

        private int idAnexo;
        public int IDAnexo
        {
            get { return idAnexo; }
            set { idAnexo = value; }
        }

        private int idSalidasConsol;
        public int IDSalidasConsol
        {
            get { return idSalidasConsol; }
            set { idSalidasConsol = value; }
        }

        private DateTime fechaSalidaConsol;
        public DateTime FechaSalidaConsol
        {
            get { return fechaSalidaConsol; }
            set { fechaSalidaConsol = value; }
        }
    }

}

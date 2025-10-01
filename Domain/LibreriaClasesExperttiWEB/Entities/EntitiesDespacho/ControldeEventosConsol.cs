using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesDespacho
{
    public class ControldeEventosConsol
    {
        private int ideventoField;
        private int idCheckPointField;
        private int idAnexosField;
        private int idUsuarioField;
        private DateTime fechaEventoField;

        // Constructor con parámetros
        public ControldeEventosConsol(int pIdCheckPoint, int pIdAnexos, int pIdUsuario, DateTime pFechaEvento)
        {
            IDCheckPoint = pIdCheckPoint;
            IdAnexos = pIdAnexos;
            IDUsuario = pIdUsuario;
            FechaEvento = pFechaEvento;
        }

        // Constructor sin parámetros
        public ControldeEventosConsol()
        {
            // TODO: Complete member initialization
        }

        public int IDEvento
        {
            get { return ideventoField; }
            set { ideventoField = value; }
        }

        public int IDCheckPoint
        {
            get { return idCheckPointField; }
            set { idCheckPointField = value; }
        }

        public int IdAnexos
        {
            get { return idAnexosField; }
            set { idAnexosField = value; }
        }

        public int IDUsuario
        {
            get { return idUsuarioField; }
            set { idUsuarioField = value; }
        }

        public DateTime FechaEvento
        {
            get { return fechaEventoField; }
            set { fechaEventoField = value; }
        }
    }

}

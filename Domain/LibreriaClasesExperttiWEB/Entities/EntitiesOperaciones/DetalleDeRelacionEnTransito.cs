namespace LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones
{
    public class DetalleDeRelacionEnTransito
    {
        private int IdDetalleRField;
        public int IdDetalleR
        {
            get
            {
                return IdDetalleRField;
            }
            set
            {
                IdDetalleRField = value;
            }
        }


        private int IdRelacionTField;
        public int IdRelacionT
        {
            get
            {
                return IdRelacionTField;
            }
            set
            {
                IdRelacionTField = value;
            }
        }


        private string GuiaHouseField;
        public string GuiaHouse
        {
            get
            {
                return GuiaHouseField;
            }
            set
            {
                GuiaHouseField = value;
            }
        }


        private string PieceIDField;
        public string PieceID
        {
            get
            {
                return PieceIDField;
            }
            set
            {
                PieceIDField = value;
            }
        }


        private DateTime FechaDeScaneoField;
        public DateTime FechaDeScaneo
        {
            get
            {
                return FechaDeScaneoField;
            }
            set
            {
                FechaDeScaneoField = value;
            }
        }


        private int IDUsuarioScaneaSalidaField;
        public int IDUsuarioScaneaSalida
        {
            get
            {
                return IDUsuarioScaneaSalidaField;
            }
            set
            {
                IDUsuarioScaneaSalidaField = value;
            }
        }


        private DateTime LlegadaAduanaDespachoField;
        public DateTime LlegadaAduanaDespacho
        {
            get
            {
                return LlegadaAduanaDespachoField;
            }
            set
            {
                LlegadaAduanaDespachoField = value;
            }
        }


        private int IdUsuarioScaneaLlegadaField;
        public int IdUsuarioScaneaLlegada
        {
            get
            {
                return IdUsuarioScaneaLlegadaField;
            }
            set
            {
                IdUsuarioScaneaLlegadaField = value;
            }
        }

        private int IdOficinaLlegadaField;
        public int IdOficinaLlegada
        {
            get
            {
                return IdOficinaLlegadaField;
            }
            set
            {
                IdOficinaLlegadaField = value;
            }
        }
        private int IdOficinaSalidaField;
        public int IdOficinaSalida
        {
            get
            {
                return IdOficinaSalidaField;
            }
            set
            {
                IdOficinaSalidaField = value;
            }
        }
    }
}

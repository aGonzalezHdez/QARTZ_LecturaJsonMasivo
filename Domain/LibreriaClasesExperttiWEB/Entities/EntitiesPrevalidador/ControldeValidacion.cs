/******************************************************************************************************
  Fecha de Modificación: 2025-09-16
  Usuario Modifica: Edward - Cubits
  Funcionalidad: Se agrega parametro CargaManual para identificar si archivo Juliano se carga desde pc usuario.
******************************************************************************************************/
namespace LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador
{
    public class ControldeValidacion
    {
        private int IdValidadoField;
        public int IdValidado
        {
            get
            {
                return IdValidadoField;
            }
            set
            {
                IdValidadoField = value;
            }
        }


        private string ArchivoField;
        public string Archivo
        {
            get
            {
                return ArchivoField;
            }
            set
            {
                ArchivoField = value;
            }
        }

        private string RecepcionField;
        public string Recepcion
        {
            get
            {
                return RecepcionField;
            }
            set
            {
                RecepcionField = value;
            }
        }


        private string JulianoField;
        public string Juliano
        {
            get
            {
                return JulianoField;
            }
            set
            {
                JulianoField = value;
            }
        }


        private bool RecibidoField;
        public bool Recibido
        {
            get
            {
                return RecibidoField;
            }
            set
            {
                RecibidoField = value;
            }
        }


        private DateTime FechaEnvioField;
        public DateTime FechaEnvio
        {
            get
            {
                return FechaEnvioField;
            }
            set
            {
                FechaEnvioField = value;
            }
        }

        private bool ValidacionField;
        public bool Validacion
        {
            get
            {
                return ValidacionField;
            }
            set
            {
                ValidacionField = value;
            }
        }

        private string AduanaField;
        public string Aduana
        {
            get
            {
                return AduanaField;
            }
            set
            {
                AduanaField = value;
            }
        }

        private string PrevalidadorField;
        public string Prevalidador
        {
            get
            {
                return PrevalidadorField;
            }
            set
            {
                PrevalidadorField = value;
            }
        }
        private int IDOficinaField;
        public int IDOficina
        {
            get
            {
                return IDOficinaField;
            }
            set
            {
                IDOficinaField = value;
            }
        }

        private bool pGlobalField;
        public bool pGlobal
        {
            get
            {
                return pGlobalField;
            }
            set
            {
                pGlobalField = value;
            }
        }
        public bool CargaManual { get; set; } = false;
    }
}

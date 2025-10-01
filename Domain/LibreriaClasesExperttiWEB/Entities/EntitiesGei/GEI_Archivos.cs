namespace LibreriaClasesAPIExpertti.Entities.EntitiesGei
{
    using System;

    public class GEI_Archivos
    {
        private int Id_ArchivoField;
        public int Id_Archivo
        {
            get
            {
                return Id_ArchivoField;
            }
            set
            {
                Id_ArchivoField = value;
            }
        }


        private int Id_TipoField;
        public int Id_Tipo
        {
            get
            {
                return Id_TipoField;
            }
            set
            {
                Id_TipoField = value;
            }
        }


        private string PathField;
        public string Path
        {
            get
            {
                return PathField;
            }
            set
            {
                PathField = value;
            }
        }


        private string OCField;
        public string OC
        {
            get
            {
                return OCField;
            }
            set
            {
                OCField = value;
            }
        }


        private string ReferenciaField;
        public string Referencia
        {
            get
            {
                return ReferenciaField;
            }
            set
            {
                ReferenciaField = value;
            }
        }


        private string Id_UsuarioField;
        public string Id_Usuario
        {
            get
            {
                return Id_UsuarioField;
            }
            set
            {
                Id_UsuarioField = value;
            }
        }


        private DateTime Ctl_AltaField;
        public DateTime Ctl_Alta
        {
            get
            {
                return Ctl_AltaField;
            }
            set
            {
                Ctl_AltaField = value;
            }
        }




    }
}

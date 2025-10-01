namespace LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador
{

    public class CatalogoDePreval
    {
        private int IDPrevalField;
        public int IDPreval
        {
            get
            {
                return IDPrevalField;
            }
            set
            {
                IDPrevalField = value;
            }
        }


        private string TipoDeRevisionField;
        public string TipoDeRevision
        {
            get
            {
                return TipoDeRevisionField;
            }
            set
            {
                TipoDeRevisionField = value;
            }
        }


        private int TipoDeOperacionField;
        public int TipoDeOperacion
        {
            get
            {
                return TipoDeOperacionField;
            }
            set
            {
                TipoDeOperacionField = value;
            }
        }


        private string MiSPField;
        public string MiSP
        {
            get
            {
                return MiSPField;
            }
            set
            {
                MiSPField = value;
            }
        }


        private int IDErrorField;
        public int IDError
        {
            get
            {
                return IDErrorField;
            }
            set
            {
                IDErrorField = value;
            }
        }



    }
}

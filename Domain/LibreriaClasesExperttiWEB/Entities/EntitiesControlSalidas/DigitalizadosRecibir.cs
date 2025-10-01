namespace LibreriaClasesAPIExpertti.Entities.EntitiesControlSalidas
{
    using System.Collections.Generic;

    public class DigitalizadosRecibir
    {
        private string NumerodeTramiteField;
        public string NumerodeTramite
        {
            get
            {
                return NumerodeTramiteField;
            }
            set
            {
                NumerodeTramiteField = value;
            }
        }


        private string eDocumentField;
        public string eDocument
        {
            get
            {
                return eDocumentField;
            }
            set
            {
                eDocumentField = value;
            }
        }



        private string CadenaOriginalField;
        public string CadenaOriginal
        {
            get
            {
                return CadenaOriginalField;
            }
            set
            {
                CadenaOriginalField = value;
            }
        }


        private bool ErrField;
        public bool Err
        {
            get
            {
                return ErrField;
            }
            set
            {
                ErrField = value;
            }
        }


        private List<string> ErroresField;
        public List<string> Errores
        {
            get
            {
                return ErroresField;
            }
            set
            {
                ErroresField = value;
            }
        }


    }
}

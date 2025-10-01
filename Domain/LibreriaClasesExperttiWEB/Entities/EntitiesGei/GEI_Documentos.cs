namespace LibreriaClasesAPIExpertti.Entities.EntitiesGei
{

    public class GEI_Documentos
    {
        private int Id_DocumentoField;
        public int Id_Documento
        {
            get
            {
                return Id_DocumentoField;
            }
            set
            {
                Id_DocumentoField = value;
            }
        }

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
    }
}

namespace LibreriaClasesAPIExpertti.Entities.EntitiesDocumentosPorGuia
{
    public class DocumentosporGuia
    {
        CatalogodeTiposDeDocumentos objTipDoc = new CatalogodeTiposDeDocumentos();


        private int idDocumentoField;
        public int idDocumento
        {
            get
            {
                return idDocumentoField;
            }
            set
            {
                idDocumentoField = value;
            }
        }


        private int idTipoDocumentoField;
        public int idTipoDocumento
        {
            get
            {
                return idTipoDocumentoField;
            }
            set
            {
                idTipoDocumentoField = value;
            }
        }


        private int idReferenciaField;
        public int idReferencia
        {
            get
            {
                return idReferenciaField;
            }
            set
            {
                idReferenciaField = value;
            }
        }


        private int ConsecutivoField;
        public int Consecutivo
        {
            get
            {
                return ConsecutivoField;
            }
            set
            {
                ConsecutivoField = value;
            }
        }


        private string RutaFechaField;
        public string RutaFecha
        {
            get
            {
                return RutaFechaField;
            }
            set
            {
                RutaFechaField = value;
            }
        }

        private string ExtensionField;
        public string Extension
        {
            get
            {
                return ExtensionField;
            }
            set
            {
                ExtensionField = value;
            }
        }

        private DateTime FechaAltaField;
        public DateTime FechaAlta
        {
            get
            {
                return FechaAltaField;
            }
            set
            {
                FechaAltaField = value;
            }
        }

        private int IdUsuarioField;
        public int IdUsuario
        {
            get
            {
                return IdUsuarioField;
            }
            set
            {
                IdUsuarioField = value;
            }
        }


        private string RutaS3Field;
        public string RutaS3
        {
            get
            {
                return RutaS3Field;
            }
            set
            {
                RutaS3Field = value;
            }
        }

        private bool S3Field;
        public bool S3
        {
            get
            {
                return S3Field;
            }
            set
            {
                S3Field = value;
            }
        }

        private string ComplementoField;
        public string Complemento
        {
            get
            {
                return ComplementoField;
            }
            set
            {
                ComplementoField = value;
            }
        }

    }
}

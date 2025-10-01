namespace LibreriaClasesAPIExpertti.Entities.EntitiesUtilities
{
    public class Certificado
    {
        private string SujetoField;
        public string Sujeto
        {
            get
            {
                return SujetoField;
            }
            set
            {
                SujetoField = value;
            }
        }

        private string IssuerField;
        public string Issuer
        {
            get
            {
                return IssuerField;
            }
            set
            {
                IssuerField = value;
            }
        }

        private string VersionField;
        public string Version
        {
            get
            {
                return VersionField;
            }
            set
            {
                VersionField = value;
            }
        }

        private string NotBeforeField;
        public string NotBefore
        {
            get
            {
                return NotBeforeField;
            }
            set
            {
                NotBeforeField = value;
            }
        }

        private string NotAfterField;
        public string NotAfter
        {
            get
            {
                return NotAfterField;
            }
            set
            {
                NotAfterField = value;
            }
        }

        private string ThumbprintField;
        public string Thumbprint
        {
            get
            {
                return ThumbprintField;
            }
            set
            {
                ThumbprintField = value;
            }
        }

        private string SerialNumberField;
        public string SerialNumber
        {
            get
            {
                return SerialNumberField;
            }
            set
            {
                SerialNumberField = value;
            }
        }

        private string FriendlyNameField;
        public string FriendlyName
        {
            get
            {
                return FriendlyNameField;
            }
            set
            {
                FriendlyNameField = value;
            }
        }

        private string EncodedKeyValueField;
        public string EncodedKeyValue
        {
            get
            {
                return EncodedKeyValueField;
            }
            set
            {
                EncodedKeyValueField = value;
            }
        }

        private string CertificateField;
        public string Certificate
        {
            get
            {
                return CertificateField;
            }
            set
            {
                CertificateField = value;
            }
        }

        private string SignatureAlgorithmField;
        public string SignatureAlgorithm
        {
            get
            {
                return SignatureAlgorithmField;
            }
            set
            {
                SignatureAlgorithmField = value;
            }
        }
    }

}

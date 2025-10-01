using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesCasa
{
    public class CtracProved : DatosdeNavegacion
    {
        public string NumeroDeReferencia { get; set; }

        public int myTipodeForma { get; set; }

        public string CVE_PRO { get; set; }

        public int IDUSUARIO { get; set; }

        public int IDCLIENTE { get; set; }

        public string NOM_PRO { get; set; }

        public string DIR_PRO { get; set; }

        public string POB_PRO { get; set; }

        public string ZIP_PRO { get; set; }

        public string TAX_PRO { get; set; }

        public string PAI_PRO { get; set; }

        public string CTA_PRO { get; set; }

        public string EFE_PRO { get; set; }

        public string NOI_PRO { get; set; }

        public string NOE_PRO { get; set; }

        public string VIN_PRO { get; set; }

        public string EFE_DESP { get; set; }

        public string TEL_PRO { get; set; }

        public string AFE_PREC { get; set; }

        public string CVE_PROC { get; set; }

        public DateTime FEC_BAJA { get; set; }
        //public DateTime? FEC_BAJA { get; set; } 
        public int INT_PRO { get; set; }

        public string EXP_CONF { get; set; }

        public string APE_PATE { get; set; }

        public string APE_MATE { get; set; }

        public string COL_PRO { get; set; }

        public string LOC_PRO { get; set; }

        public string REFE_PRO { get; set; }

        public string NOM_COVE { get; set; }

        public string MUN_COVE { get; set; }

        public string MAIL_COVE { get; set; }

        public string RUTA_USPPI { get; set; }

        public string TIP_OPER { get; set; }

        public string IMP_EXPO { get; set; }

        public string CVE_IMP { get; set; }

        public string NombreCliente { get; set; }

        public int cve_Buscada { get; set; }

        public string CVE_PROV { get; set; }


        // ////////////////////////// PROPERTY OMAR PROVEDOR Y DESTINO /////////////////////////////////

        public string NOM_PAIS { get; set; }

        public string NOM_ENTIDADFED { get; set; }

        public string USUAIO_QDALTA { get; set; }

        public int Tipo { get; set; }

        public int Estatus { get; set; }

        /// <summary>
        ///         ''' este valor no se guarda me sirve para saber si le doy valor a fec_baja
        ///         ''' </summary>
        ///         ''' <remarks></remarks>

        public bool Activo { get; set; }

    }
}


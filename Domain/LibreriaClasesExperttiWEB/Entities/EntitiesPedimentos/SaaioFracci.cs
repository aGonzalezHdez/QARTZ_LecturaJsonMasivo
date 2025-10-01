using DocumentFormat.OpenXml.Spreadsheet;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos
{
    public class SaaioFracci
    {
        private string NUM_REFEField;
        public string NUM_REFE
        {
            get
            {
                return NUM_REFEField;
            }
            set
            {
                NUM_REFEField = value;
            }
        }


        private double NUM_PARTField;
        public double NUM_PART
        {
            get
            {
                return NUM_PARTField;
            }
            set
            {
                NUM_PARTField = value;
            }
        }


        private string SUB_PARTField;
        public string SUB_PART
        {
            get
            {
                return SUB_PARTField;
            }
            set
            {
                SUB_PARTField = value;
            }
        }


        private string FRACCIONField;
        public string FRACCION
        {
            get
            {
                return FRACCIONField;
            }
            set
            {
                FRACCIONField = value;
            }
        }


        private double MON_FACTField;
        public double MON_FACT
        {
            get
            {
                return MON_FACTField;
            }
            set
            {
                MON_FACTField = value;
            }
        }


        private string TIP_MONEField;
        public string TIP_MONE
        {
            get
            {
                return TIP_MONEField;
            }
            set
            {
                TIP_MONEField = value;
            }
        }


        private string UNI_TARIField;
        public string UNI_TARI
        {
            get
            {
                return UNI_TARIField;
            }
            set
            {
                UNI_TARIField = value;
            }
        }


        private string UNI_FACTField;
        public string UNI_FACT
        {
            get
            {
                return UNI_FACTField;
            }
            set
            {
                UNI_FACTField = value;
            }
        }


        private string DES_MERCField;
        public string DES_MERC
        {
            get
            {
                return DES_MERCField;
            }
            set
            {
                DES_MERCField = value;
            }
        }


        private string PAI_ORIGField;
        public string PAI_ORIG
        {
            get
            {
                return PAI_ORIGField;
            }
            set
            {
                PAI_ORIGField = value;
            }
        }


        private string PAI_VENDField;
        public string PAI_VEND
        {
            get
            {
                return PAI_VENDField;
            }
            set
            {
                PAI_VENDField = value;
            }
        }


        private string EFE_ORIGField;
        public string EFE_ORIG
        {
            get
            {
                return EFE_ORIGField;
            }
            set
            {
                EFE_ORIGField = value;
            }
        }


        private string EFE_DESTField;
        public string EFE_DEST
        {
            get
            {
                return EFE_DESTField;
            }
            set
            {
                EFE_DESTField = value;
            }
        }


        private string EFE_COMPField;
        public string EFE_COMP
        {
            get
            {
                return EFE_COMPField;
            }
            set
            {
                EFE_COMPField = value;
            }
        }


        private string EFE_VENDField;
        public string EFE_VEND
        {
            get
            {
                return EFE_VENDField;
            }
            set
            {
                EFE_VENDField = value;
            }
        }


        private string TIP_MERCField;
        public string TIP_MERC
        {
            get
            {
                return TIP_MERCField;
            }
            set
            {
                TIP_MERCField = value;
            }
        }


        private string USO_CARAField;
        public string USO_CARA
        {
            get
            {
                return USO_CARAField;
            }
            set
            {
                USO_CARAField = value;
            }
        }


        private string CVE_VALOField;
        public string CVE_VALO
        {
            get
            {
                return CVE_VALOField;
            }
            set
            {
                CVE_VALOField = value;
            }
        }


        private string CVE_VINCField;
        public string CVE_VINC
        {
            get
            {
                return CVE_VINCField;
            }
            set
            {
                CVE_VINCField = value;
            }
        }


        private string CAS_TLCSField;
        public string CAS_TLCS
        {
            get
            {
                return CAS_TLCSField;
            }
            set
            {
                CAS_TLCSField = value;
            }
        }


        private double ADVALField;
        public double ADVAL
        {
            get
            {
                return ADVALField;
            }
            set
            {
                ADVALField = value;
            }
        }


        private double POR_ADV1Field;
        public double POR_ADV1
        {
            get
            {
                return POR_ADV1Field;
            }
            set
            {
                POR_ADV1Field = value;
            }
        }


        private double POR_ADV2Field;
        public double POR_ADV2
        {
            get
            {
                return POR_ADV2Field;
            }
            set
            {
                POR_ADV2Field = value;
            }
        }


        private string FPA_ADV1Field;
        public string FPA_ADV1
        {
            get
            {
                return FPA_ADV1Field;
            }
            set
            {
                FPA_ADV1Field = value;
            }
        }


        private string FPA_ADV2Field;
        public string FPA_ADV2
        {
            get
            {
                return FPA_ADV2Field;
            }
            set
            {
                FPA_ADV2Field = value;
            }
        }


        private double ARA_ESPEField;
        public double ARA_ESPE
        {
            get
            {
                return ARA_ESPEField;
            }
            set
            {
                ARA_ESPEField = value;
            }
        }


        private double CON_AZUCField;
        public double CON_AZUC
        {
            get
            {
                return CON_AZUCField;
            }
            set
            {
                CON_AZUCField = value;
            }
        }


        private double MON_SUNTField;
        public double MON_SUNT
        {
            get
            {
                return MON_SUNTField;
            }
            set
            {
                MON_SUNTField = value;
            }
        }


        private double MON_DTAPField;
        public double MON_DTAP
        {
            get
            {
                return MON_DTAPField;
            }
            set
            {
                MON_DTAPField = value;
            }
        }


        private double PORC_IVAField;
        public double PORC_IVA
        {
            get
            {
                return PORC_IVAField;
            }
            set
            {
                PORC_IVAField = value;
            }
        }


        private double POR_IVA1Field;
        public double POR_IVA1
        {
            get
            {
                return POR_IVA1Field;
            }
            set
            {
                POR_IVA1Field = value;
            }
        }


        private double POR_IVA2Field;
        public double POR_IVA2
        {
            get
            {
                return POR_IVA2Field;
            }
            set
            {
                POR_IVA2Field = value;
            }
        }


        private string FPA_IVA1Field;
        public string FPA_IVA1
        {
            get
            {
                return FPA_IVA1Field;
            }
            set
            {
                FPA_IVA1Field = value;
            }
        }


        private string FPA_IVA2Field;
        public string FPA_IVA2
        {
            get
            {
                return FPA_IVA2Field;
            }
            set
            {
                FPA_IVA2Field = value;
            }
        }


        private double PRE_DETAField;
        public double PRE_DETA
        {
            get
            {
                return PRE_DETAField;
            }
            set
            {
                PRE_DETAField = value;
            }
        }


        private double NUM_UNIDField;
        public double NUM_UNID
        {
            get
            {
                return NUM_UNIDField;
            }
            set
            {
                NUM_UNIDField = value;
            }
        }


        private string CAL_IEPSField;
        public string CAL_IEPS
        {
            get
            {
                return CAL_IEPSField;
            }
            set
            {
                CAL_IEPSField = value;
            }
        }


        private double MON_IEPSField;
        public double MON_IEPS
        {
            get
            {
                return MON_IEPSField;
            }
            set
            {
                MON_IEPSField = value;
            }
        }


        private string FPA_IEPSField;
        public string FPA_IEPS
        {
            get
            {
                return FPA_IEPSField;
            }
            set
            {
                FPA_IEPSField = value;
            }
        }


        private string CAL_ISANField;
        public string CAL_ISAN
        {
            get
            {
                return CAL_ISANField;
            }
            set
            {
                CAL_ISANField = value;
            }
        }


        private string FPA_ISANField;
        public string FPA_ISAN
        {
            get
            {
                return FPA_ISANField;
            }
            set
            {
                FPA_ISANField = value;
            }
        }


        private string CAL_COMPField;
        public string CAL_COMP
        {
            get
            {
                return CAL_COMPField;
            }
            set
            {
                CAL_COMPField = value;
            }
        }


        private double MON_COM1Field;
        public double MON_COM1
        {
            get
            {
                return MON_COM1Field;
            }
            set
            {
                MON_COM1Field = value;
            }
        }


        private double MON_COM2Field;
        public double MON_COM2
        {
            get
            {
                return MON_COM2Field;
            }
            set
            {
                MON_COM2Field = value;
            }
        }


        private string TIP_MCO1Field;
        public string TIP_MCO1
        {
            get
            {
                return TIP_MCO1Field;
            }
            set
            {
                TIP_MCO1Field = value;
            }
        }


        private string TIP_MCO2Field;
        public string TIP_MCO2
        {
            get
            {
                return TIP_MCO2Field;
            }
            set
            {
                TIP_MCO2Field = value;
            }
        }


        private string FPA_COM1Field;
        public string FPA_COM1
        {
            get
            {
                return FPA_COM1Field;
            }
            set
            {
                FPA_COM1Field = value;
            }
        }


        private string FPA_COM2Field;
        public string FPA_COM2
        {
            get
            {
                return FPA_COM2Field;
            }
            set
            {
                FPA_COM2Field = value;
            }
        }


        private double IMP_GARAField;
        public double IMP_GARA
        {
            get
            {
                return IMP_GARAField;
            }
            set
            {
                IMP_GARAField = value;
            }
        }


        private double VAL_AGREField;
        public double VAL_AGRE
        {
            get
            {
                return VAL_AGREField;
            }
            set
            {
                VAL_AGREField = value;
            }
        }


        private string MAR_MERCField;
        public string MAR_MERC
        {
            get
            {
                return MAR_MERCField;
            }
            set
            {
                MAR_MERCField = value;
            }
        }


        private string MOD_MERCField;
        public string MOD_MERC
        {
            get
            {
                return MOD_MERCField;
            }
            set
            {
                MOD_MERCField = value;
            }
        }


        private string COD_PRODField;
        public string COD_PROD
        {
            get
            {
                return COD_PRODField;
            }
            set
            {
                COD_PRODField = value;
            }
        }


        private string VIN_VEHIField;
        public string VIN_VEHI
        {
            get
            {
                return VIN_VEHIField;
            }
            set
            {
                VIN_VEHIField = value;
            }
        }


        private int KMT_VEHIField;
        public int KMT_VEHI
        {
            get
            {
                return KMT_VEHIField;
            }
            set
            {
                KMT_VEHIField = value;
            }
        }


        private string OBS_FRACField;
        public string OBS_FRAC
        {
            get
            {
                return OBS_FRACField;
            }
            set
            {
                OBS_FRACField = value;
            }
        }


        private double VAL_NORFField;
        public double VAL_NORF
        {
            get
            {
                return VAL_NORFField;
            }
            set
            {
                VAL_NORFField = value;
            }
        }


        private double VAL_COMFField;
        public double VAL_COMF
        {
            get
            {
                return VAL_COMFField;
            }
            set
            {
                VAL_COMFField = value;
            }
        }


        private double FAC_PRORField;
        public double FAC_PROR
        {
            get
            {
                return FAC_PRORField;
            }
            set
            {
                FAC_PRORField = value;
            }
        }


        private string FPA_ESPEField;
        public string FPA_ESPE
        {
            get
            {
                return FPA_ESPEField;
            }
            set
            {
                FPA_ESPEField = value;
            }
        }


        private string CAS_PREFField;
        public string CAS_PREF
        {
            get
            {
                return CAS_PREFField;
            }
            set
            {
                CAS_PREFField = value;
            }
        }


        private string COM_PREFField;
        public string COM_PREF
        {
            get
            {
                return COM_PREFField;
            }
            set
            {
                COM_PREFField = value;
            }
        }


        private double MON_ESPEField;
        public double MON_ESPE
        {
            get
            {
                return MON_ESPEField;
            }
            set
            {
                MON_ESPEField = value;
            }
        }


        private double POR_COMPField;
        public double POR_COMP
        {
            get
            {
                return POR_COMPField;
            }
            set
            {
                POR_COMPField = value;
            }
        }


        private double CAN_FACTField;
        public double CAN_FACT
        {
            get
            {
                return CAN_FACTField;
            }
            set
            {
                CAN_FACTField = value;
            }
        }


        private double CAN_TARIField;
        public double CAN_TARI
        {
            get
            {
                return CAN_TARIField;
            }
            set
            {
                CAN_TARIField = value;
            }
        }


        private string COM_TLCField;
        public string COM_TLC
        {
            get
            {
                return COM_TLCField;
            }
            set
            {
                COM_TLCField = value;
            }
        }


        private string DECR_TLCField;
        public string DECR_TLC
        {
            get
            {
                return DECR_TLCField;
            }
            set
            {
                DECR_TLCField = value;
            }
        }


        private double PRE_ESTIField;
        public double PRE_ESTI
        {
            get
            {
                return PRE_ESTIField;
            }
            set
            {
                PRE_ESTIField = value;
            }
        }


        private double UNI_ESTIField;
        public double UNI_ESTI
        {
            get
            {
                return UNI_ESTIField;
            }
            set
            {
                UNI_ESTIField = value;
            }
        }


        private double VAL_RETORNOField;
        public double VAL_RETORNO
        {
            get
            {
                return VAL_RETORNOField;
            }
            set
            {
                VAL_RETORNOField = value;
            }
        }


        private string FEC_TLCField;
        public string FEC_TLC
        {
            get
            {
                return FEC_TLCField;
            }
            set
            {
                FEC_TLCField = value;
            }
        }

        private bool PartidasquenoseImprimenField;
        public bool PartidasquenoseImprimen
        {
            get
            {
                return PartidasquenoseImprimenField;
            }
            set
            {
                PartidasquenoseImprimenField = value;
            }
        }
    }
}

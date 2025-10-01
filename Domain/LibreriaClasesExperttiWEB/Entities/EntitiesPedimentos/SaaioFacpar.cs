using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos
{
    public class SaaioFacpar
    {
        public string NUM_REFE { get; set; }
        public int CONS_FACT { get; set; }
        public int CONS_PART { get; set; }
        public string NUM_PART { get; set; }
        public string PAI_ORIG { get; set; }
        public string PAI_VEND { get; set; }
        public string FRACCION { get; set; }
        public string CAS_TLCS { get; set; }
        public string COM_TLC { get; set; }
        public double ADVAL { get; set; }
        public double PORC_IVA { get; set; }
        public double MON_FACT { get; set; }
        public double MON_FACT_DLLS { get; set; }
        public string TIP_MONE { get; set; }
        public string TIP_MERC { get; set; }
        public string USO_CARA { get; set; }
        public string CVE_VALO { get; set; }
        public string CVE_VINC { get; set; }
        public double PES_UNIT { get; set; }
        public string UNI_PESO { get; set; }
        public double PES_NETO { get; set; }
        public double CAN_FACT { get; set; }
        public string UNI_FACT { get; set; }
        public double CAN_TARI { get; set; }
        public string UNI_TARI { get; set; }
        public string DES_MERC { get; set; }
        public string OBS_FRAC { get; set; }
        public double VAL_AGRE { get; set; }
        public double VAL_UNIT { get; set; }
        public string FEC_TLC { get; set; }
        public string NUM_PEDIDO { get; set; }
        public int POS_PEDIDO { get; set; }
        public string OBS_ADIC { get; set; }
        public string DECR_TLC { get; set; }
        public string MAR_MERC { get; set; }
        public string SUB_MODE { get; set; }
        public string NUM_SERI { get; set; }
        public double CANT_COVE { get; set; }
        public string UNI_COVE { get; set; }
        public string DESC_COVE { get; set; }
        public string SUB_PART { get; set; }
        public string Ultimo { get; set; }
        public string Primero { get; set; }
        public string Siguiente { get; set; }
        public string Anterior { get; set; }
        public bool Validada { get; set; }
        public string codigoSAT { get; set; }
        public string DescripcionSAT { get; set; }
    }
}

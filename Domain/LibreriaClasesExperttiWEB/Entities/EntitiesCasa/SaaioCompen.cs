using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesCasa
{
    public class SaaioCompen
    {
        private string NUM_REFEField;
        public string NUM_REFE
        {
            get { return NUM_REFEField; }
            set { NUM_REFEField = value; }
        }

        private string NUM_PEDIOField;
        public string NUM_PEDIO
        {
            get { return NUM_PEDIOField; }
            set { NUM_PEDIOField = value; }
        }

        private string CVE_IMPUField;
        public string CVE_IMPU
        {
            get { return CVE_IMPUField; }
            set { CVE_IMPUField = value; }
        }

        private string PAT_ORIGField;
        public string PAT_ORIG
        {
            get { return PAT_ORIGField; }
            set { PAT_ORIGField = value; }
        }

        private string ADU_ORIGField;
        public string ADU_ORIG
        {
            get { return ADU_ORIGField; }
            set { ADU_ORIGField = value; }
        }

        private DateTime FEC_PAGOOField;
        public DateTime FEC_PAGOO
        {
            get { return FEC_PAGOOField; }
            set { FEC_PAGOOField = value; }
        }

        private string TOT_IMPUField;
        public string TOT_IMPU
        {
            get { return TOT_IMPUField; }
            set { TOT_IMPUField = value; }
        }
    }

}

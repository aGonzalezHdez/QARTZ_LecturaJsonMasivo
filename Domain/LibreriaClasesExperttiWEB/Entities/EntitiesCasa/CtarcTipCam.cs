namespace LibreriaClasesAPIExpertti.Entities.EntitiesCasa
{
    using System;

    public class CtarcTipCam
    {
        private DateTime FEC_CAMField;
        public DateTime FEC_CAM
        {
            get
            {
                return FEC_CAMField;
            }
            set
            {
                FEC_CAMField = value;
            }
        }


        private double TIP_CAMField;
        public double TIP_CAM
        {
            get
            {
                return TIP_CAMField;
            }
            set
            {
                TIP_CAMField = value;
            }
        }


    }
}

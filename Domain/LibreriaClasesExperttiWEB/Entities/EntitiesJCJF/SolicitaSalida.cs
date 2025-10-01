using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesJCJF
{
   
        public class SolicitaSalida
    {
            private string pSalpedimentoField;
            public string pSalpedimento
            {
                get { return pSalpedimentoField; }
                set { pSalpedimentoField = value; }
            }

            private string pSalpedimentocveField;
            public string pSalpedimentocve
            {
                get { return pSalpedimentocveField; }
                set { pSalpedimentocveField = value; }
            }

            private string pSaloperacionField;
            public string pSaloperacion
            {
                get { return pSaloperacionField; }
                set { pSaloperacionField = value; }
            }

            private string pSalidaspatenteField;
            public string pSalidaspatente
            {
                get { return pSalidaspatenteField; }
                set { pSalidaspatenteField = value; }
            }

            private string pSalMasterField;
            public string pSalMaster
            {
                get { return pSalMasterField; }
                set { pSalMasterField = value; }
            }

            private string pSalHouseField;
            public string pSalHouse
            {
                get { return pSalHouseField; }
                set { pSalHouseField = value; }
            }

            private double pSalPesoField;
            public double pSalPeso
            {
                get { return pSalPesoField; }
                set { pSalPesoField = value; }
            }

            private int pSalbultosField;
            public int pSalbultos
            {
                get { return pSalbultosField; }
                set { pSalbultosField = value; }
            }

        public string pTransValorPedimento { get; set; }


        private DateTime pSalpfechaField;
            public DateTime pSalpfecha
            {
                get { return pSalpfechaField; }
                set { pSalpfechaField = value; }
            }

            public string TipodePedimento { get; set; }

            public string RFC { get; set; }

            private DateTime pSalpfechaModulacionField;
            public DateTime pSalpfechaModulacion
            {
                get { return pSalpfechaModulacionField; }
                set { pSalpfechaModulacionField = value; }
            }

            private string pSalRemitenteField;
            public string pSalRemitente
            {
                get { return pSalRemitenteField; }
                set { pSalRemitenteField = value; }
            }

            private string pSalRemitenteDirField;
            public string pSalRemitenteDir
            {
                get { return pSalRemitenteDirField; }
                set { pSalRemitenteDirField = value; }
            }

            private string pSalConsignatarioField;
            public string pSalConsignatario
            {
                get { return pSalConsignatarioField; }
                set { pSalConsignatarioField = value; }
            }

            private string pSalConsignatarioDirField;
            public string pSalConsignatarioDir
            {
                get { return pSalConsignatarioDirField; }
                set { pSalConsignatarioDirField = value; }
            }

            private string pSalDestinoField;
            public string pSalDestino
            {
                get { return pSalDestinoField; }
                set { pSalDestinoField = value; }
            }

            private string pSalDescripcionField;
            public string pSalDescripcion
            {
                get { return pSalDescripcionField; }
                set { pSalDescripcionField = value; }
            }
        }

    }

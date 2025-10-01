using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesDespacho
{
    public class ConsolCortes
    {
        private int IdCorteField;
        public int IdCorte
        {
            get { return IdCorteField; }
            set { IdCorteField = value; }
        }

        private int IdOficinaField;
        public int IdOficina
        {
            get { return IdOficinaField; }
            set { IdOficinaField = value; }
        }

        private string NoCorteField;
        public string NoCorte
        {
            get { return NoCorteField; }
            set { NoCorteField = value; }
        }

        private bool CerradoConsolField;
        public bool CerradoConsol
        {
            get { return CerradoConsolField; }
            set { CerradoConsolField = value; }
        }

        private DateTime FechaCierreConsolField;
        public DateTime FechaCierreConsol
        {
            get { return FechaCierreConsolField; }
            set { FechaCierreConsolField = value; }
        }

        private bool CerradoDespachosField;
        public bool CerradoDespachos
        {
            get { return CerradoDespachosField; }
            set { CerradoDespachosField = value; }
        }

        private DateTime FechaCierreDespachosField;
        public DateTime FechaCierreDespachos
        {
            get { return FechaCierreDespachosField; }
            set { FechaCierreDespachosField = value; }
        }

        private bool CerradoCierreField;
        public bool CerradoCierre
        {
            get { return CerradoCierreField; }
            set { CerradoCierreField = value; }
        }

        private DateTime FechadeCierreField;
        public DateTime FechadeCierre
        {
            get { return FechadeCierreField; }
            set { FechadeCierreField = value; }
        }

        private bool CerradoImpresionField;
        public bool CerradoImpresion
        {
            get { return CerradoImpresionField; }
            set { CerradoImpresionField = value; }
        }

        private DateTime FechaImpresionField;
        public DateTime FechaImpresion
        {
            get { return FechaImpresionField; }
            set { FechaImpresionField = value; }
        }

        private bool UnitariasField;
        public bool Unitarias
        {
            get { return UnitariasField; }
            set { UnitariasField = value; }
        }

        private int IdEstacionField;
        public int IdEstacion
        {
            get { return IdEstacionField; }
            set { IdEstacionField = value; }
        }

        private int OperacionField;
        public int Operacion
        {
            get { return OperacionField; }
            set { OperacionField = value; }
        }

        private int IdRegionField;
        public int IdRegion
        {
            get { return IdRegionField; }
            set { IdRegionField = value; }
        }

        private string RegionField;
        public string Region
        {
            get { return RegionField; }
            set { RegionField = value; }
        }
    }
}

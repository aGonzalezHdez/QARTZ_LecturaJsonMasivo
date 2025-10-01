using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesDespacho
{
    public class CatalogodeRegiones
    {
        private int idRegionField;
        public int IdRegion
        {
            get { return idRegionField; }
            set { idRegionField = value; }
        }

        private string regionField;
        public string Region
        {
            get { return regionField; }
            set { regionField = value; }
        }

        private bool activaField;
        public bool Activa
        {
            get { return activaField; }
            set { activaField = value; }
        }

        private string colorField;
        public string Color
        {
            get { return colorField; }
            set { colorField = value; }
        }

        private bool salidasExigePedimentoField;
        public bool SalidasExigePedimento
        {
            get { return salidasExigePedimentoField; }
            set { salidasExigePedimentoField = value; }
        }

        private bool salidasExcluyeField;
        public bool SalidasExcluye
        {
            get { return salidasExcluyeField; }
            set { salidasExcluyeField = value; }
        }

        private bool activarDiaField;
        public bool ActivarDia
        {
            get { return activarDiaField; }
            set { activarDiaField = value; }
        }

        private int idOficinaField;
        public int IdOficina
        {
            get { return idOficinaField; }
            set { idOficinaField = value; }
        }
    }

}

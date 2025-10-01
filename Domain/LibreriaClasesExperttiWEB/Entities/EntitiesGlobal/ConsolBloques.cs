using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesGlobal
{
    public class ConsolBloques
    {
        public int IdBloque { get; set; }
        public string NoBloque { get; set; }

        public int idTipodePedimento { get; set; }

        public int IdPedimento { get; set; }

        public int IDMasterConsol { get; set; }

        public DateTime FechaBloque { get; set; }

        public int Estatus { get; set; }

        public int idSalidasConsol { get; set; }

        public DateTime FechaCierre { get; set; }

        public int Corte { get; set; }

        public int IdRegion { get; set; }

        public bool Automatico { get; set; }

        public bool ftpAutomatico { get; set; }

        public bool ServicioExtraordinario { get; set; }

        public double Acumulado { get; set; }

        public double ValorDlls { get; set;}

        public int IdCorte { get; set; }


    }
}

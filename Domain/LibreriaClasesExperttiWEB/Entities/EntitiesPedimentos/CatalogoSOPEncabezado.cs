using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos
{
    public class CatalogoSOPEncabezado
    {
        public int IDSOP { get; set; }
        public int IdCliente { get; set; }
        public string DescripcionDelObjetivo { get; set; }
        public string Antecedentes { get; set; }
        public string Autor { get; set; }
        public string Documento { get; set; }
        public DateTime FechaDeImplementacion { get; set; }
        public DateTime FechaDeModificacion { get; set; }
    }
}

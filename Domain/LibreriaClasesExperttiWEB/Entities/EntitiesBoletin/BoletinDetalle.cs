using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesBoletin
{
    public class BoletinDetalle
    {
        public int IdBoletinDetalle { get; set; }
        public int? IdBoletin { get; set; }
        public string Descripcion { get; set; }
        public bool? activo { get; set; }
        public int? idUsuario { get; set; }
        public DateTime? fechaAlta { get; set; }
        public int? Consecutivo { get; set; }
    }

    public class BoletinDetalleInsert
    {
        public int? IdBoletin { get; set; }
        public string Descripcion { get; set; }
        public bool? activo { get; set; }
        public int? idUsuario { get; set; }
        public int? Consecutivo { get; set; }
    }

    public class BoletinDetalleUpdate
    {
        public int IdBoletinDetalle { get; set; }
        public int? IdBoletin { get; set; }
        public string Descripcion { get; set; }
        public bool? activo { get; set; }
        public int? idUsuario { get; set; }
        public int? Consecutivo { get; set; }
    }
}

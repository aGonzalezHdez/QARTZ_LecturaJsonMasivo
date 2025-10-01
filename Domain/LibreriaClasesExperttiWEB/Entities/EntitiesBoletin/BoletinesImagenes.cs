using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesBoletin
{
    public class BoletinesImagenes
    {
        public int IdBoletinImagen { get; set; }
        public int? IdBoletinDetalle { get; set; }
        public string RUTAS3 { get; set; }
        public bool? activo { get; set; }
        public int? idUsuario { get; set; }
        public DateTime? fechaAlta { get; set; }
    }

    public class BoletinesImagenesDTO
    {
        public int IdBoletin { get; set; }
        public int IdBoletinDetalle { get; set; }
        public string ArchivoBase64 { get; set; }
        public string Extencion { get; set; }
        public bool activo { get; set; }
        public int idUsuario { get; set; }
    }
}

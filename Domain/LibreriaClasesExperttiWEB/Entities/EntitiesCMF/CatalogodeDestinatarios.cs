using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesCMF
{
    public class CatalogodeDestinatarios
    {
        public int IdDestinatario { get; set; }
        public string Nombre { get; set; }
        [Required]
        public string RFC { get; set; }
        [Required]
        public string CURP { get; set; }
        public string NSS { get; set; }
        public string Telefono { get; set; }
        [Required]
        public string CorreoElectronico { get; set; }
        [Required]
        public string Contacto { get; set; }
        public DateTime FechaDeAlta { get; set; }
        public int IDCapturo { get; set; }
        public bool Activo { get; set; }
    }
}

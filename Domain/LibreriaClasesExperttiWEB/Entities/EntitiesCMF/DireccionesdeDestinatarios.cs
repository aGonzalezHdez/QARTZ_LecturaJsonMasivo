using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesCMF
{
    public class DireccionesdeDestinatarios
    {
        public int IdDestinatario { get; set; }
        public int IDDireccion { get; set; }
        public string Calle { get; set; }
        [Required]
        public string Colonia { get; set; }
        [Required]
        public string MunicipioAlcandia { get; set; }
        [Required]
        public string CodigoPostal { get; set; }
        [Required]
        public string NumeroExt { get; set; }
        [Required]
        public string NumeroInt { get; set; }
        public string Ciudad { get; set; }
        [Required]
        public string ClaveEntidadFederativa { get; set; }
        [Required]
        public bool Activo { get; set; }
        public string Localidad { get; set; }
        [Required]
        public int IdCaptura { get; set; }
        public DateTime FechaAlta { get; set; }

    }
}

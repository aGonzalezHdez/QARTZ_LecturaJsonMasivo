using System.ComponentModel.DataAnnotations;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class CuentasPECE
    {
        [Required]
        public string ClaveCliente { get; set; }
        //[Required]
        public string CVE_CTA { get; set; }
        [Required]
        public string NUM_CTA { get; set; }
        [Required]
        public string DES_CTA { get; set; }

        [Required]
        public string CVE_BAN { get; set; }

        [Required]
        public string CTA_CENT { get; set; }
        public string PAT_AA { get; set; } = null!;
        public string CVE_ADUA { get; set; } = null!;

        [Required]
        public string IMP_EXPO { get; set; }
        //[Required]
        public string ALC_IMP { get; set; }

        [Required]
        public int IDDatosDeEmpresa { get; set; }

        public int IdOficina { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        public bool CtaPropia { get; set; }

        public string UsuarioAlta { get; set; }

        public DateTime FechaDeAlta { get; set; }

        public bool modifica { get; set; }

    }

}
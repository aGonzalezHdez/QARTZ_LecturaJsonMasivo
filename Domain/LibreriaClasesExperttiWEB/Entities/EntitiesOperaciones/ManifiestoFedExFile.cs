using System.ComponentModel.DataAnnotations;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones
{
    public class ManifiestoFile
    {
        [Required]
        public string ArchivoBase64 { get; set; }
        [Required]
        public string NombreArchivo { get; set; }
        [Required]
        public int IDDatosDeEmpresa { get; set; }
        [Required]
        public int IdOficina { get; set; }
        [Required]
        public int IdUsuario { get; set; }

        public string TxtGuiaMaster { get; set; } = null;

        [Required]
        public int idManifiesto { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesToken
{
    public class UsuarioLogin
    {
        [StringLength(maximumLength: 30, ErrorMessage = "El campo {0} no debe de tener mas de {1} carácteres")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string PasssEncryp { get; set; } = string.Empty;
        public int IdPantalla { get; set; }
    }
}

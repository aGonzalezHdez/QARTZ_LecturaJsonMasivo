using System.ComponentModel.DataAnnotations;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class CuentasDHL
    {
        public int idCuenta { get; set; }
        [Required]
        public int IdCliente { get; set; }
        [Required]
        public string NumerodeCuenta { get; set; } = null!;
    }

}
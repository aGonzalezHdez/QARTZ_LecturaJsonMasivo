using System.ComponentModel.DataAnnotations;
namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{

    public class ClientesyOficinas
    {
        public int IdClientesYOficinas { get; set; }
        [Required]
        public int IdCliente { get; set; }
        [Required]
        public int IdOficina { get; set; }
        [Required]
        public int StatusClienteOficina { get; set; }
        [Required]
        public int Operacion { get; set; }
        //public string EmailPdfCASA { get; set; } = null!;
    }

}
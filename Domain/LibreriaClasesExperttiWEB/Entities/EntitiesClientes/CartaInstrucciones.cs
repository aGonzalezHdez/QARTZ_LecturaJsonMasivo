namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    using System.ComponentModel.DataAnnotations;

    public class CartaInstrucciones
    {
        public int IDCarta { get; set; }

        [Required]
        public int IdCliente { get; set; }
        public string NombreDelCliente { get; set; } = null!;
        public decimal ValorInicialEnDolares { get; set; }
        public decimal ValorFinalEnDolares { get; set; }

        [Required]
        public int IDCategoria { get; set; }
        public string CategoriaDescripcion { get; set; } = null!;
        public string Grupo { get; set; } = null!;
        public string ClavedePedimento { get; set; } = null!;

        [Required]
        public int Patente { get; set; }
        public string NombreAA { get; set; } = null!;
        public string Observaciones { get; set; } = null!;
        public bool? Activa { get; set; }
        ////public DateTime FechaAlta { get; set; }

        [Required]
        public int Operacion { get; set; }
        public DateTime FechaVigencia { get; set; }
        public string Email { get; set; } = null!;

        [Required]
        public int IDDatosDeEmpresa { get; set; }

        [Required]
        public int IdOficina { get; set; }
    }

}
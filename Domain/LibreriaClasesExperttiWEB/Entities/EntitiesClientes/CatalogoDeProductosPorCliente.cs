namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    using System.ComponentModel.DataAnnotations;

    public class CatalogoDeProductosPorCliente
    {
        public int IDProductoPorCliente { get; set; }

        [StringLength(8, MinimumLength = 8, ErrorMessage = "La longitud de la fracción debe ser de 8 caracteres.")]
        public string? Fraccion { get; set; }
        public int IDCliente { get; set; }
        public string? CodigoDelProducto { get; set; }
        public string? DescripcionDelProducto { get; set; }
        public string? DescripcionEnIngles { get; set; }
        public string? ObservacionesDelProducto { get; set; }
        public string? UnidadDeMedidaDelProducto { get; set; }
        public string? PaisDeOrigen { get; set; }

        [Required]
        public int Activo { get; set; }
        public DateTime FechaDeAltaDeProducto { get; set; }
        public int IdUsuarioAlta { get; set; }
        public string? FraccionEspecifica { get; set; }
        public string? DescripcionEspecifica { get; set; } 
        public double Peso { get; set; }
        public string? NICO { get; set; }
        public string? DesInglesEspecifica { get; set; }
        public string? NICOEspecifico { get; set; } 
        public string? RequisitosEspecificos { get; set; } 
        public bool? NoRealizarPrevio { get; set; }
        public string? ClaveProductoSAT { get; set; }
        public string? DescripcionSAT { get; set; } 
        public string? Proveedor { get; set; }
        public int Tipo { get; set; }

    }

}
namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class DireccionesDeClientes
    {
        public int IDDireccion { get; set; }
        public int IDCliente { get; set; }

        public string? Direccion { get; set; }

        public string? Colonia { get; set; }

        public string? Poblacion { get; set; }

        public string? CodigoPostal { get; set; }

        public string? NumeroExt { get; set; }

        public string? NumeroInt { get; set; } 

        public string? EntreLaCalleDe { get; set; }

        public string? YDe { get; set; } 

        //public string ClaveEntidadFederativa { get; set; } = null!;
        public string? Entidad { get; set; }

        public bool? Activo { get; set; } = false!;
        public int Orden { get; set; }

        public string? Localidad { get; set; }
    }
}

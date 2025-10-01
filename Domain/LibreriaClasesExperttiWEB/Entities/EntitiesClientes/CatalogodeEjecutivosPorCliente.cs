namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class CatalogodeEjecutivosPorCliente
    {
        public int Id { get; set; }

        public string Principal { get; set; }

        public string Respaldo { get; set; }

        public string Oficina { get; set; }

        public string Operacion { get; set; }

        public int IdDepartamento { get; set; }

        public string NombreDepartamento { get; set; }

        public bool Modificar { get; set; }


    }
}

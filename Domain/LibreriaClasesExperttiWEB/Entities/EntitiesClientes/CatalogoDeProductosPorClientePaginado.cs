namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class CatalogoDeProductosPorClientePaginado
    {
        public int IDProductoPorCliente { get; set; }
        public string Fraccion { get; set; }
        public string Nico { get; set; }
        public string CodigoDelProducto { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionEnIngles { get; set; }
        public string Observaciones { get; set; }
        public string UsuarioAlta { get; set; }
        public int RowNumber { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

}
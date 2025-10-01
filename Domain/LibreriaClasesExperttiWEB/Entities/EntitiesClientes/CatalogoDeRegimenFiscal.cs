namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class CatalogoDeRegimenFiscal
    {
        public int IdRegimenFiscal { get; set; }
        public string Clave { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public bool Fisica { get; set; }
        public bool Moral { get; set; }
    }
}

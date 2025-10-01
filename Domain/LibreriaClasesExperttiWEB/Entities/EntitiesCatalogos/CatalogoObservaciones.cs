namespace LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos
{
    public class CatalogoObservaciones
    {
        public int IdDepartamento { get; set; }
        public string Departamento { get; set; }
        public List<CatalogodeCheckPoints> CheckPoints { get; set; }
    }

}

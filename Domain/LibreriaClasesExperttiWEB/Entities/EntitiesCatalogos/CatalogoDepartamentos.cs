namespace LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos
{
    public class CatalogoDepartamentos
    {
        public int IdDepartamento { get; set; }
        public string NombreDepartamento { get; set; } = null!;
        public bool Tipoo { get; set; } = false;
        public int ClaveDepartamento { get; set; }
        public int Administrativo { get; set; }
        public int GrupoDeCorreoElectronico { get; set; }
        public string CampoReferencias { get; set; } = null!;
        public string CampoIdTable { get; set; } = null!;
        public int IdDepartamentosAsignados { get; set; }

    }
}

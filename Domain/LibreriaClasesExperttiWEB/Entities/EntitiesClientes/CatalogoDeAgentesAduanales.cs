namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class CatalogoDeAgentesAduanales
    {
        public int IdAgenteAduanal { get; set; }
        public int Patente { get; set; }
        public string Nombre { get; set; } = null!;
        public string Rfc { get; set; } = null!;
        public bool Activo { get; set; }
        public string Prefijo { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string ApellidoPaterno { get; set; } = null!;
        public string ApellidoMaterno { get; set; } = null!;
        public string CURP { get; set; } = null!;
        public string ClavedeRepresentante { get; set; } = null!;
        public string EmpresaFactura { get; set; } = null!;
        public string PasswordValidacion { get; set; } = null!;
        public string PasswordPago { get; set; } = null!;
        public string CiecUsuario { get; set; } = null!;
        public string CiecPassword { get; set; } = null!;
        public bool? PatentesLocales { get; set; }
    }

}
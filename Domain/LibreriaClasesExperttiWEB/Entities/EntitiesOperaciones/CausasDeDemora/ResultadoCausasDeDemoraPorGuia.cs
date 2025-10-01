namespace LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones.CausasDeDemora
{
    public class ResultadoCausasDeDemoraPorGuia
    {
        public int IdDemoraGuia { get; set; }
        public string Guia { get; set; }
        public int IdCausante { get; set; }
        public string Causante { get; set; }
        public int IdSubCausante { get; set; }
        public string SubCausante { get; set; }
        public int IdCausa { get; set; }
        public string Causa { get; set; }
        public int IdUsuario { get; set; }
        public string FechaRegistro { get; set; }
        public int IdDatosdeEmpresa { get; set; }
    }
}

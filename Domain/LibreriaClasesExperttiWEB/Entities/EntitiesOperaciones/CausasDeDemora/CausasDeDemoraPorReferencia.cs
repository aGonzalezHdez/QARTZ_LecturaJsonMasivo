namespace LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones.CausasDeDemora
{
    public class CausasDeDemoraPorReferencia
    {

        public string Referencia { get; set; }

        public List<string> Guias { get; set; }

        public int IdCausa { get; set; }

        public int IdUsuario { get; set; }

        public string FechaRegistro { get; set; }

        public int IdDatosdeEmpresa { get; set; }

    }
}

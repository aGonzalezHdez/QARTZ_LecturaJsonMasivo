using System;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones.CausasDeDemora
{
    public class CausasDeDemoraPorGuia
    {
        public int IdDemoraGuia { get; set; }

        public string Guia { get; set; }

        public int IdCausa { get; set; }

        public int IdUsuario { get; set; }

        public string FechaRegistro { get; set; }

        public int IdDatosdeEmpresa { get; set; }
    }
}

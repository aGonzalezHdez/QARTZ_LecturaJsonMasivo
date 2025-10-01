using System.ComponentModel.DataAnnotations;
namespace LibreriaClasesAPIExpertti.Entities.EntitiesTrazabilidadPorRampaFedex
{
    public class CatalogodeEstaciones
    {
        public int IdEstacion { get; set; }

        public string Estacion { get; set; } = null!;

        public string GTW { get; set; } = null!;

        public int TiempoTransGTWMin { get; set; }

        public int IdOficina { get; set; }

    }
}

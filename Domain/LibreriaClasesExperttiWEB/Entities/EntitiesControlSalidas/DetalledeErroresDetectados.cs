namespace LibreriaClasesAPIExpertti.Entities.EntitiesControlSalidas
{
    public class DetalledeErroresDetectados
    {

        public int idDetalle { get; set; }

        public int idError { get; set; }

        public int IdReferencia { get; set; }

        public int? idUsuarioDetecta { get; set; }

        public int? idUsuarioError { get; set; }

        public DateTime? Fecha { get; set; }


    }
}

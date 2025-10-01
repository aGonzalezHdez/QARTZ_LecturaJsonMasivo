namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class CartaInstruccionesIdEmpresa
    {
        public int IDCarta { get; set; }
        public decimal ValorInicialEnDolares { get; set; }
        public decimal ValorFinalEnDolares { get; set; }
        public string Categoria { get; set; }
        public string Grupo { get; set; }
        public string ClavedePedimento { get; set; }
        public int Operacion { get; set; }
        public int Patente { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaVigencia { get; set; }
        public string Email { get; set; }
        public int IdOficina { get; set; }
    }
}

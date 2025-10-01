namespace LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones
{
    public class CustomsAlerts
    {
        public int IdCustomAlert { get; set; }
        public int IdCustomAlertsBaby { get; set; }

        public string GuiaHouse { get; set; }

        public double ValorEnDolares { get; set; }

        public double PesoTotal { get; set; }

        public string Descripcion { get; set; }

        public string Cliente { get; set; }

        public string OrigenIata { get; set; }

        public string DestinoIata { get; set; }

        public string GuiaMaster { get; set; }

        public DateTime FechaDeEntrada { get; set; }

        public DateTime FechaDeAlta { get; set; }

        public int IdUsuario { get; set; }

        public int IdCategoria { get; set; }

        public int IdCliente { get; set; }

        public string DescripcionEspanol { get; set; }

        public int IdTipodePedimento { get; set; }

        public string Remitente { get; set; }

        public int Piezas { get; set; }

        public bool ProveedorConfiable { get; set; }

        public bool Detener { get; set; }

        public bool DetenidaporCliente { get; set; }

        public bool HoyMismo { get; set; }

        public string ClavedePedimento { get; set; }

        public int IdNotificador { get; set; }

        public int Patente { get; set; }

        public int IdRiel { get; set; }

        public double ValorDolaresCA { get; set; }

        public int IdRielWEC { get; set; }

        public int IDDatosDeEmpresa { get; set; }

        public double ValorMe { get; set; }

        public string Moneda { get; set; }

        public double Fletes { get; set; }

        public string FacturacionFx { get; set; }

        public string RFCCLIENTE { get; set; }

        public int idRielWEC { get; set; }

        public bool PrevioJCJF { get; set; }

    }
}

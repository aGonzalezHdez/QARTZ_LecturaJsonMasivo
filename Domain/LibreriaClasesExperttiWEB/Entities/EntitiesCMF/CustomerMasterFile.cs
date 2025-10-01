using Amazon.Runtime.Internal.Transform;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesCMF
{
    public class CustomerMasterFile
    {
        public int IdCMF { get; set; }

        public string GuiaHouse { get; set; }

        public string GtwDestino { get; set; }

        public string IataOrigen { get; set; }

        public string IataDestino { get; set; }

        public string TipoEnvio { get; set; }

        public string Descripcion { get; set; }

        public string NoCuenta { get; set; }

        public string Destinatario { get; set; }

        public string Direccion1 { get; set; }

        public string Direccion2 { get; set; }

        public string Direccion3 { get; set; }

        public string Ciudad { get; set; }

        public string CodigoPostal { get; set; }

        public string Pais { get; set; }

        public string Contacto { get; set; }

        public string MedioContacto { get; set; }

        public string DatosContacto { get; set; }

        public string Proveedor { get; set; }

        public string ProveedorDireccion { get; set; }

        public string ProveedorInterior { get; set; }

        public string ProveedorCiudad { get; set; }

        public string ProveedorEstado { get; set; }

        public string ProveedorPais { get; set; }

        public string ProveedorCodigoPostal { get; set; }

        public string ProveedorMedio { get; set; }

        public string ProveedorDatos { get; set; }

        public double Peso { get; set; }

        public double PesoVolumetrico { get; set; }

        public int Piezas { get; set; }

        public string Incoterm { get; set; }

        public string ServicioDhl { get; set; }

        public double FacturaValor { get; set; }

        public string FacturaMoneda { get; set; }

        public string PaisVendedor { get; set; }

        public string PaisComprador { get; set; }

        public string NombredeArchivo { get; set; }

        public DateTime FechaAlta { get; set; }

        public int IdCliente { get; set; }

        public int IdCategoria { get; set; }

        public string ClavedePedimento { get; set; }

        public string Patente { get; set; }

        public bool ValidarCliente { get; set; }

        public int IdRiel { get; set; }

        public bool Detener { get; set; }

        public bool ExisteGuia { get; set; }

        public bool ExisteFactura { get; set; }

        public double ValorDolares { get; set; }

        public string DescripcionEspanol { get; set; }

        public int idTipodePedimento { get; set; }

        public bool XmlEnviado { get; set; }

        public int IdUsuarioAsignado { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool PreCaptura { get; set; }

        public bool EnviarXML { get; set; }

        public int idRielWEC { get; set; }

        public DateTime EnvioDHL { get; set; }

        public DateTime EnvioWEC { get; set; }

        public bool XMLEnviadoWEC { get; set; }

        public string GuiaMaster { get; set; }

        public string ShipmentReference { get; set; }

        public string NoCuentaCliente { get; set; }

        public bool Cotizacion { get; set; }

        public double Frght { get; set; }

        public string FrghtCrncy { get; set; }

        public bool ProvConfiable { get; set; }

        public bool ASIGNADAPRECA { get; set; }

        public int idOficina { get; set; }

        public string AduanaDespacho { get; set; }

        public bool Gia { get; set; }

        public int Partidas { get; set; }

        public string? TaxIDImpo { get; set; }

      

    }
}

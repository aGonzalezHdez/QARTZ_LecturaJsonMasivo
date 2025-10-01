using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Services.Turnado
{
    public class Factura
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;
        private bool vCasa;
        private SaaioFacturRepository objFacturaCasa;
        private SaaioFacturExperttiRepository objFacturaExpertti;
        public Factura(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public Factura(bool Casa)
        {
            objFacturaCasa = new SaaioFacturRepository(_configuration);
            objFacturaExpertti = new SaaioFacturExperttiRepository(_configuration);
            vCasa = Casa;
        }

        public List<SaaioFactur> Cargar(string NumerodeReferencia)
        {
            var lstSAAIO_FACTUR = new List<SaaioFactur>();

            if (vCasa)
            {
                lstSAAIO_FACTUR = objFacturaCasa.Cargar(NumerodeReferencia);
            }
            else
            {
                lstSAAIO_FACTUR = objFacturaExpertti.Cargar(NumerodeReferencia);
            }

            return lstSAAIO_FACTUR;
        }

        public SaaioFactur Buscar(string MyNum_refe, int MyCons_Fact)
        {
            var objSAAIO_FACTUR = new SaaioFactur();

            if (vCasa)
            {
                objSAAIO_FACTUR = objFacturaCasa.Buscar(MyNum_refe, MyCons_Fact);
            }
            else
            {
                objSAAIO_FACTUR = objFacturaExpertti.Buscar(MyNum_refe, MyCons_Fact);
            }

            return objSAAIO_FACTUR;
        }

        public SaaioFactur Buscar(string NUM_REFE, string NUM_FACT)
        {
            var objSAAIO_FACTUR = new SaaioFactur();

            if (vCasa)
            {
                objSAAIO_FACTUR = objFacturaCasa.Buscar(NUM_REFE, NUM_FACT);
            }
            else
            {
                objSAAIO_FACTUR = objFacturaExpertti.Buscar(NUM_REFE, NUM_FACT);
            }

            return objSAAIO_FACTUR;
        }

        public int Insertar(SaaioFactur lsaaio_factur, int IdUsuario)
        {
            int id = 0;

            if (vCasa)
            {
                id = objFacturaCasa.Insertar(lsaaio_factur);
            }
            else
            {
                id = objFacturaExpertti.Insertar(lsaaio_factur, IdUsuario);
            }

            return id;
        }

        public int Modificar(SaaioFactur lsaaio_factur, int IdUsuario)
        {
            int id = 0;

            if (vCasa)
            {
                id = objFacturaCasa.Modificar(lsaaio_factur);
            }
            else
            {
                id = objFacturaExpertti.Modificar(lsaaio_factur, IdUsuario);
            }

            return id;

        }

        public int Modificar(string NumerodeReferencia, string CveProveedor)

        {
            int id = 0;

            if (vCasa)
            {
                id = objFacturaCasa.Modificar(NumerodeReferencia, CveProveedor);
            }
            else
            {
                id = objFacturaExpertti.Modificar(NumerodeReferencia, CveProveedor);
            }

            return id;

        }

        public bool BorraFacturaDelCasa(string MyNum_Refe, int MyCons_Fact)
        {
            bool Elimino = false;
            if (vCasa)
            {
                Elimino = objFacturaCasa.BorraFacturaDelCasa(MyNum_Refe, MyCons_Fact);
            }
            else
            {
                Elimino = objFacturaExpertti.BorraFacturaDelCasa(MyNum_Refe, MyCons_Fact);
            }

            return Elimino;
        }

        public TotalDeFacturaParaCOVE TotalDeFactura(string MyNum_Refe, int MyCons_fact)
        {
            var ObjTotalDeFacturaParaCove = new TotalDeFacturaParaCOVE();

            if (vCasa)
            {
                ObjTotalDeFacturaParaCove = objFacturaCasa.TotalDeFactura(MyNum_Refe, MyCons_fact);
            }
            else
            {
                ObjTotalDeFacturaParaCove = objFacturaExpertti.TotalDeFactura(MyNum_Refe, MyCons_fact);
            }

            return ObjTotalDeFacturaParaCove;
        }


        public DataTable CargarFacturas(string MyNum_Refe)
        {
            var dtb = new DataTable();
            if (vCasa)
            {
                dtb = objFacturaCasa.CargarFacturas(MyNum_Refe);
            }
            else
            {
                dtb = objFacturaExpertti.CargarFacturas(MyNum_Refe);
            }

            return dtb;
        }

        public DataTable VerTodasLasFacturas(string MyNum_Refe)
        {
            var dtb = new DataTable();
            if (vCasa)
            {
                dtb = objFacturaCasa.VerTodasLasFacturas(MyNum_Refe);
            }
            else
            {
                dtb = objFacturaExpertti.VerTodasLasFacturas(MyNum_Refe);
            }

            return dtb;
        }

        public int EXTRAE_MAX_CONS_FACT(string MyNum_Refe)
        {
            int id;
            if (vCasa)
            {
                id = objFacturaCasa.EXTRAE_MAX_CONS_FACT(MyNum_Refe);
            }
            else
            {
                id = objFacturaExpertti.EXTRAE_MAX_CONS_FACT(MyNum_Refe);
            }

            return id;
        }

        public bool ModificarVinculacion(int IDReferencia, int ConsFact)
        {
            bool Modifico;

            if (vCasa)
            {
                Modifico = objFacturaCasa.ModificarVinculacion(IDReferencia, ConsFact);
            }
            else
            {
                Modifico = objFacturaExpertti.ModificarVinculacion(IDReferencia, ConsFact);
            }

            return Convert.ToBoolean(Modifico);
        }

        public DataTable BuscarValordeTransaccion(string NumRefe)
        {
            var dtb = new DataTable();
            if (vCasa)
            {
                dtb = objFacturaCasa.BuscarValordeTransaccion(NumRefe);
            }
            else
            {
                dtb = objFacturaExpertti.BuscarValordeTransaccion(NumRefe);
            }
            return dtb;
        }

        public string BuscaProveedorParaReferencia(string MyNum_Refe)
        {
            string Proveedor = string.Empty;
            if (vCasa)
            {
                Proveedor = objFacturaCasa.BuscaProveedorParaReferencia(MyNum_Refe);
            }
            else
            {
                Proveedor = objFacturaExpertti.BuscaProveedorParaReferencia(MyNum_Refe);
            }

            return Proveedor;
        }

        public int InsertarNew(SaaioFactur lsaaio_factur)
        {
            int id = 0;
            if (vCasa)
            {
                id = objFacturaCasa.InsertarNew(lsaaio_factur);
            }
            else
            {
                id = objFacturaExpertti.InsertarNew(lsaaio_factur);
            }

            return id;
        }

    }
}

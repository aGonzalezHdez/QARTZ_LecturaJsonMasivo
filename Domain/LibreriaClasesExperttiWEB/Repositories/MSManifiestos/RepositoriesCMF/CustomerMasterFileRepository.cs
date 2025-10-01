using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesCMF;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using NPOI.SS.Formula.Functions;

namespace LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF
{
    public class CustomerMasterFileRepository : ICustomerMasterFileRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public CustomerMasterFileRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public int Insertar(CustomerMasterFile lcustomermasterfile)
        {
            int id;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_INSERT_CUSTOMERMASTERFILE_New2";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            // ,@GuiaHouse  varchar
            param = cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 25);
            param.Value = lcustomermasterfile.GuiaHouse;

            // ,@GtwDestino  varchar
            param = cmd.Parameters.Add("@GtwDestino", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.GtwDestino;

            // ,@IataOrigen  varchar
            param = cmd.Parameters.Add("@IataOrigen", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.IataOrigen;

            // ,@IataDestino  varchar
            param = cmd.Parameters.Add("@IataDestino", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.IataDestino;

            // ,@TipoEnvio  varchar
            param = cmd.Parameters.Add("@TipoEnvio", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.TipoEnvio;

            // ,@Descripcion  varchar
            param = cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, -1);
            param.Value = lcustomermasterfile.Descripcion;

            // ,@NoCuenta  varchar
            param = cmd.Parameters.Add("@NoCuenta", SqlDbType.VarChar, 15);
            param.Value = lcustomermasterfile.NoCuenta;

            // ,@Destinatario  varchar
            param = cmd.Parameters.Add("@Destinatario", SqlDbType.VarChar, -1);
            param.Value = lcustomermasterfile.Destinatario;

            // ,@Direccion1  varchar
            param = cmd.Parameters.Add("@Direccion1", SqlDbType.VarChar, -1);
            param.Value = lcustomermasterfile.Direccion1;

            // ,@Direccion2  varchar
            param = cmd.Parameters.Add("@Direccion2", SqlDbType.VarChar, -1);
            param.Value = lcustomermasterfile.Direccion2;

            // ,@Direccion3  varchar
            param = cmd.Parameters.Add("@Direccion3", SqlDbType.VarChar, -1);
            param.Value = lcustomermasterfile.Direccion3;

            // ,@Ciudad  varchar
            param = cmd.Parameters.Add("@Ciudad", SqlDbType.VarChar, -1);
            param.Value = lcustomermasterfile.Ciudad;

            // ,@CodigoPostal  varchar
            param = cmd.Parameters.Add("@CodigoPostal", SqlDbType.VarChar, 5);
            param.Value = lcustomermasterfile.CodigoPostal;

            // ,@Pais  varchar
            param = cmd.Parameters.Add("@Pais", SqlDbType.VarChar, 2);
            param.Value = lcustomermasterfile.Pais;

            // ,@Contacto  varchar
            param = cmd.Parameters.Add("@Contacto", SqlDbType.VarChar, 100);
            param.Value = lcustomermasterfile.Contacto;

            // ,@MedioContacto  varchar
            param = cmd.Parameters.Add("@MedioContacto", SqlDbType.VarChar, 10);
            param.Value = lcustomermasterfile.MedioContacto;


            // ,@DatosContacto  varchar
            param = cmd.Parameters.Add("@DatosContacto", SqlDbType.VarChar, 30);
            param.Value = lcustomermasterfile.DatosContacto;

            // ,@Proveedor  varchar
            param = cmd.Parameters.Add("@Proveedor", SqlDbType.VarChar, 100);
            param.Value = lcustomermasterfile.Proveedor;

            // ,@ProveedorDireccion  varchar
            param = cmd.Parameters.Add("@ProveedorDireccion", SqlDbType.VarChar, 100);
            param.Value = lcustomermasterfile.ProveedorDireccion;

            // ,@ProveedorInterior  varchar
            param = cmd.Parameters.Add("@ProveedorInterior", SqlDbType.VarChar, 100);
            param.Value = lcustomermasterfile.ProveedorInterior;

            // ,@ProveedorCiudad  varchar
            param = cmd.Parameters.Add("@ProveedorCiudad", SqlDbType.VarChar, 100);
            param.Value = lcustomermasterfile.ProveedorCiudad;

            // ,@ProveedorEstado  varchar
            param = cmd.Parameters.Add("@ProveedorEstado", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.ProveedorEstado;

            // ,@ProveedorPais  varchar
            param = cmd.Parameters.Add("@ProveedorPais", SqlDbType.VarChar, 50);
            param.Value = lcustomermasterfile.ProveedorPais;

            // ProveedorCodigoPostal varchar
            param = cmd.Parameters.Add("@ProveedorCodigoPostal", SqlDbType.VarChar, 15);
            param.Value = lcustomermasterfile.ProveedorCodigoPostal;

            // ,@ProveedorMedio  varchar
            param = cmd.Parameters.Add("@ProveedorMedio", SqlDbType.VarChar, 50);
            param.Value = lcustomermasterfile.ProveedorMedio;

            // ,@ProveedorDatos  varchar
            param = cmd.Parameters.Add("@ProveedorDatos", SqlDbType.VarChar, 30);
            param.Value = lcustomermasterfile.ProveedorDatos;

            // ,@Peso  money
            param = cmd.Parameters.Add("@Peso", SqlDbType.Money, 4);
            param.Value = lcustomermasterfile.Peso;

            // ,@PesoVolumetrico  money
            param = cmd.Parameters.Add("@PesoVolumetrico", SqlDbType.Money, 4);
            param.Value = lcustomermasterfile.PesoVolumetrico;

            // ,@Piezas  int
            param = cmd.Parameters.Add("@Piezas", SqlDbType.Int, 4);
            param.Value = lcustomermasterfile.Piezas;

            // ,@Incoterm  varchar
            param = cmd.Parameters.Add("@Incoterm", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.Incoterm;

            // ,@ServicioDhl  varchar
            param = cmd.Parameters.Add("@ServicioDhl", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.ServicioDhl;

            // ,@FacturaValor  money
            param = cmd.Parameters.Add("@FacturaValor", SqlDbType.Money, 4);
            param.Value = lcustomermasterfile.FacturaValor;

            // ,@FacturaMoneda  varchar
            param = cmd.Parameters.Add("@FacturaMoneda", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.FacturaMoneda;

            // ,@PaisVendedor  varchar
            param = cmd.Parameters.Add("@PaisVendedor", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.PaisVendedor;

            // ,@PaisComprador  varchar
            param = cmd.Parameters.Add("@PaisComprador", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.PaisComprador;

            // @NombredeArchivo
            param = cmd.Parameters.Add("@NombredeArchivo", SqlDbType.VarChar, 50);
            param.Value = lcustomermasterfile.NombredeArchivo;

            param = cmd.Parameters.Add("@ShipmentReference", SqlDbType.VarChar, 30);
            param.Value = lcustomermasterfile.ShipmentReference;

            param = cmd.Parameters.Add("@NoCuentaCliente", SqlDbType.VarChar, 30);
            param.Value = lcustomermasterfile.NoCuentaCliente;

            param = cmd.Parameters.Add("@Frght", SqlDbType.Money);
            param.Value = Convert.ToDouble(lcustomermasterfile.Frght);

            param = cmd.Parameters.Add("@FrghtCrncy", SqlDbType.VarChar, 5);
            param.Value = lcustomermasterfile.FrghtCrncy;

            param = cmd.Parameters.Add("@Partidas", SqlDbType.Int, 4);
            param.Value = lcustomermasterfile.Partidas;

            param = cmd.Parameters.Add("@TaxIDImpo", SqlDbType.VarChar, 50);
            param.Value = lcustomermasterfile.TaxIDImpo;


            param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;


            try
            {

                using (cmd.ExecuteReader())
                {

                    if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                    {
                        id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                    }
                    else
                    {
                        id = 0;
                    }
                }

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_CUSTOMERMASTERFILE_New2");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return id;
        }

        public int ModificarSimple(CustomerMasterFile lcustomermasterfile)
        {
            int id;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_CustomerMasterFile_Simple";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            param = cmd.Parameters.Add("@IdCMF", SqlDbType.Int, 4);
            param.Value = lcustomermasterfile.IdCMF;

            // ,@GuiaHouse  varchar
            param = cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 25);
            param.Value = lcustomermasterfile.GuiaHouse;

            // ,@GtwDestino  varchar
            param = cmd.Parameters.Add("@GtwDestino", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.GtwDestino;

            // ,@IataOrigen  varchar
            param = cmd.Parameters.Add("@IataOrigen", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.IataOrigen;

            // ,@IataDestino  varchar
            param = cmd.Parameters.Add("@IataDestino", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.IataDestino;

            // ,@TipoEnvio  varchar
            param = cmd.Parameters.Add("@TipoEnvio", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.TipoEnvio;

            // ,@Descripcion  varchar
            param = cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, -1);
            param.Value = lcustomermasterfile.Descripcion;

            // ,@NoCuenta  varchar
            param = cmd.Parameters.Add("@NoCuenta", SqlDbType.VarChar, 15);
            param.Value = lcustomermasterfile.NoCuenta;

            // ,@Destinatario  varchar
            param = cmd.Parameters.Add("@Destinatario", SqlDbType.VarChar, -1);
            param.Value = lcustomermasterfile.Destinatario;

            // ,@Direccion1  varchar
            param = cmd.Parameters.Add("@Direccion1", SqlDbType.VarChar, -1);
            param.Value = lcustomermasterfile.Direccion1;

            // ,@Direccion2  varchar
            param = cmd.Parameters.Add("@Direccion2", SqlDbType.VarChar, -1);
            param.Value = lcustomermasterfile.Direccion2;

            // ,@Direccion3  varchar
            param = cmd.Parameters.Add("@Direccion3", SqlDbType.VarChar, -1);
            param.Value = lcustomermasterfile.Direccion3;

            // ,@Ciudad  varchar
            param = cmd.Parameters.Add("@Ciudad", SqlDbType.VarChar, -1);
            param.Value = lcustomermasterfile.Ciudad;

            // ,@CodigoPostal  varchar
            param = cmd.Parameters.Add("@CodigoPostal", SqlDbType.VarChar, 5);
            param.Value = lcustomermasterfile.CodigoPostal;

            // ,@Pais  varchar
            param = cmd.Parameters.Add("@Pais", SqlDbType.VarChar, 2);
            param.Value = lcustomermasterfile.Pais;

            // ,@Contacto  varchar
            param = cmd.Parameters.Add("@Contacto", SqlDbType.VarChar, 100);
            param.Value = lcustomermasterfile.Contacto;

            // ,@MedioContacto  varchar
            param = cmd.Parameters.Add("@MedioContacto", SqlDbType.VarChar, 10);
            param.Value = lcustomermasterfile.MedioContacto;


            // ,@DatosContacto  varchar
            param = cmd.Parameters.Add("@DatosContacto", SqlDbType.VarChar, 30);
            param.Value = lcustomermasterfile.DatosContacto;

            // ,@Proveedor  varchar
            param = cmd.Parameters.Add("@Proveedor", SqlDbType.VarChar, 100);
            param.Value = lcustomermasterfile.Proveedor;

            // ,@ProveedorDireccion  varchar
            param = cmd.Parameters.Add("@ProveedorDireccion", SqlDbType.VarChar, 100);
            param.Value = lcustomermasterfile.ProveedorDireccion;

            // ,@ProveedorInterior  varchar
            param = cmd.Parameters.Add("@ProveedorInterior", SqlDbType.VarChar, 100);
            param.Value = lcustomermasterfile.ProveedorInterior;

            // ,@ProveedorCiudad  varchar
            param = cmd.Parameters.Add("@ProveedorCiudad", SqlDbType.VarChar, 100);
            param.Value = lcustomermasterfile.ProveedorCiudad;

            // ,@ProveedorEstado  varchar
            param = cmd.Parameters.Add("@ProveedorEstado", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.ProveedorEstado;

            // ,@ProveedorPais  varchar
            param = cmd.Parameters.Add("@ProveedorPais", SqlDbType.VarChar, 50);
            param.Value = lcustomermasterfile.ProveedorPais;

            // ProveedorCodigoPostal varchar
            param = cmd.Parameters.Add("@ProveedorCodigoPostal", SqlDbType.VarChar, 15);
            param.Value = lcustomermasterfile.ProveedorCodigoPostal;

            // ,@ProveedorMedio  varchar
            param = cmd.Parameters.Add("@ProveedorMedio", SqlDbType.VarChar, 50);
            param.Value = lcustomermasterfile.ProveedorMedio;

            // ,@ProveedorDatos  varchar
            param = cmd.Parameters.Add("@ProveedorDatos", SqlDbType.VarChar, 30);
            param.Value = lcustomermasterfile.ProveedorDatos;

            // ,@Peso  money
            param = cmd.Parameters.Add("@Peso", SqlDbType.Money, 4);
            param.Value = lcustomermasterfile.Peso;

            // ,@PesoVolumetrico  money
            param = cmd.Parameters.Add("@PesoVolumetrico", SqlDbType.Money, 4);
            param.Value = lcustomermasterfile.PesoVolumetrico;

            // ,@Piezas  int
            param = cmd.Parameters.Add("@Piezas", SqlDbType.Int, 4);
            param.Value = lcustomermasterfile.Piezas;

            // ,@Incoterm  varchar
            param = cmd.Parameters.Add("@Incoterm", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.Incoterm;

            // ,@ServicioDhl  varchar
            param = cmd.Parameters.Add("@ServicioDhl", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.ServicioDhl;

            // ,@FacturaValor  money
            param = cmd.Parameters.Add("@FacturaValor", SqlDbType.Money, 4);
            param.Value = lcustomermasterfile.FacturaValor;

            // ,@FacturaMoneda  varchar
            param = cmd.Parameters.Add("@FacturaMoneda", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.FacturaMoneda;

            // ,@PaisVendedor  varchar
            param = cmd.Parameters.Add("@PaisVendedor", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.PaisVendedor;

            // ,@PaisComprador  varchar
            param = cmd.Parameters.Add("@PaisComprador", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.PaisComprador;

            // @NombredeArchivo
            param = cmd.Parameters.Add("@NombredeArchivo", SqlDbType.VarChar, 250);
            param.Value = lcustomermasterfile.NombredeArchivo;


            param = cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4);
            param.Value = lcustomermasterfile.IdCliente;

            param = cmd.Parameters.Add("@ValidarCliente", SqlDbType.Bit);
            param.Value = Convert.ToDouble(lcustomermasterfile.ValidarCliente);

            param = cmd.Parameters.Add("@IdRiel", SqlDbType.Int, 4);
            param.Value = lcustomermasterfile.IdRiel;

            param = cmd.Parameters.Add("@IdCategoria", SqlDbType.Int, 4);
            param.Value = lcustomermasterfile.IdCategoria;

            param = cmd.Parameters.Add("@ClavedePedimento", SqlDbType.VarChar, 3);
            param.Value = lcustomermasterfile.ClavedePedimento;

            param = cmd.Parameters.Add("@XmlEnviado", SqlDbType.Bit);
            param.Value = lcustomermasterfile.XmlEnviado;

            param = cmd.Parameters.Add("@PreCaptura", SqlDbType.Bit);
            param.Value = lcustomermasterfile.PreCaptura;

            param = cmd.Parameters.Add("@ExisteGuia", SqlDbType.Bit);
            param.Value = lcustomermasterfile.ExisteGuia;

            param = cmd.Parameters.Add("@ExisteFactura", SqlDbType.Bit);
            param.Value = lcustomermasterfile.ExisteFactura;

            param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;


            try
            {

                using (cmd.ExecuteReader())
                {

                    if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                    {
                        id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                    }
                    else
                    {
                        id = 0;
                    }
                }

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_CustomerMasterFile");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return id;
        }

        public bool SinonimosdeRiesgo(string GuiaHouse, string Descripcion, int idPartidasCMF)
        {
            bool ejecuto = false;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "sRiesgo.NET_INSERT_SINONIMOSDERIESGO_GUIA_PARTIDA_Id";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            // ,@GuiaHouse  varchar
            param = cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 25);
            param.Value = GuiaHouse;

            // ,@GuiaHouse  varchar
            param = cmd.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar, 500);
            param.Value = Descripcion;

            // ,@GuiaHouse  varchar
            param = cmd.Parameters.Add("@idPartidasCMF", SqlDbType.Int, 4);
            param.Value = idPartidasCMF;

            try
            {
                ejecuto = true;
                SqlDataReader dr;

                dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                ejecuto = false;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "sRiesgo.NET_INSERT_SINONIMOSDERIESGO_GUIA_PARTIDA_Id");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return ejecuto;
        }

        public int InsertarPieceIdCMF(cmfPieces lcmfPieces)
        {
            int id;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_INSERT_PIECEID_CMF";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            param = cmd.Parameters.Add("@aWBNumber", SqlDbType.VarChar, 500);
            param.Value = lcmfPieces.aWBNumber;

            param = cmd.Parameters.Add("@licensePlate", SqlDbType.VarChar, 500);
            param.Value = lcmfPieces.licensePlate;

            param = cmd.Parameters.Add("@pieceNumber", SqlDbType.Int, 4);
            param.Value = lcmfPieces.pieceNumber;

            if (lcmfPieces.actualDepth == null) { lcmfPieces.actualDepth = 0.0; }
            param = cmd.Parameters.Add("@actualDepth", SqlDbType.Money);
            param.Value = lcmfPieces.actualDepth;

            if (lcmfPieces.actualWidth == null) { lcmfPieces.actualWidth = 0.0; }
            param = cmd.Parameters.Add("@actualWidth", SqlDbType.Money);
            param.Value = lcmfPieces.actualWidth;

            if (lcmfPieces.actualHeight == null) { lcmfPieces.actualHeight = 0.0; }
            param = cmd.Parameters.Add("@actualHeight", SqlDbType.Money);
            param.Value = lcmfPieces.actualHeight;

            if (lcmfPieces.actualWeight == null) { lcmfPieces.actualWeight = 0.0; }
            param = cmd.Parameters.Add("@actualWeight", SqlDbType.Money);
            param.Value = lcmfPieces.actualWeight;

            param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;


            try
            {
                using (cmd.ExecuteReader())
                {

                    if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                    {
                        id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                    }
                    else
                    {
                        id = 0;
                    }
                }



                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_PIECEID_CMF");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return id;
        }

        public CustomerMasterFile Buscar(string GuiaHouse)
        {
            CustomerMasterFile objCustomerMasterFile = new CustomerMasterFile();

            try
            {



                using (SqlConnection cn = new SqlConnection(sConexion))

                using (SqlCommand cmd = new("NET_SEARCH_CUSTOMERMASTERFILE_Guia", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 25).Value = GuiaHouse;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objCustomerMasterFile.IdCMF = Convert.ToInt32(dr["IdCMF"]);
                        objCustomerMasterFile.GuiaHouse = dr["GuiaHouse"].ToString();
                        objCustomerMasterFile.GtwDestino = dr["GtwDestino"].ToString();
                        objCustomerMasterFile.IataOrigen = dr["IataOrigen"].ToString();
                        objCustomerMasterFile.IataDestino = dr["IataDestino"].ToString();
                        objCustomerMasterFile.TipoEnvio = dr["TipoEnvio"].ToString();
                        objCustomerMasterFile.Descripcion = dr["Descripcion"].ToString();
                        objCustomerMasterFile.NoCuenta = dr["NoCuenta"].ToString();
                        objCustomerMasterFile.Destinatario = dr["Destinatario"].ToString();
                        objCustomerMasterFile.Direccion1 = dr["Direccion1"].ToString();
                        objCustomerMasterFile.Direccion2 = dr["Direccion2"].ToString();
                        objCustomerMasterFile.Direccion3 = dr["Direccion3"].ToString();
                        objCustomerMasterFile.Ciudad = dr["Ciudad"].ToString();
                        objCustomerMasterFile.CodigoPostal = dr["CodigoPostal"].ToString();
                        objCustomerMasterFile.Pais = dr["Pais"].ToString();
                        objCustomerMasterFile.Contacto = dr["Contacto"].ToString();
                        objCustomerMasterFile.MedioContacto = dr["MedioContacto"].ToString();
                        objCustomerMasterFile.DatosContacto = dr["DatosContacto"].ToString();
                        objCustomerMasterFile.Proveedor = dr["Proveedor"].ToString();
                        objCustomerMasterFile.ProveedorDireccion = dr["ProveedorDireccion"].ToString();
                        objCustomerMasterFile.ProveedorInterior = dr["ProveedorInterior"].ToString();
                        objCustomerMasterFile.ProveedorCiudad = dr["ProveedorCiudad"].ToString();
                        objCustomerMasterFile.ProveedorEstado = dr["ProveedorEstado"].ToString();
                        objCustomerMasterFile.ProveedorPais = dr["ProveedorPais"].ToString();
                        objCustomerMasterFile.ProveedorCodigoPostal = dr["ProveedorCodigoPostal"].ToString();
                        objCustomerMasterFile.ProveedorMedio = dr["ProveedorMedio"].ToString();
                        objCustomerMasterFile.ProveedorDatos = dr["ProveedorDatos"].ToString();
                        objCustomerMasterFile.Peso = Convert.ToDouble(dr["Peso"]);
                        objCustomerMasterFile.PesoVolumetrico = Convert.ToDouble(dr["PesoVolumetrico"]);
                        objCustomerMasterFile.Piezas = Convert.ToInt32(dr["Piezas"]);
                        objCustomerMasterFile.Incoterm = dr["Incoterm"].ToString();
                        objCustomerMasterFile.ServicioDhl = dr["ServicioDhl"].ToString();
                        objCustomerMasterFile.FacturaValor = Convert.ToDouble(dr["FacturaValor"]);
                        objCustomerMasterFile.FacturaMoneda = dr["FacturaMoneda"].ToString();
                        objCustomerMasterFile.PaisVendedor = dr["PaisVendedor"].ToString();
                        objCustomerMasterFile.PaisComprador = dr["PaisComprador"].ToString();
                        objCustomerMasterFile.NombredeArchivo = dr["NombredeArchivo"].ToString();
                        objCustomerMasterFile.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);
                        objCustomerMasterFile.IdCliente = Convert.ToInt32(dr["IdCliente"]);
                        objCustomerMasterFile.IdCategoria = Convert.ToInt32(dr["IdCategoria"]);
                        objCustomerMasterFile.ClavedePedimento = dr["ClavedePedimento"].ToString();
                        objCustomerMasterFile.Patente = dr["Patente"].ToString();
                        objCustomerMasterFile.ValidarCliente = Convert.ToBoolean(dr["ValidarCliente"]);
                        objCustomerMasterFile.IdRiel = Convert.ToInt32(dr["IdRiel"]);
                        objCustomerMasterFile.Detener = Convert.ToBoolean(dr["Detener"]);
                        objCustomerMasterFile.ExisteGuia = Convert.ToBoolean(dr["ExisteGuia"]);
                        objCustomerMasterFile.ExisteFactura = Convert.ToBoolean(dr["ExisteFactura"]);
                        objCustomerMasterFile.ValorDolares = Convert.ToDouble(dr["ValorDolares"]);
                        objCustomerMasterFile.DescripcionEspanol = dr["DescripcionEspanol"].ToString();
                        objCustomerMasterFile.idTipodePedimento = Convert.ToInt32(dr["idTipodePedimento"]);
                        objCustomerMasterFile.XmlEnviado = Convert.ToBoolean(dr["XmlEnviado"]);
                        objCustomerMasterFile.IdUsuarioAsignado = Convert.ToInt32(dr["IdUsuarioAsignado"]);
                        objCustomerMasterFile.FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]);
                        objCustomerMasterFile.PreCaptura = Convert.ToBoolean(dr["PreCaptura"]);
                        objCustomerMasterFile.EnviarXML = Convert.ToBoolean(dr["EnviarXML"]);
                        objCustomerMasterFile.idRielWEC = Convert.ToInt32(dr["idRielWEC"]);
                        objCustomerMasterFile.EnvioDHL = Convert.ToDateTime(dr["EnvioDHL"]);
                        objCustomerMasterFile.EnvioWEC = Convert.ToDateTime(dr["EnvioWEC"]);
                        objCustomerMasterFile.XMLEnviadoWEC = Convert.ToBoolean(dr["XMLEnviadoWEC"]);
                        objCustomerMasterFile.GuiaMaster = dr["GuiaMaster"].ToString();
                        objCustomerMasterFile.ShipmentReference = dr["ShipmentReference"].ToString();
                        objCustomerMasterFile.NoCuentaCliente = dr["NoCuentaCliente"].ToString();
                        objCustomerMasterFile.Cotizacion = Convert.ToBoolean(dr["Cotizacion"]);
                        objCustomerMasterFile.Frght = Convert.ToDouble(dr["Frght"]);
                        objCustomerMasterFile.FrghtCrncy = dr["FrghtCrncy"].ToString();
                        objCustomerMasterFile.ProvConfiable = Convert.ToBoolean(dr["ProvConfiable"]);
                        objCustomerMasterFile.ASIGNADAPRECA = Convert.ToBoolean(dr["ASIGNADAPRECA"]);
                        objCustomerMasterFile.idOficina = Convert.ToInt32(dr["idOficina"]);
                        objCustomerMasterFile.AduanaDespacho = dr["AduanaDespacho"].ToString();

                    }
                    else
                    {
                        objCustomerMasterFile = null!;
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objCustomerMasterFile;
        }

        public CustomerMasterFile BuscarId(int idCMF)
        {
            CustomerMasterFile objCustomerMasterFile = new CustomerMasterFile();

            try
            {



                using (SqlConnection cn = new SqlConnection(sConexion))

                using (SqlCommand cmd = new("NET_SEARCH_CUSTOMERMASTERFILE", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idCMF", SqlDbType.Int, 4).Value = idCMF;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objCustomerMasterFile.IdCMF = Convert.ToInt32(dr["IdCMF"]);
                        objCustomerMasterFile.GuiaHouse = dr["GuiaHouse"].ToString();
                        objCustomerMasterFile.GtwDestino = dr["GtwDestino"].ToString();
                        objCustomerMasterFile.IataOrigen = dr["IataOrigen"].ToString();
                        objCustomerMasterFile.IataDestino = dr["IataDestino"].ToString();
                        objCustomerMasterFile.TipoEnvio = dr["TipoEnvio"].ToString();
                        objCustomerMasterFile.Descripcion = dr["Descripcion"].ToString();
                        objCustomerMasterFile.NoCuenta = dr["NoCuenta"].ToString();
                        objCustomerMasterFile.Destinatario = dr["Destinatario"].ToString();
                        objCustomerMasterFile.Direccion1 = dr["Direccion1"].ToString();
                        objCustomerMasterFile.Direccion2 = dr["Direccion2"].ToString();
                        objCustomerMasterFile.Direccion3 = dr["Direccion3"].ToString();
                        objCustomerMasterFile.Ciudad = dr["Ciudad"].ToString();
                        objCustomerMasterFile.CodigoPostal = dr["CodigoPostal"].ToString();
                        objCustomerMasterFile.Pais = dr["Pais"].ToString();
                        objCustomerMasterFile.Contacto = dr["Contacto"].ToString();
                        objCustomerMasterFile.MedioContacto = dr["MedioContacto"].ToString();
                        objCustomerMasterFile.DatosContacto = dr["DatosContacto"].ToString();
                        objCustomerMasterFile.Proveedor = dr["Proveedor"].ToString();
                        objCustomerMasterFile.ProveedorDireccion = dr["ProveedorDireccion"].ToString();
                        objCustomerMasterFile.ProveedorInterior = dr["ProveedorInterior"].ToString();
                        objCustomerMasterFile.ProveedorCiudad = dr["ProveedorCiudad"].ToString();
                        objCustomerMasterFile.ProveedorEstado = dr["ProveedorEstado"].ToString();
                        objCustomerMasterFile.ProveedorPais = dr["ProveedorPais"].ToString();
                        objCustomerMasterFile.ProveedorCodigoPostal = dr["ProveedorCodigoPostal"].ToString();
                        objCustomerMasterFile.ProveedorMedio = dr["ProveedorMedio"].ToString();
                        objCustomerMasterFile.ProveedorDatos = dr["ProveedorDatos"].ToString();
                        objCustomerMasterFile.Peso = Convert.ToDouble(dr["Peso"]);
                        objCustomerMasterFile.PesoVolumetrico = Convert.ToDouble(dr["PesoVolumetrico"]);
                        objCustomerMasterFile.Piezas = Convert.ToInt32(dr["Piezas"]);
                        objCustomerMasterFile.Incoterm = dr["Incoterm"].ToString();
                        objCustomerMasterFile.ServicioDhl = dr["ServicioDhl"].ToString();
                        objCustomerMasterFile.FacturaValor = Convert.ToDouble(dr["FacturaValor"]);
                        objCustomerMasterFile.FacturaMoneda = dr["FacturaMoneda"].ToString();
                        objCustomerMasterFile.PaisVendedor = dr["PaisVendedor"].ToString();
                        objCustomerMasterFile.PaisComprador = dr["PaisComprador"].ToString();
                        objCustomerMasterFile.NombredeArchivo = dr["NombredeArchivo"].ToString();
                        objCustomerMasterFile.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);
                        objCustomerMasterFile.IdCliente = Convert.ToInt32(dr["IdCliente"]);
                        objCustomerMasterFile.IdCategoria = Convert.ToInt32(dr["IdCategoria"]);
                        objCustomerMasterFile.ClavedePedimento = dr["ClavedePedimento"].ToString();
                        objCustomerMasterFile.Patente = dr["Patente"].ToString();
                        objCustomerMasterFile.ValidarCliente = Convert.ToBoolean(dr["ValidarCliente"]);
                        objCustomerMasterFile.IdRiel = Convert.ToInt32(dr["IdRiel"]);
                        objCustomerMasterFile.Detener = Convert.ToBoolean(dr["Detener"]);
                        objCustomerMasterFile.ExisteGuia = Convert.ToBoolean(dr["ExisteGuia"]);
                        objCustomerMasterFile.ExisteFactura = Convert.ToBoolean(dr["ExisteFactura"]);
                        objCustomerMasterFile.ValorDolares = Convert.ToDouble(dr["ValorDolares"]);
                        objCustomerMasterFile.DescripcionEspanol = dr["DescripcionEspanol"].ToString();
                        objCustomerMasterFile.idTipodePedimento = Convert.ToInt32(dr["idTipodePedimento"]);
                        objCustomerMasterFile.XmlEnviado = Convert.ToBoolean(dr["XmlEnviado"]);
                        objCustomerMasterFile.IdUsuarioAsignado = Convert.ToInt32(dr["IdUsuarioAsignado"]);
                        objCustomerMasterFile.FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]);
                        objCustomerMasterFile.PreCaptura = Convert.ToBoolean(dr["PreCaptura"]);
                        objCustomerMasterFile.EnviarXML = Convert.ToBoolean(dr["EnviarXML"]);
                        objCustomerMasterFile.idRielWEC = Convert.ToInt32(dr["idRielWEC"]);
                        objCustomerMasterFile.EnvioDHL = Convert.ToDateTime(dr["EnvioDHL"]);
                        objCustomerMasterFile.EnvioWEC = Convert.ToDateTime(dr["EnvioWEC"]);
                        objCustomerMasterFile.XMLEnviadoWEC = Convert.ToBoolean(dr["XMLEnviadoWEC"]);
                        objCustomerMasterFile.GuiaMaster = dr["GuiaMaster"].ToString();
                        objCustomerMasterFile.ShipmentReference = dr["ShipmentReference"].ToString();
                        objCustomerMasterFile.NoCuentaCliente = dr["NoCuentaCliente"].ToString();
                        objCustomerMasterFile.Cotizacion = Convert.ToBoolean(dr["Cotizacion"]);
                        objCustomerMasterFile.Frght = Convert.ToDouble(dr["Frght"]);
                        objCustomerMasterFile.FrghtCrncy = dr["FrghtCrncy"].ToString();
                        objCustomerMasterFile.ProvConfiable = Convert.ToBoolean(dr["ProvConfiable"]);
                        objCustomerMasterFile.ASIGNADAPRECA = Convert.ToBoolean(dr["ASIGNADAPRECA"]);
                        objCustomerMasterFile.idOficina = Convert.ToInt32(dr["idOficina"]);
                        objCustomerMasterFile.AduanaDespacho = dr["AduanaDespacho"].ToString();

                    }
                    else
                    {
                        objCustomerMasterFile = null!;
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objCustomerMasterFile;
        }

        public int modificarSifty(int idCMF, int sifty)
        {
            int id = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_CUSTOMERMASTERFILE_NoAbrir";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            param = cmd.Parameters.Add("@idCMF", SqlDbType.Int, 4);
            param.Value = idCMF;

            param = cmd.Parameters.Add("@NoAbrir", SqlDbType.Int, 4);
            param.Value = sifty;


            param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;


            try
            {

                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_PIECEID_CMF");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return id;
        }

        public int ModificarPrecaptura(int idCMF)
        {
            int id = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_CUSTOMERMASTERFILE_PRECAPTURA";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            param = cmd.Parameters.Add("@idCMF", SqlDbType.Int, 4);
            param.Value = idCMF;


            param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;


            try
            {

                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_CUSTOMERMASTERFILE_PRECAPTURA");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return id;
        }


        public int modificarPorOrigen(int idCMF, string PaisOrigen, string Descripcion)
        {
            int id = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_CUSTOMERMASTERFILE_OrigenDesc";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            param = cmd.Parameters.Add("@idCMF", SqlDbType.Int, 4);
            param.Value = idCMF;

            param = cmd.Parameters.Add("@PaisOrigen", SqlDbType.VarChar, 3);
            param.Value = PaisOrigen;

            param = cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 800);
            param.Value = Descripcion;


            param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;


            try
            {

                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_CUSTOMERMASTERFILE_idTipodePedimento");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return id;
        }

        public int modificarRFC(string guiaHouse, string RFC, string Telefono, string Email)
        {
            int id = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_CUSTOMERMASTERFILE_RFC";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            param = cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 25);
            param.Value = guiaHouse;

            param = cmd.Parameters.Add("@DatosContacto", SqlDbType.VarChar, 30);
            param.Value = Telefono == null ? "" : Telefono;

            param = cmd.Parameters.Add("@TaxIDImpo", SqlDbType.VarChar, 50);
            param.Value = RFC == null ? "" : RFC;

            param = cmd.Parameters.Add("@destinatarioEmail", SqlDbType.VarChar, 100);
            param.Value = Email == null ? "" : Email;

            param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;


            try
            {

                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_PIECEID_CMF");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return id;
        }

        public int modificarFacturayGuia(CustomerMasterFile objCMF)
        {
            int id = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_CUSTOMERMASTERFILE_IMAGENES";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            //@idCMF
            param = cmd.Parameters.Add("@idCMF", SqlDbType.Int, 4);
            param.Value = objCMF.IdCMF;

            param = cmd.Parameters.Add("@ExisteFactura", SqlDbType.Bit);
            param.Value = objCMF.ExisteFactura;

            param = cmd.Parameters.Add("@ExisteGuia", SqlDbType.Bit);
            param.Value = objCMF.ExisteGuia;


            param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;


            try
            {

                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_CUSTOMERMASTERFILE_IMAGENES");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return id;
        }

        public bool DebeAsignarPrecaptura(int IdCliente)
        {
            bool Asigna = true;
            try
            {
                using (SqlConnection cn = new SqlConnection(sConexion))

                using (SqlCommand cmd = new("NET_SEARCH_CATALOGODECLIENTESPRECAPTURA_NoASIGNA", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4).Value = IdCliente;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        Asigna = Convert.ToBoolean(dr["Asigna"]);


                    }
                    else
                    {
                        Asigna = true;
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return Asigna;
        }

        public bool AsignaACMF(string GuiaHouse)
        {
            bool Asigna = true;
            try
            {
                using (SqlConnection cn = new SqlConnection(sConexion))

                using (SqlCommand cmd = new("NET_SEARCH_ASIGNACIONDEGUIAS_AsignaCMF", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 15).Value = GuiaHouse;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        Asigna = Convert.ToBoolean(dr["Asigna"]);


                    }
                    else
                    {
                        Asigna = true;
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return Asigna;
        }
        public bool Asignar(CustomerMasterFile objCMF)
        {
            bool Asigna = false;

            if (objCMF.ValidarCliente == false)
            {
                return false;
            }

            switch (objCMF.IdCategoria)
            {
                case 1:
                    return false;
                case 5:
                    return false;

                case 10:
                    return false;

                case 2:
                    Asigna = true;
                    break;
                case 3:
                    Asigna = true;
                    break;

                case 4:
                    Asigna = true;
                    break;
            }
            if (objCMF.ProvConfiable == true)
            {
                Asigna = true;
            }

            Asigna = DebeAsignarPrecaptura(objCMF.IdCliente);

            if (Asigna == true)
            {
                if ((objCMF.ExisteFactura == true) && (objCMF.ExisteGuia == true))
                {
                    int idRefe = 0;

                    Referencias objRefe = new Referencias();
                    ReferenciasRepository objRefeD = new ReferenciasRepository(_configuration);
                    objRefe = objRefeD.Buscar(objCMF.GuiaHouse);

                    if (objRefe == null)
                    {
                        Referencias objRefeNew = new Referencias();
                        objRefeNew = LlenarobjetoReferencia(objCMF.GuiaHouse, objCMF.IdCliente, objCMF.GuiaMaster, objCMF.idOficina, objCMF.AduanaDespacho);

                        idRefe = objRefeD.Insertar(objRefeNew);

                        if (Asigna == true)
                        {
                            Asigna = AsignaACMF(objCMF.GuiaHouse.Trim());
                        }
                    }
                    else
                    {
                        //Dim fecha As DateTime = Date.Now.AddDays(-90)
                        DateTime fecha = DateTime.Now.AddDays(-90);
                        if (objRefe.FechaApertura <= fecha)
                        {
                            objCMF.GuiaHouse = "D" + objCMF.GuiaHouse.Trim();
                            Referencias objRefeNew = new Referencias();
                            objRefeNew = LlenarobjetoReferencia(objCMF.GuiaHouse, objCMF.IdCliente, objCMF.GuiaMaster, objCMF.idOficina, objCMF.AduanaDespacho);

                            idRefe = objRefeD.Insertar(objRefeNew);

                            if (Asigna == true)
                            {
                                Asigna = AsignaACMF(objCMF.GuiaHouse.Trim());
                            }


                        }
                        else
                        {
                            idRefe = objRefe.IDReferencia;
                        }


                    }
                    if (idRefe != 0)
                    {
                        SaaioPedime objPedime = new SaaioPedime();
                        SaaioPedime objPedimeB = new SaaioPedime();
                        SaaioPedimeRepository objPedimeD = new(_configuration);

                        objPedimeB = objPedimeD.Buscar(objCMF.GuiaHouse.Trim());

                        if (objPedimeB == null)
                        {
                            objPedime = LlenarobjSaaioPedime(objCMF);
                            objPedimeD.Insertar(objPedime);

                        }

                        ControldeEventosRepository objControldeEventosD = new ControldeEventosRepository(_configuration);
                        ControldeEventos objControldeEventos = new ControldeEventos(436, idRefe, 6301, DateTime.Now);

                        Referencias refe = new Referencias();
                        refe = objRefeD.Buscar(idRefe);

                        objControldeEventosD.InsertarEvento(objControldeEventos, 54, refe.IdOficina, false, "", 1);

                        CustomerMasterFileRepository objCMFD = new CustomerMasterFileRepository(_configuration);
                        objCMFD.ModificarPrecaptura(objCMF.IdCMF);

                    }


                }

            }



            return Asigna;

        }

        public Referencias LlenarobjetoReferencia(string GuiaHouse, int idCliente, string GuiaMaster, int idOficina, string AduanaDespacho)
        {
            Referencias objReferencia = new Referencias();
            string GuiaMasterConGuion = string.Empty;

            if (GuiaMaster != "")
            {
                int guiamasterlen = GuiaMaster.Length;
                GuiaMasterConGuion = GuiaMaster.Substring(0, 3) + "-" + GuiaMaster.Substring(3, guiamasterlen - 3);
            }


            CatalogodeMaster objMaster = new CatalogodeMaster();
            CatalogodeMasterRepository objMasterD = new CatalogodeMasterRepository(_configuration);
            objMaster = objMasterD.Buscar(GuiaMasterConGuion);

            if (objMaster != null)
            {
                idOficina = objMaster.IdOficina;
            }

            CatalogoDeOficinas objOficina = new();
            CatalogoDeOficinasRepository objOficinaD = new(_configuration);
            objOficina = objOficinaD.Buscar(idOficina);
            if (objOficina != null)
            {
                objReferencia.IdOficina = idOficina;
                objReferencia.AduanaEntrada = objOficina.AduEntr;
                objReferencia.AduanaDespacho = objOficina.AduDesp;
                objReferencia.Patente = objOficina.PatenteDefault;
                objReferencia.NumeroDeReferencia = GuiaHouse.Trim();
                objReferencia.FechaApertura = DateTime.Now;

                if (idCliente == 0)
                {
                    objReferencia.IDCliente = 17169;
                }
                else
                {
                    objReferencia.IDCliente = idCliente;
                }

                objReferencia.Operacion = 1;
                objReferencia.IdDuenoDeLaReferencia = 6301;
                objReferencia.Subdivision = false;
                objReferencia.PendientePorRectificar = false;
                objReferencia.IdClienteDestinatario = 0;
                objReferencia.ReferenciaDestinatario = "";
                objReferencia.IdGrupo = 1;
                objReferencia.IDDatosDeEmpresa = 1;
            }

            return objReferencia;
        }

        public SaaioPedime LlenarobjSaaioPedime(CustomerMasterFile objCMF)
        {
            SaaioPedime objSaaioPedime = new SaaioPedime();
            try
            {
                Clientes objClientes = new Clientes();
                ClientesRepository objClientesD = new ClientesRepository(_configuration);
                objClientes = objClientesD.Buscar(objCMF.IdCliente);

                CatalogoDeOficinas objOficina = new CatalogoDeOficinas();
                CatalogoDeOficinasRepository objOficinaD = new CatalogoDeOficinasRepository(_configuration);
                objOficina = objOficinaD.Buscar(objCMF.idOficina);

                string ClavedePedimento = objCMF.ClavedePedimento;
                if (ClavedePedimento == "")
                {
                    ClavedePedimento = "A1";
                }

                objSaaioPedime.NUM_REFE = objCMF.GuiaHouse.Trim();
                objSaaioPedime.ADU_DESP = objOficina.AduDesp.Trim();
                objSaaioPedime.ADU_ENTR = objOficina.AduEntr.Trim();
                objSaaioPedime.FEC_ENTR = Convert.ToDateTime(DateTime.Now);
                objSaaioPedime.CVE_PEDI = ClavedePedimento;
                objSaaioPedime.REG_ADUA = "IMD";
                objSaaioPedime.DES_ORIG = objOficina.DesOrig;
                objSaaioPedime.MTR_ENTR = objOficina.MtrEntrImp.ToString();
                objSaaioPedime.MTR_ARRI = objOficina.MtrArriImp.ToString();
                objSaaioPedime.MTR_SALI = objOficina.MtrSaliImp.ToString();
                objSaaioPedime.MON_VASE = 0;
                objSaaioPedime.SEC_DESP = objOficina.SecDesp.ToString();
                objSaaioPedime.CVE_CAPT = "";
                objSaaioPedime.CVE_IMPO = objClientes.Clave.Trim();
                objSaaioPedime.IMP_EXPO = "1";
                objSaaioPedime.PAT_AGEN = objOficina.PatenteDefault.Trim();
                objSaaioPedime.PES_BRUT = objCMF.Peso;
                objSaaioPedime.CAN_BULT = objCMF.Piezas;
                objSaaioPedime.TOT_VEHI = 1;
                objSaaioPedime.TIP_MOVA = "USD";
                objSaaioPedime.TIP_CAMB = 1.0;
                objSaaioPedime.CVE_REPR = objOficina.CveMant.Trim();

            }
            catch (Exception)
            {

                throw;
            }

            return objSaaioPedime;
        }
        public bool AsignarGuias(cmfAsignar objAsignar)
        {
            bool Proceso = false;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_ASIGNAR_CUSTOMERMASTERFILE_WEB";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            param = cmd.Parameters.Add("@FechaInicial", SqlDbType.DateTime);
            param.Value = objAsignar.fechaInicial;

            param = cmd.Parameters.Add("@FechaFinal", SqlDbType.DateTime);
            param.Value = objAsignar.fechaFinal;

            param = cmd.Parameters.Add("@IdUsuarioAsignado", SqlDbType.Int, 4);
            param.Value = objAsignar.idUsuario;

            param = cmd.Parameters.Add("@Nuevos", SqlDbType.Bit);
            param.Value = objAsignar.Nuevos;


            try
            {

                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                Proceso = true;
            }
            catch (Exception ex)
            {

                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                Proceso = false;
                throw new Exception(ex.Message.ToString() + "NET_ASIGNAR_CUSTOMERMASTERFILE_WEB");

            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return Proceso;
        }
        public List<CustomerMasterFileAdministracion> CargarAdministracion(DateTime FechaInicial, DateTime FechaFinal, string GuiaHouse)
        {
            List<CustomerMasterFileAdministracion> lstCustomerMasterFile = new List<CustomerMasterFileAdministracion>();
            try
            {
                using (SqlConnection cn = new SqlConnection(sConexion))

                using (SqlCommand cmd = new("NET_LOAD_CUSTOMERMASTERFILE_WEB", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FechaInicial", SqlDbType.Date).Value = FechaInicial;
                    cmd.Parameters.Add("@FechaFinal", SqlDbType.Date).Value = FechaFinal;
                    cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 25).Value = GuiaHouse;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            CustomerMasterFileAdministracion objCustomerMasterFile = new CustomerMasterFileAdministracion();
                            objCustomerMasterFile.IdCMF = Convert.ToInt32(dr["IdCMF"]);
                            objCustomerMasterFile.GuiaHouse = dr["GuiaHouse"].ToString();
                            objCustomerMasterFile.ValorEnDolares = Convert.ToDouble(dr["ValorDolares"]);
                            objCustomerMasterFile.Piezas = Convert.ToInt32(dr["Piezas"]);
                            objCustomerMasterFile.Descripcion = dr["Descripcion"].ToString();
                            objCustomerMasterFile.Destinatario = dr["Destinatario"].ToString();
                            objCustomerMasterFile.Cliente = dr["Cliente"].ToString();
                            objCustomerMasterFile.RFC = dr["RFC"].ToString();
                            objCustomerMasterFile.Telefono = dr["Telefono"].ToString();
                            objCustomerMasterFile.CorreoElectronico = dr["CorreoElectronico"].ToString();
                            objCustomerMasterFile.Categoria = dr["Categoria"].ToString();
                            objCustomerMasterFile.Riel = dr["Riel"].ToString();
                            objCustomerMasterFile.Proveedor = dr["Proveedor"].ToString();
                            objCustomerMasterFile.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);
                            //objCustomerMasterFile.FechaAlta = dr["FechaAlta"].ToString();

                            lstCustomerMasterFile.Add(objCustomerMasterFile);
                        }


                    }
                    else
                    {
                        lstCustomerMasterFile.Clear();
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lstCustomerMasterFile;
        }
        public List<CustomerMasterFileNuevos> CargarNuevos(DateTime FechaInicial, DateTime FechaFinal, int IdUsuarioAsignado)
        {
            List<CustomerMasterFileNuevos> lstCustomerMasterFile = new List<CustomerMasterFileNuevos>();
            try
            {
                using (SqlConnection cn = new SqlConnection(sConexion))

                using (SqlCommand cmd = new("NET_LOAD_CUSTOMERMASTERFILE_CLIENTESNUEVOS_WEB", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FechaInicial", SqlDbType.DateTime).Value = FechaInicial;
                    cmd.Parameters.Add("@FechaFinal", SqlDbType.DateTime).Value = FechaFinal;
                    cmd.Parameters.Add("@IdUsuarioAsignado", SqlDbType.Int, 4).Value = IdUsuarioAsignado;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            CustomerMasterFileNuevos objCustomerMasterFile = new CustomerMasterFileNuevos();
                            objCustomerMasterFile.IdCMF = Convert.ToInt32(dr["IdCMF"]);
                            objCustomerMasterFile.GuiaHouse = dr["GuiaHouse"].ToString();
                            objCustomerMasterFile.Destinatario = dr["Destinatario"].ToString();
                            objCustomerMasterFile.Categoria = dr["Categoria"].ToString();
                            objCustomerMasterFile.ValorDolares = Convert.ToDouble(dr["ValorDolares"]);

                            lstCustomerMasterFile.Add(objCustomerMasterFile);
                        }


                    }
                    else
                    {
                        lstCustomerMasterFile.Clear();
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lstCustomerMasterFile;
        }
        public List<CustomerMasterFileExistentes> CargarExistentes(DateTime FechaInicial, DateTime FechaFinal, int IdUsuarioAsignado)
        {
            List<CustomerMasterFileExistentes> lstCustomerMasterFile = new List<CustomerMasterFileExistentes>();
            try
            {
                using (SqlConnection cn = new SqlConnection(sConexion))

                using (SqlCommand cmd = new("NET_LOAD_CUSTOMERMASTERFILE_CLIENTESEXPERTTI_WEB", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FechaInicial", SqlDbType.DateTime).Value = FechaInicial;
                    cmd.Parameters.Add("@FechaFinal", SqlDbType.DateTime).Value = FechaFinal;
                    cmd.Parameters.Add("@IdUsuarioAsignado", SqlDbType.Int, 4).Value = IdUsuarioAsignado;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            CustomerMasterFileExistentes objCustomerMasterFile = new CustomerMasterFileExistentes();
                            objCustomerMasterFile.IdCMF = Convert.ToInt32(dr["IdCMF"]);
                            objCustomerMasterFile.GuiaHouse = dr["GuiaHouse"].ToString();
                            objCustomerMasterFile.Destinatario = dr["Destinatario"].ToString();
                            objCustomerMasterFile.Cliente = dr["Cliente"].ToString();
                            objCustomerMasterFile.Categoria = dr["Categoria"].ToString();


                            lstCustomerMasterFile.Add(objCustomerMasterFile);
                        }


                    }
                    else
                    {
                        lstCustomerMasterFile.Clear();
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lstCustomerMasterFile;
        }

        public List<CustomerMasterFileArchivos> CargarImagenes(DateTime FechaInicial, DateTime FechaFinal)
        {
            List<CustomerMasterFileArchivos> lstCustomerMasterFile = new List<CustomerMasterFileArchivos>();
            try
            {
                using (SqlConnection cn = new SqlConnection(sConexion))

                using (SqlCommand cmd = new("NET_LOAD_CUSTOMERMASTERFILE_ARCHIVOS_WEB", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FechaInicial", SqlDbType.DateTime).Value = FechaInicial;
                    cmd.Parameters.Add("@FechaFinal", SqlDbType.DateTime).Value = FechaFinal;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            CustomerMasterFileArchivos objCustomerMasterFile = new CustomerMasterFileArchivos();
                            objCustomerMasterFile.IdCMF = Convert.ToInt32(dr["IdCMF"]);
                            objCustomerMasterFile.GuiaHouse = dr["GuiaHouse"].ToString();
                            objCustomerMasterFile.ValorEnDolares = Convert.ToDouble(dr["ValorDolares"]);
                            objCustomerMasterFile.Piezas = Convert.ToInt32(dr["Piezas"]);
                            objCustomerMasterFile.Descripcion = dr["Descripcion"].ToString();
                            objCustomerMasterFile.Destinatario = dr["Destinatario"].ToString();
                            objCustomerMasterFile.Cliente = dr["Cliente"].ToString();
                            objCustomerMasterFile.RFC = dr["RFC"].ToString();
                            objCustomerMasterFile.Telefono = dr["Telefono"].ToString();
                            objCustomerMasterFile.CorreoElectronico = dr["CorreoElectronico"].ToString();
                            objCustomerMasterFile.Categoria = dr["Categoria"].ToString();
                            objCustomerMasterFile.Riel = dr["Riel"].ToString();
                            objCustomerMasterFile.Proveedor = dr["Proveedor"].ToString();
                            objCustomerMasterFile.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);
                            //objCustomerMasterFile.FechaAlta = dr["FechaAlta"].ToString();
                            objCustomerMasterFile.ExisteFactura = Convert.ToBoolean(dr["ExisteFactura"]);
                            objCustomerMasterFile.ExisteGuia = Convert.ToBoolean(dr["ExisteGuia"]);

                            lstCustomerMasterFile.Add(objCustomerMasterFile);
                        }


                    }
                    else
                    {
                        lstCustomerMasterFile.Clear();
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lstCustomerMasterFile;
        }

        public List<string> ProcesarLimpieza(List<CustomerMasterFileExistentes> lstCMF)
        {
            List<string> lstErrores = new();
            try
            {
                foreach (CustomerMasterFileExistentes item in lstCMF)
                {
                    try
                    {
                        CustomerMasterFile objCMF = new CustomerMasterFile();
                        CustomerMasterFileRepository objCMFD = new CustomerMasterFileRepository(_configuration);
                        objCMF = objCMFD.Buscar(item.GuiaHouse);

                        if (item.ClienteCorrecto)
                        {
                            objCMF.ValidarCliente = true;
                            objCMF.IdRiel = 3;
                            ModificarSimple(objCMF);
                            Asignar(objCMF);
                        }
                        else
                        {
                            objCMF.IdCliente = 17169;
                            ModificarSimple(objCMF);
                        }
                        lstErrores.Add(item.GuiaHouse.Trim() + ": Procesada satisfactoriamente");
                    }
                    catch (Exception ex)
                    {

                        lstErrores.Add(item.GuiaHouse.Trim() + ": " + ex.Message.ToString());
                    }



                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lstErrores;
        }

        public cmfCompleto BuscarCompleto(int idCMF)
        {
            cmfCompleto objCustomerMasterFile = new();

            try
            {



                using (SqlConnection cn = new SqlConnection(sConexion))

                using (SqlCommand cmd = new("NET_SEARCH_CUSTOMERMASTERFILE", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idCMF", SqlDbType.Int, 4).Value = idCMF;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objCustomerMasterFile.IdCMF = Convert.ToInt32(dr["IdCMF"]);
                        objCustomerMasterFile.GuiaHouse = dr["GuiaHouse"].ToString();
                        objCustomerMasterFile.GtwDestino = dr["GtwDestino"].ToString();
                        objCustomerMasterFile.IataOrigen = dr["IataOrigen"].ToString();
                        objCustomerMasterFile.IataDestino = dr["IataDestino"].ToString();
                        objCustomerMasterFile.TipoEnvio = dr["TipoEnvio"].ToString();
                        objCustomerMasterFile.Descripcion = dr["Descripcion"].ToString();
                        objCustomerMasterFile.NoCuenta = dr["NoCuenta"].ToString();
                        objCustomerMasterFile.Destinatario = dr["Destinatario"].ToString();
                        objCustomerMasterFile.Direccion1 = dr["Direccion1"].ToString();
                        objCustomerMasterFile.Direccion2 = dr["Direccion2"].ToString();
                        objCustomerMasterFile.Direccion3 = dr["Direccion3"].ToString();
                        objCustomerMasterFile.Ciudad = dr["Ciudad"].ToString();
                        objCustomerMasterFile.CodigoPostal = dr["CodigoPostal"].ToString();
                        objCustomerMasterFile.Pais = dr["Pais"].ToString();
                        objCustomerMasterFile.Contacto = dr["Contacto"].ToString();
                        objCustomerMasterFile.MedioContacto = dr["MedioContacto"].ToString();
                        objCustomerMasterFile.DatosContacto = dr["DatosContacto"].ToString();
                        objCustomerMasterFile.Proveedor = dr["Proveedor"].ToString();
                        objCustomerMasterFile.ProveedorDireccion = dr["ProveedorDireccion"].ToString();
                        objCustomerMasterFile.ProveedorInterior = dr["ProveedorInterior"].ToString();
                        objCustomerMasterFile.ProveedorCiudad = dr["ProveedorCiudad"].ToString();
                        objCustomerMasterFile.ProveedorEstado = dr["ProveedorEstado"].ToString();
                        objCustomerMasterFile.ProveedorPais = dr["ProveedorPais"].ToString();
                        objCustomerMasterFile.ProveedorCodigoPostal = dr["ProveedorCodigoPostal"].ToString();
                        objCustomerMasterFile.ProveedorMedio = dr["ProveedorMedio"].ToString();
                        objCustomerMasterFile.ProveedorDatos = dr["ProveedorDatos"].ToString();
                        objCustomerMasterFile.Peso = Convert.ToDouble(dr["Peso"]);
                        objCustomerMasterFile.PesoVolumetrico = Convert.ToDouble(dr["PesoVolumetrico"]);
                        objCustomerMasterFile.Piezas = Convert.ToInt32(dr["Piezas"]);
                        objCustomerMasterFile.Incoterm = dr["Incoterm"].ToString();
                        objCustomerMasterFile.ServicioDhl = dr["ServicioDhl"].ToString();
                        objCustomerMasterFile.FacturaValor = Convert.ToDouble(dr["FacturaValor"]);
                        objCustomerMasterFile.FacturaMoneda = dr["FacturaMoneda"].ToString();
                        objCustomerMasterFile.PaisVendedor = dr["PaisVendedor"].ToString();
                        objCustomerMasterFile.PaisComprador = dr["PaisComprador"].ToString();
                        objCustomerMasterFile.NombredeArchivo = dr["NombredeArchivo"].ToString();
                        objCustomerMasterFile.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);
                        objCustomerMasterFile.IdCliente = Convert.ToInt32(dr["IdCliente"]);
                        objCustomerMasterFile.IdCategoria = Convert.ToInt32(dr["IdCategoria"]);
                        objCustomerMasterFile.ClavedePedimento = dr["ClavedePedimento"].ToString();
                        objCustomerMasterFile.Patente = dr["Patente"].ToString();
                        objCustomerMasterFile.ValidarCliente = Convert.ToBoolean(dr["ValidarCliente"]);
                        objCustomerMasterFile.IdRiel = Convert.ToInt32(dr["IdRiel"]);
                        objCustomerMasterFile.Detener = Convert.ToBoolean(dr["Detener"]);
                        objCustomerMasterFile.ExisteGuia = Convert.ToBoolean(dr["ExisteGuia"]);
                        objCustomerMasterFile.ExisteFactura = Convert.ToBoolean(dr["ExisteFactura"]);
                        objCustomerMasterFile.ValorDolares = Convert.ToDouble(dr["ValorDolares"]);
                        objCustomerMasterFile.DescripcionEspanol = dr["DescripcionEspanol"].ToString();
                        objCustomerMasterFile.idTipodePedimento = Convert.ToInt32(dr["idTipodePedimento"]);
                        objCustomerMasterFile.XmlEnviado = Convert.ToBoolean(dr["XmlEnviado"]);
                        objCustomerMasterFile.IdUsuarioAsignado = Convert.ToInt32(dr["IdUsuarioAsignado"]);
                        objCustomerMasterFile.FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]);
                        objCustomerMasterFile.PreCaptura = Convert.ToBoolean(dr["PreCaptura"]);
                        objCustomerMasterFile.EnviarXML = Convert.ToBoolean(dr["EnviarXML"]);
                        objCustomerMasterFile.idRielWEC = Convert.ToInt32(dr["idRielWEC"]);
                        objCustomerMasterFile.EnvioDHL = Convert.ToDateTime(dr["EnvioDHL"]);
                        objCustomerMasterFile.EnvioWEC = Convert.ToDateTime(dr["EnvioWEC"]);
                        objCustomerMasterFile.XMLEnviadoWEC = Convert.ToBoolean(dr["XMLEnviadoWEC"]);
                        objCustomerMasterFile.GuiaMaster = dr["GuiaMaster"].ToString();
                        objCustomerMasterFile.ShipmentReference = dr["ShipmentReference"].ToString();
                        objCustomerMasterFile.NoCuentaCliente = dr["NoCuentaCliente"].ToString();
                        objCustomerMasterFile.Cotizacion = Convert.ToBoolean(dr["Cotizacion"]);
                        objCustomerMasterFile.Frght = Convert.ToDouble(dr["Frght"]);
                        objCustomerMasterFile.FrghtCrncy = dr["FrghtCrncy"].ToString();
                        objCustomerMasterFile.ProvConfiable = Convert.ToBoolean(dr["ProvConfiable"]);
                        objCustomerMasterFile.ASIGNADAPRECA = Convert.ToBoolean(dr["ASIGNADAPRECA"]);
                        objCustomerMasterFile.idOficina = Convert.ToInt32(dr["idOficina"]);
                        objCustomerMasterFile.AduanaDespacho = dr["AduanaDespacho"].ToString();

                        if (objCustomerMasterFile.IdCliente != 17169)
                        {
                            List<CartaInstruccionesIdEmpresa> objCarta = new();
                            CartaDeInstruccionesRepository objCartaD = new(_configuration);
                            objCarta = objCartaD.CargarCartadeInstruccionesIdCliente(objCustomerMasterFile.IdCliente, 1);

                            objCustomerMasterFile.CartaInstrucciones = objCarta;
                        }
                        CatalogoDeCategorias objCateg = new();
                        CatalogoDeCategoriasRepository objCategD = new(_configuration);
                        objCateg = objCategD.Buscar(objCustomerMasterFile.IdCategoria);
                        objCustomerMasterFile.Categoria = objCateg;


                        CatalogodeRieles objRieles = new();
                        CatalogodeRielesRepository objRielesD = new(_configuration);
                        objRieles = objRielesD.Buscar(objCustomerMasterFile.IdRiel);
                        objCustomerMasterFile.Riel = objRieles;


                        List<CMFPartidas> Partidas = new List<CMFPartidas>();
                        CMFPartidasRepository objCMFPartidas = new(_configuration);

                        Partidas = objCMFPartidas.Cargar(objCustomerMasterFile.IdCMF);

                        objCustomerMasterFile.CMFPartidas = Partidas;



                    }
                    else
                    {
                        objCustomerMasterFile = null!;
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objCustomerMasterFile;
        }


        public cmfCompleto AsignarAPrecaptura()
        {
            cmfCompleto objCustomerMasterFile = new();

            try
            {


                using (SqlConnection cn = new SqlConnection(sConexion))

                using (SqlCommand cmd = new("NET_LOAD_CUSTOMERMASTERFILE_ASIGNAPRECAPTURA", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            objCustomerMasterFile.IdCMF = Convert.ToInt32(dr["IdCMF"]);

                            CustomerMasterFile obj = new CustomerMasterFile();
                            obj = BuscarId(objCustomerMasterFile.IdCMF);

                            Asignar(obj);
                        }


                    }
                    else
                    {
                        objCustomerMasterFile = null!;
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objCustomerMasterFile;
        }

        public cmfCompleto AsignaraAACMF(CatalogoDeUsuarios objUsuario)
        {
            cmfCompleto objCustomerMasterFile = new();

            try
            {


                using (SqlConnection cn = new SqlConnection(sConexion))

                using (SqlCommand cmd = new("NET_LOAD_CUSTOMERMASTERFILE_AGENTEADUANAL", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            objCustomerMasterFile.IdCMF = Convert.ToInt32(dr["IdCMF"]);

                            CustomerMasterFile obj = new CustomerMasterFile();
                            obj = BuscarId(objCustomerMasterFile.IdCMF);

                            AsignarAAAsync(obj, objUsuario);
                        }


                    }
                    else
                    {
                        objCustomerMasterFile = null!;
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objCustomerMasterFile;
        }

        public async Task<bool> AsignarAAAsync(CustomerMasterFile objCMF, CatalogoDeUsuarios objUsuario)
        {
            bool asigna = false;
            try
            {
                int idRefe = 0;

                Referencias objRefe = new Referencias();
                ReferenciasRepository objRefeD = new ReferenciasRepository(_configuration);
                objRefe = objRefeD.Buscar(objCMF.GuiaHouse, 1);

                if (objRefe != null)
                {
                    Referencias objRefeNew = new Referencias();
                    objRefeNew = LlenarobjetoReferencia(objCMF.GuiaHouse, objCMF.IdCliente, objCMF.GuiaMaster, objCMF.idOficina, objCMF.AduanaDespacho);

                    idRefe = objRefeD.Insertar(objRefeNew);

                    ControldeEventosRepository objEventosD = new(_configuration);
                    ControldeEventos lEventos = new(617, idRefe, objUsuario.IdUsuario, DateTime.Now);

                    AsignarGuiasRespuesta objRespuesta = new AsignarGuiasRespuesta();
                    ControldeEventosRepository ctrlEvD = new ControldeEventosRepository(_configuration);
                    objRespuesta = await ctrlEvD.InsertarEvento(lEventos, 72, objUsuario.IdOficina, false, "", objUsuario.IDDatosDeEmpresa);

                    if (objRespuesta != null)
                    {
                        CustomerMasterFileRepository objCMFD = new CustomerMasterFileRepository(_configuration);
                        objCMFD.ModificarPrecaptura(objCMF.IdCMF);
                    }
                    asigna = true;
                }
                else
                {
                    asigna = false;

                }
            }
            catch (Exception)
            {
                asigna = false;
                throw;
            }

            return asigna;
        }

        public int modificarNuevos(int idCMF, int idDestinatario)
        {
            int id = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_CUSTOMERMASTERFILE_DESTINATARIO";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            param = cmd.Parameters.Add("@idCMF", SqlDbType.Int, 4);
            param.Value = idCMF;

            param = cmd.Parameters.Add("@idDestinatario", SqlDbType.Int, 4);
            param.Value = idDestinatario;


            param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;


            try
            {

                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_CUSTOMERMASTERFILE_DESTINATARIO");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return id;
        }
    }
}

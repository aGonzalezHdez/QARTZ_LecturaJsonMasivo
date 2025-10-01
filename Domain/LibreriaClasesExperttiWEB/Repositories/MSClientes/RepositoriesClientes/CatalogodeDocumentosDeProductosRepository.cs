using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class CatalogodeDocumentosDeProductosRepository : ICatalogodeDocumentosDeProductosRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogodeDocumentosDeProductosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<CargarImagenes> Cargar(int IdCliente, string CodigoDeProducto)
        {
            List<CargarImagenes> list = new();
            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_CASAEI_CATALOGODEDOCUMENTOSDEPRODUCTOS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4).Value = IdCliente;
                    cmd.Parameters.Add("@CodigoDeProducto", SqlDbType.VarChar, 50).Value = CodigoDeProducto;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        list = SqlDataReaderToList.DataReaderMapToList<CargarImagenes>(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return list;
        }

        public int Insertar(CatalogodeDocumentosDeProductos objCatalogodeDocumentosDeProductos)
        {
            int id = 0;
            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_INSERT_CASAEI_CATALOGODEDOCUMENTOSDEPRODUCTOS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4).Value = objCatalogodeDocumentosDeProductos.IdCliente;
                    cmd.Parameters.Add("@CodigoDeProducto", SqlDbType.VarChar, 50).Value = objCatalogodeDocumentosDeProductos.CodigoDeProducto;
                    cmd.Parameters.Add("@IdTipoDocumento", SqlDbType.Int, 4).Value = objCatalogodeDocumentosDeProductos.IdTipoDocumento;
                    cmd.Parameters.Add("@Archivo", SqlDbType.VarChar, 50).Value = objCatalogodeDocumentosDeProductos.Archivo;
                    cmd.Parameters.Add("@RutaS3", SqlDbType.VarChar, 50).Value = objCatalogodeDocumentosDeProductos.RutaS3;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return id;
        }
        public int BuscarConsecutivo(int IdCliente, string CodigoDeProducto)
        {
            int Consecutivo = 0;
            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_CATALOGODEDOCUMENTOSDEPRODUCTOS_CONSECUTIVO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4).Value = IdCliente;
                    cmd.Parameters.Add("@CodigoDeProducto", SqlDbType.VarChar, 50).Value = CodigoDeProducto;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        Consecutivo = Convert.ToInt32(dr["Consecutivo"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return Consecutivo;
        }

        public async Task<bool> EliminarDocumento(EliminarArchivo objEliminarArchivo)
        {
            bool SiNo = false;

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_DELETE_CASAEI_CATALOGODEDOCUMENTOSDEPRODUCTOS", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdImagen", SqlDbType.Int, 4).Value = objEliminarArchivo.IdImagen;                  
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) == 1)
                        {
                            SiNo = true;
                            
                            BucketsS3Repository objS3 = new(_configuration);
                            await objS3.BorrarObjectoAsync("3", objEliminarArchivo.RutaS3);
                          
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return SiNo;
        }
    }
}

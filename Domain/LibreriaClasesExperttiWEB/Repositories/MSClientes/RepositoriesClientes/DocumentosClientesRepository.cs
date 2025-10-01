using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class DocumentosClientesRepository : IDocumentosClientesRepository
    {

        public string SConexion { get; set; }
        string IDocumentosClientesRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public DocumentosClientesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public DocumentosClientes Buscar(int IDDocumentosDelCliente)
        {
            DocumentosClientes objDocumentosDeClientes = new();
            try
            {
                using (SqlConnection con = new SqlConnection(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_DOCUMENTOSDECLIENTES", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDDocumentosDelCliente", SqlDbType.Int, 4).Value = IDDocumentosDelCliente;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objDocumentosDeClientes.IDDocumentosDelCliente = Convert.ToInt32(dr["IDDocumentosDelCliente"]);
                        objDocumentosDeClientes.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                        objDocumentosDeClientes.IDTiposDeDocumentosDeCliente = Convert.ToInt32(dr["IDTiposDeDocumentosDeCliente"]);
                        objDocumentosDeClientes.VigenciaDesde = Convert.ToDateTime(dr["VigenciaDesde"]);
                        objDocumentosDeClientes.VigenciaHasta = Convert.ToDateTime(dr["VigenciaHasta"]);
                        objDocumentosDeClientes.ClaveDeSubTipoDocumento = Convert.ToInt32(dr["ClaveDeSubTipoDocumento"]);
                        objDocumentosDeClientes.NombreFile = string.Format("{0}", dr["NombreFile"]);
                        //objDocumentosDeClientes.NombreDelCliente = string.Format("{0}", dr["NombreDelCliente"]);
                        objDocumentosDeClientes.Encriptado = string.Format("{0}", dr["Encriptado"]);
                        objDocumentosDeClientes.RutaVirtual = string.Format("{0}", dr["RutaVirtual"]);
                        //objDocumentosDeClientes.TipoDeDocumento = string.Format("{0}", dr["TipoDeDocumento"]);
                        //objDocumentosDeClientes.DescripcionSubTipo = string.Format("{0}", dr["DescripcionSubTipo"]);
                        //objDocumentosDeClientes.IDSubTipoDeDocumento = Convert.ToInt32(dr["IDSubTipoDeDocumento"]);
                        objDocumentosDeClientes.Ruta = string.Format("{0}", dr["Ruta"]);
                        objDocumentosDeClientes.RutaVirtualOLD = string.Format("{0}", dr["RutaVirtualOLD"]);
                        //objDocumentosDeClientes.S3 = Convert.ToBoolean(dr["S3"]);
                        //objDocumentosDeClientes.RutaS3 = string.Format("{0}", dr["RutaS3"]);
                    }
                    else
                    {
                        objDocumentosDeClientes = null!;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objDocumentosDeClientes;
        }


        public int BuscarConsecutivo(int IDCliente, int IDTiposDeDocumentosDeCliente)
        {
            int Consecutivo = 0;

            try
            {
                using (SqlConnection con = new(SConexion))

                using (SqlCommand cmd = new("NET_SEARCH_DOCUMENTOSDECLIENTES_CONSECUTIVO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCLIENTE", SqlDbType.Int, 4).Value = IDCliente;
                    cmd.Parameters.Add("@IDTiposDeDocumentosDeCliente", SqlDbType.Int, 4).Value = IDTiposDeDocumentosDeCliente;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        Consecutivo = Convert.ToInt32(dr["Consecutivo"]);
                    }
                    else
                    {
                        Consecutivo = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return Consecutivo;
        }
               

        public List<DocumentosClientesGridView> Cargar(int IdCliente)
        {
            List<DocumentosClientesGridView> listDocumentosClientesGridView = new();            

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_DGVDOCUMENTOSDELCLIENTE", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int).Value = IdCliente;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {  
                            DocumentosClientesGridView objDocumentosClientesGridView = new();
                            objDocumentosClientesGridView.TipoDeDocumento = dr["TipoDeDocumento"].ToString();
                            objDocumentosClientesGridView.IDDocumentosDelCliente = Convert.ToInt32(dr["IDDOCUMENTOSDECLIENTE"]);
                            objDocumentosClientesGridView.RutaVirtual = dr["RutaVirtual"].ToString();
                            objDocumentosClientesGridView.Encriptado = dr["Encriptado"].ToString();
                            objDocumentosClientesGridView.RutaFisica = dr["RutaFisica"].ToString();
                            objDocumentosClientesGridView.IDTiposDeDocumentosDeCliente = Convert.ToInt32(dr["IDTiposDeDocumentosDeCliente"]);
                            objDocumentosClientesGridView.DescripcionStatus = dr["Status"].ToString();
                            objDocumentosClientesGridView.Observacion = dr["Observacion"].ToString();
                            objDocumentosClientesGridView.NombreFile = dr["NombreFile"].ToString();
                            objDocumentosClientesGridView.S3 = Convert.ToBoolean(dr["S3"]);
                            objDocumentosClientesGridView.RutaS3 = dr["RutaS3"].ToString();
                           
                            if (objDocumentosClientesGridView.S3)
                            {
                                listDocumentosClientesGridView.Add(objDocumentosClientesGridView);
                                continue;
                            }

                            SubiraS3deFS objSubiraS3deFS = new();
                            objSubiraS3deFS.IDCliente = IdCliente;
                            objSubiraS3deFS.IDDocumentosDelCliente = objDocumentosClientesGridView.IDDocumentosDelCliente;
                            objSubiraS3deFS.RutaFisica = objDocumentosClientesGridView.RutaFisica;
                            objSubiraS3deFS.IDTiposDeDocumentosDeCliente = objDocumentosClientesGridView.IDTiposDeDocumentosDeCliente;
                            objSubiraS3deFS.NombreFile = objDocumentosClientesGridView.NombreFile;

                            AWSS3Repository objS3Buckets = new(_configuration);
                            string RutaS3 = objS3Buckets.SubirDocaS3Synchronous(IdCliente, objSubiraS3deFS.NombreFile, objSubiraS3deFS.RutaFisica);

                            if (string.IsNullOrEmpty(RutaS3)) continue;

                            objSubiraS3deFS.RutaS3 = RutaS3;
                            int IDDocumentosDelCliente = ModificarDocumentosS3(objSubiraS3deFS);
                         
                            if (IDDocumentosDelCliente == 0) continue;
                            
                            objDocumentosClientesGridView.S3 = true;
                            objDocumentosClientesGridView.RutaS3 = RutaS3;
                            listDocumentosClientesGridView.Add(objDocumentosClientesGridView);

                            int tipoDeCambio = 2; /*'  Cambio; */
                            TiposdeDocumentosdelClienteRepository objTiposdeDocumentosdelClienteRepository = new(_configuration);
                            objTiposdeDocumentosdelClienteRepository.InsertaBitacora(7135, IdCliente, objSubiraS3deFS.IDTiposDeDocumentosDeCliente, tipoDeCambio);
                        }
                    }
                    else
                    {
                        listDocumentosClientesGridView = new(); // Asegura que no sea `null`
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return listDocumentosClientesGridView;
        }


        public int Insertar(DocumentosClientes objDocumentosClientes)
        {
            int id;

            ValidaObjeto(objDocumentosClientes);

            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_INSERT_DOCUMENTOSDECLIENTES_S3", con))
                {

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = objDocumentosClientes.IDCliente;
                    cmd.Parameters.Add("@IDTiposDeDocumentosDeCliente", SqlDbType.Int, 4).Value = objDocumentosClientes.IDTiposDeDocumentosDeCliente;
                    cmd.Parameters.Add("@VigenciaDesde", SqlDbType.DateTime, 4).Value = objDocumentosClientes.VigenciaDesde;
                    cmd.Parameters.Add("@VigenciaHasta", SqlDbType.DateTime, 4).Value = objDocumentosClientes.VigenciaHasta;
                    cmd.Parameters.Add("@ClaveDeSubTipoDocumento", SqlDbType.Int, 4).Value = objDocumentosClientes.ClaveDeSubTipoDocumento;
                    cmd.Parameters.Add("@NombreFile", SqlDbType.VarChar, 250).Value = objDocumentosClientes.NombreFile;
                    cmd.Parameters.Add("@S3", SqlDbType.Bit).Value = objDocumentosClientes.S3;
                    cmd.Parameters.Add("@RutaS3", SqlDbType.VarChar, 250).Value = objDocumentosClientes.RutaS3;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;


                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        {
                            id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);

                            if (id != 0)
                            {

                                DocumentosClientesRepository objDocumentosClientesRepository = new(_configuration);
                                DocumentosClientes objDocumentosdeClientes = new();

                                objDocumentosdeClientes = objDocumentosClientesRepository.Buscar(id);

                                if (objDocumentosdeClientes == null)
                                {
                                    objDocumentosClientesRepository.Modificar(objDocumentosdeClientes!);
                                }
                            }
                        }
                        else
                        {
                            id = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return id;
        }

        public int Modificar(DocumentosClientes objDocumentosClientes)
        {
            int id;

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_UPDATE_DOCUMENTOSDECLIENTES", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("@IDDocumentosDelCliente", SqlDbType.Int, 4).Value = objDocumentosClientes.IDDocumentosDelCliente;
                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = objDocumentosClientes.IDCliente;
                    cmd.Parameters.Add("@IDTiposDeDocumentosDeCliente", SqlDbType.Int, 4).Value = objDocumentosClientes.IDTiposDeDocumentosDeCliente;
                    cmd.Parameters.Add("@VigenciaDesde", SqlDbType.DateTime, 4).Value = objDocumentosClientes.VigenciaDesde;
                    cmd.Parameters.Add("@VigenciaHasta", SqlDbType.DateTime, 4).Value = objDocumentosClientes.VigenciaHasta;
                    cmd.Parameters.Add("@ClaveDeSubTipoDocumento", SqlDbType.Int, 4).Value = objDocumentosClientes.ClaveDeSubTipoDocumento;
                    cmd.Parameters.Add("@NombreFile", SqlDbType.VarChar, 250).Value = objDocumentosClientes.NombreFile;

                    string CadenaAEncriptar = Convert.ToString(objDocumentosClientes.IDDocumentosDelCliente);
                    objDocumentosClientes.Encriptado = Cryptografia.MD5Hash(CadenaAEncriptar);

                    cmd.Parameters.Add("@Encriptado", SqlDbType.VarChar, 32).Value = objDocumentosClientes.Encriptado;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

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
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return id;
        }



        public bool Desactivar(int IDDocumentodeCliente)
        {
            bool id;

            try
            {
                using (SqlConnection con = new SqlConnection(SConexion))
                using (var cmd = new SqlCommand("NET_CASAEI_UPDATE_DOCUMENTOSDECLIENTES_ESTATUS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDDocumentosDelCliente", SqlDbType.Int, 4).Value = IDDocumentodeCliente;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        {
                            id = Convert.ToBoolean(cmd.Parameters["@newid_registro"].Value);
                        }
                        else
                        {
                            id = false;
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
            return id;
        }


        public string NombreFile(int IDCliente, int IDDocumentosDelCliente, string file)
        {
            string NombreFile = "";

            try
            {
                if (Path.GetExtension(file) == ".pdf")
                {
                    TiposdeDocumentosdelClienteRepository objTipDocClienteD = new(_configuration);
                    TiposdeDocumentosdelCliente objTipDocCliente = objTipDocClienteD.Buscar(IDDocumentosDelCliente);
                    if (objTipDocCliente.NombreDocumento == null)
                    {
                        throw new ArgumentException("No existe un nombre definido para este tipo de documento");
                    }

                    int Consecutivo = 0;
                    DocumentosClientesRepository objDocCli = new(_configuration);
                    Consecutivo = objDocCli.BuscarConsecutivo(IDCliente, IDDocumentosDelCliente);

                    NombreFile = objTipDocCliente.NombreDocumento + "_" + Consecutivo.ToString("00") + Path.GetExtension(file);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return NombreFile;
        }

        public List<DocumentosClientes> CargarEncargo(int IDCliente)
        {
            List<DocumentosClientes> listDocumentosDeClientes = new();
            try
            {
                using (SqlConnection con = new SqlConnection(SConexion))

                using (SqlCommand cmd = new("NET_LOAD_DOCUMENTOSDECLIENTES_ENCARGOCONFERIDO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = IDCliente;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            DocumentosClientes objDocumentosDeClientes = new();

                            objDocumentosDeClientes.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                            objDocumentosDeClientes.IDDocumentosDelCliente = Convert.ToInt32(dr["IDDocumentosDelCliente"]);
                            objDocumentosDeClientes.TipoDeDocumento = dr["TipoDeDocumento"].ToString();
                            objDocumentosDeClientes.RutaS3 = dr["RutaS3"].ToString();
                            objDocumentosDeClientes.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);

                            listDocumentosDeClientes.Add(objDocumentosDeClientes);
                        }
                    }
                    else
                    {
                        listDocumentosDeClientes = null!;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return listDocumentosDeClientes;
        }

        public void ValidaObjeto(DocumentosClientes objDocumentosClientes)
        {
            if (string.IsNullOrEmpty(objDocumentosClientes.NombreDelCliente))
            {
                throw new Exception("Ingrese el Cliente");
            }

            if (string.IsNullOrEmpty(objDocumentosClientes.TipoDeDocumento))
            {
                throw new Exception("Sleccione el tipo de Documento");
            }

        }

        public int ModificarDocumentosS3(SubiraS3deFS objSubiraS3deFS)
        {
            int id;

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_UPDATE_DOCUMENTOSDECLIENTESS3", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDDocumentosDelCliente", SqlDbType.Int, 4).Value = objSubiraS3deFS.IDDocumentosDelCliente;
                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = objSubiraS3deFS.IDCliente;
                    cmd.Parameters.Add("@IDTiposDeDocumentosDeCliente", SqlDbType.Int, 4).Value = objSubiraS3deFS.IDTiposDeDocumentosDeCliente;
                    cmd.Parameters.Add("@RutaS3", SqlDbType.VarChar, 250).Value = objSubiraS3deFS.RutaS3;

                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    
                    using (cmd.ExecuteReader())
                    {
                        id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                        id = (id != -1) ? id : 0;
                    }
                }  
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return id;
        }

        public List<int> CargarNoEstanS3() 
        {
            List<int> listIdCliente = new();

            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_DOCUMENTOSDELCLIENTE_NOESTANS3", con)
                {
                    CommandType = CommandType.StoredProcedure
                };              
                con.Open();
                
                using SqlDataReader dr = cmd.ExecuteReader();
                
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        int idCliente = dr.GetInt32(0);
                        // Asume que el valor a leer es un int y está en la primera columna (índice 0)
                        //listIdCliente.Add(dr.GetInt32(0));
                        Cargar(idCliente);
                    }
                }                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return listIdCliente;
        }
    }
}

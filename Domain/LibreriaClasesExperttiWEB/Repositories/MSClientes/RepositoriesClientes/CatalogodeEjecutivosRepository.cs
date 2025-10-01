using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class CatalogodeEjecutivosRepository : ICatalogodeEjecutivosRepository
    {

        //public string SConexion { get; set; }
        //public IConfiguration _configuration;
        //public SqlConnection con;

        //public CatalogodeEjecutivosRepository(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        //}

        public string SConexion { get; set; }
        string ICatalogodeEjecutivosRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public CatalogodeEjecutivosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        /// <summary>
        /// Tabla para llenar el GridView de las Oficinas 
        /// </summary>
        /// <param name="IdCliente"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        //public List<CatalogodeEjecutivosPorCliente> Cargar(int IdCliente, int IdOficina, int IdDepartamento, int Operacion)
        public List<CatalogodeEjecutivosPorCliente> Cargar(int IdCliente, int IdOficina, int IdUsuario)
        {
            List<CatalogodeEjecutivosPorCliente> listCatalogodeEjecutivos = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_CASAEI_EJECUTIVOS_POR_CLIENTE", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = IdCliente;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int).Value = IdOficina;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                    //cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int).Value = IdDepartamento;
                    //cmd.Parameters.Add("@Operacion", SqlDbType.Int).Value = Operacion;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            CatalogodeEjecutivosPorCliente objCatalogodeEjecutivos = new()
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Principal = dr["Principal"].ToString(),
                                Respaldo = dr["Respaldo"].ToString(),
                                Oficina = dr["Oficina"].ToString(),
                                Operacion = dr["Operacion"].ToString(),
                                IdDepartamento = Convert.ToInt32(dr["IdDepartamento"]),
                                NombreDepartamento = dr["NombreDepartamento"].ToString(),
                                Modificar = Convert.ToBoolean(dr["Modificar"])
                            };
                            listCatalogodeEjecutivos.Add(objCatalogodeEjecutivos);
                        }
                    }
                    else
                        listCatalogodeEjecutivos = null/* TODO Change to default(_) if this is not a reference type */;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return listCatalogodeEjecutivos;
        }

        /// <summary>
        /// Inertar 
        /// </summary>
        /// <param name="objCatalogodeEjecutivos"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int Insertar(CatalogodeEjecutivos objCatalogodeEjecutivos)
        {
            int Respuesta = 0;

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_INSERTAR_CASAEI_EJECUTIVOS_POR_CLIENTE", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.IdCliente;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.IdUsuario;
                    cmd.Parameters.Add("@IdUsuarioBK", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.IdUsuarioBK;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.IdOficina;
                    cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.Operacion;
                    cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.IdDepartamento;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();

                        Respuesta = Convert.ToInt32(dr["Id"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return Respuesta;
        }


        /// <summary>
        /// Modificar 
        /// </summary>
        /// <param name="objCatalogodeEjecutivos"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int Modificar(CatalogodeEjecutivos objCatalogodeEjecutivos)
        {
            int Respuesta = 0;
            try
            {

                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_UPDATE_CASAEI_EJECUTIVOS_POR_CLIENTE", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdEjecutivo", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.IdEjecutivo;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.IdCliente;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.IdUsuario;
                    cmd.Parameters.Add("@IdUsuarioBK", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.IdUsuarioBK;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.IdOficina;
                    cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.Operacion;
                    cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.IdDepartamento;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();

                        Respuesta = Convert.ToInt32(dr["Id"]);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return Respuesta;
        }

        public int SubirLayout(CatalogodeEjecutivosSubirLayout objSubirLayout)
        {
            int RegistrosInsertados;
            try
            {
                int IdOficina = objSubirLayout.IdOficina;
                int Operacion = objSubirLayout.Operacion;
                int IdDepartamento = objSubirLayout.IdDepartamento;
                string NombreArchivo = objSubirLayout.NombreArchivo;

                UbicaciondeArchivosRepository objUbicacionDeArchivosRepository = new(_configuration);
                UbicaciondeArchivos objRuta = objUbicacionDeArchivosRepository.Buscar(171);
                if (Directory.Exists(objRuta.Ubicacion) == false)
                {
                    Directory.CreateDirectory(objRuta.Ubicacion);
                }

                NombreArchivo = objRuta.Ubicacion + NombreArchivo;
                byte[] bytes = Convert.FromBase64String(objSubirLayout.ArchivoBase64);
                File.WriteAllBytes(NombreArchivo, bytes);

                //CatalogodeEjecutivosRepository objSubir = new(_configuration);
                RegistrosInsertados = ArchivoCargaMasiva(NombreArchivo, IdOficina, Operacion, IdDepartamento);

                File.Delete(NombreArchivo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return RegistrosInsertados;
        }

        /// <summary>
        /// Carga Masiva todas las oficinas
        /// </summary>
        /// <param name="Archivo"></param>
        /// <param name="Oficina"></param>
        /// <param name="Operacion"></param>
        /// <param name="IdDepartamento"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int ArchivoCargaMasiva(string Archivo, int Oficina, int Operacion, int IdDepartamento)
        {
            int RegistrosInsertados = 0;
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_UPLOAD_ARCHIVOEJECUTIVOCLIENTE_WEB", con))
                {
                    con.Open();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Archivo", SqlDbType.VarChar, 250).Value = Archivo;
                    cmd.Parameters.Add("@IDOficina", SqlDbType.Int, 4).Value = Oficina;
                    cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4).Value = Operacion;
                    cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int, 4).Value = IdDepartamento;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        RegistrosInsertados = Convert.ToInt32(dr["RegistrosInsertados"]);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return RegistrosInsertados;
        }

        /// <summary>
        /// Devuelve la lista en donde indica en que departamentos esta activo el usuario(para llenar el combo)
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <param name="IdOficina"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<DropDownListDatos> CargarDepartamentos(int IdUsuario)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODEPARTAMENTOS_EJECUTIVOSCLIENTE", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = IdUsuario;
                    using SqlDataReader reader = cmd.ExecuteReader();
                    comboList = SqlDataReaderToDropDownList.DropDownList<DropDownListDatos>(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return comboList;
        }

        public int Eliminar(CatalogodeEjecutivos objCatalogodeEjecutivos)
        {
            int Respuesta;
            try
            {

                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_DELETE_CASAEI_CATALOGODEEJECUTIVOS", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdEjecutivo", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.IdEjecutivo;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.IdCliente;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.IdUsuario;
                    cmd.Parameters.Add("@IdUsuarioBK", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.IdUsuarioBK;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.IdOficina;
                    cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.Operacion;
                    cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int, 4).Value = objCatalogodeEjecutivos.IdDepartamento;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {
                        //if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        //{
                        //if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) == 1)
                        //{
                            Respuesta = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                        //}

                        //}                  
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return Respuesta;
        }

       

    }
}

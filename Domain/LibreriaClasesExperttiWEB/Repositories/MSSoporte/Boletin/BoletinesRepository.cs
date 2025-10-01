using DocumentFormat.OpenXml.Office2010.Excel;
using LibreriaClasesAPIExpertti.Entities.EntitiesBoletin;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSSoporte.Boletin
{
    public class BoletinesRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        BucketsS3Repository _bucketRepo;

        public BoletinesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
            _bucketRepo = new(_configuration);
        }

        public Boletines GetBoletinById(int idBoletin)
        {
            Boletines boletin = null;

            using (SqlConnection connection = new SqlConnection(sConexion))
            {
                using (SqlCommand command = new SqlCommand("NET_SEARCH_CASAEI_BOLETINES", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar el parámetro requerido
                    command.Parameters.Add(new SqlParameter("@IdBoletin", idBoletin));

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            boletin = new Boletines
                            {
                                IdBoletin = reader.GetInt32(reader.GetOrdinal("IdBoletin")),
                                Titulo = reader["Titulo"].ToString(),
                                NoVersion = reader["NoVersion"].ToString(),
                                FechaPublicacion = reader.IsDBNull(reader.GetOrdinal("FechaPublicacion"))
                                ? null
                                : reader.GetDateTime(reader.GetOrdinal("FechaPublicacion")),
                                FechaAlta = reader.GetDateTime(reader.GetOrdinal("FechaAlta")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo")),
                                IdDepartamento = reader.GetInt32(reader.GetOrdinal("IdDepartamento")),
                                IdDatosDeEmpresa = reader.GetInt32(reader.GetOrdinal("IDDatosDeEmpresa")),
                                FechaVigencia = reader.IsDBNull(reader.GetOrdinal("FechaVigencia"))
                                ? null
                                : reader.GetDateTime(reader.GetOrdinal("FechaVigencia")),
                                TodoslosDeptos = reader.IsDBNull(reader.GetOrdinal("TodoslosDeptos"))
                                ? null
                                : reader.GetBoolean(reader.GetOrdinal("TodoslosDeptos")),
                                TodoslasApps = reader.IsDBNull(reader.GetOrdinal("TodoslasApps"))
                                ? null
                                : reader.GetBoolean(reader.GetOrdinal("TodoslasApps")),
                                IdModulo = reader.IsDBNull(reader.GetOrdinal("IdModulo"))
                                ? null
                                : reader.GetInt32(reader.GetOrdinal("IdModulo")),
                                RutaS3 = reader.IsDBNull(reader.GetOrdinal("RutaS3"))
                                ? null
                                : _bucketRepo.URL(reader["RutaS3"].ToString(), "grupoei.proyectos")
                            };
                        }
                    }
                }
            }


            if (boletin != null)
            {
                var listaDepartamentos = GetBoletinesDepartamentos(idBoletin);
                boletin.Departamentos = listaDepartamentos.Select(d => d.IdDepartamento).ToArray();
            }

            return boletin;
        }

        public Boletines InsertBoletin(BoletinesInsert dto)
        {
            int newid_registro = 0;
            int[] Departamentos = dto.Departamentos;
            Boletines boletin = new Boletines();

            if (dto.TodoslosDeptos == false && Departamentos.IsNullOrEmpty())
            {
                throw new ArgumentException("Debe ingresar los departamentos cuando TodoslosDeptos es false");
            }

            using (SqlConnection connection = new SqlConnection(sConexion))
            {
                using (SqlCommand command = new SqlCommand("NET_INSERT_CASAEI_BOLETINES", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Titulo", dto.Titulo);
                    //command.Parameters.AddWithValue("@FechaPublicacion", dto.FechaPublicacion);
                    command.Parameters.AddWithValue("@FechaVigencia", dto.FechaVigencia);
                    //command.Parameters.AddWithValue("@Activo", dto.Activo);
                    command.Parameters.AddWithValue("@IdDepartamento", dto.IdDepartamento);
                    command.Parameters.AddWithValue("@TodoslosDeptos", dto.TodoslosDeptos);
                    command.Parameters.AddWithValue("@TodoslasApps", dto.TodoslasApps);
                    command.Parameters.AddWithValue("@idModulo", dto.IdModulo);
                    command.Parameters.AddWithValue("@IDDatosDeEmpresa", dto.IdDatosDeEmpresa);

                    SqlParameter outputParam = new SqlParameter("@newid_registro", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputParam);

                    connection.Open();
                    command.ExecuteNonQuery();

                    newid_registro = Convert.ToInt32(outputParam.Value);



                    boletin = GetBoletinById(newid_registro);

                    boletin.Departamentos = Departamentos;

                    bool correct = InsertBoletinDepartamentos(Departamentos, newid_registro);

                }
            }

            return boletin;
        }

        private bool InsertBoletinDepartamentos(int[] Departamentos, int idBoletin)
        {
            bool correct = true;
            try
            {
                using (SqlConnection connection = new SqlConnection(sConexion))
                {
                    using (SqlCommand command = new SqlCommand("NET_INSERT_CASAEI_BOLETINESDEPARTAMENTOS", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        foreach (int departamento in Departamentos)
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@IdBoletin", idBoletin);
                            command.Parameters.AddWithValue("@IdDepartamento", departamento);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                correct = false;
            }
            return correct;
        }

        public int UpdateBoletin(BoletinesUpdate dto)
        {
            int resultado = 0;
            int[] Departamentos = dto.Departamentos;

            if (dto.TodoslosDeptos == false && Departamentos.IsNullOrEmpty())
            {
                throw new ArgumentException("Debe ingresar los departamentos cuando TodoslosDeptos es false");
            }

            using (SqlConnection connection = new SqlConnection(sConexion))
            {
                using (SqlCommand command = new SqlCommand("NET_UPDATE_CASAEI_BOLETINES", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdBoletin", dto.IdBoletin);
                    command.Parameters.AddWithValue("@Titulo", dto.Titulo);
                    //command.Parameters.AddWithValue("@NoVersion", dto.NoVersion);
                    //command.Parameters.AddWithValue("@FechaPublicacion", dto.FechaPublicacion);
                    command.Parameters.AddWithValue("@FechaVigencia", dto.FechaVigencia);
                    //command.Parameters.AddWithValue("@Activo", dto.Activo);
                    command.Parameters.AddWithValue("@IdDepartamento", dto.IdDepartamento);
                    command.Parameters.AddWithValue("@TodoslosDeptos", dto.TodoslosDeptos);
                    command.Parameters.AddWithValue("@TodoslasApps", dto.TodoslasApps);
                    command.Parameters.AddWithValue("@idModulo", dto.IdModulo);
                    command.Parameters.AddWithValue("@IDDatosDeEmpresa", dto.IdDatosDeEmpresa);


                    SqlParameter outputParam = new SqlParameter("@newid_registro", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputParam);

                    connection.Open();
                    command.ExecuteNonQuery();

                    if (DeleteDepartamentosByBoletin(dto.IdBoletin))
                    {
                        bool correct = InsertBoletinDepartamentos(Departamentos, dto.IdBoletin);
                    }



                    resultado = Convert.ToInt32(outputParam.Value);


                }
            }

            return resultado;
        }

        private bool DeleteDepartamentosByBoletin(int idBoletin)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(sConexion))
                {
                    using (SqlCommand Command = new SqlCommand("NET_DELETE_CASAEI_DEPARTAMENTOS_BY_BOLETIN", connection))
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        Command.Parameters.AddWithValue("@IdBoletin", idBoletin);
                        Command.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<BoletinesDepartamentos> GetBoletinesDepartamentos(int IdBoletin)
        {
            List<BoletinesDepartamentos> lista = new List<BoletinesDepartamentos>();

            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_LIST_CASAEI_BOLETINESDEPARTAMENTOS", cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdBoletin", IdBoletin);

                    try
                    {
                        cn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                BoletinesDepartamentos bd = new BoletinesDepartamentos
                                {
                                    IdBoletinDetalleDep = Convert.ToInt32(dr["IdBoletinDetalleDep"]),
                                    IdBoletin = Convert.ToInt32(dr["IdBoletin"]),
                                    IdDepartamento = Convert.ToInt32(dr["IdDepartamento"])
                                };

                                lista.Add(bd);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al listar boletines departamentos: " + ex.Message);
                    }
                }
            }

            return lista;
        }


        public List<Boletines> GetBoletinesActivos(int IdUsuario, int IdDepartamento, int IdModulo)
        {
            List<Boletines> boletines = new List<Boletines>();

            //Obtener IDDatosDeEmpresa usando IdUsuario
            CatalogoDeUsuariosRepository UsuariosRepository = new CatalogoDeUsuariosRepository(_configuration);
            CatalogoDeUsuarios objUsuarios = UsuariosRepository.BuscarPorId(IdUsuario);
            //

            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_LIST_CASAEI_BOLETINES_ACTIVOS", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                    cmd.Parameters.AddWithValue("@IdDepartamento", IdDepartamento);
                    cmd.Parameters.AddWithValue("@IdModulo", IdModulo);
                    cmd.Parameters.AddWithValue("@IDDatosDeEmpresa", objUsuarios.IDDatosDeEmpresa);

                    try
                    {
                        cn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Boletines b = new Boletines
                                {
                                    IdBoletin = Convert.ToInt32(dr["IdBoletin"]),
                                    Titulo = dr["Titulo"].ToString(),
                                    NoVersion = dr["NoVersion"].ToString(),
                                    Activo = Convert.ToBoolean(dr["Activo"])
                                };

                                if (dr["FechaPublicacion"] != DBNull.Value)
                                    b.FechaPublicacion = Convert.ToDateTime(dr["FechaPublicacion"]);

                                if (dr["FechaAlta"] != DBNull.Value)
                                    b.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);

                                if (dr["IdDepartamento"] != DBNull.Value)
                                    b.IdDepartamento = Convert.ToInt32(dr["IdDepartamento"]);

                                if (dr["FechaVigencia"] != DBNull.Value)
                                    b.FechaVigencia = Convert.ToDateTime(dr["FechaVigencia"]);

                                if (dr["TodoslosDeptos"] != DBNull.Value)
                                    b.TodoslosDeptos = Convert.ToBoolean(dr["TodoslosDeptos"]);

                                if (dr["TodoslasApps"] != DBNull.Value)
                                    b.TodoslasApps = Convert.ToBoolean(dr["TodoslasApps"]);

                                if (dr["idModulo"] != DBNull.Value)
                                    b.IdModulo = Convert.ToInt32(dr["idModulo"]);

                                if (dr["RutaS3"] != DBNull.Value)
                                    b.RutaS3 = _bucketRepo.URL(dr["RutaS3"].ToString(), "grupoei.proyectos");

                                if (b != null)
                                {
                                    var listaDepartamentos = GetBoletinesDepartamentos(b.IdBoletin);
                                    b.Departamentos = listaDepartamentos.Select(d => d.IdDepartamento).ToArray();
                                }

                                boletines.Add(b);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al obtener boletines activos: " + ex.Message);
                    }
                }
            }
            return boletines;
        }

        public List<Dictionary<string, object>> GetBoletines(Filtros filtros)
        {
            List<Dictionary<string, object>> boletines = new List<Dictionary<string, object>>();

            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_LIST_CASAEI_BOLETINES", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Activo", (object?)filtros.Activo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdDepartamento", (object?)filtros.IdDepartamento ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaVigencia", (object?)filtros.FechaVigencia ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TodosLosDeptos", (object?)filtros.TodoslosDeptos ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TodasLasApps", (object?)filtros.TodoslasApps ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdModulo", (object?)filtros.IdModulo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdDatosDeEmpresa", (object?)filtros.IdDatosDeEmpresa ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Titulo", (object?)filtros.Titulo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaPublicacion", (object?)filtros.FechaPublicacion ?? DBNull.Value);


                    try
                    {
                        cn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, object> row = new Dictionary<string, object>();

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetName(i);
                                    object value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                                    row[columnName] = value;
                                }

                                if (row["IdBoletin"] != null)
                                {
                                    var listaDepartamentos = GetBoletinesDepartamentos(Convert.ToInt32(row["IdBoletin"].ToString()));
                                    row["Departamentos"] = listaDepartamentos.Select(d => d.IdDepartamento).ToArray();
                                }

                                boletines.Add(row);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al obtener boletines activos: " + ex.Message);
                    }
                }
            }
            return boletines;
        }

        public async Task<Dictionary<object, object>> PublicarBoletin(int IdBoletin)
        {
            Dictionary<object, object> Response = new Dictionary<object, object>();
            bool correct = false;
            string url = string.Empty;
            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_ACTIVATE_CASAEI_BOLETIN", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idBoletin", IdBoletin);

                    SqlParameter outputParam = new SqlParameter("@Correct", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    correct = Convert.ToBoolean(outputParam.Value);
                    if (correct)
                    {
                        ImpresionBoletinRepository impresionBoletin = new(_configuration);
                        url = await impresionBoletin.GenerarBoletin(IdBoletin);
                        Response["Success"] = correct;
                        Response["URL"] = url;
                    }
                    else
                    {
                        Response["Success"] = correct;
                        Response["Message"] = "No se ha encontrado el boletín especificado o no existe en el sistema.";
                    }
                }
            }
            return Response;
        }
        public Dictionary<object, object> DesactivarBoletin(int IdBoletin)
        {
            Dictionary<object, object> Response = new Dictionary<object, object>();
            bool correct = false;
            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_DEACTIVATE_CASAEI_BOLETIN", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idBoletin", IdBoletin);

                    SqlParameter outputParam = new SqlParameter("@Correct", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(outputParam);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    correct = Convert.ToBoolean(outputParam.Value);
                    if (correct)
                    {
                        Response["Success"] = correct;
                        Response["Message"] = "El boletín ha sido desactivado exitosamente.";
                    }
                    else
                    {
                        Response["Success"] = correct;
                        Response["Message"] = "No se ha encontrado el boletín especificado o no existe en el sistema.";
                    }
                }
            }
            return Response;
        }

        public List<Boletines> GetBoletinesInactivos(int IdDepartamento)
        {
            List<Boletines> boletines = new List<Boletines>();
            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_LIST_CASAEI_BOLETINES_INACTIVOS", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdDepartamento", IdDepartamento);

                    try
                    {
                        cn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Boletines b = new Boletines
                                {
                                    IdBoletin = Convert.ToInt32(dr["IdBoletin"]),
                                    Titulo = dr["Titulo"].ToString(),
                                    NoVersion = dr["NoVersion"].ToString(),
                                    Activo = Convert.ToBoolean(dr["Activo"])
                                };

                                if (dr["FechaPublicacion"] != DBNull.Value)
                                    b.FechaPublicacion = Convert.ToDateTime(dr["FechaPublicacion"]);

                                if (dr["FechaAlta"] != DBNull.Value)
                                    b.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);

                                if (dr["IdDepartamento"] != DBNull.Value)
                                    b.IdDepartamento = Convert.ToInt32(dr["IdDepartamento"]);

                                if (dr["FechaVigencia"] != DBNull.Value)
                                    b.FechaVigencia = Convert.ToDateTime(dr["FechaVigencia"]);

                                if (dr["TodoslosDeptos"] != DBNull.Value)
                                    b.TodoslosDeptos = Convert.ToBoolean(dr["TodoslosDeptos"]);

                                if (dr["TodoslasApps"] != DBNull.Value)
                                    b.TodoslasApps = Convert.ToBoolean(dr["TodoslasApps"]);

                                if (dr["idModulo"] != DBNull.Value)
                                    b.IdModulo = Convert.ToInt32(dr["idModulo"]);

                                if (dr["RutaS3"] != DBNull.Value)
                                    b.RutaS3 = _bucketRepo.URL(dr["RutaS3"].ToString(), "grupoei.proyectos");

                                if (b != null)
                                {
                                    var listaDepartamentos = GetBoletinesDepartamentos(b.IdBoletin);
                                    b.Departamentos = listaDepartamentos.Select(d => d.IdDepartamento).ToArray();
                                }

                                boletines.Add(b);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al obtener boletines activos: " + ex.Message);
                    }
                }
            }
            return boletines;
        }
    }
}

using LibreriaClasesAPIExpertti.Utilities.Converters;
using LibreriaClasesAPIExpertti.Entities.EntitiesPreasignarRieles;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesPreasignarRieles
{
    public class PreasignarRielesRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public PreasignarRielesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = configuration.GetConnectionString("dbCASAEI");
        }

        public async Task<List<Catalogodecategoria>> GetCatalogodecategoriaPorDepto(int IdDepartamento)
        {
            var lista = new List<Catalogodecategoria>();
            try
            {
                using (con = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATEGORIASPORDEPARTAMENTO", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idDepartamento", SqlDbType.Int).Value = IdDepartamento;
                    using SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                    lista = SqlDataReaderToList.DataReaderMapToList<Catalogodecategoria>(rdr);


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return lista.ToList();
        }

        public async Task<int> InsertarPreasignacionDeRieles(string GuiaHouse, int IdCategoria, int IdUsuario)
        {
            int id = 0;
            try
            {
                using (con = new(sConexion))
                using (SqlCommand cmd = new("NET_INSERT_CASAEI_PREASIGNACIONDERIELES", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 15).Value = GuiaHouse;
                    cmd.Parameters.Add("@idCategoria", SqlDbType.Int, 4).Value = IdCategoria;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = IdUsuario;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (await cmd.ExecuteReaderAsync())
                    {
                        id = (int)cmd.Parameters["@newid_registro"].Value;
                        cmd.Parameters.Clear();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + "NET_INSERT_CASAEI_PREASIGNACIONDERIELES");
            }

            return id;
        }

        public List<Preasignacionderieles> GetPreasignarRielesPorGuia(string GuiaHouse)
        {
            var lista = new List<Preasignacionderieles>();
            try
            {
                using (con = new SqlConnection(sConexion))
                {
                    con.Open();
                    var cmd = new SqlCommand("NET_SEARCH_CASAEI_PREASIGNACIONDERIELES", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 15).Value = GuiaHouse;
                    SqlDataReader rdr = cmd.ExecuteReader();

                    lista = SqlDataReaderToList.DataReaderMapToList<Preasignacionderieles>(rdr);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return lista.ToList();
        }
    }
}

using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class CatalogoDeCategoriasRepository : ICatalogoDeCategoriasRepository
    {
        public string SConexion { get; set; }
        string ICatalogoDeCategoriasRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public CatalogoDeCategoriasRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<CatalogoDeCategorias> Cargar()
        {
            List<CatalogoDeCategorias> listCatalogoDeCategorias = new();

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODECATEGORIAS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            CatalogoDeCategorias objCategoria = new()
                            {
                                IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                                Descripcion = string.Format("{0}", dr["Descripcion"]),
                                IdRiel = Convert.ToInt32(dr["IdRiel"])
                            };
                            listCatalogoDeCategorias.Add(objCategoria);
                        }
                    }
                    else
                    {
                        listCatalogoDeCategorias = null!;
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }


            return listCatalogoDeCategorias;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCategoria"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public CatalogoDeCategorias Buscar(int IdCategoria)
        {
            CatalogoDeCategorias objCategoria = new();

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_CATALOGODECATEGORIAS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDCATEGORIA", SqlDbType.Int, 4).Value = IdCategoria;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objCategoria.IdCategoria = Convert.ToInt32(dr["IdCategoria"]);
                        objCategoria.Descripcion = string.Format("{0}", dr["Descripcion"]);
                        objCategoria.IdRiel = Convert.ToInt32(dr["IdRiel"]);
                    }

                    else
                    {
                        objCategoria = null!;
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }


            return objCategoria;
        }


    }
}

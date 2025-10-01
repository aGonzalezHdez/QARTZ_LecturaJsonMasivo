using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaClasesAPIExpertti.Entities.EntitiesDespacho;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;

namespace LibreriaClasesAPIExpertti.Repositories.MSDespachos.RepositoriesImportacion
{
    public class ControldeEventosConsolRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public ControldeEventosConsolRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int InsertarEvento(ControldeEventosConsol lEventos, int idDepartamento, int idOficina)
        {
            int id = 0;
            try
            {
                var cntlEvD = new ControldeEventosConsolRepository(_configuration);
                var objCatCheckD = new CatalogodeCheckPointsRepository(_configuration);
                var objCatCheck = objCatCheckD.Buscar(lEventos.IDCheckPoint, idOficina);

                if (objCatCheck != null)
                {
                    if (objCatCheck.PrecedenciaObligatoria)
                    {
                        if (objCatCheck.ListaDeIDPrecedencias1 != null && objCatCheck.ListaDeIDPrecedencias1.Count > 0)
                        {
                            if (ExistePrecedencia(lEventos.IdAnexos, objCatCheck.ListaDeIDPrecedencias1))
                            {
                                id = cntlEvD.Insertar(lEventos, objCatCheck.Duplicidad, objCatCheck.Automatico);
                            }
                            else
                            {
                                var lstPre = Prescedencias(objCatCheck.ListaDeIDPrecedencias1, idOficina);
                                string cad = string.Join(" | ", lstPre);
                                throw new Exception("Necesita Checkpoint de precedencia : " + cad);
                            }
                        }
                        else
                        {
                            id = cntlEvD.Insertar(lEventos, objCatCheck.Duplicidad, objCatCheck.Automatico);
                        }
                    }
                    else
                    {
                        id = cntlEvD.Insertar(lEventos, objCatCheck.Duplicidad, objCatCheck.Automatico);
                    }
                }
                else
                {
                    throw new Exception("No existe el checkpoint que está tratando de utilizar");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return id;
        }

        protected bool ExistePrecedencia(int idAnexos, List<int> lst)
        {
            foreach (var item in lst)
            {
                if (BuscaPrescedencia(idAnexos, item))
                {
                    return true;
                }
            }
            return false;
        }

        protected bool BuscaPrescedencia(int idAnexos, int idPrescedencia)
        {
            bool existe = false;

            using (var cn = new SqlConnection(sConexion))
            {
                using (var cmd = new SqlCommand("NET_SEARCH_SI_EXISTEEVENTO_EN_ANEXO", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdAnexos", idAnexos);
                    cmd.Parameters.AddWithValue("@IDCheckPoint", idPrescedencia);
                    cn.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        existe = dr.HasRows;
                    }
                }
            }
            return existe;
        }

        protected int Insertar(ControldeEventosConsol lcontroldeeventos, bool duplicidad, bool automatico)
        {
            int id;
            using (var cn = new SqlConnection(sConexion))
            {
                using (var cmd = new SqlCommand("NET_INSERT_CONTROLDEEVENTOSCONSOL", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IDCheckPoint", lcontroldeeventos.IDCheckPoint);
                    cmd.Parameters.AddWithValue("@idAnexos", lcontroldeeventos.IdAnexos);
                    cmd.Parameters.AddWithValue("@IDUsuario", lcontroldeeventos.IDUsuario);
                    cmd.Parameters.AddWithValue("@FechaEvento", lcontroldeeventos.FechaEvento);
                    cmd.Parameters.AddWithValue("@Duplica", duplicidad);
                    cmd.Parameters.AddWithValue("@Automatico", automatico);

                    var param = new SqlParameter("@newid_registro", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(param);
                    cn.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        id = (int)param.Value != -1 ? (int)param.Value : 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + " NET_INSERT_CONTROLDEEVENTOSCONSOL");
                    }
                }
            }
            return id;
        }

        public bool ValidarExisteConsolAnexo(int idAnexos)
        {
            bool result = false;

            using (var cn = new SqlConnection(sConexion))
            {
                using (var cmd = new SqlCommand("Pocket.NET_SEARCH_ANEXO_EN_EVENTOS", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdAnexos", idAnexos);
                    cn.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        result = dr.HasRows;
                    }
                }
            }
            return result;
        }

        protected List<string> Prescedencias(List<int> lst, int idOficina)
        {
            var lstPresc = new List<string>();
            var objCatD = new CatalogodeCheckPointsRepository(_configuration);

            if (lst.Count == 0)
            {
                return null;
            }

            foreach (var item in lst)
            {
                var objCat = objCatD.Buscar(item, idOficina);
                if (objCat != null)
                {
                    lstPresc.Add(objCat.Descripcion);
                }
            }

            return lstPresc;
        }

    }
}

using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using LibreriaClasesAPIExpertti.Entities.EntitiesTrazabilidad;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones.CausasDeDemora;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesOperaciones.CausasDeDemora
{
    public class CausasDeDemoraRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public CausasDeDemoraRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<DropDownListDatos> CargarCausantes()
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODECAUSANTES", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
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

        public List<DropDownListDatos> CargarSubCausantes(int IdCausante)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODESUBCAUSANTES", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdCausante", SqlDbType.Int, 4).Value = IdCausante;

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

        public List<DropDownListDatos> CargarCausas(int IdSubCausante)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODECAUSASYCAUSANTES", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdSubCausante", SqlDbType.Int, 4).Value = IdSubCausante;

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

        public async Task<int> Insertar(CausasDeDemoraPorGuia objCausasDeDemora)
        {
            int id = 0;

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_INSERT_CASAEI_CAUSASDEDEMORAPORGUIA", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Guia", SqlDbType.VarChar, 15).Value = objCausasDeDemora.Guia;
                    cmd.Parameters.Add("@IdCausa", SqlDbType.Int, 4).Value = objCausasDeDemora.IdCausa;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = objCausasDeDemora.IdUsuario;
                    cmd.Parameters.Add("@IdDatosdeEmpresa", SqlDbType.Int, 4).Value = objCausasDeDemora.IdDatosdeEmpresa;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;


                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                            id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                        else
                            id = 0;

                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + "NET_INSERT_CASAEI_CAUSASDEDEMORAPORGUIA");
            }

            return id;
        }

        public async Task<bool> BuscaSiExiste(string Guia, int IdCausa)
        {
            bool Existe;

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_SEARCH_CASAEI_CAUSASDEDEMORAPORGUIA", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@GUIA", SqlDbType.VarChar, 15).Value = Guia;
                    cmd.Parameters.Add("@IDCAUSA", SqlDbType.Int, 4).Value = IdCausa;

                    // Ejecuto el sp y obtengo el DataSet
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())

                        if (dr.HasRows)
                            Existe = true;
                        else
                            Existe = false;
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return Existe;
        }

        public async Task<List<ResultadoCausasDeDemoraPorGuia>> CargarCausasDeDemoraPorGuia(string Guia, int IdDatosdeEmpresa)
        {
            List<ResultadoCausasDeDemoraPorGuia> lstCausas = new();

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CASAEI_CAUSASDEDEMORAPORGUIA", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@GUIA", SqlDbType.VarChar, 15).Value = Guia;
                    cmd.Parameters.Add("@IdDatosdeEmpresa", SqlDbType.Int, 4).Value = IdDatosdeEmpresa;

                    // Ejecuto el sp y obtengo el DataSet
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())

                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                ResultadoCausasDeDemoraPorGuia objCAUSASDEDEMORAPORGUIA = new();
                                objCAUSASDEDEMORAPORGUIA.IdDemoraGuia = Convert.ToInt32(dr["IdDemoraGuia"]);
                                objCAUSASDEDEMORAPORGUIA.Guia = dr["Guia"].ToString();
                                objCAUSASDEDEMORAPORGUIA.IdCausante = Convert.ToInt32(dr["IdCausante"]);
                                objCAUSASDEDEMORAPORGUIA.Causante = dr["Causante"].ToString();
                                objCAUSASDEDEMORAPORGUIA.IdSubCausante = Convert.ToInt32(dr["IdSubCausante"]);
                                objCAUSASDEDEMORAPORGUIA.SubCausante = dr["SubCausante"].ToString();
                                objCAUSASDEDEMORAPORGUIA.IdCausa = Convert.ToInt32(dr["IdCausa"]);
                                objCAUSASDEDEMORAPORGUIA.Causa = dr["Causa"].ToString();
                                objCAUSASDEDEMORAPORGUIA.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                                objCAUSASDEDEMORAPORGUIA.FechaRegistro = dr["FechaRegistro"].ToString();
                                objCAUSASDEDEMORAPORGUIA.IdDatosdeEmpresa = Convert.ToInt32(dr["IdDatosdeEmpresa"]);

                                lstCausas.Add(objCAUSASDEDEMORAPORGUIA);
                            }
                        }
                        else
                            lstCausas = null;
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            //try
            //{
            //    //cn.ConnectionString = MyConnectionString;
            //    cn.Open();

            //    cmd.CommandText = "NET_LOAD_CASAEI_CAUSASDEDEMORAPORGUIA";
            //    cmd.Connection = cn;
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.Add("@GUIA", SqlDbType.VarChar, 15).Value = Guia;

            //    dr = cmd.ExecuteReader();
            //    if (dr.HasRows)
            //    {
            //        while (dr.Read())
            //        {
            //            CausasDeDemoraPorGuia objCAUSASDEDEMORAPORGUIA = new();
            //            objCAUSASDEDEMORAPORGUIA.IdDemoraGuia = Convert.ToInt32(dr["IdDemoraGuia"]);
            //            objCAUSASDEDEMORAPORGUIA.Guia = dr["Guia"].ToString();
            //            objCAUSASDEDEMORAPORGUIA.IdCausa = Convert.ToInt32(dr["IdCausa"]);
            //            objCAUSASDEDEMORAPORGUIA.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
            //            objCAUSASDEDEMORAPORGUIA.FechaRegistro = dr["FechaRegistro"].ToString();
            //            objCAUSASDEDEMORAPORGUIA.IdDatosdeEmpresa = Convert.ToInt32(dr["IdDatosdeEmpresa"]);

            //            lstCausas.Add(objCAUSASDEDEMORAPORGUIA);
            //        }
            //    }
            //    else
            //        lstCausas = null;
            //    dr.Close();
            //    cn.Close();
            //    // SqlConnection.ClearPool(cn)
            //    cn.Dispose();
            //    cmd.Parameters.Clear();
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message.ToString());
            //}

            return lstCausas;
        }

        public async Task<int> Eliminar(EliminarCausaDeDemora objEliminaCausasDeDemora)
        {
            int id = 0;

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_DELETE_CASAEI_CAUSASDEDEMORAPORGUIA", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdDemoraGuia", SqlDbType.Int, 4).Value = objEliminaCausasDeDemora.IdDemoraGuia;
                    cmd.Parameters.Add("@Guia", SqlDbType.VarChar, 15).Value = objEliminaCausasDeDemora.Guia;
                    cmd.Parameters.Add("@Justificacion", SqlDbType.VarChar, 250).Value = objEliminaCausasDeDemora.Justificacion;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = objEliminaCausasDeDemora.IdUsuario;
                    //cmd.Parameters.Add("@IdDatosdeEmpresa", SqlDbType.Int, 4).Value = objEliminaCausasDeDemora.IdDatosdeEmpresa;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;


                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                            id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                        else
                            id = 0;

                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + "NET_DELETE_CASAEI_CAUSASDEDEMORAPORGUIA");
            }

            return id;
        }

    }
}

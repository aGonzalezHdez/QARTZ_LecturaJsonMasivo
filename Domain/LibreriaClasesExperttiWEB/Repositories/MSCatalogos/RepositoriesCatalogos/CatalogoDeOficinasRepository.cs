using LibreriaClasesAPIExpertti.Utilities.Converters;
using System.Data;
using System.Data.SqlClient;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos.Interfaces;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogoDeOficinasRepository : ICatalogoDeOficinasRepository
    {
        public string SConexion { get; set; }

        string ICatalogoDeOficinasRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IConfiguration _configuration;
        //public SqlConnection con;

        public CatalogoDeOficinasRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public CatalogoDeOficinas Buscar(int idOficina)
        {
            CatalogoDeOficinas objCatalogoDeOficinas = new();

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_CatalogoDeOficinas", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idOficina", SqlDbType.Int, 4).Value = idOficina;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objCatalogoDeOficinas.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                            objCatalogoDeOficinas.idAduana = dr["idAduana"].ToString();
                            objCatalogoDeOficinas.nombre = string.Format("{0}", dr["Nombre"]);
                            objCatalogoDeOficinas.Descripcion = string.Format("{0}", dr["Descripcion"]);
                            objCatalogoDeOficinas.LimiteDePagoElectronico = Convert.ToDecimal(dr["LimiteDePagoElectronico"]);
                            objCatalogoDeOficinas.PassMail = string.Format("{0}", dr["PassMail"]);
                            objCatalogoDeOficinas.AutomaticoConsol = Convert.ToInt32(dr["AutomaticoConsol"]);
                            objCatalogoDeOficinas.IDDatosDeEmpresa = Convert.ToInt32(dr["IDDatosDeEmpresa"]);
                            objCatalogoDeOficinas.GenImprimeManif = Convert.ToBoolean(dr["GenImprimeManif"]);
                            objCatalogoDeOficinas.GenImprimePed = Convert.ToBoolean(dr["GenImprimePed"]);
                            objCatalogoDeOficinas.PatenteDefault = string.Format("{0}", dr["PatenteDefault"]);
                            objCatalogoDeOficinas.AduDesp = string.Format("{0}", dr["AduDesp"]);
                            objCatalogoDeOficinas.AduEntr = string.Format("{0}", dr["AduEntr"]);
                            objCatalogoDeOficinas.DesOrig = Convert.ToInt32(dr["DesOrig"]);
                            objCatalogoDeOficinas.MtrEntrImp = Convert.ToInt32(dr["MtrEntrImp"]);
                            objCatalogoDeOficinas.MtrArriImp = Convert.ToInt32(dr["MtrArriImp"]);
                            objCatalogoDeOficinas.MtrSaliImp = Convert.ToInt32(dr["MtrSaliImp"]);
                            objCatalogoDeOficinas.MtrEntrExp = Convert.ToInt32(dr["MtrEntrExp"]);
                            objCatalogoDeOficinas.MtrArriExp = Convert.ToInt32(dr["MtrArriExp"]);
                            objCatalogoDeOficinas.MtrSaliExp = Convert.ToInt32(dr["MtrSaliExp"]);
                            objCatalogoDeOficinas.SecDesp = Convert.ToInt32(dr["SecDesp"]);
                            objCatalogoDeOficinas.CveMant = string.Format("{0}", dr["CveMant"]);
                            objCatalogoDeOficinas.CvePrev = string.Format("{0}", dr["CvePrev"]);
                            objCatalogoDeOficinas.EmpFac = string.Format("{0}", dr["EmpFac"]);
                            objCatalogoDeOficinas.OperacionDefault = Convert.ToInt32(dr["OperacionDefault"]);
                            objCatalogoDeOficinas.IdOficinaExtra = Convert.ToInt32(dr["IdOficinaExtra"]);
                            objCatalogoDeOficinas.UtilizaGP = Convert.ToBoolean(dr["UtilizaGP"]);
                            objCatalogoDeOficinas.DHL = Convert.ToBoolean(dr["DHL"]);
                            objCatalogoDeOficinas.UsarWSWec = Convert.ToBoolean(dr["UsarWSWec"]);
                            objCatalogoDeOficinas.ValidacionAutomatica = Convert.ToBoolean(dr["ValidacionAutomatica"]);
                            objCatalogoDeOficinas.ironPDF = Convert.ToBoolean(dr["ironPDF"]);
                            objCatalogoDeOficinas.ClienteMensajeria = Convert.ToInt32(dr["ClienteMensajeria"]);
                            objCatalogoDeOficinas.ClaveGateway = string.Format("{0}", dr["ClaveGateway"]);
                            objCatalogoDeOficinas.ValidaUltimaVersion = Convert.ToBoolean(dr["ValidaUltimaVersion"]);
                            objCatalogoDeOficinas.wsDODAOld = Convert.ToBoolean(dr["wsDODAOld"]);
                            objCatalogoDeOficinas.GTWFac = dr["GTWFac"].ToString();
                        }
                        else
                        {
                            objCatalogoDeOficinas = null!;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objCatalogoDeOficinas;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Carga catalogo de oficinas ambas empresas"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<DropDownListDatos> CargarOficina(int IdDatosDeEmpresa)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_CASAEI_LOAD_CATALOGODEOFICINAS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdDatosDeEmpresa", SqlDbType.Int, 4).Value = IdDatosDeEmpresa;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Caraga catalogo de oficinas de mensajeria"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<DropDownListDatos> CargarMensajeria(int IdDatosDeEmpresa, int IdUsario)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODEOFICINAS_FDX_WEB", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdDatosDeEmpresa", SqlDbType.Int, 4).Value = IdDatosDeEmpresa;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = IdUsario;
                    using SqlDataReader reader = cmd.ExecuteReader();
                    comboList = SqlDataReaderToDropDownList.DropDownList<DropDownListDatos>(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return comboList.ToList();
        }

        /// <summary>
        ///    
        /// </summary>
        /// <param name="Carga catalogo de oficinas que son operativas y no operativas"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<DropDownListDatos> CargarOficinas(bool Operativa)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_CASAEI_LOAD_CATALOGODEOFICINAS_OPERATIVA", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Operativa", SqlDbType.Bit).Value = Operativa;
                    using SqlDataReader reader = cmd.ExecuteReader();
                    comboList = SqlDataReaderToDropDownList.DropDownList<DropDownListDatos>(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return comboList.ToList();
        }

        /// <summary>
        ///    
        /// </summary>
        /// <param name="Carga catalogo de oficinas que son operativas por operación"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<DropDownListDatosString> CargarOficinasporOperacion()
        {
            List<DropDownListDatosString> comboList = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_OFICINAS_OPERATIVAS_OPERACION", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    using SqlDataReader reader = cmd.ExecuteReader();

                    comboList = comboList = SqlDataReaderToDropDownListString.DropDownListString<DropDownListDatosString>(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return comboList;
        }

        public List<CatalogoDeOficinas> CatalogoOficinasBitacoraFX()
        {
            List<CatalogoDeOficinas> ListCatalogoDeOficinas = new();

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODEOFICINAS_BITACORAFX_NEW", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                CatalogoDeOficinas objCatalogoDeOficinas = new CatalogoDeOficinas();
                                objCatalogoDeOficinas.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                                objCatalogoDeOficinas.idAduana = dr["idAduana"].ToString();
                                objCatalogoDeOficinas.nombre = string.Format("{0}", dr["Nombre"]);
                                objCatalogoDeOficinas.Descripcion = string.Format("{0}", dr["Descripcion"]);
                                objCatalogoDeOficinas.LimiteDePagoElectronico = Convert.ToDecimal(dr["LimiteDePagoElectronico"]);
                                objCatalogoDeOficinas.PassMail = string.Format("{0}", dr["PassMail"]);
                                objCatalogoDeOficinas.AutomaticoConsol = Convert.ToInt32(dr["AutomaticoConsol"]);
                                objCatalogoDeOficinas.IDDatosDeEmpresa = Convert.ToInt32(dr["IDDatosDeEmpresa"]);
                                objCatalogoDeOficinas.GenImprimeManif = Convert.ToBoolean(dr["GenImprimeManif"]);
                                objCatalogoDeOficinas.GenImprimePed = Convert.ToBoolean(dr["GenImprimePed"]);
                                objCatalogoDeOficinas.PatenteDefault = string.Format("{0}", dr["PatenteDefault"]);
                                objCatalogoDeOficinas.AduDesp = string.Format("{0}", dr["AduDesp"]);
                                objCatalogoDeOficinas.AduEntr = string.Format("{0}", dr["AduEntr"]);
                                objCatalogoDeOficinas.DesOrig = Convert.ToInt32(dr["DesOrig"]);
                                objCatalogoDeOficinas.MtrEntrImp = Convert.ToInt32(dr["MtrEntrImp"]);
                                objCatalogoDeOficinas.MtrArriImp = Convert.ToInt32(dr["MtrArriImp"]);
                                objCatalogoDeOficinas.MtrSaliImp = Convert.ToInt32(dr["MtrSaliImp"]);
                                objCatalogoDeOficinas.MtrEntrExp = Convert.ToInt32(dr["MtrEntrExp"]);
                                objCatalogoDeOficinas.MtrArriExp = Convert.ToInt32(dr["MtrArriExp"]);
                                objCatalogoDeOficinas.MtrSaliExp = Convert.ToInt32(dr["MtrSaliExp"]);
                                objCatalogoDeOficinas.SecDesp = Convert.ToInt32(dr["SecDesp"]);
                                objCatalogoDeOficinas.CveMant = string.Format("{0}", dr["CveMant"]);
                                objCatalogoDeOficinas.CvePrev = string.Format("{0}", dr["CvePrev"]);
                                objCatalogoDeOficinas.EmpFac = string.Format("{0}", dr["EmpFac"]);
                                objCatalogoDeOficinas.OperacionDefault = Convert.ToInt32(dr["OperacionDefault"]);
                                objCatalogoDeOficinas.IdOficinaExtra = Convert.ToInt32(dr["IdOficinaExtra"]);
                                objCatalogoDeOficinas.UtilizaGP = Convert.ToBoolean(dr["UtilizaGP"]);
                                objCatalogoDeOficinas.DHL = Convert.ToBoolean(dr["DHL"]);
                                objCatalogoDeOficinas.UsarWSWec = Convert.ToBoolean(dr["UsarWSWec"]);
                                objCatalogoDeOficinas.ValidacionAutomatica = Convert.ToBoolean(dr["ValidacionAutomatica"]);
                                objCatalogoDeOficinas.ironPDF = Convert.ToBoolean(dr["ironPDF"]);
                                objCatalogoDeOficinas.ClienteMensajeria = Convert.ToInt32(dr["ClienteMensajeria"]);
                                objCatalogoDeOficinas.ClaveGateway = string.Format("{0}", dr["ClaveGateway"]);
                                objCatalogoDeOficinas.ValidaUltimaVersion = Convert.ToBoolean(dr["ValidaUltimaVersion"]);
                                objCatalogoDeOficinas.wsDODAOld = Convert.ToBoolean(dr["wsDODAOld"]);

                                ListCatalogoDeOficinas.Add(objCatalogoDeOficinas);
                            }
                        }
                        
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + " NET_LOAD_CATALOGODEOFICINAS_BITACORAFX_NEW");
            }

            return ListCatalogoDeOficinas;
        }

    }
}

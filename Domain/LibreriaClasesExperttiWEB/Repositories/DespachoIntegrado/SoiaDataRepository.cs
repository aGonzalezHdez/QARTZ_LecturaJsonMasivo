using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Services.Prevalidador;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;

namespace LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado
{
    public class SoiaDataRepository
    {
        IConfiguration _configuration;
        public string SConexion { get; set; }

        public SoiaDataRepository(IConfiguration configuracion)
        {
            _configuration = configuracion;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<string> ReferenciasDoda(int idDoda)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();

            List<string> listado = new List<string>();
            SqlDataReader reader;
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_ENVIAR_ARCHIVOS_JCJF_FINAL_DESPUES_DEL_DODA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdDODA", idDoda);

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        listado.Add(reader["Referencia"].ToString());
                    }
                }

                cn.Close();
                cn.Dispose();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_ENVIAR_ARCHIVOS_JCJF_FINAL_DESPUES_DEL_DODA");
            }
            return listado;
        }
        public void EnviaPedimentosAWsJCJFDesdePago(string MisPedimentos, int DatosDeEmpresa)
        {
            using (SqlConnection cn = new SqlConnection(SConexion))
            using (SqlCommand cmd = new SqlCommand("NET_ENVIAR_ARCHIVOS_JCJF_FINAL_DESPUES_DEL_PAGO", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ArrayPedimentos", SqlDbType.VarChar, 8000).Value = MisPedimentos;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        var objApiD = new WebApisRepository(_configuration);
                        var objApi = objApiD.Buscar(19);

                        while (dr.Read())
                        {
                            var objRefeD = new ReferenciasRepository(_configuration);
                            var objRefe = objRefeD.Buscar(Convert.ToInt32(dr["idReferencia"]));

                            var objSolicitaSalida = new SolicitaSalidaService(_configuration);
                            objSolicitaSalida.fSolicitarSalida(Convert.ToInt32(dr["idReferencia"]), DatosDeEmpresa, objApi, objRefe);
                        }
                    }
                }
            }
        }

        public void EnviaPedimentosAJCJFDesdePago(string ArchivosAIFA, string MisPedimentos, int DatosDeEmpresa)
        {
            using (SqlConnection cn = new SqlConnection(SConexion))
            using (SqlCommand cmd = new SqlCommand("NET_ENVIAR_ARCHIVOS_JCJF_FINAL_DESPUES_DEL_PAGO", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ArrayPedimentos", SqlDbType.VarChar, 8000).Value = MisPedimentos;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        var objGpoCorreoD = new CatalogoGruposdeCorreoRepository(_configuration);
                        List<CatalogoDeGruposdeCorreo> lstGpoCorreo = objGpoCorreoD.Cargar(110);
                        var lstCorreos = lstGpoCorreo.Select(itemEmail => itemEmail.Email.Trim()).ToList();

                        while (dr.Read())
                        {
                            var ObjSaaiPedimeData = new SaaioPedimeRepository(_configuration);
                            var ObjSaaioPedime = ObjSaaiPedimeData.Buscar(dr["Referencia"].ToString());

                            if (ObjSaaioPedime != null)
                            {
                                string MiReferencia = ObjSaaioPedime.NUM_REFE;
                                string syear = ObjSaaioPedime.FEC_PAGO.Year.ToString();
                                string MiPedimento = syear.Substring(2, 2) + ObjSaaioPedime.ADU_DESP + ObjSaaioPedime.PAT_AGEN + ObjSaaioPedime.NUM_PEDI;

                                var iExp = new FuncionExcelSafranRepository(_configuration);
                                var dtb = iExp.CargaReporteJCJRPorPedimento(DatosDeEmpresa, MiReferencia);

                                if (dtb != null && dtb.Rows.Count > 0)
                                {
                                   // iExp.DataTableJCJRToExcel(dtb, MiPedimento, ArchivosAIFA, lstCorreos, MiReferencia, 1, 1);
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}

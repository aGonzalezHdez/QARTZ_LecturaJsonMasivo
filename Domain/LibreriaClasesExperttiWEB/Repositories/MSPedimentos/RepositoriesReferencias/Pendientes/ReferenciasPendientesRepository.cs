using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias.Pendientes;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias.Pendientes
{
    public class ReferenciasPendientesRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public ReferenciasPendientesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<ReferenciasPendientes> PendientesValidacion(pendientesValidacion objPendientes)
        {
            Referencias objReferencias = new();
            List<ReferenciasPendientes> listaPendientesValidar = new();

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_REFERENCIAS_POR_VALIDAR_USUARIO", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@FechaIni", SqlDbType.DateTime).Value = objPendientes.FechaIni;
                    cmd.Parameters.Add("@FechaFin", SqlDbType.DateTime).Value = objPendientes.FechaFin;
                    cmd.Parameters.Add("@ImpExpo", SqlDbType.Int, 4).Value = objPendientes.Operacion;
                    cmd.Parameters.Add("@PAT_AGEN", SqlDbType.VarChar, 4).Value = objPendientes.Patente;
                    cmd.Parameters.Add("@ADU_DESP", SqlDbType.VarChar, 3).Value = objPendientes.aduanaDespacho;
                    cmd.Parameters.Add("@CVE_REPR", SqlDbType.VarChar, 2).Value = objPendientes.Representante;
                    cmd.Parameters.Add("@IDUsuario", SqlDbType.Int, 4).Value = objPendientes.idUsuario;
                    cmd.Parameters.Add("@GLOBAL", SqlDbType.Bit).Value = objPendientes.Global;
                    cmd.Parameters.Add("@IDDatosDeEmpresa", SqlDbType.Int, 4).Value = objPendientes.IDDatosDeEmpresa;
                    cmd.Parameters.Add("@IdOficinaSelec", SqlDbType.Int, 4).Value = objPendientes.IdOficina;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                            ReferenciasPendientes objPendiente = new();
                            objPendiente.idReferencia = Convert.ToInt32(dr["idReferencia"]);
                            objPendiente.Referencia = dr["Referencia"].ToString();
                            objPendiente.Pedimento = dr["Pedimento"].ToString();
                            objPendiente.FechaPago = dr["FechaPago"].ToString() == "" ? null : Convert.ToDateTime(dr["FechaPago"]);
                            objPendiente.Operacion = Convert.ToInt32(dr["Operacion"]);
                            objPendiente.Clave = dr["Clave"].ToString();
                            objPendiente.CR = dr["CR"].ToString();
                            objPendiente.Firma = dr["Firma"].ToString();
                            objPendiente.Efectivo = Convert.ToDouble(dr["Efectivo"]);
                            objPendiente.Otros = Convert.ToDouble(dr["Otros"]);
                            objPendiente.Total = Convert.ToDouble(dr["Total"]);
                            objPendiente.Representante = dr["Representante"].ToString();
                            objPendiente.FirmaPago = dr["FirmaPago"].ToString();
                            objPendiente.Cliente = dr["Cliente"].ToString();


                            listaPendientesValidar.Add(objPendiente);
                        }


                    }
                    else
                        objReferencias = null;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return listaPendientesValidar;
        }
    }
}

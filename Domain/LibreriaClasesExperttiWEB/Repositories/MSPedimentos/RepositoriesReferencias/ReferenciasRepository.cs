using LibreriaClasesAPIExpertti.Utilities.Converters;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using System.Data;
using System.Data.SqlClient;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesTrazabilidad;
using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using wsAirBus;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias

{
    public class ReferenciasRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public ReferenciasRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public Referencias BuscarUltimaReferencia(string NumeroDeReferencia, int IDDatosDeEmpresa)
        {

            var objREFERENCIAS = new Referencias();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_NEW_SEARCH_REFERENCIAS_IDDatosDeEmpresa";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@NumeroDeReferencia", SqlDbType.VarChar, 15);
            @param.Value = NumeroDeReferencia;

            @param = cmd.Parameters.Add("@IDDatosDeEmpresa", SqlDbType.Int, 4);
            @param.Value = IDDatosDeEmpresa;

            // @IDDatosDeEmpresa

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objREFERENCIAS.IDReferencia = Convert.ToInt32(dr["IDReferencia"]);
                    objREFERENCIAS.NumeroDeReferencia = dr["NumeroDeReferencia"].ToString();
                    objREFERENCIAS.FechaApertura = Convert.ToDateTime(dr["FechaApertura"]);
                    objREFERENCIAS.AduanaEntrada = dr["AduanaEntrada"].ToString();
                    objREFERENCIAS.AduanaDespacho = dr["AduanaDespacho"].ToString();
                    objREFERENCIAS.Patente = dr["Patente"].ToString();
                    objREFERENCIAS.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                    objREFERENCIAS.Operacion = Convert.ToInt32(dr["Operacion"]);
                    objREFERENCIAS.IdDuenoDeLaReferencia = Convert.ToInt32(dr["IdDuenoDeLaReferencia"]);
                    objREFERENCIAS.Subdivision = Convert.ToBoolean(dr["Subdivision"]);
                    objREFERENCIAS.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                    objREFERENCIAS.PendientePorRectificar = Convert.ToBoolean(dr["PendientePorRectificar"]);
                    objREFERENCIAS.IdGrupo = Convert.ToInt32(dr["IDGrupo"]);
                    objREFERENCIAS.FraccionRojo = Convert.ToBoolean(dr["FraccionRojo"]);
                    objREFERENCIAS.IdMasterConsol = Convert.ToInt32(dr["IDMasterConsol"]);
                    objREFERENCIAS.ReferenciaEncriptada = dr["ReferenciaEncriptada"].ToString();
                    objREFERENCIAS.PDFExpediente = dr["PDFExpediente"].ToString();
                    objREFERENCIAS.PDFPedimento = dr["PDFPedimento"].ToString();
                    objREFERENCIAS.PDFPedimentoSipl = dr["PDFPedimentoSipl"].ToString();
                    objREFERENCIAS.PDFFactura = dr["PDFFactura"].ToString();
                    objREFERENCIAS.XMLFactura = dr["XMLFactura"].ToString();
                    objREFERENCIAS.RUTAPDF = dr["RUTAPDF"].ToString();
                    objREFERENCIAS.RUTASIMPLIFICADOS = dr["RUTASIMPLIFICADOS"].ToString();
                    objREFERENCIAS.RUTAEXPEDIENTE = dr["RUTAEXPEDIENTE"].ToString();
                    objREFERENCIAS.XMLCove = dr["XMLCove"].ToString();
                    objREFERENCIAS.LineaCierre = dr["LineaCierre"].ToString();
                    objREFERENCIAS.PedimentoGlobal = Convert.ToBoolean(dr["PedimentoGlobal"]);
                    objREFERENCIAS.DODA = dr["DODA"].ToString();
                    objREFERENCIAS.RutaDODA = dr["RutaDODA"].ToString();
                    objREFERENCIAS.RutaFisicaDODA = dr["RutaFisicaDODA"].ToString();
                    objREFERENCIAS.IDEscalacion = Convert.ToInt32(dr["IDEscalacion"]);
                    objREFERENCIAS.IDDatosDeEmpresa = Convert.ToInt32(dr["IDDatosDeEmpresa"]);
                    objREFERENCIAS.CapturaenCasa = Convert.ToBoolean(dr["CapturaenCasa"]);
                    objREFERENCIAS.ReferenciaDestinatario = dr["ReferenciaDestinatario"].ToString();
                    objREFERENCIAS.Prioridad = Convert.ToInt32(dr["prioridad"]);
                    objREFERENCIAS.InterOficina = Convert.ToBoolean(dr["InterOficinas"]);
                }
                else
                {
                    objREFERENCIAS = default;
                }
                dr.Close();
                // cn.Close()
                // SqlConnection.ClearPool(cn)
                // cn.Dispose()
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString());
            }
            cn.Close();
            cn.Dispose();
            return objREFERENCIAS;
        }

        public int BuscarDuplicados(string NumeroDeReferencia, int IDDatosDeEmpresa)
        {

            int result;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_REFERENCIAS_DUPLICADAS_IDDatosDeEmpresa";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@NumeroDeReferencia", SqlDbType.VarChar, 15);
            @param.Value = NumeroDeReferencia;

            @param = cmd.Parameters.Add("@IDDatosDeEmpresa", SqlDbType.Int, 4);
            @param.Value = IDDatosDeEmpresa;

            // @IDDatosDeEmpresa

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    result = Convert.ToInt32(dr["Count"]);
                }
                else
                {
                    result = 0;
                }
                dr.Close();
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + "NET_SEARCH_REFERENCIAS_DUPLICADAS_IDDatosDeEmpresa");
            }

            return result;
        }
        public int ModificarEscaladas(string NumReferencia, int Valor)
        {

            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            try
            {

                cn.ConnectionString = sConexion;
                cn.Open();
                cmd.CommandText = "NET_UPDATE_REFERENCIAS_ESCALACION";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                // ,@IDReferencia  int
                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = NumReferencia;

                // @IDEsclacion SMALLINT,
                @param = cmd.Parameters.Add("@IDEsclacion", SqlDbType.SmallInt, 4);
                @param.Value = Valor;

                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                {
                    id = (int)cmd.Parameters["@newid_registro"].Value;
                }
                else
                {
                    id = 0;
                }

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_REFERENCIAS_ESCALACION");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }


        //public Entities.Referencias.Referencias BuscarCEventos(string NumeroDeReferencia, int IDUsuario, int IDDatosDeEmpresa, int IDOficina)
        //{
        //    Entities.Referencias.Referencias objREFERENCIAS = new();
        //    try
        //    {

        //        using (con = new SqlConnection(sConexion))
        //        {
        //            con.Open();
        //            var cmd = new SqlCommand("NET_SEARCH_REFERENCIAS_CEventosExpClient", con)
        //            {
        //                CommandType = CommandType.StoredProcedure
        //            };
        //            cmd.Parameters.Add("@NumeroDeReferencia", SqlDbType.VarChar, 15).Value = NumeroDeReferencia;
        //            cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = IDUsuario;
        //            cmd.Parameters.Add("@IDDatosDeEmpresa", SqlDbType.Int, 4).Value = IDDatosDeEmpresa;
        //            cmd.Parameters.Add("@IDOficina", SqlDbType.Int, 4).Value = IDOficina;
        //            SqlDataReader dr = cmd.ExecuteReader();

        //            dr = cmd.ExecuteReader();
        //            if (dr.HasRows)
        //            {
        //                dr.Read();
        //                objREFERENCIAS.IDReferencia = Convert.ToInt32(dr["IDReferencia"]);
        //                objREFERENCIAS.NumeroDeReferencia = string.Format("{0}", dr["NumeroDeReferencia"]);
        //                objREFERENCIAS.FechaApertura = Convert.ToDateTime(dr["FechaApertura"]);
        //                objREFERENCIAS.AduanaEntrada = string.Format("{0}", dr["AduanaEntrada"]);
        //                objREFERENCIAS.AduanaDespacho = string.Format("{0}",  dr["AduanaDespacho"]);
        //                objREFERENCIAS.Patente = string.Format("{0}", dr["Patente"]);
        //                objREFERENCIAS.IDCliente = Convert.ToInt32(dr["IDCliente"]);
        //                objREFERENCIAS.Operacion = Convert.ToInt32(dr["Operacion"]);
        //                objREFERENCIAS.IdDuenoDeLaReferencia = Convert.ToInt32(dr["IdDuenoDeLaReferencia"]);
        //                objREFERENCIAS.Subdivision = Convert.ToBoolean(dr["Subdivision"]);
        //                objREFERENCIAS.IdOficina = Convert.ToInt32(dr["IdOficina"]);
        //                objREFERENCIAS.PendientePorRectificar = Convert.ToBoolean(dr["PendientePorRectificar"]);
        //                objREFERENCIAS.IDGrupo = Convert.ToInt32(dr["IDGrupo"]);
        //                objREFERENCIAS.FraccionRojo = Convert.ToBoolean(dr["FraccionRojo"]);
        //                objREFERENCIAS.IDMasterConsol = Convert.ToInt32(dr["IDMasterConsol"]);
        //                objREFERENCIAS.ReferenciaEncriptada = string.Format("{0}", dr["ReferenciaEncriptada"]);
        //                objREFERENCIAS.PDFExpediente = string.Format("{0}", dr["PDFExpediente"]);
        //                objREFERENCIAS.PDFPedimento = string.Format("{0}", dr["PDFPedimento"]);
        //                objREFERENCIAS.PDFPedimentoSipl = string.Format("{0}",  dr["PDFPedimentoSipl"]);
        //                objREFERENCIAS.PDFFactura = string.Format("{0}", dr["PDFFactura"]);
        //                objREFERENCIAS.XMLFactura = string.Format("{0}", dr["XMLFactura"]);
        //                objREFERENCIAS.RUTAPDF = string.Format("{0}", dr["RUTAPDF"]);
        //                objREFERENCIAS.RUTASIMPLIFICADOS = string.Format("{0}", dr["RUTASIMPLIFICADOS"]);
        //                objREFERENCIAS.RUTAEXPEDIENTE = string.Format("{0}",  dr["RUTAEXPEDIENTE"]);
        //                objREFERENCIAS.XMLCove = string.Format("{0}", dr["XMLCove"]);
        //                objREFERENCIAS.LineaCierre = string.Format("{0}", dr["LineaCierre"]);
        //                objREFERENCIAS.PedimentoGlobal = Convert.ToBoolean(dr["PedimentoGlobal"]);
        //                objREFERENCIAS.DODA = string.Format("{0}", dr["DODA"]);
        //                objREFERENCIAS.RutaDODA = string.Format("{0}", dr["RutaDODA"]);
        //                objREFERENCIAS.RutaFisicaDODA = string.Format("{0}", dr["RutaFisicaDODA"]);
        //                objREFERENCIAS.IDEscalacion = Convert.ToInt32(dr["IDEscalacion"]);
        //                objREFERENCIAS.IDDatosDeEmpresa = Convert.ToInt32(dr["IDDatosDeEmpresa"]);
        //                objREFERENCIAS.CapturaenCasa = Convert.ToBoolean(dr["CapturaenCasa"]);
        //                objREFERENCIAS.ReferenciaDestinatario = string.Format("{0}", dr["ReferenciaDestinatario"]);
        //                objREFERENCIAS.prioridad = Convert.ToInt32(dr["prioridad"]);
        //            }
        //            else
        //            con.Close();
        //            cmd.Parameters.Clear();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message.ToString());
        //    }
        //    return objREFERENCIAS;
        //}




        public Referencias Buscar(int IDReferencia, int IDDatosDeEmpresa)
        {
            Referencias objReferencias = new();

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_SEARCH_REFERENCIAS_ID_IDDatosDeEmpresa", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDReferencia", SqlDbType.Int, 4).Value = IDReferencia;
                    cmd.Parameters.Add("@IDDatosDeEmpresa", SqlDbType.Int, 4).Value = IDDatosDeEmpresa;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();

                        objReferencias.IDReferencia = Convert.ToInt32(dr["IDReferencia"]);
                        objReferencias.NumeroDeReferencia = dr["NumeroDeReferencia"].ToString();
                        objReferencias.FechaApertura = Convert.ToDateTime(dr["FechaApertura"]);
                        objReferencias.AduanaEntrada = dr["AduanaEntrada"].ToString();
                        objReferencias.AduanaDespacho = dr["AduanaDespacho"].ToString();
                        objReferencias.Patente = dr["Patente"].ToString();
                        objReferencias.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                        objReferencias.Operacion = Convert.ToInt32(dr["Operacion"]);
                        objReferencias.IdDuenoDeLaReferencia = Convert.ToInt32(dr["IdDuenoDeLaReferencia"]);
                        objReferencias.Subdivision = Convert.ToBoolean(dr["Subdivision"]);
                        objReferencias.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                        objReferencias.PendientePorRectificar = Convert.ToBoolean(dr["PendientePorRectificar"]);
                        //objReferencias.IdClienteDestinatario = Convert.ToInt32(dr["IdClienteDestinatario"]);
                        //objReferencias.ReferenciaDestinatario = dr["ReferenciaDestinatario"].ToString();
                        objReferencias.IdGrupo = Convert.ToInt32(dr["IdGrupo"]);
                        objReferencias.FraccionRojo = Convert.ToBoolean(dr["FraccionRojo"]);
                        objReferencias.IdMasterConsol = Convert.ToInt32(dr["IdMasterConsol"]);
                        objReferencias.ReferenciaEncriptada = dr["ReferenciaEncriptada"].ToString();

                        objReferencias.PDFExpediente = dr["PDFExpediente"].ToString();
                        objReferencias.PDFPedimento = dr["PDFPedimento"].ToString();
                        objReferencias.PDFPedimentoSipl = dr["PDFPedimentoSipl"].ToString();
                        objReferencias.PDFFactura = dr["PDFFactura"].ToString();
                        objReferencias.XMLFactura = dr["XMLFactura"].ToString();
                        objReferencias.RUTAPDF = dr["RUTAPDF"].ToString();
                        objReferencias.RUTASIMPLIFICADOS = dr["RUTASIMPLIFICADOS"].ToString();
                        objReferencias.RUTAEXPEDIENTE = dr["RUTAEXPEDIENTE"].ToString();
                        objReferencias.XMLCove = dr["XMLCove"].ToString();
                        objReferencias.LineaCierre = dr["LineaCierre"].ToString();
                        objReferencias.PedimentoGlobal = Convert.ToBoolean(dr["PedimentoGlobal"]);
                        objReferencias.DODA = dr["DODA"].ToString();
                        objReferencias.RutaDODA = dr["RutaDODA"].ToString();


                        //objReferencias.PaperLess = Convert.ToInt32(dr["PaperLess"]);
                        objReferencias.IDDatosDeEmpresa = IDDatosDeEmpresa;
                        objReferencias.IDEscalacion = Convert.ToInt32(dr["IDEscalacion"]);
                        //objReferencias.NoCuenta = dr["NoCuenta"].ToString();
                        //objReferencias.TipodeNotificacion = Convert.ToInt32(dr["TipodeNotificacion"]);
                        //objReferencias.Remesa = Convert.ToBoolean(dr["Remesa"]);
                        objReferencias.CapturaenCasa = Convert.ToBoolean(dr["CapturaenCasa"]);
                        objReferencias.ReferenciaDestinatario = dr["ReferenciaDestinatario"].ToString();
                        objReferencias.XMLCoveAnt = dr["XMLCoveAnt"].ToString();
                        objReferencias.S3 = Convert.ToInt32(dr["S3"]);
                        objReferencias.RUTAPDFS3PEDIMENTO = dr["RUTAS3PEDIMENTO"].ToString();
                        objReferencias.RUTAPDFS3PEDIMENTOSIMPLE = dr["RUTAS3PEDIMENTOSIMPLE"].ToString();
                        //objReferencias.IdEstacion = Convert.ToInt32(dr["IdEstacion"]);
                        //objReferencias.IdReferenciaMadre = Convert.ToInt32(dr["IdReferenciaMadre"]);
                        //objReferencias.NoRemesa = Convert.ToInt32(dr["NoRemesa"]);
                        //objReferencias.Prioridad = Convert.ToInt32(dr["Prioridad"]);
                        //objReferencias.InterOficinas = Convert.ToBoolean(dr["InterOficinas"]);
                        // objReferencias.Prefijo = dr["Prefijo"].ToString();
                    }
                    else
                        objReferencias = null;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objReferencias;
        }

        public Referencias Buscar(string NumeroDeReferencia, int IDDatosDeEmpresa)
        {
            Referencias objReferencias = new();

            try
            {

                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_SEARCH_REFERENCIAS_IDDatosDeEmpresa", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@NumeroDeReferencia", SqlDbType.VarChar, 15).Value = NumeroDeReferencia;
                    cmd.Parameters.Add("@IDDatosDeEmpresa", SqlDbType.Int, 4).Value = IDDatosDeEmpresa;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();

                        objReferencias.IDReferencia = Convert.ToInt32(dr["IDReferencia"]);
                        objReferencias.NumeroDeReferencia = dr["NumeroDeReferencia"].ToString();
                        objReferencias.FechaApertura = Convert.ToDateTime(dr["FechaApertura"]);
                        objReferencias.AduanaEntrada = dr["AduanaEntrada"].ToString();
                        objReferencias.AduanaDespacho = dr["AduanaDespacho"].ToString();
                        objReferencias.Patente = dr["Patente"].ToString();
                        objReferencias.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                        objReferencias.Operacion = Convert.ToInt32(dr["Operacion"]);
                        objReferencias.IdDuenoDeLaReferencia = Convert.ToInt32(dr["IdDuenoDeLaReferencia"]);
                        objReferencias.Subdivision = Convert.ToBoolean(dr["Subdivision"]);
                        objReferencias.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                        objReferencias.PendientePorRectificar = Convert.ToBoolean(dr["PendientePorRectificar"]);
                        objReferencias.IdGrupo = Convert.ToInt32(dr["IdGrupo"]);
                        objReferencias.FraccionRojo = Convert.ToBoolean(dr["FraccionRojo"]);
                        objReferencias.IdMasterConsol = Convert.ToInt32(dr["IdMasterConsol"]);
                        objReferencias.ReferenciaEncriptada = dr["ReferenciaEncriptada"].ToString();
                        objReferencias.PDFExpediente = dr["PDFExpediente"].ToString();
                        objReferencias.PDFPedimento = dr["PDFPedimento"].ToString();
                        objReferencias.PDFPedimentoSipl = dr["PDFPedimentoSipl"].ToString();
                        objReferencias.PDFFactura = dr["PDFFactura"].ToString();
                        objReferencias.XMLFactura = dr["XMLFactura"].ToString();
                        objReferencias.RUTAPDF = dr["RUTAPDF"].ToString();
                        objReferencias.RUTASIMPLIFICADOS = dr["RUTASIMPLIFICADOS"].ToString();
                        objReferencias.RUTAEXPEDIENTE = dr["RUTAEXPEDIENTE"].ToString();
                        objReferencias.XMLCove = dr["XMLCove"].ToString();
                        objReferencias.LineaCierre = dr["LineaCierre"].ToString();
                        objReferencias.PedimentoGlobal = Convert.ToBoolean(dr["PedimentoGlobal"]);
                        objReferencias.DODA = dr["DODA"].ToString();
                        objReferencias.RutaDODA = dr["RutaDODA"].ToString();
                        objReferencias.RutaFisicaDODA = dr["RutaDODA"].ToString();
                        objReferencias.IDEscalacion = Convert.ToInt32(dr["IDEscalacion"]);
                        objReferencias.IDDatosDeEmpresa = Convert.ToInt32(dr["IDDatosDeEmpresa"]);
                        objReferencias.CapturaenCasa = Convert.ToBoolean(dr["CapturaenCasa"]);
                        objReferencias.ReferenciaDestinatario = dr["ReferenciaDestinatario"].ToString();
                        objReferencias.Prioridad = Convert.ToInt32(dr["Prioridad"]);
                        objReferencias.InterOficinas = Convert.ToBoolean(dr["InterOficinas"]);
                        //objReferencias.IdClienteDestinatario = Convert.ToInt32(dr["IdClienteDestinatario"]);
                        //objReferencias.ReferenciaDestinatario = dr["ReferenciaDestinatario"].ToString();
                        //objReferencias.PaperLess = Convert.ToInt32(dr["PaperLess"]);
                        //objReferencias.NoCuenta = dr["NoCuenta"].ToString();
                        //objReferencias.TipodeNotificacion = Convert.ToInt32(dr["TipodeNotificacion"]);
                        //objReferencias.Remesa = Convert.ToBoolean(dr["Remesa"]);
                        //objReferencias.XMLCoveAnt = dr["XMLCoveAnt"].ToString();
                        //objReferencias.S3 = Convert.ToInt32(dr["S3"]);
                        //objReferencias.RUTAPDFS3PEDIMENTO = dr["RUTAPDFS3PEDIMENTO"].ToString();
                        //objReferencias.RUTAPDFS3PEDIMENTOSIMPLE = dr["RUTAPDFS3PEDIMENTOSIMPLE"].ToString();
                        //objReferencias.IdEstacion = Convert.ToInt32(dr["IdEstacion"]);
                        //objReferencias.IdReferenciaMadre = Convert.ToInt32(dr["IdReferenciaMadre"]);
                        //objReferencias.NoRemesa = Convert.ToInt32(dr["NoRemesa"]);
                        // objReferencias.Prefijo = dr["Prefijo"].ToString();
                    }
                    else
                        objReferencias = null;
                    dr.Close();
                    cn.Close();
                    // SqlConnection.ClearPool(cn)
                    cn.Dispose();
                    cmd.Parameters.Clear();
                }
                    

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objReferencias;
        }

        public int RegistarControlReferencias(int IdReferencia, int IdReferenciaDuplicada)
        {

            int result = 0;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();
                cmd.CommandText = "NET_INSERT_CONTROL_REFERENCIAS";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @IdReferencia varchar
                @param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int);
                @param.Value = IdReferencia;

                // @IdReferenciaDuplicada varchar
                @param = cmd.Parameters.Add("@IdReferenciaDuplicada", SqlDbType.Int);
                @param.Value = IdReferenciaDuplicada;

                cmd.ExecuteNonQuery();
                result = 1;
                cmd.Parameters.Clear();
            }

            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_CONTROL_REFERENCIAS");
            }
            cn.Close();
            cn.Dispose();
            return result;
        }

        public int Insertar(Referencias lReferencias, int IDDatosDeEmpresa)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();
                cmd.CommandText = "NET_INSERT_Referencias";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // ,@NumeroDeReferencia  varchar
                @param = cmd.Parameters.Add("@NumeroDeReferencia", SqlDbType.VarChar, 15);
                @param.Value = lReferencias.NumeroDeReferencia;


                // ,@AduanaEntrada  varchar
                @param = cmd.Parameters.Add("@AduanaEntrada", SqlDbType.VarChar, 3);
                @param.Value = lReferencias.AduanaEntrada;

                // ,@AduanaDespacho  varchar
                @param = cmd.Parameters.Add("@AduanaDespacho", SqlDbType.VarChar, 3);
                @param.Value = lReferencias.AduanaDespacho;

                // ,@Patente  varchar
                @param = cmd.Parameters.Add("@Patente", SqlDbType.VarChar, 4);
                @param.Value = lReferencias.Patente;

                // ,@IDCliente  int
                @param = cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4);
                @param.Value = lReferencias.IDCliente;

                // ,@Operacion  tinyint
                @param = cmd.Parameters.Add("@Operacion", SqlDbType.TinyInt, 4);
                @param.Value = lReferencias.Operacion;

                // ,@IdDuenoDeLaReferencia  int
                @param = cmd.Parameters.Add("@IdDuenoDeLaReferencia", SqlDbType.Int, 4);
                @param.Value = lReferencias.IdDuenoDeLaReferencia;

                // ,@Subdivision  bit
                @param = cmd.Parameters.Add("@Subdivision", SqlDbType.Bit, 4);
                @param.Value = lReferencias.Subdivision;

                // ,@IdOficina  int
                @param = cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4);
                @param.Value = lReferencias.IdOficina;

                // ,@PendientePorRectificar  bit
                @param = cmd.Parameters.Add("@PendientePorRectificar", SqlDbType.Bit, 4);
                @param.Value = lReferencias.PendientePorRectificar;

                // ,@IdClienteDestinatario int
                @param = cmd.Parameters.Add("@IdClienteDestinatario", SqlDbType.Int, 4);
                @param.Value = lReferencias.IdClienteDestinatario;

                // ,@ReferenciaDestinatario varchar(15)
                @param = cmd.Parameters.Add("@ReferenciaDestinatario", SqlDbType.VarChar, 15);
                @param.Value = lReferencias.ReferenciaDestinatario;


                // ,@IDGrupo  int
                @param = cmd.Parameters.Add("@IDGrupo", SqlDbType.Int, 4);
                @param.Value = lReferencias.IdGrupo;

                // ',@IDMasterConsol int
                // param = cmd.Parameters.Add("@IDMasterConsol", SqlDbType.Int, 4)
                // param.Value = lReferencias.IDMasterConsol

                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;


                cmd.ExecuteNonQuery();
                if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                {
                    id = (int)cmd.Parameters["@newid_registro"].Value;

                    if (id != 0)
                    {
                        var objrefe = new Referencias();
                        objrefe = Buscar(id, IDDatosDeEmpresa);
                        if (!(objrefe == null))
                        {
                            Modificar(objrefe);
                        }
                    }
                }
                else
                {
                    id = 0;
                }

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_Referencias");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }

        public int Insertar(Referencias objReferencias)
        {
            int id;

            try
            {

                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_INSERT_REFERENCIAS_NEW_2", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@NumeroDeReferencia", SqlDbType.VarChar, 15).Value = objReferencias.NumeroDeReferencia;
                    cmd.Parameters.Add("@AduanaEntrada", SqlDbType.VarChar, 3).Value = objReferencias.AduanaEntrada;
                    cmd.Parameters.Add("@AduanaDespacho", SqlDbType.VarChar, 3).Value = objReferencias.AduanaDespacho;
                    cmd.Parameters.Add("@Patente", SqlDbType.VarChar, 4).Value = objReferencias.Patente;
                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = objReferencias.IDCliente;
                    cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4).Value = objReferencias.Operacion;
                    cmd.Parameters.Add("@IdDuenoDeLaReferencia", SqlDbType.Int, 4).Value = objReferencias.IdDuenoDeLaReferencia;
                    cmd.Parameters.Add("@Subdivision", SqlDbType.VarChar, 20).Value = objReferencias.Subdivision;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = objReferencias.IdOficina;
                    cmd.Parameters.Add("@PendientePorRectificar", SqlDbType.Bit).Value = objReferencias.PendientePorRectificar;
                    cmd.Parameters.Add("@IdClienteDestinatario", SqlDbType.Int, 4).Value = objReferencias.IdClienteDestinatario;
                    cmd.Parameters.Add("@ReferenciaDestinatario", SqlDbType.VarChar, 15).Value = objReferencias.ReferenciaDestinatario;
                    cmd.Parameters.Add("@IdGrupo", SqlDbType.Int, 4).Value = objReferencias.IdGrupo;
                    cmd.Parameters.Add("@IdEstacion", SqlDbType.Int, 4).Value = objReferencias.IdEstacion;
                    cmd.Parameters.Add("@InterOficinas", SqlDbType.Int, 4).Value = objReferencias.InterOficinas;
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
        public bool NoValidada(string NumeroReferencia)
        {
            bool Sinvalidacion = false;
            try
            {
                SaaioPedime objPedime = new SaaioPedime();
                SaaioPedimeRepository objPedimeD = new SaaioPedimeRepository(_configuration);
                objPedime = objPedimeD.Buscar(NumeroReferencia);
                if (objPedime != null)
                {
                    if (objPedime.FIR_ELEC == "")
                        Sinvalidacion = true;
                    else
                        Sinvalidacion = false;
                }
                else
                    Sinvalidacion = true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return Sinvalidacion;
        }

        public async Task<string> AsignadaaDepartamentoAsync(int IdReferencia, int IdDepartamento)
        {
            string result = string.Empty;
            try
            {
                Referencias objReferencia = Buscar(IdReferencia);
                if (NoValidada(objReferencia.NumeroDeReferencia) == false)
                    return "Guia Validada";
                AsignaciondeGuias objAsig = new AsignaciondeGuias();
                AsignacionDeGuiasRepository objAsigD = new AsignacionDeGuiasRepository(_configuration);

                objAsig = objAsigD.BuscarUltimoDepartamento(IdReferencia);
                if (objAsig != null)
                {
                    if (objAsig.idDepartamento != IdDepartamento)
                    {
                        CatalogoDepartamentos objDep = new CatalogoDepartamentos();
                        CatalogoDepartamentosRepository objDepD = new CatalogoDepartamentosRepository(_configuration);
                        objDep = await objDepD.Buscar(objAsig.idDepartamento);
                        if (objDep == null)
                        {
                            return "Hubo un problema con el departamento asignado, favor de reportar al area de desarrollo";
                        }
                        return "La guia se encuentra asignada al departamento " + objDep.NombreDepartamento.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            return result;
        }

        public Referencias Buscar(int IDReferencia)
        {
            Referencias objREFERENCIAS = new Referencias();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;


            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_REFERENCIAS_Id";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("@IDReferencia", SqlDbType.Int, 4);
            param.Value = IDReferencia;
            // Insertar Parametro de busqueda

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    objREFERENCIAS.IDReferencia = Convert.ToInt32(dr["IDReferencia"]);
                    objREFERENCIAS.NumeroDeReferencia = dr["NumeroDeReferencia"].ToString();
                    objREFERENCIAS.FechaApertura = Convert.ToDateTime(dr["FechaApertura"]);
                    objREFERENCIAS.AduanaEntrada = dr["AduanaEntrada"].ToString();
                    objREFERENCIAS.AduanaDespacho = dr["AduanaDespacho"].ToString();
                    objREFERENCIAS.Patente = dr["Patente"].ToString();
                    objREFERENCIAS.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                    objREFERENCIAS.Operacion = Convert.ToInt32(dr["Operacion"]);
                    objREFERENCIAS.IdDuenoDeLaReferencia = Convert.ToInt32(dr["IdDuenoDeLaReferencia"]);
                    objREFERENCIAS.IDDatosDeEmpresa = Convert.ToInt32(dr["IDDatosDeEmpresa"]);
                    objREFERENCIAS.Subdivision = Convert.ToBoolean(dr["Subdivision"]);
                    objREFERENCIAS.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                    objREFERENCIAS.PendientePorRectificar = Convert.ToBoolean(dr["PendientePorRectificar"]);
                    objREFERENCIAS.IdGrupo = Convert.ToInt32(dr["IDGrupo"]);
                    objREFERENCIAS.FraccionRojo = Convert.ToBoolean(dr["FraccionRojo"]);
                    objREFERENCIAS.IdMasterConsol = Convert.ToInt32(dr["IDMasterConsol"]);
                    objREFERENCIAS.ReferenciaEncriptada = dr["ReferenciaEncriptada"].ToString();
                    objREFERENCIAS.PDFExpediente = dr["PDFExpediente"].ToString();
                    objREFERENCIAS.PDFPedimento = dr["PDFPedimento"].ToString();
                    objREFERENCIAS.PDFPedimentoSipl = dr["PDFPedimentoSipl"].ToString();
                    objREFERENCIAS.PDFFactura = dr["PDFFactura"].ToString();
                    objREFERENCIAS.XMLFactura = dr["XMLFactura"].ToString();
                    objREFERENCIAS.RUTAPDF = dr["RUTAPDF"].ToString();
                    objREFERENCIAS.RUTASIMPLIFICADOS = dr["RUTASIMPLIFICADOS"].ToString();
                    objREFERENCIAS.RUTAEXPEDIENTE = dr["RUTAEXPEDIENTE"].ToString();
                    objREFERENCIAS.XMLCove = dr["XMLCove"].ToString();
                    // objREFERENCIAS.XMLCove = dr["Carpeta").ToString
                    objREFERENCIAS.LineaCierre = dr["LineaCierre"].ToString();
                    objREFERENCIAS.PedimentoGlobal = Convert.ToBoolean(dr["PedimentoGlobal"]);
                    objREFERENCIAS.DODA = dr["DODA"].ToString();
                    objREFERENCIAS.RutaDODA = dr["RutaDODA"].ToString();
                    objREFERENCIAS.IDEscalacion = Convert.ToInt32(dr["IDEscalacion"]);
                    objREFERENCIAS.S3 = Convert.ToInt32(dr["S3"]);
                    objREFERENCIAS.RUTAPDFS3PEDIMENTO = dr["RUTAS3PEDIMENTO"].ToString();
                    objREFERENCIAS.RUTAPDFS3PEDIMENTOSIMPLE = dr["RUTAS3PEDIMENTOSIMPLE"].ToString();
                }
                else
                    objREFERENCIAS = null/* TODO Change to default(_) if this is not a reference type */;
                dr.Close();
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objREFERENCIAS;
        }

        public int Modificar(Referencias lreferencias)
        {
            int id = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();
                cmd.CommandText = "NET_UPDATE_Referencias_2";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                param = cmd.Parameters.Add("@IDReferencia", SqlDbType.Int);
                param.Value = lreferencias.IDReferencia;

                param = cmd.Parameters.Add("@AduanaEntrada", SqlDbType.VarChar, 3);
                param.Value = lreferencias.AduanaEntrada;

                param = cmd.Parameters.Add("@AduanaDespacho", SqlDbType.VarChar, 3);
                param.Value = lreferencias.AduanaDespacho;

                param = cmd.Parameters.Add("@Patente", SqlDbType.VarChar, 4);
                param.Value = lreferencias.Patente;

                param = cmd.Parameters.Add("@IDCliente", SqlDbType.Int);
                param.Value = lreferencias.IDCliente;

                param = cmd.Parameters.Add("@Operacion", SqlDbType.TinyInt);
                param.Value = lreferencias.Operacion;

                param = cmd.Parameters.Add("@IdDuenoDeLaReferencia", SqlDbType.Int);
                param.Value = lreferencias.IdDuenoDeLaReferencia;

                param = cmd.Parameters.Add("@Subdivision", SqlDbType.Bit);
                param.Value = lreferencias.Subdivision;

                param = cmd.Parameters.Add("@IdOficina", SqlDbType.Int);
                param.Value = lreferencias.IdOficina;

                param = cmd.Parameters.Add("@PendientePorRectificar", SqlDbType.Bit);
                param.Value = lreferencias.PendientePorRectificar;

                param = cmd.Parameters.Add("@IdClienteDestinatario", SqlDbType.Int);
                param.Value = lreferencias.IdClienteDestinatario;

                param = cmd.Parameters.Add("@IDGrupo", SqlDbType.Int);
                param.Value = lreferencias.IdGrupo;

                param = cmd.Parameters.Add("@IDMasterConsol", SqlDbType.Int);
                param.Value = lreferencias.IdMasterConsol;

                string CadenaAEncriptar = lreferencias.NumeroDeReferencia.Trim() + lreferencias.IDReferencia.ToString();
                lreferencias.ReferenciaEncriptada = Cryptografia.MD5Hash(CadenaAEncriptar);

                param = cmd.Parameters.Add("@ReferenciaEncriptada", SqlDbType.VarChar, 32);
                param.Value = lreferencias.ReferenciaEncriptada;

                param = cmd.Parameters.Add("@interoficinas", SqlDbType.Int);
                param.Value = lreferencias.InterOficina;

                param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                {
                    id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                }
                else
                {
                    id = 0;
                }

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_Referencias");
            }
            finally
            {
                cn.Close();
                cn.Dispose();
            }

            return id;

        }

        public DataTable ValidaEmpresaOfnaPorPedimRef(string MiPedimento, string MiReferencia)
        {
            var dtb = new DataTable();
            SqlParameter @param;
            try
            {
                using (var cn = new SqlConnection(sConexion))
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_SEARCH_VALIDA_PEDIREF_EMPRESAOFNA";


                    @param = dap.SelectCommand.Parameters.Add("@PEDIMENTO", SqlDbType.VarChar, 30);
                    @param.Value = MiPedimento;

                    @param = dap.SelectCommand.Parameters.Add("@REFERENCIA", SqlDbType.VarChar, 15);
                    @param.Value = MiReferencia;

                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;

                    dap.Fill(dtb);

                    cn.Close();

                    cn.Dispose();

                }
            }



            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dtb;
        }

        public Referencias Buscar(string NumeroDeReferencia)
        {
            Referencias objReferencias = new();
            try
            {
                using var con = new SqlConnection(sConexion);
                using SqlCommand cmd = new("NET_SEARCH_REFERENCIAS", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@NumeroDeReferencia", SqlDbType.VarChar, 15).Value = NumeroDeReferencia;

                using SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    objReferencias.IDReferencia = Convert.ToInt32(dr["IDReferencia"]);
                    objReferencias.NumeroDeReferencia = dr["NumeroDeReferencia"].ToString();
                    objReferencias.FechaApertura = Convert.ToDateTime(dr["FechaApertura"]);
                    objReferencias.AduanaEntrada = dr["AduanaEntrada"].ToString();
                    objReferencias.AduanaDespacho = dr["AduanaDespacho"].ToString();
                    objReferencias.Patente = dr["Patente"].ToString();
                    objReferencias.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                    objReferencias.Operacion = Convert.ToInt32(dr["Operacion"]);
                    objReferencias.IdDuenoDeLaReferencia = Convert.ToInt32(dr["IdDuenoDeLaReferencia"]);
                    objReferencias.Subdivision = Convert.ToBoolean(dr["Subdivision"]);
                    objReferencias.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                    objReferencias.PendientePorRectificar = Convert.ToBoolean(dr["PendientePorRectificar"]);
                    //objReferencias.IdClienteDestinatario = Convert.ToInt32(dr["IdClienteDestinatario"]);
                    //objReferencias.ReferenciaDestinatario = dr["ReferenciaDestinatario"].ToString();
                    objReferencias.IdGrupo = Convert.ToInt32(dr["IdGrupo"]);
                    objReferencias.FraccionRojo = Convert.ToBoolean(dr["FraccionRojo"]);
                    objReferencias.IdMasterConsol = Convert.ToInt32(dr["IdMasterConsol"]);
                    objReferencias.ReferenciaEncriptada = dr["ReferenciaEncriptada"].ToString();

                    objReferencias.PDFExpediente = dr["PDFExpediente"].ToString();
                    objReferencias.PDFPedimento = dr["PDFPedimento"].ToString();
                    objReferencias.PDFPedimentoSipl = dr["PDFPedimentoSipl"].ToString();
                    objReferencias.PDFFactura = dr["PDFFactura"].ToString();
                    objReferencias.XMLFactura = dr["XMLFactura"].ToString();
                    objReferencias.RUTAPDF = dr["RUTAPDF"].ToString();
                    objReferencias.RUTASIMPLIFICADOS = dr["RUTASIMPLIFICADOS"].ToString();
                    objReferencias.RUTAEXPEDIENTE = dr["RUTAEXPEDIENTE"].ToString();
                    objReferencias.XMLCove = dr["XMLCove"].ToString();
                    objReferencias.LineaCierre = dr["LineaCierre"].ToString();
                    objReferencias.PedimentoGlobal = Convert.ToBoolean(dr["PedimentoGlobal"]);
                    objReferencias.DODA = dr["DODA"].ToString();
                    objReferencias.RutaDODA = dr["RutaDODA"].ToString();


                    //objReferencias.PaperLess = Convert.ToInt32(dr["PaperLess"]);
                    objReferencias.IDDatosDeEmpresa = Convert.ToInt32(dr["IDDatosDeEmpresa"]); 
                    objReferencias.IDEscalacion = Convert.ToInt32(dr["IDEscalacion"]);
                    //objReferencias.NoCuenta = dr["NoCuenta"].ToString();
                    //objReferencias.TipodeNotificacion = Convert.ToInt32(dr["TipodeNotificacion"]);
                    //objReferencias.Remesa = Convert.ToBoolean(dr["Remesa"]);
                    objReferencias.CapturaenCasa = Convert.ToBoolean(dr["CapturaenCasa"]);
                    objReferencias.ReferenciaDestinatario = dr["ReferenciaDestinatario"].ToString();
                    //objReferencias.XMLCoveAnt = dr["XMLCoveAnt"].ToString();
                    objReferencias.S3 = Convert.ToInt32(dr["S3"]);
                }
                else
                {
                    objReferencias = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objReferencias;
        }

        public List<SaaioGuias> CargarGuiasPorReferencia(string NumeroDeReferencia)
        {
            List<SaaioGuias> lstSaaioGuias = new();
            SqlConnection con;
            try
            {
                using (con = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_SAAIO_GUIAS_TODAS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar).Value = NumeroDeReferencia;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        lstSaaioGuias = SqlDataReaderToList.DataReaderMapToList<SaaioGuias>(reader);
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
            return lstSaaioGuias;
        }

        public int Modificar(AsignarCliente objAsignarCliente)
        {
            int id;

            try
            {
                using (SqlConnection con = new(sConexion))
                using (var cmd = new SqlCommand("NET_UPDATE_CLIENTE_EN_REFERENCIAS_Y_SAAIO_PEDIME", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4).Value = objAsignarCliente.IDCliente;
                    cmd.Parameters.Add("@NumeroDeReferencia", SqlDbType.VarChar, 15).Value = objAsignarCliente.NumeroDeReferencia;
                    cmd.Parameters.Add("@Clave", SqlDbType.VarChar, 6).Value = objAsignarCliente.Clave;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);

                    cmd.Parameters.Clear();
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
    }
}

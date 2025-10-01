using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales;
using LibreriaClasesAPIExpertti.Entities.EntitiesControlSalidas;
using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas
{
    public class GuiasqueNoseDebenLiberarRepository
    {
        public string Mensaje = string.Empty;
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public GuiasqueNoseDebenLiberarRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public bool GenerarArchivoITCE(int MiIdNoLiberar, string MiEstatus, int MiIdusuario, string MyUbicacionDeArchivos)
        {
            bool GenerarArchivoITCERet = default;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;
            string Separador = "|";
            int i = 1;
            string FileName = "";
            string FilePath = "";
            var objHelp = new Helper();
            var lstError = new List<string>();
            try
            {
                cmd.CommandText = "NET_GENERA_TXT_PARA_BLOQUEAR_GUIA_EN_ITCE_NEW";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                // @IdNoLiberar int 
                @param = cmd.Parameters.Add("@IdNoLiberar", SqlDbType.Int, 4);
                @param.Value = MiIdNoLiberar;

                // ,@Estatus CHAR(1)
                @param = cmd.Parameters.Add("@Estatus", SqlDbType.Char, 1);
                @param.Value = MiEstatus;

                // ,@IdUsuario INT 
                @param = cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4);
                @param.Value = MiIdusuario;

                cn.ConnectionString = SConexion;
                cn.Open();

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {

                        Stream strStreamW;
                        StreamWriter strStreamWriter;

                        try
                        {
                            FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                            FilePath = MyUbicacionDeArchivos + FileName;

                            // Se abre el archivo y si este no existe se crea
                            strStreamW = File.OpenWrite(FilePath);

                            strStreamWriter = new StreamWriter(strStreamW, Encoding.ASCII);
                            strStreamWriter.Write(Strings.Trim(dr["CADENA"].ToString()));
                            strStreamWriter.Close();
                            strStreamWriter.Dispose();

                            var DFacturas = new FacturasRepository(_configuration);
                            DFacturas.GeneraLineaDeComandosEnvioDesftp("sftpPrevAQuitana.bat", FilePath);

                            GenerarArchivoITCERet = true;
                        }
                        catch (Exception ex)
                        {
                            lstError.Add(FilePath);
                            GenerarArchivoITCERet = false;
                        }
                    }
                }
                dr.Close();
                cn.Close();

                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return GenerarArchivoITCERet;
        }
        public int NoLiberar(string GuiaHouse, int iddepartamento, int idOficina)
        {
            int Nodebeliberar = 0;
            List<GuiaQueNoseDebeLiberar> lstGuiasQueNo = new List<GuiaQueNoseDebeLiberar>();

            lstGuiasQueNo = CargarporReferencia(GuiaHouse.Trim(), iddepartamento, idOficina, SConexion);


            foreach (GuiaQueNoseDebeLiberar item in lstGuiasQueNo)
            {
                if (idOficina == 2)
                {
                    if (item.IdTipoErrorFiltro > 0)
                        Nodebeliberar = item.IdTipoErrorFiltro;
                    else if (item.IdTipoError == 2 | item.IdTipoError == 9)
                        Nodebeliberar = 2;
                    else
                    {
                        if (item.TodaslasAreas == true)
                        {
                            Nodebeliberar = 4;
                            Mensaje = item.NumeroDeGuia + " Detenida en todas las areas " + item.Motivo.Trim();
                            break;
                        }

                        if (Nodebeliberar < 1)
                            Nodebeliberar = 1;
                    }
                }
                else
                {
                    if (item.TodaslasAreas == true)
                    {
                        Nodebeliberar = 4;
                        Mensaje = item.NumeroDeGuia + " Detenida en todas las areas " + item.Motivo.Trim();
                        break;
                    }

                    if (Nodebeliberar < 1)
                        Nodebeliberar = 1;
                }
                Mensaje += item.NumeroDeGuia + ".-" + item.Motivo.Trim() + "\r\n";
            }

            // If Nodebeliberar = 1 Then
            // Throw New ArgumentException("Guia que no se debe liberar :" & Mensaje.Trim)
            // End If


            return Nodebeliberar;
        }
        public List<GuiaQueNoseDebeLiberar> CargarporReferencia(string GuiaHouse, int idDepartamento, int idoficina, string MyConnectionString)
        {
            List<GuiaQueNoseDebeLiberar> lst = new List<GuiaQueNoseDebeLiberar>();

            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;

            try
            {
                cn.ConnectionString = MyConnectionString;
                cn.Open();

                cmd.CommandText = "NET_SEARCH_GUIASQUENOSEDEBENLIBERARPORGUIA_DEPARTAMENTOXGUIA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                param = cmd.Parameters.Add("@NumerodeReferencia", SqlDbType.Char, 15);
                param.Value = GuiaHouse;

                param = cmd.Parameters.Add("@idDepartamento", SqlDbType.Int, 4);
                param.Value = idDepartamento;

                // @idoficina 
                param = cmd.Parameters.Add("@idoficina ", SqlDbType.Int, 4);
                param.Value = idoficina;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        GuiaQueNoseDebeLiberar objGUIASQUENOSEDEBENLIBERAR = new GuiaQueNoseDebeLiberar();

                        objGUIASQUENOSEDEBENLIBERAR.IDNoLiberar = Convert.ToInt32(dr["IDNoLiberar"]);
                        objGUIASQUENOSEDEBENLIBERAR.NumeroDeGuia = dr["NumeroDeGuia"].ToString();
                        objGUIASQUENOSEDEBENLIBERAR.Motivo = dr["Motivo"].ToString();
                        objGUIASQUENOSEDEBENLIBERAR.FechaCaptura = Convert.ToDateTime(dr["FechaCaptura"]);
                        objGUIASQUENOSEDEBENLIBERAR.FechaBaja = Convert.ToDateTime(dr["FechaBaja"]);
                        objGUIASQUENOSEDEBENLIBERAR.Activa = Convert.ToBoolean(dr["Activa"]);
                        objGUIASQUENOSEDEBENLIBERAR.TodaslasAreas = Convert.ToBoolean(dr["TodaslasAreas"]);
                        objGUIASQUENOSEDEBENLIBERAR.IDDepartamento = Convert.ToInt32(dr["IDDepartamento"]);
                        objGUIASQUENOSEDEBENLIBERAR.IdUsuarioAlta = Convert.ToInt32(dr["IdUsuarioAlta"]);
                        objGUIASQUENOSEDEBENLIBERAR.IDUsuarioBaja = Convert.ToInt32(dr["IDUsuarioBaja"]);
                        objGUIASQUENOSEDEBENLIBERAR.Posicion = dr["Posicion"].ToString();
                        objGUIASQUENOSEDEBENLIBERAR.IdTipoError = Convert.ToInt32(dr["IdTipoError"]);
                        objGUIASQUENOSEDEBENLIBERAR.IdTipoErrorFiltro = Convert.ToInt32(dr["IdTipoErrorFiltro"]);

                        lst.Add(objGUIASQUENOSEDEBENLIBERAR);
                    }
                }
                else
                    lst.Clear();
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

            return lst;
        }

        public GuiasQueNoseDebenLiberar Buscar(string MyGuiaHouse)
        {

            var objGUIASQUENOSEDEBENLIBERAR = new GuiasQueNoseDebenLiberar();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_SEARCH_GUIASQUENOSEDEBENLIBERARPORGUIA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@NumeroDeGuia", SqlDbType.Char, 13);
                @param.Value = MyGuiaHouse;


                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objGUIASQUENOSEDEBENLIBERAR.IDNoLiberar = Convert.ToInt32(dr["IDNoLiberar"]);
                    objGUIASQUENOSEDEBENLIBERAR.NumeroDeGuia = dr["NumeroDeGuia"].ToString();
                    objGUIASQUENOSEDEBENLIBERAR.Motivo = dr["Motivo"].ToString();
                    objGUIASQUENOSEDEBENLIBERAR.FechaCaptura = Convert.ToDateTime(dr["FechaCaptura"]);
                    objGUIASQUENOSEDEBENLIBERAR.FechaBaja = Convert.ToDateTime(dr["FechaBaja"]);
                    objGUIASQUENOSEDEBENLIBERAR.Activa = Convert.ToBoolean(dr["Activa"]);
                    objGUIASQUENOSEDEBENLIBERAR.TodaslasAreas = Convert.ToBoolean(dr["TodaslasAreas"]);
                    objGUIASQUENOSEDEBENLIBERAR.IDDepartamento = Convert.ToInt32(dr["IDDepartamento"]);
                    objGUIASQUENOSEDEBENLIBERAR.IdUsuarioAlta = Convert.ToInt32(dr["IdUsuarioAlta"]);
                    objGUIASQUENOSEDEBENLIBERAR.IDUsuarioBaja = Convert.ToInt32(dr["IDUsuarioBaja"]);
                    objGUIASQUENOSEDEBENLIBERAR.Posicion = dr["Posicion"].ToString();
                    objGUIASQUENOSEDEBENLIBERAR.IdTipoError = Convert.ToInt32(dr["IdTipoError"]);
                }
                else
                {
                    objGUIASQUENOSEDEBENLIBERAR = default;
                }
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

            return objGUIASQUENOSEDEBENLIBERAR;
        }
        public int Insertar(GuiasQueNoseDebenLiberar lguiasquenosedebenliberar)
        {

            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_INSERT_GuiasQueNoseDebenLiberar";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            // ,@NumeroDeGuia  varchar
            @param = cmd.Parameters.Add("@NumeroDeGuia", SqlDbType.VarChar, 13);
            @param.Value = lguiasquenosedebenliberar.NumeroDeGuia;

            // ,@Motivo  varchar
            @param = cmd.Parameters.Add("@Motivo", SqlDbType.VarChar, 500);
            @param.Value = lguiasquenosedebenliberar.Motivo;


            // ,@TodasLasAreas  bit
            @param = cmd.Parameters.Add("@TodasLasAreas", SqlDbType.Bit, 4);
            @param.Value = lguiasquenosedebenliberar.TodaslasAreas;

            // ,@IDDepartamento  int
            @param = cmd.Parameters.Add("@IDDepartamento", SqlDbType.Int, 4);
            @param.Value = lguiasquenosedebenliberar.IDDepartamento;

            // ,@IdUsuarioAlta  int
            @param = cmd.Parameters.Add("@IdUsuarioAlta", SqlDbType.Int, 4);
            @param.Value = lguiasquenosedebenliberar.IdUsuarioAlta;


            // ,@Posicion  varchar
            @param = cmd.Parameters.Add("@Posicion", SqlDbType.VarChar, 50);
            @param.Value = lguiasquenosedebenliberar.Posicion;


            @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            @param.Direction = ParameterDirection.Output;


            try
            {
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
                throw new Exception(ex.Message.ToString() + " NET_INSERT_GuiasQueNoseDebenLiberar");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }

    }
}

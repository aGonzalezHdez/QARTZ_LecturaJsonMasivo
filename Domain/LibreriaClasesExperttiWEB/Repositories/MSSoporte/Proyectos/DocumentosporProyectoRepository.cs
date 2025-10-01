using LibreriaClasesAPIExpertti.Entities.EntitiesProyectos;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula.Functions;
using SixLabors.ImageSharp.Drawing;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSSoporte.Proyectos
{
    public class DocumentosporProyectoRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        public DocumentosporProyectoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int Insertar(DocumentosporProyecto objDocumentosporProyecto)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_INSERT_CASAEI_DocumentosporProyecto";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

  
                @param = cmd.Parameters.Add("@IdProyecto", SqlDbType.SmallInt);
                @param.Value = objDocumentosporProyecto.IdProyecto;

                @param = cmd.Parameters.Add("@consecutivo", SqlDbType.SmallInt);
                @param.Value = objDocumentosporProyecto.consecutivo;

                @param = cmd.Parameters.Add("@RutaS3", SqlDbType.VarChar, 500);
                @param.Value = objDocumentosporProyecto.RutaS3;

                @param = cmd.Parameters.Add("@Tipo", SqlDbType.VarChar, 150);
                @param.Value = objDocumentosporProyecto.Tipo;

                @param = cmd.Parameters.Add("@IdUsuario", SqlDbType.SmallInt);
                @param.Value = objDocumentosporProyecto.IdUsuario;

                @param = cmd.Parameters.Add("@idTipo", SqlDbType.SmallInt);
                @param.Value = objDocumentosporProyecto.idTipo;

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
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_CASAEI_DocumentosporProyecto");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }

        public List<DocumentosporProyectoLista> Cargar(int IdProyecto, int? IdTipo)
        {
            List<DocumentosporProyectoLista> lst = new();
            var objREFERENCIAS = new Referencias();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_DocumentosporProyecto";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@IdProyecto", SqlDbType.Int, 4);
            @param.Value = IdProyecto;

            @param = cmd.Parameters.Add("@IdTipo", SqlDbType.Int, 4);
            @param.Value = IdTipo;

 
            // @IDDatosDeEmpresa
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        DocumentosporProyectoLista obj= new();
                        obj.Documento = (dr["Documento"]).ToString();
                        obj.TipoDocumento = (dr["TipoDocumento"]).ToString();
                        obj.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);
                        obj.Nombre = (dr["Nombre"]).ToString();
                        obj.RutaS3 = (dr["RutaS3"]).ToString();

                        lst.Add(obj);
                    }


                }
                else
                {
                    lst.Clear();
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
            return lst;
        }

        public int Consecutivo(int IdProyecto)
        {
            int vConsecutivo = 0;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CASAEI_DocumentosporProyecto";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@IdProyecto", SqlDbType.Int, 4);
            @param.Value = IdProyecto;



            // @IDDatosDeEmpresa
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    vConsecutivo = Convert.ToInt32(dr["Consecutivo"]);
                 
                }
                else
                {
                    vConsecutivo = 0;
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
            return vConsecutivo;
        }

        public List<DropDownListDatos> CargarTiposdeDocumentos()
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (con = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODEDOCUMENTOSPROYECTOS", con))
                {
                    con.Open();
               

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

        public bool Agregar(DocumentosporProyectosExt obj )
        {
            bool EnS3= false;
            UbicaciondeArchivos objUbi = new();
            UbicaciondeArchivosRepository objUbiD = new(_configuration);
            objUbi = objUbiD.Buscar(237);
            
            int Cons = Consecutivo(obj.IDProyecto);
            string RutaFisica = objUbi.Ubicacion.Trim() +obj.NombreArch + "_" + Cons.ToString() + obj.Extension.Trim();


            try
            {
                ActividadesdeDesarrollo objProy = new();
                ActividadesdeDesarrolloRepository objProyD = new(_configuration);

                objProy = objProyD.Buscar(obj.IDProyecto);

            

                string RutaS3=string.Empty;
                RutaS3 = Convert.ToInt32(objProy.FechaSolicitud.ToString("yyyy")).ToString("0000") + "/" +
                        Convert.ToInt32(objProy.FechaSolicitud.ToString("MM")).ToString("00")+ "/" +
                        Convert.ToInt32(objProy.FechaSolicitud.ToString("dd")).ToString("00") + "/" +
                    objProy.IdProyecto.ToString() + "/" + obj.NombreArch + "_" + Cons.ToString() + obj.Extension;

                DocumentosporProyecto objDoc = new();
                objDoc.IdProyecto = obj.IDProyecto;
                objDoc.Tipo = obj.NombreArch;
                objDoc.RutaS3 = RutaS3;
                objDoc.consecutivo = Cons;
                objDoc.IdUsuario =  obj.IdUsuario;
                objDoc.idTipo = obj.idTipo;

              
                File.WriteAllBytes(RutaFisica, Convert.FromBase64String(obj.ArchivoBase64));

                string Subio = string.Empty;
                BucketsS3Repository objS3 = new BucketsS3Repository(_configuration);
                Subio= objS3.SubirObjeto(RutaFisica, "grupoei.proyectos", RutaS3);

                if (Subio.ToUpper()=="OK")
                {

                    Insertar(objDoc);
                    EnS3 = true;
                    
                }
                File.Delete(RutaFisica);

            }
            catch (Exception)
            {
                EnS3 = false;
                File.Delete(RutaFisica);
                throw;
            }
            
            return EnS3;
        }
    }
}

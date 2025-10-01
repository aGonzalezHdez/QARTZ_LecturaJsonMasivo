using LibreriaClasesAPIExpertti.Entities.EntitiesDocumentosPorGuia;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;



namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesDocumentosPorGuia
{
    public class CatalogodeTiposDeDocumentosRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con = null!;

        public CatalogodeTiposDeDocumentosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public CatalogodeTiposDeDocumentos Buscar(int IdTipoDocumento)
        {
            var objCatalogodeTiposDeDocumentos = new CatalogodeTiposDeDocumentos();

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlDataReader dr;
            SqlParameter param;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CATALOGODETIPOSDEDOCUMENTOS";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("@IdTipoDocumento", SqlDbType.Int, 4);
            param.Value = IdTipoDocumento;


            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        objCatalogodeTiposDeDocumentos.IdTipoDocumento = Convert.ToInt32(dr["IdTipoDocumento"]);
                        objCatalogodeTiposDeDocumentos.TipodeDocumento = dr["TipodeDocumento"].ToString();
                        objCatalogodeTiposDeDocumentos.Identificador = dr["Identificador"].ToString();
                        objCatalogodeTiposDeDocumentos.Bajar_Web = Convert.ToBoolean(dr["Bajar_Web"]);
                        objCatalogodeTiposDeDocumentos.Subir_WEB = Convert.ToBoolean(dr["Subir_WEB"]);
                        objCatalogodeTiposDeDocumentos.OrdenDeDespliegue = Convert.ToInt32(dr["OrdenDeDespliegue"]);
                        objCatalogodeTiposDeDocumentos.Activo = Convert.ToBoolean(dr["Activo"]);
                        objCatalogodeTiposDeDocumentos.IdDocumentoVuce = Convert.ToInt32(dr["IdDocumentoVuce"]);
                        objCatalogodeTiposDeDocumentos.IDRequisitos = Convert.ToInt32(dr["IDRequisitos"]);
                        objCatalogodeTiposDeDocumentos.IdDocumentoGP = Convert.ToInt32(dr["IdDocumentoGP"]);
                        objCatalogodeTiposDeDocumentos.TipoArchivo = dr["TipoArchivo"].ToString();
                        objCatalogodeTiposDeDocumentos.NombreExpediente = dr["NombreExpediente"].ToString();
                        objCatalogodeTiposDeDocumentos.ComplementoObligatorio = Convert.ToBoolean(dr["ComplementoObligatorio"]);
                        objCatalogodeTiposDeDocumentos.ComplementoDesc = dr["ComplementoDesc"].ToString();

                    }
                }

                else
                {
                    objCatalogodeTiposDeDocumentos = default;
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

            return objCatalogodeTiposDeDocumentos;
        }
        public CatalogodeTiposDeDocumentos BuscarS3(int IdTipoDocumento, int IdReferencia, int Id)
        {
            var objCatalogodeTiposDeDocumentos = new CatalogodeTiposDeDocumentos();

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlDataReader dr;
            SqlParameter param;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CATALOGODETIPOSDEDOCUMENTOS_S3";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("@IdTipoDocumento", SqlDbType.Int, 4);
            param.Value = IdTipoDocumento;

            param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
            param.Value = IdReferencia;

            param = cmd.Parameters.Add("@Id", SqlDbType.Int, 4);
            param.Value = Id;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        objCatalogodeTiposDeDocumentos.IdTipoDocumento = Convert.ToInt32(dr["IdTipoDocumento"]);
                        objCatalogodeTiposDeDocumentos.TipodeDocumento = dr["TipodeDocumento"].ToString();
                        objCatalogodeTiposDeDocumentos.Identificador = dr["Identificador"].ToString();
                        objCatalogodeTiposDeDocumentos.Bajar_Web = Convert.ToBoolean(dr["Bajar_Web"]);
                        objCatalogodeTiposDeDocumentos.Subir_WEB = Convert.ToBoolean(dr["Subir_WEB"]);
                        objCatalogodeTiposDeDocumentos.OrdenDeDespliegue = Convert.ToInt32(dr["OrdenDeDespliegue"]);
                        objCatalogodeTiposDeDocumentos.Activo = Convert.ToBoolean(dr["Activo"]);
                        objCatalogodeTiposDeDocumentos.IdDocumentoVuce = Convert.ToInt32(dr["IdDocumentoVuce"]);
                        objCatalogodeTiposDeDocumentos.IDRequisitos = Convert.ToInt32(dr["IDRequisitos"]);
                        objCatalogodeTiposDeDocumentos.IdDocumentoGP = Convert.ToInt32(dr["IdDocumentoGP"]);
                        objCatalogodeTiposDeDocumentos.TipoArchivo = dr["TipoArchivo"].ToString();
                        objCatalogodeTiposDeDocumentos.NombreExpediente = dr["NombreExpediente"].ToString();
                        objCatalogodeTiposDeDocumentos.Consecutivo = Convert.ToInt32(dr["Consecutivo"]);
                        objCatalogodeTiposDeDocumentos.EXPEDIENTEDIGITAL = Convert.ToBoolean(dr["EXPEDIENTEDIGITAL"].ToString());
                    }
                }

                else
                {
                    objCatalogodeTiposDeDocumentos = default;
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

            return objCatalogodeTiposDeDocumentos;
        }


        //       public CatalogodeTiposDeDocumentos GetTiposDeDocumentos_IdTipoDocumento(string IdTipoDocumento)
        //       {
        //           var objCatalogodeTiposdeDocumentos = new CatalogodeTiposDeDocumentos();
        //           try
        //           {
        //               using (con = new SqlConnection(sConexion))
        //               {
        //                   con.Open();
        //                   var cmd = new SqlCommand("NET_SEARCH_CATALOGODETIPOSDEDOCUMENTOS", con)
        //                   {
        //                       CommandType = CommandType.StoredProcedure
        //                   };
        //                   cmd.Parameters.Add("@IdTipoDocumento", SqlDbType.VarChar).Value = IdTipoDocumento;
        //                   SqlDataReader reader = cmd.ExecuteReader();

        //                   DataTable  dtb =new DataTable();

        //                   dtb.Load(reader);

        //                   if (dtb != null)
        //                   {

        //                       foreach (DataRow item in dtb.Rows)
        //                       {
        //                           objCatalogodeTiposdeDocumentos.IdTipoDocumento = int.Parse(item["IdTipoDocumento"].ToString());
        //                           objCatalogodeTiposdeDocumentos.TipodeDocumento = item["TipodeDocumento"].toString();
        //                           objCatalogodeTiposdeDocumentos.Identificador = item["Identificador"].toString();
        //                           objCatalogodeTiposdeDocumentos.Bajar_Web = bool.Parse(item["Bajar_Web"].ToString());
        //                           objCatalogodeTiposdeDocumentos.Subir_WEB = bool.Parse(item["Subir_WEB"].ToString());
        //                           objCatalogodeTiposdeDocumentos.OrdenDeDespliegue = int.Parse(item["OrdenDeDespliegue"].ToString());
        //                           objCatalogodeTiposdeDocumentos.Activo = bool.Parse(item["Activo"].ToString());
        //                           objCatalogodeTiposdeDocumentos.IdDocumentoVuce = int.Parse(item["IdDocumentoVuce"].ToString());
        //                           objCatalogodeTiposdeDocumentos.IDRequisitos = int.Parse(item["IDRequisitos"].ToString());
        //                           objCatalogodeTiposdeDocumentos.AdjuntarExp = bool.Parse(item["AdjuntarExp"].ToString());
        //// objCatalogodeTiposdeDocumentos.OrdenExp = int.Parse(item["OrdenExp"].ToString())
        //// objCatalogodeTiposdeDocumentos.IdDocumentoGP = int.Parse(item["IdDocumentoGP"].ToString())
        //// objCatalogodeTiposdeDocumentos.MostrarWEB = bool.Parse(item["MostrarWEB"].ToString())
        //// objCatalogodeTiposdeDocumentos.DHL = bool.Parse(item["DHL"].ToString())
        //// objCatalogodeTiposdeDocumentos.Elimina = bool.Parse(item["Elimina"].ToString())
        ////objCatalogodeTiposdeDocumentos.EliminaWEB = item["EliminaWEB"].toString
        //// objCatalogodeTiposdeDocumentos.TipoArchivo = item["TipoArchivo"].toString
        //// objCatalogodeTiposdeDocumentos.Operacion = int.Parse(item["Operacion"].ToString())
        //// objCatalogodeTiposdeDocumentos.EXPEDIENTEDIGITAL = bool.Parse(item["EXPEDIENTEDIGITAL"].ToString())
        //// objCatalogodeTiposdeDocumentos.NombreExpediente = item["NombreExpediente"].toString
        //// objCatalogodeTiposdeDocumentos.OrdenExAA = int.Parse(item["OrdenExAA"].ToString())
        //// objCatalogodeTiposdeDocumentos.TipoDoc = int.Parse(item["TipoDoc"].ToString())




        //                       }
        //                   }

        //                       //''TiposDeDocumentos.IdTipoDocumento = dtb.Columns("IdTipoDocumento");
        //                   }
        //           }
        //           catch (Exception ex)
        //           {
        //               throw new Exception(ex.Message + " SP: NET_SEARCH_CATALOGODETIPOSDEDOCUMENTOS");
        //           }

        //           return TiposDeDocumentos;
        //       }




    }
}

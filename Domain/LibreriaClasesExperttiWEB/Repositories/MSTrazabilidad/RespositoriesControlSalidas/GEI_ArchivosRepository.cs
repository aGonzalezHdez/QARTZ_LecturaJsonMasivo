using LibreriaClasesAPIExpertti.Entities.EntitiesTrazabilidad;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using LibreriaClasesAPIExpertti.Entities.EntitiesGei;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas
{
    public class GEI_ArchivosRepository
    {
        public string SConexion { get; set; }
        public string sConexionGP { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public GEI_ArchivosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
            sConexionGP = _configuration.GetConnectionString("dbCASAEIGP")!;
        }

        public string getUbicacionGP(string NumerodeReferencia)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;

            string RUTA = string.Empty;

            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_SEARCH_UBICACION_GP";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // @IdUsuario INT
            param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
            param.Value = NumerodeReferencia;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    RUTA = dr["RUTA"].ToString().Trim();
                }


                cmd.Parameters.Clear();
            }

            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString() + "NET_SEARCH_UBICACION_GP");
            }

            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return RUTA;
        }
        public int Insertar(GEI_Archivos lGEI_Archivos)
        {
            var id = default(int);
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;

            cn.ConnectionString = sConexionGP;
            cn.Open();
            cmd.CommandText = "NET_INSERT_GEI_Archivos";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            // ,@Id_Archivo  int
            // param = cmd.Parameters.Add("Id_Archivo", SqlDbType.Int, 4)
            // param.Value = lGEI_Archivos.Id_Archivo

            // ,@Id_Tipo  int
            param = cmd.Parameters.Add("Id_Tipo", SqlDbType.Int, 4);
            param.Value = lGEI_Archivos.Id_Tipo;

            // ,@Path  nvarchar
            param = cmd.Parameters.Add("Path", SqlDbType.NVarChar, 200);
            param.Value = lGEI_Archivos.Path;

            // ,@OC  nvarchar
            param = cmd.Parameters.Add("OC", SqlDbType.NVarChar, 50);
            param.Value = lGEI_Archivos.OC;

            // ,@Referencia  nvarchar
            param = cmd.Parameters.Add("Referencia", SqlDbType.NVarChar, 50);
            param.Value = lGEI_Archivos.Referencia;

            // ,@Id_Usuario  nvarchar
            param = cmd.Parameters.Add("Id_Usuario", SqlDbType.NVarChar, 50);
            param.Value = lGEI_Archivos.Id_Usuario;

            // ,@Ctl_Alta  datetime
            param = cmd.Parameters.Add("Ctl_Alta", SqlDbType.DateTime, 4);
            param.Value = lGEI_Archivos.Ctl_Alta;

            try
            {
                cmd.ExecuteNonQuery();
                // If CInt(cmd.Parameters("@newid_registro").Value) <> -1 Then
                // id = CInt(cmd.Parameters("@newid_registro").Value)
                // Else
                // id = 0
                // End If

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                throw new Exception(ex.Message.ToString() + "NET_INSERT_GEI_Archivos");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }


    }
}

using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos
{


    public class CatalogodePrevalRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogodePrevalRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int Buscar(string MiPedimento, string MiReferencias, int MiIdUsuario, string MIJuliano)
        {
            // Dim objCATALOGODEPREVAL As New CatalogoDePreval
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlDataReader dr;
            string MiSp = "";
            int MiIdError = 0;

            try
            {

                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_SEARCH_CATALOGODEPREVAL";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;



                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        MiSp = dr["MiSP"].ToString();
                        MiIdError = int.Parse(dr["IdError"].ToString());


                        // If MiSp = "NET_PREVAL_VERIFICA_GUIA_HOUSE" Then
                        // Beep()
                        // End If
                        if (BuscarErroresGenerales(dr["MiSP"].ToString(), MiPedimento, MiReferencias, MiIdUsuario, MiIdError, MIJuliano))
                        {
                            MiIdError = 1;
                        }
                    }
                }

                else
                {
                    MiIdError = 0;
                }

                dr.Close();
                cn.Close();
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + " " + MiSp);
            }

            return MiIdError;
        }
        public bool EjecutarPrevalidadores(string MiPedimento, string MiReferencia, int MiIDUsuario, string Juliano = "")
        {
            bool MiRegreso;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;
            try
            {

                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_EJECUTAR_CATALOGODEPREVAL";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                param = cmd.Parameters.Add("@PEDIMENTO", SqlDbType.VarChar, 30);
                param.Value = MiPedimento;

                // @NUM_REFE VARCHAR(15)
                param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                param.Value = MiReferencia;

                // , @IDUsuario INT 
                param = cmd.Parameters.Add("@IDUsuario", SqlDbType.Int, 4);
                param.Value = MiIDUsuario;

                // , @IDUsuario INT 
                param = cmd.Parameters.Add("@JULIANO", SqlDbType.VarChar, 255);
                param.Value = Juliano;

                param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                param.Direction = ParameterDirection.Output;

                // , @Messange varchar(500) OUTPUT 
                param = cmd.Parameters.Add("@Messange", SqlDbType.VarChar, 500);
                param.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                MiRegreso = true;

                cn.Close();
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return MiRegreso;

        }
        public bool BuscarErroresGenerales(string MiSp, string MiPedimento, string MiReferencia, int MiIDUsuario, int MiIDError, string MiJuliano)
        {
            bool MiRegreso;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;
            try
            {

                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = MiSp;
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                param = cmd.Parameters.Add("@PEDIMENTO", SqlDbType.VarChar, 30);
                param.Value = MiPedimento;

                // @NUM_REFE VARCHAR(15)
                param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                param.Value = MiReferencia;

                // , @IDUsuario INT 
                param = cmd.Parameters.Add("@IDUsuario", SqlDbType.Int, 4);
                param.Value = MiIDUsuario;

                param = cmd.Parameters.Add("@IDError", SqlDbType.Int, 4);
                param.Value = MiIDError;

                param = cmd.Parameters.Add("@Juliano", SqlDbType.VarChar, 50);
                param.Value = MiJuliano;

                param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                param.Direction = ParameterDirection.Output;

                // , @Messange varchar(500) OUTPUT 
                param = cmd.Parameters.Add("@Messange", SqlDbType.VarChar, 500);
                param.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                MiRegreso = Convert.ToBoolean(cmd.Parameters["@newid_registro"].Value);

                cn.Close();
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return MiRegreso;

        }

    }
}

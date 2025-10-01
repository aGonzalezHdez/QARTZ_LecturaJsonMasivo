using LibreriaClasesAPIExpertti.Entities.EntitiesCMF;
using LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF
{
    public class BitacoraJsonCMFRepository : IBitacoraJsonCMFRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public BitacoraJsonCMFRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public int Insertar()
        {
            int id;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_INSERT_BitacoraJsonCMF";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;


            try
            {

                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    id = Convert.ToInt32(dr["newid_registro"]);
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
                throw new Exception(ex.Message.ToString() + "NET_INSERT_BitacoraJsonCMF");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return id;
        }

        public int Modificar(BitacoraJsonCMF objBitacoraJsonCMF)
        {
            int id;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_BitacoraJsonCMF";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("@Guias", SqlDbType.Int, 4);
            param.Value = objBitacoraJsonCMF.Guias;

            param = cmd.Parameters.Add("@Archivos", SqlDbType.Int, 4);
            param.Value = objBitacoraJsonCMF.Archivos;


            param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;


            try
            {

                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    id = Convert.ToInt32(dr["newid_registro"]);
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
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_BitacoraJsonCMF");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return id;
        }

        public BitacoraJsonCMF BuscarActivos()
        {
            BitacoraJsonCMF objBitacoraJsonCMF = new BitacoraJsonCMF();
            SqlConnection con;

            try
            {
                using (con = new SqlConnection(sConexion))

                using (SqlCommand cmd = new("NET_SEARCH_BitacoraJsonCMF_ACTIVO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;



                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objBitacoraJsonCMF.idBitacora = Convert.ToInt32(dr["idBitacora"]);
                        objBitacoraJsonCMF.IniciaProceso = Convert.ToDateTime(dr["IniciaProceso"]);
                        //objBitacoraJsonCMF.TerminaProceso = Convert.ToDateTime(dr["TerminaProceso"]);
                        //objBitacoraJsonCMF.Guias = Convert.ToInt32(dr["Guias"]);
                        //objBitacoraJsonCMF.Archivos = Convert.ToInt32(dr["Archivos"]);
                        //objBitacoraJsonCMF.Activo = Convert.ToBoolean(dr["Activo"]);

                    }
                    else
                    {
                        objBitacoraJsonCMF = null!;
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objBitacoraJsonCMF;
        }
    }
}

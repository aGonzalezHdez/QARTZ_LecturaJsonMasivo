using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesControldeLlamadas;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesControldeLlamadas
{

    public class ControldeLlamadasRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public ControldeLlamadasRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public DataTable Cargar(int IdReferencia, string myConnectionString)
        {

            var dtb = new DataTable();
            SqlParameter param;

            using (var cn = new SqlConnection(myConnectionString))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_LOAD_CONTROLDELLAMADAS";

                    // @IdReferencia INT
                    param = dap.SelectCommand.Parameters.Add("@IdReferencia", SqlDbType.Int, 25);
                    param.Value = IdReferencia;



                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;

                    dap.Fill(dtb);

                    cn.Close();
                    cn.Dispose();
                }

                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();

                    throw new Exception(ex.Message.ToString() + "NET_LOAD_CONTROLDELLAMADAS");
                }

            }

            return dtb;
        }

        public List<ControlDeLlamadas> Cargar(int IdReferencia)
        {
            List<ControlDeLlamadas> lstControlDeLlamadas = new List<ControlDeLlamadas>();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;


            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_CONTROLDELLAMADAS";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            // @NUM_REFE VARCHAR(15) 
            param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int);
            param.Value = IdReferencia;


            // Insertar Parametro de busqueda

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ControlDeLlamadas objGuias = new ControlDeLlamadas();
                        objGuias.Llamada = int.Parse(dr["Llamada"].ToString());
                        objGuias.Descripcion = dr["Descripcion"].ToString();
                        objGuias.Fecha = DateTime.Parse(dr["Fecha"].ToString());
                        objGuias.Hora = DateTime.Parse(dr["Hora"].ToString());
                        objGuias.Notificador = dr["Notificador"].ToString();
                        objGuias.LlamadaEfectiva = bool.Parse(dr["LlamadaEfectiva"].ToString());
                        objGuias.Telefono = dr["Telefono"].ToString();
                        objGuias.NoSujetaNotifica = bool.Parse(dr["NoSujetaNotifica"].ToString());
                        lstControlDeLlamadas.Add(objGuias);
                    }
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

            return lstControlDeLlamadas;
        }
        public int Insertar(ControlDeLlamadas lcontroldellamadas)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_INSERT_CONTROLDELLAMADAS";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // ,@IdMensaje  int
            @param = cmd.Parameters.Add("@IdMensaje", SqlDbType.Int, 4);
            @param.Value = lcontroldellamadas.IdMensaje;

            // ,@IdReferencia  varchar
            @param = cmd.Parameters.Add("@IdReferencia", SqlDbType.VarChar, 25);
            @param.Value = lcontroldellamadas.IdReferencia;

            // ,@Fecha  datetime
            @param = cmd.Parameters.Add("@Fecha", SqlDbType.DateTime, 4);
            @param.Value = lcontroldellamadas.Fecha;

            // @IdUsuario
            @param = cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4);
            @param.Value = lcontroldellamadas.IdUsuario;

            // LlamadaEfectiva
            @param = cmd.Parameters.Add("@LlamadaEfectiva", SqlDbType.Bit, 1);
            @param.Value = lcontroldellamadas.LlamadaEfectiva;

            // ,@Telefono varchar(50)
            @param = cmd.Parameters.Add("@Telefono", SqlDbType.VarChar, 50);
            @param.Value = lcontroldellamadas.Telefono;

            @param = cmd.Parameters.Add("@NoSujetaNotifica", SqlDbType.Bit, 1);
            @param.Value = lcontroldellamadas.NoSujetaNotifica;

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
                throw new Exception(ex.Message.ToString() + "NET_INSERT_CONTROLDELLAMADAS");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }

    }
}

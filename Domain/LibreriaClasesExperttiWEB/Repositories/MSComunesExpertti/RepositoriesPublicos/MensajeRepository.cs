namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos
{
    using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
    using LibreriaClasesAPIExpertti.Utilities.Converters;
    using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
    using Microsoft.VisualBasic;
    using System.Data;
    using System.Data.SqlClient;
    using Microsoft.Extensions.Configuration;

    public class MensajeRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public MensajeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = configuration.GetConnectionString("dbCASAEI");
        }

        public List<MensajesporGuia> Cargar(int IdReferencia)
        {
            List<MensajesporGuia> lstMENSAJESPORGUIA = new List<MensajesporGuia>();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;


            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_MENSAJESPORGUIA";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
            param.Value = IdReferencia;
            // Insertar Parametro de busqueda

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        MensajesporGuia objMENSAJESPORGUIA = new MensajesporGuia();
                        objMENSAJESPORGUIA.IdMensajePorGuia = Convert.ToInt32(dr["IdMensajePorGuia"]);
                        objMENSAJESPORGUIA.GuiaHouse = dr["GuiaHouse"].ToString();
                        objMENSAJESPORGUIA.IdMensaje = Convert.ToInt32(dr["idMensaje"]);
                        objMENSAJESPORGUIA.Complemento = dr["Complemento"].ToString();
                        objMENSAJESPORGUIA.Mensaje = dr["Mensaje"].ToString();

                        lstMENSAJESPORGUIA.Add(objMENSAJESPORGUIA);
                    }
                }
                else
                    lstMENSAJESPORGUIA = null;
                dr.Close();
                cn.Close();
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lstMENSAJESPORGUIA;
        }

        public string existeMensaje(int IdReferencia)
        {
            string Cadena = string.Empty;
            List<MensajesporGuia> lst = new List<MensajesporGuia>();
            lst = Cargar(IdReferencia);

            if (!Information.IsNothing(lst))
            {
                foreach (var iMensaje in lst)
                    Cadena += iMensaje.Mensaje.Trim() + " : " + iMensaje.Complemento.Trim() + "\r\n";
            }

            return Cadena;
        }
    }
}

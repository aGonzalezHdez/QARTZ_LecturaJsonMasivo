using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Utilities.Converters;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class SaaioGuiasRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn = null!;

        public SaaioGuiasRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<SaaioGuias> CargarTodas(string MyNum_Refe)
        {
            List<SaaioGuias> lstGuias = new List<SaaioGuias>();

            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandText = "NET_LOAD_SAAIO_GUIAS_TODAS",
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure
                };

                // Add parameter
                SqlParameter param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                param.Value = MyNum_Refe;

                try
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                SaaioGuias objGuias = new SaaioGuias
                                {
                                    NUM_REFE = dr["NUM_REFE"].ToString(),
                                    GUIA = dr["GUIA"].ToString(),
                                    IDE_MH = dr["IDE_MH"].ToString(),
                                    CONS_GUIA = Convert.ToDouble(dr["CONS_GUIA"].ToString())
                                };

                                lstGuias.Add(objGuias);
                            }
                        }
                        else
                        {
                            lstGuias = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    cmd.Parameters.Clear();
                }
            }

            return lstGuias;
        }

        public int EliminarGuia(string NUM_REFE, string Guia, string IDE_MH)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_DELETE_SAAIOGUIAS_NEW";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // ,@NUM_REFE  varchar
            @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
            @param.Value = NUM_REFE;

            // ,@GUIA  varchar
            @param = cmd.Parameters.Add("@GUIA", SqlDbType.VarChar, 20);
            @param.Value = Guia;

            @param = cmd.Parameters.Add("@IDE_MH", SqlDbType.VarChar, 15);
            @param.Value = IDE_MH;


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
                throw new Exception(ex.Message.ToString() + "NET_DELETE_SAAIOGUIAS_NEW");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return id;
        }
        public SaaioGuias BuscarGuia(string GUIA, string IDE_MH)
        {
            var objSAAIO_GUIAS = new SaaioGuias();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;
            try
            {

                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_SEARCH_SAAIO_GUIAS_IDE_MH_GUIA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @NUM_REFE VARCHAR(15) 
                @param = cmd.Parameters.Add("@GUIA", SqlDbType.VarChar, 15);
                @param.Value = GUIA;

                @param = cmd.Parameters.Add("@IDE_MH", SqlDbType.VarChar, 1);
                @param.Value = IDE_MH;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objSAAIO_GUIAS.NUM_REFE = dr["NUM_REFE"].ToString();
                    objSAAIO_GUIAS.GUIA = dr["GUIA"].ToString();
                    objSAAIO_GUIAS.IDE_MH = dr["IDE_MH"].ToString();
                    objSAAIO_GUIAS.CONS_GUIA = Convert.ToDouble(dr["CONS_GUIA"]);
                }
                else
                {
                    objSAAIO_GUIAS = default;
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

            return objSAAIO_GUIAS;
        }


        public SaaioGuias BuscarGuiaUltima(string GUIA, string IDE_MH)
        {
            var objSAAIO_GUIAS = new SaaioGuias();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;
            try
            {

                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_SEARCH_SAAIO_GUIAS_IDE_MH_GUIA_ULTIMA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @NUM_REFE VARCHAR(15) 
                @param = cmd.Parameters.Add("@GUIA", SqlDbType.VarChar, 15);
                @param.Value = GUIA;

                @param = cmd.Parameters.Add("@IDE_MH", SqlDbType.VarChar, 1);
                @param.Value = IDE_MH;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objSAAIO_GUIAS.NUM_REFE = dr["NUM_REFE"].ToString();
                    objSAAIO_GUIAS.GUIA = dr["GUIA"].ToString();
                    objSAAIO_GUIAS.IDE_MH = dr["IDE_MH"].ToString();
                    objSAAIO_GUIAS.CONS_GUIA = Convert.ToDouble(dr["CONS_GUIA"]);
                }
                else
                {
                    objSAAIO_GUIAS = default;
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

            return objSAAIO_GUIAS;
        }
        public int Insertar(SaaioGuias lSaaioGuias)
        {

            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_INSERT_SAAIOGUIAS";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // ,@NUM_REFE  varchar
            @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
            @param.Value = lSaaioGuias.NUM_REFE == null ? DBNull.Value : lSaaioGuias.NUM_REFE;

            // ,@NUM_REFE  varchar
            @param = cmd.Parameters.Add("@GUIA", SqlDbType.VarChar, 15);
            @param.Value = lSaaioGuias.GUIA == null ? DBNull.Value : lSaaioGuias.GUIA;

            // ,@NUM_REFE  varchar
            @param = cmd.Parameters.Add("@IDE_MH", SqlDbType.VarChar, 15);
            @param.Value = lSaaioGuias.IDE_MH == null ? DBNull.Value : lSaaioGuias.IDE_MH;

            // ,@CONS_GUIA float
            @param = cmd.Parameters.Add("@CONS_GUIA", SqlDbType.Float, 4);
            @param.Value = lSaaioGuias.CONS_GUIA == null ? DBNull.Value : lSaaioGuias.CONS_GUIA;

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
                throw new Exception(ex.Message.ToString() + "NET_INSERT_SAAIOGUIAS");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return id;
        }

        public List<SaaioGuias> CargarMaster(string MyNum_Refe)
        {
            List<SaaioGuias> lstGuias = new List<SaaioGuias>();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;


            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_SAAIO_GUIAS_MASTER";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            // @NUM_REFE VARCHAR(15) 
            param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
            param.Value = MyNum_Refe;


            // Insertar Parametro de busqueda

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        SaaioGuias objGuias = new SaaioGuias();
                        objGuias.NUM_REFE = dr["NUM_REFE"].ToString();
                        objGuias.GUIA = dr["GUIA"].ToString();
                        objGuias.IDE_MH = dr["IDE_MH"].ToString();
                        objGuias.CONS_GUIA = double.Parse(dr["CONS_GUIA"].ToString());
                        lstGuias.Add(objGuias);
                    }
                }
                else
                    lstGuias = null;
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

            return lstGuias;
        }
        public SaaioGuias Buscar(string NUM_REFE, string IDE_MH)
        {
            var objSAAIO_GUIAS = new SaaioGuias();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;
            try
            {

                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_SEARCH_SAAIO_GUIAS_IDE_MH";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @NUM_REFE VARCHAR(15) 
                param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                param.Value = NUM_REFE;

                param = cmd.Parameters.Add("@IDE_MH", SqlDbType.VarChar, 1);
                param.Value = IDE_MH;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objSAAIO_GUIAS.NUM_REFE = dr["NUM_REFE"].ToString();
                    objSAAIO_GUIAS.GUIA = dr["GUIA"].ToString();
                    objSAAIO_GUIAS.IDE_MH = dr["IDE_MH"].ToString();
                    objSAAIO_GUIAS.CONS_GUIA = Convert.ToDouble(dr["CONS_GUIA"]);
                }
                else
                {
                    objSAAIO_GUIAS = default;
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

            return objSAAIO_GUIAS;
        }
        public List<SaaioGuias> CargarHouse(string NUM_REFE)
        {
            List<SaaioGuias> listadoguias = new List<SaaioGuias>();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;
            try
            {

                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_LOAD_SAAIO_GUIAS_HOUSE";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @NUM_REFE VARCHAR(15) 
                param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                param.Value = NUM_REFE;

                dr = cmd.ExecuteReader();

                listadoguias = SqlDataReaderToList.DataReaderMapToList<SaaioGuias>(dr);

                dr.Close();
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + "NET_LOAD_SAAIO_GUIAS_HOUSE");
            }

            return listadoguias;
        }
        public SaaioGuias BuscarGuiaEnReferencia(string NumeroDeReferencia, string IDE_MH)
        {
            SaaioGuias SaaioGuias = new();
            SqlConnection con;
            SqlDataReader reader;
            try
            {
                using (con = new(sConexion))
                using (SqlCommand cmd = new("NET_SEARCH_SAAIO_GUIAS_IDE_MH_GUIA_NUM_REFE", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar).Value = NumeroDeReferencia;
                    cmd.Parameters.Add("@IDE_MH", SqlDbType.VarChar).Value = IDE_MH;
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        SaaioGuias.NUM_REFE = reader["NUM_REFE"].ToString();
                        SaaioGuias.GUIA = reader["GUIA"].ToString();
                        SaaioGuias.IDE_MH = reader["IDE_MH"].ToString();
                        SaaioGuias.CONS_GUIA = Convert.ToDouble(reader["CONS_GUIA"]);
                    }
                    else
                    {
                        SaaioGuias = null;
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString() + " NET_SEARCH_SAAIO_GUIAS_IDE_MH_GUIA_NUM_REFE");
            }
            return SaaioGuias;
        }
    }
}

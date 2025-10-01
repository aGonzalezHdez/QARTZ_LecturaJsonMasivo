using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesDocumentosPorGuia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesConsultasWsExternos
{
    public class SDTConsultaWecRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public SDTConsultaWecRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public int Insertar(SDTConsultaWec lsdtconsultawec, int IdUsuario)
        {
            int id;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            try
            {
                cmd.CommandText = "NET_INSERT_CASAEI_SDTConsultaWec_NEW";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                // ,@GuiaHouse  varchar
                param = cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 10);
                param.Value = lsdtconsultawec.GuiaHouse;

                // ,@GuiaMaster  varchar
                param = cmd.Parameters.Add("@GuiaMaster", SqlDbType.VarChar, 15);
                param.Value = lsdtconsultawec.GuiaMaster;

                // ,@REFI  varchar
                param = cmd.Parameters.Add("@REFI", SqlDbType.VarChar, 15);
                param.Value = lsdtconsultawec.REFI;

                // ,@RegistroEntrada  varchar
                param = cmd.Parameters.Add("@RegistroEntrada", SqlDbType.VarChar, 15);
                param.Value = lsdtconsultawec.RegistroEntrada;

                // ,@EntradaAduana  date
                param = cmd.Parameters.Add("@EntradaAduana", SqlDbType.Date, 4);
                param.Value = lsdtconsultawec.EntradaAduana;

                // ,@AlmacenArribo  varchar
                param = cmd.Parameters.Add("@AlmacenArribo", SqlDbType.VarChar, 15);
                param.Value = lsdtconsultawec.AlmacenArribo;

                // ,@AlmacenNuevo  varchar
                param = cmd.Parameters.Add("@AlmacenNuevo", SqlDbType.VarChar, 15);
                param.Value = lsdtconsultawec.AlmacenNuevo;

                // ,@RevalidadaxAgteExterno  bit
                param = cmd.Parameters.Add("@RevalidadaxAgteExterno", SqlDbType.Bit, 4);
                param.Value = lsdtconsultawec.RevalidadaxAgteExterno;

                // ,@MercanciaAlertada  bit
                param = cmd.Parameters.Add("@MercanciaAlertada", SqlDbType.Bit, 4);
                param.Value = lsdtconsultawec.MercanciaAlertada;

                // ,@ClavePedimento  varchar
                param = cmd.Parameters.Add("@ClavePedimento", SqlDbType.VarChar, 15);
                param.Value = lsdtconsultawec.ClavePedimento;

                // ,@Bultos  int
                param = cmd.Parameters.Add("@Bultos", SqlDbType.Int, 4);
                param.Value = lsdtconsultawec.Bultos;

                // ,@Peso  decimal
                param = cmd.Parameters.Add("@Peso", SqlDbType.Decimal, 4);
                param.Value = lsdtconsultawec.Peso;

                // ,@Salida  bit
                param = cmd.Parameters.Add("@Salida", SqlDbType.Bit, 4);
                param.Value = lsdtconsultawec.Salida;

                // ,@RevalidaOtroAgenteAduanal  bit
                param = cmd.Parameters.Add("@RevalidaOtroAgenteAduanal", SqlDbType.Bit, 4);
                param.Value = lsdtconsultawec.RevalidaOtroAgenteAduanal;

                // ,@Ubicacion  varchar
                param = cmd.Parameters.Add("@Ubicacion", SqlDbType.VarChar, 50);
                param.Value = lsdtconsultawec.Ubicacion;


                param = cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4);
                param.Value = IdUsuario;


                // ,@Consecutivo  int
                param = cmd.Parameters.Add("@Consecutivo", SqlDbType.Int, 4);
                param.Value = lsdtconsultawec.Consecutivo;


                param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                param.Direction = ParameterDirection.Output;


                cn.ConnectionString = sConexion;
                cn.Open();
                cmd.ExecuteNonQuery();
                id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_INSERT_CASAEI_SDTConsultaWec_NEW");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }

        public List<SDTConsultaWec> Cargar(string MyNum_Refe)
        {
            List<SDTConsultaWec> sDTConsultaWecs = new List<SDTConsultaWec>();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;


            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_CASAEI_SDTConsultaWec";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            // @NUM_REFE VARCHAR(15) 
            param = cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 15);
            param.Value = MyNum_Refe;


            // Insertar Parametro de busqueda

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        SDTConsultaWec sDTConsultaWec = new SDTConsultaWec();

                        sDTConsultaWec.Consecutivo = dr["Consecutivo"].ToString();
                        sDTConsultaWec.GuiaHouse = dr["GuiaHouse"].ToString();
                        sDTConsultaWec.GuiaMaster = dr["GuiaMaster"].ToString();
                        sDTConsultaWec.REFI = dr["REFI"].ToString();
                        sDTConsultaWec.RegistroEntrada = dr["RegistroEntrada"].ToString();
                        sDTConsultaWec.EntradaAduana = dr["EntradaAduana"].ToString();
                        sDTConsultaWec.AlmacenArribo = dr["AlmacenArribo"].ToString();
                        sDTConsultaWec.AlmacenNuevo = dr["AlmacenNuevo"].ToString();
                        sDTConsultaWec.RevalidadaxAgteExterno = dr["RevalidadaxAgteExterno"].ToString();
                        sDTConsultaWec.MercanciaAlertada = dr["MercanciaAlertada"].ToString();
                        sDTConsultaWec.ClavePedimento = dr["ClavePedimento"].ToString();
                        sDTConsultaWec.Bultos = dr["Bultos"].ToString();
                        sDTConsultaWec.Peso = dr["Peso"].ToString();
                        sDTConsultaWec.Salida = dr["Salida"].ToString();
                        sDTConsultaWec.RevalidaOtroAgenteAduanal = dr["RevalidaOtroAgenteAduanal"].ToString();
                        sDTConsultaWec.Ubicacion = dr["Ubicacion"].ToString();
                        sDTConsultaWec.FechadeConsulta = dr["FechadeConsulta"].ToString();
                        sDTConsultaWec.IdWec = dr["IdWec"].ToString();

                        sDTConsultaWecs.Add(sDTConsultaWec);



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

            return sDTConsultaWecs;
        }
        public int BuscarConsecutivo(string GuiaHouse)
        {
            int Consecutivo = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;


            cn.ConnectionString = sConexion;
            cn.Open();


            cmd.CommandText = "NET_SEARCH_SDTConsultaWec_CONSECUTIVO";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 10);
            param.Value = GuiaHouse;
            // Insertar Parametro de busqueda

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    Consecutivo = Convert.ToInt32(dr["Consecutivo"]);
                }
                else
                    Consecutivo = 1;
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

            return Consecutivo;
        }


    }//Clase
}

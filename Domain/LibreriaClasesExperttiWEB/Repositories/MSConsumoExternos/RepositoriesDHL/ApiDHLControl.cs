using LibreriaClasesAPIExpertti.Entities.EntitiesGlobal;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaClasesAPIExpertti.Entities;
using LibreriaClasesAPIExpertti.Entities.EntitiesJsonDHL;
using System.ServiceModel.Channels;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesTurnado;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesDHL
{
    public class ApiDHLControl
    {

        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public ApiDHLControl(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<DropDownListDatos> CargarDodasporDia(int idOficina, DateTime Fecha)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (SqlConnection con = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_DODAPOROFICINA", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = idOficina;
                    cmd.Parameters.Add("@Fecha", SqlDbType.DateTime, 4).Value = Fecha;

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

        public List<JsonEstadisticaporDODA> EstadisticaporDODA(int idDODA)
        {
            List<JsonEstadisticaporDODA> Guias = new List<JsonEstadisticaporDODA>();

            var objCONSOLANEXOS = new ConsolAnexos();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlDataReader dr;
            SqlParameter @param;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_DETALLEDODAID_ESTADISTICA";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@IdDODA", SqlDbType.Int, 4);
            @param.Value = idDODA;


            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        JsonEstadisticaporDODA objJsonFaltantesporDODA = new JsonEstadisticaporDODA();

                        objJsonFaltantesporDODA.idEstadistica = Convert.ToInt32(dr["idEstadistica"]);
                        objJsonFaltantesporDODA.Estatus = dr["Estadistica"].ToString();
                        objJsonFaltantesporDODA.guias= Convert.ToInt32(dr["guias"]);
               

                        Guias.Add(objJsonFaltantesporDODA);
                    }

                }
                else
                {
                    Guias.Clear();
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

            return Guias;

        }

        public List<JsonDetalleporDODA> DetalleporDODA(int idDODA)
        {
            List<JsonDetalleporDODA> Guias = new List<JsonDetalleporDODA>();

            var objCONSOLANEXOS = new ConsolAnexos();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_DETALLEDODAID";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            @param = cmd.Parameters.Add("@IdDODA", SqlDbType.Int, 4);
            @param.Value = idDODA;



            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        JsonDetalleporDODA objJsonFaltantesporDODA = new JsonDetalleporDODA();

                        objJsonFaltantesporDODA.IdReferencia = Convert.ToInt32(dr["IdReferencia"]);
                        objJsonFaltantesporDODA.NumeroDeReferencia = dr["NumeroDeReferencia"].ToString();
                        objJsonFaltantesporDODA.GuiaHouse = dr["GuiaHouse"].ToString();
                        objJsonFaltantesporDODA.transferReceiptId = dr["transferReceiptId"].ToString();
                        objJsonFaltantesporDODA.TieneError = Convert.ToBoolean(dr["TieneError"]);
                        objJsonFaltantesporDODA.Mensaje = dr["Mensaje"].ToString();
                        objJsonFaltantesporDODA.FechaEnvio = Convert.ToDateTime(dr["FechaEnvio"]);
                        objJsonFaltantesporDODA.Tipo = dr["Tipo"].ToString();
                        objJsonFaltantesporDODA.GeneraRecibo= Convert.ToBoolean(dr["GeneraRecibo"]);
                        objJsonFaltantesporDODA.Estatus = dr["Estatus"].ToString();
                        objJsonFaltantesporDODA.idEstadistica = Convert.ToInt32(dr["idEstadistica"]);

                        Guias.Add(objJsonFaltantesporDODA);
                    }

                }
                else
                {
                    Guias.Clear();
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

            return Guias;

        }



        public List<JsonDetalleporDODA> DetalleporDODAConAgtAdu(int idDODA)
        {
            List<JsonDetalleporDODA> Guias = new List<JsonDetalleporDODA>();

            var objCONSOLANEXOS = new ConsolAnexos();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_DETALLEDODAID_ConAgtAdu";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            @param = cmd.Parameters.Add("@IdDODA", SqlDbType.Int, 4);
            @param.Value = idDODA;



            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        JsonDetalleporDODA objJsonFaltantesporDODA = new JsonDetalleporDODA();

                        objJsonFaltantesporDODA.IdReferencia = Convert.ToInt32(dr["IdReferencia"]);
                        objJsonFaltantesporDODA.NumeroDeReferencia = dr["NumeroDeReferencia"].ToString();
                        objJsonFaltantesporDODA.GuiaHouse = dr["GuiaHouse"].ToString();
                        objJsonFaltantesporDODA.transferReceiptId = dr["transferReceiptId"].ToString();
                        objJsonFaltantesporDODA.TieneError = Convert.ToBoolean(dr["TieneError"]);
                        objJsonFaltantesporDODA.Mensaje = dr["Mensaje"].ToString();
                        objJsonFaltantesporDODA.FechaEnvio = Convert.ToDateTime(dr["FechaEnvio"]);
                        objJsonFaltantesporDODA.Tipo = dr["Tipo"].ToString();
                        objJsonFaltantesporDODA.GeneraRecibo = Convert.ToBoolean(dr["GeneraRecibo"]);
                        objJsonFaltantesporDODA.Estatus = dr["Estatus"].ToString();
                        objJsonFaltantesporDODA.idEstadistica = Convert.ToInt32(dr["idEstadistica"]);

                        Guias.Add(objJsonFaltantesporDODA);
                    }

                }
                else
                {
                    Guias.Clear();
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

            return Guias;

        }

        public List<JsonDetalleporDODA> DetalleporDODAFacs(int idDODA)
        {
            List<JsonDetalleporDODA> Guias = new List<JsonDetalleporDODA>();

            var objCONSOLANEXOS = new ConsolAnexos();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_DETALLEDODAID_Facs";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            @param = cmd.Parameters.Add("@IdDODA", SqlDbType.Int, 4);
            @param.Value = idDODA;



            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        JsonDetalleporDODA objJsonFaltantesporDODA = new JsonDetalleporDODA();

                        objJsonFaltantesporDODA.IdReferencia = Convert.ToInt32(dr["IdReferencia"]);
                        objJsonFaltantesporDODA.NumeroDeReferencia = dr["NumeroDeReferencia"].ToString();
                        objJsonFaltantesporDODA.GuiaHouse = dr["GuiaHouse"].ToString();
                        objJsonFaltantesporDODA.transferReceiptId = dr["transferReceiptId"].ToString();
                        objJsonFaltantesporDODA.TieneError = Convert.ToBoolean(dr["TieneError"]);
                        objJsonFaltantesporDODA.Mensaje = dr["Mensaje"].ToString();
                        objJsonFaltantesporDODA.FechaEnvio = Convert.ToDateTime(dr["FechaEnvio"]);
                        objJsonFaltantesporDODA.Tipo = dr["Tipo"].ToString();
                        objJsonFaltantesporDODA.GeneraRecibo = Convert.ToBoolean(dr["GeneraRecibo"]);
                        objJsonFaltantesporDODA.Estatus = dr["Estatus"].ToString();
                        objJsonFaltantesporDODA.idEstadistica = Convert.ToInt32(dr["idEstadistica"]);

                        Guias.Add(objJsonFaltantesporDODA);
                    }

                }
                else
                {
                    Guias.Clear();
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

            return Guias;

        }

    }
}

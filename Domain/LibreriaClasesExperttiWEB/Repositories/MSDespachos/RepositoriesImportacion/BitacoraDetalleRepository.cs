using LibreriaClasesAPIExpertti.Entities.EntitiesDespacho;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSDespachos.RepositoriesImportacion
{
    public class BitacoraDetalleRepository
    {
        IConfiguration _configuration;
        public string SConexion { get; set; }
        public BitacoraDetalleRepository(IConfiguration configuracion)
        {
            _configuration = configuracion;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public BitacoraDetalle Buscar(string GuiaHouse)
        {
            BitacoraDetalle objBitacoraDetalle = new BitacoraDetalle();
            using (SqlConnection cn = new SqlConnection(SConexion))
            using (SqlCommand cmd = new SqlCommand("[Pocket].[NET_SEARCH_BitacoraDetalleporGuia]", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 15).Value = GuiaHouse;

                try
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objBitacoraDetalle.IdDetalleBitacora = Convert.ToInt32(dr["IdDetalleBitacora"]);
                            objBitacoraDetalle.IdRelacionBitacora = Convert.ToInt32(dr["IdRelacionBitacora"]);
                            objBitacoraDetalle.GuiaHouse = dr["GuiaHouse"].ToString();
                            objBitacoraDetalle.Descripcion = dr["Descripcion"].ToString();
                            objBitacoraDetalle.ValorDolares = Convert.ToDecimal(dr["ValorDolares"]);
                            objBitacoraDetalle.CantidadDeBultos = Convert.ToInt32(dr["CantidadDeBultos"]);
                            objBitacoraDetalle.Peso = Convert.ToDecimal(dr["Peso"]);
                            objBitacoraDetalle.FechaAsignacion = Convert.ToDateTime(dr["FechaAsignacion"]);
                            objBitacoraDetalle.FechaLlegada = Convert.ToDateTime(dr["FechaLlegada"]);
                            objBitacoraDetalle.FechaSalida = Convert.ToDateTime(dr["FechaSalida"]);
                            objBitacoraDetalle.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                            objBitacoraDetalle.Proceso = Convert.ToInt32(dr["Proceso"]);

                            if (!dr.IsDBNull(dr.GetOrdinal("Flujo")))
                            {
                                objBitacoraDetalle.Flujo = dr["Flujo"].ToString();
                            }
                        }
                        else
                        {
                            objBitacoraDetalle = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return objBitacoraDetalle;
        }
        public int ModificarporGuia(BitacoraDetalle lbitacoradetalle)
        {
            int id;
            using (SqlConnection cn = new SqlConnection(SConexion))
            using (SqlCommand cmd = new SqlCommand("fdx.NET_UPDATE_BitacoraDetalleporGuia", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 15).Value = lbitacoradetalle.GuiaHouse;
                cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 250).Value = lbitacoradetalle.Descripcion;
                cmd.Parameters.Add("@ValorDolares", SqlDbType.Decimal, 4).Value = lbitacoradetalle.ValorDolares;
                cmd.Parameters.Add("@CantidadDeBultos", SqlDbType.Int, 4).Value = lbitacoradetalle.CantidadDeBultos;
                cmd.Parameters.Add("@Peso", SqlDbType.Decimal, 4).Value = lbitacoradetalle.Peso;
                cmd.Parameters.Add("@FechaAsignacion", SqlDbType.DateTime, 4).Value = lbitacoradetalle.FechaAsignacion;
                cmd.Parameters.Add("@FechaLlegada", SqlDbType.DateTime, 4).Value = lbitacoradetalle.FechaLlegada;
                cmd.Parameters.Add("@FechaSalida", SqlDbType.DateTime, 4).Value = lbitacoradetalle.FechaSalida;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = lbitacoradetalle.IdUsuario;
                cmd.Parameters.Add("@IDDatosDeEmpresa", SqlDbType.Int, 4).Value = lbitacoradetalle.IDDatosDeEmpresa;

                cmd.Parameters.Add("@Flujo", SqlDbType.VarChar, 10).Value = lbitacoradetalle.Flujo ?? (object)DBNull.Value;

                SqlParameter outputParam = cmd.Parameters.Add("@newid_registro", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    id = (int)outputParam.Value;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " fdx.NET_UPDATE_BitacoraDetalleporGuia");
                }
            }
            return id;
        }

    }
}

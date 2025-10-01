using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaClasesAPIExpertti.Entities.EntitiesJsonDHL;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesDHL
{
    internal class BitacoraApiDHLRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public BitacoraApiDHLRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int Insertar(BitacoraApiDHL lbitacoraApiDHL)
        {
            int id;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_INSERT_CASAEI_BitacoraAPIDHL";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
            param.Value = lbitacoraApiDHL.IdReferencia;

            param = cmd.Parameters.Add("@transferReceiptId", SqlDbType.VarChar, 50);
            param.Value = lbitacoraApiDHL.transferReceiptId;

            param = cmd.Parameters.Add("@TieneError", SqlDbType.Bit, 4);
            param.Value = lbitacoraApiDHL.TieneError;

            param = cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 250);
            param.Value = lbitacoraApiDHL.Mensaje;

            param = cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 25);
            param.Value = lbitacoraApiDHL.GuiaHouse;

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
                throw new Exception(ex.Message.ToString() + " NET_INSERT_CASAEI_BitacoraAPIDHL");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return id;
        }
    }
}

using LibreriaClasesAPIExpertti.Entities.EntitiesVucem;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesVUCEM
{


    public class ConsolFacturasRepository
    {
        public string sConexion { get; set; }        
        public IConfiguration _configuration;
        public SqlConnection cn;

        public ConsolFacturasRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public ConsolFacturas Buscar(string numerodeReferencia, int consFact)
        {
            var objConsolFacturas = new ConsolFacturas();

            using (var cn = new SqlConnection(sConexion))
            using (var cmd = new SqlCommand("NET_SEARCH_ConsolFacturas", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("NumerodeReferencia", SqlDbType.VarChar, 15)).Value = numerodeReferencia;
                cmd.Parameters.Add(new SqlParameter("ConsFact", SqlDbType.Int)).Value = consFact;

                try
                {
                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows && dr.Read())
                        {
                            objConsolFacturas.IdConsolFactura = Convert.ToInt32(dr["IdConsolFactura"]);
                            objConsolFacturas.IdCustomAlert = Convert.ToInt32(dr["IdCustomAlert"]);
                            objConsolFacturas.GuiaHouse = dr["GuiaHouse"].ToString();
                            objConsolFacturas.ConsFact = Convert.ToInt32(dr["ConsFact"]);
                            objConsolFacturas.NumerodeReferencia = dr["NumerodeReferencia"].ToString();
                            objConsolFacturas.IdCliente = Convert.ToInt32(dr["IdCliente"]);
                        }
                        else
                        {
                            objConsolFacturas = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return objConsolFacturas;
        }

        public int Modificar(string numerodeReferencia, int consFact, int idCliente)
        {
            int id = 0;

            using (var cn = new SqlConnection(sConexion))
            using (var cmd = new SqlCommand("NET_UPDATE_ConsolFacturas", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@NumerodeReferencia", SqlDbType.VarChar, 15)).Value = numerodeReferencia;
                cmd.Parameters.Add(new SqlParameter("@ConsFact", SqlDbType.Int)).Value = consFact;
                cmd.Parameters.Add(new SqlParameter("@IdCliente", SqlDbType.Int)).Value = idCliente;
                var outputParam = cmd.Parameters.Add("@newid_registro", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    id = Convert.ToInt32(outputParam.Value);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " NET_UPDATE_ConsolFacturas");
                }
            }

            return id;
        }

        public int Insertar(ConsolFacturas lconsolfacturas)
        {
            int id = 0;

            using (var cn = new SqlConnection(sConexion))
            using (var cmd = new SqlCommand("NET_INSERT_CASAEI_ConsolFacturas", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@IdCustomAlert", SqlDbType.Int)).Value = lconsolfacturas.IdCustomAlert;
                cmd.Parameters.Add(new SqlParameter("@GuiaHouse", SqlDbType.VarChar, 25)).Value = lconsolfacturas.GuiaHouse;
                cmd.Parameters.Add(new SqlParameter("@ConsFact", SqlDbType.Int)).Value = lconsolfacturas.ConsFact;
                cmd.Parameters.Add(new SqlParameter("@NumerodeReferencia", SqlDbType.VarChar, 15)).Value = lconsolfacturas.NumerodeReferencia;
                cmd.Parameters.Add(new SqlParameter("@IdCliente", SqlDbType.Int)).Value = lconsolfacturas.IdCliente;

                var outputParam = cmd.Parameters.Add("@newid_registro", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    id = Convert.ToInt32(outputParam.Value);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " NET_INSERT_CASAEI_ConsolFacturas");
                }
            }

            return id;
        }
    }
}

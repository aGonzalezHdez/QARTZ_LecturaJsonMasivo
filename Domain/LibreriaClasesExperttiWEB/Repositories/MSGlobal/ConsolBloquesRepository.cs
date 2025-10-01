using LibreriaClasesAPIExpertti.Entities.EntitiesGlobal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSGlobal
{
    public class ConsolBloquesRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public ConsolBloquesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }


        public ConsolBloques Buscar(int IdBloque)
        {

            ConsolBloques objConsolBloques = new();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CONSOLBLOQUES";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@IdBloque", SqlDbType.Int, 4);
            @param.Value = IdBloque;

            
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objConsolBloques.IdBloque = Convert.ToInt32(dr["IdBloque"]);
                    objConsolBloques.NoBloque= dr["NoBloque"].ToString();
                    objConsolBloques.idTipodePedimento = Convert.ToInt32(dr["idTipodePedimento"]);
                    objConsolBloques.IdPedimento = Convert.ToInt32(dr["IdPedimento"]);
                    objConsolBloques.IDMasterConsol = Convert.ToInt32(dr["IDMasterConsol"]);
                    objConsolBloques.FechaBloque = Convert.ToDateTime(dr["FechaBloque"]);
                    objConsolBloques.Estatus = Convert.ToInt32(dr["Estatus"]);
                    objConsolBloques.idSalidasConsol = Convert.ToInt32(dr["idSalidasConsol"]);
                    objConsolBloques.FechaCierre = Convert.ToDateTime(dr["FechaCierre"]);
                    objConsolBloques.Corte = Convert.ToInt32(dr["Corte"]);
                    objConsolBloques.IdRegion = Convert.ToInt32(dr["IdRegion"]);
                    objConsolBloques.Automatico = Convert.ToBoolean(dr["Automatico"]);
                    objConsolBloques.ftpAutomatico = Convert.ToBoolean(dr["ftpAutomatico"]);
                    objConsolBloques.ServicioExtraordinario = Convert.ToBoolean(dr["ServicioExtraordinario"]);
                    objConsolBloques.IdCorte = Convert.ToInt32(dr["IdCorte"]);
                }
                else
                {
                    objConsolBloques = null;
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

            return objConsolBloques;

        }

    }
}

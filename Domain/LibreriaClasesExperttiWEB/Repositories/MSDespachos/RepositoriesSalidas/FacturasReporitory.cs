using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesGlobal;

namespace LibreriaClasesAPIExpertti.Repositories.MSDespachos.RepositoriesSalidas
{
    public class FacturasReporitory
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public FacturasReporitory(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public DataTable ArchivoFacEnDatatable(int IdReferencia)
        {

            var dtb = new DataTable();
            SqlParameter @param;

            using (var cn = new SqlConnection(sConexion))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_GENERA_ARCHIVOS_FAC_IDREFERENCIA_JSON";


                    @param = dap.SelectCommand.Parameters.Add("@IDREFERENCIA", SqlDbType.Int, 4);
                    @param.Value = IdReferencia;



                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;

                    dap.Fill(dtb);

                    cn.Close();
                    // SqlConnection.ClearPool(cn)
                    cn.Dispose();
                }

                catch (Exception ex)
                {
                    cn.Close();
                    // SqlConnection.ClearPool(cn)
                    cn.Dispose();

                    throw new Exception(ex.Message.ToString() + "NET_GENERA_ARCHIVOS_FAC_IDREFERENCIA_JSON");
                }

            }

            return dtb;
        }

        public List<int> FacsPendientes()
        {
            List<int> Referencias = new ();

            var objCONSOLANEXOS = new ConsolAnexos();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_FACS_PENDIENTES";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        int idReferencia = Convert.ToInt32(dr["IDReferenciaReal"]);
                        Referencias.Add(idReferencia);
                    }

                }
                else
                {
                    Referencias.Clear();
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

            return Referencias;

        }
    }
}

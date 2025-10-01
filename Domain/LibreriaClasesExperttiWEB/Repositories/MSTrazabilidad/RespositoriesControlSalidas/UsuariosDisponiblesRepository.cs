using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas
{
    public class UsuariosDisponiblesRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public UsuariosDisponiblesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public DataTable DepartamentoAsignado(int IdUsuario, int IdDepartamento)
        {
            DataTable dtb = new DataTable();

            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                try
                {
                    cn.Open();
                    SqlDataAdapter dap = new SqlDataAdapter
                    {
                        SelectCommand = new SqlCommand("NET_LOAD_DEPARTAMENTOS_DISPONIBLES", cn)
                    };

                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;

                    dap.SelectCommand.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                    dap.SelectCommand.Parameters.Add("@IdDepartamento", SqlDbType.Int).Value = IdDepartamento;

                    dap.Fill(dtb);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString() + " NET_LOAD_DEPARTAMENTOS_DISPONIBLES");
                }
            }

            return dtb;
        }

    }
}

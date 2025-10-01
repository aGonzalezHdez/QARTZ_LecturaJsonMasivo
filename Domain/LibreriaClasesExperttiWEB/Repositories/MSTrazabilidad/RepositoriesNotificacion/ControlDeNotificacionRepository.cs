using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesNotificacion.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesNotificacion.Interfaces;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesNotificacion
{
    public class ControlDeNotificacionRepository: IControlDeNotificacionRepository
    {
        public string SConexion { get; set; }
        string IControlDeNotificacionRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        
        public IConfiguration _configuration;
        public ControlDeNotificacionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<DropDownListDatos> CargarVentanasControlNotificacion(int IdUsuario)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_DEPTOSVENTANACONTROLNOTIFICACION", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = IdUsuario;

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

        public DataTable CargarControlNotificacion(int Ide, int IdUsuario, int IdVentana)
        {
            DataTable dtb = new DataTable();
            SqlParameter param;

            using (SqlConnection cn = new SqlConnection(SConexion))
            {
                try
                {
                    cn.Open();
                    SqlDataAdapter dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "RPT_REPORT_CONTROLNOTIFICACION_NEW";

                    param = dap.SelectCommand.Parameters.Add("@IDE", SqlDbType.Int);
                    param.Value = Ide;

                    param = dap.SelectCommand.Parameters.Add("@IdUsuario", SqlDbType.Int);
                    param.Value = IdUsuario;

                    param = dap.SelectCommand.Parameters.Add("@IdVentana", SqlDbType.Int);
                    param.Value = IdVentana;


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

                    throw new Exception(ex.Message.ToString() + "RPT_REPORT_CONTROLNOTIFICACION_NEW");
                }
            }
            return dtb;
        }

        public DataTable CargarControlNotificacionDetalle(int IdFiltro, int IdUsuario, int IdOficina, int IdGrupo , int IdVentana)
        {
            DataTable dtb = new DataTable();
            SqlParameter param;

            using (SqlConnection cn = new SqlConnection(SConexion))
            {
                try
                {
                    cn.Open();
                    SqlDataAdapter dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_REPORT_NOTIFICACION_PENDIENTE_DETALLE_NEW";

                    param = dap.SelectCommand.Parameters.Add("@FILTRO", SqlDbType.Int);
                    param.Value = IdFiltro;

                    param = dap.SelectCommand.Parameters.Add("@IDUSUARIO", SqlDbType.Int);
                    param.Value = IdUsuario;

                    param = dap.SelectCommand.Parameters.Add("@IDOFICINA", SqlDbType.Int);
                    param.Value = IdOficina;

                    param = dap.SelectCommand.Parameters.Add("@IDGRUPO", SqlDbType.Int);
                    param.Value = IdGrupo;

                    param = dap.SelectCommand.Parameters.Add("@IdVentana", SqlDbType.Int);
                    param.Value = IdVentana;


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

                    throw new Exception(ex.Message.ToString() + "NET_REPORT_NOTIFICACION_PENDIENTE_DETALLE_NEW");
                }
            }
            return dtb;
        }
    }
}

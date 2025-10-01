using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
using LibreriaClasesAPIExpertti.Entities.EntitiesTrazabilidad;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos.Interfaces;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos
{
    public class AsignacionDeGuiasRepository : IAsignacionDeGuiasRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con = null!;

        public AsignacionDeGuiasRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public AsignaciondeGuias BuscarUltimoAsignado(int IDReferencia, int IDDepartamento, int IDOficina)
        {
            var objASIGNACIONDEGUIAS = new AsignaciondeGuias();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_ASIGNACIONDEGUIAS_ULTIMA";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@IDReferencia", SqlDbType.Int);
            @param.Value = IDReferencia;

            @param = cmd.Parameters.Add("@IDDepartamento", SqlDbType.Int);
            @param.Value = IDDepartamento;

            @param = cmd.Parameters.Add("@IDOficina", SqlDbType.Int);
            @param.Value = IDOficina;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    objASIGNACIONDEGUIAS.idAsignacionDeGuias = Convert.ToInt32(dr["idAsignacionDeGuias"]);
                    objASIGNACIONDEGUIAS.idReferencia = Convert.ToInt32(dr["IdReferencia"]);
                    objASIGNACIONDEGUIAS.idDepartamento = Convert.ToInt32(dr["IdDepartamento"]);
                    objASIGNACIONDEGUIAS.idUsuarioAsignado = Convert.ToInt32(dr["IdUsuarioAsignado"]);
                    objASIGNACIONDEGUIAS.Nombre = dr["Nombre"].ToString();
                    objASIGNACIONDEGUIAS.IdUsuarioAsigna = Convert.ToInt32(dr["IdUsuarioAsigna"]);
                    objASIGNACIONDEGUIAS.FechaAsignacion = Convert.ToDateTime(dr["FechaAsignacion"]);
                    objASIGNACIONDEGUIAS.IdCheckPointSalida = Convert.ToInt32(dr["IdCheckPointSalida"]);
                    objASIGNACIONDEGUIAS.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                }
                else
                {
                    objASIGNACIONDEGUIAS = default;
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

            return objASIGNACIONDEGUIAS;
        }

        public async Task<int[]> Asignar(int IdReferencia, int IdDepartamento, int IdUsuarioAsigna, int IdCheckpointSalida)
        {
            int IdUsuario;
            int IdGrupo = 0;
            int TipodeAsignacion = 0;
            int[] usuarios = new int[3];

            try
            {
                using (con = new(sConexion))
                using (SqlCommand cmd = new("NET_ASIGNACION_GUIA", con))
                {

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4).Value = IdReferencia;
                    cmd.Parameters.Add("@IdDepartamentoDestino", SqlDbType.Int, 4).Value = IdDepartamento;
                    cmd.Parameters.Add("@IdUsuarioAsigna", SqlDbType.Int, 4).Value = IdUsuarioAsigna;
                    cmd.Parameters.Add("@IdCheckpointSalida", SqlDbType.Int, 4).Value = IdCheckpointSalida;
                    cmd.Parameters.Add("@IDUSUARIO", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@IDGRUPO", SqlDbType.Int, 4).Direction = ParameterDirection.Output;


                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                        if (dr.HasRows)
                        {
                            dr.Read();
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                            IdGrupo = Convert.ToInt32(dr["IdGrupo"]);
                            TipodeAsignacion = Convert.ToInt32(dr["TIPODEASIGNACION"]);

                            usuarios[0] = IdUsuario;
                            usuarios[1] = IdGrupo;
                            usuarios[2] = TipodeAsignacion;
                        }
                }
            }
            catch (Exception ex)
            {
                IdUsuario = 0;
                throw new Exception(ex.Message.ToString() + " NET_ASIGNACION_GUIA");
            }
            return usuarios;
        }
        public AsignaciondeGuias BuscarUltimoDepartamento(int IDReferencia)
        {
            AsignaciondeGuias objASIGNACIONDEGUIAS = new AsignaciondeGuias();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;


            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_ASIGNACIONDEGUIAS_ULTIMODEPARTAMENTO";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("@IDReferencia", SqlDbType.Int);
            param.Value = IDReferencia;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    objASIGNACIONDEGUIAS.idAsignacionDeGuias = Convert.ToInt32(dr["idAsignacionDeGuias"]);
                    objASIGNACIONDEGUIAS.idReferencia = Convert.ToInt32(dr["IdReferencia"]);
                    objASIGNACIONDEGUIAS.idDepartamento = Convert.ToInt32(dr["IdDepartamento"]);
                    objASIGNACIONDEGUIAS.idUsuarioAsignado = Convert.ToInt32(dr["IdUsuarioAsignado"]);
                    objASIGNACIONDEGUIAS.IdUsuarioAsigna = Convert.ToInt32(dr["IdUsuarioAsigna"]);
                    objASIGNACIONDEGUIAS.FechaAsignacion = Convert.ToDateTime(dr["FechaAsignacion"]);
                    objASIGNACIONDEGUIAS.IdCheckPointSalida = Convert.ToInt32(dr["IdCheckPointSalida"]);
                    objASIGNACIONDEGUIAS.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                }
                else
                    objASIGNACIONDEGUIAS = null/* TODO Change to default(_) if this is not a reference type */;
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

            return objASIGNACIONDEGUIAS;
        }

        public AsignarGuiasRespuesta ReasignarGuia(int IdUsuarioAsigna, int IdUsuarioAsignado, int IdReferencia)


        {

            var objRespuesta = new AsignarGuiasRespuesta();


            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_REASIGNACION_GUIA";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // @IdUsuarioAsigna INT
            param = cmd.Parameters.Add("@IdUsuarioAsigna", SqlDbType.Int, 4);
            param.Value = IdUsuarioAsigna;

            // @IdUsuarioAsignado INT,
            param = cmd.Parameters.Add("@IdUsuarioAsignado", SqlDbType.Int, 4);
            param.Value = IdUsuarioAsignado;

            // @IdReferencia INT
            param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
            param.Value = IdReferencia;

            // @IDUSUARIO INT
            param = cmd.Parameters.Add("@IdUsuarioAsignadoC", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;

            // @IdEvento INT
            param = cmd.Parameters.Add("@NEWID_REGISTRO", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;


            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    objRespuesta.IdUsuarioAsignado = Convert.ToInt32(dr["IdUsuario"]);
                    objRespuesta.IdEvento = Convert.ToInt32(dr["IdEvento"]);

                }

                cmd.Parameters.Clear();
            }

            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString() + "NET_REASIGNACION_GUIA");
            }

            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return objRespuesta;

        }
        public int ValidaFastMorning(string Referencia)
        {
            var id = default(int);
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_VALIDATE_CLASIASALIDAS";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
            param.Value = Referencia;



            try
            {
                cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }
        public int ValidaCove(string Referencia)
        {
            var id = default(int);
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_VALIDATE_COVE_FASTMORNING";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
            param.Value = Referencia;



            try
            {
                cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }

        public DataTable CargarPendientes(int IdUsuario)
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
                    dap.SelectCommand.CommandText = "NET_LOAD_GUIASPENDIENTES";


                    @param = dap.SelectCommand.Parameters.Add("@IDUSUARIO", SqlDbType.Int, 4);
                    @param.Value = IdUsuario;



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

                    throw new Exception(ex.Message.ToString() + "NET_LOAD_GUIASPENDIENTES");
                }

            }

            return dtb;
        }


    }
}

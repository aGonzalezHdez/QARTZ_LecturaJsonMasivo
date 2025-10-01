using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos
{
    public class UbicaciondeArchivosRepository: IUbicaciondeArchivosRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public UbicaciondeArchivosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public string BuscaUbicacionDeArchivos(int MyIdUbicacion)
        {

            var UADATA = new UbicaciondeArchivosRepository(_configuration);
            var UA = new UbicaciondeArchivos();
            string MiRegreso = "";
            UA = UADATA.Buscar(MyIdUbicacion);

            if (!(UA == null))
            {
                MiRegreso = UA.Ubicacion;


            }
            return MiRegreso;
        }
        public UbicaciondeArchivos Buscar(int IdUbicacion)
        {
            UbicaciondeArchivos objUbicaciondeArchivos = new UbicaciondeArchivos();
            try
            {
                using (con = new SqlConnection(SConexion))

                using (SqlCommand cmd = new("NET_SEARCH_UbicaciondeArchivos", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdUbicacion", SqlDbType.Int, 4).Value = IdUbicacion;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objUbicaciondeArchivos.IdUbicacion = Convert.ToInt32(dr["IdUbicacion"]);
                        objUbicaciondeArchivos.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                        objUbicaciondeArchivos.Patente = dr["Patente"].ToString();
                        objUbicaciondeArchivos.Descripcion = dr["Descripcion"].ToString();
                        objUbicaciondeArchivos.Ubicacion = dr["Ubicacion"].ToString();
                        objUbicaciondeArchivos.UbicacionAnterior = dr["UbicacionAnterior"].ToString();
                    }
                    else
                    {
                        objUbicaciondeArchivos = null!;
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objUbicaciondeArchivos;
        }



        public string DeterminarUbicacionDeArchivos(string MiPatente, string MiAduana, string MiPreval)
        {
            string MiRegreso;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {

                cmd.CommandText = "NET_LOAD_UBICACION_PARA_VALIDAR_Y_PAGAR";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @MiPatente VARCHAR(4)
                @param = cmd.Parameters.Add("@MiPatente", SqlDbType.VarChar, 4);
                @param.Value = MiPatente;

                // , @MiAduana VARCHAR(3)
                @param = cmd.Parameters.Add("@MiAduana", SqlDbType.VarChar, 3);
                @param.Value = MiAduana;

                // ,@MiPreval VARCHAR(3)
                @param = cmd.Parameters.Add("@MiPreval", SqlDbType.VarChar, 3);
                @param.Value = MiPreval;

                // @MiUbicacion VARCHAR(800) OUTPUT 
                @param = cmd.Parameters.Add("@MiUbicacion", SqlDbType.VarChar, 800);
                @param.Direction = ParameterDirection.Output;

                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.ExecuteNonQuery();
                MiRegreso = (string)cmd.Parameters["@MiUbicacion"].Value;


                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                MiRegreso = "";
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_LOAD_UBICACION_PARA_VALIDAR_Y_PAGAR");
            }
            cn.Close();
            cn.Dispose();

            return MiRegreso;



        }

        public string fMisDocumentos (int idUsuario)
        {
            string vMisDocumentos= string.Empty;
            try
            {
                UbicaciondeArchivos ubicacionDeArchivos = new();
                ubicacionDeArchivos = Buscar(121);
                if (ubicacionDeArchivos == null)
                {
                    throw new Exception("No existe el id: 121 Ruta para MisDocumentos");
                }

                CatalogoDeUsuarios catalogoDeUsuarios = new CatalogoDeUsuarios();
                CatalogoDeUsuariosRepository catalogoDeUsuariosRepository = new(_configuration);
                catalogoDeUsuarios = catalogoDeUsuariosRepository.BuscarPorId(idUsuario);

                if (catalogoDeUsuarios == null)
                {
                    throw new Exception("El usuario, no existe en expertti");
                }

                vMisDocumentos = ubicacionDeArchivos.Ubicacion + @"\" + catalogoDeUsuarios.Usuario.Trim() + @"\ExperttiTmp\";
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.Trim()) ;
            }
          

                return vMisDocumentos;
        }

    }
}

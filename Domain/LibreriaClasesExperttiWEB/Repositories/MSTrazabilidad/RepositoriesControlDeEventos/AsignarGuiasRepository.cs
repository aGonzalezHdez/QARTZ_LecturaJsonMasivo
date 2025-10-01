using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos
{
    public class AsignarGuiasRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public AsignarGuiasRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI");
        }

        public AsignarGuiasRespuesta Validaciones(CatalogodeCheckPoints objCheck, string Referencia, int IdUsuario, bool Extraordinario, string Observaciones, int IDDatosDeEmpresa)
        {
            AsignarGuiasRespuesta objRespuesta = new();


            //CatalogoDeUsuarios ObjUsuario = new CatalogoDeUsuarios();
            //CatalogoDeUsuariosData ObjUsuarioD = new CatalogoDeUsuariosData();

            //ObjUsuario = ObjUsuarioD.BuscarPorId(IdUsuario, MyConnectionString);
            //if (IsNothing(ObjUsuario))
            //    throw new ArgumentException("Hubo un problema con el usuario que ingreso al sistema");

            //SaaioPedime objPedime = new SaaioPedime();
            //SaaioPedimeData objPedimeD = new SaaioPedimeData();
            //objPedime = objPedimeD.Buscar(Referencias, MyConnectionString);

            //if (!IsNothing(objPedime))
            //{
            //    if (objCheck.AsignarPedimento)
            //    {
            //        if (IsNothing(objPedime.NUM_PEDI))
            //        {
            //            if (objPedime.NUM_PEDI == "")
            //            {
            //                objRespuesta = AsignarPedimento(Referencias, ObjUsuario, Extraordinario, IDDatosDeEmpresa, MyConnectionString);
            //                objPedime.NUM_PEDI = objRespuesta.Pedimento;
            //            }
            //        }
            //    }

            //    if (objCheck.ValidaPedimento)
            //    {
            //        if (IsNothing(objPedime))
            //            throw new ArgumentException("La referencia no existe en el sistema de pedimentos");

            //        if (objPedime.NUM_PEDI == "")
            //            throw new ArgumentException(objCheck.Descripcion.Trim() + ":Requiere numero de Pedimento");
            //    }
            //    objRespuesta.Pedimento = IIf(IsNothing(objPedime.NUM_PEDI), "", objPedime.NUM_PEDI);


            //    if (objCheck.ValidaProforma)
            //        ValidarProforma(Referencias);

            //    objRespuesta.Prevalidar = objCheck.Prevalida;
            //}

            //if (objCheck.ObservacionObligatoria)
            //{
            //    if (Observaciones.Trim().Length < 15)
            //        throw new ArgumentException("Es Necesario insertar una observación mas especifica");
            //}

            return objRespuesta;
        }

        public async Task<int> BuscaDepartamentoAnterior(int IdReferencia)
        {

            int idDepartamento = 0;

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_SEARCH_DEPARTAMENTO_ANTERIOR_EN_REFERENCIA", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    // @IdReferencia INT ,
                    cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4).Value = IdReferencia;

                    using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                    if (dr.HasRows)
                    {
                        dr.Read();

                        idDepartamento = Convert.ToInt32(dr["idDepartamento"]);
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return idDepartamento;
        }

    }
}

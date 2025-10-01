using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesJCJF;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesOperaciones;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesJsonDHL;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesDHL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1;
using LibreriaClasesAPIExpertti.Entities.EntitiesDespacho;
using LibreriaClasesAPIExpertti.Repositories.MSDespachos.RepositoriesDoda;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos
{
    public class ControldeEventosRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public ControldeEventosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public async Task<AsignarGuiasRespuesta> InsertarEvento(ControldeEventos lEventos, int idDepartamento, int IdOficina, bool Extraordinario, string Observaciones, int IDDatosDeEmpresa)
        {

            var objRespuesta = new AsignarGuiasRespuesta();

            var objCatCheck = new CatalogodeCheckPoints();
            var objCatCheckD = new CatalogodeCheckPointsRepository(_configuration);
            var CntlEvD = new ControldeEventosRepository(_configuration);

            if (Observaciones is not null)
            {
                Observaciones = Observaciones.Trim();
            }
            else
            {
                Observaciones = "";

            }

            objCatCheck = objCatCheckD.BuscarId(lEventos.IDCheckPoint, IdOficina, lEventos.IDReferencia);
            if (objCatCheck == null)
            {
                throw new ArgumentException("El checkpoint no esta configurado " + lEventos.IDCheckPoint + " Oficina " + IdOficina);
            }

            // SI DEBO TOMAR EL DEPARTAMENTO DEL POR EL CUAL LLEGO A ESTA AREA
            if (objCatCheck.DepOrigen)
            {

                int DepartamentoAnterior;
                DepartamentoAnterior = BuscaDepartamentoAnterior(lEventos.IDReferencia);

                objCatCheck.IdDepartamentoDestino = DepartamentoAnterior;
            }



            if (objCatCheck.ObservacionObligatoria)
            {
                if (Observaciones.Trim().Length < 15)
                {
                    throw new ArgumentException("Es Necesario insertar una observación mas especifica");
                }
            }
            // Cambie a que respete las prescedencias IVBM
            if (objCatCheck.PrecedenciaObligatoria)
            {
                if (objCatCheck.ListaDeIDPrecedencias1.Count() == 0 | await ExistePrecedencia(lEventos.IDReferencia, objCatCheck.ListaDeIDPrecedencias1))
                {
                }
                else
                {
                    var LstPre = new List<string>();
                    LstPre = await Prescedencias(objCatCheck.ListaDeIDPrecedencias1, IdOficina);
                    string cad = "";

                    foreach (string item in LstPre)
                        cad = cad + " | " + item;

                    throw new Exception("Necesita Check point de precedencia : " + cad);
                }
            }

            if (objCatCheck.TipoDeEvento == 4) // Informativos
            {
                objRespuesta.IdEvento = await CntlEvD.Insertar(lEventos, objCatCheck.Duplicidad, objCatCheck.Automatico, Observaciones);
            }

            else
            {
                int departamentoActual = 0;
                int departamentoDestino = 0;
                departamentoActual = await BuscaDepartamentoActual(lEventos.IDReferencia, IdOficina);
                if (departamentoActual == 0)
                {
                    departamentoActual = idDepartamento;
                }

                if (departamentoActual > 0)
                {
                    objCatCheck = await objCatCheckD.BuscarPorDepto(lEventos.IDCheckPoint, IdOficina, departamentoActual, lEventos.IDReferencia);

                    var objDep = new CatalogoDepartamentos();
                    var objDepD = new CatalogoDepartamentosRepository(_configuration);
                    objDep = await objDepD.Buscar(departamentoActual);

                    if (objCatCheck == null)
                    {
                        throw new Exception("La referencia está asignada al departamento " + objDep.NombreDepartamento.Trim() + " y el checkpoint es de otro departamento");
                    }
                }
                else
                {
                    objCatCheck = objCatCheckD.BuscarId(lEventos.IDCheckPoint, IdOficina, lEventos.IDReferencia);
                }

                if (objCatCheck.Duplicidad == false && await BuscaPrescedencia(lEventos.IDReferencia, lEventos.IDCheckPoint))
                {
                    throw new Exception("La configuración del checkpoint no permite ingresarlo en más de una ocasión.");
                }

                var asignarObj = new AsignarGuias(_configuration);
                var objRefe = new Referencias();
                var objRefeD = new ReferenciasRepository(_configuration);
                objRefe = objRefeD.Buscar(lEventos.IDReferencia, IDDatosDeEmpresa);

                if (objRefe == null)
                {
                    throw new ArgumentException("No existe el numero de referencia");
                }
                objRespuesta = await asignarObj.Validaciones(objCatCheck, objRefe, lEventos.IDUsuario, Extraordinario, Observaciones, IDDatosDeEmpresa);


                departamentoDestino = objCatCheck.IdDepartamentoDestino;

                var objAsignaD = new AsignacionDeGuiasRepository(_configuration);
                var usuariosAsignados = new int[3];

                if (departamentoActual == departamentoDestino)
                {
                    departamentoActual = 0;
                    // departamentoDestino = 0
                }

                if (departamentoDestino > 0)
                {

                    usuariosAsignados = await objAsignaD.Asignar(lEventos.IDReferencia, departamentoDestino, lEventos.IDUsuario, lEventos.IDCheckPoint);
                }

                var objRespIds = new AsignarGuiasRespuesta();

                objRespIds = await InsertarConAsignacion(lEventos, departamentoActual, departamentoDestino, objCatCheck.Automatico, usuariosAsignados, Observaciones);

                objRespuesta.IdEvento = objRespIds.IdEvento;
                objRespuesta.IdUsuarioAsignado = objRespIds.IdUsuarioAsignado;

            }

            return objRespuesta;
        }
        public List<ControldeEventos> ObtenerUltimosEventos(string GuiaHouse, int IDDatosDeEmpresa, int Limit)
        {
            var controlDeEventos = new List<ControldeEventos>();
            var objReferencia = new Referencias();
            var objReferenciaD = new ReferenciasRepository(_configuration);

            objReferencia = objReferenciaD.Buscar(GuiaHouse, IDDatosDeEmpresa);

            if (!(objReferencia == null))
            {
                SqlDataReader dataReader;
                var cmd = new SqlCommand();
                SqlParameter @param;

                using (var cn = new SqlConnection(sConexion))
                {
                    try
                    {
                        cn.Open();

                        cmd.Connection = cn;
                        cmd.CommandText = "NET_ULTIMOS_EVENTOS";
                        cmd.CommandType = CommandType.StoredProcedure;

                        @param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int);
                        @param.Value = objReferencia.IDReferencia;

                        @param = cmd.Parameters.Add("@Limit", SqlDbType.Int);
                        @param.Value = Limit;

                        dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                var item = new ControldeEventos();
                                item.IDEvento = Convert.ToInt32(dataReader["IDEvento"]);
                                item.IDCheckPoint = Convert.ToInt32(dataReader["IDCheckPoint"]);
                                if (dataReader["IDDepartamento"] != null)
                                {
                                    item.IdDepartamento = Convert.ToInt32(dataReader["IDDepartamento"]);
                                }
                                item.Descripcion = dataReader["Descripcion"].ToString();
                                item.FechaEvento = Convert.ToDateTime(dataReader["FechaEvento"]);
                                item.Observacion = dataReader["Observacion"].ToString();
                                item.ObservacionCompleta = dataReader["ObservacionCompleta"].ToString();
                                item.Color = dataReader["Color"].ToString();
                                item.NombreUsuario = dataReader["NombreUsuario"].ToString();
                                controlDeEventos.Add(item);
                            }
                        }
                        else
                        {
                            controlDeEventos = null;
                        }

                        dataReader.Close();
                        cn.Close();
                        cn.Dispose();
                        cmd.Parameters.Clear();
                    }


                    catch (Exception ex)
                    {
                        cn.Close();
                        // SqlConnection.ClearPool(cn)
                        cn.Dispose();

                        throw new Exception(ex.Message.ToString() + "NET_ULTIMOS_EVENTOS");
                    }
                    return controlDeEventos;
                }
            }
            else
            {
                throw new Exception("No se encontró la referencia: " + GuiaHouse);
            }

        }

        public int BuscaDepartamentoAnterior(int IdReferencia)
        {

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;
            var idDepartamento = default(int);

            cn.ConnectionString = sConexion;
            cn.Open();

            try
            {
                // Asigno el Stored Procedure
                cmd.CommandText = "NET_SEARCH_DEPARTAMENTO_ANTERIOR_EN_REFERENCIA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @IdReferencia INT ,
                @param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
                @param.Value = IdReferencia;


                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    idDepartamento = Convert.ToInt32(dr["idDepartamento"]);

                }
                cmd.Parameters.Clear();
            }

            catch (Exception ex)
            {
                idDepartamento = 0;
                cmd.Parameters.Clear();
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message);
            }

            dr.Close();
            cmd.Parameters.Clear();
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return idDepartamento;
        }
        public async Task<int> InsertarEvento(ControldeEventos lEventos, int idDepartamento, int IdOficina, int IDDatosDeEmpresa, bool TieneFechaEvento = false, string UUID = null, string Model = null)
        {

            var objRespuesta = new AsignarGuiasRespuesta();

            string obs = "";

            if (TieneFechaEvento)
            {
                var fechaServidor = DateTime.Now;

                if (lEventos.FechaEvento > fechaServidor)
                {
                    TieneFechaEvento = false;
                    obs = "Se detectó hora mayor a la del servidor, tomamos la hora del servidor.";
                    if (UUID is not null & Model is not null)
                    {
                        obs += " Modelo: " + Model + ", UUID: " + UUID;
                    }
                }
            }

            objRespuesta = await InsertarEvento(lEventos, idDepartamento, IdOficina, false, obs, IDDatosDeEmpresa, TieneFechaEvento);
            return objRespuesta.IdEvento;
        }
        public async Task<AsignarGuiasRespuesta> InsertarEvento(ControldeEventos lEventos, int idDepartamento, int IdOficina, bool Extraordinario, string Observaciones, int IDDatosDeEmpresa, bool TieneFechaEvento = false)
        {

            var objRespuesta = new AsignarGuiasRespuesta();

            var objCatCheck = new CatalogodeCheckPoints();
            var objCatCheckD = new CatalogodeCheckPointsRepository(_configuration);
            var CntlEvD = new ControldeEventosRepository(_configuration);

            if (Observaciones is not null)
            {
                Observaciones = Observaciones.Trim();
            }
            else
            {
                Observaciones = "";

            }

            objCatCheck = objCatCheckD.Buscar(lEventos.IDCheckPoint, IdOficina);
            if (objCatCheck == null)
            {
                throw new ArgumentException("El checkpoint no esta configurado " + lEventos.IDCheckPoint + " Oficina " + IdOficina);
            }

            if (objCatCheck.DepOrigen)
            {

                int DepartamentoAnterior;
                DepartamentoAnterior = BuscaDepartamentoAnterior(lEventos.IDReferencia);

                objCatCheck.IdDepartamentoDestino = DepartamentoAnterior;
            }

            if (objCatCheck.ObservacionObligatoria)
            {
                if (Observaciones.Trim().Length < 15)
                {
                    throw new ArgumentException("Es Necesario insertar una observación mas especifica");
                }
            }

            if (objCatCheck.ListaDeIDPrecedencias1 == null)
            {
                objCatCheck.PrecedenciaObligatoria = false;
            }
            // Cambie a que respete las prescedencias IVBM

            if (objCatCheck.PrecedenciaObligatoria)
            {

                if (objCatCheck.ListaDeIDPrecedencias1.Count == 0 || await ExistePrecedencia(lEventos.IDReferencia, objCatCheck.ListaDeIDPrecedencias1))
                {
                }
                else
                {
                    var LstPre = new List<string>();
                    LstPre = await Prescedencias(objCatCheck.ListaDeIDPrecedencias1, IdOficina);
                    string cad = "";

                    foreach (string item in LstPre)
                        cad = cad + " | " + item;

                    throw new Exception("Necesita Checkpoint de precedencia : " + cad);
                }
            }

            if (objCatCheck.TipoDeEvento == 4) // Informativos
            {

                objRespuesta.IdEvento = CntlEvD.Insertar(lEventos, objCatCheck.Duplicidad, objCatCheck.Automatico, Observaciones, TieneFechaEvento);
            }
            else
            {
                int departamentoActual = 0;
                int departamentoDestino = 0;
                departamentoActual = await BuscaDepartamentoActual(lEventos.IDReferencia, IdOficina);

                if (departamentoActual > 0)
                {
                    objCatCheck = await objCatCheckD.BuscarPorDepto(lEventos.IDCheckPoint, IdOficina, departamentoActual, lEventos.IDReferencia);

                    if (objCatCheck == null)
                    {
                        throw new Exception("La referencia está asignada al departamento " + departamentoActual.ToString() + " y el checkpoint es de otro departamento");
                    }
                }
                else
                {
                    objCatCheck = objCatCheckD.Buscar(lEventos.IDCheckPoint, IdOficina);
                }

                if (objCatCheck.Duplicidad == false && await BuscaPrescedencia(lEventos.IDReferencia, lEventos.IDCheckPoint))
                {
                    throw new Exception("La configuración del checkpoint no permite ingresarlo en más de una ocasión.");
                }

                var asignarObj = new AsignarGuias(_configuration);
                var objRefe = new Referencias();
                var objRefeD = new ReferenciasRepository(_configuration);
                objRefe = objRefeD.Buscar(lEventos.IDReferencia, IDDatosDeEmpresa);

                if (objRefe == null)
                {
                    throw new ArgumentException("No existe el numero de referencia");
                }
                objRespuesta = await asignarObj.Validaciones(objCatCheck, objRefe, lEventos.IDUsuario, Extraordinario, Observaciones, IDDatosDeEmpresa);


                departamentoDestino = objCatCheck.IdDepartamentoDestino;

                var objAsignaD = new AsignacionDeGuiasRepository(_configuration);
                var usuariosAsignados = new int[3];

                if (departamentoActual == departamentoDestino)
                {
                    departamentoActual = 0;
                    // departamentoDestino = 0
                }

                if (departamentoDestino > 0)
                {
                    usuariosAsignados = await objAsignaD.Asignar(lEventos.IDReferencia, departamentoDestino, lEventos.IDUsuario, lEventos.IDCheckPoint);
                }

                var objRespIds = new AsignarGuiasRespuesta();

                objRespIds = await InsertarConAsignacion(lEventos, departamentoActual, departamentoDestino, objCatCheck.Automatico, usuariosAsignados, Observaciones);

                objRespuesta.IdEvento = objRespIds.IdEvento;
                objRespuesta.IdUsuarioAsignado = objRespIds.IdUsuarioAsignado;

            }

            return objRespuesta;
        }
        public async Task<int> InsertarEventoPocket(ControldeEventos lEventos, int idDepartamento, int IdOficina, int IDDatosDeEmpresa)
        {

            var objRespuesta = new AsignarGuiasRespuesta();

            objRespuesta = await InsertarEventoPocket(lEventos, idDepartamento, IdOficina, false, "", IDDatosDeEmpresa);
            return objRespuesta.IdEvento;
        }

        public async Task<AsignarGuiasRespuesta> InsertarEventoPocket(ControldeEventos lEventos, int idDepartamento, int IdOficina, bool Extraordinario, string Observaciones, int IDDatosDeEmpresa)
        {

            var objRespuesta = new AsignarGuiasRespuesta();

            var objCatCheck = new CatalogodeCheckPoints();
            var objCatCheckD = new CatalogodeCheckPointsRepository(_configuration);
            var CntlEvD = new ControldeEventosRepository(_configuration);

            if (Observaciones is not null)
            {
                Observaciones = Observaciones.Trim();
            }
            else
            {
                Observaciones = "";

            }

            objCatCheck = objCatCheckD.Buscar(lEventos.IDCheckPoint, IdOficina);
            if (objCatCheck == null)
            {
                throw new ArgumentException("El checkpoint no esta configurado " + lEventos.IDCheckPoint + " Oficina " + IdOficina);
            }

            // SI DEBO TOMAR EL DEPARTAMENTO DEL POR EL CUAL LLEGO A ESTA AREA
            if (objCatCheck.DepOrigen)
            {

                int DepartamentoAnterior;
                DepartamentoAnterior = BuscaDepartamentoAnterior(lEventos.IDReferencia);

                objCatCheck.IdDepartamentoDestino = DepartamentoAnterior;
            }

            if (objCatCheck.ObservacionObligatoria)
            {
                if (Observaciones.Trim().Length < 15)
                {
                    throw new ArgumentException("Es Necesario insertar una observación mas especifica");
                }
            }

            if (objCatCheck.ListaDeIDPrecedencias1 == null)
            {
                objCatCheck.PrecedenciaObligatoria = false;
            }
            // Cambie a que respete las prescedencias IVBM
            if (objCatCheck.PrecedenciaObligatoria)
            {

                if (objCatCheck.ListaDeIDPrecedencias1.Count == 0 || await ExistePrecedencia(lEventos.IDReferencia, objCatCheck.ListaDeIDPrecedencias1))
                {
                }
                // objRespuesta.IdEvento = CntlEvD.Insertar(lEventos, objCatCheck.Duplicidad, objCatCheck.Automatico, Observaciones)
                else
                {
                    var LstPre = new List<string>();
                    LstPre = await Prescedencias(objCatCheck.ListaDeIDPrecedencias1, IdOficina);
                    string cad = "";

                    foreach (string item in LstPre)
                        cad = cad + " | " + item;

                    throw new Exception("Necesita Checkpoint de precedencia : " + cad);
                }
            }

            if (objCatCheck.TipoDeEvento == 4)
            {
                objRespuesta.IdEvento = await CntlEvD.Insertar(lEventos, objCatCheck.Duplicidad, objCatCheck.Automatico, Observaciones);
            }
            else
            {
                int departamentoActual = 0;
                int departamentoDestino = 0;
                departamentoActual = await BuscaDepartamentoActual(lEventos.IDReferencia, IdOficina);
                if (departamentoActual == 0)
                {
                    departamentoActual = idDepartamento;
                }
                if (departamentoActual > 0)
                {
                    objCatCheck = await objCatCheckD.BuscarPorDepto(lEventos.IDCheckPoint, IdOficina, departamentoActual, lEventos.IDReferencia);

                    if (objCatCheck == null)
                    {
                        throw new Exception("La referencia está asignada al departamento " + departamentoActual.ToString() + " y el checkpoint es de otro departamento");
                    }
                }
                else
                {
                    objCatCheck = objCatCheckD.Buscar(lEventos.IDCheckPoint, IdOficina);
                }

                if (objCatCheck.Duplicidad == false && await BuscaPrescedencia(lEventos.IDReferencia, lEventos.IDCheckPoint))
                {
                    throw new Exception("La configuración del checkpoint no permite ingresarlo en más de una ocasión.");
                }

                var asignarObj = new AsignarGuias(_configuration);
                var objRefe = new Referencias();
                var objRefeD = new ReferenciasRepository(_configuration);
                objRefe = objRefeD.Buscar(lEventos.IDReferencia, IDDatosDeEmpresa);

                if (objRefe == null)
                {
                    throw new ArgumentException("No existe el numero de referencia");
                }
                objRespuesta = await asignarObj.Validaciones(objCatCheck, objRefe, lEventos.IDUsuario, Extraordinario, Observaciones, IDDatosDeEmpresa);


                departamentoDestino = objCatCheck.IdDepartamentoDestino;

                var objAsignaD = new AsignacionDeGuiasRepository(_configuration);
                int[] usuariosAsignados = new int[] { 0, 0, 0 };

                if (departamentoActual == departamentoDestino)
                {
                    departamentoActual = 0;
                    // departamentoDestino = 0
                }

                if (departamentoDestino > 0)
                {
                    usuariosAsignados = await objAsignaD.Asignar(lEventos.IDReferencia, departamentoDestino, lEventos.IDUsuario, lEventos.IDCheckPoint);
                }

                var objRespIds = new AsignarGuiasRespuesta();

                objRespIds = await InsertarConAsignacion(lEventos, departamentoActual, departamentoDestino, objCatCheck.Automatico, usuariosAsignados, Observaciones);

                objRespuesta.IdEvento = objRespIds.IdEvento;
                objRespuesta.IdUsuarioAsignado = objRespIds.IdUsuarioAsignado;

            }

            return objRespuesta;
        }
        public async Task<AsignarGuiasRespuesta> InsertarEventoAsync(ControldeEventos lEventos)
        {
            AsignarGuiasRespuesta objRespuesta = new();
            CatalogoDeUsuarios objUsuario = new();
            CatalogoDeUsuariosRepository objUsuarioD = new(_configuration);
            objUsuario = objUsuarioD.BuscarPorId(lEventos.IDUsuario);
            if (objUsuario == null)
            {
                throw new ArgumentException("Error: no se encontro en el usuario que esta insertando");
            }



            Referencias objRefe = new();
            ReferenciasRepository objRefeD = new(_configuration);
            objRefe = objRefeD.Buscar(lEventos.IDReferencia, objUsuario.IDDatosDeEmpresa);

            if (objRefe == null)
            {
                throw new ArgumentException("Error: no se encontro la referencia a la que se quiere agregar el checkpoint");
            }

            string RespuestaJCJF = string.Empty;
            //Previo solicitado a Almacen
            if (objRefe.IDDatosDeEmpresa == 1)
            {
                //ANGEL CUBITS 28-03-2024

                if (objRefe.AduanaEntrada == "850" && objRefe.IdOficina == 2)
                {
                    objRefe.IdOficina = 24;
                    objRefeD.Modificar(objRefe);
                }

                if (objRefe.IdOficina == 24)
                {
                    if (lEventos.IDCheckPoint == 294 || lEventos.IDCheckPoint == 666)
                    {

                        try
                        {
                            wsJCJFSolicitarPrevioRepository objJCJF = new wsJCJFSolicitarPrevioRepository(_configuration);
                            string Respuesta = await objJCJF.fSolicitarPrevioAsync(objRefe.NumeroDeReferencia);
                            lEventos.Observaciones = lEventos.Observaciones + " WebAPi JCJF: " + Respuesta;
                        }
                        catch (Exception ex)
                        {
                            lEventos.Observaciones = lEventos.Observaciones + " WebAPi JCJF: " + ex.Message.Trim();
                            RespuestaJCJF = ex.Message.Trim();
                        }
                    }
                }

            }

             if (RespuestaJCJF.Length>0)
                throw new ArgumentException("Error envío JCJF: " + RespuestaJCJF.Trim());

            objRespuesta = await InsertarEvento1(lEventos, objUsuario.IdDepartamento, objRefe.IdOficina, false, objRefe.IDDatosDeEmpresa);
            
          

            return objRespuesta;
        }

        public int BuscaGrupoClasi(int IdReferencia)
        {

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;
            var idUsuario = default(int);

            cn.ConnectionString = sConexion;
            cn.Open();

            try
            {
                // Asigno el Stored Procedure
                cmd.CommandText = "NET_SEARCH_ULTIMO_GRUPO_CLASI";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @IdReferencia INT ,
                @param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
                @param.Value = IdReferencia;


                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    idUsuario = Convert.ToInt32(dr["idGrupo"]);

                }
                cmd.Parameters.Clear();
            }

            catch (Exception ex)
            {
                idUsuario = 0;
                cmd.Parameters.Clear();
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message);
            }

            dr.Close();
            cmd.Parameters.Clear();
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return idUsuario;
        }


        public async Task<AsignarGuiasRespuesta> InsertarEvento1(ControldeEventos lEventos, int idDepartamento
                            , int IdOficina, bool Extraordinario, int IDDatosDeEmpresa)
        {
            AsignarGuiasRespuesta objRespuesta = new();

            try
            {
                CatalogodeCheckPoints objCatCheck = new();
                CatalogodeCheckPointsRepository objCatCheckD = new(_configuration);

                ControldeEventosRepository CntlEvD = new(_configuration);
                if (lEventos.Observaciones == null)
                {
                    lEventos.Observaciones = "";
                }

                objCatCheck = objCatCheckD.Buscar(lEventos.IDCheckPoint, IdOficina);


                if (objCatCheck == null)
                {
                    throw new ArgumentException("El checkpoint no esta configurado " + lEventos.IDCheckPoint + " Oficina " + IdOficina);
                }

                ReferenciasRepository objRefeD = new(_configuration);




                //SI DEBO TOMAR EL DEPARTAMENTO DEL POR EL CUAL LLEGO A ESTA AREA
                if (objCatCheck.DepOrigen == true)
                {
                    AsignarGuiasRepository objCntrol = new(_configuration);

                    int DepartamentoAnterior = await objCntrol.BuscaDepartamentoAnterior(lEventos.IDReferencia);
                    objCatCheck.IdDepartamentoDestino = DepartamentoAnterior;
                }

                if (objCatCheck.ObservacionObligatoria == true)
                {
                    if (lEventos.Observaciones.Trim().Length < 15)
                    {
                        throw new ArgumentException("Es Necesario insertar una observación mas especifica");
                    }
                }

                if (objCatCheck.PrecedenciaObligatoria)
                {

                    if (objCatCheck.ListaDeIDPrecedencias1.Count == 0 || await ExistePrecedencia(lEventos.IDReferencia, objCatCheck.ListaDeIDPrecedencias1))
                    {
                        //   objRespuesta.IdEvento = CntlEvD.Insertar(lEventos, objCatCheck.Duplicidad, objCatCheck.Automatico, Observaciones);
                    }
                    else
                    {
                        List<string> LstPre = new List<string>();
                        LstPre = await Prescedencias(objCatCheck.ListaDeIDPrecedencias1, IdOficina);
                        string cad = "";

                        foreach (string item in LstPre)
                            cad = cad + " | " + item;

                        throw new Exception("Necesita Check point de precedencia : " + cad);
                    }
                }
                Referencias objRefe = new();


                objRefe = objRefeD.Buscar(lEventos.IDReferencia, IDDatosDeEmpresa);
                if (objRefe == null)
                {
                    throw new Exception("No existe el número de referencia");
                }


                if (objCatCheck.TipoDeEvento == 4)
                {
                    objRespuesta.IdEvento = await CntlEvD.Insertar(lEventos, objCatCheck.Duplicidad, objCatCheck.Automatico, lEventos.Observaciones);

                    SaaioPedime objPedi = new SaaioPedime();
                    SaaioPedimeRepository objPediD = new(_configuration);
                    objPedi = objPediD.Buscar(objRefe.NumeroDeReferencia.Trim());
                    if (objPedi != null)
                    {
                        if (objPedi.NUM_PEDI != null)
                        {
                            objRespuesta.Pedimento = objPedi.NUM_PEDI;
                        }
                        objRespuesta.Patente = objPedi.PAT_AGEN;
                    }

                    objRespuesta.UsuarioAsignado = "CheckPoint Informativo";


                }
                else
                {
                    int departamentoActual = 0;
                    int departamentoDestino = 0;


                    departamentoActual = await BuscaDepartamentoActual(lEventos.IDReferencia, IdOficina);
                    if (departamentoActual == 0)
                    {
                        departamentoActual = idDepartamento;
                    }

                    if (departamentoActual != idDepartamento)
                    {
                        objCatCheck = await objCatCheckD.BuscarPorDepto(lEventos.IDCheckPoint, IdOficina, departamentoActual, lEventos.IDReferencia);

                        CatalogoDepartamentos objDep = new CatalogoDepartamentos();
                        CatalogoDepartamentosRepository objDepD = new CatalogoDepartamentosRepository(_configuration);
                        objDep = await objDepD.Buscar(departamentoActual);

                        if (objCatCheck == null)
                        {
                            throw new Exception("La referencia está asignada al departamento " + objDep.NombreDepartamento.Trim() + " y el checkpoint es de otro departamento");
                        }


                    }
                    else
                    {
                        objCatCheck = objCatCheckD.Buscar(lEventos.IDCheckPoint, IdOficina);
                    }

                    if ((objCatCheck.Duplicidad = false) && await BuscaPrescedencia(lEventos.IDReferencia, lEventos.IDCheckPoint))
                    {
                        throw new Exception("La configuración del checkpoint no permite ingresarlo en más de una ocasión.");
                    }

                    AsignarGuias asignarObj = new(_configuration);

                    objRespuesta = await asignarObj.Validaciones(objCatCheck, objRefe, lEventos.IDUsuario, Extraordinario, lEventos.Observaciones, objRefe.IDDatosDeEmpresa);

                    departamentoDestino = objCatCheck.IdDepartamentoDestino;

                    AsignacionDeGuiasRepository objAsignaD = new(_configuration);

                    int[] usuariosAsignados = new int[3];

                    if (departamentoActual == departamentoDestino)
                    {
                        departamentoActual = 0;

                    }

                    if (departamentoDestino > 0)
                    {
                        usuariosAsignados = await objAsignaD.Asignar(objRefe.IDReferencia, departamentoDestino, lEventos.IDUsuario, lEventos.IDCheckPoint);
                    }
                    else
                    { usuariosAsignados[2] = 0; }

                    AsignarGuiasRespuesta objRespIds = new();

                    objRespIds = await InsertarConAsignacion(lEventos, departamentoActual, departamentoDestino, objCatCheck.Automatico, usuariosAsignados, lEventos.Observaciones);

                    string nombre = string.Empty;
                    if (departamentoDestino == 0)
                    {
                        nombre = "";
                    }
                    else
                    {
                        CatalogoDeUsuarios objUsu = new();
                        CatalogoDeUsuariosRepository objUsuD = new(_configuration);
                        objUsu = objUsuD.BuscarPorId(objRespIds.IdUsuarioAsignado);
                        if (objUsu == null)
                        {
                            throw new Exception("Existe un error con el usuario asignado, favor de revisar configuración de usuarios disponibles!");
                        }
                        nombre = objUsu.Nombre.Trim();
                    }



                    objRespuesta.IdEvento = objRespIds.IdEvento;
                    objRespuesta.IdUsuarioAsignado = objRespIds.IdUsuarioAsignado;
                    objRespuesta.UsuarioAsignado = nombre;

                }
            }
            catch (Exception)
            {

                throw;
            }

            return objRespuesta;


        }


        public async Task<bool> BuscaPrescedencia(int IdReferencia, int IDPrescedencia)
        {
            bool Existe;

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_SEARCH_SI_EXISTEEVENTO_EN_REFERENCIA", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4).Value = Convert.ToInt32(IdReferencia);

                    cmd.Parameters.Add("@IDCheckPoint", SqlDbType.Int, 4).Value = Convert.ToInt32(IDPrescedencia);

                    using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                    if (dr.HasRows)
                        Existe = true;
                    else
                        Existe = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return Existe;
        }
        public async Task<bool> ExistePrecedencia(int IdReferencia, List<int> lst)
        {
            bool Existe;
            Existe = false;
            foreach (int item in lst)
            {
                if (await BuscaPrescedencia(IdReferencia, item))
                {
                    Existe = true;
                    break;
                }
            }
            return Existe;
        }
        protected Task<List<string>> Prescedencias(List<int> lst, int Idoficina)
        {
            List<string> lstPresc = new List<string>();
            CatalogodeCheckPoints ObjCat = new();
            CatalogodeCheckPointsRepository ObjCatD = new(_configuration);

            if (lst.Count == 0)
            {
                return Task.FromResult<List<string>>(null);

            }
            foreach (int item in lst)
            {
                ObjCat = ObjCatD.Buscar(item, Idoficina);
                if (ObjCat != null)
                    lstPresc.Add(ObjCat.Descripcion);
            }

            return Task.FromResult(lstPresc);
        }

        public async Task<DataTable> Cargar(int IdReferencia)
        {
            DataTable dtb = new DataTable();


            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_ControldeEventos", cn))
                {

                    cn.Open();

                    cmd.Parameters.Add("@IdReferencia", SqlDbType.Int).Value = IdReferencia;

                    cmd.CommandType = CommandType.StoredProcedure;

                    using SqlDataAdapter da = new(cmd);
                    await Task.Run(() => da.Fill(dtb));

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + "NET_LOAD_ControldeEventos");
            }

            return dtb;
        }

        public async Task<DataTable> CargarGlobal(int IdAnexos)
        {
            DataTable dtb = new DataTable();

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CONTROLDEEVENTOSCONSOL", cn))
                {

                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdAnexos", SqlDbType.Int).Value = IdAnexos;

                    using SqlDataAdapter da = new(cmd);
                    await Task.Run(() => da.Fill(dtb));

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + "NET_LOAD_ControldeEventos");
            }
            return dtb;
        }


        public async Task<DataTable> CargarCompleto(string GuiaHouse)
        {
            DataTable dtb = new DataTable();

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CONTROLDEEVENTOS_GUIAHOUSE", cn))
                {

                    cn.Open();

                    cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 15).Value = GuiaHouse;

                    cmd.CommandType = CommandType.StoredProcedure;

                    using SqlDataAdapter da = new(cmd);
                    await Task.Run(() => da.Fill(dtb));

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString() + "NET_LOAD_CONTROLDEEVENTOS_GUIAHOUSE");
            }

            return dtb;
        }
        public async Task<int> DeleteEvento(int IDEvento)
        {
            int id = 0;

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_DELETE_EVENTOSPORIDEVENTO", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDEvento", SqlDbType.Int).Value = IDEvento;

                    using (await cmd.ExecuteReaderAsync())
                    {
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + "NET_DELETE_EVENTOSPORIDEVENTO");
            }

            return id;
        }

        public async Task<ControldeEventos> BuscaEventoporDia(int IDReferencia, int IdCheckPoint)
        {
            ControldeEventos objEvento = new ControldeEventos();

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_SEARCH_EVENTOPORDIA", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDReferencia", SqlDbType.Int).Value = IDReferencia;
                    cmd.Parameters.Add("@IdCheckPoint", SqlDbType.Int).Value = IdCheckPoint;

                    using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objEvento.IDEvento = Convert.ToInt32(dr["IDEvento"]);
                        objEvento.IDCheckPoint = Convert.ToInt32(dr["IDCheckPoint"]);
                        objEvento.IDReferencia = Convert.ToInt32(dr["IDReferencia"]);
                        objEvento.IDUsuario = Convert.ToInt32(dr["IDUsuario"]);
                        objEvento.FechaEvento = Convert.ToDateTime(dr["FechaEvento"]);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objEvento;
        }

        public async Task<bool> BuscaSiExisteIDCheckpoint(int IDCheckpoint, string IDReferencia)
        {
            bool Existe;

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_SEARCH_SI_EXISTEEVENTO_EN_REFERENCIA", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCheckpoint", SqlDbType.Int).Value = IDCheckpoint;
                    cmd.Parameters.Add("@IdReferencia", SqlDbType.Int).Value = IDReferencia;

                    // Ejecuto el sp y obtengo el DataSet
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())

                        if (dr.HasRows)
                            Existe = true;
                        else
                            Existe = false;
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return Existe;
        }

        public async Task<int> CuentaEventos(int IDCheckpoint, int IDReferencia)
        {
            int vContar;

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_COUNT_CONTROLDEEVENTOS", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCheckpoint", SqlDbType.Int).Value = IDCheckpoint;
                    cmd.Parameters.Add("@IDReferencia", SqlDbType.Int).Value = IDReferencia;


                    using SqlDataReader dr = await cmd.ExecuteReaderAsync();

                    if (dr.HasRows)
                    {
                        dr.Read();
                        vContar = Convert.ToInt32(dr["Cuantos"]);
                    }
                    else
                        vContar = 0;
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return vContar;
        }
        public async Task<bool> BuscaRecepciondeGuias(string IDReferencia)
        {
            bool Existe;

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_SEARCH_EVENTOPORDIA_RECEPCIONDEGUIAS", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdReferencia", SqlDbType.Int).Value = IDReferencia;

                    using SqlDataReader dr = await cmd.ExecuteReaderAsync();

                    if (dr.HasRows)
                        Existe = true;
                    else
                        Existe = false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Existe;
        }

        public async Task<bool> BuscarArchivo(string RutaArchivo)
        {
            bool Existe;

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_SEARCH_FILE", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Ruta", SqlDbType.VarChar, 250).Value = RutaArchivo;
                    cmd.Parameters.Add("@Existe", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                    if (Convert.ToInt32(cmd.Parameters["@Existe"].Value) == 1)
                        Existe = true;
                    else
                        Existe = false;

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + "NET_SEARCH_FILE");
            }
            return Existe;
        }

        public async Task<DataTable> CargarPorUsurio(int IdReferencia, int IdUsuario)
        {
            DataTable dtb = new DataTable();

            using (SqlConnection cn = new SqlConnection(sConexion))
            using (SqlCommand cmd = new("NET_LOAD_CONTROLDEEVENTOS_USUARIO", cn))
            {
                try
                {
                    cn.Open();

                    cmd.Parameters.Add("@IdReferencia", SqlDbType.Int).Value = IdReferencia;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;

                    cmd.CommandType = CommandType.StoredProcedure;

                    using SqlDataAdapter da = new(cmd);
                    await Task.Run(() => da.Fill(dtb));

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString() + "NET_LOAD_CONTROLDEEVENTOS_USUARIO");
                }
            }

            return dtb;
        }

        public async Task<bool> ValidaPrecedencia(int idCheckPoint, int idReferencia, CatalogoDeUsuarios objUser)
        {
            ControldeEventosRepository objEventosD = new(_configuration);
            ControldeEventos lEventos = new(idCheckPoint, idReferencia, objUser.IdUsuario, Convert.ToDateTime("01/01/1900"));

            bool precede = false;
            try
            {
                CatalogodeCheckPoints objCatCheck = new();
                CatalogodeCheckPointsRepository objCatCheckD = new(_configuration);

                objCatCheck = objCatCheckD.Buscar(lEventos.IDCheckPoint, objUser.IdOficina);
                if (objCatCheck != null)
                {
                    if (objCatCheck.PrecedenciaObligatoria)
                    {
                        if (objCatCheck.ListaDeIDPrecedencias1 != null)
                        {
                            if (objCatCheck.ListaDeIDPrecedencias1.Count > 0)
                            {
                                if (await ExistePrecedencia(lEventos.IDReferencia, objCatCheck.ListaDeIDPrecedencias1) == true)
                                //ExistePrecedencia(lEventos.IDReferencia, objCatCheck.ListaDeIDPrecedencias1) != null
                                {
                                    List<string> LstPre = new List<string>();
                                    LstPre = await Prescedencias(objCatCheck.ListaDeIDPrecedencias1, objUser.IdOficina);
                                    string cad = "";

                                    foreach (string item in LstPre)
                                        cad = cad + " | " + item;
                                    precede = true;
                                    throw new Exception("Necesita Check point de precedencia : " + cad);
                                }
                            }
                        }
                    }
                }
                else
                    throw new Exception("No existe el check point que esta tratando de utilizar");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return precede;
        }

        protected async Task<int> Insertar(ControldeEventos lcontroldeeventos, bool Duplicidad, bool Automatico, string observaciones)
        {
            int id;

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_INSERT_ControldeEventosSinAsignacion", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDCheckPoint", SqlDbType.Int, 4).Value = lcontroldeeventos.IDCheckPoint;
                    cmd.Parameters.Add("@IDReferencia", SqlDbType.Int, 4).Value = lcontroldeeventos.IDReferencia;
                    cmd.Parameters.Add("@IDUsuario", SqlDbType.Int, 4).Value = lcontroldeeventos.IDUsuario;
                    cmd.Parameters.Add("@FechaEvento", SqlDbType.DateTime, 4).Value = lcontroldeeventos.FechaEvento;
                    cmd.Parameters.Add("@Duplica", SqlDbType.Bit).Value = Duplicidad;
                    cmd.Parameters.Add("@Observacion", SqlDbType.Text).Value = observaciones;
                    cmd.Parameters.Add("@Automatico", SqlDbType.Bit).Value = Automatico;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;



                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                            id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                        else
                            id = 0;

                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + "NET_INSERT_ControldeEventosSinAsignacion");
            }

            return id;
        }

        protected int Insertar(ControldeEventos lcontroldeeventos, bool Duplicidad, bool Automatico, string observaciones, bool TieneFechaEvento = false)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_INSERT_ControldeEventosSinAsignacion";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // ,@IDCheckPoint  int
            @param = cmd.Parameters.Add("@IDCheckPoint", SqlDbType.Int, 4);
            @param.Value = lcontroldeeventos.IDCheckPoint;

            // ,@IDReferencia  int
            @param = cmd.Parameters.Add("@IDReferencia", SqlDbType.Int, 4);
            @param.Value = lcontroldeeventos.IDReferencia;

            // ,@IDUsuario  int
            @param = cmd.Parameters.Add("@IDUsuario", SqlDbType.Int, 4);
            @param.Value = lcontroldeeventos.IDUsuario;

            // ,@FechaEvento  datetime
            @param = cmd.Parameters.Add("@FechaEvento", SqlDbType.DateTime, 4);
            @param.Value = lcontroldeeventos.FechaEvento;

            // ,@Duplica bit
            @param = cmd.Parameters.Add("@Duplica", SqlDbType.Bit);
            @param.Value = Duplicidad;

            // ,@Observacion  text
            @param = cmd.Parameters.Add("@Observacion", SqlDbType.Text);
            @param.Value = observaciones;

            // ,@Automatico bit
            @param = cmd.Parameters.Add("@Automatico", SqlDbType.Bit);
            @param.Value = Automatico;

            // ,@TieneFechaEvento  bit
            if (TieneFechaEvento)
            {
                @param = cmd.Parameters.Add("@TieneFechaEvento", SqlDbType.Bit);
                @param.Value = TieneFechaEvento;
            }

            @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            @param.Direction = ParameterDirection.Output;


            try
            {
                cmd.ExecuteNonQuery();
                if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                {
                    id = (int)cmd.Parameters["@newid_registro"].Value;
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
                throw new Exception(ex.Message.ToString() + "NET_INSERT_ControldeEventosSinAsignacion");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }

        public async Task<int> BuscaDepartamentoActual(int IdReferencia, int IdOficina)
        {
            int idDepartamento = 0;

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_SEARCH_DEPARTAMENTO_ACTUAL_EN_REFERENCIA", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4).Value = Convert.ToInt32(IdReferencia);
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = Convert.ToInt32(IdOficina);


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

        protected async Task<AsignarGuiasRespuesta> InsertarConAsignacion(ControldeEventos lcontroldeeventos, int idDepartamentoActual, int departamentoDestino, bool Automatico, int[] usuarios, string observaciones)
        {
            AsignarGuiasRespuesta objRespuesta = new AsignarGuiasRespuesta();

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_INSERT_ControldeEventosConAsignacionTipo", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    if (usuarios[0] == 0 && departamentoDestino != 0)
                        throw new Exception("No se logro asignar la guia a un usuario, favor  de revisar usuarios disponibles!");

                    cmd.Parameters.Add("@IDCheckPoint", SqlDbType.Int, 4).Value = lcontroldeeventos.IDCheckPoint;
                    cmd.Parameters.Add("@IDReferencia", SqlDbType.Int, 4).Value = lcontroldeeventos.IDReferencia;
                    cmd.Parameters.Add("@IDUsuario", SqlDbType.Int, 4).Value = lcontroldeeventos.IDUsuario;
                    cmd.Parameters.Add("@IDUsuarioAsignado", SqlDbType.Int, 4).Value = usuarios[0];
                    cmd.Parameters.Add("@IdDepartamentoActual", SqlDbType.Int, 4).Value = idDepartamentoActual;
                    cmd.Parameters.Add("@departamentoDestino", SqlDbType.Int, 4).Value = departamentoDestino;
                    cmd.Parameters.Add("@IDGrupo", SqlDbType.Int, 4).Value = usuarios[1];
                    cmd.Parameters.Add("@FechaEvento", SqlDbType.DateTime, 4).Value = lcontroldeeventos.FechaEvento;
                    cmd.Parameters.Add("@Automatico", SqlDbType.Bit).Value = Automatico;
                    cmd.Parameters.Add("@Observacion", SqlDbType.Text).Value = observaciones;
                    cmd.Parameters.Add("@TipodeAsignacion", SqlDbType.Int, 4).Value = usuarios[2];
                    cmd.Parameters.Add("@usuarioAsignado", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;


                    using (await cmd.ExecuteReaderAsync())
                        if (Convert.ToInt32(cmd.Parameters["@NEWID_REGISTRO"].Value) != -1)
                            objRespuesta.IdEvento = Convert.ToInt32(cmd.Parameters["@NEWID_REGISTRO"].Value);
                    if (Convert.ToInt32(cmd.Parameters["@usuarioAsignado"].Value) != -1)
                        objRespuesta.IdUsuarioAsignado = Convert.ToInt32(cmd.Parameters["@usuarioAsignado"].Value);




                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString() + "NET_INSERT_ControldeEventosConAsignacionTipo");
            }
            return objRespuesta;

        }


        public int CrearReferencia(int IDCheckPoint, string GuiaHouse, int IDUsuario)
        {
            int id = 0;
            try
            {

                CatalogoDeUsuarios objUsuario = new CatalogoDeUsuarios();
                CatalogoDeUsuariosRepository objUsuarioD = new CatalogoDeUsuariosRepository(_configuration);

                objUsuario = objUsuarioD.BuscarPorId(IDUsuario);

                if (objUsuario == null)
                {
                    throw new ArgumentException("No existe Usuario para dar de alta la referencia");
                }

                CatalogodeCheckPoints objCatCheck = new();
                CatalogodeCheckPointsRepository objCatCheckD = new(_configuration);

                ControldeEventosRepository CntlEvD = new(_configuration);
                objCatCheck = objCatCheckD.Buscar(IDCheckPoint, objUsuario.IdOficina);

                if (objCatCheck == null)
                {
                    throw new ArgumentException("El checkpoint no esta configurado " + IDCheckPoint + " Oficina " + objUsuario.IdOficina);
                }

                if (objCatCheck.CrearRef == true)
                {
                    Referencias objRefe = new();
                    ReferenciasRepository objRefeD = new(_configuration);



                    CustomsAlerts objCA = new CustomsAlerts();
                    CargaManifiestosRepository objCAFD = new CargaManifiestosRepository(_configuration);
                    objCA = objCAFD.BuscarIndividual(GuiaHouse.Trim());
                    if (objCA == null)
                    {
                        throw new ArgumentException("No existe información en manifiesto, para dar de alta la referencia.");
                    }



                    objRefe.NumeroDeReferencia = GuiaHouse;
                    objRefe.IdOficina = objUsuario.IdOficina;

                    CatalogoDeOficinas objOficina = new CatalogoDeOficinas();
                    CatalogoDeOficinasRepository objOficinaD = new CatalogoDeOficinasRepository(_configuration);
                    objOficina = objOficinaD.Buscar(objUsuario.IdOficina);
                    if (objOficina == null)
                    {
                        throw new ArgumentException("Hay un error en la configuración de la oficina");
                    }

                    objRefe.AduanaDespacho = objOficina.AduDesp;
                    objRefe.AduanaEntrada = objOficina.AduEntr;
                    objRefe.Patente = objOficina.PatenteDefault;
                    objRefe.FechaApertura = DateTime.Now;

                    int oper = 0;
                    if (objUsuario.Operacion == 0)
                    {
                        oper = 1;
                    }
                    else
                    {
                        oper = objUsuario.Operacion;
                    }
                    objRefe.Operacion = oper;
                    objRefe.IDDatosDeEmpresa = objOficina.IDDatosDeEmpresa;
                    objRefe.IdDuenoDeLaReferencia = IDUsuario;
                    objRefe.Subdivision = false;
                    objRefe.PendientePorRectificar = false;

                    int idCliente;
                    if (objCA.IdCliente == 0)
                    {
                        idCliente = 17169;
                    }
                    else
                    {
                        idCliente = objCA.IdCliente;
                    }

                    objRefe.IDCliente = idCliente;
                    objRefe.IdClienteDestinatario = 0;
                    objRefe.ReferenciaDestinatario = "";
                    objRefe.IdGrupo = 0;

                    id = objRefeD.Insertar(objRefe);


                }
                else
                {
                    throw new ArgumentException("No existe la referencia en Expertti");
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }

            return id;
        }

        public bool SendMailTurnado(string GuiaHouse, int idOficinaSalida, int idOficinaLlegada, int idReferencia)
        {
            bool result = false;
            SqlParameter @param;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_EMAIL_TRANSITO_GUIA";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            @param = cmd.Parameters.Add("@GUIA_HOUSE", SqlDbType.VarChar, 20);
            @param.Value = GuiaHouse;


            @param = cmd.Parameters.Add("@ID_OFICINA_SALIDA", SqlDbType.Int, 4);
            @param.Value = idOficinaSalida;


            @param = cmd.Parameters.Add("@ID_OFICINA_LLEGADA", SqlDbType.Int, 4);
            @param.Value = idOficinaLlegada;

            // ,@IDUsuarioAsignado  int
            @param = cmd.Parameters.Add("@ID_REFERENCIA", SqlDbType.Int, 4);
            @param.Value = idReferencia;

            try
            {
                cmd.ExecuteNonQuery();
                result = true;
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {

                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_EMAIL_TRANSITO_GUIA");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return result;
        }

        public async Task<List<AsignarGuiasRespuestaMasivo>> InsertarporIdDODAAsync(ControldeEventosDoda objMasivos)
        {
            List<AsignarGuiasRespuestaMasivo> lstRespuestas = new List<AsignarGuiasRespuestaMasivo>();

            try
            {
                ApiDHLControl objApiDHLControl = new(_configuration);
                List<JsonDetalleporDODA> lstDetDoda = new();
                lstDetDoda = objApiDHLControl.DetalleporDODA(objMasivos.idDODA);

                DodaRepository objDoda = new (_configuration);
                objDoda.DODA_Modulado(objMasivos.idDODA, objMasivos.IDCheckPoint, objMasivos.FechaEvento);

                foreach (JsonDetalleporDODA Referencia in lstDetDoda)
                {
                    try
                    {
                        ControldeEventosRepository objEventosD = new(_configuration);
                        Referencias objRefe = new();
                        Referencias objRefeBuscar = new();
                        ReferenciasRepository objRefeD = new(_configuration);

                        objRefeBuscar = objRefeD.Buscar(Referencia.NumeroDeReferencia.Trim(), objMasivos.IdDatosdeEmpresa);
                        if (objRefeBuscar == null)
                        {
                            objEventosD.CrearReferencia(objMasivos.IDCheckPoint, Referencia.NumeroDeReferencia.Trim(), objMasivos.IDUsuario);

                        }

                        objRefe = objRefeD.Buscar(Referencia.NumeroDeReferencia.Trim(), objMasivos.IdDatosdeEmpresa);

                        ControldeEventos objEventos = new()
                        {
                            IDCheckPoint = objMasivos.IDCheckPoint,
                            IDReferencia = objRefe.IDReferencia,
                            FechaEvento = objMasivos.FechaEvento,
                            IDUsuario = objMasivos.IDUsuario,
                            Observaciones = objMasivos.Observaciones.Trim()
                        };


                        AsignarGuiasRespuesta objRespuestaIndividual = new();

                        objRespuestaIndividual = await objEventosD.InsertarEventoAsync(objEventos);

                        //AsignarGuiasRespuestaMasivo objRespuesta = new();
                        //objRespuesta.IdReferencia = objRefe.IDReferencia;
                        //objRespuesta.Referencia = Referencia.NumeroDeReferencia.Trim();
                        //objRespuesta.Patente = objRespuestaIndividual.Patente;
                        //objRespuesta.Pedimento = objRespuestaIndividual.Pedimento;
                        //objRespuesta.UsuarioAsignado = objRespuestaIndividual.UsuarioAsignado;
                        //objRespuesta.Resultado = "CheckPoint OK";

                        //lstRespuestas.Add(objRespuesta);

                        if (objRefe.IdOficina == 24)
                        {
                            if (objMasivos.IDCheckPoint == 146)
                            {
                                objEventos.IDCheckPoint = 294;
                                objRespuestaIndividual = await objEventosD.InsertarEventoAsync(objEventos);
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        AsignarGuiasRespuestaMasivo objRespuesta = new();
                        objRespuesta.Referencia = Referencia.NumeroDeReferencia.Trim();
                        objRespuesta.Patente = "";
                        objRespuesta.Pedimento = "";
                        objRespuesta.Resultado = "Error: " + ex.Message.ToString();

                        lstRespuestas.Add(objRespuesta);
                        continue;
                    }
                }
               
            }
            catch (Exception error)
            {
                
                throw new Exception(error.Message.Trim());
            }
            return lstRespuestas;
        }
    }//Clase
}// NameSpace


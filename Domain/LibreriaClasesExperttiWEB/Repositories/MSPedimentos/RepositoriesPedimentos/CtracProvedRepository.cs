using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using System.Data;
using System.Data.SqlClient;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPedimentos
{
    public class CtracProvedRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CtracProvedRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public CtracProved Buscar(string MyCve_Pro)
        {
            CtracProved objCTRAC_PROVED = new();
            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_CTRAC_PROVED", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CVE_PRO", SqlDbType.VarChar, 6).Value = MyCve_Pro;

                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        dr.Read();
                        objCTRAC_PROVED.CVE_PRO = string.Format("{0}", dr["CVE_PRO"]);
                        objCTRAC_PROVED.NOM_PRO = string.Format("{0}", dr["NOM_PRO"]);
                        objCTRAC_PROVED.DIR_PRO = string.Format("{0}", dr["DIR_PRO"]);
                        objCTRAC_PROVED.POB_PRO = string.Format("{0}", dr["POB_PRO"]);
                        objCTRAC_PROVED.ZIP_PRO = string.Format("{0}", dr["ZIP_PRO"]);
                        objCTRAC_PROVED.TAX_PRO = string.Format("{0}", dr["TAX_PRO"]);
                        objCTRAC_PROVED.PAI_PRO = string.Format("{0}", dr["PAI_PRO"]);
                        objCTRAC_PROVED.CTA_PRO = string.Format("{0}", dr["CTA_PRO"]);
                        objCTRAC_PROVED.EFE_PRO = string.Format("{0}", dr["EFE_PRO"]);
                        objCTRAC_PROVED.NOI_PRO = string.Format("{0}", dr["NOI_PRO"]);
                        objCTRAC_PROVED.NOE_PRO = string.Format("{0}", dr["NOE_PRO"]);
                        objCTRAC_PROVED.VIN_PRO = string.Format("{0}", dr["VIN_PRO"]);
                        objCTRAC_PROVED.EFE_DESP = string.Format("{0}", dr["EFE_DESP"]);
                        objCTRAC_PROVED.TEL_PRO = string.Format("{0}", dr["TEL_PRO"]);
                        objCTRAC_PROVED.AFE_PREC = string.Format("{0}", dr["AFE_PREC"]);
                        objCTRAC_PROVED.CVE_PROC = string.Format("{0}", dr["CVE_PROC"]);
                        objCTRAC_PROVED.FEC_BAJA = Convert.ToDateTime(dr["FEC_BAJA"]);
                        objCTRAC_PROVED.INT_PRO = Convert.ToInt32(dr["INT_PRO"]);
                        objCTRAC_PROVED.EXP_CONF = string.Format("{0}", dr["EXP_CONF"]);
                        objCTRAC_PROVED.APE_PATE = string.Format("{0}", dr["APE_PATE"]);
                        objCTRAC_PROVED.APE_MATE = string.Format("{0}", dr["APE_MATE"]);
                        objCTRAC_PROVED.COL_PRO = string.Format("{0}", dr["COL_PRO"]);
                        objCTRAC_PROVED.LOC_PRO = string.Format("{0}", dr["LOC_PRO"]);
                        objCTRAC_PROVED.REFE_PRO = string.Format("{0}", dr["REFE_PRO"]);
                        objCTRAC_PROVED.NOM_COVE = string.Format("{0}", dr["NOM_COVE"]);
                        objCTRAC_PROVED.MUN_COVE = string.Format("{0}", dr["MUN_COVE"]);
                        objCTRAC_PROVED.MAIL_COVE = string.Format("{0}", dr["MAIL_COVE"]);
                        objCTRAC_PROVED.RUTA_USPPI = string.Format("{0}", dr["RUTA_USPPI"]);
                        objCTRAC_PROVED.TIP_OPER = string.Format("{0}", dr["TIP_OPER"]);
                        objCTRAC_PROVED.IDCLIENTE = Convert.ToInt32(dr["idcliente"]);
                        objCTRAC_PROVED.Ultimo = string.Format("{0}", dr["Ultimo"]);
                        objCTRAC_PROVED.Primero = string.Format("{0}", dr["Primero"]);
                        objCTRAC_PROVED.Siguiente = string.Format("{0}", dr["Siguiente"]);
                        objCTRAC_PROVED.Anterior = string.Format("{0}", dr["Anterior"]);
                    }
                    else
                        objCTRAC_PROVED = null/* TODO Change to default(_) if this is not a reference type */;
                    dr.Close();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objCTRAC_PROVED;
        }

        public int ReglasDeNegocio(CtracProved objCtracProved, bool PlNuevo)
        {
            int MyIdUsuarioManif = 0;
            string NomNotificador = "";
            int MyIdCliente = objCtracProved.IDCLIENTE;


            CatalogoDeClientesFormalesRepository CatalogoDeClientesFormalesRepository = new(_configuration);
            CatalogoDeClientesFormales ObjCatalogoClientes = CatalogoDeClientesFormalesRepository.Buscar(MyIdCliente);

            CatalogoDeUsuariosRepository CatalogoDeUsuariosRepository = new(_configuration);
            CatalogoDeUsuarios ObjNotificador = CatalogoDeUsuariosRepository.BuscarPorId(ObjCatalogoClientes.IDNotificador);

            if (ObjNotificador != null)
            {
                NomNotificador = ObjNotificador.Nombre;
            }
            else
            {
                NomNotificador = "Sin notificador asignado";
            }

            CtracProvedRepository CtracProvedRepository = new(_configuration);
            CtracProvedRepository.ValidacionesComponentes(objCtracProved);

            int idUsuario = objCtracProved.IDUSUARIO;
            string txtReferencia = objCtracProved.NumeroDeReferencia;

            CatalogoDeUsuarios GObjUsuario = CatalogoDeUsuariosRepository.BuscarPorId(idUsuario);

            switch (GObjUsuario.IdDepartamento)
            {
                case 21:
                case 36:
                case 38:
                    {
                        txtReferencia = "";
                        break;
                    }

                default:
                    {

                        //SaaioPedime objPedime = new();
                        SaaioPedimeRepository objReferenciaD = new(_configuration);
                        SaaioPedime objPedime = objReferenciaD.Buscar(txtReferencia);

                        if (objPedime == null)
                        {
                            throw new Exception("No existe la referencia en sus sistema de pedimentos.");
                        }

                        if (objPedime.FIR_ELEC != "")
                            throw new Exception("La referencia ya fue validada.");
                        break;
                    }
            }

            if (objCtracProved.Activo == true)
            {
                if (PlNuevo == true)
                {
                    objCtracProved.Estatus = 1;
                }
                else
                {
                    objCtracProved.Estatus = 2;
                }
            }
            else
            {
                objCtracProved.Estatus = 3;
            }

            switch (objCtracProved.myTipodeForma)
            {
                case 1:
                    {
                        objCtracProved.Tipo = 1;
                        objCtracProved.IMP_EXPO = "1";
                        break;
                    }

                case 2:
                    {
                        objCtracProved.Tipo = 2;
                        objCtracProved.IMP_EXPO = "2";
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            return MyIdUsuarioManif = ObjCatalogoClientes.IDNotificador;
        }

        public string Insertar(CtracProved lctracproved)
        {

            string MyClave = "";
            try
            {
                lctracproved.Activo = true;
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_INSERT_CTRAC_PROVED", con))
                {

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@NOM_PRO", SqlDbType.VarChar, 120).Value = lctracproved.NOM_PRO == null ? DBNull.Value : lctracproved.NOM_PRO;
                    cmd.Parameters.Add("@DIR_PRO", SqlDbType.VarChar, 80).Value = lctracproved.DIR_PRO == null ? DBNull.Value : lctracproved.DIR_PRO;
                    cmd.Parameters.Add("@POB_PRO", SqlDbType.VarChar, 80).Value = lctracproved.POB_PRO == null ? DBNull.Value : lctracproved.POB_PRO;
                    cmd.Parameters.Add("@ZIP_PRO", SqlDbType.VarChar, 10).Value = lctracproved.ZIP_PRO == null ? DBNull.Value : lctracproved.ZIP_PRO;
                    cmd.Parameters.Add("@TAX_PRO", SqlDbType.VarChar, 18).Value = lctracproved.TAX_PRO == null ? DBNull.Value : lctracproved.TAX_PRO;
                    cmd.Parameters.Add("@PAI_PRO", SqlDbType.VarChar, 3).Value = lctracproved.PAI_PRO == null ? DBNull.Value : lctracproved.PAI_PRO;
                    cmd.Parameters.Add("@CTA_PRO", SqlDbType.VarChar, 6).Value = lctracproved.CTA_PRO == null ? DBNull.Value : lctracproved.CTA_PRO;
                    cmd.Parameters.Add("@EFE_PRO", SqlDbType.VarChar, 2).Value = lctracproved.EFE_PRO == null ? DBNull.Value : lctracproved.EFE_PRO;
                    cmd.Parameters.Add("@NOI_PRO", SqlDbType.VarChar, 10).Value = lctracproved.NOI_PRO == null ? DBNull.Value : lctracproved.NOI_PRO;
                    cmd.Parameters.Add("@NOE_PRO", SqlDbType.VarChar, 10).Value = lctracproved.NOE_PRO == null ? DBNull.Value : lctracproved.NOE_PRO;
                    cmd.Parameters.Add("@VIN_PRO", SqlDbType.Char, 1).Value = lctracproved.VIN_PRO == null ? DBNull.Value : lctracproved.VIN_PRO;
                    cmd.Parameters.Add("@EFE_DESP", SqlDbType.VarChar, 2).Value = lctracproved.EFE_DESP == null ? DBNull.Value : lctracproved.EFE_DESP;
                    cmd.Parameters.Add("@TEL_PRO", SqlDbType.VarChar, 50).Value = lctracproved.TEL_PRO == null ? DBNull.Value : lctracproved.TEL_PRO;
                    cmd.Parameters.Add("@AFE_PREC", SqlDbType.Char, 1).Value = lctracproved.AFE_PREC == null ? DBNull.Value : lctracproved.AFE_PREC;
                    cmd.Parameters.Add("@CVE_PROC", SqlDbType.VarChar, 15).Value = lctracproved.CVE_PROC == null ? DBNull.Value : lctracproved.CVE_PROC;
                    cmd.Parameters.Add("@INT_PRO", SqlDbType.Int, 4).Value = lctracproved.INT_PRO == 0 ? DBNull.Value : lctracproved.INT_PRO;
                    cmd.Parameters.Add("@EXP_CONF", SqlDbType.VarChar, 50).Value = lctracproved.EXP_CONF == null ? DBNull.Value : lctracproved.EXP_CONF;
                    cmd.Parameters.Add("@APE_PATE", SqlDbType.VarChar, 200).Value = lctracproved.APE_PATE == null ? DBNull.Value : lctracproved.APE_PATE;
                    cmd.Parameters.Add("@APE_MATE", SqlDbType.VarChar, 200).Value = lctracproved.APE_MATE == null ? DBNull.Value : lctracproved.APE_MATE;
                    cmd.Parameters.Add("@COL_PRO", SqlDbType.VarChar, 120).Value = lctracproved.COL_PRO == null ? DBNull.Value : lctracproved.COL_PRO;
                    cmd.Parameters.Add("@LOC_PRO", SqlDbType.VarChar, 120).Value = lctracproved.LOC_PRO == null ? DBNull.Value : lctracproved.LOC_PRO;
                    cmd.Parameters.Add("@REFE_PRO", SqlDbType.VarChar, 250).Value = lctracproved.REFE_PRO == null ? DBNull.Value : lctracproved.REFE_PRO;
                    cmd.Parameters.Add("@NOM_COVE", SqlDbType.VarChar, 200).Value = lctracproved.NOM_COVE == null ? DBNull.Value : lctracproved.NOM_COVE;
                    cmd.Parameters.Add("@MUN_COVE", SqlDbType.VarChar, 120).Value = lctracproved.MUN_COVE == null ? DBNull.Value : lctracproved.MUN_COVE;
                    cmd.Parameters.Add("@MAIL_COVE", SqlDbType.VarChar, 60).Value = lctracproved.MAIL_COVE == null ? DBNull.Value : lctracproved.MAIL_COVE;
                    cmd.Parameters.Add("@RUTA_USPPI", SqlDbType.VarChar, 250).Value = lctracproved.RUTA_USPPI == null ? DBNull.Value : lctracproved.RUTA_USPPI;
                    cmd.Parameters.Add("@TIP_OPER", SqlDbType.VarChar, 15).Value = lctracproved.TIP_OPER == null ? DBNull.Value : lctracproved.TIP_OPER;
                    cmd.Parameters.Add("@Tipo", SqlDbType.Int).Value = lctracproved.Tipo == 0 ? DBNull.Value : lctracproved.Tipo;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = lctracproved.IDUSUARIO == 0 ? DBNull.Value : lctracproved.IDUSUARIO;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = lctracproved.IDCLIENTE == 0 ? DBNull.Value : lctracproved.IDCLIENTE;
                    cmd.Parameters.Add("@IMP_EXPO", SqlDbType.Char, 1).Value = lctracproved.IMP_EXPO == null ? DBNull.Value : lctracproved.IMP_EXPO;
                    cmd.Parameters.Add("@CVE_IMP", SqlDbType.VarChar, 6).Value = lctracproved.CVE_IMP == null ? DBNull.Value : lctracproved.CVE_IMP;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@myclave", SqlDbType.VarChar, 6).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Estatus", SqlDbType.SmallInt).Value = lctracproved.Estatus == 0 ? DBNull.Value : lctracproved.Estatus;


                    cmd.ExecuteNonQuery();
                    if (Convert.ToString(cmd.Parameters["@myclave"].Value) != "")
                        MyClave = Convert.ToString(cmd.Parameters["@myclave"].Value);
                    else
                        MyClave = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + "NET_INSERT_CTRAC_PROVED");
            }
            return MyClave;
        }

        public int Modificar(CtracProved lctracproved)
        {
            int id;

            try
            {
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_UPDATE_CTRAC_PROVED", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CVE_PRO", SqlDbType.VarChar, 6).Value = lctracproved.CVE_PRO;
                    cmd.Parameters.Add("@NOM_PRO", SqlDbType.VarChar, 120).Value = lctracproved.NOM_PRO == null ? DBNull.Value : lctracproved.NOM_PRO;
                    cmd.Parameters.Add("@DIR_PRO", SqlDbType.VarChar, 80).Value = lctracproved.DIR_PRO == null ? DBNull.Value : lctracproved.DIR_PRO;
                    cmd.Parameters.Add("@POB_PRO", SqlDbType.VarChar, 80).Value = lctracproved.POB_PRO == null ? DBNull.Value : lctracproved.POB_PRO;
                    cmd.Parameters.Add("@ZIP_PRO", SqlDbType.VarChar, 10).Value = lctracproved.ZIP_PRO == null ? DBNull.Value : lctracproved.ZIP_PRO;
                    cmd.Parameters.Add("@TAX_PRO", SqlDbType.VarChar, 18).Value = lctracproved.TAX_PRO == null ? DBNull.Value : lctracproved.TAX_PRO;
                    cmd.Parameters.Add("@PAI_PRO", SqlDbType.VarChar, 3).Value = lctracproved.PAI_PRO == null ? DBNull.Value : lctracproved.PAI_PRO;
                    cmd.Parameters.Add("@CTA_PRO", SqlDbType.VarChar, 6).Value = lctracproved.CTA_PRO == null ? DBNull.Value : lctracproved.CTA_PRO;
                    cmd.Parameters.Add("@EFE_PRO", SqlDbType.VarChar, 2).Value = lctracproved.EFE_PRO == null ? DBNull.Value : lctracproved.EFE_PRO;
                    cmd.Parameters.Add("@NOI_PRO", SqlDbType.VarChar, 10).Value = lctracproved.NOI_PRO == null ? DBNull.Value : lctracproved.NOI_PRO;
                    cmd.Parameters.Add("@NOE_PRO", SqlDbType.VarChar, 10).Value = lctracproved.NOE_PRO == null ? DBNull.Value : lctracproved.NOE_PRO;
                    cmd.Parameters.Add("@VIN_PRO", SqlDbType.Char, 1).Value = lctracproved.VIN_PRO == null ? DBNull.Value : lctracproved.VIN_PRO;
                    cmd.Parameters.Add("@EFE_DESP", SqlDbType.VarChar, 2).Value = lctracproved.EFE_DESP == null ? DBNull.Value : lctracproved.EFE_DESP;
                    cmd.Parameters.Add("@TEL_PRO", SqlDbType.VarChar, 50).Value = lctracproved.TEL_PRO == null ? DBNull.Value : lctracproved.TEL_PRO;
                    cmd.Parameters.Add("@AFE_PREC", SqlDbType.Char, 1).Value = lctracproved.AFE_PREC == null ? DBNull.Value : lctracproved.AFE_PREC;
                    cmd.Parameters.Add("@CVE_PROC", SqlDbType.VarChar, 15).Value = lctracproved.CVE_PROC == null ? DBNull.Value : lctracproved.CVE_PROC;
                    //cmd.Parameters.Add("@FEC_BAJA", SqlDbType.DateTime).Value = If(IsNothing(lctracproved.FEC_BAJA), DBNull.Value, lctracproved.FEC_BAJA)
                    cmd.Parameters.Add("@INT_PRO", SqlDbType.Int, 4).Value = lctracproved.INT_PRO == 0 ? DBNull.Value : lctracproved.INT_PRO;
                    cmd.Parameters.Add("@EXP_CONF", SqlDbType.VarChar, 50).Value = lctracproved.EXP_CONF == null ? DBNull.Value : lctracproved.EXP_CONF;
                    cmd.Parameters.Add("@APE_PATE", SqlDbType.VarChar, 200).Value = lctracproved.APE_PATE == null ? DBNull.Value : lctracproved.APE_PATE;
                    cmd.Parameters.Add("@APE_MATE", SqlDbType.VarChar, 200).Value = lctracproved.APE_MATE == null ? DBNull.Value : lctracproved.APE_MATE;
                    cmd.Parameters.Add("@COL_PRO", SqlDbType.VarChar, 120).Value = lctracproved.COL_PRO == null ? DBNull.Value : lctracproved.COL_PRO;
                    cmd.Parameters.Add("@LOC_PRO", SqlDbType.VarChar, 120).Value = lctracproved.LOC_PRO == null ? DBNull.Value : lctracproved.LOC_PRO;
                    cmd.Parameters.Add("@REFE_PRO", SqlDbType.VarChar, 250).Value = lctracproved.REFE_PRO == null ? DBNull.Value : lctracproved.REFE_PRO;
                    cmd.Parameters.Add("@NOM_COVE", SqlDbType.VarChar, 200).Value = lctracproved.NOM_COVE == null ? DBNull.Value : lctracproved.NOM_COVE;
                    cmd.Parameters.Add("@MUN_COVE", SqlDbType.VarChar, 120).Value = lctracproved.MUN_COVE == null ? DBNull.Value : lctracproved.MUN_COVE;
                    cmd.Parameters.Add("@MAIL_COVE", SqlDbType.VarChar, 60).Value = lctracproved.MAIL_COVE == null ? DBNull.Value : lctracproved.MAIL_COVE;
                    cmd.Parameters.Add("@RUTA_USPPI", SqlDbType.VarChar, 250).Value = lctracproved.RUTA_USPPI == null ? DBNull.Value : lctracproved.RUTA_USPPI;
                    cmd.Parameters.Add("@TIP_OPER", SqlDbType.VarChar, 15).Value = lctracproved.TIP_OPER == null ? DBNull.Value : lctracproved.TIP_OPER;
                    cmd.Parameters.Add("@Tipo", SqlDbType.Int).Value = lctracproved.Tipo == 0 ? DBNull.Value : lctracproved.Tipo;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = lctracproved.IDUSUARIO == 0 ? DBNull.Value : lctracproved.IDUSUARIO;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = lctracproved.IDCLIENTE == 0 ? DBNull.Value : lctracproved.IDCLIENTE;
                    //cmd.Parameters.Add("@IMP_EXPO", SqlDbType.Char, 1).Value = If(IsNothing(lctracproved.IMP_EXPO), DBNull.Value, lctracproved.IMP_EXPO);
                    //cmd.Parameters.Add("@CVE_IMP", SqlDbType.VarChar, 6).Value = If(IsNothing(lctracproved.CVE_IMP), DBNull.Value, lctracproved.CVE_IMP);
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Estatus", SqlDbType.SmallInt).Value = lctracproved.Estatus;
                    cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = lctracproved.Activo;
                    //cmd.Parameters.Add("@NombreCliente", SqlDbType.VarChar, 60).Value = If(IsNothing(lctracproved.NombreCliente), DBNull.Value, lctracproved.NombreCliente);

                    cmd.ExecuteNonQuery();
                    if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                    else
                        id = 0;

                }
            }
            catch (Exception ex)
            {
                id = 0;
                throw new Exception(ex.Message.ToString());
            }
            return id;
        }


        public void ValidacionesComponentes(CtracProved objCtracProved)
        {

            if (objCtracProved.NOM_PRO == "")
                throw new Exception("Ingrese el Nombre del Provedor");

            if (objCtracProved.DIR_PRO == "")
                throw new Exception("Ingrese la Calle");

            //// If txtCiudadEstado.Text.Trim() = "" Then
            //// Throw New Exception("Ingrese la Ciudad/Estado")
            //// End If

            if (objCtracProved.TAX_PRO == "")
                throw new Exception("Ingrese el Tax ID");

            if (objCtracProved.NOE_PRO == "")
                throw new Exception("Ingrese el Numero Exterior");

            if (objCtracProved.ZIP_PRO == "")
                throw new Exception("Ingrese el Codigo Postal");

            if (objCtracProved.PAI_PRO == "")
                throw new Exception("Ingrese la Clave del Pais");

            if (objCtracProved.NOM_PAIS == "")
                throw new Exception("Ingrese el Nombre del Pais");

            if (objCtracProved.NombreCliente == "")
                throw new Exception("Ingrese el Nombre del Cliente");
        }

        //public int Modificar(string NumerodeReferencia, string CveProveedor)
        //{
        //    int id;
        //    SqlConnection cn = new SqlConnection();
        //    SqlCommand cmd = new SqlCommand();
        //    SqlParameter param;

        //    try
        //    {
        //        cmd.CommandText = "NET_UPDATE_SAAIO_FACTUR_PROVED";
        //        cmd.Connection = cn;
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        // ,@NUM_REFE  varchar
        //        param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
        //        param.Value = NumerodeReferencia;

        //        // ,@CVE_PROV  varchar    
        //        param = cmd.Parameters.Add("@CVE_PRO", SqlDbType.VarChar, 6);
        //        param.Value = CveProveedor;

        //        param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
        //        param.Direction = ParameterDirection.Output;

        //        cn.Open();

        //        cmd.ExecuteNonQuery();
        //        id = System.Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
        //        cmd.Parameters.Clear();
        //    }
        //    catch (Exception ex)
        //    {
        //        id = 0;
        //        cn.Close();

        //        cn.Dispose();
        //        throw new Exception(ex.Message.ToString() + " NET_INSERT_SAAIO_PEDIME");
        //    }

        //    cn.Close();           
        //    cn.Dispose();
        //    return id;
        //}

        public void altaRegistro(CtracProved ObjProveedModificado, int MyIdUsuarioManif, string MyCveProveed)
        {
            string RegistroCambios = "Datos nuevos.";
            bool hayCambios = true;
            int TipoDeOperacion = 0; // (Alta)

            if (hayCambios)
            {
                insertaBitacoraCambios(ObjProveedModificado.IDUSUARIO, MyIdUsuarioManif, ObjProveedModificado.IDCLIENTE, MyCveProveed, TipoDeOperacion, RegistroCambios);
            }
        }

        public void huboCambios(CtracProved objProveedModificado, int MyIdUsuarioManif)
        {
            bool myProveedorActivo = objProveedModificado.Activo;
            string RegistroCambios = "";
            bool hayCambios = false;
            bool ProveedorActivoBD;
            int TipoDeCambio = 2;


            CtracProvedRepository ObjBuscaProveedD = new(_configuration);
            CtracProved ObjBuscaProveedBD = ObjBuscaProveedD.Buscar(objProveedModificado.CVE_PRO);

            BitacoraDeProveedoresRepository ObjBitacoraProveedDB = new(_configuration);
            BitacoraDeProveedores ObjBitacoraProveed = ObjBitacoraProveedDB.Buscar(objProveedModificado.CVE_PRO);

            ProveedorActivoBD = ObjBitacoraProveed.Estatus == 1 | ObjBitacoraProveed.Estatus == 2;

            if (ProveedorActivoBD & !myProveedorActivo)
            {
                RegistroCambios = RegistroCambios + "El proveedor se ha desactivado. CrLf";
                hayCambios = true;
            }
            else if (!ProveedorActivoBD & myProveedorActivo)
            {
                RegistroCambios = RegistroCambios + "El proveedor se ha reactivado. CrLf";
                hayCambios = true;
            }

            if (ObjBuscaProveedBD.NOM_PRO != objProveedModificado.NOM_PRO)
            {
                RegistroCambios = RegistroCambios + "[NOM_PRO] {" + ObjBuscaProveedBD.NOM_PRO + "} cambió a {" + objProveedModificado.NOM_PRO + "} CrLf";
                hayCambios = true;
            }

            if (ObjBuscaProveedBD.APE_PATE != objProveedModificado.APE_PATE)
            {
                RegistroCambios = RegistroCambios + "[APE_PATE] {" + ObjBuscaProveedBD.APE_PATE + "} cambió a {" + objProveedModificado.APE_PATE + "} CrLf";
                hayCambios = true;
            }

            if (ObjBuscaProveedBD.APE_MATE != objProveedModificado.APE_MATE)
            {
                RegistroCambios = RegistroCambios + "[APE_MATE] {" + ObjBuscaProveedBD.APE_MATE + "} cambió a {" + objProveedModificado.APE_MATE + "} CrLf";
                hayCambios = true;
            }

            if (ObjBuscaProveedBD.DIR_PRO != objProveedModificado.DIR_PRO)
            {
                RegistroCambios = RegistroCambios + "[DIR_PRO] {" + ObjBuscaProveedBD.DIR_PRO + "} cambió a {" + objProveedModificado.DIR_PRO + "} CrLf";
                hayCambios = true;
            }

            if (ObjBuscaProveedBD.POB_PRO != objProveedModificado.POB_PRO)
            {
                RegistroCambios = RegistroCambios + "[POB_PRO] {" + ObjBuscaProveedBD.POB_PRO + "} cambió a {" + objProveedModificado.POB_PRO + "} CrLf";
                hayCambios = true;
            }

            if (ObjBuscaProveedBD.ZIP_PRO != objProveedModificado.ZIP_PRO)
            {
                RegistroCambios = RegistroCambios + "[ZIP_PRO] {" + ObjBuscaProveedBD.ZIP_PRO + "} cambió a {" + objProveedModificado.ZIP_PRO + "} CrLf";
                hayCambios = true;
            }

            if (ObjBuscaProveedBD.TAX_PRO != objProveedModificado.TAX_PRO)
            {
                RegistroCambios = RegistroCambios + "[TAX_PRO] {" + ObjBuscaProveedBD.TAX_PRO + "} cambió a {" + objProveedModificado.TAX_PRO + "} CrLf";
                hayCambios = true;
            }

            if (ObjBuscaProveedBD.PAI_PRO != objProveedModificado.PAI_PRO)
            {
                RegistroCambios = RegistroCambios + "[PAI_PRO] {" + ObjBuscaProveedBD.PAI_PRO + "} cambió a {" + objProveedModificado.PAI_PRO + "} CrLf";
                hayCambios = true;
            }

            if (ObjBuscaProveedBD.NOI_PRO != objProveedModificado.NOI_PRO)
            {
                RegistroCambios = RegistroCambios + "[NOI_PRO] {" + ObjBuscaProveedBD.NOI_PRO + "} cambió a {" + objProveedModificado.NOI_PRO + "} CrLf";
                hayCambios = true;
            }

            if (ObjBuscaProveedBD.NOE_PRO != objProveedModificado.NOE_PRO)
            {
                RegistroCambios = RegistroCambios + "[NOE_PRO] {" + ObjBuscaProveedBD.NOE_PRO + "} cambió a {" + objProveedModificado.NOE_PRO + "} CrLf";
                hayCambios = true;
            }

            if (ObjBuscaProveedBD.VIN_PRO != objProveedModificado.VIN_PRO)
            {
                RegistroCambios = RegistroCambios + "[VIN_PRO] {" + ObjBuscaProveedBD.VIN_PRO + "} cambió a {" + objProveedModificado.VIN_PRO + "} CrLf";
                hayCambios = true;
            }

            if (ObjBuscaProveedBD.TEL_PRO != objProveedModificado.TEL_PRO)
            {
                RegistroCambios = RegistroCambios + "[TEL_PRO] {" + ObjBuscaProveedBD.TEL_PRO + "} cambió a {" + objProveedModificado.TEL_PRO + "} CrLf";
                hayCambios = true;
            }

            if (ObjBuscaProveedBD.COL_PRO != objProveedModificado.COL_PRO)
            {
                RegistroCambios = RegistroCambios + "[COL_PRO] {" + ObjBuscaProveedBD.COL_PRO + "} cambió a {" + objProveedModificado.COL_PRO + "} CrLf";
                hayCambios = true;
            }

            if (ObjBuscaProveedBD.LOC_PRO != objProveedModificado.LOC_PRO)
            {
                RegistroCambios = RegistroCambios + "[LOC_PRO] {" + ObjBuscaProveedBD.LOC_PRO + "} cambió a {" + objProveedModificado.LOC_PRO + "} CrLf";
                hayCambios = true;
            }

            if (ObjBuscaProveedBD.MAIL_COVE != objProveedModificado.MAIL_COVE)
            {
                RegistroCambios = RegistroCambios + "[MAIL_COVE] {" + ObjBuscaProveedBD.MAIL_COVE + "} cambió a {" + objProveedModificado.MAIL_COVE + "} CrLf";
                hayCambios = true;
            }

            if (hayCambios)
                insertaBitacoraCambios(objProveedModificado.IDUSUARIO, MyIdUsuarioManif, objProveedModificado.IDCLIENTE, objProveedModificado.CVE_PRO, TipoDeCambio, RegistroCambios);
        }
        private void insertaBitacoraCambios(int MyIdUsuario, int MyIdNotificador, int MyIDCliente, string MyCveProveed, int TipoDeOperacion, string RegistroCambios)
        {

            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_INSERT_BITACORACAMBIOSPROVEEDORES", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDUSUARIO", SqlDbType.Int).Value = MyIdUsuario; // 2
                    cmd.Parameters.Add("@IDNOTIFICADOR", SqlDbType.Int).Value = MyIdNotificador;// 3
                    cmd.Parameters.Add("@IDCLIENTE", SqlDbType.Int).Value = MyIDCliente;// 4
                    cmd.Parameters.Add("@CVEPROVEEDOR", SqlDbType.VarChar, 6).Value = MyCveProveed;// 5
                    cmd.Parameters.Add("@TIPODECAMBIO", SqlDbType.Int).Value = TipoDeOperacion; // 
                    cmd.Parameters.Add("@CAMBIOS", SqlDbType.VarChar, 4096).Value = RegistroCambios;
                    cmd.Parameters.Add("@NEWID_REGISTRO", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

    }
}

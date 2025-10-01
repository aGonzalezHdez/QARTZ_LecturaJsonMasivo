using LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos;
using LibreriaClasesAPIExpertti.Entities.EntitiesGlobal;
using LibreriaClasesAPIExpertti.Entities.EntitiesRFC;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesRFC
{
    public class RFCRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        BucketsS3Repository _bucketRepo;
        public RFCRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
            _bucketRepo = new BucketsS3Repository(_configuration);
        }

        

        public bool ActualizarRFCDHL(string TaxIDImpo, int IdRiel, string GuiaHouse, string destinatarioEmail, string DatosContacto, out string mensaje)
        {
            bool result = false;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_CMF_CRF";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@TaxIDImpo", SqlDbType.VarChar, 15);
            @param.Value = TaxIDImpo;

            @param = cmd.Parameters.Add("@IdRiel", SqlDbType.Int);
            @param.Value = IdRiel;

            @param = cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 15);
            @param.Value = GuiaHouse;

            @param = cmd.Parameters.Add("@destinatarioEmail", SqlDbType.VarChar, 100);
            @param.Value = destinatarioEmail;

            @param = cmd.Parameters.Add("@DatosContacto", SqlDbType.VarChar, 50);
            @param.Value = DatosContacto;

            try
            {
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                result = true;
                mensaje = string.Empty;
            }
            catch (SqlException e)
            {
                result = false;
                cn.Close();
                cn.Dispose();
                mensaje = e.Message.ToString();
                throw new Exception(e.Message.ToString());

            }
            catch (Exception ex)
            {
                result = false;
                cn.Close();
                cn.Dispose();
                mensaje = ex.Message.ToString();
                throw new Exception(ex.Message.ToString());
            }
            cn.Close();
            cn.Dispose();

            return result;
        }



        

        public bool ActualizarRFCFedex(string rfc, string GuiaHouse, string nombre, string destinatarioEmail, string DatosContacto,int IdUsuario,string CURP, out string mensaje)
        {
            bool result = false;
            mensaje = string.Empty;

            using (var cn = new SqlConnection(sConexion))
            using (var cmd = new SqlCommand("NET_UPDATE_CMF_RFC_FEDEX_EXPERTTIWEB", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@RFC", SqlDbType.VarChar, 50).Value = rfc;
                cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 25).Value = GuiaHouse;

                cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 100).Value = nombre;
                cmd.Parameters.Add("@DestinatarioEmail", SqlDbType.VarChar, 500).Value = destinatarioEmail;
                cmd.Parameters.Add("@DatosContacto", SqlDbType.VarChar, 50).Value = DatosContacto;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = IdUsuario;
                cmd.Parameters.Add("@CURP", SqlDbType.VarChar, 50).Value = CURP;

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    result = true;
                }
                catch (SqlException e)
                {
                    mensaje = $"{e.Message}";
                    throw new Exception(mensaje);
                }
                catch (Exception ex)
                {
                    mensaje = $"{ex.Message}";
                    throw new Exception(mensaje);
                }
            }

            return result;
        }

       
        

        public string GetFile(int IDDatosDeEmpresa)
        {
            string UrlS3 = string.Empty;
            string filename = string.Empty;
            switch (IDDatosDeEmpresa)
            {
                case 1:
                    filename = "Layout_dhl.xlsx";
                    break;
                case 2:
                    filename = "Layout_fedex.xlsx";
                    break;
                default:
                    return "Not Found";
            }
            UrlS3 = _bucketRepo.URL(filename, "grupoei.documentos");
            return UrlS3;
        }

        public List<string> EjecutarActualizacionMasiva<T>(List<T> registros,int idUsuario,Func<List<T>, int, Guid, bool> metodoTVP)
        {
            var respuestas = new List<string>();
            Guid guid = Guid.NewGuid();

            if (metodoTVP(registros, idUsuario, guid))
            {
                using (var cn = new SqlConnection(sConexion))
                {
                    cn.Open();

                    using (var cmd = new SqlCommand("NET_CONSULTA_MENSAJE_RFC_MASIVO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        cmd.Parameters.AddWithValue("@Guid", guid);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    respuestas.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }

            return respuestas;
        }

        public List<string> ActualizarRFCFedexMasivo(List<RegistroRfcFedex> registros, int IdUsuario)
        {
            return EjecutarActualizacionMasiva(registros, IdUsuario, ActualizarRFCFedexMasivoTVP);

        }

        public List<string> ActualizarRFCDHLMasivo(List<RegistroRFCDHL> registros, int idUsuario)
        {
            return EjecutarActualizacionMasiva(registros, idUsuario, ActualizarRFCDHLMasivoTVP);
        }

        public bool ActualizarRFCDHLMasivoTVP(List<RegistroRFCDHL> registros, int IdUsuario, Guid guid)
        {
            bool response = false;
            var dt = new DataTable();
            dt.Columns.Add("RFC", typeof(string));
            dt.Columns.Add("GuiaHouse", typeof(string));
            dt.Columns.Add("IdRiel", typeof(string));
            dt.Columns.Add("DestinatarioEmail", typeof(string));
            dt.Columns.Add("DatosContacto", typeof(string));
            dt.Columns.Add("IdUsuario", typeof(int));

            foreach (var r in registros)
            {
                dt.Rows.Add(r.TaxIDImpo, r.GuiaHouse, r.IdRiel, r.DestinatarioEmail, r.DatosContacto, IdUsuario);
            }

            using (var cn = new SqlConnection(sConexion))
            using (var cmd = new SqlCommand("NET_UPDATE_CMF_RFC_DHL_EXPERTTIWEB_MASIVO", cn)) // Asegúrate de que este SP exista
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Registros", dt);
                cmd.Parameters.AddWithValue("@Guid", guid);

                cn.Open();
                cmd.ExecuteNonQuery();
                response = true;
            }

            return response;
        }


        public bool ActualizarRFCFedexMasivoTVP(List<RegistroRfcFedex> registros, int IdUsuario, Guid guid)
        {
            bool response = false;
            var dt = new DataTable();
            dt.Columns.Add("RFC", typeof(string));
            dt.Columns.Add("GuiaHouse", typeof(string));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("DestinatarioEmail", typeof(string));
            dt.Columns.Add("DatosContacto", typeof(string));
            dt.Columns.Add("IdUsuario", typeof(int));
            dt.Columns.Add("CURP", typeof(string));

            foreach (var r in registros)
                dt.Rows.Add(r.Rfc, r.GuiaHouse, r.Nombre, r.DestinatarioEmail, r.DatosContacto, IdUsuario, r.CURP);

            using (var cn = new SqlConnection(sConexion))
            using (var cmd = new SqlCommand("NET_UPDATE_CMF_RFC_FEDEX_EXPERTTIWEB_MASIVO", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Registros", dt);
                cmd.Parameters.AddWithValue("@Guid", guid);

                cn.Open();
                cmd.ExecuteNonQuery();
                response = true;
            }
            return response;
        }


        public Dictionary<string, object> InsertInfoFiscalCliente(InfoFiscalCliente infoFiscalCliente)
        {
            Dictionary<string, object> Response = new Dictionary<string, object>();

            try
            {
                using (SqlConnection connection = new(sConexion))
                {
                    using (SqlCommand command = new("NET_INSERTINFOFISCALCLIENTE", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@GuiaHouse", infoFiscalCliente.GuiaHouse);
                        command.Parameters.AddWithValue("@IdCustomAlert", infoFiscalCliente.IdCustomAlert);
                        command.Parameters.AddWithValue("@Nombre", infoFiscalCliente.Nombre);
                        command.Parameters.AddWithValue("@RFC", (object?)infoFiscalCliente.RFC ?? DBNull.Value);
                        command.Parameters.AddWithValue("@CURP", (object?)infoFiscalCliente.CURP ?? DBNull.Value);
                        command.Parameters.AddWithValue("@NSS", infoFiscalCliente.NSS ?? ""); // Default es ''
                        command.Parameters.AddWithValue("@Telefono", infoFiscalCliente.Telefono);
                        command.Parameters.AddWithValue("@CorreoElectronico", infoFiscalCliente.CorreoElectronico);
                        command.Parameters.AddWithValue("@Contacto", infoFiscalCliente.Contacto);
                        command.Parameters.AddWithValue("@IDCapturo", infoFiscalCliente.IDCapturo);
                        command.Parameters.AddWithValue("@Activo", infoFiscalCliente.Activo);
                        command.Parameters.AddWithValue("@DocExtranjero", (object?)infoFiscalCliente.DocExtranjero ?? DBNull.Value);
                        command.Parameters.AddWithValue("@IDCIF", (object?)infoFiscalCliente.IDCIF ?? DBNull.Value);
                        command.Parameters.AddWithValue("@IdTipoDocumento", (object?)infoFiscalCliente.IdTipoDocumento ?? DBNull.Value);
                        command.Parameters.AddWithValue("@IdDatosEmpresa", (object?)infoFiscalCliente.IdDatosEmpresa ?? DBNull.Value);
                        command.Parameters.AddWithValue("@EsPersonaMoral", (object?)infoFiscalCliente.EsPersonaMoral ?? DBNull.Value);
                        command.Parameters.AddWithValue("@RazonSocial", (object?)infoFiscalCliente.RazonSocial ?? DBNull.Value);


                        //if (infoFiscalCliente.IdUsuario != null && infoFiscalCliente.IdUsuario != 0)
                        //{
                        //    command.Parameters.AddWithValue("@IdUsuario", infoFiscalCliente.IdUsuario);
                        //}

                        connection.Open();

                        //int rowsAffected = command.ExecuteNonQuery();

                        //Response["Success"] = rowsAffected > 0;
                        //Response["Message"] = rowsAffected > 0 ? "Registro insertado correctamente." : "No se insertó el registro.";

                        object result = command.ExecuteScalar(); // <-- AQUÍ
                        int newId = Convert.ToInt32(result);

                        Response["Success"] = true;
                        Response["Message"] = "Registro insertado correctamente.";
                        Response["IdDestinatario"] = newId;
                    }
                }
            }
            catch (Exception ex)
            {
                Response["Success"] = false;
                Response["Error"] = ex.Message;
            }

            return Response;
        }
        public Dictionary<string,object> GetInfoFiscalClienteByGuia(InfoFiscalClienteGuia infoFiscalClienteGuia)
        {
            Dictionary<string, object> response = new();
            try
            {
                using (SqlConnection connection = new(sConexion))
                {
                    using (SqlCommand command = new ("NET_GETINFOFISCALCLIENTEBYGUIA", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@GuiaHouse", infoFiscalClienteGuia.GuiaHouse);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                response["Success"] = true;
                                Dictionary<string, object> data = new();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    data[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                                }
                                response["Data"] = data;
                                return response;
                            }
                        }

                    }

                    using (SqlCommand altCommand = new ("NET_GETCUSTOMALERTINFOBYGUIA_TEMP", connection))
                    {
                        altCommand.CommandType = CommandType.StoredProcedure;
                        altCommand.Parameters.AddWithValue("@GuiaHouse", infoFiscalClienteGuia.GuiaHouse);

                        using (SqlDataReader altReader = altCommand.ExecuteReader())
                        {
                            if (altReader.Read())
                            {
                                Dictionary<string, object> data = new();
                                for (int i = 0; i < altReader.FieldCount; i++)
                                {
                                    data[altReader.GetName(i)] = altReader.IsDBNull(i) ? null : altReader.GetValue(i);
                                }
                                response["Success"] = true;
                                response["Data"] = data;
                            }
                            else
                            {
                                response["Success"] = false;
                                response["Message"] = "No se encontraron datos.";
                                response["Data"] = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response["Success"] = false;
                response["Message"] = "Error en la consulta.";
                response["Error"] = ex.Message;
                response["Data"] = null;
            }
            return response;
        }
        public Dictionary<string, object> GetRFCByGuia(InfoFiscalClienteGuia infoFiscalClienteGuia)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            try
            {
                using (SqlConnection connection = new(sConexion))
                {
                    using (SqlCommand command = new("NET_GETRFCBYGUIA", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Guia", infoFiscalClienteGuia.GuiaHouse);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                response["Success"] = true;
                                Dictionary<string, object> data = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    data[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                                }
                                response["Data"] = data;
                            }
                            else
                            {
                                response["Success"] = false;
                                response["Message"] = "No se encontraron datos.";
                                response["Data"] = null;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                response["Success"] = false;
                response["Message"] = "Error en la consulta.";
                response["Error"] = ex.Message;
                response["Data"] = null;
            }
            return response;
        }
        public Dictionary<string, object> UpdateDatosFiscales(InfoFiscalClienteUpdate infoFiscalCliente)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();

            try
            {
                using (SqlConnection connection = new(sConexion))
                {
                    using (SqlCommand command = new("NET_UPDATEDATOSFISCALES", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@GuiaHouse", infoFiscalCliente.GuiaHouse ?? string.Empty);
                        command.Parameters.AddWithValue("@Nombre", infoFiscalCliente.Nombre ?? string.Empty);
                        command.Parameters.AddWithValue("@Telefono", infoFiscalCliente.Telefono ?? string.Empty);
                        command.Parameters.AddWithValue("@Email", infoFiscalCliente.Email ?? string.Empty);
                        command.Parameters.AddWithValue("@Curp", infoFiscalCliente.Curp ?? string.Empty);
                        command.Parameters.AddWithValue("@Rfc", infoFiscalCliente.Rfc ?? string.Empty);
                        command.Parameters.AddWithValue("@DocExtranjero", infoFiscalCliente.DocExtranjero ?? string.Empty);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        response["Success"] = rowsAffected > 0;
                        response["Message"] = rowsAffected > 0 ? "Registro actualizado correctamente." : "No se encontró la guía o no hubo cambios.";
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                response["Success"] = false;
                response["Message"] = "Error en la base de datos.";
                response["Error"] = sqlEx.Message;
            }
            catch (Exception ex)
            {
                response["Success"] = false;
                response["Message"] = "Ocurrió un error inesperado.";
                response["Error"] = ex.Message;
                response["StackTrace"] = ex.StackTrace;
            }

            return response;
        }
        public Dictionary<string, object> GetDireccionesDestinatario(DireccionesDestinatario direccionesDestinatario)
        {
            Dictionary<string, object> response = new();
            try
            {
                using (SqlConnection connection = new(sConexion))
                {
                    using (SqlCommand command = new("NET_GET_DIRECCIONES_BY_DESTINATARIO", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdDestinatario", direccionesDestinatario.IdDestinatario);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<Dictionary<string, object>> results = new();

                            while (reader.Read())
                            {
                                Dictionary<string, object> data = new();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    data[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                                }
                                results.Add(data);
                            }
                                response["Success"] = true;
                                response["Data"] = results;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response["Success"] = false;
                response["Message"] = "Error en la consulta.";
                response["Error"] = ex.Message;
                response["Data"] = null;
            }
            return response;
        }
        public Dictionary<string, object> CreateDireccionDestinatario(DireccionesDestinatarioInsert direccionesDestinatario)
        {
            Dictionary<string, object> Response = new Dictionary<string, object>();

            try
            {
                using (SqlConnection connection = new(sConexion))
                {
                    using (SqlCommand command = new("NET_CREATEDIRECCIONDESTINATARIO", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Calle", direccionesDestinatario.Calle);
                        command.Parameters.AddWithValue("@Colonia", direccionesDestinatario.Colonia);
                        command.Parameters.AddWithValue("@MunicipioAlcandia", direccionesDestinatario.MunicipioAlcandia);
                        command.Parameters.AddWithValue("@CodigoPostal", direccionesDestinatario.CodigoPostal);
                        command.Parameters.AddWithValue("@NumeroExt", direccionesDestinatario.NumeroExt);
                        command.Parameters.AddWithValue("@NumeroInt", (object?)direccionesDestinatario.NumeroInt ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Ciudad", direccionesDestinatario.Ciudad);
                        command.Parameters.AddWithValue("@ClaveEntidadFederativa", direccionesDestinatario.ClaveEntidadFederativa);
                        command.Parameters.AddWithValue("@Localidad", (object?)direccionesDestinatario.Localidad ?? DBNull.Value);
                        command.Parameters.AddWithValue("@IdUsuarioAlta", direccionesDestinatario.IdUsuarioAlta);

                        connection.Open();

                        object result = command.ExecuteScalar(); // <-- AQUÍ
                        int newId = Convert.ToInt32(result);

                        Response["Success"] = true;
                        Response["Message"] = "Registro insertado correctamente.";
                        Response["IDDireccion"] = newId;
                    }
                }
            }
            catch (Exception ex)
            {
                Response["Success"] = false;
                Response["Error"] = ex.Message;
            }

            return Response;
        }

        public Dictionary<string, object> UpdateDireccionDestinatario(DireccionesDestinatarioUpdate direccionesDestinatario)
        {
            Dictionary<string, object> Response = new Dictionary<string, object>();

            try
            {
                using (SqlConnection connection = new(sConexion))
                {
                    using (SqlCommand command = new("NET_UPDATEDIRECCIONDESTINATARIO", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@IDDireccion", direccionesDestinatario.IDDireccion);
                        command.Parameters.AddWithValue("@Calle", (object?)direccionesDestinatario.Calle ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Colonia", (object?)direccionesDestinatario.Colonia ?? DBNull.Value);
                        command.Parameters.AddWithValue("@MunicipioAlcandia", (object?)direccionesDestinatario.MunicipioAlcandia ?? DBNull.Value);
                        command.Parameters.AddWithValue("@CodigoPostal", (object?)direccionesDestinatario.CodigoPostal ?? DBNull.Value);
                        command.Parameters.AddWithValue("@NumeroExt", (object?)direccionesDestinatario.NumeroExt ?? DBNull.Value);
                        command.Parameters.AddWithValue("@NumeroInt", (object?)direccionesDestinatario.NumeroInt ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Ciudad", (object?)direccionesDestinatario.Ciudad ?? DBNull.Value);
                        command.Parameters.AddWithValue("@ClaveEntidadFederativa", (object?)direccionesDestinatario.ClaveEntidadFederativa ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Localidad", (object?)direccionesDestinatario.Localidad ?? DBNull.Value);
                        command.Parameters.AddWithValue("@IdUsuarioModifica", direccionesDestinatario.IdUsuarioModifica);
                        command.Parameters.AddWithValue("@Activo", direccionesDestinatario.Activo);

                        connection.Open();

                        int rowsAffected = command.ExecuteNonQuery();

                        Response["Success"] = rowsAffected > 0;
                        Response["Message"] = rowsAffected > 0 ? "Registro insertado correctamente." : "No se insertó el registro.";
                    }
                }
            }
            catch (Exception ex)
            {
                Response["Success"] = false;
                Response["Error"] = ex.Message;
            }

            return Response;
        }

        public Dictionary<string, object> InsertDestinatarioDireccion(DireccionRelacion direccionRelacion)
        {
            Dictionary<string, object> Response = new Dictionary<string, object>();

            try
            {
                using (SqlConnection connection = new(sConexion))
                {
                    using (SqlCommand command = new("NET_INSERT_DESTINATARIO_DIRECCION", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@IdDestinatario", direccionRelacion.IdDestinatario);
                        command.Parameters.AddWithValue("@IDDireccion", direccionRelacion.IDDireccion);

                        connection.Open();

                        int rowsAffected = command.ExecuteNonQuery();

                        Response["Success"] = rowsAffected > 0;
                        Response["Message"] = rowsAffected > 0 ? "Registro insertado correctamente." : "No se insertó el registro.";
                    }
                }
            }
            catch (Exception ex)
            {
                Response["Success"] = false;
                Response["Error"] = ex.Message;
            }

            return Response;
        }

    }
}

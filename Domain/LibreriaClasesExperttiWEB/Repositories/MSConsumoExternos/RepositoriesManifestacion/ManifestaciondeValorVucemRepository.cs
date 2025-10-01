using DocumentFormat.OpenXml.Office2010.PowerPoint;
using LibreriaClasesAPIExpertti.Entities.EntitiesManifestacion;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesManifestacion.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesManifestacion
{    
    public class ManifestaciondeValorVucemRepository : IManifestaciondeValorVucemRepository
    {

        public string SConexion { get; set; }
        string IManifestaciondeValorVucemRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public ManifestaciondeValorVucemRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        //Buscar(IdReferencia)
        public ManifestaciondeValorVucem Buscar(string NumerodeReferencia)
        {
            ManifestaciondeValorVucem objManifestaciondeValorVucem = new();
            try
            {
                Referencias referencias = new();
                ReferenciasRepository referenciasRepository = new(_configuration);

                referencias = referenciasRepository.Buscar(NumerodeReferencia);
                if (referencias == null)
                {
                    throw new Exception("No fue posible encontrar la referencia");
                } 

                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_SEARCH_CASAEI_ManifestaciondeValorVucem", con);
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@idReferencia", SqlDbType.VarChar, 10).Value = referencias.IDReferencia;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objManifestaciondeValorVucem.idMV = Convert.ToInt32(dr["idMV"]);
                        objManifestaciondeValorVucem.idReferencia  = Convert.ToInt32(dr["idReferencia"]);
                        objManifestaciondeValorVucem.ExisteVinculacion  = Convert.ToBoolean(dr["ExisteVinculacion"]);
                        objManifestaciondeValorVucem.MetododeValoracion = Convert.ToInt32(dr["MetododeValoracion"]);
                        objManifestaciondeValorVucem.PrecioPagado  = Convert.ToDecimal(dr["PrecioPagado"]);
                        objManifestaciondeValorVucem.pagadoFecha  = Convert.ToDateTime(dr["pagadoFecha"]);
                        objManifestaciondeValorVucem.PagadoFormadePago = Convert.ToInt32(dr["PagadoFormadePago"]);
                        objManifestaciondeValorVucem.PagadoMoneda = Convert.ToString(dr["PagadoMoneda"]);
                        objManifestaciondeValorVucem.pagadoEspecifique = Convert.ToString(dr["pagadoEspecifique"]);
                        objManifestaciondeValorVucem.precioPorPagar = Convert.ToDecimal(dr["precioPorPagar"]);
                        objManifestaciondeValorVucem.porPagarFecha = Convert.ToDateTime(dr["porPagarFecha"]);
                        objManifestaciondeValorVucem.porPagarMomento = Convert.ToString(dr["porPagarMomento"]);
                        objManifestaciondeValorVucem.porPagarFormadePago = Convert.ToInt32(dr["porPagarFormadePago"]);
                        objManifestaciondeValorVucem.porPagarEspecifique = Convert.ToString(dr["porPagarEspecifique"]);
                        objManifestaciondeValorVucem.porPagarMoneda = Convert.ToString(dr["porPagarMoneda"]);
                        objManifestaciondeValorVucem.compensoPago = Convert.ToDecimal(dr["compensoPago"]);
                        objManifestaciondeValorVucem.compensoFecha = Convert.ToDateTime(dr["compensoFecha"]);
                        objManifestaciondeValorVucem.compensoFormadePago = Convert.ToInt32(dr["compensoFormadePago"]);
                        objManifestaciondeValorVucem.compensoMotivo = Convert.ToString(dr["compensoMotivo"]);
                        objManifestaciondeValorVucem.compensoPrestacion = Convert.ToString(dr["compensoPrestacion"]);
                        objManifestaciondeValorVucem.compensoEspecifique = Convert.ToString(dr["compensoEspecifique"]);
                        objManifestaciondeValorVucem.RFCSella = Convert.ToString(dr["RFCSella"]);
                        objManifestaciondeValorVucem.CadenaOriginal = Convert.ToString(dr["CadenaOriginal"]);
                        objManifestaciondeValorVucem.firmaDigital = Convert.ToString(dr["firmaDigital"]);
                        objManifestaciondeValorVucem.fechaEnvio = Convert.ToDateTime(dr["fechaEnvio"]);
                        objManifestaciondeValorVucem.NumerodeOperacion = Convert.ToString(dr["NumerodeOperacion"]);
                        objManifestaciondeValorVucem.fechaRecepcion = Convert.ToDateTime(dr["fechaRecepcion"]);
                        objManifestaciondeValorVucem.eDocument = Convert.ToString(dr["eDocument"]);
                        objManifestaciondeValorVucem.idDocumentoAcuse = Convert.ToInt32(dr["idDocumentoAcuse"]);
                        objManifestaciondeValorVucem.idDocumentoMV = Convert.ToInt32(dr["idDocumentoMV"]);
                        objManifestaciondeValorVucem.Aceptada = Convert.ToBoolean(dr["Aceptada"]);
                        objManifestaciondeValorVucem.idUsuarioAcepta = Convert.ToInt32(dr["idUsuarioAcepta"]);
                        objManifestaciondeValorVucem.FechaAceptacion = Convert.ToDateTime(dr["FechaAceptacion"]);
                        objManifestaciondeValorVucem.Observaciones = Convert.ToString(dr["Observaciones"]);
                        objManifestaciondeValorVucem.Estatus = Convert.ToBoolean(dr["Estatus"]);
                    }
                    else
                    {
                        objManifestaciondeValorVucem = null!;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objManifestaciondeValorVucem;
        }
        //Insertar(objManifestacion)
        public int Insertar(ManifestaciondeValorVucem objManifestaciondeValorVucem)
        {
            int Id = 0;

            try
            {
                using SqlConnection con = new(SConexion);
                using var cmd = new SqlCommand("NET_INSERT_CASAEI_ManifestaciondeValorVucem", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@idReferencia", SqlDbType.Int, 4).Value = objManifestaciondeValorVucem.idReferencia;
                cmd.Parameters.Add("@ExisteVinculacion", SqlDbType.Bit ).Value = objManifestaciondeValorVucem.ExisteVinculacion;
                cmd.Parameters.Add("@MetododeValoracion", SqlDbType.Int, 4).Value = objManifestaciondeValorVucem.MetododeValoracion;
                cmd.Parameters.Add("@PrecioPagado", SqlDbType.Decimal).Value = objManifestaciondeValorVucem.PrecioPagado;
                cmd.Parameters.Add("@pagadoFecha", SqlDbType.DateTime).Value = objManifestaciondeValorVucem.pagadoFecha;
                cmd.Parameters.Add("@PagadoFormadePago", SqlDbType.Int, 4).Value = objManifestaciondeValorVucem.PagadoFormadePago;
                cmd.Parameters.Add("@PagadoMoneda", SqlDbType.VarChar, 3).Value = objManifestaciondeValorVucem.PagadoMoneda;
                cmd.Parameters.Add("@pagadoEspecifique", SqlDbType.VarChar, 70).Value = objManifestaciondeValorVucem.pagadoEspecifique;
                cmd.Parameters.Add("@precioPorPagar", SqlDbType.Decimal).Value = objManifestaciondeValorVucem.precioPorPagar;
                cmd.Parameters.Add("@porPagarFecha", SqlDbType.DateTime ).Value = objManifestaciondeValorVucem.porPagarFecha;
                cmd.Parameters.Add("@porPagarMomento", SqlDbType.VarChar, 1000).Value = objManifestaciondeValorVucem.porPagarMomento;
                cmd.Parameters.Add("@porPagarFormadePago", SqlDbType.Int, 4).Value = objManifestaciondeValorVucem.porPagarFormadePago;
                cmd.Parameters.Add("@porPagarEspecifique", SqlDbType.VarChar, 70).Value = objManifestaciondeValorVucem.porPagarEspecifique;
                cmd.Parameters.Add("@porPagarMoneda", SqlDbType.VarChar, 3).Value = objManifestaciondeValorVucem.porPagarMoneda;
                cmd.Parameters.Add("@compensoPago", SqlDbType.Decimal).Value = objManifestaciondeValorVucem.compensoPago;
                cmd.Parameters.Add("@compensoFecha", SqlDbType.DateTime).Value = objManifestaciondeValorVucem.compensoFecha;
                cmd.Parameters.Add("@compensoFormadePago", SqlDbType.Int, 4).Value = objManifestaciondeValorVucem.compensoFormadePago;
                cmd.Parameters.Add("@compensoMotivo", SqlDbType.VarChar, 1000).Value = objManifestaciondeValorVucem.compensoMotivo;
                cmd.Parameters.Add("@compensoPrestacion", SqlDbType.VarChar, 1000).Value = objManifestaciondeValorVucem.compensoPrestacion;
                cmd.Parameters.Add("@compensoEspecifique", SqlDbType.VarChar, 70).Value = objManifestaciondeValorVucem.compensoEspecifique;
                cmd.Parameters.Add("@RFCSella", SqlDbType.VarChar, 13).Value = objManifestaciondeValorVucem.RFCSella;
                cmd.Parameters.Add("@CadenaOriginal", SqlDbType.VarChar, -1 ).Value = objManifestaciondeValorVucem.CadenaOriginal;
                cmd.Parameters.Add("@firmaDigital", SqlDbType.VarChar, -1).Value = objManifestaciondeValorVucem.firmaDigital;
                cmd.Parameters.Add("@fechaEnvio", SqlDbType.DateTime).Value = objManifestaciondeValorVucem.fechaEnvio;
                cmd.Parameters.Add("@NumerodeOperacion", SqlDbType.VarChar, 1000).Value = objManifestaciondeValorVucem.NumerodeOperacion;
                cmd.Parameters.Add("@fechaRecepcion", SqlDbType.DateTime).Value = objManifestaciondeValorVucem.fechaRecepcion;
                cmd.Parameters.Add("@eDocument", SqlDbType.VarChar, 20).Value = objManifestaciondeValorVucem.eDocument;
                cmd.Parameters.Add("@idDocumentoAcuse", SqlDbType.Int, 4).Value = objManifestaciondeValorVucem.idDocumentoAcuse;
                cmd.Parameters.Add("@idDocumentoMV", SqlDbType.Int, 4).Value = objManifestaciondeValorVucem.idDocumentoMV;
                cmd.Parameters.Add("@Aceptada", SqlDbType.Bit).Value = objManifestaciondeValorVucem.Aceptada;
                cmd.Parameters.Add("@idUsuarioAcepta", SqlDbType.Int, 4).Value = objManifestaciondeValorVucem.idUsuarioAcepta;
                cmd.Parameters.Add("@FechaAceptacion", SqlDbType.DateTime ).Value = objManifestaciondeValorVucem.FechaAceptacion;
                cmd.Parameters.Add("@Observaciones", SqlDbType.VarChar, 8000).Value = objManifestaciondeValorVucem.Observaciones;
                cmd.Parameters.Add("@Estatus", SqlDbType.Bit).Value = objManifestaciondeValorVucem.Estatus;
                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                con.Open();
                using SqlDataReader dr = cmd.ExecuteReader();
                {
                    if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                    {
                        Id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el registro.", ex);
            }
            return Id;
        }

        //Modificar(objSaaioPerPar):bool
        public bool Modificar(ManifestaciondeValorVucem objManifestaciondeValorVucem)
        {
            bool Id = false;

            try
            {
                using SqlConnection con = new(SConexion);
                using var cmd = new SqlCommand("NET_UPDATE_CASAEI_ManifestaciondeValorVucem", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@idMV", SqlDbType.Int, 4).Value = objManifestaciondeValorVucem.idMV;
                cmd.Parameters.Add("@idReferencia", SqlDbType.Int, 4).Value = objManifestaciondeValorVucem.idReferencia;
                cmd.Parameters.Add("@ExisteVinculacion", SqlDbType.Bit).Value = objManifestaciondeValorVucem.ExisteVinculacion;
                cmd.Parameters.Add("@MetododeValoracion", SqlDbType.Int, 4).Value = objManifestaciondeValorVucem.MetododeValoracion;
                cmd.Parameters.Add("@PrecioPagado", SqlDbType.Decimal).Value = objManifestaciondeValorVucem.PrecioPagado;
                cmd.Parameters.Add("@pagadoFecha", SqlDbType.DateTime).Value = objManifestaciondeValorVucem.pagadoFecha;
                cmd.Parameters.Add("@PagadoFormadePago", SqlDbType.Int, 4).Value = objManifestaciondeValorVucem.PagadoFormadePago;
                cmd.Parameters.Add("@PagadoMoneda", SqlDbType.VarChar, 3).Value = objManifestaciondeValorVucem.PagadoMoneda;
                cmd.Parameters.Add("@pagadoEspecifique", SqlDbType.VarChar, 70).Value = objManifestaciondeValorVucem.pagadoEspecifique;
                cmd.Parameters.Add("@precioPorPagar", SqlDbType.Decimal).Value = objManifestaciondeValorVucem.precioPorPagar;
                cmd.Parameters.Add("@porPagarFecha", SqlDbType.DateTime).Value = objManifestaciondeValorVucem.porPagarFecha;
                cmd.Parameters.Add("@porPagarMomento", SqlDbType.VarChar, 1000).Value = objManifestaciondeValorVucem.porPagarMomento;
                cmd.Parameters.Add("@porPagarFormadePago", SqlDbType.Int, 4).Value = objManifestaciondeValorVucem.porPagarFormadePago;
                cmd.Parameters.Add("@porPagarEspecifique", SqlDbType.VarChar, 70).Value = objManifestaciondeValorVucem.porPagarEspecifique;
                cmd.Parameters.Add("@porPagarMoneda", SqlDbType.VarChar, 3).Value = objManifestaciondeValorVucem.porPagarMoneda;
                cmd.Parameters.Add("@compensoPago", SqlDbType.Decimal).Value = objManifestaciondeValorVucem.compensoPago;
                cmd.Parameters.Add("@compensoFecha", SqlDbType.DateTime).Value = objManifestaciondeValorVucem.compensoFecha;
                cmd.Parameters.Add("@compensoFormadePago", SqlDbType.Int, 4).Value = objManifestaciondeValorVucem.compensoFormadePago;
                cmd.Parameters.Add("@compensoMotivo", SqlDbType.VarChar, 1000).Value = objManifestaciondeValorVucem.compensoMotivo;
                cmd.Parameters.Add("@compensoPrestacion", SqlDbType.VarChar, 1000).Value = objManifestaciondeValorVucem.compensoPrestacion;
                cmd.Parameters.Add("@compensoEspecifique", SqlDbType.VarChar, 70).Value = objManifestaciondeValorVucem.compensoEspecifique;
                cmd.Parameters.Add("@RFCSella", SqlDbType.VarChar, 13).Value = objManifestaciondeValorVucem.RFCSella;
                cmd.Parameters.Add("@CadenaOriginal", SqlDbType.VarChar, -1).Value = objManifestaciondeValorVucem.CadenaOriginal;
                cmd.Parameters.Add("@firmaDigital", SqlDbType.VarChar, -1).Value = objManifestaciondeValorVucem.firmaDigital;
                cmd.Parameters.Add("@fechaEnvio", SqlDbType.DateTime).Value = objManifestaciondeValorVucem.fechaEnvio;
                cmd.Parameters.Add("@NumerodeOperacion", SqlDbType.VarChar, 1000).Value = objManifestaciondeValorVucem.NumerodeOperacion;
                cmd.Parameters.Add("@fechaRecepcion", SqlDbType.DateTime).Value = objManifestaciondeValorVucem.fechaRecepcion;
                cmd.Parameters.Add("@eDocument", SqlDbType.VarChar, 20).Value = objManifestaciondeValorVucem.eDocument;
                cmd.Parameters.Add("@idDocumentoAcuse", SqlDbType.Int, 4).Value = objManifestaciondeValorVucem.idDocumentoAcuse;
                cmd.Parameters.Add("@idDocumentoMV", SqlDbType.Int, 4).Value = objManifestaciondeValorVucem.idDocumentoMV;
                cmd.Parameters.Add("@Aceptada", SqlDbType.Bit).Value = objManifestaciondeValorVucem.Aceptada;
                cmd.Parameters.Add("@idUsuarioAcepta", SqlDbType.Int, 4).Value = objManifestaciondeValorVucem.idUsuarioAcepta;
                cmd.Parameters.Add("@FechaAceptacion", SqlDbType.DateTime).Value = objManifestaciondeValorVucem.FechaAceptacion;
                cmd.Parameters.Add("@Observaciones", SqlDbType.VarChar, 8000).Value = objManifestaciondeValorVucem.Observaciones;
                cmd.Parameters.Add("@Estatus", SqlDbType.Bit).Value = objManifestaciondeValorVucem.Estatus;
                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                con.Open();
                using SqlDataReader dr = cmd.ExecuteReader();
                {
                    if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                    {
                        Id = Convert.ToBoolean(cmd.Parameters["@newid_registro"].Value);
                    }
                }

            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return Id;
        }

        public bool Borrar(int idMV)
        {
            bool id;

            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_DELETE_CASAEI_ManifestaciondeValorVucem", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@idMV", SqlDbType.Int, 4).Value = idMV;  
                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                con.Open();

                cmd.ExecuteNonQuery();
                id = Convert.ToBoolean(cmd.Parameters["@newid_registro"].Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString()  );
            }
            return id;
        }

        //EnviarVucem(IdReferencia): NoOperacion
        //RecibirVucem(objManifestacion):eDocument
        //Imprimir(IdReferencia)
        
        public bool getVinculacion(string numerodeReferencia)
        {
            bool existeVinculacion = false;
            string Vinculacion = "";
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_MV_EXISTEVINCULACION", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 25).Value = numerodeReferencia;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        Vinculacion = dr["VIN_PRO"].ToString();


                    }
                    else
                    {
                        Vinculacion = string.Empty;
                        throw new Exception("No fue posible establecer la vinculación");
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
            if (Vinculacion.ToUpper() == "S")
            {
                existeVinculacion = true;
            }
            else
            {
                existeVinculacion = false;
            }

            return existeVinculacion;
        }

        public int getMetododeValoracion(string numerodeReferencia)
        {
            int metododeValoracion = 0;
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_MV_METODODEVALORACION", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 25).Value = numerodeReferencia;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        metododeValoracion = Convert.ToInt32(dr["CVE_VALO"]);
                    }
                    else
                    {
                        metododeValoracion = 0;
                        throw new Exception("No fue posible establecer el método de valoración");
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }


            return metododeValoracion;
        }

    }
}


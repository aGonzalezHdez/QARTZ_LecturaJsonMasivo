using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class TiposdeDocumentosdelClienteRepository : ITiposdeDocumentosdelClienteRepository
    {
        public string SConexion { get; set; }
        string ITiposdeDocumentosdelClienteRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public TiposdeDocumentosdelClienteRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }



        public async Task<List<TiposdeDocumentosdelCliente>> CargarCombo(int MyIDCliente)
        {
            List<TiposdeDocumentosdelCliente> lstTiposdeDocumentos = new();

            try
            {
                using (SqlConnection con = new(SConexion))

                using (SqlCommand cmd = new("NET_LOAD_TIPOSDEDOCUMENTOSDECLIENTE", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int).Value = MyIDCliente;

                    using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            TiposdeDocumentosdelCliente objTiposdeDocumentos = new();

                            objTiposdeDocumentos.IDTiposDeDocumentosDeCliente = Convert.ToInt32(dr["IDTiposDeDocumentosDeCliente"]);
                            objTiposdeDocumentos.TipoDeDocumento = string.Format("{0}", dr["TipoDeDocumento"]);
                            objTiposdeDocumentos.AplicaVigencia = Convert.ToBoolean(dr["AplicaVigencia"]);
                            objTiposdeDocumentos.FechaFinalObligada = Convert.ToInt32(dr["FechaFinalObligada"]);
                            objTiposdeDocumentos.FiguraFiscal = Convert.ToInt32(dr["FiguraFiscal"]);
                            objTiposdeDocumentos.SubTipoDeDocumento = Convert.ToInt32(dr["SubTipoDeDocumento"]);
                            objTiposdeDocumentos.ID_Requisitos = Convert.ToInt32(dr["ID_Requisitos"]);
                            objTiposdeDocumentos.Ayuda = string.Format("{0}", dr["Ayuda"]);

                            lstTiposdeDocumentos.Add(objTiposdeDocumentos);
                        }
                    }
                    else
                    {
                        lstTiposdeDocumentos = null!;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }


            return lstTiposdeDocumentos;
        }

        public TiposdeDocumentosdelCliente Buscar(int IDTiposDeDocumentosDeCliente)
        {
            TiposdeDocumentosdelCliente objTiposDeDocumentosdeCliente = new();

            try
            {

                using (SqlConnection con = new(SConexion))

                using (SqlCommand cmd = new("NET_SEARCH_TIPOSDEDOCUMENTOSDELCLIENTE", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDTiposDeDocumentosDeCliente", SqlDbType.Int).Value = IDTiposDeDocumentosDeCliente;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objTiposDeDocumentosdeCliente.IDTiposDeDocumentosDeCliente = Convert.ToInt32(dr["IDTiposDeDocumentosDeCliente"]);
                        objTiposDeDocumentosdeCliente.TipoDeDocumento = string.Format("{0}", dr["TipoDeDocumento"]);
                        objTiposDeDocumentosdeCliente.AplicaVigencia = Convert.ToBoolean(dr["AplicaVigencia"]);
                        objTiposDeDocumentosdeCliente.FechaFinalObligada = Convert.ToInt32(dr["FechaFinalObligada"]);
                        objTiposDeDocumentosdeCliente.FiguraFiscal = Convert.ToInt32(dr["FiguraFiscal"]);
                        objTiposDeDocumentosdeCliente.SubTipoDeDocumento = Convert.ToInt32(dr["SubTipoDeDocumento"]);
                        objTiposDeDocumentosdeCliente.ID_Requisitos = Convert.ToInt32(dr["ID_Requisitos"]);
                        objTiposDeDocumentosdeCliente.Ayuda = string.Format("{0}", dr["Ayuda"]);
                        objTiposDeDocumentosdeCliente.NombreDocumento = string.Format("{0}", dr["NombreDocumento"]);
                    }
                    else
                    {
                        objTiposDeDocumentosdeCliente = null!;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objTiposDeDocumentosdeCliente;
        }


        public void InsertaBitacora(int IdUsuario, int IdCliente, int IdTiposDeDocumentosDeCliente, int TipoDeCambio)
        {
            try
            {
                using (SqlConnection con = new(SConexion))

                using (var cmd = new SqlCommand("NET_INSERT_BITACORAEXPEDIENTECLIENTES", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ID_USUARIO", SqlDbType.Int, 4).Value = IdUsuario;
                    cmd.Parameters.Add("@ID_CLIENTE", SqlDbType.Int, 4).Value = IdCliente;
                    cmd.Parameters.Add("@ID_TIPO_DOCUMENTO", SqlDbType.Int, 4).Value = IdTiposDeDocumentosDeCliente;
                    cmd.Parameters.Add("@TIPO_CAMBIO", SqlDbType.Int, 4).Value = TipoDeCambio;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

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

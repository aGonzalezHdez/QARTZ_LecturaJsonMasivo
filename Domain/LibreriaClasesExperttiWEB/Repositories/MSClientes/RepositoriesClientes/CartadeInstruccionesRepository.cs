using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class CartaDeInstruccionesRepository : ICartaDeInstruccionesRepository
    {
        public string SConexion { get; set; }
        string ICartaDeInstruccionesRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public CartaDeInstruccionesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public DataTable LoadDvgCartadeInstruccionesForCambioValor(int IdCliente, int IdOficina)
        {

            var dtb = new DataTable();
            SqlParameter @param;

            using (var cn = new SqlConnection(SConexion))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_LOAD_CARTADEINSTRUCCIONES_FOR_CAMBIO_VALOR";
                    @param = dap.SelectCommand.Parameters.Add("@IDCliente", SqlDbType.Int);
                    @param.Value = IdCliente;

                    @param = dap.SelectCommand.Parameters.Add("@IdOficina", SqlDbType.Int);
                    @param.Value = IdOficina;


                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dap.Fill(dtb);
                    cn.Close();
                    cn.Dispose();
                }
                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();

                    throw new Exception(ex.Message.ToString() + "NET_LOAD_CARTADEINSTRUCCIONES_FOR_CAMBIO_VALOR");
                }
            }
            return dtb;
        }
        public List<CartaInstruccionesIdEmpresa> CargarCartadeInstruccionesIdCliente(int IdCliente, int IDDatosDeEmpresa)
        {
            List<CartaInstruccionesIdEmpresa> listCartaInstrucciones = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("[Cl].[NET_LOAD_CASAEI_CARTADEINSTRUCCIONES_IDEMPRESA]", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int).Value = IdCliente;
                    cmd.Parameters.Add("@IDDatosDeEmpresa", SqlDbType.Int, 4).Value = IDDatosDeEmpresa;

                    using SqlDataReader sdr = cmd.ExecuteReader();
                    listCartaInstrucciones = SqlDataReaderToList.DataReaderMapToList<CartaInstruccionesIdEmpresa>(sdr);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return listCartaInstrucciones;
        }

        public double UltimoValor(int IdCliente, int IdOficina)
        {

            double ValorFinalEnDolares = 0;

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_ULTIMOVALORCARTADEINSTRUCCIONES", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = IdCliente;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = IdOficina;
                    cmd.Parameters.Add("@MyValor", SqlDbType.Money).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {
                        ValorFinalEnDolares = Convert.ToDouble(string.Format("{0:0.00}", cmd.Parameters["@MyValor"].Value));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return ValorFinalEnDolares;
        }

        public bool RangoValorInicial(int IdCliente, double MyValor, int IdOficina)
        {
            bool MyBol = false;

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_RANGOSDEVALORINICIALCARTADEINSTRUCCIONES", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCliente ", SqlDbType.Int, 4).Value = IdCliente;
                    cmd.Parameters.Add("@MyValor", SqlDbType.Money).Value = MyValor;
                    cmd.Parameters.Add("@IdOficina ", SqlDbType.Int, 4).Value = IdOficina;
                    cmd.Parameters.Add("@Existe", SqlDbType.Int).Direction = ParameterDirection.Output;


                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@Existe"].Value) == 1)
                        {
                            MyBol = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return MyBol;
        }


        public int Insertar(CartaInstrucciones objCartaDeInstrucciones)
        {
            ValidarCarta(objCartaDeInstrucciones);
            //objCartaDeInstrucciones.ValorInicialEnDolares= Convert.ToDecimal(UltimoValor(objCartaDeInstrucciones.IdCliente, objCartaDeInstrucciones.IdOficina));

            int id;

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_INSERT_CARTADEINSTRUCCIONES_IDDatosDeEmpresa", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4).Value = objCartaDeInstrucciones.IdCliente;
                    cmd.Parameters.Add("@NombreDelCliente", SqlDbType.VarChar, 150).Value = objCartaDeInstrucciones.NombreDelCliente.Trim();
                    cmd.Parameters.Add("@ValorInicialEnDolares", SqlDbType.Money, 4).Value = objCartaDeInstrucciones.ValorInicialEnDolares;
                    cmd.Parameters.Add("@ValorFinalEnDolares", SqlDbType.Money, 4).Value = objCartaDeInstrucciones.ValorFinalEnDolares;
                    cmd.Parameters.Add("@IDCategoria", SqlDbType.Int, 4).Value = objCartaDeInstrucciones.IDCategoria;
                    cmd.Parameters.Add("@Grupo", SqlDbType.VarChar, 50).Value = objCartaDeInstrucciones.Grupo.Trim();
                    cmd.Parameters.Add("@ClavedePedimento", SqlDbType.VarChar, 2).Value = objCartaDeInstrucciones.ClavedePedimento;
                    cmd.Parameters.Add("@Patente", SqlDbType.Int, 4).Value = objCartaDeInstrucciones.Patente;
                    cmd.Parameters.Add("@NombreAA", SqlDbType.VarChar, 120).Value = objCartaDeInstrucciones.NombreAA.Trim();
                    cmd.Parameters.Add("@Observaciones", SqlDbType.Text).Value = objCartaDeInstrucciones.Observaciones.Trim();
                    cmd.Parameters.Add("@Activa", SqlDbType.Bit, 4).Value = objCartaDeInstrucciones.Activa;
                    cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4).Value = objCartaDeInstrucciones.Operacion;
                    cmd.Parameters.Add("@FechaVigencia", SqlDbType.DateTime).Value = Convert.ToDateTime(objCartaDeInstrucciones.FechaVigencia);
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = objCartaDeInstrucciones.Email;
                    cmd.Parameters.Add("@IDDatosDeEmpresa", SqlDbType.Int, 4).Value = objCartaDeInstrucciones.IDDatosDeEmpresa;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = objCartaDeInstrucciones.IdOficina;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {

                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        {
                            id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                        }
                        else
                        {
                            id = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + "NET_INSERT_CARTADEINSTRUCCIONES_IDDatosDeEmpresa");
            }
            return id;
        }


        public int Modificar(CartaInstrucciones objCartaDeInstrucciones)
        {
            ValidarCarta(objCartaDeInstrucciones);

            int id;

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_UPDATE_CARTADEINSTRUCCIONES_VIGENCIA", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCarta", SqlDbType.Int, 4).Value = objCartaDeInstrucciones.IDCarta;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4).Value = objCartaDeInstrucciones.IdCliente;
                    cmd.Parameters.Add("@NombreDelCliente", SqlDbType.VarChar, 150).Value = objCartaDeInstrucciones.NombreDelCliente;
                    cmd.Parameters.Add("@ValorInicialEnDolares", SqlDbType.Money, 4).Value = objCartaDeInstrucciones.ValorInicialEnDolares;
                    cmd.Parameters.Add("@ValorFinalEnDolares", SqlDbType.Money, 4).Value = objCartaDeInstrucciones.ValorFinalEnDolares;
                    cmd.Parameters.Add("@IDCategoria", SqlDbType.Int, 4).Value = objCartaDeInstrucciones.IDCategoria;
                    cmd.Parameters.Add("@Grupo", SqlDbType.VarChar, 50).Value = objCartaDeInstrucciones.Grupo;
                    cmd.Parameters.Add("@ClavedePedimento", SqlDbType.VarChar, 2).Value = objCartaDeInstrucciones.ClavedePedimento;
                    cmd.Parameters.Add("@Patente", SqlDbType.Int, 4).Value = objCartaDeInstrucciones.Patente;
                    cmd.Parameters.Add("@NombreAA", SqlDbType.VarChar, 120).Value = objCartaDeInstrucciones.NombreAA;
                    cmd.Parameters.Add("@Observaciones", SqlDbType.Text).Value = objCartaDeInstrucciones.Observaciones;
                    cmd.Parameters.Add("@Activa", SqlDbType.Bit, 4).Value = objCartaDeInstrucciones.Activa;
                    cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4).Value = objCartaDeInstrucciones.Operacion;
                    cmd.Parameters.Add("@FechaVigencia", SqlDbType.DateTime).Value = objCartaDeInstrucciones.FechaVigencia;
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = objCartaDeInstrucciones.Email;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        {
                            id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                        }
                        else
                        {
                            id = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return id;
        }

        public bool Eliminar(int IDCarta)
        {
            //int id = 0;
            bool result = false;
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_DELETE_CARTADEINSTRUCCIONES", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCARTA", SqlDbType.Int, 4).Value = IDCarta;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    using (cmd.ExecuteReader())
                    {
                        //if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        //{
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) == 1)
                        {
                            result = true;
                        }

                        //}                  
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return result;
        }

        public List<CartaInstrucciones> CargarporOficina(int IdCliente, int IdOficina)
        {
            var result = new List<CartaInstrucciones>();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_CARTADEINSTRUCCIONES", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4).Value = IdCliente;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = IdOficina;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            CartaInstrucciones objCARTADEINSTRUCCIONES = new();
                            objCARTADEINSTRUCCIONES.IDCarta = Convert.ToInt32(dr["ID Carta"]);
                            objCARTADEINSTRUCCIONES.ValorInicialEnDolares = (decimal)Convert.ToDouble(string.Format("{0:0.00}", dr["Valor Inicial En Dolares"]));
                            objCARTADEINSTRUCCIONES.ValorFinalEnDolares = (decimal)Convert.ToDouble(string.Format("{0:0.00}", dr["Valor Final En Dolares"]));
                            objCARTADEINSTRUCCIONES.CategoriaDescripcion = dr["Categoria"].ToString();
                            objCARTADEINSTRUCCIONES.Grupo = dr["Grupo"].ToString();
                            objCARTADEINSTRUCCIONES.ClavedePedimento = dr["Clave de Pedimento"].ToString();
                            objCARTADEINSTRUCCIONES.Patente = Convert.ToInt32(dr["Patente"]);
                            objCARTADEINSTRUCCIONES.Observaciones = dr["Observaciones"].ToString();
                            objCARTADEINSTRUCCIONES.Operacion = Convert.ToInt32(dr["Operacion"]);

                            //objCARTADEINSTRUCCIONES.FechaVigencia = Convert.ToDateTime(dr["FechaVigencia"]);

                            if (dr["FechaVigencia"].ToString() != "__/__/____")
                            {
                                objCARTADEINSTRUCCIONES.FechaVigencia = Convert.ToDateTime(dr["FechaVigencia"]);
                            }
                            else
                            {
                                objCARTADEINSTRUCCIONES.FechaVigencia = DateTime.MinValue;
                            }

                            objCARTADEINSTRUCCIONES.Email = dr["Email"].ToString();
                            objCARTADEINSTRUCCIONES.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                            result.Add(objCARTADEINSTRUCCIONES);

                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error: " + sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general: " + ex.Message, ex);
            }            
            return result;
        }

        public void ValidarCarta(CartaInstrucciones objCartaInstrucciones)
        {
            try
            {

                if (objCartaInstrucciones.IdCliente == 0)
                {
                    throw new Exception("Ingrese el Cliente.");
                }

                if (objCartaInstrucciones.IDCategoria == 0)
                {
                    throw new Exception("Ingrese la Categoria.");
                }

                if (string.IsNullOrEmpty(objCartaInstrucciones.Grupo))
                {
                    throw new Exception("Ingrese el Grupo.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

        }

    }
}

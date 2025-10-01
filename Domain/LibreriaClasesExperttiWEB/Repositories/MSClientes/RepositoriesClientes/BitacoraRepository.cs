using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class BitacoraRepository : IBitacoraRepository
    {     
        public string SConexion { get; set; }
        string IBitacoraRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public BitacoraRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public int Insertar(Bitacora objBitacora)
        {
            int IDBitacora = 0;
            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_INSERT_BITACORADECLIENTES", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = objBitacora.IDCliente;
                    cmd.Parameters.Add("@Estatus", SqlDbType.SmallInt, 4).Value = objBitacora.Estatus;
                    cmd.Parameters.Add("@IDUsuario", SqlDbType.Int, 4).Value = objBitacora.IDUsuario;
                    cmd.Parameters.Add("@Observaciones", SqlDbType.VarChar, 250).Value = objBitacora.Observaciones;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {
                        IDBitacora = Convert.ToInt32(cmd.Parameters.Contains("@newid_registro"));
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString() + "NET_INSERT_BITACORADECLIENTES");
            }
            return IDBitacora;
        }
        public Bitacora LoadBitacora(int IDCliente, int IDCapturo, string Observaciones, string Metodo)
        {
            Bitacora objBitacora = new();

            try
            {
                if (Metodo == "POST")
                {

                    objBitacora.IDCliente = IDCliente;
                    objBitacora.Estatus = 3;
                    objBitacora.IDUsuario = IDCapturo;
                    objBitacora.Observaciones = "ALTA DE CLIENTE";
                }
                if (Metodo == "PUT")
                {
                    objBitacora.IDCliente = IDCliente;
                    objBitacora.Estatus = 2;
                    objBitacora.IDUsuario = IDCapturo;
                    objBitacora.Observaciones = Observaciones;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return objBitacora;
        }

        public List<BitacoraDeClientesDataGridView> Cargar(int MyIDCliente)
        {
            List<BitacoraDeClientesDataGridView> lstBitacora = new();

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_BITACORADECLIENTES", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = MyIDCliente;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            BitacoraDeClientesDataGridView objBitacora = new();
                            objBitacora.Evento = dr["Evento"].ToString();
                            objBitacora.Nombre = dr["Nombre"].ToString();
                            objBitacora.Fecha = dr["Fecha"].ToString();
                            objBitacora.Observaciones = dr["Observaciones"].ToString();

                            lstBitacora.Add(objBitacora);
                        }
                    }
                    else
                        lstBitacora = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lstBitacora;
        }
    }
}

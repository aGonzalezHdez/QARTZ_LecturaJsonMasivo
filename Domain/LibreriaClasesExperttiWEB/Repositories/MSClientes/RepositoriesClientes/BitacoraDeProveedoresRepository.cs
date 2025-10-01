using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class BitacoraDeProveedoresRepository : IBitacoraDeProveedoresRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public BitacoraDeProveedoresRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public BitacoraDeProveedores Buscar(string CveProv)
        {
            BitacoraDeProveedores objBITACORADEPROVEEDORES = new();

            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_BITACORADEPROVEEDORES", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CveProv", SqlDbType.VarChar, 6).Value = CveProv;
                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        dr.Read();
                        objBITACORADEPROVEEDORES.idBitacoradeProveedores = Convert.ToInt32(dr["idBitacoradeProveedores"]);
                        objBITACORADEPROVEEDORES.CveProv = dr["CveProv"].ToString();
                        objBITACORADEPROVEEDORES.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                        objBITACORADEPROVEEDORES.Fecha = Convert.ToDateTime(dr["Fecha"]);
                        objBITACORADEPROVEEDORES.Tipo = Convert.ToInt32(dr["Tipo"]);
                        objBITACORADEPROVEEDORES.IdCliente = Convert.ToInt32(dr["IdCliente"]);
                        objBITACORADEPROVEEDORES.Estatus = Convert.ToInt32(dr["Estatus"]);
                        objBITACORADEPROVEEDORES.NombreCliente = dr["Nombre"].ToString();
                        objBITACORADEPROVEEDORES.Ultimo = dr["Ultimo"].ToString();
                        objBITACORADEPROVEEDORES.Primero = dr["Primero"].ToString();
                        objBITACORADEPROVEEDORES.Siguiente = dr["Siguiente"].ToString();
                        objBITACORADEPROVEEDORES.Anterior = dr["Anterior"].ToString();
                    }
                    else
                        objBITACORADEPROVEEDORES = null/* TODO Change to default(_) if this is not a reference type */;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objBITACORADEPROVEEDORES;
        }

        public List<BitacoraClientProv> LlenaDataGridViewBitacoraClienteProv(string cveprov)
        {
            List<BitacoraClientProv> lstbitaclientprov = new();

            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_BITACORACLIENTEPROV_PARA_DATAGRIDVIEW", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@cveprov", SqlDbType.VarChar, 6).Value = cveprov;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            BitacoraClientProv objbitaclientprov = new();
                            objbitaclientprov.Estatus = dr["Evento"].ToString();
                            objbitaclientprov.NombreCliente = dr["Nombre"].ToString();
                            objbitaclientprov.Fecha = dr["Fecha"].ToString();

                            lstbitaclientprov.Add(objbitaclientprov);
                        }
                    }
                    else
                        lstbitaclientprov = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return lstbitaclientprov;
        }
    }
}

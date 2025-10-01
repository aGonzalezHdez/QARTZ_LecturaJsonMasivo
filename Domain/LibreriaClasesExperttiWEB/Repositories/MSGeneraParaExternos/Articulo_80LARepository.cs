using LibreriaClasesAPIExpertti.Entities.EntitiesGeneraParaExternos;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPedimentos;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSGeneraParaExternos
{
    public class Articulo_80LARepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public Articulo_80LARepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbMSG")!;

        }
        //public int Insertar(List<Articulo_80LA> lstArticulo_80LA)
        //{
        //    int id;
        //    var cn = new SqlConnection();
        //    var cmd = new SqlCommand();
        //    SqlParameter @param;

        //    try
        //    {

        //        DataTable dtb = new DataTable();
        //        ListtoDataTableConverter x = new ListtoDataTableConverter();
        //        dtb = x.ToDataTable(lstArticulo_80LA);


        //        cn.ConnectionString = sConexion;
        //        cn.Open();

        //        cmd.CommandText = "NET_VALIDATE_Articulo_80LA";
        //        cmd.Connection = cn;
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.Add("@Tabla", SqlDbType.Structured).Value = dtb;

        //        @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
        //        @param.Direction = ParameterDirection.Output;

        //        cmd.ExecuteNonQuery();

        //        if ((int)cmd.Parameters["@newid_registro"].Value != -1)
        //        {
        //            id = (int)cmd.Parameters["@newid_registro"].Value;
        //        }
        //        else
        //        {
        //            id = 0;
        //        }

        //        cmd.Parameters.Clear();
        //    }
        //    catch (Exception ex)
        //    {
        //        id = 0;
        //        cn.Close();
        //        cn.Dispose();
        //        throw new Exception(ex.Message.ToString() + "NET_INSERT_Articulo_80LA");
        //    }
        //    cn.Close();
        //    cn.Dispose();
        //    return id;
        //}

        public List<RespuestaSR> Guardar(List<Articulo_80LA> lstArticulo_80LA)
        {
            List<RespuestaSR> lstRespuestaSR =new List<RespuestaSR>();
            try
            {
                DataTable dtb = new DataTable();
                ListtoDataTableConverter x = new ListtoDataTableConverter();
                dtb = x.ToDataTable(lstArticulo_80LA);


                using (cn = new SqlConnection(sConexion))

                using (SqlCommand cmd = new("NET_VALIDATE_Articulo_78LA", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.Add("@Tabla", SqlDbType.Structured).Value = dtb;
                    
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RespuestaSR objRespuestaSR = new RespuestaSR();
                            objRespuestaSR.GuiaHouse = dr["GuiaHouse"].ToString();
                            objRespuestaSR.Errores = dr["Errores"].ToString();

                            lstRespuestaSR.Add(objRespuestaSR);
                        }
                       
                     
                    }
                    else
                    {
                        lstRespuestaSR = null!;
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lstRespuestaSR;
        }

    }
}

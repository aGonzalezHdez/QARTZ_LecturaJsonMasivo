using LibreriaClasesAPIExpertti.Entities.EntitiesProyectos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSSoporte.Proyectos
{
    public class EventosDesarrolloRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        public EventosDesarrolloRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<EventosDesarrollo> Cargar(int IdDetalle)
        {
            List<EventosDesarrollo> lista = new ();

            var objREFERENCIAS = new Referencias();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_CASAEI_EVENTOSDESARROLLO";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@IdDetalle", SqlDbType.Int, 4);
            @param.Value = IdDetalle;



            // @IDDatosDeEmpresa
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        EventosDesarrollo objEventosDesarrollo = new();
                        objEventosDesarrollo.Estatus = (dr["Estatus"]).ToString();
                        objEventosDesarrollo.NombreUsuario = (dr["NombreUsuario"]).ToString();
                        objEventosDesarrollo.Fecha = Convert.ToDateTime(dr["Fecha"]);

                        lista.Add(objEventosDesarrollo);
                    }
                }
                else
                {
                    lista.Clear();
                }
                dr.Close();
                // cn.Close()
                // SqlConnection.ClearPool(cn)
                // cn.Dispose()
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString());
            }
            cn.Close();
            cn.Dispose();
            return lista;
        }
    }
}

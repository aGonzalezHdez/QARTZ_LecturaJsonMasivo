using LibreriaClasesAPIExpertti.Entities.EntitiesAnam;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSDigitalizar;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepisitoriesRepadi
{
    public class RepadiRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public RepadiRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public RequestAnam DatosREPADI(int idReferencia, int IDDatosDeEmpresa,int NoRemesa, string DocumentoBase64 , string TipoDeDocumento )
        {

            var objRequestAnam = new RequestAnam();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_DATOS_REPADI";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
            @param.Value = idReferencia;

            @param = cmd.Parameters.Add("@IDDatosDeEmpresa", SqlDbType.Int, 4);
            @param.Value = IDDatosDeEmpresa;

            @param = cmd.Parameters.Add("@NoRemesa", SqlDbType.Int, 4);
            @param.Value = NoRemesa;

            // @IDDatosDeEmpresa

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objRequestAnam.Aduana  = dr["Aduana"].ToString();

                    objRequestAnam.Consecutivo =Int32.Parse(dr["Consecutivo"].ToString());                    
                    objRequestAnam.FolioMaster = dr["FolioMaster"].ToString();
                    
                    objRequestAnam.Aduana = dr["Aduana"].ToString();
                    objRequestAnam.Patente = dr["Patente"].ToString();
                    objRequestAnam.Email = dr["Email"].ToString();


                    Entities.EntitiesAnam.Documento obj = new Entities.EntitiesAnam.Documento();
                    obj.UsuarioConsulta = dr["usuario_consulta"].ToString();                    
                    obj.MedioTransporte = dr["medio_transporte"].ToString();
                    obj.TipoDocumento = TipoDeDocumento;
                    obj.Archivo = DocumentoBase64;

                    List<Entities.EntitiesAnam.Documento> listDocs = new();
                    listDocs.Add(obj);
                    
                    objRequestAnam.Documentos = listDocs;



                }
                else
                {
                    objRequestAnam = default;
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
            return objRequestAnam;
        }
    }
}

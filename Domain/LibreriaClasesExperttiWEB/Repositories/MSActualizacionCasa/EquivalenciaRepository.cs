using LibreriaClasesAPIExpertti.Entities.EntitiesDespacho;
using LibreriaClasesAPIExpertti.Entities.EntitiesEquialencia;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace LibreriaClasesAPIExpertti.Repositories.MSActualizacionCasa
{
    public class EquivalenciaRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        

        public EquivalenciaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
            
        }

        public List<CargaArchivos> ConsultaRegistros(out string mensaje)
        {
            var list = new List<CargaArchivos>();

            using (var cn = new SqlConnection(sConexion))
            using (var cmd = new SqlCommand("NET_LOAD_REGISTROS_POR_ARCHIVO", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var obj = new CargaArchivos
                            {
                                Nombre = dr["Nombre"] != DBNull.Value ? dr["Nombre"].ToString() : string.Empty,
                                NumRegistros = dr["NumRegistros"] != DBNull.Value ? Convert.ToInt32(dr["NumRegistros"]) : 0,
                            };
                            list.Add(obj);
                        }
                    }
                    mensaje = string.Empty;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return list;
        }

        public List<CargaArchivos> ConsultaRegistrosXmlPrevio(out string mensaje)
        {
            var list = new List<CargaArchivos>();

            using (var cn = new SqlConnection(sConexion))
            using (var cmd = new SqlCommand("NET_LOAD_REGISTROS_POR_ARCHIVO_P", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var obj = new CargaArchivos
                            {
                                Nombre = dr["Nombre"] != DBNull.Value ? dr["Nombre"].ToString() : string.Empty,
                                NumRegistros = dr["NumRegistros"] != DBNull.Value ? Convert.ToInt32(dr["NumRegistros"]) : 0,
                            };
                            list.Add(obj);
                        }
                    }
                    mensaje = string.Empty;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return list;
        }

        public List<ProcedimientosInsertarXML> GetProcedimientosInsertCasaXML()
        {
            var list = new List<ProcedimientosInsertarXML>();

            using (var cn = new SqlConnection(sConexion))
            using (var cmd = new SqlCommand("NET_LOAD_PROCEDIMIENTOS_INSERT_CASA_XML", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ProcedimientosInsertarXML procedimientosInsertarXML = new ProcedimientosInsertarXML();
                            string tabla = dr["Tabla"] != DBNull.Value ? dr["Tabla"].ToString() : string.Empty;
                            string procedimiento = dr["Procedimiento"] != DBNull.Value ? dr["Procedimiento"].ToString() : string.Empty;
                            int plano = dr["Plano"] != DBNull.Value ? Convert.ToInt32(dr["Plano"]) : 0;
                            procedimientosInsertarXML.Tabla = tabla;
                            procedimientosInsertarXML.Procedimiento = procedimiento;
                            procedimientosInsertarXML.Plano = plano;
                            list.Add(procedimientosInsertarXML);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener procedimientos: " + ex.Message);
                }
            }

            return list;
        }


        public bool InsertarXml(string xml, string procedimientoAlmacenado, out string mensaje)
        {
            mensaje = string.Empty;
            bool resultado = false;
            
            try
            {
                List<string> lotesXml = DividirXmlEnLotes(xml, 5000);
                bool isFirstBatch = true; // Variable para el primer lote

                using (SqlConnection cn = new SqlConnection(sConexion))
                {
                    cn.Open();

                    for (int i = 0; i < lotesXml.Count; i++)
                    {
                        bool isLastBatch = (i == lotesXml.Count - 1); // Último lote

                        using (SqlCommand cmd = new SqlCommand(procedimientoAlmacenado, cn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            cmd.Parameters.AddWithValue("@Xml", lotesXml[i]);
                            cmd.Parameters.AddWithValue("@isFirstBatch", isFirstBatch ? 1 : 0); // 1 en la primera iteración, 0 después
                            cmd.Parameters.AddWithValue("@isLastBatch", isLastBatch ? 1 : 0); // 1 en la última iteración, 0 antes

                            cmd.ExecuteNonQuery();
                        }

                        isFirstBatch = false;
                    }
                }

                resultado = true;
            }
            catch (SqlException ex)
            {
                mensaje = "Error SQL en InsertarXml: " + ex.Message;
                resultado = false;
            }
            catch (Exception ex)
            {
                mensaje = "Error en InsertarXml: " + ex.Message;
                resultado = false;
            }

            return resultado;
        }


        private List<string> DividirXmlEnLotes(string xml, int tamañoLote)
        {
            List<string> lotes = new List<string>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNodeList nodos = doc.SelectNodes("/FDBS/Manager/TableList/Table/RowList/Row");
            int total = nodos.Count;
            int inicio = 0;

            while (inicio < total)
            {
                XmlDocument nuevoXml = new XmlDocument();
                XmlElement root = nuevoXml.CreateElement("FDBS");
                XmlElement manager = nuevoXml.CreateElement("Manager");
                XmlElement tableList = nuevoXml.CreateElement("TableList");
                XmlElement table = nuevoXml.CreateElement("Table");
                XmlElement rowList = nuevoXml.CreateElement("RowList");

                nuevoXml.AppendChild(root);
                root.AppendChild(manager);
                manager.AppendChild(tableList);
                tableList.AppendChild(table);
                table.AppendChild(rowList);

                for (int i = inicio; i < Math.Min(inicio + tamañoLote, total); i++)
                {
                    XmlNode nuevoNodo = nuevoXml.ImportNode(nodos[i], true);
                    rowList.AppendChild(nuevoNodo);
                }

                lotes.Add(nuevoXml.OuterXml);
                inicio += tamañoLote;
            }

            return lotes;
        }
    }
}
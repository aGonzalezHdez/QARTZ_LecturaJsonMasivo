using DocumentFormat.OpenXml.Office2010.Excel;
using LibreriaClasesAPIExpertti.Entities.EntitiesManifestacion;
using LibreriaClasesAPIExpertti.Entities.EntitiesUsuarios;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesManifestacion.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesManifestacion
{

    public class IncrementablesDecrementablesMVRepository : IIncrementablesDecrementablesMVRepository
    {
        public string SConexion { get; set; }
        string IIncrementablesDecrementablesMVRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public IncrementablesDecrementablesMVRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }



        public int Insertar(IncrementablesDecrementablesMV objIncrementablesDecrementablesMV)
        {
            int Id = 0;

            try
            {
                using SqlConnection con = new(SConexion);
                using var cmd = new SqlCommand("NET_INSERT_CASAEI_IncrementablesDecrementablesMV", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@idIncrementable", SqlDbType.Int, 4).Value = objIncrementablesDecrementablesMV.idIncrementable;
                cmd.Parameters.Add("@tipoIncrementable", SqlDbType.VarChar, 10).Value = objIncrementablesDecrementablesMV.tipoIncrementable;
                cmd.Parameters.Add("@fechaErogacion", SqlDbType.DateTime).Value = objIncrementablesDecrementablesMV.fechaErogacion;
                cmd.Parameters.Add("@importe", SqlDbType.Decimal).Value = objIncrementablesDecrementablesMV.importe;
                cmd.Parameters.Add("@tipoMoneda", SqlDbType.VarChar, 3).Value = objIncrementablesDecrementablesMV.tipoMoneda;
                cmd.Parameters.Add("@aCargoImportador", SqlDbType.Bit).Value = objIncrementablesDecrementablesMV.aCargoImportador;
                cmd.Parameters.Add("@incrementable", SqlDbType.Bit).Value = objIncrementablesDecrementablesMV.incrementable;
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

        public bool Modificar(IncrementablesDecrementablesMV objIncrementablesDecrementablesMV)
        {
            bool SiNo = false;

            try
            {
                using SqlConnection con = new(SConexion);
                using var cmd = new SqlCommand("NET_UPDATE_CASAEI_IncrementablesDecrementablesMV", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@idIncrementable", SqlDbType.Int, 4).Value = objIncrementablesDecrementablesMV.idIncrementable;
                cmd.Parameters.Add("@tipoIncrementable", SqlDbType.VarChar, 10).Value = objIncrementablesDecrementablesMV.tipoIncrementable;
                cmd.Parameters.Add("@fechaErogacion", SqlDbType.DateTime).Value = objIncrementablesDecrementablesMV.fechaErogacion;
                cmd.Parameters.Add("@importe", SqlDbType.Decimal).Value = objIncrementablesDecrementablesMV.importe;
                cmd.Parameters.Add("@tipoMoneda", SqlDbType.VarChar, 3).Value = objIncrementablesDecrementablesMV.tipoMoneda;
                cmd.Parameters.Add("@aCargoImportador", SqlDbType.Bit).Value = objIncrementablesDecrementablesMV.aCargoImportador;
                cmd.Parameters.Add("@incrementable", SqlDbType.Bit).Value = objIncrementablesDecrementablesMV.incrementable;
                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                con.Open();
                using SqlDataReader dr = cmd.ExecuteReader();
                {
                    if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                    {
                        SiNo = Convert.ToBoolean(cmd.Parameters["@newid_registro"].Value);
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
            return SiNo;
        }

        public bool Eliminar(int idIncrementable)
        {
            bool id = false;
            try
            {
                using SqlConnection con = new(SConexion);
                using var cmd = new SqlCommand("NET_DELETE_CASAEI_IncrementablesDecrementablesMV", con);
                                   
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@idIncrementable", SqlDbType.Int, 4).Value = idIncrementable;
                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();

                id = Convert.ToBoolean(cmd.Parameters["@newid_registro"].Value);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return id;
        }

        //Comentado por IVBM ya que no existe el sp que se esta invocando 30/09/2025
        //public IncrementablesDecrementablesMV Buscar(int idIncrementable)
        //{
        //    IncrementablesDecrementablesMV objIncrementablesDecrementablesMV = new();
        //    try
        //    {
        //        using SqlConnection con = new(SConexion);
        //        using SqlCommand cmd = new("NET_SEARCH_CASAEI_IncrementablesDecrementablesMV", con);
        //        {
        //            con.Open();
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.Add("@idIncrementable", SqlDbType.VarChar, 10).Value = idIncrementable;

        //            using SqlDataReader dr = cmd.ExecuteReader();
        //            if (dr.HasRows)
        //            {
        //                dr.Read();
        //                objIncrementablesDecrementablesMV.idIncrementable = Convert.ToInt32(dr["idIncrementable"]);
        //                objIncrementablesDecrementablesMV.tipoIncrementable = Convert.ToString(dr["tipoIncrementable"]);
        //                objIncrementablesDecrementablesMV.fechaErogacion = Convert.ToDateTime(dr["fechaErogacion"]);
        //                objIncrementablesDecrementablesMV.importe = Convert.ToDecimal(dr["importe"]);
        //                objIncrementablesDecrementablesMV.tipoMoneda = dr["tipoMoneda"].ToString();
        //                objIncrementablesDecrementablesMV.aCargoImportador = Convert.ToBoolean(dr["aCargoImportador"]);
        //                objIncrementablesDecrementablesMV.incrementable = Convert.ToBoolean(dr["incrementable"]);
                       
                     
        //            }
        //            else
        //            {
        //                objIncrementablesDecrementablesMV = null!;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message.ToString());
        //    }

        //    return objIncrementablesDecrementablesMV;
        //}
        public List<IncrementablesDecrementablesMV> Cargar()
        {
            List<IncrementablesDecrementablesMV>? listIncrementablesDecrementablesMV = new();
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_LOAD_CASAEI_IncrementablesDecrementablesMV", con);
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;                   

                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            IncrementablesDecrementablesMV objIncrementablesDecrementablesMV = new()
                            {
                                idIncrementable = Convert.ToInt32(dr["idIncrementable"]),
                                tipoIncrementable = dr["tipoIncrementable"].ToString(),
                                fechaErogacion = Convert.ToDateTime(dr["fechaErogacion"]),
                                importe = Convert.ToDecimal(dr["importe"]),
                                tipoMoneda = dr["tipoMoneda"].ToString(),
                                aCargoImportador = Convert.ToBoolean(dr["aCargoImportador"]),
                                incrementable = Convert.ToBoolean(dr["incrementable"]),
                                DescLegal = (dr["DESC_LEGAL"]).ToString()
                            };
                            listIncrementablesDecrementablesMV.Add(objIncrementablesDecrementablesMV);                        }
                    }
                    else
                    {
                        listIncrementablesDecrementablesMV.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return listIncrementablesDecrementablesMV;
        }
    }
}

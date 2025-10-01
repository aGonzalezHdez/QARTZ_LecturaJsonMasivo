using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaClasesAPIExpertti.Entities.EntitiesDespacho;

namespace LibreriaClasesAPIExpertti.Repositories.MSDespachos.RepositoriesImportacion
{
    public class RecoleccionRepository
    {
        IConfiguration _configuration;
        public string SConexion { get; set; }
        public RecoleccionRepository(IConfiguration configuracion)
        {
            _configuration = configuracion;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int InsertarBitacora(BitacoraDetalle lbitacoradetalle)
        {
            int id;
            using (var cn = new SqlConnection(SConexion))
            using (var cmd = new SqlCommand("fdx.NET_INSERT_BitacoraDetalleCourier", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@IdRelacionBitacora", SqlDbType.Int).Value = lbitacoradetalle.IdRelacionBitacora;
                cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 15).Value = lbitacoradetalle.GuiaHouse;
                cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 250).Value = lbitacoradetalle.Descripcion;
                cmd.Parameters.Add("@DescripcionIngles", SqlDbType.VarChar, 250).Value = lbitacoradetalle.DescripcionIngles;
                cmd.Parameters.Add("@Latitud", SqlDbType.VarChar, 20).Value = lbitacoradetalle.Latitud;
                cmd.Parameters.Add("@Longitud", SqlDbType.VarChar, 20).Value = lbitacoradetalle.Longitud;
                cmd.Parameters.Add("@CodigoPostal", SqlDbType.VarChar, 50).Value = lbitacoradetalle.CodigoPostal;
                cmd.Parameters.Add("@ValorDolares", SqlDbType.Decimal).Value = lbitacoradetalle.ValorDolares;
                cmd.Parameters.Add("@CantidadDeBultos", SqlDbType.Int).Value = lbitacoradetalle.CantidadDeBultos;
                cmd.Parameters.Add("@Peso", SqlDbType.Decimal).Value = lbitacoradetalle.Peso;
                cmd.Parameters.Add("@FechaAsignacion", SqlDbType.DateTime).Value = lbitacoradetalle.FechaAsignacion;
                cmd.Parameters.Add("@FechaLlegada", SqlDbType.DateTime).Value = lbitacoradetalle.FechaLlegada;
                cmd.Parameters.Add("@FechaSalida", SqlDbType.DateTime).Value = lbitacoradetalle.FechaSalida;
                cmd.Parameters.Add("@Moneda", SqlDbType.VarChar, 3).Value = lbitacoradetalle.Moneda;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = lbitacoradetalle.IdUsuario;
                cmd.Parameters.Add("@tipo", SqlDbType.Int).Value = lbitacoradetalle.Tipo;
                cmd.Parameters.Add("@cliente", SqlDbType.VarChar, 250).Value = lbitacoradetalle.Cliente;
                cmd.Parameters.Add("@embalaje", SqlDbType.Bit).Value = lbitacoradetalle.Embalaje;
                cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = lbitacoradetalle.Activo;
                cmd.Parameters.Add("@TipoServicio", SqlDbType.Int).Value = lbitacoradetalle.TipoServicio;
                cmd.Parameters.Add("@IDDatosDeEmpresa", SqlDbType.Int).Value = lbitacoradetalle.IDDatosDeEmpresa;
                cmd.Parameters.Add("@Flujo", SqlDbType.VarChar, 10).Value = lbitacoradetalle.Flujo ?? (object)DBNull.Value;

                var paramId = new SqlParameter("@newid_registro", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(paramId);

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    id = (int)paramId.Value;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " fdx.NET_INSERT_BitacoraDetalleCourier");
                }
            }
            return id;
        }

    }
}

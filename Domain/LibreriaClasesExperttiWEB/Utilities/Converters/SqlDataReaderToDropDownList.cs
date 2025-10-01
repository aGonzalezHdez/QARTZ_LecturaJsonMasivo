
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;

namespace LibreriaClasesAPIExpertti.Utilities.Converters
{
    public class SqlDataReaderToDropDownList
    {
        //public class Datos
        //{

        //    public int Id { get; set; }
        //    public string Descripcion { get; set; }

        //}
        public static List<DropDownListDatos> DropDownList<T>(SqlDataReader reader)
        {
            List<DropDownListDatos> listDatos = new();

            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    DropDownListDatos d = new()
                    {
                        Id = Convert.ToInt32(reader[0]),
                        Descripcion = reader[1].ToString()
                    };

                    listDatos.Add(d);
                }
            }
            return listDatos;
        }

        public static List<DropDownListDatosColor> DropDownListColor<T>(SqlDataReader reader)
        {
            List<DropDownListDatosColor> listDatos = new();

            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    DropDownListDatosColor d = new()
                    {
                        Id = Convert.ToInt32(reader[0]),
                        Descripcion = reader[1].ToString(),
                        Color = reader[2].ToString()
                    };

                    listDatos.Add(d);
                }
            }
            return listDatos;
        }
    }
}

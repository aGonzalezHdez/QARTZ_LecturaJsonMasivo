
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;

namespace LibreriaClasesAPIExpertti.Utilities.Converters
{
    public class SqlDataReaderToDropDownListString
    {
        public static List<DropDownListDatosString> DropDownListString<T>(SqlDataReader reader)
        {
            List<DropDownListDatosString> listDatos = new();

            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    DropDownListDatosString d = new()
                    {
                        Id = reader[0].ToString(),
                        Descripcion = reader[1].ToString()
                    };

                    listDatos.Add(d);
                }
            }
            return listDatos;
        }
    }
}

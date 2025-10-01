using System.Data.SqlClient;
using System.Reflection;

namespace LibreriaClasesAPIExpertti.Utilities.Converters
{
    public class SqlDataReaderToList
    {
        public static List<T> DataReaderMapToList<T>(SqlDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default;

            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    bool hasMyColumn = dr.GetSchemaTable().Select("ColumnName = '" + prop.Name + "'").Count() == 1;
                    if (hasMyColumn)
                    {
                        if (!Equals(dr[prop.Name], DBNull.Value))
                        {
                            prop.SetValue(obj, dr[prop.Name], null);
                        }
                    }

                }
                list.Add(obj);
            }
            return list;
        }
    }
}

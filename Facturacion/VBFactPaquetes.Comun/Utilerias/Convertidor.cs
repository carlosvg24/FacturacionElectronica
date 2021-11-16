using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VBFactPaquetes.Comun.Log;

namespace VBFactPaquetes.Comun.Utilerias
{
    /// <summary>
    /// Convierte un objetos en diferentes tipo
    /// </summary>
    public static class Convertidor
    {

        public static Dictionary<string, string> DataTableToDiccionario(DataTable dt, string llave, string valor)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                diccionario.Add(dt.Rows[i][llave].ToString(), dt.Rows[i][valor].ToString());
            }

            return diccionario;
        }

        /// <summary>
        /// Convierte una lista tipada en DataTable
        /// </summary>
        /// <typeparam name="T">Objeto</typeparam>
        /// <param name="data">Lista Tipada a convertir</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                dt.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }

        /// <summary>
        /// Convierte un DataTable en Lista tipada
        /// </summary>
        /// <typeparam name="T">Objeto</typeparam>
        /// <param name="dt">DataTable a convertir</param>
        /// <returns></returns>
        public static List<T> ToList<T>(DataTable dt)
        {
            List<T> list = new List<T>();
            DataRow row;

            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    row = dt.Rows[i];
                    T item = GetItem<T>(row);
                    list.Add(item);
                }
            }
            catch (Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("dt", dt);
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                                      MethodBase.GetCurrentMethod().Name,
                                                      parametros,
                                                      ex, Excepciones.TipoPortal.VivaPaquetes);
            }

            return list;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type tipo = typeof(T);
            T obj = Activator.CreateInstance<T>();
            DataColumn column = new DataColumn();
            PropertyInfo pro;
            object valor = null;

            try
            {
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    column = dr.Table.Columns[i];
                    for (int j = 0; j < tipo.GetProperties().Length; j++)
                    //foreach (PropertyInfo pro in tipo.GetProperties())
                    {
                        pro = tipo.GetProperties()[j];

                        if (pro.Name == column.ColumnName)
                        {
                            valor = dr[column.ColumnName];
                            valor = valor.GetType().Name == "DBNull" ? null : valor;
                            pro.SetValue(obj, valor, null);
                            break;
                        }
                        else
                            continue;
                    }
                }
            }
            catch (Exception ex)
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("dr", dr);
                parametros.Add("ColumnName", column.ColumnName);
                parametros.Add("valor", valor);
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                                      MethodBase.GetCurrentMethod().Name,
                                                      parametros,
                                                      ex, Excepciones.TipoPortal.VivaPaquetes);
            }

            return obj;
        }

        /// <summary>
        /// Convierte un valor nulo de entero en vacio
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static int EmptyToZero(String valor)
        {
            try
            {
                if (String.IsNullOrEmpty((string)valor))
                    return 0;
                else
                    return int.Parse((string)valor);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// Convierte un valor nulo a Empty
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string NullToEmpty(String valor)
        {
            try
            {
                if (String.IsNullOrEmpty(valor))
                    return String.Empty;
                else
                    return valor;
            }
            catch (Exception ex)
            {

                return String.Empty;
            }
        }

        /// <summary>
        /// Convierte un valor DBNull a un mínimo en DateTime
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static DateTime DBNullToMinDate(String valor)
        {
            try
            {
                if (String.IsNullOrEmpty(valor))
                    return DateTime.MinValue;
                else
                    return DateTime.Parse(valor);
            }
            catch (Exception)
            {

                return DateTime.MinValue;
            }
        }


        /// <summary>
        /// Convierte una instancia de una clase en diccionario
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clase"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ClaseToDiccionario<T>(object clase)
        {
            var obj = ((T)clase);
            Dictionary<string, string> dicccionario = new Dictionary<string, string>();

            PropertyInfo[] infos = obj.GetType().GetProperties();

            foreach (PropertyInfo info in infos)
            {
                var valor = info.GetValue(obj, null);
                valor = valor == null ? "" : valor;
                dicccionario.Add(info.Name, valor.ToString());
            }

            return dicccionario;
        }

        public static string DataTableToJSON(DataTable dt)
        {
            var JSONString = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }


    }



}

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IntusWindowsInterview.Common
{
    public static class Utilities
    {
        // Public Methods
        public static DateTime GetDate()
        {
            return DateTime.Now;
        }
        public static string GetRequestResponseTime()
        {
            return DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
        public static List<T> ConvertFromDataTable<T>(DataTable dt) where T : new()
        {
            var items = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                T obj = new T();
                foreach (DataColumn col in dt.Columns)
                {
                    try
                    {
                        var objProperty = obj.GetType().GetProperty(col.ColumnName);
                        if (objProperty == null) continue;

                        var value = row.Field<dynamic>(col.ColumnName);
                        if (value == null) continue;

                        var propType = objProperty.PropertyType.FullName;

                        if (propType.Contains("System.Double") && !(value is double))
                        {
                            value = value is string && value == "" ? 0 : Convert.ToDouble(value);
                        }
                        else if (propType.Contains("System.Int64") && !(value is Int64))
                        {
                            value = value is string && value == "" ? (Int64)0 : Convert.ToInt64(value);
                        }
                        else if (propType.Contains("System.Int32") && !(value is Int32))
                        {
                            value = value is string && value == "" ? 0 : Convert.ToInt32(value);
                        }
                        else if (propType.Contains("System.Boolean") && !(value is bool))
                        {
                            value = value is string && value == "" ? false : Convert.ToBoolean(value);
                        }
                        objProperty.SetValue(obj, value);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                items.Add(obj);
            }

            return items;
        }
    }
}

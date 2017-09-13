#if UNITY_EDITOR
using Excel;
using LitJson;
using RPGGame.Table;
using RPGGame.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class ExcelResolver : Editor
{
    private class ExcelDto
    {
        public DataTable Data { get; set; }
        public string Path { get; set; }
    }
    private static int num = 0;
    private static string excelPath = Path.Combine(Application.dataPath, "Table");
    private static string outPath = Path.Combine(Application.dataPath, "Resources/Data/Table");
    private static List<ExcelDto> excelList = new List<ExcelDto>();

    [MenuItem("Tools/ReadExcel", priority = 1)]
    public static void ReadExcel()
    {
        excelList.Clear();
        num = 0;
        string[] paths = Directory.GetFiles(excelPath, "*.xlsx", SearchOption.AllDirectories);
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }
        Assembly assembly = Assembly.GetAssembly(typeof(FoodTable));
        EditorUtility.DisplayProgressBar("打包中...", "   ", (float)0);
        try
        {
            for (int i = 0; i < paths.Length; i++)
            {
                FileStream stream = File.Open(paths[i], FileMode.Open, FileAccess.Read, FileShare.Read);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                DataSet result = excelReader.AsDataSet();
                for (int j = 0; j < result.Tables.Count; j++)
                {
                    excelList.Add(new ExcelDto() { Data = result.Tables[j], Path = paths[i] });
                }
            }
            System.Collections.IEnumerator ienumerator = BuildExcel(assembly);
            while (ienumerator.MoveNext())
            {
                Thread.Sleep(500);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            EditorUtility.ClearProgressBar();
        }

        EditorUtility.ClearProgressBar();
    }

    static System.Collections.IEnumerator BuildExcel(Assembly assembly)
    {
        for (int i = 0; i < excelList.Count; i++)
        {
            BuildExcel(assembly, excelList[i].Data, excelList[i].Path);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private static void BuildExcel(Assembly assembly, DataTable dataTable, string path)
    {
        num++;
        string className = dataTable.TableName + "Table";// Path.GetFileNameWithoutExtension(path);
        Type myType = assembly.GetType("RPGGame.Table." + className);
        if (myType == null)
        {
            Debug.LogError("没找到RPGGame.Data.Tables." + className + "对应的Class");
            return;
        }
        PropertyInfo[] attrs = GetPropertyInfo(myType);
        if (attrs.Length < 1)
        {
            Debug.LogError(className + "类中没有属性！");
            return;
        }
        int columns = dataTable.Columns.Count;
        int rows = dataTable.Rows.Count;
        List<object> list = new List<object>();
        for (int j = 0; j < rows; j++)
        {
            if (j == 0)
            {
                continue;
            }
            string isContinue = dataTable.Rows[j][0].ToString();
            if (isContinue == "#")
            {
                continue;

            }
            object obj = Activator.CreateInstance(myType);
            for (int n = 0; n < columns; n++)
            {
                if (n == 0)
                {
                    continue;
                }
                string fieldName = dataTable.Rows[0][n].ToString();
                PropertyInfo propInfo = myType.GetProperty(fieldName, BindingFlags.Instance | BindingFlags.Public);
                if (propInfo == null)
                {
                    continue;
                }
                string nvalue = dataTable.Rows[j][n].ToString();
                switch (propInfo.PropertyType.Name)
                {
                    case "Int64":
                        propInfo.SetValue(obj, Convert.ToInt64(nvalue), null);
                        break;
                    case "Int32":
                        propInfo.SetValue(obj, Convert.ToInt32(nvalue), null);
                        break;
                    case "String":
                        propInfo.SetValue(obj, nvalue, null);
                        break;
                    case "Float":
                    case "Single":
                        propInfo.SetValue(obj, Convert.ToSingle(nvalue), null);
                        break;
                    case "Boolean":
                        bool re = nvalue == "1";
                        propInfo.SetValue(obj, re, null);
                        break;
                    default:
                        throw new Exception("未知类型" + propInfo.Name + "在" + className);
                }
            }
            list.Add(obj);
        }
        string content = JsonMapper.ToJson(list);
        FileUtils.Instance.WriteFile(EncryptUtils.Instance.Encryption(content, EncryptUtils.GlobalEncryptKey), Path.Combine(outPath, className));//+ ".bytes");
        //FileStream fs = File.Create(Path.Combine(outPath, className)+".bytes");
        //foreach (object item in list)
        //{
        //    RuntimeTypeModel @default = RuntimeTypeModel.Default;
        //    @default.SerializeWithLengthPrefix(fs, item, item.GetType(), PrefixStyle.Base128, 0);
        //}
        //fs.Close();
        //fs.Dispose();
        Debug.Log(Path.Combine(outPath, className) + "打包完成！");
        EditorUtility.DisplayProgressBar("打包中...", className + "   " + num + "/" + excelList.Count, (float)(num / (float)excelList.Count));

    }

    //private static void BuildExcel(Assembly assembly, DataSet result, string path)
    //{

    //    string className = Path.GetFileNameWithoutExtension(path);
    //    Type myType = assembly.GetType("RPGGame.Table." + className);
    //    if (myType == null)
    //    {
    //        Debug.LogError("没找到RPGGame.Data.Tables." + className + "对应的Class");
    //        return;
    //    }

    //    PropertyInfo[] attrs = GetPropertyInfo(myType);
    //    if (attrs.Length < 1)
    //    {
    //        Debug.LogError(className + "类中没有属性！");
    //        return;
    //    }
    //    int columns = result.Tables[0].Columns.Count;
    //    int rows = result.Tables[0].Rows.Count;
    //    List<object> list = new List<object>();

    //    for (int j = 0; j < rows; j++)
    //    {
    //        if (j == 0)
    //        {
    //            continue;
    //        }
    //        string isContinue = result.Tables[0].Rows[j][0].ToString();
    //        if (isContinue == "#")
    //        {
    //            continue;

    //        }
    //        object obj = Activator.CreateInstance(myType);
    //        for (int n = 0; n < columns; n++)
    //        {
    //            if (n == 0)
    //            {
    //                continue;
    //            }
    //            string fieldName = result.Tables[0].Rows[0][n].ToString();
    //            PropertyInfo propInfo = myType.GetProperty(fieldName, BindingFlags.Instance | BindingFlags.Public);
    //            if (propInfo == null)
    //            {
    //                continue;
    //            }
    //            string nvalue = result.Tables[0].Rows[j][n].ToString();
    //            switch (propInfo.PropertyType.Name)
    //            {
    //                case "Int64":
    //                    propInfo.SetValue(obj, Convert.ToInt64(nvalue), null);
    //                    break;
    //                case "Int32":
    //                    propInfo.SetValue(obj, Convert.ToInt32(nvalue), null);
    //                    break;
    //                case "String":
    //                    propInfo.SetValue(obj, nvalue, null);
    //                    break;
    //                case "Float":
    //                case "Single":
    //                    propInfo.SetValue(obj, Convert.ToSingle(nvalue), null);
    //                    break;
    //                case "Boolean":
    //                    bool re = nvalue == "1";
    //                    propInfo.SetValue(obj, re, null);
    //                    break;
    //                default:
    //                    throw new Exception("未知类型" + propInfo.Name + "在" + className);
    //            }
    //        }
    //        list.Add(obj);
    //    }
    //    string content = JsonMapper.ToJson(list);
    //    FileUtils.Instance.WriteFile(EncryptUtils.Instance.Encryption(content), Path.Combine(outPath, className) + ".bytes");
    //    //FileStream fs = File.Create(Path.Combine(outPath, className)+".bytes");
    //    //foreach (object item in list)
    //    //{
    //    //    RuntimeTypeModel @default = RuntimeTypeModel.Default;
    //    //    @default.SerializeWithLengthPrefix(fs, item, item.GetType(), PrefixStyle.Base128, 0);
    //    //}
    //    //fs.Close();
    //    //fs.Dispose();
    //    Debug.Log(Path.Combine(outPath, className) + "打包完成！");

    //}

    private static PropertyInfo[] GetPropertyInfo(Type type)
    {
        PropertyInfo[] props = null;
        try
        {
            //object obj = Activator.CreateInstance(type);
            props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return props;
        }
        catch (Exception ex)
        {

            throw new Exception("获取类型失败，" + type.Name + "===>" + ex.Message);
        }
    }
}
#endif
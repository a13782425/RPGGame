using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;
using System.Text;
using System.Security.Cryptography;
using System;

namespace RPGGame.Utils
{
    public sealed class FileUtils : Singleton<FileUtils>
    {
        private static Encoding _encoding = new UTF8Encoding();

        public bool ExistsFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            return File.Exists(path);
        }

        public bool CreateFile(string path)
        {
            if (ExistsFile(path))
            {
                return false;
            }
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            FileStream fs = File.Create(path);
            fs.Close();
            fs.Dispose();
            return true; ;
        }


        public bool DeleteFile(string path)
        {
            if (!ExistsFile(path))
            {
                return false;
            }
            File.Delete(path);
            return true; ;
        }

        public bool WriteFile(string content, string path)
        {
            CreateFile(path);
            Debug.LogError(path);
            StreamWriter sw = new StreamWriter(path, false, _encoding);
            //byte[] bytes = _encoding.GetBytes(content);
            sw.Write(content);
            sw.Close();
            sw.Dispose();
            return true;
        }
        public string ReadFile(string path)
        {
            if (!ExistsFile(path))
            {
                throw new Exception(path + "路径不存在！");
            }
            FileStream fs = File.OpenRead(path);
            BinaryReader binReader = new BinaryReader(fs);

            byte[] bytes = new byte[fs.Length];
            binReader.Read(bytes, 0, (int)fs.Length);
            string content = _encoding.GetString(bytes, 0, bytes.Length);
            binReader.Close();
            fs.Close();
            fs.Dispose();
            return content;
        }
        public CreativeSpore.RpgMapEditor.AutoTileMapSerializeData ReadMap(string path)
        {
            if (!ExistsFile(path))
            {
                throw new Exception(path + "路径不存在！");
            }
            return CreativeSpore.RpgMapEditor.AutoTileMapSerializeData.LoadFromFile(path);
            //FileStream fs = File.OpenRead(path);
            //BinaryReader binReader = new BinaryReader(fs);

            //byte[] bytes = new byte[fs.Length];
            //binReader.Read(bytes, 0, (int)fs.Length);
            //AssetBundle ass = AssetBundle.LoadFromMemory(bytes);
            //Debug.LogError(ass);
            //System.IO.MemoryStream _memory = new System.IO.MemoryStream(bytes);
            //_memory.Position = 0;
            //System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            //T t = formatter.Deserialize(_memory) as T;
            //_memory.Close();
            //binReader.Close();
            //fs.Close();
            //fs.Dispose();
            //return t;
        }
    }
}
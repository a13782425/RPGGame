using RPGGame.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;


public class PackEditor
{
    [MenuItem("Tools/Zip", priority = 3)]
    static void ZipPack()
    {
        string[] paths = Directory.GetFiles(Application.dataPath + "/Resources/DataAssets/Maps");
        string savePath = Application.dataPath + "/Resources/Data/Default/Maps/";
        for (int i = 0; i < paths.Length; i++)
        {
            if (Path.GetExtension(paths[i]) == ".meta")
            {
                continue;
            }
            CreativeSpore.RpgMapEditor.AutoTileMapData data = Resources.Load<CreativeSpore.RpgMapEditor.AutoTileMapData>("DataAssets/Maps/" + Path.GetFileNameWithoutExtension(paths[i]));
            data.Data.SaveToFile(savePath + Path.GetFileNameWithoutExtension(paths[i]));
        }
        string str = Application.dataPath + "/Resources/Data/Default";
        string str1 = Application.dataPath + "/Resources/Data/Global";
        ZipUtils.Zip(new string[] { str, str1 }, Application.dataPath + "/StreamingAssets/GlobalData");
        Debug.LogError("压缩完成");
    }
    [MenuItem("Tools/UnZip", priority = 3)]
    static void UnZipPack()
    {
        //string str = Application.dataPath + "/Resources/Data";
        ZipUtils.UnzipFile(Application.dataPath + "/StreamingAssets/GlobalData", Application.persistentDataPath);
        Debug.LogError("解压完成");
    }



}


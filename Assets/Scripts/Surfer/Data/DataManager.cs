using System;
using System.IO;
using Surfer.Data;
using UnityEngine;

namespace Surfer.Managers
{
    public class DataManager : Manager
    {
        private string dataPath;
        private string dataName;

        public DataManager(string path, string name)
        {
            dataName = path;
            dataName = name;
        }


        public virtual T LoadData<T>() where T : BaseData
        {
            string fullPath = Path.Combine(dataPath, dataName);
            T data = null;

            if (!File.Exists(fullPath))
                throw new Exception($"The file you are trying at {fullPath} to access does not exist! ");

            try
            {
                string loadedData = "";

                FileStream stream = new FileStream(fullPath, FileMode.Open);
                StreamReader reader = new StreamReader(stream);
                loadedData = reader.ReadToEnd();
                data = JsonUtility.FromJson<T>(loadedData);
                reader.Close();
            }
            catch (Exception e)
            {
                Debug.LogError($"The data you are trying to load does not exist or is of the wrong dataType: {e}");
            }

            return data;
        }

        public virtual void SaveData<T>(T data) where T : BaseData
        {
            string fullPath = Path.Combine(dataPath, dataName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                string savedData = JsonUtility.ToJson(data, true);

                FileStream stream = new FileStream(fullPath, FileMode.Create);
                StreamWriter writer = new StreamWriter(stream);

                writer.Write(savedData);
                writer.Close();
            }
            catch (Exception e)
            {
                Debug.LogError($"The data you were trying to save has failed: {e}");
            }
        }
    }
}
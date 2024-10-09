using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataSerializer : MonoBehaviour
{

    private const string defaultFileExtension = ".json";

    public static DataSerializer Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogWarning("Instance already exists, destroying this instance");
            Destroy(gameObject);
        }
    }


    public void SaveData<T>(T data, string subPath, string fileName, string defaultExtension = defaultFileExtension) {

        // get full path and create directory if it doesn't exist
        string fullPath = Path.Combine(Application.persistentDataPath, subPath, fileName + defaultExtension);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

        try {
            // serialize level data
            string levelDataJson = JsonUtility.ToJson(data, true);

            // write to file
            using (FileStream fs = new FileStream(fullPath, FileMode.Create)) {
                using (StreamWriter writer = new StreamWriter(fs)) {
                    writer.Write(levelDataJson);
                }
            }

        } catch (System.Exception e) {
            Debug.LogError("Error saving data to path: " + fullPath + ", error: " + e.Message);
        }
    }


    public LoadedData<T> LoadData<T>(string subPath, string fileName, string defaultExtension = defaultFileExtension) {

        // create full path on any os
        string fullPath = Path.Combine(Application.persistentDataPath, subPath, fileName + defaultExtension);

        if (File.Exists(fullPath)) {
            try {

                // deserialize data from the path
                string dataAsJson = "";
                using (FileStream fs = new FileStream(fullPath, FileMode.Open)) {
                    using (StreamReader reader = new StreamReader(fs)) {
                        dataAsJson = reader.ReadToEnd();
                    }
                }
                // return the deserialized data
                T data = JsonUtility.FromJson<T>(dataAsJson);
                return new LoadedData<T> { data = data, didExist = true , loadedSuccessfully = true};
                
                
            } catch (System.Exception e) {
                Debug.LogError("Error loading data from path: " + fullPath + ", error: " + e.Message);
                T data = default;
                return new LoadedData<T> { data = data, didExist = true , loadedSuccessfully = false};
            }
        } else {
            // the file did not exist
                Debug.LogWarning("File not found at path: " + fullPath + ", creating new file");
                T data = default;
                return new LoadedData<T> { data = data, didExist = false , loadedSuccessfully = false};
        }

    }

    public List<T> LoadDatasInDirectory<T>(string path, string requiredSuffix = "", string defaultExtension = defaultFileExtension, bool CreateDirectoryIfNotExists = true) {
        List<T> loadedDatas = new List<T>();

        DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(Application.persistentDataPath, path));
        FileInfo[] files; 
        try {
            files = directoryInfo.GetFiles("*" + requiredSuffix + defaultExtension);
        } catch {
            if (CreateDirectoryIfNotExists) {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, path));
                files = directoryInfo.GetFiles("*" + requiredSuffix + defaultExtension);
            } else {
                return loadedDatas;
            }
        }

        foreach (FileInfo file in files) {
            LoadedData<T> loadedData = LoadData<T>(path, file.Name.Replace(defaultExtension, ""));
            if (loadedData.loadedSuccessfully) loadedDatas.Add(loadedData.data);
        }

        return loadedDatas;
    }

    public FileInfo[] GetFilesInDirectory(string path, string requiredSuffix = "", string defaultExtension = defaultFileExtension, bool CreateDirectoryIfNotExists = true) {
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(Application.persistentDataPath, path));
        try {
            return directoryInfo.GetFiles("*" + requiredSuffix + defaultExtension);
        } catch {
            if (CreateDirectoryIfNotExists) {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, path));
                return directoryInfo.GetFiles("*" + requiredSuffix + defaultExtension);
            } else {
                return new FileInfo[0];
            }
        }
    }
}


public class LoadedData<T> {
    public T data;
    public bool didExist;
    public bool loadedSuccessfully;
}
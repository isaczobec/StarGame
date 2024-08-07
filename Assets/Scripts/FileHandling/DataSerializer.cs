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


    public void SaveData<T>(T data, string subPath, string fileName, string defaultExpression = defaultFileExtension) {

        // get full path and create directory if it doesn't exist
        string fullPath = Path.Combine(Application.persistentDataPath, subPath, fileName + defaultExpression);
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


    public LoadedData<T> LoadData<T>(string subPath, string fileName, string defaultExpression = defaultFileExtension) {

        // create full path on any os
        string fullPath = Path.Combine(Application.persistentDataPath, subPath, fileName + defaultExpression);

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

}


public class LoadedData<T> {
    public T data;
    public bool didExist;
    public bool loadedSuccessfully;
}
using UnityEngine;

[CreateAssetMenu(fileName = "New Spawnable Level Object", menuName = "Spawnable Level Object")]
public class SpawnnableLevelObjectSO : ScriptableObject
{
    public string SpawnnableObjectID = "";
    public GameObject prefab;
}
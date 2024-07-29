using UnityEngine;

public class ShockWaveHandler : MonoBehaviour {

    public static ShockWaveHandler instance {get; private set;}

    [Header("Shock Wave Prefabs")]
    [SerializeField] private GameObject shockWavePrefab;



    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    /// <summary>
    /// Spawns a shock wave at the given position with the given lifetime and max size.
    /// </summary>
    /// <param name="position">The position to spawn a ShockWave At.</param>
    /// <param name="lifeTime">How long the shockwaves lives.</param>
    /// <param name="maxSize"></param>
    /// <param name="parentTransform"></param>
    public void SpawnShockWave(Vector2 position, Transform parentTransform) {
        Debug.Log("SpawnShockWave");
        GameObject shockWave = Instantiate(shockWavePrefab, position, Quaternion.identity);
        shockWave.transform.SetParent(parentTransform, true);
        shockWave.GetComponent<ShockWave>().InitiateShockWave();
    }

}
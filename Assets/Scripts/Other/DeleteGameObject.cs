using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This script is used to delete a game object after a certain amount of time.
/// </summary>
public class DeleteGameObject : MonoBehaviour
{

    [SerializeField] private bool deleteAfterSpawn;
    [SerializeField] private float timeToDestroyAfterSpawn = 0;

    private Coroutine destroyCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        if (deleteAfterSpawn)
        {
            DeleteGameObjectAfterTime(timeToDestroyAfterSpawn);
        }
    }


    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        destroyCoroutine = null;
        Destroy(gameObject);
    }


    /// <summary>
    /// Deletes the game object after a certain amount of time.
    /// </summary>
    public void DeleteGameObjectAfterTime(float afterTime = 0) {
        if (afterTime > 0)
        {
            if (destroyCoroutine != null) {
            StopCoroutine(destroyCoroutine);
            }
            destroyCoroutine = StartCoroutine(DestroyAfterTime(afterTime));
        }
        else
        {
            Destroy(gameObject);
        }
    }


}

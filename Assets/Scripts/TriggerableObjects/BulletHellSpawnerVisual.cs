using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHellSpawnerVisual : MonoBehaviour
{
    [SerializeField] private GameObject distortionEffectPrefab;
    [SerializeField] private float distortionEffectScaleMargin = 0.4f;

    private void Start() {
        InitializeVisuals();
    }

    private void InitializeVisuals() {
        GameObject bulletHellSpawner = Instantiate(distortionEffectPrefab, transform.position, Quaternion.identity);
        bulletHellSpawner.transform.position = transform.position;
        bulletHellSpawner.transform.localScale = new Vector3(transform.localScale.x + distortionEffectScaleMargin, transform.localScale.y + distortionEffectScaleMargin, transform.localScale.z);
    }

}

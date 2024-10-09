using System;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class BulletHellSpawner : MonoBehaviour, ISpawnFromEditorObjectData {

    [Header("TRANSFORM SCALE DECIDES WHERE THE BULLETS CAN SPAWN!")]
    [SerializeField] private int startTriggerIndex;
    [SerializeField] private int endTriggerIndex;
    [SerializeField] private GameObject bulletHellPrefab;


    [Header("Bullet Settings")]
    [SerializeField] private int randomSeed;

    [Header("Bullet Settings")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float timeBetweenBullets;
    [SerializeField] private Vector2 bulletDirection;
    [SerializeField] private float bulletLifeTime;


    private bool isEmitting;
    private System.Random random;

    private float timeSinceLastBullet = 0;

    private List<BulletHellBullet> bullets = new List<BulletHellBullet>();

    private void Update() {
        if (isEmitting) {
            timeSinceLastBullet += Time.deltaTime;
            while (timeSinceLastBullet >= timeBetweenBullets) {
                timeSinceLastBullet -= timeBetweenBullets;
                FireBullet();
            }
        }
    }

    private void Start() {
        TriggerEffector.OnEffectorTriggeredByPlayerIndexed += OnEffectorTriggeredByPlayer;
        Player.Instance.OnPlayerDeath += OnPlayerDeath;

    }

    private void OnEffectorTriggeredByPlayer(object sender, int triggerIndex)
    {
        if (triggerIndex == startTriggerIndex) {
            StartEmittingBullets();
        } else if (triggerIndex == endTriggerIndex) {
            EndEmittingBullets();
        }
    }


    private void OnStartTriggered(object sender, TriggerHitEventArgs e)
    {
        if (e.isPlayer) {
            StartEmittingBullets();
        }
    }

    private void OnEndTriggered(object sender, TriggerHitEventArgs e)
    {
        if (e.isPlayer) {
            EndEmittingBullets();
        }
    }
    private void OnPlayerDeath(object sender, PlayerDeathEventArgs e)
    {
        EndEmittingBullets();
        DestroyAllBulletHellBullets();
    }


    private void StartEmittingBullets() {
        random = new System.Random(randomSeed); // reset random seed to make sure the bullets are the same every time
        isEmitting = true;
    }

    private void EndEmittingBullets() {
        isEmitting = false;
    }

    private void FireBullet() {

        Vector2 bottomLeft = new Vector2(transform.position.x - transform.localScale.x / 2, transform.position.y - transform.localScale.y / 2);
        Vector2 randomPos = new Vector2((float)random.NextDouble() * transform.localScale.x, (float)random.NextDouble() * transform.localScale.y);

        Vector2 spawnPos = bottomLeft + randomPos;

        GameObject bullet = Instantiate(bulletHellPrefab, spawnPos, Quaternion.identity);
        BulletHellBullet bulletHellBullet = bullet.GetComponent<BulletHellBullet>();
        bulletHellBullet.SetSettings(bulletSpeed, bulletDirection,bulletLifeTime);
        bullets.Add(bulletHellBullet);
    }

    public void CopyEditorObjectData(EditorObjectData editorObjectData)
    {
        editorObjectData.CopyTransformSettingsToGameObject(gameObject); // set transform
        startTriggerIndex = editorObjectData.GetSetting<int>("Start Trigger Index"); // set the start trigger index
        endTriggerIndex = editorObjectData.GetSetting<int>("End Trigger Index"); // set the start trigger index
        // copy bullet settings
        bulletDirection.x = editorObjectData.GetSetting<float>("Bullet Speed X"); // set the start trigger index
        bulletDirection.y = editorObjectData.GetSetting<float>("Bullet Speed Y"); // set the start trigger index
        timeBetweenBullets = MathF.Max(editorObjectData.GetSetting<float>("Fire Rate"),0.1f); // set the fire rate
    }

    private void DestroyAllBulletHellBullets() {
        foreach (BulletHellBullet bullet in bullets) {
            if (bullet != null) {
            Destroy(bullet.gameObject);
            }
        }
        bullets.Clear();
    }
}
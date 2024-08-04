using System;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class BulletHellSpawner : MonoBehaviour {

    [Header("TRANSFORM SCALE DECIDES WHERE THE BULLETS CAN SPAWN!")]
    [SerializeField] private TriggerEffector startTrigger;
    [SerializeField] private TriggerEffector endTrigger;
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
        if (startTrigger!= null) startTrigger.OnTriggered += OnStartTriggered;
        if (endTrigger!= null) endTrigger.OnTriggered += OnEndTriggered;
        Player.Instance.OnPlayerDeath += OnPlayerDeath;

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
    }
}
using UnityEngine;

public class BulletHellBullet : MonoBehaviour {

    private float speed;
    private Vector2 direction;

    private float lifeTime = 5f;

    
    public void SetSettings(float speed, Vector2 direction, float lifeTime) {
        this.speed = speed;
        this.direction = direction;
        this.lifeTime = lifeTime;
    }

    private void Update() {
        transform.Translate(direction * speed * Time.deltaTime);
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) {
            Destroy(gameObject);
        }
    }
 
}
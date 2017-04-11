using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControllerEnemy : MonoBehaviour {

    public float initialSpeed = 20;
    public float lifeSpan = 3;

    public void Fire (Vector2 direction)
    {
        gameObject.SetActive(true);
        AudioManager.PlayEffect("Powerup2", 1, 1);
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        body.velocity = direction * initialSpeed;
        StartCoroutine("LifecycleCoroutine");
    }

    IEnumerator LifecycleCoroutine()
    {
        yield return new WaitForSeconds(lifeSpan);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "shield")
        {
            gameObject.SetActive(false);
        }
    }
}

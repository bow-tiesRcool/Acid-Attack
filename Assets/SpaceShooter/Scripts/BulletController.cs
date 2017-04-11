using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float initialSpeed = 50;
    public float lifeSpan = 3;

    void Update()
    {
        OffScreenCheck();
    }

    public void Fire (Vector2 direction)
    {
        gameObject.SetActive(true);
        AudioManager.PlayEffect("Laser_Shoot7", 1, 1);
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
        if (collision.gameObject.tag == "AlienAttack")
        {
            gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "EyeAlien")
        {
            gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "Alien")
        {
            gameObject.SetActive(false);
        }
    }
    void OffScreenCheck()
    {
        Vector3 view = Camera.main.WorldToViewportPoint(transform.position);
        if (view.x > 1)
        {
            gameObject.SetActive(false);
        }
    }
}

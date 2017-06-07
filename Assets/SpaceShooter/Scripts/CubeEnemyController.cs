using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEnemyController : MonoBehaviour {

    public float speed = 10;
    public float ShootSpeed = 50;
    private Rigidbody2D body;
    public int points = 20;
    GameObject death;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        StartCoroutine("Attack");

    }

    void Update()
    {
        body.velocity = Vector2.left * speed;
        OffScreenCheck();
    }

    void OffScreenCheck()
    {
        Vector3 view = Camera.main.WorldToViewportPoint(transform.position);
        if (view.x < 0)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator Attack()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(Random.Range(1, 2));
            GameObject alien = Spawner.Spawn("EyeAlien");
            alien.gameObject.SetActive(true);
            alien.transform.position = transform.position + Vector3.down;
            alien.GetComponent<EnemyController>().Start();
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            death = Spawner.Spawn("EnemyDeath");
            death.transform.position = transform.position;
            death.GetComponent<DeathController>().Die();
            AudioManager.PlayEffect("Randomize33", 1, 1);
            gameObject.SetActive(false);
            GameManager.Points(points);
            SpawnPowerUp();

        }
        if (collision.gameObject.tag == "shield")
        {
            death = Spawner.Spawn("EnemyDeath");
            death.transform.position = transform.position;
            death.GetComponent<DeathController>().Die();
            AudioManager.PlayEffect("Randomize33", 1, 1);
            gameObject.SetActive(false);

        }
        if (collision.gameObject.tag == "Player")
        {
            death = Spawner.Spawn("EnemyDeath");
            death.transform.position = transform.position;
            death.GetComponent<DeathController>().Die();
            AudioManager.PlayEffect("Randomize33", 1, 1);
            gameObject.SetActive(false);

        }
    }

    void SpawnPowerUp()
    {
        if (Random.value < GameManager.instance.powerUpChance)
        {
            GameManager.instance.DropPowerUp(transform.position);
        }
    }
}

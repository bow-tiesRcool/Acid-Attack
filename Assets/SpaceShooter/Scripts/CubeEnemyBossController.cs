using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEnemyBossController : MonoBehaviour {

    public float speed = 5;
    public float ShootSpeed = 50;
    private Rigidbody2D body;
    public int points = 100;
    public int life = 50;
    public Vector3 view;
    GameObject death;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        StartCoroutine("EnemyMovement");
    }

    void Update()
    {
        view = Camera.main.WorldToViewportPoint(transform.position);
        OffScreenCheck();
    }

    void OffScreenCheck()
    {
        if (view.x < 0)
        {
            gameObject.SetActive(false);
            GameManager.instance.BossDefeated();
        }
    }

    IEnumerator EnemyMovement()
    {
        body.velocity = Vector2.left * speed;
        yield return new WaitForSeconds(Random.Range(1, 4));
        body.velocity = Vector2.up * speed;
        StartCoroutine("Attack");
        while (enabled)
        {

            if (view.y > .9f)
            {
                body.velocity = Vector2.down * speed;
            }
            if (view.y < 0.1f)
            {
                body.velocity = Vector2.up * speed;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Attack()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(Random.Range(1, 5));
            for (int i = 0; i <= 20; i++)
            {
                GameObject alien = Spawner.Spawn("EyeAlien");
                alien.gameObject.SetActive(true);
                alien.transform.position = transform.position + Vector3.down;
                yield return new WaitForSeconds(.25f);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            LifeOfBoss(1);
            AudioManager.PlayEffect("Randomize33", 1, 1);
        }
        if (collision.gameObject.tag == "shield")
        {
            AudioManager.PlayEffect("Randomize33", 1, 1);
        }
        if (collision.gameObject.tag == "Player")
        {
            AudioManager.PlayEffect("Randomize33", 1, 1);
        }
        if (collision.gameObject.tag == "HoldAttack")
        {
            LifeOfBoss(25);
            AudioManager.PlayEffect("Randomize33", 1, 1);
        }
    }

    void SpawnPowerUp()
    {
        if (Random.value < GameManager.instance.powerUpChance)
        {
            GameManager.instance.DropPowerUp(transform.position);
        }
    }

    void LifeOfBoss(int damage)
    {
        life -= damage;

        if (life <= 0)
        {
            death = Spawner.Spawn("EnemyDeathBoss");
            death.transform.position = transform.position;
            death.GetComponent<DeathController>().Die();
            GameManager.Points(points);
            AudioManager.PlayEffect("Randomize33", 1, 1);
            gameObject.SetActive(false);
            GameManager.instance.BossDefeated();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarEnemyBossController : MonoBehaviour {

    public float speed = 15;
    public float ShootSpeed = 30;
    private Rigidbody2D body;
    public Vector3 view;
    public int points = 100;
    public int life = 50;
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
        yield return new WaitForSeconds(Random.Range(1, 2));
        body.velocity = Vector2.left * 0;
        yield return new WaitForSeconds(Random.Range(20, 50));
        body.velocity = Vector2.left * ShootSpeed;
        AudioManager.PlayEffect("Randomize74", 1, 1);
        yield return new WaitForEndOfFrame();

    }

    IEnumerator Attack()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(Random.Range(20, 50));
            body.velocity = Vector2.left * ShootSpeed;
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


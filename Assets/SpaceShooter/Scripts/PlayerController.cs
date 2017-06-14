using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 20;
    public float ShootSpeed = 50;
    Renderer renderer;
    private Rigidbody2D body;
    public SpriteRenderer sprite;
    public Collider2D playerCollider;
    public int superBulletAmount = 0;
    public static PlayerController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        renderer = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        float y = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");
        body.velocity = new Vector3(x, y, 0) * speed;

        HoldShot();



        ClampToScreen(renderer.bounds.extents.x);
        ClampToScreen(-renderer.bounds.extents.x);
        ClampToScreen(renderer.bounds.extents.y);
        ClampToScreen(-renderer.bounds.extents.y);
    }

    void ClampToScreen(float xOffset)
    {
        Vector3 v = Camera.main.WorldToViewportPoint(transform.position + Vector3.right * xOffset);
        v.x = Mathf.Clamp01(v.x);
        transform.position = Camera.main.ViewportToWorldPoint(v) - Vector3.right * xOffset;

        Vector3 u = Camera.main.WorldToViewportPoint(transform.position + Vector3.down * xOffset);
        u.y = Mathf.Clamp01(u.y);
        transform.position = Camera.main.ViewportToWorldPoint(u) - Vector3.down * xOffset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ShieldController.instance.activeShield) return;

        if (collision.gameObject.tag == "AlienAttack")
        {
            gameObject.SetActive(false);
            AudioManager.PlayEffect("Explosion20", 1, 1);
            ExplosionSpawner.instance.SpawnExplosion(transform.position);
            GameManager.LifeLost();
        }
        if (collision.gameObject.tag == "EyeAlien")
        {
            gameObject.SetActive(false);
            AudioManager.PlayEffect("Explosion20", 1, 1);
            ExplosionSpawner.instance.SpawnExplosion(transform.position);
            GameManager.LifeLost();
        }
        if (collision.gameObject.tag == "Alien")
        {
            gameObject.SetActive(false);
            AudioManager.PlayEffect("Explosion20", 1, 1);
            ExplosionSpawner.instance.SpawnExplosion(transform.position);
            GameManager.LifeLost();
        }
        if (collision.gameObject.tag == "shieldPU")
        {
            ShieldController.ShieldActive(true);
        }
        if (collision.gameObject.tag == "Life")
        {
            GameManager.AddLife();
        }
        if (collision.gameObject.tag == "SuperBulletPower")
        {
            ++instance.superBulletAmount;
            GameManager.instance.superBullet.text = "Super Bullets: " + instance.superBulletAmount;
        }
    }

    float holdTime = 0;
    public void HoldShot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            holdTime = Time.time;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            GameObject bullet = null;
            if (Time.time - holdTime > 3f && instance.superBulletAmount > 0)
            {
                bullet = Spawner.Spawn("HoldAttack");
                AudioManager.PlayEffect("Randomize45", 1, 1);
                --instance.superBulletAmount;
                GameManager.instance.superBullet.text = "Super Bullets: " + instance.superBulletAmount;
            }
            else
            {
                bullet = Spawner.Spawn("Bullet");
            }
            bullet.transform.position = transform.position;
            bullet.GetComponent<BulletController>().Fire(Vector2.right);
        }
    }
}
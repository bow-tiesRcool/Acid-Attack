using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public Text gameOverUI;
    public Text scoreUI;
    public Text highScoreUI;
    public Text livesUI;
    public Text levelComplete;
    public Text superBullet;
    public string Player = "Player";
    public string[] PowerUpPrefabNames;
    public int score = 0;
    public int highScore;
    public int lives = 3;
    public int enemyAmount = 100;
    [Range(0, 1)] public float powerUpChance = 0.1f;
    public float timer = 10;
    public string scene;

    private MenuControllerLevel menuController;
    private MenuControllerLevel menuScreen;
    private Spawner spawnerController;
    public bool bossSpawned = false;

    private void Awake()
    {
        spawnerController = GetComponent<Spawner>();
        menuController = GetComponentInChildren<MenuControllerLevel>();
        menuController.gameObject.SetActive(false);
        menuScreen = GetComponentInChildren<MenuControllerLevel>();
        menuScreen.gameObject.SetActive(false);
        scene = SceneManager.GetActiveScene().name;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            instance.spawnerController.prefabs = this.spawnerController.prefabs;
            instance.spawnerController.boss = this.spawnerController.boss;
            instance.spawnerController.enemyPrefabNames = this.spawnerController.enemyPrefabNames;
            instance.spawnerController.enemyBossNames = this.spawnerController.enemyBossNames;
            instance.spawnerController.spawn = true;
            instance.spawnerController.Reset();

            instance.menuController.level = this.menuController.level;
            instance.menuScreen.level = this.menuScreen.level;
            instance.levelComplete.gameObject.SetActive(false);
            instance.gameOverUI.gameObject.SetActive(false);
            ExplosionSpawner.instance.Reset();
            ExplosionSpawner.instance.SpawnExplosion(new Vector3(-30,0,0));
            instance.spawnerController.StartCoroutine("SpawnEnemiesCoroutine");
            instance.scene = this.scene;
            Destroy(gameObject);
        }
    } 

    void Start ()
    {
        Cursor.visible = false;

        Spawner.spawner.StartCoroutine("SpawnEnemiesCoroutine");
        livesUI.text = "Lives: " + lives;
        scoreUI.text = "Score: " + score;
        instance.highScoreUI.text = "HighScore: " + PlayerPrefs.GetInt("highScore");
        PlayerController.instance.transform.position = new Vector2(-20, 0);
        PlayerController.instance.gameObject.SetActive(true);
        scene = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        HighScore();
    }

    public static void GameOver()
    {
        Debug.Log("you lost");
        instance.gameOverUI.text = "Game Over";
        instance.gameOverUI.gameObject.SetActive(true);
        HighScoreSaver();
    }

    public static void Points(int points)
    {
        int score = points;
        instance.score += score;
        instance.scoreUI.text = "Score: " + instance.score;
    }

    public static void HighScore()
    {
        if (instance.score > instance.highScore)
        {
            instance.highScore = instance.score;
        }
        if (instance.highScore > PlayerPrefs.GetInt("highScore"))
        {
            instance.highScoreUI.text = "highScore: " + instance.highScore;
        }
    }
    public static void HighScoreSaver()
    {
        if (PlayerPrefs.HasKey("highScore") == true)
        {
            if (instance.highScore > PlayerPrefs.GetInt("highScore"))
            {
                int newHighScore = instance.highScore;
                PlayerPrefs.SetInt("highScore", newHighScore);
                PlayerPrefs.Save();
            }
        }
        else
        {
            int newHighScore = instance.highScore;
            PlayerPrefs.SetInt("highScore", newHighScore);
            PlayerPrefs.Save();
        }
    }

    public static void LifeLost()
    {
        instance.lives = instance.lives - 1;
        instance.livesUI.text = "Lives: " + instance.lives;

        if (instance.lives < 0)
        {
            GameOver();
        }
        else
        {
            instance.livesUI.text = "Lives: " + instance.lives;
            instance.StartCoroutine("SpawnTimer");

        }
    }

    public void DropPowerUp(Vector3 pos)
    {
        Debug.Log("Spawning PowerUp");
        string PowerUpPrefabName = PowerUpPrefabNames[Random.Range(0, PowerUpPrefabNames.Length)];

        GameObject power = Spawner.Spawn(PowerUpPrefabName);
        AudioManager.PlayEffect("Powerup11", 1, 1);

        power.transform.position = pos;
        power.SetActive(true);
    }

    public static void AddLife()
    {
        if (instance.lives < 5)
        {
            instance.lives = instance.lives + 1;
            instance.livesUI.text = "Lives: " + instance.lives;
        }
        else
        {
            Debug.Log("Max Lives");
        }
    }

    public static void EnemyTilBoss()
    {
        --instance.enemyAmount;

        if (instance.enemyAmount <= 0 && !instance.bossSpawned)
        {
            Spawner.spawner.spawn = false;
            instance.StartCoroutine("BossSpawn");

        }
    }

    IEnumerator BossSpawn()
    {
        instance.bossSpawned = true;
        yield return new WaitForSeconds(timer);
        if (instance.scene == "Level6")
        {
            Spawner.spawner.SpawnRandomBoss();
            AudioManager.PlayEffect("Randomize42", 1, 1);
        }
        else
        {
            GameObject bossEnemy = Spawner.BossSpawn();
            AudioManager.PlayEffect("Randomize42", 1, 1);
            bossEnemy.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1f, .5f, -Camera.main.transform.position.z));
            bossEnemy.SetActive(true);
        }
    }

    public void BossDefeated()
    {
        if (instance.scene == "Level6")
        {
            instance.spawnerController.Reset();
            instance.bossSpawned = false;
            instance.enemyAmount = 75;
            Spawner.spawner.spawn = true;
            Spawner.spawner.StartCoroutine("SpawnEnemiesCoroutine");
        }
        else
        {
            levelComplete.text = "Level complete!";
            levelComplete.gameObject.SetActive(true);
            instance.bossSpawned = false;
            instance.enemyAmount = 75;
            Spawner.spawner.spawn = true;
        }
    }

    IEnumerator SpawnTimer()
    {
        PlayerController.instance.transform.position = new Vector2(-20, 0);
        PlayerController.instance.gameObject.SetActive(true);
        PlayerController.instance.playerCollider.enabled = false;
        yield return StartCoroutine(Flash(.5f, .1f));
        PlayerController.instance.playerCollider.enabled = true;
        yield return null;

    }

    public IEnumerator Flash(float time, float interval)
    {
        for(float i = 0; i < time; i += Time.deltaTime)
        {
            Debug.Log(i);
            PlayerController.instance.sprite.enabled = true;
            yield return new WaitForSeconds(interval);
            PlayerController.instance.sprite.enabled = false;
            yield return new WaitForSeconds(interval);
        }
        yield return PlayerController.instance.sprite.enabled = true;
    }
}
    
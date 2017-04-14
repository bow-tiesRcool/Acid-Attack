using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public Text gameOverUI;
    public Text scoreUI;
    public Text highScoreUI;
    public Text livesUI;
    public string Player = "Player";
    public string[] enemyPrefabNames;
    public string[] PowerUpPrefabNames;
    public int score = 0;
    public int highScore;
    public int lives = 3;
    [Range(0, 1)] public float powerUpChance = 0.1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    } 

    void Start ()
    {
        Cursor.visible = false;
        StartCoroutine("SpawnEnemiesCoroutine");
        livesUI.text = "Lives: " + lives;
        scoreUI.text = "Score: " + score;
        instance.highScoreUI.text = "HighScore: " + PlayerPrefs.GetInt("highScore");
        GameObject player = Spawner.Spawn(Player);
        player.transform.position = new Vector2(-20, 0);
        player.SetActive(true);
    }

    private void Update()
    {
        HighScore();
    }

    IEnumerator SpawnEnemiesCoroutine()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(1);

            string enemyPrefabName = enemyPrefabNames[Random.Range(0, enemyPrefabNames.Length)];

            GameObject enemy = Spawner.Spawn(enemyPrefabName);
            Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, Random.Range(.1f,.9f), -Camera.main.transform.position.z));

            enemy.transform.position = pos;
            enemy.SetActive(true);
        }
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
            GameObject player = Spawner.Spawn(instance.Player);
            player.transform.position = new Vector2(-20, 0);
            player.SetActive(true);

        }
    }

    public void DropPowerUp(Vector3 pos)
    {
        Debug.Log("Spawning PowerUp");
        string PowerUpPrefabName = PowerUpPrefabNames[Random.Range(0, PowerUpPrefabNames.Length)];

        GameObject power = Spawner.Spawn(PowerUpPrefabName);
        
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
}
    
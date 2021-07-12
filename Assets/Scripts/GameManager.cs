using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float respawnTime = 3;
    [SerializeField] float invulnerabilityTime = 3;
    [SerializeField] ParticleSystem explosionFX;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject startTitleUI;
    [SerializeField] Text scoreText;
    [SerializeField] Text livesText;

    public int lives = 3;
    public int score;

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            startTitleUI.SetActive(false);
            NewGame();
        }
    }
    private void NewGame()
    {
        Asteroid[] asteroids = FindObjectsOfType<Asteroid>();
        for (int i = 0; i < asteroids.Length; i++)
        {
            Destroy(asteroids[i].gameObject);
            //asteroids[i].gameObject.SetActive(false);
        }

        gameOverUI.SetActive(false);
        SetLives(3);
        SetScore(0);
        PlayerRespawn();
    }

    public void PlayerDead()
    {
        explosionFX.transform.position = player.transform.position;
        explosionFX.Play();

        lives--;
        livesText.text = lives.ToString();
        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            Invoke(nameof(PlayerRespawn), respawnTime);
        }
    }

    public void OnKillEnemy(Enemy enemy)
    {
        explosionFX.transform.position = enemy.transform.position;
        explosionFX.Play();
        score += 150;
        SetScore(score);
        Debug.Log(score);

        //todo:Crear EnemySpawner
    }

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        explosionFX.transform.position = asteroid.transform.position;
        explosionFX.Play();
        ScoreRules(asteroid);
    }

    private void ScoreRules(Asteroid asteroid)
    {
        if (asteroid.size < 0.75)
        {
            score += 100;
        }
        else if (asteroid.size < 1.2)
        {
            score += 50;
        }
        else
        {
            score += 25;
        }
        scoreText.text = score.ToString();
    }

    private void GameOver()
    {
        print("GAME OVER");
        lives = 3;
        score = 0;
        Invoke(nameof(PlayerRespawn), respawnTime);
        gameOverUI.SetActive(true);
    }

    public void PlayerRespawn()
    {
        player.transform.position = Vector3.zero;
        player.gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");
        player.gameObject.SetActive(true);
         
        Invoke(nameof(TurnOnCollisions), invulnerabilityTime);
    }

    private void TurnOnCollisions()
    {
        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = lives.ToString();
    }

}

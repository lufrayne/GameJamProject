using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public int playerScore;
    public Text scoreText;
    public GameObject gameOverScreen;
    private bool playerIsAlive = true;

    [ContextMenu("Increase Score")]

    private void Start()
    {
    }
    private void Update()
    {
        GameObject runnerInstance = GameObject.Find("Runner");
        RunnerScript runnerScriptInstance = runnerInstance.GetComponent<RunnerScript>();

        GameObject ChaserSpawnerInstance = GameObject.FindWithTag("ChaserSpawner");
        ChaserSpawnScript chaserSpawnScriptInstance = ChaserSpawnerInstance.GetComponent<ChaserSpawnScript>();

        GameObject CardSpawnerInstance = GameObject.FindWithTag("CardSpawner");
        CardSpawnerScript cardSpawnerScriptInstance = CardSpawnerInstance.GetComponent<CardSpawnerScript>();

        if (playerIsAlive == false)
        {
            runnerScriptInstance.enabled = false;
            chaserSpawnScriptInstance.enabled = false;
            cardSpawnerScriptInstance.enabled = false; 
        }
    }

    public void addScore()
    {
        playerScore = playerScore + 1;
        scoreText.text = playerScore.ToString();
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameOver()
    {
        Debug.Log("Game ending...");
        gameOverScreen.SetActive(true);
        playerIsAlive = false;
    }
}

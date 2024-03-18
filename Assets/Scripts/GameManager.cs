using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float killCount;
    public TextMeshProUGUI killCountText;
    public GameObject pauseMenuUI;
    public GameObject winLevelUI;
    private bool gameIsPaused;
    private bool gameWon;
    public int enemyNumber;
    public int killedEnemyNumber;
    List<GameObject> listOfOpponents = new List<GameObject>();

    void Start()
    {
        Time.timeScale = 1;
        listOfOpponents.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        enemyNumber = listOfOpponents.Count;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameWon)
        {
            if (gameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        if(enemyNumber == 0)
        {
            winLevelUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            gameIsPaused = true;
            gameWon = true;
        }

    }
    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        gameIsPaused = true;
    }
    void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        gameIsPaused = false;
    }
    public void AppQuit()
    {
        Application.Quit();
    }
    public void RestartGame()
    {
        gameIsPaused = false;
        gameWon = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void NextLevel()
    {
        gameIsPaused = false;
        gameWon = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void AddKill()
    {
        killCount++;
        killCountText.text = "Kill Count: " + killCount;
    }
}

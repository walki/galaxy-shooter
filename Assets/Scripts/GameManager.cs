using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public  bool _isGameOver;


    [SerializeField]
    public  bool isCoopMode = false;


    [SerializeField]
    public bool isPaused;

    [SerializeField]
    private GameObject _pauseMenu;

    private Animator _pauseAnimator;

    public void GameOver()
    {
        _isGameOver = true;
    }


    private void Start()
    {
        Debug.Log("Coop Mode: " + isCoopMode);
        _pauseAnimator = _pauseMenu.GetComponent<Animator>();
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            StartCoroutine(RestartGame());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }

    }

    public void PauseGame()
    {
        _pauseMenu.SetActive(true);
        _pauseAnimator.SetBool("isPaused", true);

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void UnpauseGame()
    {
        _pauseMenu.SetActive(false);
        _pauseAnimator.SetBool("isPaused", false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void BackToMainMenu()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
        }
        SceneManager.LoadScene("Main_Menu2");
        
    }

    private IEnumerator RestartGame()
    {
        AsyncOperation asyncload = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

        while (!asyncload.isDone)
        {
            yield return null;
        }
    }
}

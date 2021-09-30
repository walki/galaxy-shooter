using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private int _currentScore = 0;

    [SerializeField]
    private int _bestScore = 0;


    [SerializeField]
    private Text _scoreText;
    private string scorePrefix = "Score: ";

    [SerializeField]
    private Text _bestScoreText;
    private string bestScorePrefix = "Best: ";

    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Image _livesDisplay;

    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;



    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = scorePrefix + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);

        _livesDisplay.sprite = _livesSprites[3];

        if (_bestScoreText != null)
        {
            _bestScoreText.text = bestScorePrefix + _bestScore;
        }

    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("bestScore", _bestScore);
    }

    private void OnEnable()
    {
        _bestScore = PlayerPrefs.GetInt("bestScore", 0);
    }

    public void UpdateScore()
    {
        _currentScore += 10;
        _scoreText.text = scorePrefix + _currentScore;

        CheckForBestScore();
    }

    public void CheckForBestScore()
    {
        if (_bestScoreText != null & _currentScore > _bestScore)
        {
            _bestScore = _currentScore;
            _bestScoreText.text = bestScorePrefix + _bestScore;
        }
    }

    public void SetLives(int lives)
    {

        if (lives >= 0 && lives < 3)
        {
            _livesDisplay.sprite = _livesSprites[lives];
        }

        if (lives <= 0)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        GameManager gameManager = GameObject.Find("Game_Manager")?.GetComponent<GameManager>();
        gameManager.GameOver();

        _restartText.gameObject.SetActive(true);
        
        StartCoroutine(GameOverFlicker());
        
    }

    private IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);

            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);

            if (_gameOverText.fontSize < 90)
            {
                _gameOverText.fontSize += 5;
            }
            else
            {
                _gameOverText.fontSize = 10;
            }
        }
    }


}

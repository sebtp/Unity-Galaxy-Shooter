using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (!_gameManager)
        {
            Debug.LogError("The Game Manager is NULL");
        }

        _scoreText.text = "Score: 0";
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
    }

    private void Update()
    {
        
    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];
    }

    public void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverTextBlink());
    }

    IEnumerator GameOverTextBlink()
    {
        for (int i = 1; i < 3; i++)
        {
            yield return new WaitForSeconds(0.2f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            _gameOverText.gameObject.SetActive(true);
        }
    }

}

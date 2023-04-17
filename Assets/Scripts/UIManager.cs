using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _lifeSprites;
    [SerializeField]
    internal TextMeshProUGUI _gameoverText;
    [SerializeField]
    private TextMeshProUGUI _restartText;


    // Start is called before the first frame update
    void Start()
    {
        _livesImg = GetComponentInChildren<Image>();
        _gameoverText = GameObject.Find("GameOverText").GetComponent<TextMeshProUGUI>();
        //_gameoverText.gameObject.SetActive(false);
        _restartText = GameObject.Find("RestartText").GetComponent<TextMeshProUGUI>();
        _restartText.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ViewScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int currentLives)
    {
        //display img
        _livesImg.sprite = _lifeSprites[currentLives];
        if (currentLives == 0)
        {
            DisplayText("GameOver");
        }
    }

    public virtual void DisplayText(string text)
    {
        _gameoverText.text = text;
        _restartText.gameObject.SetActive(true);
        StartCoroutine("GameOverFlickerRoutine");

    }
    IEnumerator GameOverFlickerRoutine()
    {
        _gameoverText.enabled = true;

        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            _gameoverText.gameObject.active = false;
            yield return new WaitForSeconds(0.5f);
            _gameoverText.gameObject.active = true;
        }
    }
}
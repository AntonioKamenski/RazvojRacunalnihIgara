using UnityEngine;
using TMPro;
using System;

public class GameLogic : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI guessText;
    int min, max, guess; 

    void Start()
    {
        min = 1;
        max = 1000;
        GuessNext();
    }

    private void GuessNext()
    {
        guess = UnityEngine.Random.Range(min, max);
        guessText.text = guess.ToString();
    }

    public void OnPressHigher()
    {
        min = guess;
        GuessNext();
    }

    public void OnPressLower()
    {
        max = guess;
        GuessNext();
    }
}

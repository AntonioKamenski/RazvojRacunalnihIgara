using UnityEngine;
using TMPro;
using System;

public class GameLogic : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] TextMeshProUGUI questionNumberText;
    [SerializeField] TextMeshProUGUI OptionA;
    [SerializeField] TextMeshProUGUI OptionB;
    [SerializeField] TextMeshProUGUI OptionC;
    [SerializeField] TextMeshProUGUI OptionD;
    int questionNumber = 1;
    Question[] questions;
    string guess;

    public void Start()
    {
        questionNumberText.text = questionNumber.ToString();
        questions = Question.GetQuestions();
        if (questions.Length == 0)        {
            Debug.LogError("No questions to be loaded.");
            return;
        }
        Question currentQuestion = questions[questionNumber - 1];
        questionText.text = currentQuestion.question;
        OptionA.text = currentQuestion.optionA;
        OptionB.text = currentQuestion.optionB;
        OptionC.text = currentQuestion.optionC;
        OptionD.text = currentQuestion.optionD;
    }

    private void NextQuestion()
    {
        questionNumber++;
        questionNumberText.text = questionNumber.ToString();
        if (questionNumber > questions.Length)
        {
            SceneLoader.LoadSceneByName("Win Scene");
            return;
        }
        Question currentQuestion = questions[questionNumber - 1];
        questionText.text = currentQuestion.question;
        OptionA.text = currentQuestion.optionA;
        OptionB.text = currentQuestion.optionB;
        OptionC.text = currentQuestion.optionC;
        OptionD.text = currentQuestion.optionD;
    }

    public void onPressA()
    {
        guess = "A";
        if (questions[questionNumber - 1].CheckAnswer(guess))
        {
            NextQuestion();
        }
        else
        {
            SceneLoader.LoadSceneByName("Lose Scene");
        }
    }

    public void onPressB()
    {
        guess = "B";
        if (questions[questionNumber - 1].CheckAnswer(guess))
        {
            NextQuestion();
        }
        else
        {
            SceneLoader.LoadSceneByName("Lose Scene");
        }
    }

    public void onPressC()
    {
        guess = "C";
        if (questions[questionNumber - 1].CheckAnswer(guess))
        {
            NextQuestion();
        }
        else
        {
            SceneLoader.LoadSceneByName("Lose Scene");
        }
    }

    public void onPressD()
    {
        guess = "D";
        if (questions[questionNumber - 1].CheckAnswer(guess))
        {
            NextQuestion();
        }
        else
        {
            SceneLoader.LoadSceneByName("Lose Scene");
        }
    }
}

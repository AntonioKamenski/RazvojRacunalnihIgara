using System;
using System.IO;
using System.Collections.Generic;

public class Question
{
    public string question;
    public string correctOption;
    public string optionA;
    public string optionB;
    public string optionC;
    public string optionD;

    public Question(string question, string correctOption, string optionA, string optionB, string optionC, string optionD)
    {
        this.question = question;
        this.correctOption = correctOption;
        this.optionA = optionA;
        this.optionB = optionB;
        this.optionC = optionC;
        this.optionD = optionD;
    }

    public bool CheckAnswer(string selected)
    {
        return selected == correctOption;
    }

    public static Question[] GetQuestions()
    {
        var questions = new System.Collections.Generic.List<Question>();

        questions.Add(new Question("What is the capital of France?",             "B", "Berlin",        "Paris",       "Madrid", "Rome"));
        questions.Add(new Question("Which planet is known as the Red Planet?",   "C", "Venus",         "Jupiter",     "Mars",   "Saturn"));
        questions.Add(new Question("What is the largest mammal in the world?",   "A", "Blue Whale",    "Elephant",    "Giraffe","Hippopotamus"));
        questions.Add(new Question("In which year did World War II end?",        "D", "1943",          "1944",        "1946",   "1945"));
        questions.Add(new Question("Which element has the chemical symbol 'O'?", "B", "Gold",          "Oxygen",      "Iron",   "Silver"));
        questions.Add(new Question("What is the main language spoken in Brazil?","C", "Spanish",       "English",     "Portuguese","French"));
        questions.Add(new Question("Which planet is closest to the Sun?",        "A", "Mercury",       "Venus",       "Earth",  "Mars"));
        questions.Add(new Question("What is the tallest mountain on Earth?",     "D", "K2",            "Kangchenjunga","Lhotse","Mount Everest"));
        questions.Add(new Question("Which of these is NOT a primary color of light?","C", "Red",       "Green",       "Purple","Blue"));
        questions.Add(new Question("Who wrote the play 'Hamlet'?",              "B", "George Orwell", "William Shakespeare","Charles Dickens","Jane Austen"));
        questions.Add(new Question("What is the capital of Japan?",              "B", "Seoul",         "Tokyo",       "Beijing","Bangkok"));
        questions.Add(new Question("Which gas do plants absorb from the air?",   "A", "Carbon dioxide","Oxygen",      "Nitrogen","Hydrogen"));
        questions.Add(new Question("Which ocean is the largest?",                "C", "Atlantic",      "Indian",      "Pacific","Arctic"));
        questions.Add(new Question("Which is the smallest planet in the Solar System?","C", "Venus",    "Earth",       "Mercury","Mars"));
        questions.Add(new Question("What is the currency of Germany?",           "A", "Euro",          "Pound",       "Dollar","Yen"));
        questions.Add(new Question("Which sport uses the term 'birdie'?",        "D", "Baseball",      "Football",    "Basketball","Golf"));
        questions.Add(new Question("Which element is represented by the symbol 'H'?", "B", "Helium",   "Hydrogen",    "Hafnium","Hassium"));
        questions.Add(new Question("Which country is known as the Land of the Rising Sun?","C", "China","Thailand",    "Japan","South Korea"));
        questions.Add(new Question("Which planet has the most moons in the Solar System?", "D", "Earth", "Mars",    "Venus","Jupiter"));
        questions.Add(new Question("Which organ produces insulin in the human body?", "C", "Liver",      "Heart",       "Pancreas","Kidney"));

        return questions.ToArray();
    }
}
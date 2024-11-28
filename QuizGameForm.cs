using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class QuizGameForm : Form
{
    private int score = 0;
    private List<Question> questions;
    private Label scoreLabel;
    private Button nextButton;
    private Label questionLabel;
    private RadioButton[] answerButtons;
    private Label resultLabel;
    private Button showResultButton;

    public QuizGameForm()
    {
        // Initialize questions
        questions = new List<Question>
        {
            new Question("What is the capital of France?", new string[] { "Berlin", "Madrid", "Paris", "Lisbon" }, 2),
            new Question("Which planet is known as the Red Planet?", new string[] { "Earth", "Mars", "Jupiter", "Venus" }, 1),
            new Question("What is the largest mammal?", new string[] { "Elephant", "Whale", "Giraffe", "Rhino" }, 1),
            new Question("Which language is primarily used for web development?", new string[] { "Python", "JavaScript", "C#", "Ruby" }, 1),
            new Question("What is the square root of 64?", new string[] { "6", "7", "8", "9" }, 2)
        };

        // Initialize UI components
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Text = "Interactive Quiz Game";
        this.Size = new Size(500, 400);

        scoreLabel = new Label
        {
            Location = new Point(10, 10),
            Size = new Size(300, 30),
            ForeColor = Color.Yellow,
            Text = "Score: 0",
            Font = new Font("Arial", 14)
        };

        questionLabel = new Label
        {
            Location = new Point(10, 50),
            Size = new Size(300, 40),
            ForeColor = Color.White,
            Font = new Font("Arial", 12)
        };

        answerButtons = new RadioButton[4];
        for (int i = 0; i < 4; i++)
        {
            answerButtons[i] = new RadioButton
            {
                Location = new Point(10, 100 + (i * 30)),
                ForeColor = Color.White,
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(answerButtons[i]);
        }

        nextButton = new Button
        {
            Text = "Next",
            Location = new Point(10, 250),
            Size = new Size(100, 40)
        };
        nextButton.Click += NextButton_Click;

        resultLabel = new Label
        {
            Location = new Point(10, 300),
            Size = new Size(400, 30),
            ForeColor = Color.White
        };

        showResultButton = new Button
        {
            Text = "Show Results",
            Location = new Point(120, 250),
            Size = new Size(100, 40)
        };
        showResultButton.Click += ShowResultButton_Click;

        this.Controls.Add(scoreLabel);
        this.Controls.Add(questionLabel);
        this.Controls.Add(nextButton);
        this.Controls.Add(resultLabel);
        this.Controls.Add(showResultButton);

        DisplayNextQuestion();
    }

    private void DisplayNextQuestion()
    {
        if (questions.Count == 0)
        {
            resultLabel.Text = "Quiz Finished!";
            return;
        }

        var question = questions[0];
        questionLabel.Text = question.QuestionText;

        for (int i = 0; i < 4; i++)
        {
            answerButtons[i].Text = question.Options[i];
            answerButtons[i].Checked = false;
        }
    }

    private void NextButton_Click(object sender, EventArgs e)
    {
        var question = questions[0];
        for (int i = 0; i < 4; i++)
        {
            if (answerButtons[i].Checked && i == question.CorrectAnswerIndex)
            {
                score++;
                break;
            }
        }

        questions.RemoveAt(0);
        scoreLabel.Text = $"Score: {score}";

        DisplayNextQuestion();
    }

    private void ShowResultButton_Click(object sender, EventArgs e)
    {
        var resultForm = new ResultForm(score, questions.Count);
        resultForm.Show();
    }

    // Main entry point
    public static void Main()
    {
        Application.Run(new QuizGameForm());
    }
}

public class Question
{
    public string QuestionText { get; set; }
    public string[] Options { get; set; }
    public int CorrectAnswerIndex { get; set; }

    public Question(string questionText, string[] options, int correctAnswerIndex)
    {
        QuestionText = questionText;
        Options = options;
        CorrectAnswerIndex = correctAnswerIndex;
    }
}

public class ResultForm : Form
{
    private int score;
    private int totalQuestions;

    public ResultForm(int score, int totalQuestions)
    {
        this.score = score;
        this.totalQuestions = totalQuestions;
        this.Text = "Quiz Results";
        this.Size = new Size(400, 400);
        this.Paint += ResultForm_Paint;
    }

    private void ResultForm_Paint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        Rectangle rect = new Rectangle(50, 50, 300, 300);

        // Create a pie chart representation
        float correctPercentage = (float)score / totalQuestions * 100;
        float incorrectPercentage = 100 - correctPercentage;

        // Set colors for the chart
        Brush correctBrush = new SolidBrush(Color.Green);
        Brush incorrectBrush = new SolidBrush(Color.Red);

        // Draw the pie chart segments
        g.FillPie(correctBrush, rect, 0, correctPercentage * 3.6f);
        g.FillPie(incorrectBrush, rect, correctPercentage * 3.6f, incorrectPercentage * 3.6f);

        // Display the percentages
        g.DrawString($"Correct: {correctPercentage}%", new Font("Arial", 12), Brushes.Black, new PointF(150, 200));
        g.DrawString($"Incorrect: {incorrectPercentage}%", new Font("Arial", 12), Brushes.Black, new PointF(150, 220));
    }
}

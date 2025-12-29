using Godot;
using System;

public partial class Main : Node2D
{
    private int p1Score = 0;
    private int p2Score = 0;
    private int countdownValue = 4;
    private Label countdownLabel;
    [Signal]
    public delegate void StartGameEventHandler();
    [Signal]
    public delegate void QuitGameEventHandler();
    [Signal]
    public delegate void BackgroundMusicEventHandler();

    public override void _Ready()
    {
        countdownLabel = GetNode<Label>("CountdownLabel");
        countdownLabel.Visible = false;
        GetNode<Label>("P1WinLabel").Visible = false;
        GetNode<Label>("P2WinLabel").Visible = false;

        var ball = GetNode<BallMovement>("Ball");
        ball.Player1Scored += OnPlayer1Scored;
        ball.Player2Scored += OnPlayer2Scored;

        GetNode<AudioStreamPlayer2D>("Background music").Play();
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionPressed("close_game"))
            GetTree().Quit();
    }

    private void OnPlayer1Scored()
    {
        p1Score++;
        GetNode<Label>("P1Score").Text = p1Score.ToString();

        if (p1Score == 3)
        {
            GetNode<Label>("P1WinLabel").Text = "Player 1 Wins!";
            GetNode<Label>("P1WinLabel").Visible = true;
            GetNode<Button>("StartButton").Visible = true;
            GetNode<Button>("QuitButton").Visible = true;

            GetNode<BallMovement>("Ball").Stop();
            GetNode<Timer>("StartTimer").Stop();
        }
        else
        {
            NewGame();
        }
    }

    private void OnPlayer2Scored()
    {
        p2Score++;
        GetNode<Label>("P2Score").Text = p2Score.ToString();

        if (p2Score == 3)
        {
            GetNode<Label>("P2WinLabel").Text = "Player 2 Wins!";
            GetNode<Label>("P2WinLabel").Visible = true;
            GetNode<Button>("StartButton").Visible = true;
            GetNode<Button>("QuitButton").Visible = true;

            GetNode<BallMovement>("Ball").Stop();
            GetNode<Timer>("StartTimer").Stop();
        }
        else
        {
            NewGame();
        }
    }

    public void NewGame()
    {
        var p1 = GetNode<Node2D>("Player one");
        var p2 = GetNode<Node2D>("Player two");
        var ball = GetNode<Node2D>("Ball");
        var ballMovement = GetNode<BallMovement>("Ball");

        var p1StartPosition = GetNode<Marker2D>("P1 Start");
        var p2StartPosition = GetNode<Marker2D>("P2 Start");
        var ballStartPosition = GetNode<Marker2D>("Ball Start");

        ballMovement.Stop();
        p1.Position = p1StartPosition.Position;
        p2.Position = p2StartPosition.Position;
        ball.Position = ballStartPosition.Position;

        GetNode<Timer>("StartTimer").Start();
    }

    public void OnStartTimerTimeout()
    {
        GetNode<Timer>("StartTimer").Start();
        countdownValue--;

        if (countdownValue > 0)
        {
            countdownLabel.Text = countdownValue.ToString();
            countdownLabel.Visible = true;
            GetNode<AudioStreamPlayer2D>("Countdown").Play();
        }
        else
        {
            countdownLabel.Visible = false;

            GetNode<BallMovement>("Ball").StartMoving();
            GetNode<Timer>("StartTimer").Stop();
            countdownValue = 4; // Reset for next time
        }
    }

    private void OnStartButtonPressed()
    {
        GetNode<Button>("StartButton").Visible = false;
        GetNode<Button>("QuitButton").Visible = false;
        GetNode<Label>("P1 Instructions").Visible = false;
        GetNode<Label>("P2 Instructions").Visible = false;
        GetNode<Label>("P1WinLabel").Visible = false;
        GetNode<Label>("P2WinLabel").Visible = false;

        p1Score = 0;
        p2Score = 0;
        GetNode<Label>("P1Score").Text = p1Score.ToString();
        GetNode<Label>("P2Score").Text = p2Score.ToString();
        NewGame();
    }

    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }

    private void OnButtonMouseEntered()
    {
        GetNode<AudioStreamPlayer2D>("MenuClick").Play();
    }

    private void OnButtonMouseExited()
    {
        GetNode<AudioStreamPlayer2D>("MenuClick").Play();
    }

    private void OnBackgroundMusicFinished()
    {
        GetNode<AudioStreamPlayer2D>("Background music").Play();
    }
}

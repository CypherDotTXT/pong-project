using Godot;
using System;

public partial class Main : Node2D
{
    private int p1Score = 0;
    private int p2Score = 0;
    private int countdownValue = 4;
    private Label countdownLabel;

    public override void _Ready()
    {
        countdownLabel = GetNode<Label>("CountdownLabel");
        countdownLabel.Visible = false;

        var ball = GetNode<BallMovement>("Ball");
        ball.Player1Scored += OnPlayer1Scored;
        ball.Player2Scored += OnPlayer2Scored;
        NewGame();
    }

    private void OnPlayer1Scored()
    {
        p1Score++;
        GetNode<Label>("P1Score").Text = p1Score.ToString();
        NewGame();
    }

    private void OnPlayer2Scored()
    {
        p2Score++;
        GetNode<Label>("P2Score").Text = p2Score.ToString();
        NewGame();
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
        }
        else
        {
            countdownLabel.Visible = false;

            GetNode<BallMovement>("Ball").StartMoving();
            GetNode<Timer>("StartTimer").Stop();
            countdownValue = 4; // Reset for next time
        }
    }
}

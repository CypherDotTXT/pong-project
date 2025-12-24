using Godot;
using System;

public partial class Main : Node2D
{
    private int _p1score = 0;
    private int _p2score = 0;

    /* KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);
    Node collider = collision.GetCollider() as Node; */

    public void UponScore()
    {
        var hasScored = new BallMovement();
        bool p1Scored = hasScored.HandleScoreP1();
        bool p2Scored = hasScored.HandleScoreP2();

        if (p1Scored == true)
        {
            _p1score++;
            GetNode<Label>("P1Score").Text = _p1score.ToString();
        }
        else if (p2Scored == true)
        {
            _p2score++;
            GetNode<Label>("P2Score").Text = _p2score.ToString();
        }
        else
        {
            return;
        }
    }

    public void NewGame()
    {
        var p1 = GetNode<Node2D>("Player one");
        var p2 = GetNode<Node2D>("Player two");
        var ball = GetNode<Node2D>("Ball");

        var p1StartPosition = GetNode<Marker2D>("P1 Start");
        var p2StartPosition = GetNode<Marker2D>("P2 Start");
        var ballStartPosition = GetNode<Marker2D>("Ball Start");

        p1.Position = p1StartPosition.Position;
        p2.Position = p2StartPosition.Position;
        ball.Position = ballStartPosition.Position;

        GetNode<Timer>("StartTimer").Start();
    }
}

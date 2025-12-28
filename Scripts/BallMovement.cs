using Godot;
using System;

public partial class BallMovement : CharacterBody2D
{
    [Export] public float speed = 500.0f;
    private Vector2 direction;
    [Signal]
    public delegate void Player1ScoredEventHandler();
    [Signal]
    public delegate void Player2ScoredEventHandler();

    public bool p1Scored = false;
    public bool p2Scored = false;

    public override void _Ready()
    {
        direction = Vector2.Zero; // Start stationary
    }

    public void StartMoving()
    {
        float startAngleDegrees = (float)GD.RandRange(15.0, 75.0);
        float angleRadians = Mathf.DegToRad(startAngleDegrees);
        Vector2 direction = new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));

        if (GD.Randf() > 0.5f) direction.X *= -1;
        if (GD.Randf() > 0.5f) direction.Y *= -1;

        Velocity = direction * speed;
    }
    public override void _PhysicsProcess(double delta)
    {
        KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);

        if (collision == null)
            return;

        if (collision != null)
        {
            Node collider = collision.GetCollider() as Node;
            string stringCollider = collider.Name;

            if (collider.IsInGroup("Player"))
            {
                HandlePlayerBounce(collider);
            }
            else if (stringCollider == "Right Boundary")
            {
                EmitSignal(SignalName.Player1Scored);
            }
            else if (stringCollider == "Left Boundary")
            {
                EmitSignal(SignalName.Player2Scored);
            }
            else
            {
                Velocity = Velocity.Bounce(collision.GetNormal());
            }
        }
    }

    private void HandlePlayerBounce(Node player)
    {
        var paddle = player as CharacterBody2D;
        if (paddle == null) return;

        var shape = paddle.GetNode<CollisionShape2D>("CollisionShape2D")
                      .Shape as RectangleShape2D;

        if (shape == null) return;

        float paddleY = paddle.GlobalPosition.Y;
        float ballY = GlobalPosition.Y;

        float halfHeight = shape.Size.Y / 2f;
        float relativeHit = (ballY - paddleY) / halfHeight;
        relativeHit = Mathf.Clamp(relativeHit, -1f, 1f);

        float maxAngle = Mathf.DegToRad(60);
        float bounceAngle = relativeHit * maxAngle;

        float directionX = -Mathf.Sign(Velocity.X);

        Vector2 newDirection = new Vector2(
            Mathf.Cos(bounceAngle) * directionX,
            Mathf.Sin(bounceAngle)
        );

        Velocity = newDirection.Normalized() * speed;
    }

    public void Stop()
    {
        direction = Vector2.Zero;
        Velocity = Vector2.Zero;
    }
}

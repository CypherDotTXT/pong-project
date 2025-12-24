using Godot;
using System;

public partial class BallMovement : CharacterBody2D
{
    [Export] public float speed = 500.0f;

    public override void _Ready()
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
        Node collider = collision.GetCollider() as Node;

        Node current = collider;
        while (current != null)
        {
            current = current.GetParent();
        }

        if (collision != null)
        {
            GD.Print(collider.Name); //Maybe try printing "current" name? See what comes up

            Velocity = Velocity.Bounce(collision.GetNormal()); //Get rid of this line when uncommenting below lines when testing HandlePlayerBounce

            /* When the code below is used there is no collision bounce at all, 
            which means that the if statement might be coming back as false.
            OR ther may be an issue with what variable "current" is outputing *\

            /* if (current.IsInGroup("Player")) 
            {
                HandlePlayerBounce(current);
            }
            else
            {
                Velocity = Velocity.Bounce(collision.GetNormal());
            } */
        }
    }

    private void HandlePlayerBounce(Node player)
    {
        GD.Print("PADDLE HIT");

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
}

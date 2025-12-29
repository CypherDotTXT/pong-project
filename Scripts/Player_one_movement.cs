using Godot;
using System;

public partial class Player_one_movement : StaticBody2D
{
    [Export] public int speed { get; set; } = 300;
    public Vector2 Screensize;

    public override void _Ready()
    {
        Screensize = GetViewportRect().Size;
    }

    public override void _Process(double delta)
    {
        var velocity = Vector2.Zero;

        if (Input.IsActionPressed("move_up"))
            velocity.Y -= 1;

        if (Input.IsActionPressed("move_down"))
            velocity.Y += 1;

        Position += velocity * speed * (float)delta;
        Position = new Vector2(
            Mathf.Clamp(Position.X, 50, Screensize.X - 50),
            Mathf.Clamp(Position.Y, 50, Screensize.Y - 50)
        );
    }
}

using Godot;
using System;

public partial class Player_two_movement : CharacterBody2D
{
    [Export] public int Speed { get; set; } = 300;
    public Vector2 ScreenSize;
    public override void _Ready()
    {
        ScreenSize = GetViewportRect().Size;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Vector2.Zero;

        if (Input.IsActionPressed("move_up2"))
            velocity.Y -= 1;

        if (Input.IsActionPressed("move_down2"))
            velocity.Y += 1;
        
        Velocity = velocity.Normalized() * Speed;
        MoveAndSlide();
    }
}
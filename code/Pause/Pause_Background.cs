using Godot;
using System;

public class Pause_Background : TextureRect
{
    public bool Press_Left_Mouse = false;
    public Vector2 distance_pos;
    public override void _GuiInput(InputEvent @event)
    {
        if (Input.IsActionJustPressed("Left_Mouse"))
        {
            var Pause_main = GetNode<Node2D>("..");
            distance_pos=Pause_main.GetLocalMousePosition()-RectPosition;
            Press_Left_Mouse = true;
        }
        else if (Input.IsActionJustReleased("Left_Mouse"))
        {
            Press_Left_Mouse = false;
        }
    }
    public override void _Process(float delta)
    {
        if (!GetTree().Paused)
        {
            Press_Left_Mouse = false;
        }
        if (Press_Left_Mouse)
        {
            var Pause_main = GetNode<Node2D>("..");
            RectPosition = Pause_main.GetLocalMousePosition() - distance_pos;
        }
    }
}

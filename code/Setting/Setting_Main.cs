using Godot;
using System;

public class Setting_Main : Control
{
    public override void _Ready()
    {
        Raise();
        var Click = GetNode<AudioStreamPlayer>("Setting/button_Click");
        Click.Stream.Set("loop", false);
        Hide();
    }
    public override void _Process(float delta)
    {
        if (!Public_Main.Starting)
        {
            var Pause_main = GetNode<Pause_Main>("/root/Pause");
            GetTree().Paused = Visible||Pause_main.Visible;
        }
        if (Input.IsActionJustPressed("Cancel") && !Public_Main.Starting)
        {
            var Pause_main = GetNode<Pause_Main>("/root/Pause");
            if (!Pause_main.Visible)
            {
                var Click = GetNode<AudioStreamPlayer>("Setting/button_Click");
                Click.Play();
                Raise();
                Visible = !Visible;
                GetTree().Paused = !GetTree().Paused;
            }
        }
    }
}

using Godot;
using System;

public class Pause_Main : Control
{
    public override void _Ready()
    {
        Vector2 new_pos;
        new_pos.x = 352f;
        new_pos.y = 167.5f;
        var This_Background = GetNode<TextureRect>("Pause/Background");
        This_Background.RectPosition = new_pos;
        //Raise();
        var Click = GetNode<AudioStreamPlayer>("Pause/button_Click");
        Click.Stream.Set("loop", false);
        Hide();
    }
    public override void _Process(float delta)
    {
        if (!Public_Main.Starting)
        {
            var Setting_main = GetNode<Setting_Main>("/root/Setting");
            GetTree().Paused = Visible||Setting_main.Visible;
        }
        if (Input.IsActionJustPressed("Space") && !Public_Main.Starting)
        {
            var Setting_main = GetNode<Setting_Main>("/root/Setting");
            if (!Setting_main.Visible)
            {
                Vector2 new_pos;
                new_pos.x = 352f;
                new_pos.y = 167.5f;
                var This_Background = GetNode<TextureRect>("Pause/Background");
                This_Background.RectPosition = new_pos;
                var Click = GetNode<AudioStreamPlayer>("Pause/button_Click");
                Click.Play();
                Raise();
                Visible = !Visible;
                GetTree().Paused = !GetTree().Paused;
            }
        }
    }
}

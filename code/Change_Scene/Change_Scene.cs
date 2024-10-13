using Godot;
using System;

public class Change_Scene : ColorRect
{
    public Color color_godot;
    public override void _Ready()
    {
        color_godot.r = 0;
        color_godot.g = 0;
        color_godot.b = 0;
        color_godot.a = 0;
        Color = color_godot;
        Hide();
    }
    public void Set_B(bool show=true)
    {
        color_godot.r = 0;
        color_godot.g = 0;
        color_godot.b = 0;
        color_godot.a = 1;
        Color = color_godot;
        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    public void Set_W(bool show = true)
    {
        color_godot.r = 1;
        color_godot.g = 1;
        color_godot.b = 1;
        color_godot.a = 1;
        Color = color_godot;
        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    public void Set_E(bool show = true)
    {
        color_godot.r = 0;
        color_godot.g = 0;
        color_godot.b = 0;
        color_godot.a = 0;
        Color = color_godot;
        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    public async void B_to_E(Control hide_control=null)
    {
        color_godot.r = 0;
        color_godot.g = 0;
        color_godot.b = 0;
        color_godot.a = 1;
        Color = color_godot;
        Raise();
        Show();
        var player = GetNode<AnimationPlayer>("AnimationPlayer");
        player.Play("B_to_E");
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        if (hide_control==null)
        { }
        else
        {
            hide_control.Hide();
        }
        await ToSignal(player, "animation_finished");
        Hide();
    }
    public async void E_to_B(Control hide_control = null)
    {
        color_godot.r = 0;
        color_godot.g = 0;
        color_godot.b = 0;
        color_godot.a = 0;
        Color = color_godot;
        Raise();
        Show();
        var player = GetNode<AnimationPlayer>("AnimationPlayer");
        player.Play("E_to_B");
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        if (hide_control == null)
        { }
        else
        {
            hide_control.Hide();
        }
        await ToSignal(player, "animation_finished");
    }
    public async void W_to_E(Control hide_control = null)
    {
        color_godot.r = 1;
        color_godot.g = 1;
        color_godot.b = 1;
        color_godot.a = 1;
        Color = color_godot;
        Raise();
        Show();
        var player = GetNode<AnimationPlayer>("AnimationPlayer");
        player.Play("W_to_E");
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        if (hide_control == null)
        { }
        else
        {
            hide_control.Hide();
        }
        await ToSignal(player, "animation_finished");
        Hide();
    }
    public async void E_to_W(Control hide_control = null)
    {
        color_godot.r = 1;
        color_godot.g = 1;
        color_godot.b = 1;
        color_godot.a = 0;
        Color = color_godot;
        Raise();
        Show();
        var player = GetNode<AnimationPlayer>("AnimationPlayer");
        player.Play("E_to_W");
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        if (hide_control == null)
        { }
        else
        {
            hide_control.Hide();
        }
        await ToSignal(player, "animation_finished");
    }
}

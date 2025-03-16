using Godot;
using System;

public class In_Game_Choose_Card_OK : Button
{
    public async override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/In_Game/button_Click");
        Click.Play();
        var player1 = GetNode<AnimationPlayer>("/root/In_Game/Main/Card_Player");
        var player2 = GetNode<AnimationPlayer>("/root/In_Game/Main/AnimationPlayer");
        player1.Play("ok");
        await ToSignal(player1, "animation_finished");
        player2.Play("R_to_C");
        await ToSignal(player2, "animation_finished");
        In_Game_Main.Game_Start_Effect(this);
    }
}

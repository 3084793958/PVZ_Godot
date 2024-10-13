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
        var Before_bgm = GetNode<AudioStreamPlayer>("/root/In_Game/Before_bgm");
        Before_bgm.Stop();
        var player3 = GetNode<AnimationPlayer>("/root/In_Game/Ready_Animation");
        player3.Play("start");
        In_Game_Main.is_playing = true;
        await ToSignal(GetTree().CreateTimer(3), "timeout");
        if (In_Game_Main.background_number == 1)
        {
            var B1_bgm = GetNode<AudioStreamPlayer>("/root/In_Game/Music/GrassWalk");
            B1_bgm.Play();
        }
        else if (In_Game_Main.background_number == 3)
        {
            var B3_bgm = GetNode<AudioStreamPlayer>("/root/In_Game/Music/WateryGraves");
            B3_bgm.Play();
        }
    }
}

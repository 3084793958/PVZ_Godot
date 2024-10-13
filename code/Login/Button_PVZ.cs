using Godot;
using System;

public class Button_PVZ : TextureButton
{
    public float music_sec = 0f;
    public override void _Ready()
    {
        var music_player = GetNode<AudioStreamPlayer>("PVZ");
        music_player.Stream.Set("loop", false);
    }
    public override void _Pressed()
    {
        var music_player = GetNode<AudioStreamPlayer>("PVZ");
        if (!music_player.Playing)
        {
            music_player.Play(music_sec);
        }
        else
        {
            music_sec = music_player.GetPlaybackPosition();
            music_player.Stop();
        }
        Public_Main.bgm_value = 0;
        var Bgm_HSlider = GetNode<HSlider>("/root/Setting/Menu/BGM/HSlider");
        Bgm_HSlider.Value = Public_Main.bgm_value;
    }
}

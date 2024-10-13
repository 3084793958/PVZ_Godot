using Godot;
using System;

public class Set_sound : AudioStreamPlayer
{
    [Export] public bool set_loop = false;
    public override void _Ready()
    {
        this.Stream.Set("loop", set_loop);
        this.VolumeDb = (float)(10 * Math.Log((float)Public_Main.sound_value / 100) / Math.Log(10));
        this.PauseMode = PauseModeEnum.Process;
    }
    public override void _Process(float delta)
    {
        this.VolumeDb = (float)(10 * Math.Log((float)Public_Main.sound_value / 100) / Math.Log(10));
    }
}

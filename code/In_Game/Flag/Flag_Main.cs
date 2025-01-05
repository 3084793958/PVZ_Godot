using Godot;
using System;

public class Flag_Main : Control
{
    public bool _Played = false;
    public void Play_Player()
    {
        GetNode<AnimationPlayer>("Flag_Player").Play("Player");
        _Played = true;
    }
}

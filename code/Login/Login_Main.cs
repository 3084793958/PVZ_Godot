using Godot;
using System;
using System.Collections.Generic;
public class Login_Main : Node2D
{
    static public string Dropped_File_Name = null;
	public override void _Ready()
	{
        Dropped_File_Name = null;
		Public_Main.Starting = false;
		var Click = GetNode<AudioStreamPlayer>("button_Click");
		Click.Stream.Set("loop", false);
		var Hover = GetNode<AudioStreamPlayer>("button_Hover");
		Hover.Stream.Set("loop", false);
		var player1 = GetNode<AnimationPlayer>("Main/User_Animation");
		player1.Play("load");
		var player2 = GetNode<AnimationPlayer>("Main/Mode_Animation");
		player2.Play("load");
        GetTree().Connect("files_dropped", this, nameof(Files_Dropped));
	}
    public void Files_Dropped(List<string> files,int screen)
    {
        if (Public_Main.user_name != string.Empty)
        {
            Dropped_File_Name = files[0];
            GetNode<Dropped_Level_Main>("Main/Dropped_Level_Main").Start();
        }
    }
}

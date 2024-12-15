using Godot;
using System;

public class Login_Setting : Button
{
	public override void _Pressed()
	{
		var Click = GetNode<AudioStreamPlayer>("/root/Login/button_Click");
		Click.Play();
		var setting = GetNode<Control>("/root/Setting");
		setting.Raise();
		setting.Show();
	}
}

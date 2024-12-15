using Godot;
using System;

public class Button_Quit : Button
{
	public override void _Ready()
	{
		Connect("mouse_entered", this, "Mouse_EnterEvent");
	}
	public async override void _Pressed()
	{
		var Click = GetNode<AudioStreamPlayer>("/root/Login/button_Click");
		Click.Play();
		await ToSignal(GetTree().CreateTimer(0.72f), "timeout");
		GetTree().Quit();
	}
	public void Mouse_EnterEvent()
	{
		var Hover = GetNode<AudioStreamPlayer>("/root/Login/button_Hover");
		Hover.Play();
	}
}

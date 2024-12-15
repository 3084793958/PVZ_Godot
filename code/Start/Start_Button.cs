using Godot;
using System;

public class Start_Button : Button
{
	public override void _Ready()
	{
		Connect("mouse_entered", this, "Mouse_EnterEvent");
		var Click = GetNode<AudioStreamPlayer>("Click");
		Click.Stream.Set("loop", false);
		var Hover = GetNode<AudioStreamPlayer>("Hover");
		Hover.Stream.Set("loop", false);
	}
	public async override void _Pressed()
	{
		var Click = GetNode<AudioStreamPlayer>("Click");
		Click.Play();
		await ToSignal(GetTree().CreateTimer(0.72f), "timeout");
		GetTree().ChangeScene("res://scene/Login/Login.tscn");
	}
	public void Mouse_EnterEvent()
	{
		var Hover = GetNode<AudioStreamPlayer>("Hover");
		Hover.Play();
	}
}

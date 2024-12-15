using Godot;
using System;

public class Button_Mode4 : TextureButton
{
	public override void _Ready()
	{
		Connect("mouse_entered", this, "Mouse_EnterEvent");
	}
	public override void _Pressed()
	{
		var Click = GetNode<AudioStreamPlayer>("/root/Login/button_Click");
		Click.Play();
	}
	public void Mouse_EnterEvent()
	{
		var Hover = GetNode<AudioStreamPlayer>("/root/Login/button_Hover");
		Hover.Play();
	}
}

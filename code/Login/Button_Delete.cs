using Godot;
using System;

public class Button_Delete : Button
{
	public override void _Ready()
	{
		Disabled = Public_Main.choose_user == -1;
	}
	public override void _Process(float delta)
	{
		Disabled = Public_Main.choose_user == -1;
	}
	public override void _Pressed()
	{
		Directory directory = new Directory();
		directory.Remove("user://Users/" + Public_Main.user_list[Public_Main.choose_user]);
		Name_List_Main.Call_Change_number(this);
		Public_Main.user_list.RemoveAt(Public_Main.choose_user);
		var name_list = GetNode<VBoxContainer>("/root/Login/Main/Create_User/Background/Main/Main");
		name_list.GetChild(Public_Main.choose_user).QueueFree();
		Public_Main.choose_user = -1;
	}
}

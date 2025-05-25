using Godot;
using System;
using System.Collections.Generic;

public class Bug_Main : Node2D
{
    //for Android
    protected Timer Android_Timer = new Timer();
    protected bool Is_Double_Click = false;
    //
    public bool by_H2SO4 = false;
    public Normal_Plants_Area Top_Area_2D = null;
    public List<Normal_Plants_Area> Plants_Area_2D_List = new List<Normal_Plants_Area>();
    //public Control_Area_2D plants_Area_2d=null;
    private bool playing = false;
    protected bool real_touching = false;
    protected bool has_Done = false;
    public override void _Ready()
    {
        this.AddChild(Android_Timer);
        Android_Timer.WaitTime = 0.5f;
        Android_Timer.Autostart = false;
        Android_Timer.OneShot = true;
        GetNode<AudioStreamPlayer>("Sound/Press").Stream.Set("loop", false);
        In_Game_Main.Choosing_List.Add(this);
    }
    public void Area2D_Entered(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        if (area2D.Area2D_type == "Plants" && area2D.Sec_Info != "Zombies")
        {
            Plants_Area_2D_List.Add((Normal_Plants_Area)area2D);
        }
    }
    public void Area2D_Exited(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        if (area2D.Area2D_type == "Plants")
        {
            Plants_Area_2D_List.Remove((Normal_Plants_Area)area2D);
        }
    }
    public override void _Process(float delta)
    {
        Android_Timer.OneShot = Public_Main.for_Android;
        if (Input.IsActionJustReleased("Left_Mouse"))
        {
            if (Android_Timer.IsStopped())
            {
                Android_Timer.Start();
            }
            else
            {
                Is_Double_Click = true;
                Android_Timer.Start();
            }
        }
        if (Android_Timer.IsStopped() && Public_Main.for_Android && !Input.IsActionJustReleased("Left_Mouse"))
        {
            Is_Double_Click = false;
        }
        if (Normal_Plants.Choosing && !playing && !has_Done) 
        {
            Show();
            this.GlobalPosition = GetTree().Root.GetMousePosition();
            Top_Area_2D = null;
            for (int i = 0; i < Plants_Area_2D_List.Count; i++)
            {
                if (Plants_Area_2D_List[i] == null)
                {
                    Plants_Area_2D_List.RemoveAt(i);
                    i--;
                    continue;
                }
                if (!Plants_Area_2D_List[i].has_planted)
                {
                    continue;
                }
                if (Top_Area_2D == null)
                {
                    Top_Area_2D = Plants_Area_2D_List[i];
                }
                if (Plants_Area_2D_List[i].ZIndex > Top_Area_2D.ZIndex)
                {
                    Top_Area_2D = Plants_Area_2D_List[i];
                }
                else if (Plants_Area_2D_List[i].ZIndex == Top_Area_2D.ZIndex)
                {
                    if (Plants_Area_2D_List[i].GetParent().GetParent().GetIndex() > Top_Area_2D.GetParent().GetParent().GetIndex())
                    {
                        Top_Area_2D = Plants_Area_2D_List[i];
                    }
                }
            }
            GetNode<Bug_Area2D>("Main/Area2D").Choose_Plants_Area = Top_Area_2D;
            if (Input.IsActionJustPressed("Right_Mouse"))
            {
                GetNode<AudioStreamPlayer>("/root/In_Game/Cancel").Play();
                Normal_Plants.Choosing = false;
                In_Game_Main.Choosing_List.Remove(this);
                Hide();
                this.QueueFree();
                GetNode<TextureRect>("/root/In_Game/Main/Card/BugBank/Bug").Show();
            }
            if (Is_Double_Click || (Input.IsActionJustPressed("Left_Mouse") && !real_touching))
            {
                Is_Double_Click = false;
                if (Plants_Area_2D_List.Count == 0)
                {
                    GetNode<AudioStreamPlayer>("/root/In_Game/Cancel").Play();
                    has_Done = true;
                    Normal_Plants.Choosing = false;
                    In_Game_Main.Choosing_List.Remove(this);
                    Hide();
                    GetNode<TextureRect>("/root/In_Game/Main/Card/BugBank/Bug").Show();
                    this.QueueFree();
                }
                else
                {
                    if (by_H2SO4 || Public_Main.debuging)
                    {
                        has_Done = true;
                        Normal_Plants.Choosing = false;
                        In_Game_Main.Choosing_List.Remove(this);
                        playing = true;
                        GlobalPosition = Top_Area_2D.GlobalPosition;
                        GetNode<Bug_Area2D>("Main/Area2D").Choose_Plants_Area = Top_Area_2D;
                        GetNode<Bug_Area2D>("Main/Area2D").playing = true;
                        GetNode<AnimationPlayer>("Main/DO").Play("DO");
                        GetNode<TextureRect>("/root/In_Game/Main/Card/BugBank/Bug").Show();
                    }
                    else if (In_Game_Main.Sun_Number >= 150)
                    {
                        has_Done = true;
                        Normal_Plants.Choosing = false;
                        In_Game_Main.Choosing_List.Remove(this);
                        playing = true;
                        GlobalPosition = Top_Area_2D.GlobalPosition;
                        GetNode<Bug_Area2D>("Main/Area2D").Choose_Plants_Area = Top_Area_2D;
                        GetNode<Bug_Area2D>("Main/Area2D").playing = true;
                        In_Game_Main.Sun_Number -= 150;
                        In_Game_Main.Update_Sun(this);
                        GetNode<AnimationPlayer>("Main/DO").Play("DO");
                        GetNode<TextureRect>("/root/In_Game/Main/Card/BugBank/Bug").Show();
                    }
                    else
                    {
                        In_Game_Main.Warning_Sun(this);
                    }
                }
            }
        }
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touchEvent)
        {
            real_touching = touchEvent.Device != -1;//真触摸
        }
    }
    public void Re_Set()
    {
        playing = false;
        GetNode<Bug_Area2D>("Main/Area2D").playing = false;
    }
}

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
    public override void _Ready()
    {
        this.AddChild(Android_Timer);
        Android_Timer.WaitTime = 0.3f;
        Android_Timer.Autostart = false;
        Android_Timer.OneShot = true;
        GetNode<AudioStreamPlayer>("Sound/Press").Stream.Set("loop", false);
    }
    public void Area2D_Entered(Control_Area_2D area2D)
    {
        if (area2D.Area2D_type == "Plants")
        {
            var plants_Area_2d = (Normal_Plants_Area)area2D;
            if (plants_Area_2d.has_planted)
            {
                Plants_Area_2D_List.Add(plants_Area_2d);
                Top_Area_2D = Plants_Area_2D_List[0];
                for (int i = 0; i < Plants_Area_2D_List.Count; i++)
                {
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
            }
        }
    }
    public void Area2D_Exited(Control_Area_2D area2D)
    {
        if (area2D.Area2D_type == "Plants")
        {
            var plants_Area_2d = (Normal_Plants_Area)area2D;
            if (plants_Area_2d.has_planted)
            {
                Plants_Area_2D_List.Remove(plants_Area_2d);
                if (Plants_Area_2D_List.Count == 0)
                {
                    Top_Area_2D = null;
                }
                else
                {
                    Top_Area_2D = Plants_Area_2D_List[0];
                    for (int i = 0; i < Plants_Area_2D_List.Count; i++)
                    {
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
                }
                GetNode<Bug_Area2D>("Main/Area2D").Choose_Plants_Area = Top_Area_2D;
            }
        }
    }
    public override void _Process(float delta)
    {
        if (Normal_Plants.Choosing && !playing)
        {
            Show();
            this.GlobalPosition = GetTree().Root.GetMousePosition();
            if (Input.IsActionPressed("Right_Mouse"))
            {
                GetNode<AudioStreamPlayer>("/root/In_Game/Cancel").Play();
                Normal_Plants.Choosing = false;
                Hide();
                this.QueueFree();
                GetNode<TextureRect>("/root/In_Game/Main/Card/BugBank/Bug").Show();
            }
            if (Public_Main.for_Android && Input.IsActionJustReleased("Left_Mouse"))
            {
                if (Android_Timer.IsStopped())
                {
                    Android_Timer.Start();
                }
                else
                {
                    Is_Double_Click = true;
                    Android_Timer.Stop();
                }
            }
            if ((Input.IsActionPressed("Left_Mouse") && !Public_Main.for_Android) || (Public_Main.for_Android && Is_Double_Click))
            {
                Is_Double_Click = false;
                if (Plants_Area_2D_List.Count == 0)
                {
                    GetNode<AudioStreamPlayer>("/root/In_Game/Cancel").Play();
                    Normal_Plants.Choosing = false;
                    Hide();
                    this.QueueFree();
                    GetNode<TextureRect>("/root/In_Game/Main/Card/BugBank/Bug").Show();
                }
                else
                {
                    if (by_H2SO4 || Public_Main.debuging)
                    {
                        Normal_Plants.Choosing = false;
                        playing = true;
                        GlobalPosition = Top_Area_2D.GlobalPosition;
                        GetNode<Bug_Area2D>("Main/Area2D").Choose_Plants_Area = Top_Area_2D;
                        GetNode<Bug_Area2D>("Main/Area2D").playing = true;
                        GetNode<AnimationPlayer>("Main/DO").Play("DO");
                        GetNode<TextureRect>("/root/In_Game/Main/Card/BugBank/Bug").Show();
                    }
                    else if (In_Game_Main.Sun_Number >= 150)
                    {
                        Normal_Plants.Choosing = false;
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
}

using Godot;
using System;
using System.Collections.Generic;
public class Hammer_Main : Node2D
{
    //for Android
    protected Timer Android_Timer = new Timer();
    protected bool Is_Double_Click = false;
    //
    protected bool real_touching = false;
    [Export] public bool Running = false;
    protected Normal_Zombies_Area Zombies_Area_2D = null;
    protected List<Normal_Zombies_Area> Zombies_Area_2D_List = new List<Normal_Zombies_Area>();
    public override void _Ready()
    {
        this.AddChild(Android_Timer);
        Android_Timer.WaitTime = 0.5f;
        Android_Timer.Autostart = false;
        Android_Timer.OneShot = true;
        GetNode<AudioStreamPlayer>("Sound/Press").Stream.Set("loop", false);
        GetNode<Crash_Area_2D>("Crash_Area").Re_Used = true;
    }
    protected void Area_Entered(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        string Type_string = area2D?.Area2D_type;
        if (Type_string != null && Type_string == "Zombies")
        {
            Zombies_Area_2D_List.Add((Normal_Zombies_Area)area2D);
        }
    }
    protected void Area_Exited(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        string Type_string = area2D?.Area2D_type;
        if (Type_string != null && Type_string == "Zombies")
        {
            Zombies_Area_2D_List.Remove((Normal_Zombies_Area)area2D);
        }
    }
    public override void _Process(float delta)
    {
        if (!Visible)
        {
            GetNode<AudioStreamPlayer>("Sound/Press").Stop();
        }
    }
    public override void _PhysicsProcess(float delta)
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
        this.Visible = !Normal_Plants.Choosing && In_Game_Main.is_playing;
        if (!Running)
        {
            this.GlobalPosition = GetTree().Root.GetMousePosition();
        }
        Zombies_Area_2D = null;
        for (int i = 0; i < Zombies_Area_2D_List.Count; i++)
        {
            if (Zombies_Area_2D_List[i] == null)
            {
                Zombies_Area_2D_List.RemoveAt(i);
                i--;
                continue;
            }
            if (!Zombies_Area_2D_List[i].has_plant || Zombies_Area_2D_List[i].has_lose_head || Zombies_Area_2D_List[i].health < 0)
            {
                continue;
            }
            if (Zombies_Area_2D != null)
            {
                if (!Zombies_Area_2D.has_plant || Zombies_Area_2D.has_lose_head || Zombies_Area_2D.health < 0)
                {
                    Zombies_Area_2D = null;
                }
            }
            if (Zombies_Area_2D == null)
            {
                Zombies_Area_2D = Zombies_Area_2D_List[i];
            }
            else
            {
                if (Zombies_Area_2D.GetParent().GetParent().GetParent<Node2D>().ZIndex < Zombies_Area_2D_List[i].GetParent().GetParent().GetParent<Node2D>().ZIndex)
                {
                    Zombies_Area_2D = Zombies_Area_2D_List[i];
                }
                else if (Zombies_Area_2D.GetParent().GetParent().GetParent<Node2D>().ZIndex == Zombies_Area_2D_List[i].GetParent().GetParent().GetParent<Node2D>().ZIndex)
                {
                    if (Zombies_Area_2D.GetParent().GetParent().GetParent().GetIndex() < Zombies_Area_2D_List[i].GetParent().GetParent().GetParent().GetIndex())
                    {
                        Zombies_Area_2D = Zombies_Area_2D_List[i];
                    }
                }
            }
        }
        GetNode<Crash_Area_2D>("Crash_Area").Crash_Area = Zombies_Area_2D;
        if (((Is_Double_Click && real_touching) || (Input.IsActionJustPressed("Left_Mouse") && !real_touching)) && !Normal_Plants.Choosing && In_Game_Main.is_playing && Visible)  
        {
            Is_Double_Click = false;
            if (Zombies_Area_2D != null)
            {
                if (!GetNode<AnimationPlayer>("Main/DO").IsPlaying())
                {
                    GetNode<AudioStreamPlayer>("Sound/Press").Play();
                }
            }
            GetNode<AnimationPlayer>("Main/DO").Play("DO");
        }
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touchEvent)
        {
            real_touching = touchEvent.Device != -1;//真触摸
        }
    }
}

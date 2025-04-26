using Godot;
using System;

public class Open_File : FileDialog
{
    public bool real_touching = false;
    protected Timer Android_Timer = new Timer();
    protected bool Is_Double_Click = false;
    public override void _Ready()
    {
        this.AddChild(Android_Timer);
        Android_Timer.WaitTime = 0.5f;
        Android_Timer.Autostart = false;
        Android_Timer.OneShot = true;
    }
    public override void _Process(float delta)
    {
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
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touchEvent)
        {
            real_touching = touchEvent.Device != -1;//真触摸
            if (real_touching)
            {
                if (Is_Double_Click)
                {
                    var ent = new InputEventMouseButton();
                    ent.Pressed = touchEvent.IsPressed();
                    ent.ButtonIndex = (int)ButtonList.Left;
                    ent.Position = touchEvent.Position + this.RectPosition;
                    Is_Double_Click = false;
                    ent.Doubleclick = true;
                    Input.ParseInputEvent(ent);
                }
            }
        }
    }
}

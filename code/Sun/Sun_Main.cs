using Godot;
using System;

public class Sun_Main : Node2D
{
    [Export] public int sun_value = 50;
    [Export] public float size = 0.5f;
    public bool is_from_plant = false;
    [Export] public bool is_lock = false;
    protected int Down_Run_Time = 0;
    protected int Get_Run_Time = 0;
    protected Vector2 Down_Delta_Vector2 = new Vector2(0f, 0.6f);
    protected Vector2 Get_Delta_Vector2 = new Vector2(0f, 0.6f);
    public override void _Ready()
    {
        GetNode<AudioStreamPlayer>("Sun_player").Stream.Set("loop", false);
        this.Scale = new Vector2(size,size);
        GetNode<AnimationPlayer>("Sun/AnimationPlayer").Play("sun");
        if (is_from_plant)
        {
            this.Position += new Vector2((float)GD.RandRange(-10d, 10d),0);
            GetNode<AnimationPlayer>("Sun/Running").Play("SunFlower");
            GetNode<Timer>("Timer").Start();
        }
        else
        {
            Down_Delta_Vector2 = new Vector2(0f, 1f);
            Down_Run_Time = (int)GD.RandRange(100, 590);
            is_lock = false;
            GetNode<Timer>("Run_Timer").Start();
        }
    }
    public void Hide_TimerOut()
    {
        GetNode<AnimationPlayer>("Sun/Hide").Play("Hide");
    }
    public void Run_Timer_Timeout()
    {
        var Run_Timer = GetNode<Timer>("Run_Timer");
        if (is_lock)
        {
            if (Get_Run_Time > 0)
            {
                this.Position -= Get_Delta_Vector2;
                Get_Run_Time--;
            }
            else
            {
                GetNode<AnimationPlayer>("Sun/Hide").Play("Hide");
                Run_Timer.Stop();
            }
        }
        else
        {
            if (Down_Run_Time > 0)
            {
                this.Position += Down_Delta_Vector2;
                Down_Run_Time--;
            }
            else
            {
                GetNode<Timer>("Timer").Start();
                Run_Timer.Stop();
            }
        }
    }
    public override void _Process(float delta)
    {
        if (In_Game_Main.is_playing && !GetTree().Paused) 
        {
            var Mouse_position = GetTree().Root.GetMousePosition();
            if (Math.Abs(GetNode<Control>("Sun").RectGlobalPosition.x - Mouse_position.x) < 40 && Math.Abs(GetNode<Control>("Sun").RectGlobalPosition.y - Mouse_position.y) < 40)
            {
                if (Input.IsActionPressed("Left_Mouse") && !is_lock) 
                {
                    GetNode<Timer>("Run_Timer").Stop();
                    is_lock = true;
                    GetNode<AudioStreamPlayer>("Sun_player").Play();
                    In_Game_Main.Sun_Number += this.sun_value;
                    In_Game_Main.Update_Sun(this);
                    Get_Delta_Vector2 = new Vector2(((float)(Position.x + 250)) / 20, ((float)(Position.y + 250)) / 20);
                    Get_Run_Time = 20;
                    GetNode<Timer>("Run_Timer").Start();
                }
            }
        }
    }
}

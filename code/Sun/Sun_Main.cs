using Godot;
using System;

public class Sun_Main : Node2D
{
    [Export] public int sun_value = 50;
    [Export] public float size = 0.5f;
    public bool is_from_plant = false;
    [Export] public bool is_lock = false;
    public override async void _Ready()
    {
        GetNode<AudioStreamPlayer>("Sun_player").Stream.Set("loop", false);
        this.Scale = new Vector2(size,size);
        GetNode<AnimationPlayer>("Sun/AnimationPlayer").Play("sun");
        if (is_from_plant)
        {
            this.Position += new Vector2((float)GD.RandRange(-10d, 10d),0);
            GetNode<AnimationPlayer>("Sun/Running").Play("SunFlower");
        }
        else
        {
            var Delta_Vector2 = new Vector2(0f, 0.6f);
            for (int i = 0; i < GD.RandRange(100,1000); i++)
            {
                if (is_lock)
                {
                    break;
                }
                await ToSignal(GetTree().CreateTimer(0.03f), "timeout");
                this.Position += Delta_Vector2;
            }
            is_lock = true;
        }
    }
    public void Hide_TimerOut()
    {
        if (is_lock)
        {
            GetNode<AnimationPlayer>("Sun/Hide").Play("Hide");
        }
    }
    public async void Click_Button_pressed()
    {
        GetNode<Button>("Sun/Click_Button").Visible = false;
        is_lock = true;
        GetNode<AudioStreamPlayer>("Sun_player").Play();
        In_Game_Main.Sun_Number += this.sun_value;
        In_Game_Main.Update_Sun(this);
        var Delta_Vector2 = new Vector2(((float)(Position.x + 220)) / 22, ((float)(Position.y + 0)) / 22);
        for (int i = 0; i < 22; i++)
        {
            await ToSignal(GetTree().CreateTimer(0.03f), "timeout");
            this.Position -= Delta_Vector2;
        }
        GetNode<AnimationPlayer>("Sun/Hide").Play("Hide");
    }
}

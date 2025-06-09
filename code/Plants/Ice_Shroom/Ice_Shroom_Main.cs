using Godot;
using System;

public class Ice_Shroom_Main : Normal_Plants
{
    public bool doing_bug = false;
    [Export] public bool has_boom = false;
    public override void _PhysicsProcess(float delta)
    {
        if (!GetNode<Area2D>("Main/Shovel_Area").IsConnected("area_entered", this, nameof(Area_Entered)))
        {
            return;
        }
        base._PhysicsProcess(delta);
        if (has_planted)
        {
            if (Zombies_Area_2D_List.Count != 0)
            {
                for (int i = 0; i < Zombies_Area_2D_List.Count; i++)
                {
                    if (Zombies_Area_2D_List[i].Choose_Plants_Area == GetNode<Normal_Plants_Area>("Main/Shovel_Area"))
                    {
                        if (Zombies_Area_2D_List[i].is_eating_show && !Zombies_Area_2D_List[i].has_lose_head)
                        {
                            Zombies_Area_2D_List[i].is_eating_show = false;
                            health -= Zombies_Area_2D_List[i].hurt;
                            GetNode<AnimationPlayer>("Is_Eated").Play("Is_Eated");
                        }
                    }
                }
            }
            if (health <= 0)
            {
                if (!GetNode<AnimationPlayer>("Died").IsPlaying())
                {
                    GetNode<AnimationPlayer>("Died").Play("Died");
                    Plants_Remove_List();
                }
            }
        }
    }
    public override void _Ready()
    {
        can_sleep = true;
        GetNode<AudioStreamPlayer>("Main/Ice_Lock").Stream.Set("loop", false);
        base._Ready();
    }
    protected override void Plants_Add_List()
    {
        if (Dock_Area_2D == null)
        {
            return;
        }
        Dock_Area_2D.Normal_Plant_List.Add(this);
    }
    protected override void Plants_Remove_List()
    {
        if (Dock_Area_2D == null)
        {
            return;
        }
        Dock_Area_2D.Normal_Plant_List.Remove(this);
    }
    protected override void Plants_Init()
    {
        if (can_sleep && In_Game_Main.allow_sun && !Public_Main.debuging)
        {
            sleep = true;
        }
        GetNode<AnimationPlayer>("Main/Player1").Play("Player1");
        if (!sleep)
        {
            GetNode<AnimationPlayer>("Main/Player2").Play("Player1");
            GetNode<Node2D>("Main/Sleep").Hide();
            GetNode<Timer>("Ice_Doing_Timer").Start();
        }
        else
        {
            GetNode<Sprite>("Main/eyes/Eye1").Hide();
            GetNode<Sprite>("Main/eyes/Eye2").Show();
            GetNode<Node2D>("Main/Sleep").Show();
            GetNode<AnimationPlayer>("Main/Sleep/Sleep").Play("Sleep");
        }
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
    }
    protected override bool Allow_Plants()
    {
        return ((In_Game_Main.Sun_Number >= this_sun && Dock_Area_2D.Normal_Plant_List.Count == 0 && Dock_Area_2D.now_type[Dock_Area_2D.now_type.Count - 1] == 1) || Public_Main.debuging) && on_lock_grid;
    }
    public void Bug_Doing()
    {
        if (sleep)
        {
            return;
        }
        doing_bug = true;
    }
    public override void Free_Self()
    {
        Plants_Remove_List();
        if (!has_boom && !on_Shovel)
        {
            Ice_Doing_Timer_timeout();
            has_boom = true;
            GetNode<AnimationPlayer>("Died").Play("Died");
        }
        else
        {
            base.Free_Self();
        }
    }
    protected void Ice_Doing_Timer_timeout()
    {
        if (!has_boom)
        {
            GetNode<AudioStreamPlayer>("Main/Ice_Lock").Play();
            GetNode<AnimationPlayer>("Main/Shining/Shining").Play("Shining");
            has_boom = true;
            for (int i = 0; i < GetNode<Control>("/root/In_Game/Object").GetChildCount(); i++)
            {
                if (GetNode<Control>("/root/In_Game/Object").GetChild(i) is Normal_Zombies zombies_object)
                {
                    if (zombies_object.has_planted)
                    {
                        zombies_object.Ice_Lock_Do();
                        zombies_object.health_list[0].Health -= 50 + 300 * Convert.ToInt32(doing_bug);
                    }
                }
                else if (GetNode<Control>("/root/In_Game/Object").GetChild(i) is Tomb_Main Tomb_object)
                {
                    if (Tomb_object.has_planted)
                    {
                        Tomb_object.health -= 50 + 300 * Convert.ToInt32(doing_bug);
                    }
                }
            }
        }
    }
}

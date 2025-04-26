using Godot;
using System;

public class Level_Page_Control_2 : Control
{
    static public int Choose_Page = 1;
    public override void _Ready()
    {
        Choose_Page = 1;
        Update_Page();
    }
    public void Up()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/Mode2/button_Click");
        Click.Play();
        if (Choose_Page > 1)
        {
            Choose_Page--;
        }
        Update_Page();
    }
    public void Down()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/Mode2/button_Click");
        Click.Play();
        int Max_Number = 1;
        for (int i = 0; i < this.GetChildCount(); i++)
        {
            if (this.GetChild(i).GetChildCount() < 15)
            {
                Max_Number = i + 1;
                break;
            }
        }
        if (Choose_Page < Max_Number)
        {
            Choose_Page++;
        }
        Update_Page();
    }
    public void Update_Page()
    {
        for (int i = 0; i < this.GetChildCount(); i++)
        {
            GetNode<Control>("Page" + (i + 1).ToString()).Visible = Choose_Page == i + 1;
        }
    }
}

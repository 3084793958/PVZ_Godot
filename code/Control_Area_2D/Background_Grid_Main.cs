using Godot;
using System;
using System.Collections.Generic;

public class Background_Grid_Main : Control_Area_2D
{
    [Export] public int type=1;//1:land 2:water
    [Export] public int[] pos = { 1, 1 };//行,列
    public List<Node2D> Normal_Plant_List=new List<Node2D>();
    public List<Node2D> Down_Plant_List = new List<Node2D>();
    public List<Node2D> Top_Plant_List = new List<Node2D>();
    public List<Node2D> Casing_Plant_List = new List<Node2D>();
    public List<int> now_type = new List<int>();//末项
    public List<Node2D> Small_Plants_List = new List<Node2D> { null, null, null };
    public List<Vector2> Plants_Up_Pos_List = new List<Vector2>();
    public bool on_PL_Casing_Save = false;
    public bool on_New_Horizons = false;
    public Node2D pH_Meter = null;
    [Export] public float pH = 7f;
    public float N_H = 1E-7f;
    public float N_OH = 1E-7f;
    public float N_H2O = 55.6f;
    public float Kw = 1E-14f;
    //-23,0
    public override void _Ready()
    {
        now_type.Add(type);
        pos[1] = int.Parse(this.GetParent().Name);
        pos[0] = int.Parse(this.Name);
        Area2D_type = "Grid";
    }
    public void Change_pH(float n_H, float n_OH, float n_H2O)
    {
        N_H += n_H;
        N_OH += n_OH;
        N_H2O += n_H2O;
        if (N_H2O < 1)
        {
            N_H2O = 1;
        }
        float Delta_N = 0f;
        if (N_H > N_OH)
        {
            N_H -= N_OH;
            N_H2O += N_OH;
            N_OH = 0f;
            //float精度不足,故用double
            double a = 1 - Kw * 3091.36f;
            double b = 2 * Kw * N_H2O / 3091.36f + N_H;
            double c = -Kw * N_H2O / 55.6f * N_H2O / 55.6f;
            Delta_N = (float)((-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a));
            N_H2O -= Delta_N;
            N_H += Delta_N;
            N_OH += Delta_N;
        }
        else
        {
            N_OH -= N_H;
            N_H2O += N_H;
            N_H = 0f;
            //float精度不足,故用double
            double a = 1 - Kw * 3091.36f;
            double b = 2 * Kw * N_H2O / 3091.36f + N_OH;
            double c = -Kw * N_H2O / 55.6f * N_H2O / 55.6f;
            Delta_N = (float)((-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a));
            N_H2O -= Delta_N;
            N_H += Delta_N;
            N_OH += Delta_N;
        }
        if (N_H2O < 1)
        {
            N_H2O = 1;
        }
        pH = -(float)Math.Log10(N_H / (N_H2O / 55.6f));
    }
    public override void _PhysicsProcess(float delta)
    {
        bool has_New_Horizons = false;
        for (int i = 0; i < Normal_Plant_List.Count; i++)
        {
            if (Normal_Plant_List[i] == null || !IsInstanceValid(Normal_Plant_List[i])) 
            {
                Normal_Plant_List.RemoveAt(i);
                i--;
            }
            if (Normal_Plant_List[i] is New_Horizons_Main New_Horizons_object)
            {
                if (!New_Horizons_object.sleep)
                {
                    has_New_Horizons = true;
                }
            }
        }
        on_New_Horizons = has_New_Horizons;
        for (int j = 0; j < Small_Plants_List.Count; j++)
        {
            bool has_Plants = false;
            for (int i = 0; i < Normal_Plant_List.Count; i++)
            {
                if ((Small_Plants_List[j] != null || IsInstanceValid(Small_Plants_List[j])) && Normal_Plant_List[i] == Small_Plants_List[j]) 
                {
                    has_Plants = true;
                    break;
                }
            }
            if (!has_Plants)
            {
                Small_Plants_List[j] = null;
            }
        }
        for (int i = 0; i < Down_Plant_List.Count; i++)
        {
            if (Down_Plant_List[i] == null || !IsInstanceValid(Down_Plant_List[i]))  
            {
                Down_Plant_List.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < Top_Plant_List.Count; i++)
        {
            if (Top_Plant_List[i] == null || !IsInstanceValid(Top_Plant_List[i]))
            {
                Top_Plant_List.RemoveAt(i);
                i--;
            }
        }
        bool has_PL_Casing = false;
        for (int i = 0; i < Casing_Plant_List.Count; i++)
        {
            if (Casing_Plant_List[i] == null || !IsInstanceValid(Casing_Plant_List[i]))
            {
                Casing_Plant_List.RemoveAt(i);
                i--;
                continue;
            }
            if (Casing_Plant_List[i] is PL_Casing_Main)
            {
                has_PL_Casing = true;
            }
        }
        on_PL_Casing_Save = has_PL_Casing;
        if (pH_Meter != null && !IsInstanceValid(pH_Meter)) 
        {
            pH_Meter = null;
        }
    }
    public bool Allow_Down_Plant(int type)
    {
        bool result = true;
        int lotus_number = 0;
        int pot_number = 0;
        for (int i = 0; i < Down_Plant_List.Count; i++)
        {
            if (Down_Plant_List[i] is Lotus_Main)
            {
                lotus_number++;
            }
            else if (Down_Plant_List[i] is Small_Shroom_Pot_Main) 
            {
                pot_number++;
            }
            else
            {
                result = false;
                break;
            }
        }
        result = result && ((lotus_number < 1 && type == 1) || (pot_number < 1 && type == 2));
        return result;
    }
    public bool Is_All_Small_in_normal()
    {
        bool result = true;
        for (int i = 0; i < Normal_Plant_List.Count; i++)
        {
            if (!(Normal_Plant_List[i] is Small_Shroom_Main|| Normal_Plant_List[i] is Sun_Shroom_Main)) 
            {
                result = false;
                break;
            }
        }
        return result;
    }
    public int Empty_Small_Pos()
    {
        int result = -1;
        for (int i = 0; i < Small_Plants_List.Count; i++)
        {
            if (Small_Plants_List[i] == null) 
            {
                result = i;
                break;
            }
        }
        return result;
    }
}

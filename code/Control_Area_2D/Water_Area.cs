using Godot;
using System;

public class Water_Area : Control_Area_2D
{
    [Export] public int Base_V = 1;
    [Export] public float pH = 7f;
    public float N_H = 1E-7f;
    public float N_OH = 1E-7f;
    public float N_H2O = 55.6f;
    public float Kw = 1E-14f;
    public override void _Ready()
    {
        N_H2O = Base_V * 55.6f;
        N_H *= Base_V;
        N_OH *= Base_V;
        Area2D_type = "Water_Area";
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
}

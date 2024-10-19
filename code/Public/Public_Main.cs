using Godot;
using System;
using System.Collections.Generic;

static public class Public_Main
{
    static public List<string> user_list = new List<string>();
    static public string user_name = string.Empty;
    static public int choose_user = -1;
    static public bool debuging = false;
    static public int bgm_value = 100, sound_value = 100;
    static public bool Starting = true;
    static public int allow_card_number=13;
    static public List<Tuple<int, string, string, string, Texture, float, float, Tuple<int,string>>> Plant_list = new List<Tuple<int, string, string, string, Texture, float, float, Tuple<int, string>>>
    {
    //ID,Name,Info,More_Info,Texture(有搞头,可能变成List<Texture>),First_Time(Normal),Wait_Time(Normal),Sun(Normal),path
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    1,
    "godot",
    "开源的力量!",
    "godot3.5.stable.mono,你值得拥有!",
    GD.Load<Texture>("res://image/Plants/godot/godot.png"),
    30f,
    120f,
    new Tuple<int,string>(0,null)
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    2,
    "向日葵",
    "不要种前排!",
    "由Gimp和Inkscape花费4小时打造",
    GD.Load<Texture>("res://image/Plants/SunFlower/sunflower_All.png"),
    0f,
    10f,
    new Tuple<int,string>(50,"res://scene/Plants/SunFlower/SunFlower.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    3,
    "豌豆射手",
    "发射1颗豌豆,伤害20",
    "发射1颗豌豆,伤害20",
    GD.Load<Texture>("res://image/Plants/Van_Door/Van_Door.png"),
    5f,
    15f,
    new Tuple<int,string>(100,"res://scene/Plants/Van_Door/Van_Door.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    4,
    "樱桃炸弹",
    "血量就是伤害\n他的血量也有3000,非常耐揍",
    "血量就是伤害\n他的血量也有3000,非常耐揍",
    GD.Load<Texture>("res://image/Plants/C_Boom/C_Boom.png"),
    10f,
    30f,
    new Tuple<int,string>(150,"res://scene/Plants/C_Boom/C_Boom.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    5,
    "酒精灯",
    "终有酒精燃尽的一天",
    "从化学实验室回来的酒精灯\n诶?帽子去哪了?",
    GD.Load<Texture>("res://image/Plants/C2H5OH/C2H5OH.png"),
    5f,
    30f,
    new Tuple<int,string>(50,"res://scene/Plants/C2H5OH/C2H5OH.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    6,
    "硫酸",
    "危险的东西",
    "杀敌1000,自损800",
    GD.Load<Texture>("res://image/Plants/H2SO4/H2SO4.png"),
    0f,
    20f,
    new Tuple<int,string>(100,null)
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    7,
    "Mg",
    "燃烧吧!",
    "也不一定要配合酒精灯使用",
    GD.Load<Texture>("res://image/Plants/Mg/Mg.png"),
    10f,
    10f,
    new Tuple<int,string>(25,null)
    )
    };
    static public List<Tuple<int, string, string, string, Texture, float, float, Tuple<int, string>>> Zombies_list = new List<Tuple<int, string, string, string, Texture, float, float, Tuple<int, string>>>
    {
    //僵尸列表
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    1,
    "普通僵尸",
    "这是个历史性的突破",
    "第一个僵尸",
    GD.Load<Texture>("res://image/Zombies/Simple_Zombies/Simple_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(50,"res://scene/Zombies/Simple_Zombies/Simple_Zombies.tscn")
    )
    };
}

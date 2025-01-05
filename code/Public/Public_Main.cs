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
    static public int allow_card_number = 13;
    static public int now_card_number = 0;
    static public bool for_Android = false;//无鼠标模式,双击
    static public List<Tuple<int, string, string, string, Texture, float, float, Tuple<int,string>>> Plant_list = new List<Tuple<int, string, string, string, Texture, float, float, Tuple<int, string>>>
    {
    //ID,Name,Info,More_Info,Texture(有搞头,可能变成List<Texture>),First_Time(Normal),Wait_Time(Normal),Sun(Normal),path
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    1,
    "godot3.5",
    "开源的力量!",
    "godot3.5.stable.mono,你值得拥有!",
    GD.Load<Texture>("res://image/Plants/godot/godot.png"),
    0f,
    35f,
    new Tuple<int,string>(0,"res://scene/Plants/Godot/Godot.tscn")
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
    "发射1颗豌豆",
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
    "危险的东西,伤害500\n试着放上Mg",
    "MgSO4也不错",
    GD.Load<Texture>("res://image/Plants/H2SO4/H2SO4.png"),
    0f,
    20f,
    new Tuple<int,string>(75,"res://scene/Plants/H2SO4/H2SO4.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    7,
    "Mg",
    "燃烧吧!\n在点燃的酒精灯上使用,放出刺眼的光",
    "也不一定要配合酒精灯使用",
    GD.Load<Texture>("res://image/Plants/Mg/Mg.png"),
    10f,
    10f,
    new Tuple<int,string>(25,"res://scene/Plants/Mg/Mg.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    8,
    "坚果墙",
    "与二爷的关系很好",
    "与二爷的关系很好",
    GD.Load<Texture>("res://image/Plants/WallNut/Wallnut.png"),
    10f,
    30f,
    new Tuple<int,string>(50,"res://scene/Plants/WallNut/WallNut.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    9,
    "爆炸坚果墙",
    "4000血量,1800伤害",
    "4000血量,1800伤害",
    GD.Load<Texture>("res://image/Plants/WallNut/Boom_Wallnut.png"),
    10f,
    30f,
    new Tuple<int,string>(75,"res://scene/Plants/Boom_WallNut/Boom_WallNut.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    10,
    "坚果保龄球",
    "4000血量,500伤害",
    "4000血量,500伤害",
    GD.Load<Texture>("res://image/Plants/WallNut/Wallnut.png"),
    0f,
    30f,
    new Tuple<int,string>(25,"res://scene/Plants/WallNut_Ball/WallNut_Ball.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    11,
    "爆炸保龄球",
    "4000血量,1800伤害",
    "4000血量,1800伤害",
    GD.Load<Texture>("res://image/Plants/WallNut/Boom_Wallnut.png"),
    0f,
    30f,
    new Tuple<int,string>(50,"res://scene/Plants/Boom_WallNut_Ball/Boom_WallNut_Ball.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    12,
    "土豆地雷",
    "1800伤害",
    "1800伤害",
    GD.Load<Texture>("res://image/Plants/Potato/Potato.png"),
    15f,
    30f,
    new Tuple<int,string>(25,"res://scene/Plants/Potato/Potato.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    13,
    "大嘴花",
    "一口吃掉",
    "一口吃掉",
    GD.Load<Texture>("res://image/Plants/Eating_Flower/Eating_Flower.png"),
    2.5f,
    45f,
    new Tuple<int,string>(150,"res://scene/Plants/Eating_Flower/Eating_Flower.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    14,
    "寒冰射手",
    "生活在寒带,与豌豆射手不是一族",
    "生活在寒带,与豌豆射手不是一族",
    GD.Load<Texture>("res://image/Plants/Ice_Van_Door/Ice_Van_Door.png"),
    7.5f,
    35f,
    new Tuple<int,string>(175,"res://scene/Plants/Ice_Van_Door/Ice_Van_Door.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    15,
    "双发射手",
    "发射两颗豌豆",
    "发射两颗豌豆",
    GD.Load<Texture>("res://image/Plants/Double_Van_Door/Double_Van_Door.png"),
    5f,
    20f,
    new Tuple<int,string>(150,"res://scene/Plants/Double_Van_Door/Double_Van_Door.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    16,
    "莲叶",
    "莲叶",
    "莲叶",
    GD.Load<Texture>("res://image/Plants/Lotus/Lotus.png"),
    0f,
    15f,
    new Tuple<int,string>(25,"res://scene/Plants/Lotus/Lotus.tscn")
    )
    };
    static public List<Tuple<int, string, string, string, Texture, float, float, Tuple<int, string>>> Plants_Zombies_list = new List<Tuple<int, string, string, string, Texture, float, float, Tuple<int, string>>>
    {
    //魅惑僵尸列表
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    1,
    "魅惑普通僵尸",
    "魅惑僵尸",
    "第一个魅惑僵尸",
    GD.Load<Texture>("res://image/Plants/Zombies/Simple_Zombies/Simple_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(50,"res://scene/Plants/Zombies/Simple_Zombies/Plants_Simple_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    2,
    "魅惑摇旗僵尸",
    "Godot",
    "第二个僵尸",
    GD.Load<Texture>("res://image/Plants/Zombies/Flag_Zombies/Flag_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(100,"res://scene/Plants/Zombies/Flag_Zombies/Plants_Flag_Zombies.tscn")
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
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    2,
    "摇旗僵尸",
    "亡语",
    "第二个僵尸",
    GD.Load<Texture>("res://image/Zombies/Flag_Zombies/Flag_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(100,"res://scene/Zombies/Flag_Zombies/Flag_Zombies.tscn")
    )
    };
    static public List<string> Zombies_Path_List = new List<string>
    {
        "res://scene/Zombies/Simple_Zombies/Simple_Zombies.tscn",
        "res://scene/Zombies/Flag_Zombies/Flag_Zombies.tscn"
    };
}

/*************************************************************************/
/* Public_Main.cs                                                        */
/*                              GPL v3.0                                 */
/*                  This file is part of: PVZ_Godot                      */
/*            <https://github.com/3084793958/PVZ_Godot.git>              */
/*************************************************************************/
/* Copyright (C) 2024-present github.com:3084793958                      */
/*                                                                       */
/* This program is free software: you can redistribute it and/or modify  */
/* it under the terms of the GNU General Public License as published by  */
/* the Free Software Foundation, either version 3 of the License, or     */
/* (at your option) any later version.                                   */
/*                                                                       */
/* This program is distributed in the hope that it will be useful,       */
/* but WITHOUT ANY WARRANTY; without even the implied warranty of        */
/* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         */
/* GNU General Public License for more details.                          */
/*                                                                       */
/* You should have received a copy of the GNU General Public License     */
/* along with this program.  If not, see <https://www.gnu.org/licenses/>.*/
/*************************************************************************/
using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

static public class Public_Main
{
    //for R
    static public bool Using_Clone_Limit = true;
    static public int Max_Object_Clone_In_F = 5;
    //for R
    static public List<string> user_list = new List<string>();
    static public string user_name = string.Empty;
    static public int choose_user = -1;
    static public bool debuging = false;
    static public int bgm_value = 100, sound_value = 100;
    static public bool Starting = true;
    static public int allow_card_number = 13;
    static public int now_card_number = 0;
    static public bool for_Android = false;//无鼠标模式,双击
    static public bool Show_Zombies_Health = true;
    static public bool Show_Plants_Health = true;
    static public bool Show_Plants_Zombies_Health = true;
    static public bool Show_Mouse_Effect = true;
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
    "<!>化学仪器\n内焰具有C2H5OH蒸汽\n终有酒精燃尽的一天\n使用喷剂重燃(不具备科学性)",
    "从化学实验室回来的酒精灯\n诶?帽子去哪了?",
    GD.Load<Texture>("res://image/Plants/C2H5OH/C2H5OH.png"),
    5f,
    30f,
    new Tuple<int,string>(50,"res://scene/Plants/C2H5OH/C2H5OH.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    6,
    "硫酸",
    "<!>危险化学品\n稀硫酸pH=1\nMg+H+->H2+Mg2+\nMg盐有利于植物生长\n危险的东西,伤害1000",
    "MgSO4也不错",
    GD.Load<Texture>("res://image/Plants/H2SO4/H2SO4.png"),
    0f,
    20f,
    new Tuple<int,string>(50,"res://scene/Plants/H2SO4/H2SO4.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    7,
    "Mg",
    "<!>危险化学品\nMg+O2->MgO\nMg+H+->H2+Mg2+\n燃烧后放出刺眼的光,燃烧吧!",
    "也不一定要配合酒精灯使用",
    GD.Load<Texture>("res://image/Plants/Mg/Mg.png"),
    10f,
    20f,
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
    45f,
    new Tuple<int,string>(25,"res://scene/Plants/WallNut_Ball/WallNut_Ball.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    11,
    "爆炸保龄球",
    "4000血量,1800伤害",
    "4000血量,1800伤害",
    GD.Load<Texture>("res://image/Plants/WallNut/Boom_Wallnut.png"),
    0f,
    60f,
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
    "小喷菇",
    "国服小喷菇,可叠种",
    "小喷菇",
    GD.Load<Texture>("res://image/Plants/Small_Shroom/Small_Shroom.png"),
    0f,
    5f,
    new Tuple<int,string>(0,"res://scene/Plants/Small_Shroom/Small_Shroom.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    17,
    "阳光菇",
    "夜晚效率更快,可叠种",
    "夜晚效率更快",
    GD.Load<Texture>("res://image/Plants/Sun_Shroom/SunShroom_All.png"),
    0f,
    10f,
    new Tuple<int,string>(25,"res://scene/Plants/Sun_Shroom/Sun_Shroom.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    18,
    "氢气",
    "<!>化学品\nH2+O2->H2O\n点燃不纯的H2会产生爆炸\n验纯!",
    "氢气",
    GD.Load<Texture>("res://image/Plants/H2/H2.png"),
    0f,
    15f,
    new Tuple<int,string>(5,"res://scene/Plants/H2/H2.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    19,
    "New Horizons",
    "只是个人喜好(O^<)",
    "New Horizons",
    GD.Load<Texture>("res://image/Plants/New_Horizons/New_Horizons.png"),
    0f,
    40f,
    new Tuple<int,string>(385,"res://scene/Plants/New_Horizons/New_Horizons.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    20,
    "大喷菇",
    "大喷菇",
    "大喷菇",
    GD.Load<Texture>("res://image/Plants/Fume_Shroom/FumeShroom_Main.png"),
    0f,
    15f,
    new Tuple<int,string>(75,"res://scene/Plants/Fume_Shroom/Fume_Shroom.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    21,
    "寒冰大喷菇",
    "伤害降低,攻速减慢",
    "大喷菇",
    GD.Load<Texture>("res://image/Plants/Ice_Fume_Shroom/Ice_FumeShroom_Main.png"),
    20f,
    45f,
    new Tuple<int,string>(225,"res://scene/Plants/Ice_Fume_Shroom/Ice_Fume_Shroom.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    22,
    "墓碑吞",
    "死亡生成土豆雷",
    "75",
    GD.Load<Texture>("res://image/Plants/Grave_Buster/Grave_Buster_Main.png"),
    10f,
    15f,
    new Tuple<int,string>(75,"res://scene/Plants/Grave_Buster/Grave_Buster.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    23,
    "小盆菇",
    "小喷菇+花盆",
    "小喷菇+花盆",
    GD.Load<Texture>("res://image/Plants/Small_Shroom_Pot/Small_Shroom_Pot.png"),
    5f,
    15f,
    new Tuple<int,string>(0,"res://scene/Plants/Small_Shroom_Pot/Small_Shroom_Pot.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    24,
    "球形盾",
    "简单护盾",
    "简单护盾",
    GD.Load<Texture>("res://image/Plants/PL_Casing/PL.png"),
    10f,
    20f,
    new Tuple<int,string>(50,"res://scene/Plants/PL_Casing/PL_Casing.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    25,
    "魅惑菇",
    "魅惑菇",
    "魅惑菇",
    GD.Load<Texture>("res://image/Plants/Hypno_Shroom/Hypno_Shroom.png"),
    10f,
    30f,
    new Tuple<int,string>(75,"res://scene/Plants/Hypno_Shroom/Hypno_Shroom.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    26,
    "胆小菇",
    "真的...要我...上场吗",
    "胆小菇",
    GD.Load<Texture>("res://image/Plants/Scaredy_Shroom/Scaredy_Shroom.png"),
    10f,
    15f,
    new Tuple<int,string>(25,"res://scene/Plants/Scaredy_Shroom/Scaredy_Shroom.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    27,
    "寒冰菇",
    "法师控制强,冰霜秒全场!",
    "法师控制强,冰霜秒全场!",
    GD.Load<Texture>("res://image/Plants/Ice_Shroom/IceShroom.png"),
    20f,
    50f,
    new Tuple<int,string>(75,"res://scene/Plants/Ice_Shroom/Ice_Shroom.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    28,
    "阳光寒冰菇",
    "考虑是否需要削弱",
    "考虑是否需要削弱",
    GD.Load<Texture>("res://image/Plants/Ice_Sun_Shroom/Ice_Sun_Shroom.png"),
    20f,
    50f,
    new Tuple<int,string>(175,"res://scene/Plants/Ice_Sun_Shroom/Ice_Sun_Shroom.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    29,
    "毁灭菇",
    "大范围1800伤害",
    "大范围1800伤害",
    GD.Load<Texture>("res://image/Plants/Doom_Shroom/DoomShroom.png"),
    15f,
    50f,
    new Tuple<int,string>(125,"res://scene/Plants/Doom_Shroom/Doom_Shroom.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    30,
    "莲叶",
    "莲叶",
    "莲叶",
    GD.Load<Texture>("res://image/Plants/Lotus/Lotus.png"),
    0f,
    15f,
    new Tuple<int,string>(25,"res://scene/Plants/Lotus/Lotus.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    31,
    "Na",
    "<!>危险化学品\nNa+H+->H2+Na+\nNa+R-OH->R-ONa+H2\nR-OH为醇类,如CH3CH2OH(C2H5OH)\nH2O在任何PH下都能电离出H+\nNa熔点97.8℃\nNa:别抢我电子了!",
    "Na:别抢我电子了!",
    GD.Load<Texture>("res://image/Plants/Na/Na.png"),
    0f,
    23f,
    new Tuple<int,string>(125,"res://scene/Plants/Na/Na.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    32,
    "电子pH计",
    "读取土壤或液体中的pH值\n双击顶部可破坏广口瓶",
    "读取土壤或液体中的pH值",
    GD.Load<Texture>("res://image/Plants/PH_Meter/PH.png"),
    14f,
    25f,
    new Tuple<int,string>(0,"res://scene/Plants/PH_Meter/PH_Meter.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    33,
    "水",
    "<!>化学品\n用于稀释",
    "用于稀释",
    GD.Load<Texture>("res://image/Plants/H2O/H2O.png"),
    0f,
    5f,
    new Tuple<int,string>(0,"res://scene/Plants/H2O/H2O.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    34,
    "硫磺悬浮液",
    "<!>化学品\n用于处理土壤碱化问题",
    "用于处理土壤碱化问题",
    GD.Load<Texture>("res://image/Plants/S_Emulsion/S_Emulsion.png"),
    0f,
    3f,
    new Tuple<int,string>(5,"res://scene/Plants/S_Emulsion/S_Emulsion.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    35,
    "石灰悬浮液",
    "<!>化学品\n用于处理土壤酸化问题",
    "用于处理土壤酸化问题",
    GD.Load<Texture>("res://image/Plants/CaO_Emulsion/CaO_Emulsion.png"),
    0f,
    3f,
    new Tuple<int,string>(5,"res://scene/Plants/CaO_Emulsion/CaO_Emulsion.tscn")
    )
    };
    static public List<Tuple<int, string, string, string, Texture, float, float, Tuple<int, string>>> Spec_Plants_list = new List<Tuple<int, string, string, string, Texture, float, float, Tuple<int, string>>>
    {
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    1,
    "随机种植",
    "随机种植",
    "随机种植",
    GD.Load<Texture>("res://image/Plants/Random/Random.png"),
    0f,
    1437f,
    new Tuple<int,string>(50,"res://scene/Plants/Random/Random.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    2,
    "弹坑",
    "弹坑",
    "弹坑",
    GD.Load<Texture>("res://image/Crater/Day1.png"),
    0f,
    0f,
    new Tuple<int,string>(100,"res://scene/Crater/Crater.tscn")
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
    "魅惑旗帜僵尸",
    "Godot",
    "第二个僵尸",
    GD.Load<Texture>("res://image/Plants/Zombies/Flag_Zombies/Flag_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(100,"res://scene/Plants/Zombies/Flag_Zombies/Plants_Flag_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    3,
    "魅惑路障僵尸",
    "路障",
    "第3个僵尸",
    GD.Load<Texture>("res://image/Plants/Zombies/Cone_Zombies/Plants_Cone_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(75,"res://scene/Plants/Zombies/Cone_Zombies/Plants_Cone_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    4,
    "魅惑快速路障僵尸",
    "单挑4个普僵",
    "普僵噩梦",
    GD.Load<Texture>("res://image/Plants/Zombies/Fast_Cone_Zombies/Fast_Cone_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(125,"res://scene/Plants/Zombies/Fast_Cone_Zombies/Plants_Fast_Cone_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    5,
    "魅惑忽略路障僵尸",
    "不知道有什么用",
    "不知道有什么用",
    GD.Load<Texture>("res://image/Plants/Zombies/Ignore_Cone_Zombies/Ignore_Cone_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(100,"res://scene/Plants/Zombies/Ignore_Zombies/Plants_Ignore_Cone_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    6,
    "魅惑铁桶僵尸",
    "铁桶是坚硬的",
    "装甲",
    GD.Load<Texture>("res://image/Plants/Zombies/Bucket_Zombies/Bucket_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(125,"res://scene/Plants/Zombies/Bucket_Zombies/Plants_Bucket_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    7,
    "魅惑撑杆跳僵尸",
    "起飞",
    "起飞",
    GD.Load<Texture>("res://image/Plants/Zombies/Polevaulter_Zombies/Polevaulter_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(125,"res://scene/Plants/Zombies/Polevaulter_Zombies/Plants_Polevaulter_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    8,
    "魅惑标枪撑杆跳僵尸",
    "一枪2000",
    "起飞",
    GD.Load<Texture>("res://image/Plants/Zombies/Darts_Polevaulter_Zombies/Darts_Polevaulter_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(225,"res://scene/Plants/Zombies/Darts_Polevaulter_Zombies/Plants_Darts_Polevaulter_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    9,
    "我方墓碑",
    "生产魅惑僵尸",
    "生产魅惑僵尸",
    GD.Load<Texture>("res://image/Plants/Zombies/Tomb/0.png"),
    0f,
    20f,
    new Tuple<int,string>(2000,"res://scene/Plants/Zombies/Tomb/Plants_Tomb.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    10,
    "魅惑铁门僵尸",
    "区区致命伤",
    "区区致命伤",
    GD.Load<Texture>("res://image/Plants/Zombies/Screen_Door_Zombies/Plants_Screen_Door_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(125,"res://scene/Plants/Zombies/Screen_Door_Zombies/Plants_Screen_Door_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    11,
    "魅惑火炬僵尸",
    "移动酒精灯",
    "(O^<)",
    GD.Load<Texture>("res://image/Plants/Zombies/Fire_Zombies/Fire_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(200,"res://scene/Plants/Zombies/Fire_Zombies/Plants_Fire_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    12,
    "我方氢气战神",
    "都是氢气战神",
    "H2+O2",
    GD.Load<Texture>("res://image/Plants/Zombies/H2_Maker_Zombies/H2_Maker_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(150,"res://scene/Plants/Zombies/H2_maker_Zombies/Plants_H2_maker_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    13,
    "魅惑三爷",
    "魅惑三爷",
    "魅惑三爷",
    GD.Load<Texture>("res://image/Plants/Zombies/Bucket_Screen_Door_Zombies/Plants_Bucket_Screen_Door_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(550,"res://scene/Plants/Zombies/Bucket_Screen_Door_Zombies/Plants_Bucket_Screen_Door_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    14,
    "魅惑标枪铁门僵尸",
    "WC挂!",
    "WC挂!",
    GD.Load<Texture>("res://image/Plants/Zombies/Dart_Screen_Door_Zombies/Dart_Screen_Door_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(750,"res://scene/Plants/Zombies/Darts_Screen_Door_Zombies/Plants_Darts_Screen_Door_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    15,
    "魅惑墓碑僵尸",
    "死亡生成墓碑",
    "死亡生成墓碑",
    GD.Load<Texture>("res://image/Plants/Zombies/Bucket_Tomb_Zombies/Plants_Bucket_Tomb_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(725,"res://scene/Plants/Zombies/Bucket_Tomb_Zombies/Plants_Bucket_Tomb_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    16,
    "魅惑二爷",
    "不再怜悯",
    "你二爷还是你二爷",
    GD.Load<Texture>("res://image/Plants/Zombies/Paper_Zombies/Plants_Paper_Zombies_Main.png"),
    0f,
    0f,
    new Tuple<int,string>(1000,"res://scene/Plants/Zombies/Paper_Zombies/Plants_Paper_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    17,
    "魅惑大爷",
    "PFA球员",
    "你大爷还是你大爷",
    GD.Load<Texture>("res://image/Plants/Zombies/FootBall_Zombies/Plants_FootBall_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(625,"res://scene/Plants/Zombies/FootBall_Zombies/Plants_FootBall_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    18,
    "魅惑舞王僵尸",
    "全场动作必须跟我整齐化一",
    "墓碑,起!",
    GD.Load<Texture>("res://image/Plants/Zombies/JackSon_Zombies/Zombie_jackson.png"),
    0f,
    0f,
    new Tuple<int,string>(525,"res://scene/Plants/Zombies/JackSon_Zombies/Plants_JackSon_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    19,
    "魅惑伴舞僵尸",
    "舞王粉丝应援团",
    "舞王粉丝应援团",
    GD.Load<Texture>("res://image/Plants/Zombies/Dancer_Zombies/Dancer_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(125,"res://scene/Plants/Zombies/Dancer_Zombies/Plants_Dancer_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    20,
    "魅惑篮球僵尸",
    "阻挡篮球僵尸",
    "阻挡篮球僵尸",
    GD.Load<Texture>("res://image/Plants/Zombies/Basketball_Zombies/Plants_BasketBall_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(325,"res://scene/Plants/Zombies/Basketball_Zombies/Plants_Basketball_Zombies.tscn")
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
    "旗帜僵尸",
    "闪电战+亡语",
    "第二个僵尸",
    GD.Load<Texture>("res://image/Zombies/Flag_Zombies/Flag_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(100,"res://scene/Zombies/Flag_Zombies/Flag_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    3,
    "路障僵尸",
    "路障",
    "第三个僵尸",
    GD.Load<Texture>("res://image/Zombies/Cone_Zombies/Cone_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(75,"res://scene/Zombies/Cone_Zombies/Cone_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    4,
    "快速路障僵尸",
    "兼具速度与血量",
    "完啦",
    GD.Load<Texture>("res://image/Zombies/Fast_Cone_Zombies/Fast_Cone_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(125,"res://scene/Zombies/Fast_Cone_Zombies/Fast_Cone_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    5,
    "忽略路障僵尸",
    "不吃植物",
    "完啦",
    GD.Load<Texture>("res://image/Zombies/Ignore_Cone_Zombies/Ignore_Cone_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(100,"res://scene/Zombies/Ignore_Cone_Zombies/Ignore_Cone_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    6,
    "铁桶僵尸",
    "铁桶",
    "血量多",
    GD.Load<Texture>("res://image/Zombies/Bucket_Zombies/Bucket_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(125,"res://scene/Zombies/Bucket_Zombies/Bucket_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    7,
    "撑杆跳僵尸",
    "起飞",
    "起飞",
    GD.Load<Texture>("res://image/Zombies/Polevaulter_Zombies/Polevaulter_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(125,"res://scene/Zombies/Polevaulter_Zombies/Polevaulter_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    8,
    "标枪撑杆跳僵尸",
    "一枪2000",
    "起飞",
    GD.Load<Texture>("res://image/Zombies/Darts_Polevaulter_Zombies/Darts_Polevaulter_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(225,"res://scene/Zombies/Darts_Polevaulter_Zombies/Darts_Polevaulter_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    9,
    "墓碑",
    "生产僵尸",
    "生产僵尸",
    GD.Load<Texture>("res://image/Zombies/Tomb/0.png"),
    0f,
    20f,
    new Tuple<int,string>(2000,"res://scene/Zombies/Tomb/Tomb.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    10,
    "铁门僵尸",
    "专防寒冰射手",
    "铁门僵尸",
    GD.Load<Texture>("res://image/Zombies/Screen_Door_Zombies/Screen_Door_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(125,"res://scene/Zombies/Screen_Door_Zombies/Screen_Door_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    11,
    "火炬僵尸",
    "防寒冰",
    "(O^<)",
    GD.Load<Texture>("res://image/Zombies/Fire_Zombies/Fire_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(200,"res://scene/Zombies/Fire_Zombies/Fire_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    12,
    "氢气战神",
    "致敬我班氢气战神",
    "H2+O2",
    GD.Load<Texture>("res://image/Zombies/H2_Maker_Zombies/H2_Maker_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(150,"res://scene/Zombies/H2_Maker_Zombies/H2_Maker_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    13,
    "三爷",
    "3170血量",
    "你三爷",
    GD.Load<Texture>("res://image/Zombies/Bucket_Screen_Door_Zombies/Bucket_Screen_Door_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(550,"res://scene/Zombies/Bucket_Screen_Door_Zombies/Bucket_Screen_Door_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    14,
    "标枪铁门僵尸",
    "破阵神器",
    "破阵神器",
    GD.Load<Texture>("res://image/Zombies/Darts_Screen_Door_Zombies/Dart_Screen_Door_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(750,"res://scene/Zombies/Darts_Screen_Door_Zombies/Darts_Screen_Door_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    15,
    "墓碑僵尸",
    "死亡生成墓碑",
    "死亡生成墓碑",
    GD.Load<Texture>("res://image/Zombies/Bucket_Tomb_Zombies/Bucket_Tomb_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(725,"res://scene/Zombies/Bucket_Tomb_Zombies/Bucket_Tomb_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    16,
    "二爷",
    "不再怜悯",
    "你二爷",
    GD.Load<Texture>("res://image/Zombies/Paper_Zombies/Paper_Zombies_Main.png"),
    0f,
    0f,
    new Tuple<int,string>(1000,"res://scene/Zombies/Paper_Zombies/Paper_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    17,
    "大爷",
    "现役ZFA球员",
    "现役ZFA球员",
    GD.Load<Texture>("res://image/Zombies/FootBall_Zombies/FootBall_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(625,"res://scene/Zombies/FootBall_Zombies/FootBall_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    18,
    "舞王僵尸",
    "全场动作必须跟我整齐化一",
    "墓碑,起!",
    GD.Load<Texture>("res://image/Zombies/JackSon_Zombies/Zombie_jackson.png"),
    0f,
    0f,
    new Tuple<int,string>(525,"res://scene/Zombies/JackSon_Zombies/JackSon_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    19,
    "伴舞僵尸",
    "舞王粉丝应援团",
    "舞王粉丝应援团",
    GD.Load<Texture>("res://image/Zombies/Dancer_Zombies/Dancer.png"),
    0f,
    0f,
    new Tuple<int,string>(125,"res://scene/Zombies/Dancer_Zombies/Dancer_Zombies.tscn")
    ),
    new Tuple<int,string,string,string,Texture,float,float,Tuple<int,string>>(
    20,
    "篮球僵尸",
    "ZBA球员",
    "Man!What can i say?",
    GD.Load<Texture>("res://image/Zombies/Basketball_Zombies/BasketBall_Zombies.png"),
    0f,
    0f,
    new Tuple<int,string>(325,"res://scene/Zombies/Basketball_Zombies/Basketball_Zombies.tscn")
    )
    };
    static public List<string> Zombies_Path_List = new List<string>
    {
        "res://scene/Zombies/Simple_Zombies/Simple_Zombies.tscn",
        "res://scene/Zombies/Flag_Zombies/Flag_Zombies.tscn",
        "res://scene/Zombies/Cone_Zombies/Cone_Zombies.tscn",
        "res://scene/Zombies/Fast_Cone_Zombies/Fast_Cone_Zombies.tscn",
        "res://scene/Zombies/Ignore_Cone_Zombies/Ignore_Cone_Zombies.tscn",//5
        "res://scene/Zombies/Bucket_Zombies/Bucket_Zombies.tscn",
        "res://scene/Zombies/Polevaulter_Zombies/Polevaulter_Zombies.tscn",
        "res://scene/Zombies/Darts_Polevaulter_Zombies/Darts_Polevaulter_Zombies.tscn",
        "res://scene/Zombies/Tomb/Tomb.tscn",//9
        "res://scene/Zombies/Screen_Door_Zombies/Screen_Door_Zombies.tscn",//10
        "res://scene/Zombies/Fire_Zombies/Fire_Zombies.tscn",
        "res://scene/Zombies/H2_Maker_Zombies/H2_Maker_Zombies.tscn",
        "res://scene/Zombies/Bucket_Screen_Door_Zombies/Bucket_Screen_Door_Zombies.tscn",
        "res://scene/Zombies/Darts_Screen_Door_Zombies/Darts_Screen_Door_Zombies.tscn",
        "res://scene/Zombies/Bucket_Tomb_Zombies/Bucket_Tomb_Zombies.tscn",//15
        "res://scene/Zombies/Paper_Zombies/Paper_Zombies.tscn",
        "res://scene/Zombies/FootBall_Zombies/FootBall_Zombies.tscn",
        "res://scene/Zombies/JackSon_Zombies/JackSon_Zombies.tscn",
        "res://scene/Zombies/Dancer_Zombies/Dancer_Zombies.tscn",
        "res://scene/Zombies/Basketball_Zombies/Basketball_Zombies.tscn"
    };
    static public List<Tuple<string, string, Texture, int>> Level_Mode1 = new List<Tuple<string, string, Texture, int>>
    { 
        new Tuple<string, string,Texture, int>("第1关","res://level/Mode1/Mode1_1.cfg",
        GD.Load<Texture>("res://image/Plants/SunFlower/sunflower_All.png"),
        1),
        new Tuple<string, string,Texture, int>("第2关:偷袭","res://level/Mode1/Mode1_2.cfg",
        GD.Load<Texture>("res://image/Plants/C2H5OH/C2H5OH.png"),
        1),
        new Tuple<string, string,Texture, int>("第3关:防线","res://level/Mode1/Mode1_3.cfg",
        GD.Load<Texture>("res://image/Plants/WallNut/Wallnut.png"),
        1),
        new Tuple<string, string,Texture, int>("第4关:保龄球","res://level/Mode1/Mode1_4.cfg",
        GD.Load<Texture>("res://image/Plants/WallNut/Boom_Wallnut.png"),
        1),
        new Tuple<string, string,Texture, int>("第5关:铁(桶)幕","res://level/Mode1/Mode1_5.cfg",
        GD.Load<Texture>("res://image/Plants/Potato/Potato.png"),
        1),
        new Tuple<string, string,Texture, int>("第6关:撑杆跳锦标赛","res://level/Mode1/Mode1_6.cfg",
        GD.Load<Texture>("res://image/Plants/Eating_Flower/Eating_Flower.png"),
        1),
        new Tuple<string, string,Texture, int>("第7关:撑杆跳锦标赛2","res://level/Mode1/Mode1_7.cfg",
        GD.Load<Texture>("res://image/Plants/Ice_Van_Door/Ice_Van_Door.png"),
        1),
        new Tuple<string, string,Texture, int>("第8关:重装出击","res://level/Mode1/Mode1_8.cfg",
        GD.Load<Texture>("res://image/Plants/Double_Van_Door/Double_Van_Door.png"),
        1),
        new Tuple<string, string,Texture, int>("第9关:传送带","res://level/Mode1/Mode1_9.cfg",
        GD.Load<Texture>("res://image/null/Null.png"),//(O^<)
        1),                                           // V V 
        new Tuple<string, string,Texture, int>("第10关:小喷菇之梦","res://level/Mode1/Mode1_10.cfg",
        GD.Load<Texture>("res://image/Plants/Small_Shroom/Small_Shroom.png"),
        2),
        new Tuple<string, string,Texture, int>("第11关:钢门批发","res://level/Mode1/Mode1_11.cfg",
        GD.Load<Texture>("res://image/Plants/Sun_Shroom/SunShroom_All.png"),
        2),
        new Tuple<string, string,Texture, int>("第12关:特刊","res://level/Mode1/Mode1_12.cfg",
        GD.Load<Texture>("res://image/Plants/H2/H2.png"),
        2),
        new Tuple<string, string,Texture, int>("第13关:总有一天","res://level/Mode1/Mode1_13.cfg",
        GD.Load<Texture>("res://image/Plants/Fume_Shroom/FumeShroom_Main.png"),
        2),
        new Tuple<string, string,Texture, int>("第14关:千坟山","res://level/Mode1/Mode1_14.cfg",
        GD.Load<Texture>("res://image/Plants/Grave_Buster/Grave_Buster_Main.png"),
        2),
        new Tuple<string, string,Texture, int>("第15关:二爷的复仇","res://level/Mode1/Mode1_15.cfg",
        GD.Load<Texture>("res://image/Hammer/hammer.png"),
        2),
        new Tuple<string, string,Texture, int>("第16关:将大局逆转吧","res://level/Mode1/Mode1_16.cfg",
        GD.Load<Texture>("res://image/Plants/Hypno_Shroom/Hypno_Shroom.png"),
        2),
        new Tuple<string, string,Texture, int>("第17关:人山人海","res://level/Mode1/Mode1_17.cfg",
        GD.Load<Texture>("res://image/Plants/Scaredy_Shroom/Scaredy_Shroom.png"),
        2),
        new Tuple<string, string,Texture, int>("第18关:群魔乱舞","res://level/Mode1/Mode1_18.cfg",
        GD.Load<Texture>("res://image/Plants/Ice_Shroom/IceShroom.png"),
        2),
        new Tuple<string, string,Texture, int>("第19关:爆炸的艺术","res://level/Mode1/Mode1_19.cfg",
        GD.Load<Texture>("res://image/Plants/Doom_Shroom/DoomShroom.png"),
        2),
        new Tuple<string, string,Texture, int>("第20关:强弩之末","res://level/Mode1/Mode1_20.cfg",
        GD.Load<Texture>("res://image/null/Null.png"),
        2),
        new Tuple<string, string,Texture, int>("第21关:涉水","res://level/Mode1/Mode1_21.cfg",
        GD.Load<Texture>("res://image/Plants/Lotus/Lotus.png"),
        3),
        new Tuple<string, string,Texture, int>("白天草坪(测试专用)","res://level/Mode1/Mode1_demo_Day.cfg",
        GD.Load<Texture>("res://image/Plants/godot/godot.png"),
        1),
        new Tuple<string, string,Texture, int>("夜晚草坪(测试专用)","res://level/Mode1/Mode1_demo_Night.cfg",
        GD.Load<Texture>("res://image/Plants/godot/godot.png"),
        2),
        new Tuple<string, string,Texture, int>("白天后院(测试专用)","res://level/Mode1/Mode1_demo_Day_Pool.cfg",
        GD.Load<Texture>("res://image/Plants/godot/godot.png"),
        3)
    };
    static public List<Tuple<string, string, Texture, int>> Level_Mode2 = new List<Tuple<string, string, Texture, int>>
    {
        new Tuple<string, string,Texture, int>("随机种植","res://level/Mode2/Mode2_1.cfg",
        GD.Load<Texture>("res://image/Plants/Random/Random.png"),
        2),
        new Tuple<string, string,Texture, int>("噩梦1","res://level/Mode2/Mode2_2.cfg",
        GD.Load<Texture>("res://image/Zombies/Bucket_Screen_Door_Zombies/Bucket_Screen_Door_Zombies.png"),
        2)
    };
    static public void Save_Value()
    {
        ConfigFile file = new ConfigFile();
        file.SetValue("Setting", "debug", Public_Main.debuging);
        file.SetValue("Setting", "Android", Public_Main.for_Android);
        file.SetValue("Setting", "Limit", Public_Main.Using_Clone_Limit);
        file.SetValue("Setting", "Limit_Number", Public_Main.Max_Object_Clone_In_F);
        file.SetValue("Setting", "Zombies_Health", Public_Main.Show_Zombies_Health);
        file.SetValue("Setting", "Plants_Health", Public_Main.Show_Plants_Health);
        file.SetValue("Setting", "Plants_Zombies_Health", Public_Main.Show_Plants_Zombies_Health);
        file.SetValue("Setting", "Mouse_Effect", Public_Main.Show_Mouse_Effect);
        file.Save("user://Users/" + Public_Main.user_name + "/Develop_Setting.cfg");
    }
}
public class Health_Container
{
    public int Health { get; set; }
    public bool Is_lock { get; set; }
    public string Info { get; set; }
    public bool Effect { get; set; }
    public string Material { get; set; }
    public Health_Container(int health, bool is_lock, string info = "Main", bool effect = false, string material = "Main")
    {
        Health = health;
        Is_lock = is_lock;
        Info = info;
        Effect = effect;
        Material = material;
    }
}
public class Health_List : List<Health_Container>
{ 
    public void Update_Health()
    {
        for (int i = 0; i < this.Count; i++)
        {
            if (this[i].Health < 0)
            {
                if (this[i].Is_lock)
                {
                    this[i].Health = 0;
                }
                else
                {
                    if (i + 1 < this.Count)
                    {
                        this[i + 1].Health += this[i].Health;
                        this[i].Health = 0;
                    }
                }
            }
            if (this[i].Health <= 0 && this.Count != 1)
            {
                this.RemoveAt(i);
                i--;
            }
        }
    }
    public int Get_Health()
    {
        Update_Health();
        int count_health = 0;
        for (int i = 0; i < this.Count; i++)
        {
            count_health += this[i].Health;
        }
        return count_health;
    }
    public bool Get_Has_Sound(bool Through)
    {
        Update_Health();
        bool result = false;
        if (Through && Get_Can_Through_To_Index() != -1) 
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Effect)
                {
                    result = true;
                    break;
                }
            }
        }
        else
        {
            result = this[0].Effect;
        }
        return result;
    }
    public int Get_Can_Through_To_Index()
    {
        Update_Health();
        int result = -1;
        for (int i = 0; i < this.Count; i++)
        {
            if (this[i].Info != "Door")
            {
                result = i;
                break;
            }
        }
        return result;
    }
    public bool Get_Can_Ignore_Ice_Bullets()
    {
        bool result = false;
        for (int i = 0; i < this.Count; i++)
        {
            if (this[i].Info == "Door")
            {
                result = true;
                break;
            }
        }
        return result;
    }
}
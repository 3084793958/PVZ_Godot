shader_type canvas_item;
uniform float r : hint_range(0.0, 1.0, 0.01) = 0.1;
uniform float DR : hint_range(0.0, 1.0, 0.01) = 0.1;
uniform float DR2 : hint_range(0.0, 1.0, 0.01) = 0.1;
uniform float DR3 : hint_range(0.0, 1.0, 0.01) = 0.1;
uniform float DR4 : hint_range(0.0, 1.0, 0.01) = 0.1;
uniform float DR5 : hint_range(0.0, 1.0, 0.01) = 0.1;
uniform vec4 Color : hint_color = vec4(1.0);
uniform float k = 20.0;
void fragment()
{
	vec2 uv = UV - vec2(0.5);
	if (distance(uv,vec2(0.0))>r && distance(uv,vec2(0.0))<r+DR)
	{
		COLOR.rgb=Color.rgb;
		COLOR.a=1.0-k*abs(distance(uv,vec2(0.0))-r-DR/2.0);
	}
	else if (distance(uv,vec2(0.0))>r+DR+DR2 && distance(uv,vec2(0.0))<r+DR+DR2+DR3)
	{
		COLOR.rgb=Color.rgb;
		COLOR.a=1.0-k*abs(distance(uv,vec2(0.0))-r-DR-DR2-DR3/2.0);
	}
	else if (distance(uv,vec2(0.0))>r+DR+DR2+DR3 && distance(uv,vec2(0.0))<r+DR+DR2+DR3+DR4)
	{
		COLOR.rgb=Color.rgb;
		COLOR.a=1.0-k*abs(distance(uv,vec2(0.0))-r-DR-DR2-DR3-DR4/2.0);
	}
	else
	{
		COLOR=vec4(0.0);
	}
	COLOR.a*=1.0-distance(uv,vec2(0.0))*2.0;
}
shader_type canvas_item;
uniform float value:hint_range(0.0, 1.0)=0.5;
uniform sampler2D noise_texture;
void fragment()
{
	vec4 texture_color=texture(TEXTURE,UV);
	vec4 noise_color=texture(noise_texture,UV);
	if (noise_color.r>value)
	{
		COLOR.a=0.0;
	}
	else
	{
		COLOR=texture_color;
	}
}
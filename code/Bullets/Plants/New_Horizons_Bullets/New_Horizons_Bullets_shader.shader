//https://godotshaders.com/shader/water-like-wavelet/
shader_type canvas_item;

uniform float progression : hint_range(0.0, 1.0) = 0.7;

uniform float fade : hint_range(0.0, 1.0) = 0.9;

uniform float thickness : hint_range(0.01, 1.0) = 0.2;

uniform float wavelet_factor : hint_range(.1, 4.) = 4.;

// In pixels.
uniform float deformation_length = 16;

uniform vec2 screen_pos;

uniform vec2 screen_size;

uniform vec4 tint : hint_color = vec4(.5, .5, .9, 1.);

const float PI = 3.1415926535897932384626433832795;
void vertex()
{
	UV.y/=1.3;
	UV.x/=1.2;
	UV.x+=0.06;
	UV.y+=0.115;
}
void fragment() {
	vec2 norm = normalize(UV - vec2(.5));
	float dist = distance(UV, vec2(.5)) * 2.;
	float prog = progression * (1. - thickness - 0.01);
	float distortion = clamp((dist - prog) / thickness, -1., 1.);
	distortion = mix(cos(distortion * (PI / wavelet_factor)), 0., step(0.99, abs(distortion)));
	vec2 def = distortion * deformation_length * SCREEN_PIXEL_SIZE;
	vec2 offset_vector = norm * def;
	vec2 target_id = screen_pos + ((UV + offset_vector) * screen_size);
	target_id.y = 1.0 - target_id.y;
	COLOR = mix(vec4(0.),
				vec4(texture(SCREEN_TEXTURE, target_id).rgb, mix(1.0, smoothstep(1., 0., (progression - fade) / (1. - fade)),
				step( fade, progression ))), step(0.01, distortion));
	if (UV.x<0.5)
	{
		COLOR=vec4(0.0);
	}
	else
	{
		COLOR.rgb*=1.5;
	}
}
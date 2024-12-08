//copy from https://godotshaders.com/shader/2d-water-with-reflections/
//changed
//倒影要求:对象Z_Index<this.Z_Index
shader_type canvas_item;

uniform float level : hint_range(0.0, 1.0) = 0.5; 
uniform vec4 water_albedo : hint_color = vec4(0.26, 0.23, 0.73, 1.0);
uniform float water_opacity : hint_range(0.0, 1.0) = 0.35;
uniform float water_speed = 0.05;
uniform float wave_distortion = 0.2;
uniform int wave_multiplyer = 7;
uniform bool water_texture_on = true;
uniform float reflection_X_offset = 0.0;
uniform float reflection_Y_offset = 0.0;
uniform sampler2D noise_texture : hint_albedo;
uniform sampler2D noise_texture2 : hint_albedo;

void fragment() {
	vec2 uv = UV;
	COLOR = vec4(0.0);
	
	if (uv.y >= level) {
		COLOR.a = 1.0;
		
		// distorted reflections
		vec2 water_uv = vec2(uv.x, uv.y * float(wave_multiplyer));
		float noise = texture(noise_texture, vec2(water_uv.x + TIME * water_speed, water_uv.y)).x * wave_distortion;
		noise -= (0.5 * wave_distortion);
		
		// water texture
		if (water_texture_on) {
			float water_texture_limit = 0.35;
			vec4 water_texture = texture(noise_texture2, uv * vec2(0.5, 4.0) + vec2(noise, 0.0));
			float water_texture_value = (water_texture.x < water_texture_limit) ? 1.0 : 0.0;	
			COLOR.xyz = vec3(water_texture_value);
		}
	
		// putting everything toghether 
		
		vec4 current_texture = texture(SCREEN_TEXTURE, vec2(SCREEN_UV.x + noise + reflection_X_offset, 1.0 - SCREEN_UV.y - (level - 0.5) * 2.0 + reflection_Y_offset));
		COLOR = mix(COLOR, current_texture, 1.0);
		COLOR = mix(COLOR, water_albedo, water_opacity);
	}
}
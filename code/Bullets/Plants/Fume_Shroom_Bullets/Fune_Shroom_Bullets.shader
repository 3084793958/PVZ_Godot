shader_type canvas_item;

uniform sampler2D Bullets_Texture;
uniform vec2 shader_rect = vec2(2140.0, 46.0);
uniform int Bullets_width = 32;
uniform float density_falloff = 0.5; // 密度衰减系数
uniform float time_frequency = 3.0;  // 随机变化频率
uniform float start_time;
// 伪随机函数
float rand(vec2 seed) {
    return fract(sin(dot(seed, vec2(12.9898,78.233))) * 43758.5453);
}

void fragment() {
    // 基础坐标计算
    vec2 base_uv = UV;
    vec2 emission_point = vec2(0.0, 0.5);
    
    // 计算到发射点的水平距离
    float distance = base_uv.x - emission_point.x;
    
    // 密度衰减计算（指数衰减）
    float density = exp(-distance * density_falloff);
    
    // 生成动态噪声图案
    vec2 noise_uv = base_uv * vec2(1.0, 0.2) + vec2((start_time) * 0.3, 0.0);
    float noise_pattern = rand(floor(noise_uv * 20.0 + vec2((start_time) * time_frequency)));
    
    // 多层孢子叠加
    vec4 final_color = vec4(0.0);
    for(int i = 0; i < 3; i++) {
        // 分层UV偏移
        float layer_offset = float(i) * 0.15;
        vec2 layer_uv = base_uv * vec2(1.0 + float(i)*0.2, 1.0) 
                       + vec2(layer_offset, sin((start_time)*3.0 + float(i)*10.0) * 0.1);
        
        // 密度控制
        float layer_density = density * (1.0 - float(i)*0.3);
        
        // 孢子采样
        vec4 spores = texture(Bullets_Texture, 
                            fract(layer_uv * vec2(shader_rect.x/float(Bullets_width), 3.0)));
		
		// 动态透明度控制
        float alpha = smoothstep(0.3, 0.6, density) * 
                     (1.0 - smoothstep(0.0, 0.8, fract(layer_uv.x * -0.2 + (start_time)*0.5)));
        // 距离衰减
        alpha *= 1.0 - smoothstep(0.0, 0.8, distance/3.0);
		spores.a=alpha;
		spores.rgb *= 1.0 + float(i)*0.1; // 越靠前的层越亮
        // 叠加混合
        final_color = mix(final_color, spores, spores.a);
    }
    // 纵向渐变
    float vertical_fade = smoothstep(0.3, 0.5, abs(base_uv.y - 0.5));
    final_color.a *= 1.0 - vertical_fade;
    COLOR = final_color;
}
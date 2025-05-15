float heightmapValue(vec2 uv,
	sampler2D noise_curve_texture,
	sampler2D contribution_curve_texture,
	sampler2D noise_texture,
	float scale,
	float power) 
{
	float value = texture(noise_texture, uv).r;
	float value_raised_to_power = pow(value, power);
	
	float value_on_curve = texture(noise_curve_texture, vec2(value_raised_to_power, 0.0)).r;
	float value_on_contribution_curve = texture(contribution_curve_texture, vec2(value, 0.0)).r;
	
	float final_value = value_on_curve * value_on_contribution_curve;
	
	return final_value;
}

vec4 heightmapVisualization(float value, float sea_level_value)
{
	if (value <= sea_level_value) {
		return vec4(value / 2.0, value / 2.0, value, 1.0);
	} else {
		return vec4(value, value, value, 1.0);
	}
}
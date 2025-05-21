float heightmapValue(vec2 uv_argument,
	sampler2D noise_curve_texture_argument,
	sampler2D contribution_curve_texture_argument,
	sampler2D noise_texture_argument,
	float scale_argument,
	float power_argument) 
{
	float value = texture(noise_texture_argument, uv_argument).r;
	float value_raised_to_power = pow(value, power_argument);
	
	float value_on_curve = texture(noise_curve_texture_argument, vec2(value_raised_to_power, 0.0)).r;
	float value_on_contribution_curve = texture(contribution_curve_texture_argument, vec2(value, 0.0)).r;
	
	float final_value = value_on_curve * value_on_contribution_curve;
	
	return final_value;
}

vec4 heightmapVisualization(float value_argument, float sea_level_value_argument)
{
	if (value_argument <= sea_level_value_argument) {
		return vec4(value_argument / 2.0, value_argument / 2.0, value_argument, 1.0);
	} else {
		return vec4(value_argument, value_argument, value_argument, 1.0);
	}
}

vec4 heightmapShader(vec2 uv_argument,
	sampler2D noise_curve_texture_argument,
	sampler2D contribution_curve_texture_argument,
	sampler2D noise_texture_argument,
	float scale_argument,
	float power_argument,
	float sea_level_value_argument) 
{
	return heightmapVisualization(heightmapValue(uv_argument, noise_curve_texture_argument, contribution_curve_texture_argument, noise_texture_argument, scale_argument, power_argument), sea_level_value_argument);
}
#include "res://Source/ShaderLibraries/heightmap.gdshaderinc"

float continentalnessFinalHeightmapValue(vec2 uv,

	// Continentalness
	sampler2D continentalness_noise_curve_texture,
	sampler2D continentalness_contribution_curve_texture,
	sampler2D continentalness_noise_texture,
	float continentalness_scale,
	float continentalness_power,
	
	// Continentalness2
	sampler2D continentalness2_noise_curve_texture,
	sampler2D continentalness2_contribution_curve_texture,
	sampler2D continentalness2_noise_texture,
	float continentalness2_scale,
	float continentalness2_power,
	
	// Continentalness Picker
	sampler2D continentalness_picker_noise_curve_texture,
	sampler2D continentalness_picker_contribution_curve_texture,
	sampler2D continentalness_picker_noise_texture,
	float continentalness_picker_scale,
	float continentalness_picker_power,
	
	// Continentalness Final
	sampler2D continentalness_final_noise_curve_texture,
	sampler2D continentalness_final_contribution_curve_texture,
	float continentalness_final_scale,
	float continentalness_final_power
	) 
{
	float continentalness_value = heightmapValue(
		uv,
		continentalness_noise_curve_texture,
		continentalness_contribution_curve_texture, 
		continentalness_noise_texture, 
		continentalness_scale, 
		continentalness_power
	);
	
	float continentalness2_value = heightmapValue(
		uv,
		continentalness2_noise_curve_texture,
		continentalness2_contribution_curve_texture, 
		continentalness2_noise_texture, 
		continentalness2_scale, 
		continentalness2_power
	);
	
	float continentalness_picker_value = heightmapValue(
		uv,
		continentalness_picker_noise_curve_texture,
		continentalness_picker_contribution_curve_texture, 
		continentalness_picker_noise_texture, 
		continentalness_picker_scale, 
		continentalness_picker_power
	);
	
	float continentalness_final_value = mix(continentalness_value, continentalness2_value, continentalness_picker_value);
	float continentalness_final_value_raised_to_power = pow(continentalness_final_value, continentalness_final_power);
	
	float continentalness_final_value_on_curve = texture(continentalness_final_noise_curve_texture, vec2(continentalness_final_value_raised_to_power, 0.0)).r;
	float continentalness_final_value_on_contribution_curve = texture(continentalness_final_contribution_curve_texture, vec2(continentalness_final_value, 0.0)).r;
	
	float continentalness_final_final_value = continentalness_final_value_on_curve * continentalness_final_value_on_contribution_curve;
	
	return continentalness_final_final_value;
}

vec4 continentalnessFinalHeightmapVisualization(float value, float sea_level_value)
{
	if (value <= sea_level_value) {
		return vec4(value / 2.0, value / 2.0, value, 1.0);
	} else {
		return vec4(value, value, value, 1.0);
	}
}

vec4 continentalnessFinalHeightmapShader(vec2 uv,

	// Continentalness
	sampler2D continentalness_noise_curve_texture,
	sampler2D continentalness_contribution_curve_texture,
	sampler2D continentalness_noise_texture,
	float continentalness_scale,
	float continentalness_power,
	
	// Continentalness2
	sampler2D continentalness2_noise_curve_texture,
	sampler2D continentalness2_contribution_curve_texture,
	sampler2D continentalness2_noise_texture,
	float continentalness2_scale,
	float continentalness2_power,
	
	// Continentalness Picker
	sampler2D continentalness_picker_noise_curve_texture,
	sampler2D continentalness_picker_contribution_curve_texture,
	sampler2D continentalness_picker_noise_texture,
	float continentalness_picker_scale,
	float continentalness_picker_power,
	
	// Continentalness Final
	sampler2D continentalness_final_noise_curve_texture,
	sampler2D continentalness_final_contribution_curve_texture,
	float continentalness_final_scale,
	float continentalness_final_power,
	
	float sea_level_value
	) 
{
	return continentalnessFinalHeightmapVisualization(continentalnessFinalHeightmapValue(
		uv,

		// Continentalness
		continentalness_noise_curve_texture,
		continentalness_contribution_curve_texture,
		continentalness_noise_texture,
		continentalness_scale,
		continentalness_power,
		
		// Continentalness2
		continentalness2_noise_curve_texture,
		continentalness2_contribution_curve_texture,
		continentalness2_noise_texture,
		continentalness2_scale,
		continentalness2_power,
		
		// Continentalness Picker
		continentalness_picker_noise_curve_texture,
		continentalness_picker_contribution_curve_texture,
		continentalness_picker_noise_texture,
		continentalness_picker_scale,
		continentalness_picker_power,
		
		// Continentalness Final
		continentalness_final_noise_curve_texture,
		continentalness_final_contribution_curve_texture,
		continentalness_final_scale,
		continentalness_final_power
		), sea_level_value);
}
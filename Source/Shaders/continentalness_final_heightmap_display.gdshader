shader_type canvas_item;

#include "res://Source/ShaderLibraries/uv_rotation.gdshaderinc"
//#include "res://Source/ShaderLibraries/heightmap.gdshaderinc"
#include "res://Source/ShaderLibraries/continentalness_final.gdshaderinc"

// Continentalness
uniform sampler2D continentalness_noise_curve_texture;
uniform sampler2D continentalness_contribution_curve_texture;
uniform sampler2D continentalness_noise_texture;
uniform float continentalness_scale = 1.0;
uniform float continentalness_power = 1.0;

// Continentalness2
uniform sampler2D continentalness2_noise_curve_texture;
uniform sampler2D continentalness2_contribution_curve_texture;
uniform sampler2D continentalness2_noise_texture;
uniform float continentalness2_scale = 1.0;
uniform float continentalness2_power = 1.0;

// Continentalness Picker
uniform sampler2D continentalness_picker_noise_curve_texture;
uniform sampler2D continentalness_picker_contribution_curve_texture;
uniform sampler2D continentalness_picker_noise_texture;
uniform float continentalness_picker_scale = 1.0;
uniform float continentalness_picker_power = 1.0;

// Continentalness Final

uniform sampler2D continentalness_final_noise_curve_texture;
uniform sampler2D continentalness_final_contribution_curve_texture;
uniform float continentalness_final_scale = 1.0;
uniform float continentalness_final_power = 1.0;

uniform float continentalness_final_sea_level_value = 0.5;
uniform float rotation = 0;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	//vec2 rotated_uv = rotateUV(zoomUV(UV, 0.7), rotation);
	vec2 rotated_uv = rotateUV(UV, rotation);
	COLOR = continentalnessFinalHeightmapShader(
		rotated_uv,

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
		continentalness_final_power,
		continentalness_final_sea_level_value
	);
}

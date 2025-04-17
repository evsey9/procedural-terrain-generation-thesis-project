shader_type canvas_item;

#include "res://Source/ShaderLibraries/uv_rotation.gdshaderinc"
#include "res://Source/ShaderLibraries/zoom.gdshaderinc"

uniform sampler2D noise_curve_texture;
uniform sampler2D contribution_curve_texture;
uniform sampler2D noise_texture;
uniform float scale = 1.0;
uniform float power = 1.0;
uniform float sea_level_value = 0.5;
uniform float rotation = 0;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	//vec2 rotated_uv = rotateUV(zoomUV(UV, 0.7), rotation);
	vec2 rotated_uv = rotateUV(UV, rotation);
	float value = texture(noise_texture, rotated_uv).r;
	float value_raised_to_power = pow(value, power);
	
	float value_on_curve = texture(noise_curve_texture, vec2(value_raised_to_power, 0.0)).r;
	float value_on_contribution_curve = texture(contribution_curve_texture, vec2(value, 0.0)).r;
	
	float final_value = value_on_curve * value_on_contribution_curve;
	
	if (final_value <= sea_level_value) {
		COLOR = vec4(final_value / 2.0, final_value / 2.0, final_value, 1.0);
	} else {
		COLOR = vec4(final_value, final_value, final_value, 1.0);
	}
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}

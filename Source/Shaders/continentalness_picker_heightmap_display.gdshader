shader_type canvas_item;

#include "res://Source/ShaderLibraries/uv_rotation.gdshaderinc"
#include "res://Source/ShaderLibraries/heightmap.gdshaderinc"

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
	COLOR = heightmapShader(rotated_uv, noise_curve_texture, contribution_curve_texture, noise_texture, scale, power, sea_level_value);
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}

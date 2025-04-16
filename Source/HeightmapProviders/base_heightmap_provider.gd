class_name HeightmapProvider
extends Node

var default_noise: FastNoiseLite
var default_curve : Curve
var default_contribution_curve : Curve
var default_scale : float = 1
var default_power : float = 1

var noise : FastNoiseLite
var curve : Curve
var contribution_curve : Curve
var scale : float = 1
var power : float = 1

var preview_curve : Curve
var preview_contribution_curve : Curve

func _init():
	reset()

func get_value_at(x: float, z: float) -> float:
	var noise_value = 0.5 + 0.5 * noise.get_noise_2d(x * scale, z * scale)
	return curve.sample_baked(noise_value ** power) * contribution_curve.sample_baked(noise_value)

func get_height_at(x: float, z: float) -> int:
	return int(get_value_at(x, z) * WorldGenerationSettings.heightmap_range) + WorldGenerationSettings.heightmap_min_y

func get_image():
	var new_noise : FastNoiseLite = noise.duplicate(true)
	new_noise.frequency *= scale
	new_noise.frequency *= 5
	var a : Image = new_noise.get_image(512, 512)
	var b = ImageFunctions.convert_to_matrix(a)
	b = ImageFunctions.matrix_apply(b, func(x): return preview_curve.sample(x ** power) * preview_contribution_curve.sample(x))
	return ImageFunctions.matrix_to_image(b)

func reset():
	noise = default_noise.duplicate()
	curve = default_curve.duplicate()
	contribution_curve = default_contribution_curve.duplicate()
	scale = default_scale
	power = default_power
	curve.bake()
	contribution_curve.bake()
	
	preview_curve = default_curve.duplicate()
	preview_contribution_curve = default_contribution_curve.duplicate()

func bake_curves():
	curve = preview_curve.duplicate()
	contribution_curve = preview_contribution_curve.duplicate()
	curve.bake()
	contribution_curve.bake()

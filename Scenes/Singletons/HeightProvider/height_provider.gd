extends Node

#const ContinentalnessHeightmapCurve : Curve = preload("res://Assets/Curves/NoSpline.tres")
const ContinentalnessHeightmapCurve : Curve = preload("res://Assets/Curves/Continentalness.tres")
const ErosionHeightmapCurve : Curve = preload("res://Assets/Curves/Erosion.tres")
const PeaksAndValleysHeightmapCurve : Curve = preload("res://Assets/Curves/PeaksAndValleys.tres")

const _moore_dirs = [
	Vector3(-1, 0, -1),
	Vector3(0, 0, -1),
	Vector3(1, 0, -1),
	Vector3(-1, 0, 0),
	Vector3(1, 0, 0),
	Vector3(-1, 0, 1),
	Vector3(0, 0, 1),
	Vector3(1, 0, 1)
]

var sea_level_y = 64
var heightmap_min_y := 0
var heightmap_max_y := 128
var heightmap_range := 0
#var heightmap_noise : ZN_FastNoiseLite = preload("res://Assets/NoiseMaps/ContinentalnessLiteNoWarp.tres")
var heightmap_noise : ZN_FastNoiseLite = preload("res://Assets/NoiseMaps/ContinentalnessLite.tres")
#var heightmap_noise : ZN_FastNoiseLite = preload("res://Assets/NoiseMaps/ContinentalnessLiteNoFractal.tres")
var trees_min_y := 0
var trees_max_y := 0

func _init():
	# TODO Even this must be based on a seed, but I'm lazy
	
	# IMPORTANT
	# If we don't do this `Curve` could bake itself when interpolated,
	# and this causes crashes when used in multiple threads
	ContinentalnessHeightmapCurve.bake()
	ErosionHeightmapCurve.bake()
	PeaksAndValleysHeightmapCurve.bake()


func get_height_at(x: int, z: int) -> int:
	heightmap_range = heightmap_max_y - heightmap_min_y
	var t = 0.5 + 0.5 * heightmap_noise.get_noise_2d(x, z)
	return int(ContinentalnessHeightmapCurve.sample_baked(t) * heightmap_range) + heightmap_min_y

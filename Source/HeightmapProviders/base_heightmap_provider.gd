class_name HeightmapProvider
extends Node

var default_noise: FastNoiseLite
var default_curve : Curve
var default_contribution_curve : Curve
var default_scale : float = 1
var default_power : float = 1

var noise : FastNoiseLite
var display_noise : FastNoiseLite
var curve : Curve
var contribution_curve : Curve
var frequency : float = 1
var scale : float = 1
var power : float = 1

var default_display_noise_scale : float = 5.0
var display_noise_scale : float = default_display_noise_scale
var display_noise_scale_zoom_factor : float = 1.0
var _actual_display_noise_scale : float :
	get:
		return display_noise_scale / display_noise_scale_zoom_factor

var default_noise_texture_width : float = 256.0 / 1
var default_noise_texture_height : float = 256.0 / 1

var preview_curve : Curve
var preview_contribution_curve : Curve

var noise_texture : NoiseTexture2D = NoiseTexture2D.new()

var curve_texture : CurveTexture = CurveTexture.new()
var contribution_curve_texture : CurveTexture = CurveTexture.new()

var player : Node3D

func _init():
	noise_texture.generate_mipmaps = true
	noise_texture.normalize = false
	curve_texture.texture_mode = CurveTexture.TEXTURE_MODE_RED
	contribution_curve_texture.texture_mode = CurveTexture.TEXTURE_MODE_RED
	reset()

func _ready():
	player = get_tree().get_first_node_in_group("player")

func _process(delta: float) -> void:
	if player == null:
		player = get_tree().get_first_node_in_group("player")
	else:
		if display_noise != null:
			display_noise.offset = Vector3(player.position.x, player.position.z, 0) / _actual_display_noise_scale - Vector3(default_noise_texture_width / 2, default_noise_texture_height / 2, 0)
			#display_noise.offset = Vector3()

func get_value_at(x: float, z: float) -> float:
	#var noise_value = 0.5 + 0.5 * noise.get_noise_2d(x * scale, z * scale)
	var noise_value = 0.5 + 0.5 * noise.get_noise_2d(x, z)
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
	
	frequency = noise.frequency
	noise.frequency = scale * frequency
	
	display_noise = noise.duplicate()
	display_noise.offset = Vector3(default_noise_texture_width / 2, default_noise_texture_height / 2, 0)
	
	noise_texture.noise = display_noise
	update_noise_texture()
	
	preview_curve = default_curve.duplicate()
	preview_contribution_curve = default_contribution_curve.duplicate()
	
	curve_texture.curve = preview_curve
	contribution_curve_texture.curve = preview_contribution_curve

func bake_curves():
	curve = preview_curve.duplicate()
	contribution_curve = preview_contribution_curve.duplicate()
	
	curve.bake()
	contribution_curve.bake()

func update_noise_texture():
	noise_texture.width = default_noise_texture_width * 1
	noise_texture.height = default_noise_texture_height * 1
	noise.frequency = frequency * scale
	display_noise.frequency = frequency * scale * _actual_display_noise_scale
	display_noise.fractal_gain = noise.fractal_gain
	display_noise.fractal_lacunarity = noise.fractal_lacunarity
	display_noise.fractal_octaves = noise.fractal_octaves
	display_noise.fractal_weighted_strength = noise.fractal_weighted_strength
	

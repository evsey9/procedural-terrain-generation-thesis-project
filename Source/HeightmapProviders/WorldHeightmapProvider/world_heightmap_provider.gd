extends HeightmapProviderOld

func _init():
	default_noise = null
	default_curve = null
	default_contribution_curve = null
	default_scale = 1.0
	default_power = 1.0

func get_height_at(x: float, z: float) -> int:
	return ContinentalnessHeightmapProvider.get_height_at(x * WorldGenerationSettings.horizontal_scale, z * WorldGenerationSettings.horizontal_scale)

func get_image():
	var new_noise : FastNoiseLite = ContinentalnessHeightmapProvider.noise.duplicate(true)
	new_noise.frequency *= ContinentalnessHeightmapProvider.scale
	new_noise.frequency *= WorldGenerationSettings.horizontal_scale
	new_noise.frequency *= 5
	var a : Image = new_noise.get_image(512, 512)
	var b = ImageFunctions.convert_to_matrix(a)
	b = ImageFunctions.matrix_apply(b, func(x): return ContinentalnessHeightmapProvider.preview_curve.sample(x ** ContinentalnessHeightmapProvider.power) * ContinentalnessHeightmapProvider.preview_contribution_curve.sample(x))
	return ImageFunctions.matrix_to_image(b)

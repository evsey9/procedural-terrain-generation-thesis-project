extends HeightmapProvider

func _init():
	default_noise = preload("res://Source/HeightmapProviders/ContinentalnessHeightmapProvider/continentalness_noise.tres")
	default_curve = preload("res://Source/HeightmapProviders/ContinentalnessHeightmapProvider/continentalness_curve.tres")
	default_contribution_curve = preload("res://Source/HeightmapProviders/ContinentalnessHeightmapProvider/continentalness_contribution_curve_basic.tres")
	default_scale = 1.0
	default_power = 1.5
	super()

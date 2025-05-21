extends HeightmapProviderDisplay

func _init() -> void:
	heightmap_provider = ContinentalnessPickerHeightmapProvider
	super()

func _ready() -> void:
	super()

func update_shader() -> void:
	super()
	noise_texture_rect.material.set_shader_parameter("sea_level_value", 0)
	

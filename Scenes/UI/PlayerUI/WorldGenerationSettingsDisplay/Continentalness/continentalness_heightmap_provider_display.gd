extends HeightmapProviderDisplay

func _init() -> void:
	heightmap_provider = ContinentalnessHeightmapProvider

func _ready() -> void:
	super()

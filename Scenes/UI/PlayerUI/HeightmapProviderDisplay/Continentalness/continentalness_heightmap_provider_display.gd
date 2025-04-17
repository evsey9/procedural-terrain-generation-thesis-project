extends HeightmapProviderDisplay

func _init() -> void:
	heightmap_provider = ContinentalnessHeightmapProvider
	super()

func _ready() -> void:
	super()

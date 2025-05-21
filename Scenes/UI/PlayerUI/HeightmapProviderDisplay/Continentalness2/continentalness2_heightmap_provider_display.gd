extends HeightmapProviderDisplay

func _init() -> void:
	heightmap_provider = Continentalness2HeightmapProvider
	super()

func _ready() -> void:
	super()

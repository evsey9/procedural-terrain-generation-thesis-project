extends "res://addons/boujie_water_shader/prefabs/ocean_prefab.gd"

func _process(delta: float) -> void:
	super(delta)
	#position.x = get_tree().get_first_node_in_group("player").global_position.x
	#position.z = get_tree().get_first_node_in_group("player").global_position.z
	position.y = WorldGenerationSettingsProvider.GetWorldGenerationSettings(self).SeaLevelY
	

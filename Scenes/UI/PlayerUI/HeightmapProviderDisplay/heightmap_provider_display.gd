class_name HeightmapProviderDisplay
extends Control

var heightmap_provider
@onready var noise_texture_rect : TextureRect = $NoiseTextureControl/NoiseTextureRect
@onready var properties_box : PropertiesBox = $ScaleSliderControl/VBoxContainer/PropertiesBox
@onready var curve_editor : CurveEditor = $CurveControl/HBoxContainer/VBoxContainer/FirstCurveControl/VBoxContainer/CurveEditor
@onready var contribution_curve_editor : CurveEditor = $CurveControl/HBoxContainer/VBoxContainer/SecondCurveControl/VBoxContainer/CurveEditor
var curves_baked := true
signal noise_updated

var player : Node3D


func _init() -> void:
	pass

func _ready() -> void:
	setup_properties()
	setup_curves()
	setup_noise_texture()
	player = get_tree().get_first_node_in_group("player")
	WorldGenerationSettingsProvider.Changed.connect(world_generation_settings_changed)
	WorldGenerationSettingsProvider.Reloaded.connect(update_all_info)

func _process(delta: float) -> void:
	if player == null:
		player = get_tree().get_first_node_in_group("player")
	else:
		noise_texture_rect.material.set_shader_parameter("rotation", player.rotation.y)
		pass
		#display_noise.offset = Vector3()

func setup_properties() -> void:
	properties_box.clear()
	properties_box.add_group("Base")
	properties_box.add_float("Scale", heightmap_provider.HeightmapSettings.Scale, 0.00001, 16, 0.01)
	properties_box.add_float("Period", 1 / heightmap_provider.HeightmapSettings.Noise.frequency, 0.1, 20000, 0.1)
	properties_box.end_group()
	properties_box.add_spacer(false)
	properties_box.add_group("Fractal")
	properties_box.add_int("Fractal Octaves", heightmap_provider.HeightmapSettings.Noise.fractal_octaves, 1, 10)
	properties_box.add_float("Fractal Lacunarity", heightmap_provider.HeightmapSettings.Noise.fractal_lacunarity, 0, 5, 0.01)
	properties_box.add_float("Fractal Gain", heightmap_provider.HeightmapSettings.Noise.fractal_gain, 0, 1, 0.01)
	properties_box.end_group()
	properties_box.add_spacer(false)
	properties_box.add_group("Additional")
	properties_box.add_float("Power", heightmap_provider.HeightmapSettings.Power, 0.00001, 10, 0.01)
	properties_box.end_group()
	properties_box.add_float("Preview Zoom", heightmap_provider.HeightmapSettings.DisplayNoiseScale, 0.1, 100, 0.1)

func setup_curves() -> void:
	curve_editor.set_curve(heightmap_provider.HeightmapSettings.PreviewCurve)
	contribution_curve_editor.set_curve(heightmap_provider.HeightmapSettings.PreviewContributionCurve)
	$CurveControl/HBoxContainer/RotatedLabelNode/CurvesUnbakedWarning.hide()

func setup_noise_texture():
	noise_texture_rect.material.set_shader_parameter("noise_texture", heightmap_provider.HeightmapSettings.NoiseTexture)
	noise_texture_rect.material.set_shader_parameter("noise_curve_texture", heightmap_provider.HeightmapSettings.CurveTexture)
	noise_texture_rect.material.set_shader_parameter("contribution_curve_texture", heightmap_provider.HeightmapSettings.ContributionCurveTexture)
	update_shader()

func _on_player_ui_enabled() -> void:
	pass


func _on_player_ui_disabled() -> void:
	pass # Replace with function body.


func _on_properties_box_value_changed(key: StringName, new_value: Variant) -> void:
	#get_new_noise_texture()
	pass

func _on_properties_box_number_changed(key: StringName, new_value: float) -> void:
	match key:
		"Scale":
			heightmap_provider.HeightmapSettings.Scale = new_value
		"Period":
			heightmap_provider.HeightmapSettings.Frequency = 1 / new_value
		"Fractal Octaves":
			heightmap_provider.HeightmapSettings.Noise.fractal_octaves = int(new_value)
		"Fractal Lacunarity":
			heightmap_provider.HeightmapSettings.Noise.fractal_lacunarity = new_value
		"Fractal Gain":
			heightmap_provider.HeightmapSettings.Noise.fractal_gain = new_value
		"Power":
			heightmap_provider.HeightmapSettings.Power = new_value
		"Preview Zoom":
			heightmap_provider.HeightmapSettings.DisplayNoiseScale = new_value
		_:
			pass
	if key != "Preview Zoom":
		properties_changed()
	else:
		update_shader()


func update_shader():
	heightmap_provider.HeightmapSettings.UpdateNoiseTexture()
	noise_texture_rect.material.set_shader_parameter("scale", heightmap_provider.HeightmapSettings.Scale)
	noise_texture_rect.material.set_shader_parameter("power", heightmap_provider.HeightmapSettings.Power)
	noise_texture_rect.material.set_shader_parameter("sea_level_value", WorldGenerationSettingsProvider.Resource.WorldGenerationSettings.SeaLevelRatio)


func _on_reset_button_pressed() -> void:
	reset()
	#print()

func reset():
	heightmap_provider.Reset()
	#update_all_info()

func update_all_info():
	setup_properties()
	properties_changed()
	setup_curves()
	setup_noise_texture()

func _on_curve_editor_curve_changed(curve: Curve) -> void:
	properties_changed()
	#$CurveControl/HBoxContainer/RotatedLabelNode/CurvesUnbakedWarning.show()


func _on_contribution_curve_editor_curve_changed(curve: Curve) -> void:
	properties_changed()
	#$CurveControl/HBoxContainer/RotatedLabelNode/CurvesUnbakedWarning.show()


func _on_bake_button_pressed() -> void:
	heightmap_provider.HeightmapSettings.BakeCurves()
	#$CurveControl/HBoxContainer/RotatedLabelNode/CurvesUnbakedWarning.hide()

func properties_changed() -> void:
	#update_shader()
	heightmap_provider.HeightmapSettings.ChangeSettings()
	#get_tree().get_first_node_in_group("world_generation_settings_display").children_heightmap_properties_changed()

func world_generation_settings_changed():
	update_shader()

func disable_reset_button() -> void:
	$ScaleSliderControl/VBoxContainer/HBoxContainer/ResetButton.enabled = false

func enable_reset_button() -> void:
	$ScaleSliderControl/VBoxContainer/HBoxContainer/ResetButton.enabled = true

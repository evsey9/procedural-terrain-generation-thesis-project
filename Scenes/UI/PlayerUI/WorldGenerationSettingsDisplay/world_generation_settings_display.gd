class_name WorldGenerationSettingsDisplay
extends Control

@onready var noise_texture_rect : TextureRect = $NoiseTextureRect
@onready var properties_box : PropertiesBox = $ScaleSliderControl/VBoxContainer/PropertiesBox
@onready var curve_editor : CurveEditor = $CurveControl/HBoxContainer/VBoxContainer/FirstCurveControl/VBoxContainer/CurveEditor
@onready var contribution_curve_editor : CurveEditor = $CurveControl/HBoxContainer/VBoxContainer/SecondCurveControl/VBoxContainer/CurveEditor
var curves_baked := true

func _ready() -> void:
	setup_properties()
	get_new_noise_texture()
	setup_curves()

func setup_properties() -> void:
	properties_box.clear()
	properties_box.add_group("Base")
	properties_box.add_float("Horizontal Scale", WorldGenerationSettings.horizontal_scale, 0.00001, 10, 0.01)
	properties_box.add_float("Sea Level Height", WorldGenerationSettings.sea_level_y, -512, 512, 16)
	properties_box.add_int("Heightmap Min Level", WorldGenerationSettings.heightmap_min_y, -512, 512)
	properties_box.add_int("Heightmap Max Level", WorldGenerationSettings.heightmap_max_y, -512, 512)
	properties_box.end_group()

func setup_curves() -> void:
	curve_editor.set_curve(ContinentalnessHeightmapProvider.preview_curve)
	contribution_curve_editor.set_curve(ContinentalnessHeightmapProvider.preview_contribution_curve)
	$CurveControl/HBoxContainer/RotatedLabelNode/CurvesUnbakedWarning.hide()

func _on_player_ui_enabled() -> void:
	pass


func _on_player_ui_disabled() -> void:
	pass # Replace with function body.


func _on_properties_box_value_changed(key: StringName, new_value: Variant) -> void:
	#get_new_noise_texture()
	pass

func _on_properties_box_number_changed(key: StringName, new_value: float) -> void:
	match key:
		"Horizontal Scale":
			WorldGenerationSettings.horizontal_scale = new_value
		"Sea Level Height":
			WorldGenerationSettings.sea_level_y = int(new_value)
		"Heightmap Min Level":
			WorldGenerationSettings.heightmap_min_y = int(new_value)
		"Heightmap Max Level":
			WorldGenerationSettings.heightmap_max_y = int(new_value)
		_:
			pass
	properties_changed()


func get_new_noise_texture() -> void:
	#noise_texture_rect.texture = ImageTexture.create_from_image(heightmap_provider.get_image())
	pass


func _on_reset_button_pressed() -> void:
	reset.call_deferred()

func reset():
	WorldGenerationSettings.reset()
	for heightmap_provider_display in get_tree().get_nodes_in_group("heightmap_provider_displays"):
		heightmap_provider_display.reset()
	setup_properties()
	properties_changed()
	setup_curves()

func _on_curve_editor_curve_changed(curve: Curve) -> void:
	properties_changed()
	$CurveControl/HBoxContainer/RotatedLabelNode/CurvesUnbakedWarning.show()


func _on_contribution_curve_editor_curve_changed(curve: Curve) -> void:
	properties_changed()
	$CurveControl/HBoxContainer/RotatedLabelNode/CurvesUnbakedWarning.show()


func _on_bake_button_pressed() -> void:
	#heightmap_provider.bake_curves()
	$CurveControl/HBoxContainer/RotatedLabelNode/CurvesUnbakedWarning.hide()

func properties_changed() -> void:
	get_new_noise_texture()

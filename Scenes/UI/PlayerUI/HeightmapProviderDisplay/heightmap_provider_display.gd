class_name HeightmapProviderDisplay
extends Control

var heightmap_provider : HeightmapProvider
@onready var noise_texture_rect : TextureRect = $NoiseTextureRect
@onready var properties_box : PropertiesBox = $ScaleSliderControl/VBoxContainer/PropertiesBox
@onready var curve_editor : CurveEditor = $CurveControl/HBoxContainer/VBoxContainer/FirstCurveControl/VBoxContainer/CurveEditor
@onready var contribution_curve_editor : CurveEditor = $CurveControl/HBoxContainer/VBoxContainer/SecondCurveControl/VBoxContainer/CurveEditor
var curves_baked := true
signal noise_updated

func _ready() -> void:
	setup_properties()
	get_new_noise_texture()
	setup_curves()

func setup_properties() -> void:
	properties_box.clear()
	properties_box.add_group("Base")
	properties_box.add_float("Scale", heightmap_provider.scale, 0.00001, 10, 0.01)
	properties_box.add_float("Period", 1 / heightmap_provider.noise.frequency, 0.1, 20000, 0.1)
	properties_box.end_group()
	properties_box.add_spacer(false)
	properties_box.add_group("Fractal")
	properties_box.add_int("Fractal Octaves", heightmap_provider.noise.fractal_octaves, 1, 10)
	properties_box.add_float("Fractal Lacunarity", heightmap_provider.noise.fractal_lacunarity, 0, 5, 0.01)
	properties_box.add_float("Fractal Gain", heightmap_provider.noise.fractal_gain, 0, 1, 0.01)
	properties_box.end_group()
	properties_box.add_spacer(false)
	properties_box.add_group("Additional")
	properties_box.add_float("Power", heightmap_provider.power, 0.00001, 10, 0.01)
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
		"Scale":
			heightmap_provider.scale = new_value
		"Period":
			heightmap_provider.noise.frequency = 1 / new_value
		"Fractal Octaves":
			heightmap_provider.noise.fractal_octaves = int(new_value)
		"Fractal Lacunarity":
			heightmap_provider.noise.fractal_lacunarity = new_value
		"Fractal Gain":
			heightmap_provider.noise.fractal_gain = new_value
		"Power":
			heightmap_provider.power = new_value
		_:
			pass
	properties_changed()


func get_new_noise_texture() -> void:
	noise_texture_rect.texture = ImageTexture.create_from_image(heightmap_provider.get_image())
	noise_updated.emit()


func _on_reset_button_pressed() -> void:
	reset()
	#print()

func reset():
	heightmap_provider.reset()
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
	heightmap_provider.bake_curves()
	$CurveControl/HBoxContainer/RotatedLabelNode/CurvesUnbakedWarning.hide()

func properties_changed() -> void:
	get_new_noise_texture()
	get_tree().get_first_node_in_group("world_generation_settings_display").properties_changed()

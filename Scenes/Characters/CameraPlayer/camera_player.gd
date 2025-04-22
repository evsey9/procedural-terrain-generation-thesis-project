extends CameraMovementOld

@onready var hud : Control = $HUD
@onready var player_ui : PlayerUI = $PlayerUI

func _process(delta: float) -> void:
	super(delta)
	hud.get_node("CoordinateLabel").text = str(position) + " " + str(Engine.get_frames_per_second())
	$HUD/Control/Line.rotation = -rotation.y

func _physics_process(delta: float) -> void:
	super(delta)
	pass

func _input(event: InputEvent) -> void:
	super(event)
	if event.is_action_pressed("show_info"):
		hud.visible = !hud.visible
	if event.is_action_pressed("show_menu"):
		player_ui.toggle()
	

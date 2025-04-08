extends "res://addons/Free fly camera/Src/free_fly_startup.gd"

func _physics_process(delta: float) -> void:
	super(delta)
	$UI/CoordinateLabel.text = str(position) + " " + str(Engine.get_frames_per_second())
	pass

func _input(event: InputEvent) -> void:
	super(event)
	if event.is_action_pressed("show_info"):
		$UI.visible = !$UI.visible

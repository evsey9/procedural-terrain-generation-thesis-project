class_name PlayerUI
extends Control

signal enabled
signal disabled

func _ready() -> void:
	disable()

func toggle():
	if visible:
		disable()
	else:
		enable()

func enable():
	enabled.emit()
	show()

func disable():
	disabled.emit()
	hide()

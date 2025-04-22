using System;
using Godot;
using ProceduralTerrainGenerationThesisProject.UI;

namespace ProceduralTerrainGenerationThesisProject.Characters;

public partial class CameraPlayer : CameraMovement
{
	[Export]
	private Control Hud { get; set; } = null!;
	
	[Export]
	private PlayerUi PlayerUi { get; set; } = null!;

	public override void _Process(Double delta)
	{
		base._Process(delta);
		Hud.GetNode<Label>("CoordinateLabel").Text =
			$"({Position.X:f2}, {Position.Y:f2}, {Position.Z:f2}) {Engine.GetFramesPerSecond()}";
	}
	
	public override void _PhysicsProcess(Double delta)
	{
		base._PhysicsProcess(delta);
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event.IsActionPressed("show_info"))
		{
			Hud.Visible = !Hud.Visible;
		}

		if (@event.IsActionPressed("show_menu"))
		{
			PlayerUi.Toggle();
		}
	}
}
using System;
using Godot;

namespace ProceduralTerrainGenerationThesisProject.Characters;

public partial class CameraMovement : CharacterBody3D
{
	#region Movement Settings
	
	[ExportSubgroup("Movement Settings")]
	[Export]
	public Double MovementSpeed { get; set; } = 10;
	
	[Export]
	public Double SprintMultiplier { get; set; } = 2;
	
	#endregion
	
	#region Movement Actions
	
	[ExportSubgroup("Movement Actions")]
	[Export]
	public String KeyForward { get; set; } = "move_forward";
	
	[Export]
	public String KeyBackward { get; set; } = "move_backward";
	
	[Export]
	public String KeyLeft { get; set; } = "move_left";
	
	[Export]
	public String KeyRight { get; set; } = "move_right";
	
	[Export]
	public String KeyUp { get; set; } = "move_up";
	
	[Export]
	public String KeyDown { get; set; } = "move_down";
	
	[Export]
	public String KeySprint { get; set; } = "sprint";
	
	#endregion
	
	#region Mouse Settings
	
	[ExportSubgroup("Mouse Settings")]
	[Export]
	private Boolean MouseAccelerationEnabled { get; set; } = true;
	
	[Export]
	private Double MouseSensitivity { get; set; } = 0.005;
	
	[Export]
	private Double MouseAcceleration { get; set; } = 50;
	
	#endregion
	
	#region Head Rotation Clamping
	
	[ExportSubgroup("Head Rotation Clamping")]
	[Export]
	private Boolean HeadRotationClampingEnabled { get; set; } = true;
	
	[Export]
	private Double HeadRotationClampingMinAngleDegrees { get; set; } = -90;
	
	[Export]
	private Double HeadRotationClampingMaxAngleDegrees { get; set; } = 90;
	
	#endregion
	
	#region Camera Node

	[ExportSubgroup("Camera Node")]
	[Export]
	private Camera3D? Camera { get; set; }
	
	#endregion
	
	#region Collision Settings
	
	[ExportSubgroup("Collision Settings")]
	[Export]
	private Boolean CollisionEnabled { get; set; } = false;
	
	[Export]
	private CollisionShape3D? CollisionShape { get; set; }
	
	#endregion
	
	#region Advanced Settings
	
	[ExportSubgroup("Advanced Settings")]
	[Export]
	private Boolean UpdateOnPhysics { get; set; } = false;
	
	[Export]
	private String KeyEscape { get; set; } = "escape";

	#endregion
	
	private Double PlayerRotationTarget { get; set; }
	private Double HeadRotationTarget { get; set; }
	private Boolean MouseCaptured { get; set; } = true;

	public override void _Ready()
	{
		if (CollisionShape is not null)
		{
			CollisionShape.Disabled = !CollisionEnabled;
		}

		if (Camera is null)
		{
			Camera = new Camera3D();
			AddChild(Camera);
		}

		Input.SetMouseMode(Input.MouseModeEnum.Captured);
	}

	public override void _Process(Double delta)
	{
		if (!UpdateOnPhysics)
		{
			HandleInput(delta); 
			RotatePlayer(delta);
		}
	}

	public override void _PhysicsProcess(Double delta)
	{
		if (UpdateOnPhysics)
		{
			HandleInput(delta); 
			RotatePlayer(delta);
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (Engine.IsEditorHint())
		{
			return;
		}

		if ((@event is InputEventMouseMotion mouseMotion) && (Input.MouseMode == Input.MouseModeEnum.Captured))
		{
			SetRotationTarget(mouseMotion.Relative);
		}
	}

	private void HandleInput(Double deltaTime)
	{
		Vector3 movementDirection = Vector3.Zero;
		if (Input.IsActionJustPressed(KeyEscape))
		{
			MouseCaptured = !MouseCaptured;
			Input.SetMouseMode(MouseCaptured ? Input.MouseModeEnum.Captured : Input.MouseModeEnum.Visible);
		}

		if (MouseCaptured)
		{
			movementDirection.X += Input.GetActionStrength(KeyRight) - Input.GetActionStrength(KeyLeft);
			movementDirection.Y += Input.GetActionStrength(KeyUp) - Input.GetActionStrength(KeyDown);
			movementDirection.Z += Input.GetActionStrength(KeyForward) - Input.GetActionStrength(KeyBackward);
		}
		
		movementDirection = movementDirection.Normalized();

		Double speedModifier = Input.IsActionPressed(KeySprint) ? SprintMultiplier : 1;
		
		GlobalTranslate(
			(Camera!.GlobalTransform.Basis.Z * -movementDirection.Z + 
			 Camera!.GlobalTransform.Basis.X * movementDirection.X + 
			 Vector3.Up * movementDirection.Y).Normalized() *
			(Single)(MovementSpeed * speedModifier * deltaTime));
	}

	private void SetRotationTarget(Vector2 mouseMotion)
	{
		PlayerRotationTarget += -mouseMotion.X * MouseSensitivity;
		HeadRotationTarget += -mouseMotion.Y * MouseSensitivity;

		if (HeadRotationClampingEnabled)
		{
			HeadRotationTarget = Double.Clamp(HeadRotationTarget, Double.DegreesToRadians(HeadRotationClampingMinAngleDegrees), Double.DegreesToRadians(HeadRotationClampingMaxAngleDegrees));
		}
	}

	private void RotatePlayer(Double deltaTime)
	{
		if (MouseAccelerationEnabled)
		{
			Quaternion = Quaternion.Slerp(new Quaternion(Vector3.Up, (Single)PlayerRotationTarget), (Single)(MouseAcceleration * deltaTime));
			Camera!.Quaternion = Camera!.Quaternion.Slerp(new Quaternion(Vector3.Right, (Single)HeadRotationTarget), (Single)(MouseAcceleration * deltaTime));
		}
		else
		{
			Quaternion = new Quaternion(Vector3.Up, (Single)PlayerRotationTarget);
			Camera!.Quaternion = new Quaternion(Vector3.Right, (Single)HeadRotationTarget);
		}
	}
	
	
}
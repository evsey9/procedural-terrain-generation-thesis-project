using Godot;

namespace ProceduralTerrainGenerationThesisProject.UI;

public partial class PlayerUi : Control
{
	[Signal]
	public delegate void EnabledEventHandler();
	
	[Signal]
	public delegate void DisabledEventHandler();
	
	public override void _Ready()
	{
		Disable();
	}

	public void Toggle()
	{
		if (Visible)
		{
			Disable();
		}
		else
		{
			Enable();
		}
	}

	public void Enable()
	{
		Show();
		EmitSignal(SignalName.Enabled);
	}
	
	public void Disable()
	{
		Hide();
		EmitSignal(SignalName.Disabled);
	}
}
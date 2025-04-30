using System;
using Godot;
using ProceduralTerrainGenerationThesisProject.Singletons;
using ProceduralTerrainGenerationThesisProject.UI;

namespace ProceduralTerrainGenerationThesisProject.Characters;

public partial class CameraPlayer : CameraMovement
{
	[Export]
	private Control Hud { get; set; } = null!;
	
	[Export]
	private PlayerUi PlayerUi { get; set; } = null!;
	
	[Export]
	private FileDialog SettingsSaveDialog { get; set; } = null!;
	
	[Export]
	private FileDialog SettingsLoadDialog { get; set; } = null!;

	public override void _Ready()
	{
		SettingsSaveDialog.FileSelected += SettingsSaveDialogFileSelectedEventHandler;
		SettingsLoadDialog.FileSelected += SettingsLoadDialogFileSelectedEventHandler;
		SwitchMouseCaptured(true);
		if (!DirAccess.Open("user://").DirExists("WorldGenerationBundles"))
		{
			DirAccess.Open("user://").MakeDir("WorldGenerationBundles");
		}
	}
	
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

		if (@event.IsActionPressed("save_settings"))
		{
			SwitchMouseCaptured(false);
			SettingsSaveDialog.Show();
		}
		
		if (@event.IsActionPressed("load_settings"))
		{
			SwitchMouseCaptured(false);
			SettingsLoadDialog.Show();
		}
	}

	private void SettingsSaveDialogFileSelectedEventHandler(String filePath)
	{
		GD.Print($"saving at path {filePath}");
		var b = WorldGenerationSettingsProvider.GetSingleton(this).Resource;
		var savedata =
			new Resources.SaveData.WorldGenerationBundle(WorldGenerationSettingsProvider.GetSingleton(this).Resource);
		Error error = ResourceSaver.Save(savedata, filePath);
		SwitchMouseCaptured(true);
	}
	
	private void SettingsLoadDialogFileSelectedEventHandler(String filePath)
	{
		GD.Print($"loading at path {filePath}");
		WorldGenerationSettingsProvider.GetSingleton(this).LoadFromSaveData(GD.Load<Resources.SaveData.WorldGenerationBundle>(filePath));
		SwitchMouseCaptured(true);
	}
}
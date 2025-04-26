using System;
using Godot;

namespace ProceduralTerrainGenerationThesisProject.Singletons;

public partial class WorldGenerationSettingsProvider : Node
{
	
	#region Signals
	
	[Signal]
	public delegate void ChangedEventHandler();
	
	[Signal]
	public delegate void ReloadedEventHandler();
	
	#endregion

	public Resources.WorldGenerationBundle Resource { get; private set; } = new Resources.WorldGenerationBundle();
	public static WorldGenerationSettingsProvider? Singleton { get; private set; }

	[Export]
	public Resources.SaveData.WorldGenerationBundle? SaveData { get; set; }

	public WorldGenerationSettingsProvider()
	{
		Singleton = this;
		Resource.Changed += ChangeSettings;
		Resource.Reloaded += Reload;
	}

	public override void _Ready()
	{
		if (SaveData is not null)
		{
			Resource.LoadFromSaveData(SaveData);
		}
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("save_settings"))
		{
			
		}
	}

	public void Reset(Boolean sendSignal = true)
	{
		Resource.Reset(sendSignal);
	}

	public void ChangeSettings()
	{
		EmitSignal(SignalName.Changed);
	}
	
	public void Reload()
	{
		EmitSignal(SignalName.Reloaded);
	}

	public static WorldGenerationSettingsProvider GetSingleton(Node sceneNode)
	{
		return sceneNode.GetNode<WorldGenerationSettingsProvider>("/root/WorldGenerationSettingsProvider");
	}
	
	public static Resources.WorldGenerationBundle GetResource(Node sceneNode)
	{
		return sceneNode.GetNode<WorldGenerationSettingsProvider>("/root/WorldGenerationSettingsProvider").Resource;
	}
	
	public static Resources.WorldGenerationSettings GetWorldGenerationSettings(Node sceneNode)
	{
		return sceneNode.GetNode<WorldGenerationSettingsProvider>("/root/WorldGenerationSettingsProvider").Resource.WorldGenerationSettings;
	}
}
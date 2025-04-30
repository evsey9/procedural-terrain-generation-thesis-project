using System;
using Godot;
using ProceduralTerrainGenerationThesisProject.Resources;
using ProceduralTerrainGenerationThesisProject.VoxelGeneratorScripts;

namespace ProceduralTerrainGenerationThesisProject.Singletons;

public partial class WorldGenerationSettingsProvider : Node
{
	
	#region Signals
	
	[Signal]
	public delegate void ChangedEventHandler();
	
	[Signal]
	public delegate void ReloadedEventHandler();
	
	[Signal]
	public delegate void WorldGenerationProgressStartedEventHandler();
	
	[Signal]
	public delegate void WorldGenerationProgressEndedEventHandler();
	
	#endregion

	public Resources.WorldGenerationBundle Resource { get; private set; } = new Resources.WorldGenerationBundle();
	public static WorldGenerationSettingsProvider? Singleton { get; private set; }

	[Export]
	public Resources.SaveData.WorldGenerationBundle? SaveData { get; set; }
	
	[Export]
	public Timer? WorldResetTimer { get; set; }
	
	[Export]
	public Timer? WorldGenerationInProgressTimer { get; set; }
	
	[Export]
	public Timer? InstantResetTimer { get; set; }
	
	[Export]
	private WorldVoxelGenerator WorldVoxelGenerator { get; set; } = new WorldVoxelGenerator();
	
	public Boolean WorldGenerationEnabledFlag { get; set; } = true;
	public Boolean WorldGenerationInProgress { get; set; } = true;
	public Boolean InstantResetFlag { get; set; } = true;

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

		if (WorldResetTimer is not null)
		{
			WorldResetTimer.Timeout += () =>
			{
				if (Time.GetTicksMsec() > 3000)
				{
					ResetGenerator();
				}
			};
		}

		if (WorldGenerationInProgressTimer is not null)
		{
			WorldGenerationInProgressTimer.Timeout += () =>
			{
				WorldGenerationInProgress = false;
				EmitSignal(SignalName.WorldGenerationProgressEnded);
			};
		}

		if (InstantResetTimer is not null)
		{
			InstantResetTimer.Timeout += () =>
			{
				InstantResetFlag = false;
			};
		}
	}

	public void ResetGenerator()
	{
		VoxelTerrain voxelTerrain = (VoxelTerrain)GetTree().GetFirstNodeInGroup("voxel_terrain");
		if (voxelTerrain.Generator is not null)
		{
			((WorldVoxelGenerator)voxelTerrain.Generator).WorldGenerationEnabled = false;
			voxelTerrain.Generator = null;
		}
		Resource.BakeCurves();
		voxelTerrain.Generator = new WorldVoxelGenerator();
		GetTree().CreateTimer(timeSec: 0.2).Timeout += () =>
		{
			//voxelTerrain.Generator = new WorldVoxelGenerator();
		};
	}

	public void Reset(Boolean sendSignal = true)
	{
		Resource.Reset(sendSignal);
	}

	public void ChangeSettings()
	{
		EmitSignal(SignalName.Changed);
		if (WorldResetTimer is not null)
		{
			WorldResetTimer.WaitTime = InstantResetFlag ? 0.05 : 0.2;
			WorldResetTimer.Start();
		}
		
	}
	
	public void Reload()
	{
		EmitSignal(SignalName.Reloaded);
	}

	public void UpdateWorldGenerationInProgress()
	{
		if (!WorldGenerationInProgress)
		{
			EmitSignal(SignalName.WorldGenerationProgressStarted);
		}
		WorldGenerationInProgress = true;
		WorldGenerationInProgressTimer?.Start();
	}

	public void SetInstantReset()
	{
		InstantResetFlag = true;
		InstantResetTimer?.Start();
	}
	
	public void LoadFromSaveData(Resources.SaveData.WorldGenerationBundle saveData)
	{
		VoxelTerrain voxelTerrain = (VoxelTerrain)GetTree().GetFirstNodeInGroup("voxel_terrain");
		((WorldVoxelGenerator)voxelTerrain.Generator).WorldGenerationEnabled = false;
		SetInstantReset();
		Resource.LoadFromSaveData(saveData);
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
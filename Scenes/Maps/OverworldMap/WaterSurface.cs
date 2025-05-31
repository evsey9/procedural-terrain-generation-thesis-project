using Godot;
using System;
using ProceduralTerrainGenerationThesisProject.Singletons;

namespace ProceduralTerrainGenerationThesisProject.Maps.OverworldMap;

public partial class WaterSurface : MeshInstance3D
{
	public Node3D? Player { get; set; }
	private WorldGenerationSettingsProvider? worldGenerationSettingsProvider;
	
	public override void _Ready()
	{
		
	}

	public override void _Process(Double delta)
	{
		Player ??= (Node3D)(GetTree().GetFirstNodeInGroup("player"));
		
		if (WorldGenerationSettingsProvider.Singleton is null)
		{
			return;
		}

		worldGenerationSettingsProvider ??= WorldGenerationSettingsProvider.Singleton;

		if (Player is not null)
		{
			Vector3 position = GlobalPosition;
			//position.X = Player.GlobalPosition.X;
			//position.Z = Player.GlobalPosition.Z;
			position.Y = worldGenerationSettingsProvider.Resource.WorldGenerationSettings.SeaLevelY;
			GlobalPosition = position;
		}
	}
}

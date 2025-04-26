using System;
using Godot;
using ProceduralTerrainGenerationThesisProject.Resources;
using ProceduralTerrainGenerationThesisProject.Singletons;

namespace ProceduralTerrainGenerationThesisProject.HeightmapProviders;

public partial class HeightmapProvider : Node
{
	private WorldGenerationSettingsProvider worldGenerationSettingsProvider = null!;
	
	public HeightmapSettings HeightmapSettings { get; protected set; } = null!;
	
	private Node3D? Player { get; set; }

	public static HeightmapProvider? Singleton { get; private set; }
	
	public HeightmapProvider()
	{
		Singleton = this;
	}
	
	public override void _Ready()
	{
		worldGenerationSettingsProvider = WorldGenerationSettingsProvider.GetSingleton(this);
	}

	public override void _Process(Double delta)
	{
		if (Player is null)
		{
			Player = GetTree().GetFirstNodeInGroup("player") as Node3D;
		}
		else
		{
			Vector3 playerPosition = Player.Position;
			if (HeightmapSettings.DisplayNoise is not null)
			{
				HeightmapSettings.DisplayNoise.Offset = new Vector3(playerPosition.X, playerPosition.Z, 0) / (Single)HeightmapSettings.DisplayNoiseScale -
				                                        new Vector3(HeightmapSettings.NoiseTextureWidth / 2f, HeightmapSettings.NoiseTextureHeight / 2f, 0);
			}
		}
	}
}
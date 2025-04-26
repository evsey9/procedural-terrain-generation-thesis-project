using System;
using Godot;
using ProceduralTerrainGenerationThesisProject.Singletons;

namespace ProceduralTerrainGenerationThesisProject.HeightmapProviders;

public partial class WorldHeightmapProvider : HeightmapProvider
{
	private WorldGenerationSettingsProvider worldGenerationSettingsProvider = null!;
	private ContinentalnessHeightmapProvider continentalnessHeightmapProvider = null!;
	
	public new static WorldHeightmapProvider? Singleton { get; private set; }
	
	public WorldHeightmapProvider()
	{
		Singleton = this;
	}

	public override void _Ready()
	{
		worldGenerationSettingsProvider = WorldGenerationSettingsProvider.GetSingleton(this);
		continentalnessHeightmapProvider = ContinentalnessHeightmapProvider.GetSingleton(this);
		HeightmapSettings = worldGenerationSettingsProvider.Resource.WorldGenerationHeightmapSettings;
	}
}
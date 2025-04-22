using System;
using Godot;
using ProceduralTerrainGenerationThesisProject.Singletons;

namespace ProceduralTerrainGenerationThesisProject.HeightmapProviders;

public partial class WorldHeightmapProvider : HeightmapProvider
{
	private WorldGenerationSettings worldGenerationSettings = null!;
	private ContinentalnessHeightmapProvider continentalnessHeightmapProvider = null!;
	
	public new static WorldHeightmapProvider? Singleton { get; private set; }
	
	public WorldHeightmapProvider()
	{
		Singleton = this;
		DefaultNoise = null;
		DefaultCurve = null;
		DefaultContributionCurve = null;
		DefaultScale = 1.0;
		DefaultPower = 1.0;
	}

	public override void _Ready()
	{
		worldGenerationSettings = WorldGenerationSettings.GetSingleton(this);
		continentalnessHeightmapProvider = ContinentalnessHeightmapProvider.GetSingleton(this);
	}

	public override Int32 GetHeightAt(Double x, Double z)
	{
		return (Int32)(continentalnessHeightmapProvider.GetValueAt(x, z) * worldGenerationSettings.HeightmapRange) + worldGenerationSettings.HeightmapMinY;
	}
}
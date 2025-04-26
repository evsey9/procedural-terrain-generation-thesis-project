using System;
using Godot;

namespace ProceduralTerrainGenerationThesisProject.Resources;

public partial class ContinentalnessHeightmapSettings : HeightmapSettings
{
	public ContinentalnessHeightmapSettings(WorldGenerationBundle worldGenerationBundle) : base(worldGenerationBundle)
	{
		DefaultNoise = 
			GD.Load<FastNoiseLite>(
				"res://Source/HeightmapProviders/ContinentalnessHeightmapProvider/continentalness_noise.tres");
		DefaultCurve =
			GD.Load<Curve>(
				"res://Source/HeightmapProviders/ContinentalnessHeightmapProvider/continentalness_curve.tres");
		DefaultContributionCurve =
			GD.Load<Curve>(
				"res://Source/HeightmapProviders/ContinentalnessHeightmapProvider/continentalness_contribution_curve_basic.tres");
		DefaultScale = 1.0;
		DefaultPower = 1.5;
		Initialize();
	}

	public ContinentalnessHeightmapSettings() : this(WorldGenerationBundle.Singleton!)
	{
	}
	
	public void LoadFromSaveData(SaveData.ContinentalnessHeightmapSettings saveData)
	{
		base.LoadFromSaveData(saveData);
	}
}
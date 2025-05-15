using System;
using Godot;

namespace ProceduralTerrainGenerationThesisProject.Resources;

public partial class Continentalness2HeightmapSettings : HeightmapSettings
{
	public Continentalness2HeightmapSettings(WorldGenerationBundle worldGenerationBundle) : base(worldGenerationBundle)
	{
		DefaultNoise = 
			GD.Load<FastNoiseLite>(
				"res://Source/Resources/WorldGenerationBundle/Continentalness2HeightmapSettings/continentalness2_noise.tres");
		DefaultCurve =
			GD.Load<Curve>(
				"res://Source/Resources/WorldGenerationBundle/Continentalness2HeightmapSettings/continentalness2_curve.tres");
		DefaultContributionCurve =
			GD.Load<Curve>(
				"res://Source/Resources/WorldGenerationBundle/Continentalness2HeightmapSettings/continentalness2_contribution_curve_basic.tres");
		DefaultScale = 1.0;
		DefaultPower = 1.5;
		Initialize();
	}

	public Continentalness2HeightmapSettings() : this(WorldGenerationBundle.Singleton!)
	{
	}
	
	public void LoadFromSaveData(SaveData.Continentalness2HeightmapSettings saveData)
	{
		base.LoadFromSaveData(saveData);
	}
}
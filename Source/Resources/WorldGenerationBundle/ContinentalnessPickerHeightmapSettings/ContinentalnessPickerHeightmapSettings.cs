using System;
using Godot;

namespace ProceduralTerrainGenerationThesisProject.Resources;

public partial class ContinentalnessPickerHeightmapSettings : HeightmapSettings
{
	public ContinentalnessPickerHeightmapSettings(WorldGenerationBundle worldGenerationBundle) : base(worldGenerationBundle)
	{
		DefaultNoise = 
			GD.Load<FastNoiseLite>(
				"res://Source/Resources/WorldGenerationBundle/ContinentalnessPickerHeightmapSettings/continentalness_picker_noise.tres");
		DefaultCurve =
			GD.Load<Curve>(
				"res://Source/Resources/WorldGenerationBundle/ContinentalnessPickerHeightmapSettings/continentalness_picker_curve.tres");
		DefaultContributionCurve =
			GD.Load<Curve>(
				"res://Source/Resources/WorldGenerationBundle/ContinentalnessPickerHeightmapSettings/continentalness_picker_contribution_curve_basic.tres");
		DefaultScale = 1.0;
		DefaultPower = 1.5;
		Initialize();
	}

	public ContinentalnessPickerHeightmapSettings() : this(WorldGenerationBundle.Singleton!)
	{
	}
	
	public void LoadFromSaveData(SaveData.ContinentalnessPickerHeightmapSettings saveData)
	{
		base.LoadFromSaveData(saveData);
	}
}
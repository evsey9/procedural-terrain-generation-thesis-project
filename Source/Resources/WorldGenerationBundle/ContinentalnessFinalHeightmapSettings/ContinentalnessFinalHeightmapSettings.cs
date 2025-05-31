using System;
using Godot;

namespace ProceduralTerrainGenerationThesisProject.Resources;

public partial class ContinentalnessFinalHeightmapSettings : HeightmapSettings
{
	public ContinentalnessFinalHeightmapSettings(WorldGenerationBundle worldGenerationBundle) : base(worldGenerationBundle)
	{
		DefaultNoise = null;
		DefaultCurve =
			GD.Load<Curve>(
				"res://Source/Resources/WorldGenerationBundle/ContinentalnessFinalHeightmapSettings/continentalness_final_curve.tres");
		DefaultContributionCurve =
			GD.Load<Curve>(
				"res://Source/Resources/WorldGenerationBundle/ContinentalnessFinalHeightmapSettings/continentalness_final_contribution_curve_basic.tres");
		DefaultScale = 1.0;
		DefaultPower = 1.0;
		Initialize();
	}

	public ContinentalnessFinalHeightmapSettings() : this(WorldGenerationBundle.Singleton!)
	{
	}
	
	public override Double GetValueAt(Double x, Double z)
	{
		if (Curve is not null && ContributionCurve is not null)
		{
			Double continentalnessValue = WorldGenerationBundle.ContinentalnessHeightmapSettings.GetValueAt(x, z);
			Double continentalness2Value = WorldGenerationBundle.Continentalness2HeightmapSettings.GetValueAt(x, z);
			Double continentalnessPickerValue = WorldGenerationBundle.ContinentalnessPickerHeightmapSettings.GetValueAt(x, z);
			Double value = Double.Lerp(continentalnessValue, continentalness2Value, continentalnessPickerValue);
			return Curve.Sample((Single)Math.Pow(value, Power)) * ContributionCurve.Sample((Single)value);
		}
		return 0;
	}
	
	public override Int32 GetHeightAt(Double x, Double z)
	{
		Double value = GetValueAt(x, z);
		return (Int32)(value * WorldGenerationBundle.WorldGenerationSettings.HeightmapRange) + WorldGenerationBundle.WorldGenerationSettings.HeightmapMinY;
	}
	
	public void LoadFromSaveData(SaveData.ContinentalnessFinalHeightmapSettings saveData)
	{
		base.LoadFromSaveData(saveData);
	}
}
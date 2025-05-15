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
	
	public override Int32 GetHeightAt(Double x, Double z)
	{
		return 0;
		//return (Int32)(WorldGenerationBundle.ContinentalnessHeightmapSettings.GetValueAt(x, z) * WorldGenerationBundle.WorldGenerationSettings.HeightmapRange) + WorldGenerationBundle.WorldGenerationSettings.HeightmapMinY;
	}
	
	public void LoadFromSaveData(SaveData.ContinentalnessFinalHeightmapSettings saveData)
	{
		base.LoadFromSaveData(saveData);
	}
}
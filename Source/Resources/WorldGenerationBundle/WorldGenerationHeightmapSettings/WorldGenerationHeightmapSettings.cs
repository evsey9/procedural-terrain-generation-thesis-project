using System;
using Godot;

namespace ProceduralTerrainGenerationThesisProject.Resources;

public partial class WorldGenerationHeightmapSettings : HeightmapSettings
{
	public WorldGenerationHeightmapSettings(WorldGenerationBundle worldGenerationBundle) : base(worldGenerationBundle)
	{
		DefaultNoise = null;
		DefaultCurve = null;
		DefaultContributionCurve = null; 
		DefaultScale = 1.0;
		DefaultPower = 1.0;
		Initialize();
	}

	public WorldGenerationHeightmapSettings() : this(WorldGenerationBundle.Singleton!)
	{
	}
	
	public override Int32 GetHeightAt(Double x, Double z)
	{
		//return (Int32)(WorldGenerationBundle.ContinentalnessHeightmapSettings.GetValueAt(x, z) * WorldGenerationBundle.WorldGenerationSettings.HeightmapRange) + WorldGenerationBundle.WorldGenerationSettings.HeightmapMinY;
		return (Int32)(WorldGenerationBundle.ContinentalnessFinalHeightmapSettings.GetValueAt(x / WorldGenerationBundle.WorldGenerationSettings.HorizontalScale, z / WorldGenerationBundle.WorldGenerationSettings.HorizontalScale) * WorldGenerationBundle.WorldGenerationSettings.HeightmapRange) + WorldGenerationBundle.WorldGenerationSettings.HeightmapMinY;
	}
	
	public void LoadFromSaveData(SaveData.WorldGenerationHeightmapSettings saveData)
	{
		base.LoadFromSaveData(saveData);
	}
}
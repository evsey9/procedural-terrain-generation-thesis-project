using System;
using Godot;

namespace ProceduralTerrainGenerationThesisProject.Resources.SaveData;

public partial class WorldGenerationHeightmapSettings : HeightmapSettings
{
	public WorldGenerationHeightmapSettings()
	{
		/*
		DefaultNoise = null;
		DefaultCurve = null;
		DefaultContributionCurve = null; 
		DefaultScale = 1.0;
		DefaultPower = 1.0;
		*/
	}

	public WorldGenerationHeightmapSettings(Resources.WorldGenerationHeightmapSettings heightmapSettings) : base(heightmapSettings)
	{
		
	}
	
}
using System;
using Godot;

namespace ProceduralTerrainGenerationThesisProject.Resources.SaveData;

[GlobalClass]
public partial class WorldGenerationBundle : Resource
{
	[Export]
	public WorldGenerationSettings WorldGenerationSettings { get; set; } = new WorldGenerationSettings();
	
	[Export]
	public ContinentalnessHeightmapSettings ContinentalnessHeightmapSettings { get; set; } = new ContinentalnessHeightmapSettings();
	
	[Export]
	public WorldGenerationHeightmapSettings WorldGenerationHeightmapSettings { get; set; } = new WorldGenerationHeightmapSettings();

	public WorldGenerationBundle()
	{
		
	}

	public WorldGenerationBundle(Resources.WorldGenerationBundle bundle)
	{
		WorldGenerationSettings = new WorldGenerationSettings(bundle.WorldGenerationSettings);
		ContinentalnessHeightmapSettings = new ContinentalnessHeightmapSettings(bundle.ContinentalnessHeightmapSettings);
		WorldGenerationHeightmapSettings = new WorldGenerationHeightmapSettings(bundle.WorldGenerationHeightmapSettings);
	}
}
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
	public Continentalness2HeightmapSettings Continentalness2HeightmapSettings { get; set; } = new Continentalness2HeightmapSettings();
	
	[Export]
	public ContinentalnessPickerHeightmapSettings ContinentalnessPickerHeightmapSettings { get; set; } = new ContinentalnessPickerHeightmapSettings();
	
	[Export]
	public ContinentalnessFinalHeightmapSettings ContinentalnessFinalHeightmapSettings { get; set; } = new ContinentalnessFinalHeightmapSettings();
	
	[Export]
	public WorldGenerationHeightmapSettings WorldGenerationHeightmapSettings { get; set; } = new WorldGenerationHeightmapSettings();
	
	[Export]
	public TreeHeightmapSettings TreeHeightmapSettings { get; set; } = new TreeHeightmapSettings();

	public WorldGenerationBundle()
	{
		
	}

	public WorldGenerationBundle(Resources.WorldGenerationBundle bundle)
	{
		WorldGenerationSettings = new WorldGenerationSettings(bundle.WorldGenerationSettings);
		ContinentalnessHeightmapSettings = new ContinentalnessHeightmapSettings(bundle.ContinentalnessHeightmapSettings);
		Continentalness2HeightmapSettings = new Continentalness2HeightmapSettings(bundle.Continentalness2HeightmapSettings);
		ContinentalnessPickerHeightmapSettings = new ContinentalnessPickerHeightmapSettings(bundle.ContinentalnessPickerHeightmapSettings);
		ContinentalnessFinalHeightmapSettings = new ContinentalnessFinalHeightmapSettings(bundle.ContinentalnessFinalHeightmapSettings);
		WorldGenerationHeightmapSettings = new WorldGenerationHeightmapSettings(bundle.WorldGenerationHeightmapSettings);
		TreeHeightmapSettings = new TreeHeightmapSettings(bundle.TreeHeightmapSettings);
	}
}
using System;
using Godot;

namespace ProceduralTerrainGenerationThesisProject.Resources.SaveData;

public partial class WorldGenerationSettings : Resource
{
	#region Defaults
	
	[Export]
	public Int32 DefaultSeaLevelY { get; set; } = 64;
	
	[Export]
	public Int32 DefaultHeightmapMinY { get; set; } = 0;
	
	[Export]
	public Int32 DefaultHeightmapMaxY { get; set; } = 128;
	
	[Export]
	public Double DefaultHorizontalScale { get; set; } = 1.0;
	
	[Export]
	public Int32 DefaultTreesMinY { get; set; } = 0;
	
	[Export]
	public Int32 DefaultTreesMaxY { get; set; } = 0;
	
	[Export]
	public Curve? DefaultSandCurve { get; set; } = new Curve();
	
	[Export]
	public Curve? DefaultStoneCurve { get; set; } = new Curve();
	
	#endregion

	public WorldGenerationSettings()
	{
		
	}

	public WorldGenerationSettings(Resources.WorldGenerationSettings settings)
	{
		DefaultSeaLevelY = settings.SeaLevelY;
		DefaultHeightmapMinY = settings.HeightmapMinY;
		DefaultHeightmapMaxY = settings.HeightmapMaxY;
		DefaultHorizontalScale = settings.HorizontalScale;
		DefaultTreesMinY = settings.TreesMinY;
		DefaultTreesMaxY = settings.TreesMaxY;
		DefaultSandCurve = settings.SandCurve;
		DefaultStoneCurve = settings.StoneCurve;
	}
}
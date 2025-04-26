using System;
using Godot;

namespace ProceduralTerrainGenerationThesisProject.Resources;

public partial class WorldGenerationSettings : Resource
{
	#region Defaults
	
	public Int32 DefaultSeaLevelY { get; set; } = 64;
	public Int32 DefaultHeightmapMinY { get; set; } = 0;
	public Int32 DefaultHeightmapMaxY { get; set; } = 128;
	public Double DefaultHorizontalScale { get; set; } = 1.0;
	public Int32 DefaultTreesMinY { get; set; } = 0;
	public Int32 DefaultTreesMaxY { get; set; } = 0;
	
	#endregion
	
	#region Values
	
	public Int32 SeaLevelY	{ get; set; }
	public Int32 HeightmapMinY	{ get; set; }
	public Int32 HeightmapMaxY	{ get; set; }
	public Double HorizontalScale	{ get; set; }
	public Int32 TreesMinY	{ get; set; }
	public Int32 TreesMaxY	{ get; set; }
	
	#endregion
	
	#region Inferred Values
	
	public Int32 HeightmapRange => HeightmapMaxY - HeightmapMinY;
	public Double SeaLevelRatio => (Double)(SeaLevelY - HeightmapMinY) / HeightmapRange;
	
	#endregion
	
	#region Signals
	
	[Signal]
	public delegate void ReloadedEventHandler();
	
	#endregion
	
	public WorldGenerationSettings() : base()
	{
		Reset();
	}
	
	public void Reset(Boolean sendSignal = true)
	{
		SeaLevelY = DefaultSeaLevelY;
		HeightmapMinY = DefaultHeightmapMinY;
		HeightmapMaxY = DefaultHeightmapMaxY;
		HorizontalScale = DefaultHorizontalScale;
		TreesMinY = DefaultTreesMinY;
		TreesMaxY = DefaultTreesMaxY;
		if (sendSignal)
		{
			EmitSignal(Resource.SignalName.Changed);
			EmitSignal(SignalName.Reloaded);
		}
	}

	public void ChangeSettings()
	{
		EmitSignal(Resource.SignalName.Changed);
	}
	
	public void LoadFromSaveData(SaveData.WorldGenerationSettings saveData)
	{
		DefaultSeaLevelY = saveData.DefaultSeaLevelY;
		DefaultHeightmapMinY = saveData.DefaultHeightmapMinY;
		DefaultHeightmapMaxY = saveData.DefaultHeightmapMaxY;
		DefaultHorizontalScale = saveData.DefaultHorizontalScale;
		DefaultTreesMinY = saveData.DefaultTreesMinY;
		DefaultTreesMaxY = saveData.DefaultTreesMaxY;
		Reset(true);
		EmitSignal(SignalName.Reloaded);
	}
}
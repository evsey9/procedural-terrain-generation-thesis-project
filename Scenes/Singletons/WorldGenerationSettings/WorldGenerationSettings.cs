using System;
using Godot;

namespace ProceduralTerrainGenerationThesisProject.Singletons;

public partial class WorldGenerationSettings : Node
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
	public delegate void ChangedEventHandler();
	
	#endregion

	public static WorldGenerationSettings? Singleton { get; private set; }
	
	public WorldGenerationSettings() : base()
	{
		Singleton = this;
		Reset();
	}

	public void Reset(Boolean sendSignal = true)
	{
		if (sendSignal)
		{
			EmitSignal(SignalName.Changed);
		}
		SeaLevelY = DefaultSeaLevelY;
		HeightmapMinY = DefaultHeightmapMinY;
		HeightmapMaxY = DefaultHeightmapMaxY;
		HorizontalScale = DefaultHorizontalScale;
		TreesMinY = DefaultTreesMinY;
		TreesMaxY = DefaultTreesMaxY;
	}

	public void ChangeSettings()
	{
		EmitSignal(SignalName.Changed);
	}

	public static WorldGenerationSettings GetSingleton(Node sceneNode)
	{
		return sceneNode.GetNode<WorldGenerationSettings>("/root/WorldGenerationSettings");
	}
}
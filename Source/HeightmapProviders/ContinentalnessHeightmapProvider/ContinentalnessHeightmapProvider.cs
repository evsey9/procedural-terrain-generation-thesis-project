using Godot;
using ProceduralTerrainGenerationThesisProject.Resources;

namespace ProceduralTerrainGenerationThesisProject.HeightmapProviders;

public partial class ContinentalnessHeightmapProvider : HeightmapProvider
{
	public new static ContinentalnessHeightmapProvider? Singleton { get; private set; }
	
	public ContinentalnessHeightmapProvider() : base()
	{
		Singleton = this;
	}

	public override void _Ready()
	{
		HeightmapSettings = Resources.WorldGenerationBundle.Singleton!.ContinentalnessHeightmapSettings;
	}
	
	public static ContinentalnessHeightmapProvider GetSingleton(Node sceneNode)
	{
		return sceneNode.GetNode<ContinentalnessHeightmapProvider>("/root/ContinentalnessHeightmapProvider");
	}
}
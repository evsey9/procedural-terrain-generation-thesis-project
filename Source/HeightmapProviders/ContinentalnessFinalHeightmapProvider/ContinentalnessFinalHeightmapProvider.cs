using Godot;
using ProceduralTerrainGenerationThesisProject.Resources;

namespace ProceduralTerrainGenerationThesisProject.HeightmapProviders;

public partial class ContinentalnessFinalHeightmapProvider : HeightmapProvider
{
	public new static ContinentalnessFinalHeightmapProvider? Singleton { get; private set; }
	
	public ContinentalnessFinalHeightmapProvider() : base()
	{
		Singleton = this;
	}

	public override void _Ready()
	{
		HeightmapSettings = Resources.WorldGenerationBundle.Singleton!.ContinentalnessFinalHeightmapSettings;
	}
	
	public static ContinentalnessFinalHeightmapProvider GetSingleton(Node sceneNode)
	{
		return sceneNode.GetNode<ContinentalnessFinalHeightmapProvider>("/root/ContinentalnessFinalHeightmapProvider");
	}
}
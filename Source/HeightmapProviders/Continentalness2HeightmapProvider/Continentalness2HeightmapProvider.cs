using Godot;
using ProceduralTerrainGenerationThesisProject.Resources;

namespace ProceduralTerrainGenerationThesisProject.HeightmapProviders;

public partial class Continentalness2HeightmapProvider : HeightmapProvider
{
	public new static Continentalness2HeightmapProvider? Singleton { get; private set; }
	
	public Continentalness2HeightmapProvider() : base()
	{
		Singleton = this;
	}

	public override void _Ready()
	{
		HeightmapSettings = Resources.WorldGenerationBundle.Singleton!.Continentalness2HeightmapSettings;
	}
	
	public static Continentalness2HeightmapProvider GetSingleton(Node sceneNode)
	{
		return sceneNode.GetNode<Continentalness2HeightmapProvider>("/root/Continentalness2HeightmapProvider");
	}
}
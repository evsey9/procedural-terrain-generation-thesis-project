using Godot;
using ProceduralTerrainGenerationThesisProject.Resources;

namespace ProceduralTerrainGenerationThesisProject.HeightmapProviders;

public partial class ContinentalnessPickerHeightmapProvider : HeightmapProvider
{
	public new static ContinentalnessPickerHeightmapProvider? Singleton { get; private set; }
	
	public ContinentalnessPickerHeightmapProvider() : base()
	{
		Singleton = this;
	}

	public override void _Ready()
	{
		HeightmapSettings = Resources.WorldGenerationBundle.Singleton!.ContinentalnessPickerHeightmapSettings;
	}
	
	public static ContinentalnessPickerHeightmapProvider GetSingleton(Node sceneNode)
	{
		return sceneNode.GetNode<ContinentalnessPickerHeightmapProvider>("/root/ContinentalnessPickerHeightmapProvider");
	}
}
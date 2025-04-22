using Godot;

namespace ProceduralTerrainGenerationThesisProject.HeightmapProviders;

public partial class ContinentalnessHeightmapProvider : HeightmapProvider
{
	public new static ContinentalnessHeightmapProvider? Singleton { get; private set; }
	
	public ContinentalnessHeightmapProvider() : base()
	{
		Singleton = this;
		DefaultNoise = 
			GD.Load<Noise>(
				"res://Source/HeightmapProviders/ContinentalnessHeightmapProvider/continentalness_noise.tres");
		DefaultCurve =
			GD.Load<Curve>(
				"res://Source/HeightmapProviders/ContinentalnessHeightmapProvider/continentalness_curve.tres");
		DefaultContributionCurve =
			GD.Load<Curve>(
				"res://Source/HeightmapProviders/ContinentalnessHeightmapProvider/continentalness_contribution_curve_basic.tres");
		DefaultScale = 1.0;
		DefaultPower = 1.5;
		Initialize();
	}
	
	public static ContinentalnessHeightmapProvider GetSingleton(Node sceneNode)
	{
		return sceneNode.GetNode<ContinentalnessHeightmapProvider>("/root/ContinentalnessHeightmapProvider");
	}
}
using System;
using Godot;

namespace ProceduralTerrainGenerationThesisProject.Resources;

public partial class TreeHeightmapSettings : HeightmapSettings
{
	public TreeHeightmapSettings(WorldGenerationBundle worldGenerationBundle) : base(worldGenerationBundle)
	{
		DefaultNoise = 
			GD.Load<FastNoiseLite>(
				"res://Source/Resources/WorldGenerationBundle/TreeHeightmapSettings/tree_noise.tres");
		DefaultCurve =
			GD.Load<Curve>(
				"res://Source/Resources/WorldGenerationBundle/TreeHeightmapSettings/tree_curve.tres");
		DefaultContributionCurve =
			GD.Load<Curve>(
				"res://Source/Resources/WorldGenerationBundle/TreeHeightmapSettings/tree_contribution_curve_basic.tres");
		DefaultScale = 1.0;
		DefaultPower = 1.0;
		Initialize();
	}

	public TreeHeightmapSettings() : this(WorldGenerationBundle.Singleton!)
	{

	}
	
	public override Double GetValueAt(Double x, Double z)
	{
		if (Noise is not null && Curve is not null && ContributionCurve is not null)
		{
			Double noiseValue = 0.5 + 0.5 * Noise.GetNoise2D((Single)(x / WorldGenerationBundle.WorldGenerationSettings.HorizontalScale), (Single)(z / WorldGenerationBundle.WorldGenerationSettings.HorizontalScale));
			return
				Curve.SampleBaked((Single)Math.Pow(noiseValue,
					Power)); // * ContributionCurve.SampleBaked((Single)noiseValue);
		}
		return 0;
	}
	
	public void LoadFromSaveData(SaveData.TreeHeightmapSettings saveData)
	{
		base.LoadFromSaveData(saveData);
	}
}
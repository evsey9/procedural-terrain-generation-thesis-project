using System;
using Godot;

namespace ProceduralTerrainGenerationThesisProject.Resources.SaveData;

public partial class ContinentalnessHeightmapSettings : HeightmapSettings
{
	public ContinentalnessHeightmapSettings()
	{
		/*
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
		*/
	}
	

	public ContinentalnessHeightmapSettings(Resources.HeightmapSettings heightmapSettings) : base(heightmapSettings)
	{
		
	}
}
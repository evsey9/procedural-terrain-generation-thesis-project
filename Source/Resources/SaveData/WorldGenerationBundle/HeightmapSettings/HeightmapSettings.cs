using System;
using Godot;

namespace ProceduralTerrainGenerationThesisProject.Resources.SaveData;

public partial class HeightmapSettings : Resource
{
	#region Defaults

	[Export]
	public FastNoiseLite? DefaultNoise { get; set; } = new FastNoiseLite();
	
	[Export]
	public Curve? DefaultCurve { get; set; } = new Curve();
	
	[Export]
	public Curve? DefaultContributionCurve { get; set; } = new Curve();
	
	[Export]
	public Double DefaultScale { get; set; } = 1;
	
	[Export]
	public Double DefaultPower { get; set; } = 1;
	
	#endregion

	public HeightmapSettings()
	{
		
	}
	
	public HeightmapSettings(Resources.HeightmapSettings heightmapSettings)
	{
		DefaultNoise = (FastNoiseLite?)heightmapSettings.Noise?.Duplicate();
		if (DefaultNoise is not null)
		{
			DefaultNoise.Frequency /= (Single)heightmapSettings.Scale;
		}
		DefaultCurve = (Curve?)heightmapSettings.Curve?.Duplicate();
		DefaultContributionCurve = (Curve?)heightmapSettings.ContributionCurve?.Duplicate();
		DefaultScale = heightmapSettings.Scale;
		DefaultPower = heightmapSettings.Power;
	}
}
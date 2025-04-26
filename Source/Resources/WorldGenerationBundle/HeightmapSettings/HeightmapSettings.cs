using System;
using Godot;

namespace ProceduralTerrainGenerationThesisProject.Resources;

public partial class HeightmapSettings : Resource
{
	#region Defaults
	
	public FastNoiseLite? DefaultNoise { get; set; }
	public Curve? DefaultCurve { get; set; }
	public Curve? DefaultContributionCurve { get; set; }
	public Double DefaultScale { get; set; } = 1;
	public Double DefaultPower { get; set; } = 1;
	
	#endregion
	
	#region World Generation

	public FastNoiseLite? Noise { get; set; }
	public Curve? Curve { get; set; }
	public Curve? ContributionCurve { get; set; }
	public Double Frequency { get; set; }
	public Double Scale { get; set; }
	public Double Power { get; set; }
	
	#endregion
	
	#region Preview
	
	public FastNoiseLite? DisplayNoise { get; set; }
	public Double DefaultDisplayNoiseScale { get; set; } = 5;
	public Double DisplayNoiseScale { get; set; }

	public Int32 NoiseTextureWidth { get; set; } = 256;
	public Int32 NoiseTextureHeight { get; set; } = 256;
	
	public Curve? PreviewCurve { get; set; }
	public Curve? PreviewContributionCurve { get; set; }

	public NoiseTexture2D NoiseTexture { get; set; } = new NoiseTexture2D();
	public CurveTexture CurveTexture { get; set; } = new CurveTexture();
	public CurveTexture ContributionCurveTexture { get; set; } = new CurveTexture();
	
	#endregion

	#region Signals

	[Signal]
	public delegate void ReloadedEventHandler();

	#endregion
	
	protected WorldGenerationBundle WorldGenerationBundle { get; set; }
	
	public HeightmapSettings(WorldGenerationBundle worldGenerationBundle) : base()
	{
		WorldGenerationBundle = worldGenerationBundle;
		Reset();
	}

	public HeightmapSettings() : this(WorldGenerationBundle.Singleton!)
	{
	}

	public virtual Double GetValueAt(Double x, Double z)
	{
		if (Noise is not null && Curve is not null && ContributionCurve is not null)
		{
			Double noiseValue = 0.5 + 0.5 * Noise.GetNoise2D((Single)x, (Single)z);
			return Curve.SampleBaked((Single)Math.Pow(noiseValue, Power)) * ContributionCurve.SampleBaked((Single)noiseValue);
		}
		return 0;
	}

	public virtual Int32 GetHeightAt(Double x, Double z)
	{
		return (Int32)(GetValueAt(x, z) * WorldGenerationBundle.WorldGenerationSettings.HeightmapRange) + WorldGenerationBundle.WorldGenerationSettings.HeightmapMinY;
	}

	protected virtual void Initialize()
	{
		NoiseTexture.GenerateMipmaps = false;
		NoiseTexture.Normalize = false;
		CurveTexture.TextureMode = CurveTexture.TextureModeEnum.Red;
		ContributionCurveTexture.TextureMode = CurveTexture.TextureModeEnum.Red;
		DisplayNoiseScale = DefaultDisplayNoiseScale;
		Reset();
	}

	public virtual void Reset()
	{
		#region World Generation
		
		Noise = (FastNoiseLite?)DefaultNoise?.Duplicate();
		Curve = (Curve?)DefaultCurve?.Duplicate();
		ContributionCurve = (Curve?)DefaultContributionCurve?.Duplicate();
		
		if (Curve is not null && ContributionCurve is not null)
		{
			Curve.Bake();
			ContributionCurve.Bake();
		}
		
		Scale = DefaultScale;
		Power = DefaultPower;
		Frequency = Noise?.Frequency ?? 1;
		if (Noise is not null)
		{
			Noise.Frequency = (Single)(Scale * Frequency);
		}
		
		#endregion
		
		#region Preview
		
		DisplayNoise = (FastNoiseLite?)DefaultNoise?.Duplicate();
		if (DisplayNoise is not null)
		{
			DisplayNoise.Offset = new Vector3(NoiseTextureWidth / 2.0f, NoiseTextureHeight / 2.0f, 0);
		}

		if (DisplayNoise is not null)
		{
			NoiseTexture.Noise = DisplayNoise;
			UpdateNoiseTexture();
		}
		
		PreviewCurve = (Curve?)DefaultCurve?.Duplicate();
		PreviewContributionCurve = (Curve?)DefaultContributionCurve?.Duplicate();

		if (Curve is not null && PreviewContributionCurve is not null)
		{
			CurveTexture.Curve = PreviewCurve;
			ContributionCurveTexture.Curve = PreviewContributionCurve;
		}
		
		#endregion
		
		ChangeSettings();
		EmitSignal(SignalName.Reloaded);
	}

	public virtual void BakeCurves()
	{
		Curve = (Curve?)PreviewCurve?.Duplicate();
		ContributionCurve = (Curve?)PreviewContributionCurve?.Duplicate();

		Curve?.Bake();
		ContributionCurve?.Bake();
	}
	
	#region Preview
	
	public virtual void UpdateNoiseTexture()
	{
		NoiseTexture.Width = NoiseTextureWidth;
		NoiseTexture.Height = NoiseTextureHeight;
		if (Noise is not null)
		{
			Noise.Frequency = (Single)(Frequency * Scale);
			
		}
		if (Noise is not null && DisplayNoise is not null)
		{
			DisplayNoise.Frequency = (Single)(Frequency * Scale * DisplayNoiseScale);
			DisplayNoise.FractalGain = Noise.FractalGain;
			DisplayNoise.FractalLacunarity = Noise.FractalLacunarity;
			DisplayNoise.FractalOctaves = Noise.FractalOctaves;
			DisplayNoise.FractalWeightedStrength = Noise.FractalWeightedStrength;
		}
	}
	
	#endregion

	public void ChangeSettings()
	{
		EmitSignal(Resource.SignalName.Changed);
	}
	
	public void LoadFromSaveData(SaveData.HeightmapSettings saveData)
	{
		DefaultNoise = (FastNoiseLite?)saveData.DefaultNoise?.Duplicate();
		DefaultCurve = (Curve?)saveData.DefaultCurve?.Duplicate();
		DefaultContributionCurve = (Curve?)saveData.DefaultContributionCurve?.Duplicate();
		DefaultScale = saveData.DefaultScale;
		DefaultPower = saveData.DefaultPower;
		Reset();
	}
}
using System;
using Godot;

namespace ProceduralTerrainGenerationThesisProject.Resources;

public partial class WorldGenerationBundle : Resource
{
	public static WorldGenerationBundle? Singleton { get; set; }
	
	public Resources.WorldGenerationSettings WorldGenerationSettings { get; set; }
	public ContinentalnessHeightmapSettings ContinentalnessHeightmapSettings { get; set; }
	public Continentalness2HeightmapSettings Continentalness2HeightmapSettings { get; set; }
	public ContinentalnessPickerHeightmapSettings ContinentalnessPickerHeightmapSettings { get; set; }
	public ContinentalnessFinalHeightmapSettings ContinentalnessFinalHeightmapSettings { get; set; }
	public WorldGenerationHeightmapSettings WorldGenerationHeightmapSettings { get; set; }
	
	#region Signals

	[Signal]
	public delegate void ReloadedEventHandler();

	#endregion
	
	public WorldGenerationBundle()
	{
		Singleton ??= this;
		
		WorldGenerationSettings = new Resources.WorldGenerationSettings();
		ContinentalnessHeightmapSettings = new ContinentalnessHeightmapSettings(this);
		Continentalness2HeightmapSettings = new Continentalness2HeightmapSettings(this);
		ContinentalnessPickerHeightmapSettings = new ContinentalnessPickerHeightmapSettings(this);
		ContinentalnessFinalHeightmapSettings = new ContinentalnessFinalHeightmapSettings(this);
		WorldGenerationHeightmapSettings = new WorldGenerationHeightmapSettings(this);
		
		WorldGenerationSettings.Changed += ChangeSettings;
		ContinentalnessHeightmapSettings.Changed += ChangeSettings;
		Continentalness2HeightmapSettings.Changed += ChangeSettings;
		ContinentalnessPickerHeightmapSettings.Changed += ChangeSettings;
		ContinentalnessFinalHeightmapSettings.Changed += ChangeSettings;
		WorldGenerationHeightmapSettings.Changed += ChangeSettings;
		
		WorldGenerationSettings.Reloaded += Reload;
		ContinentalnessHeightmapSettings.Reloaded += Reload;
		Continentalness2HeightmapSettings.Reloaded += Reload;
		ContinentalnessPickerHeightmapSettings.Reloaded += Reload;
		ContinentalnessFinalHeightmapSettings.Reloaded += Reload;
		WorldGenerationHeightmapSettings.Reloaded += Reload;
	}
	
	public void ChangeSettings()
	{
		EmitSignal(Resource.SignalName.Changed);
	}
	
	public void Reload()
	{
		EmitSignal(SignalName.Reloaded);
	}
	
	public void Reset(Boolean sendSignal = true)
	{
		WorldGenerationSettings.Reset(sendSignal);
		if (sendSignal)
		{
			EmitSignal(Resource.SignalName.Changed);
		}
	}
	
	public virtual void BakeCurves()
	{
		ContinentalnessHeightmapSettings.BakeCurves();
		Continentalness2HeightmapSettings.BakeCurves();
		ContinentalnessPickerHeightmapSettings.BakeCurves();
		ContinentalnessFinalHeightmapSettings.BakeCurves();
		//WorldGenerationHeightmapSettings.BakeCurves();
	}

	public void LoadFromSaveData(SaveData.WorldGenerationBundle saveData)
	{
		WorldGenerationSettings.LoadFromSaveData(saveData.WorldGenerationSettings);
		ContinentalnessHeightmapSettings.LoadFromSaveData(saveData.ContinentalnessHeightmapSettings);
		Continentalness2HeightmapSettings.LoadFromSaveData(saveData.Continentalness2HeightmapSettings);
		ContinentalnessPickerHeightmapSettings.LoadFromSaveData(saveData.ContinentalnessPickerHeightmapSettings);
		ContinentalnessFinalHeightmapSettings.LoadFromSaveData(saveData.ContinentalnessFinalHeightmapSettings);
		WorldGenerationHeightmapSettings.LoadFromSaveData(saveData.WorldGenerationHeightmapSettings);
		EmitSignal(SignalName.Reloaded);
	}
}
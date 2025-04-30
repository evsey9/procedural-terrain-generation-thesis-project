using System;
using Godot;

namespace ProceduralTerrainGenerationThesisProject.Resources;

public partial class WorldGenerationBundle : Resource
{
	public static WorldGenerationBundle? Singleton { get; set; }
	
	public Resources.WorldGenerationSettings WorldGenerationSettings { get; set; }
	public ContinentalnessHeightmapSettings ContinentalnessHeightmapSettings { get; set; }
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
		WorldGenerationHeightmapSettings = new WorldGenerationHeightmapSettings(this);
		
		WorldGenerationSettings.Changed += ChangeSettings;
		ContinentalnessHeightmapSettings.Changed += ChangeSettings;
		WorldGenerationHeightmapSettings.Changed += ChangeSettings;
		
		WorldGenerationSettings.Reloaded += Reload;
		ContinentalnessHeightmapSettings.Reloaded += Reload;
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
		//WorldGenerationHeightmapSettings.BakeCurves();
	}

	public void LoadFromSaveData(SaveData.WorldGenerationBundle saveData)
	{
		WorldGenerationSettings.LoadFromSaveData(saveData.WorldGenerationSettings);
		ContinentalnessHeightmapSettings.LoadFromSaveData(saveData.ContinentalnessHeightmapSettings);
		WorldGenerationHeightmapSettings.LoadFromSaveData(saveData.WorldGenerationHeightmapSettings);
		EmitSignal(SignalName.Reloaded);
	}
}
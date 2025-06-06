using System;
using Godot;
using ProceduralTerrainGenerationThesisProject.Resources;

namespace ProceduralTerrainGenerationThesisProject.HeightmapProviders;

public partial class TreeHeightmapProvider : HeightmapProvider
{
	public new static TreeHeightmapProvider? Singleton { get; private set; }
	
	public TreeHeightmapProvider() : base()
	{
		Singleton = this;
	}

	public override void _Ready()
	{
		HeightmapSettings = Resources.WorldGenerationBundle.Singleton!.TreeHeightmapSettings;
	}
	
	public static TreeHeightmapProvider GetSingleton(Node sceneNode)
	{
		return sceneNode.GetNode<TreeHeightmapProvider>("/root/TreeHeightmapProvider");
	}
}
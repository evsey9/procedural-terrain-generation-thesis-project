using Godot;

namespace ProceduralTerrainGenerationThesisProject.StructureGeneration;

public class Structure
{
	public Vector3 Offset { get; set; } = new Vector3(0, 0, 0);
	public VoxelBuffer VoxelBuffer { get; set; } = new VoxelBuffer();
}
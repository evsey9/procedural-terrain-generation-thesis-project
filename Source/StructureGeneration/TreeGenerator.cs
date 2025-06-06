using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using ProceduralTerrainGenerationThesisProject.VoxelGeneratorScripts;

namespace ProceduralTerrainGenerationThesisProject.StructureGeneration;

public class TreeGenerator
{
	public Int32 TrunkLengthMin { get; set; } = 6;
	public Int32 TrunkLengthMax { get; set; } = 15;
	private RandomNumberGenerator randomNumberGenerator;

	public WorldVoxelGenerator.Blocks LogType { get; set; } =
		WorldVoxelGenerator.Blocks.TreeTrunk;
	
	public WorldVoxelGenerator.Blocks LeavesType { get; set; } =
		WorldVoxelGenerator.Blocks.TreeLeaves;

	public VoxelBuffer.ChannelId Channel { get; set; } = VoxelBuffer.ChannelId.ChannelType;

	public TreeGenerator(RandomNumberGenerator randomNumberGenerator)
	{
		this.randomNumberGenerator = randomNumberGenerator;
	}

	public Structure Generate()
	{
		Dictionary<Vector3, WorldVoxelGenerator.Blocks> voxels = new();
		
		// Trunk
		
		Int32 trunkLength = randomNumberGenerator.RandiRange(TrunkLengthMin, TrunkLengthMax);

		for (Int32 y = 0; y < trunkLength; y++)
		{
			voxels[new Vector3(0, y, 0)] = LogType;
		}
		
		Int32 branchesStart = randomNumberGenerator.RandiRange(trunkLength / 3, trunkLength / 2);
		for (Int32 y = branchesStart; y < trunkLength; y++)
		{
			Double t = (Double)(y - branchesStart) / trunkLength;
			Double branchChance = 1.0 - Math.Pow(t - 0.5, 2);
			if (randomNumberGenerator.Randf() < branchChance)
			{
				Int32 branchLength = (Int32)((trunkLength / 2.0) * branchChance * randomNumberGenerator.Randf());
				Vector3 position = new Vector3(0, y, 0);
				Double angle = randomNumberGenerator.RandfRange(-((Single)Math.PI), (Single)Math.PI);
				Vector3 direction = new Vector3((Single)Math.Sin(angle), 0.45f, (Single)Math.Cos(angle));
				for (Int32 i = 0; i < branchLength; i++)
				{
					position += direction;
					Vector3 integerPosition = position.Round();
					voxels[integerPosition] = LogType;
				}
			}
		}
		
		// Leaves

		Vector3[] logPositions = voxels.Keys.ToArray();
		
		foreach (Vector3 logPos in logPositions)
		{
			Vector3 leafpos = logPos + new Vector3(0, 1, 0);
			if (!voxels.ContainsKey(leafpos))
			{
				voxels.TryAdd(leafpos, LeavesType);
			}
		}

		Random random = new Random((Int32)randomNumberGenerator.Seed);
		random.Shuffle<Vector3>(logPositions);

		Int32 leafCount = (Int32)(0.75 * logPositions.Length);
		Array.Resize(ref logPositions, leafCount);

		Vector3[] dirs = new Vector3[]
		{
			new Vector3(-1, 0, 0),
			new Vector3(1, 0, 0),
			new Vector3(0, 0, -1),
			new Vector3(0, 0, 1),
			new Vector3(0, 1, 0),
			new Vector3(0, -1, 0)
		};

		for (Int32 c = 0; c < leafCount; c++)
		{
			Vector3 position = logPositions[c];
			if (position.Y < branchesStart)
			{
				continue;
			}

			for (Int32 di = 0; di < dirs.Length; di++)
			{
				Vector3 npos = position + dirs[di];
				if (!voxels.ContainsKey(npos))
				{
					voxels.TryAdd(npos, LeavesType);
				}
			}
		}
		
		// Make structure
		
		Aabb aabb = new Aabb();
		foreach (Vector3 pos in voxels.Keys)
		{
			aabb = aabb.Expand(pos);
		}
		
		Structure structure = new Structure();
		structure.Offset = -aabb.Position;

		VoxelBuffer voxelBuffer = structure.VoxelBuffer;
		voxelBuffer.Create((Int32)(aabb.Size.X) + 1, (Int32)(aabb.Size.Y) + 1, (Int32)(aabb.Size.Z) + 1);

		foreach (Vector3 pos in voxels.Keys)
		{
			Vector3 rpos = pos + structure.Offset;
			WorldVoxelGenerator.Blocks v = voxels[pos];
			voxelBuffer.SetVoxel((UInt32)v, (Int32)Math.Round(rpos.X), (Int32)Math.Round(rpos.Y), (Int32)Math.Round(rpos.Z));
		}

		return structure;
	}
}
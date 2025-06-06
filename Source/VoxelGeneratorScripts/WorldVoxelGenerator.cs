using System;
using System.Collections.Generic;
using Godot;
using ProceduralTerrainGenerationThesisProject.HeightmapProviders;
using ProceduralTerrainGenerationThesisProject.Singletons;
using ProceduralTerrainGenerationThesisProject.StructureGeneration;

namespace ProceduralTerrainGenerationThesisProject.VoxelGeneratorScripts;

public partial class WorldVoxelGenerator : VoxelGeneratorScript
{
	private WorldGenerationSettingsProvider? worldGenerationSettingsProvider;
	private WorldHeightmapProvider? worldHeightmapProvider;
	private TreeHeightmapProvider? treeHeightmapProvider;
	public Boolean WorldGenerationEnabled { get; set; } = true;
	private IList<Structure> treeStructures = new List<Structure>();
	public RandomNumberGenerator RandomNumberGeneratorSingleton = new RandomNumberGenerator();
	
	public enum Blocks
	{
		Air = 0,
		Stone = 1,
		Grass = 2,
		FullWater = 3,
		TreeTrunk = 4,
		TreeLeaves = 5,
		Sand = 6
	}
	
	private readonly Vector3[] mooreDirs = new Vector3[]
	{
		new Vector3(-1, 0, -1),
		new Vector3(0, 0, -1),
		new Vector3(1, 0, -1),
		new Vector3(-1, 0, 0),
		new Vector3(1, 0, 0),
		new Vector3(-1, 0, 1),
		new Vector3(0, 0, 1),
		new Vector3(1, 0, 1)
	};

	private const VoxelBuffer.ChannelId Channel = VoxelBuffer.ChannelId.ChannelType;

	public WorldVoxelGenerator() : base()
	{
		RandomNumberGeneratorSingleton.Seed = 0;
		TreeGenerator treeGenerator = new TreeGenerator(RandomNumberGeneratorSingleton);
		treeGenerator.LogType = Blocks.TreeTrunk;
		treeGenerator.LeavesType = Blocks.TreeLeaves;
		for (int i = 0; i < 4096; i++)
		{
			Structure structure = treeGenerator.Generate();
			treeStructures.Add(structure);
		}
	}
	
	public override void _GenerateBlock(VoxelBuffer outBuffer, Vector3I originInVoxels, Int32 lod)
	{
		if (WorldGenerationSettingsProvider.Singleton is null || WorldHeightmapProvider.Singleton is null || TreeHeightmapProvider.Singleton is null)
		{
			return;
		}

		worldGenerationSettingsProvider ??= WorldGenerationSettingsProvider.Singleton;
		worldHeightmapProvider ??= WorldHeightmapProvider.Singleton;
		treeHeightmapProvider ??= TreeHeightmapProvider.Singleton;

		if (!WorldGenerationEnabled)
		{
			return;
		}
		//base._GenerateBlock(outBuffer, originInVoxels, lod);
		
		//worldGenerationSettingsProvider.UpdateWorldGenerationInProgress();
		
		Int32 blockSize = outBuffer.GetSize().X;

		Int32 originY = originInVoxels.Y;
		
		Vector3 chunkPosition = new Vector3(originInVoxels.X >> 4, originInVoxels.Y >> 4, originInVoxels.Z >> 4);

		if (originInVoxels.Y > worldGenerationSettingsProvider.Resource.WorldGenerationSettings.HeightmapMaxY)
		{
			//outBuffer.Fill((Int32)Blocks.Air, (Int32)Channel);
		}
		else if (originInVoxels.Y + blockSize < worldGenerationSettingsProvider.Resource.WorldGenerationSettings.HeightmapMinY)
		{
			outBuffer.Fill((Int32)Blocks.Stone, (Int32)Channel);
		}
		else
		{
			RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();
			randomNumberGenerator.Seed = GetChunkSeed2d(chunkPosition);

			Int64 groundX;
			Int64 groundZ = originInVoxels.Z;

			for (Int32 z = 0; z < blockSize; z++)
			{
				groundX = originInVoxels.X;

				for (Int32 x = 0; x < blockSize; x++)
				{
					//Tuple<Int64, Double> tuple = GetHeightValueTupleAt(groundX, groundZ);
					//Int64 height = tuple.Item1;
					//Double heightValue = tuple.Item2;
					Int64 height = GetHeightAt(groundX, groundZ);
					Double heightValue = GetValueFromHeight(height) ?? 0;
					
					Int64 relativeHeight = height - originY;
					Double sandProbabilityRandomNumber = randomNumberGenerator.Randf();
					//Double stoneProbabilityRandomNumber = randomNumberGenerator.Randf();

					WorldVoxelGenerator.Blocks blockToPlace = Blocks.Grass;
					WorldVoxelGenerator.Blocks altBlockToPlace = Blocks.Stone;

					if (sandProbabilityRandomNumber < GetSandValueAt(heightValue))
					{
						blockToPlace = Blocks.Sand;
						altBlockToPlace = Blocks.Sand;
					} 
					/*else if (stoneProbabilityRandomNumber < GetStoneValueAt(heightValue))
					{
						blockToPlace = Blocks.Stone;
					}*/
					
					// Dirt and grass

					if (relativeHeight > blockSize)
					{
						outBuffer.FillArea((UInt64)altBlockToPlace, new Vector3I(x, 0, z), new Vector3I(x + 1, blockSize, z + 1), (Int32)Channel);
					}
					else if (relativeHeight > 0)
					{
						outBuffer.FillArea((UInt64)altBlockToPlace, new Vector3I(x, 0, z), new Vector3I(x + 1, (Int32)relativeHeight, z + 1), (Int32)Channel);
						if (height >= worldGenerationSettingsProvider.Resource.WorldGenerationSettings.SeaLevelY)
						{
							outBuffer.SetVoxel((UInt64)blockToPlace, x, (Int32)relativeHeight - 1, z, (Int32)Channel);
							if (relativeHeight < blockSize && randomNumberGenerator.Randf() < 0.2)
							{
								// Trees
							}
						}
					}
					
					// Water
					if (height < worldGenerationSettingsProvider.Resource.WorldGenerationSettings.SeaLevelY && originY < worldGenerationSettingsProvider.Resource.WorldGenerationSettings.SeaLevelY)
					{
						Int64 startRelativeHeight = 0;
						if (relativeHeight > 0)
						{
							startRelativeHeight = relativeHeight;
						}
						/*outBuffer.FillArea((Int32)Blocks.FullWater, new Vector3I(x, (Int32)startRelativeHeight, z), new Vector3I(x + 1, blockSize, z + 1), (Int32)Channel);
						if (originY + blockSize == worldGenerationSettingsProvider.Resource.WorldGenerationSettings.SeaLevelY)
						{
							outBuffer.SetVoxel((Int32)Blocks.FullWater, x, blockSize - 1, z, (Int32)Channel);
						}*/
					}

					groundX += 1;
				}

				groundZ += 1;
			}
		}
		
		// Trees

		if (originInVoxels.Y <= worldGenerationSettingsProvider.Resource.WorldGenerationSettings.TreesMaxY &&
		    originInVoxels.Y + blockSize >= worldGenerationSettingsProvider.Resource.WorldGenerationSettings.TreesMinY)
		{
			VoxelTool voxelTool = outBuffer.GetVoxelTool();
			List<Tuple<Vector3, Structure>> structureInstances = new List<Tuple<Vector3, Structure>>();
			
			structureInstances.AddRange(getTreeInstancesInChunk(chunkPosition, originInVoxels, blockSize));
			
			Aabb blockAabb = new Aabb(new Vector3(), outBuffer.GetSize() + new Vector3I(1, 1, 1));

			foreach (Vector3 dir in mooreDirs)
			{
				Vector3 ncpos = (chunkPosition + dir).Round();
				structureInstances.AddRange(getTreeInstancesInChunk(ncpos, originInVoxels, blockSize));
			}

			foreach (Tuple<Vector3, Structure> structureTuple in structureInstances)
			{
				Vector3 pos = structureTuple.Item1;
				Structure structure = structureTuple.Item2;
				Vector3 lowerCornerPos = pos - structure.Offset;
				Aabb aabb = new Aabb(lowerCornerPos, structure.VoxelBuffer.GetSize() + new Vector3I(1, 1, 1));

				if (aabb.Intersects(blockAabb))
				{
					voxelTool.PasteMasked(
						(Vector3I)lowerCornerPos,
						structure.VoxelBuffer,
						1 << (Int32)VoxelBuffer.ChannelId.ChannelType,
						(Int32)VoxelBuffer.ChannelId.ChannelType,
						(Int32)Blocks.Air
						);
				}
			}
		}
	}

	private static UInt64 GetChunkSeed2d(Vector3 chunkPosition)
	{
		//return (UInt64)chunkPosition.X ^ (31 * (UInt64)chunkPosition.Z);
		return (UInt64)HashCode.Combine(chunkPosition.X, chunkPosition.Z);
	}
	
	private static UInt64 GetTreeChunkSeed2d(Vector3 chunkPosition)
	{
		//Vector3 twoDimensionalChunk = new Vector3(chunkPosition.X, 0, chunkPosition.Z);
		return (UInt64)HashCode.Combine(chunkPosition.X, chunkPosition.Z);
		UInt64 output = ((UInt64)chunkPosition.X * (UInt64)chunkPosition.Z) * ((UInt64)chunkPosition.X << 4) * ((UInt64)chunkPosition.Z << 8);
		//GD.Print($"for chunk position ${chunkPosition} seed is {output}");
		return output;
	}

	private Int64 GetHeightAt(Int64 x, Int64 z)
	{
		return worldHeightmapProvider?.HeightmapSettings.GetHeightAt(x, z) ?? 0;
	}
	
	private Tuple<Int64, Double> GetHeightValueTupleAt(Int64 x, Int64 z)
	{
		return worldHeightmapProvider?.HeightmapSettings.GetHeightValueTupleAt(x, z) ?? new Tuple<Int64, Double>(0, 0);
	}
	
	private Int64 GetTreeAmountAt(Int64 x, Int64 z)
	{
		Double treeValue = treeHeightmapProvider?.HeightmapSettings.GetValueAt(x, z) ?? 0;
		return (Int64)Math.Round(
			   worldGenerationSettingsProvider?.Resource.WorldGenerationSettings.TreesMinAmount +
	           treeValue *
	           (worldGenerationSettingsProvider?.Resource.WorldGenerationSettings.TreesMaxAmount -
	            worldGenerationSettingsProvider?.Resource.WorldGenerationSettings.TreesMinAmount) ?? 0);
	}

	private Double? GetValueFromHeight(Int64 height)
	{
		return (Double?)(height - worldGenerationSettingsProvider?.Resource.WorldGenerationSettings.HeightmapMinY) /
		       worldGenerationSettingsProvider?.Resource.WorldGenerationSettings.HeightmapRange;
	}
	
	private Double GetSandValueAt(Double heightValue)
	{
		return worldGenerationSettingsProvider?.Resource.WorldGenerationSettings.SandCurve?.Sample((Single)heightValue) ?? 0;
	}
	
	private Double GetStoneValueAt(Double heightValue)
	{
		return worldGenerationSettingsProvider?.Resource.WorldGenerationSettings.StoneCurve?.Sample((Single)heightValue) ?? 0;
	}

	private IList<Tuple<Vector3, Structure>> getTreeInstancesInChunk(Vector3 cpos, Vector3 offset, Int32 chunkSize)
	{
		RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();
		randomNumberGenerator.Seed = GetTreeChunkSeed2d(cpos);
		List<Tuple<Vector3, Structure>> treeInstances = new List<Tuple<Vector3, Structure>>();
		for (Int32 i = 0; i < GetTreeAmountAt((Int64)cpos.X * chunkSize, (Int64)cpos.Z * chunkSize); i++)
		{
			Vector3 pos = new Vector3(randomNumberGenerator.Randi() % chunkSize, 0, randomNumberGenerator.Randi() % chunkSize);
			pos += cpos * chunkSize;
			pos.Y = GetHeightAt((Int64)pos.X, (Int64)pos.Z);

			if (pos.Y > worldGenerationSettingsProvider?.Resource.WorldGenerationSettings.TreesMinY)
			{
				pos -= offset;
				Int64 si = randomNumberGenerator.Randi() % treeStructures.Count;
				treeInstances.Add(new Tuple<Vector3, Structure>(pos.Round(), treeStructures[(Int32)si]));
			}
		}

		return treeInstances;
	}
}
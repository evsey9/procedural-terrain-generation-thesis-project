using System;
using Godot;
using ProceduralTerrainGenerationThesisProject.HeightmapProviders;
using ProceduralTerrainGenerationThesisProject.Singletons;

namespace ProceduralTerrainGenerationThesisProject.VoxelGeneratorScripts;

public partial class WorldVoxelGenerator : VoxelGeneratorScript
{
	private WorldGenerationSettingsProvider? worldGenerationSettingsProvider;
	private WorldHeightmapProvider? worldHeightmapProvider;
	public Boolean WorldGenerationEnabled { get; set; } = true;

	private enum Blocks
	{
		Air = 0,
		Stone = 1,
		Grass = 2,
		FullWater = 3,
		TreeTrunk = 4,
		TreeLeaves = 5,
		Sand = 6
	}

	private const VoxelBuffer.ChannelId Channel = VoxelBuffer.ChannelId.ChannelType;

	public WorldVoxelGenerator() : base()
	{
		
	}
	
	public override void _GenerateBlock(VoxelBuffer outBuffer, Vector3I originInVoxels, Int32 lod)
	{
		if (WorldGenerationSettingsProvider.Singleton is null || WorldHeightmapProvider.Singleton is null)
		{
			return;
		}

		worldGenerationSettingsProvider ??= WorldGenerationSettingsProvider.Singleton;
		worldHeightmapProvider ??= WorldHeightmapProvider.Singleton;

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
			outBuffer.Fill((Int32)Blocks.Air, (Int32)Channel);
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
	}

	private static UInt64 GetChunkSeed2d(Vector3 chunkPosition)
	{
		return (UInt64)chunkPosition.X ^ (31 * (UInt64)chunkPosition.Z);
	}

	private Int64 GetHeightAt(Int64 x, Int64 z)
	{
		return worldHeightmapProvider?.HeightmapSettings.GetHeightAt(x, z) ?? 0;
	}
	
	private Tuple<Int64, Double> GetHeightValueTupleAt(Int64 x, Int64 z)
	{
		return worldHeightmapProvider?.HeightmapSettings.GetHeightValueTupleAt(x, z) ?? new Tuple<Int64, Double>(0, 0);
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
}
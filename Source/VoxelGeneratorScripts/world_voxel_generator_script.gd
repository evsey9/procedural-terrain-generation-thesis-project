extends VoxelGeneratorScript

# TODO Don't hardcode, get by name from library somehow
const AIR = 0
const STONE = 1
const GRASS = 2
const WATER_FULL = 3

const _CHANNEL = VoxelBuffer.CHANNEL_TYPE

func _generate_block(buffer: VoxelBuffer, origin_in_voxels: Vector3i, lod: int) -> void:
	# Assuming input is cubic in our use case (it doesn't have to be!)
	var block_size := int(buffer.get_size().x)
	var origin_y := origin_in_voxels.y
	# TODO This hardcodes a cubic block size of 16, find a non-ugly way...
	# Dividing is a false friend because of negative values
	var chunk_pos := Vector3(
		origin_in_voxels.x >> 4,
		origin_in_voxels.y >> 4,
		origin_in_voxels.z >> 4)

	# Ground

	if origin_in_voxels.y > HeightProvider.heightmap_max_y:
		buffer.fill(AIR, _CHANNEL)

	elif origin_in_voxels.y + block_size < HeightProvider.heightmap_min_y:
		buffer.fill(STONE, _CHANNEL)

	else:
		var rng := RandomNumberGenerator.new()
		rng.seed = _get_chunk_seed_2d(chunk_pos)
		
		var ground_x : int
		var ground_z := origin_in_voxels.z

		for z in block_size:
			ground_x = origin_in_voxels.x

			for x in block_size:
				var height := _get_height_at(ground_x, ground_z)
				var relative_height := height - origin_y
				
				# Dirt and grass
				if relative_height > block_size:
					buffer.fill_area(STONE,
						Vector3(x, 0, z), Vector3(x + 1, block_size, z + 1), _CHANNEL)
				elif relative_height > 0:
					buffer.fill_area(STONE,
						Vector3(x, 0, z), Vector3(x + 1, relative_height, z + 1), _CHANNEL)
					if height >= HeightProvider.sea_level_y:
						buffer.set_voxel(GRASS, x, relative_height - 1, z, _CHANNEL)
						if relative_height < block_size and rng.randf() < 0.2:
							pass
							#var foliage = TALL_GRASS
							#if rng.randf() < 0.1:
							#	foliage = DEAD_SHRUB
							#buffer.set_voxel(foliage, x, relative_height, z, _CHANNEL)
				
				# Water
				if height < HeightProvider.sea_level_y and origin_y < HeightProvider.sea_level_y:
					var start_relative_height := 0
					if relative_height > 0:
						start_relative_height = relative_height
					buffer.fill_area(WATER_FULL,
						Vector3(x, start_relative_height, z), 
						Vector3(x + 1, block_size, z + 1), _CHANNEL)
					if origin_y + block_size == HeightProvider.sea_level_y:
						# Surface block TODO fix
						buffer.set_voxel(WATER_FULL, x, block_size - 1, z, _CHANNEL)
						
				ground_x += 1

			ground_z += 1
	
	buffer.compress_uniform_channels()


static func _get_chunk_seed_2d(cpos: Vector3) -> int:
	return int(cpos.x) ^ (31 * int(cpos.z))


func _get_height_at(x: int, z: int) -> int:
	#var t = 0.5 + 0.5 * _heightmap_noise.get_noise_2d(x, z)
	#return int(HeightmapCurve.sample_baked(t))
	return HeightProvider.get_height_at(x, z)

func _get_used_channels_mask() -> int:
	return 1 << _CHANNEL

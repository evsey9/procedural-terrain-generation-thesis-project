extends Node

var default_sea_level_y = 64
var default_heightmap_min_y := 0
var default_heightmap_max_y := 128
var default_horizontal_scale := 1.0

var sea_level_y = default_sea_level_y

var heightmap_min_y := default_heightmap_min_y
var heightmap_max_y := default_heightmap_max_y
var heightmap_range : int : 
	get:
		return heightmap_max_y - heightmap_min_y

var horizontal_scale := default_horizontal_scale

var default_trees_min_y := 0
var default_trees_max_y := 0

var trees_min_y := default_trees_min_y
var trees_max_y := default_trees_max_y

func reset():
	sea_level_y = default_sea_level_y
	heightmap_min_y = default_heightmap_min_y
	heightmap_max_y = default_heightmap_max_y
	horizontal_scale = default_horizontal_scale
	trees_min_y = default_trees_min_y
	trees_max_y = default_trees_max_y

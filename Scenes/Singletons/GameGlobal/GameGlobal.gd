extends Node
var in_game = false
var world_gridmap : GridMap

# Worldgen stuff
const CHUNK_SIZE = 16
var CHUNK_RENDER_DISTANCE = 8

const MAX_HEIGHT = 255

# Item stuff
const STACK_SIZE_NORMAL = 64
const STACK_SIZE_SMALL = 16
var ITEM_ID_DICT = {}

# Called when the node enters the scene tree for the first time.
func _ready():
	initialize_game()
	

func initialize_game(): # Call when starting the game
	print("Initializing game")
	get_tree().paused = true
	if not in_game:
		in_game = true
		#world_gridmap = get_tree().get_nodes_in_group("world_gridmap")[0]
	get_tree().paused = false

func pos_to_grid_pos(position: Vector3):
	var gridpos = position
	gridpos = gridpos / 1 #world_gridmap.cell_size
	gridpos.x = int(floor(gridpos.x))
	gridpos.y = int(floor(gridpos.y))
	gridpos.z = int(floor(gridpos.z))
	return gridpos

func pos_to_chunk_coord(position: Vector3):
	var chunkpos = pos_to_grid_pos(position)
	chunkpos = Vector2(chunkpos.x, chunkpos.z)
	chunkpos.x = int(floor(chunkpos.x / CHUNK_SIZE))
	chunkpos.y = int(floor(chunkpos.y / CHUNK_SIZE))
	return chunkpos

extends GridMap

var scheduled_tiles = []
var tiles_per_tick = 60
var mutex
var threads = []
# Declare member variables here. Examples:
# var a = 2
# var b = "text"
#var threads = []
# Called when the node enters the scene tree for the first time.
func _ready():
	add_to_group("world_gridmap")
	mutex = Mutex.new()

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta):
	if scheduled_tiles:
		var to_slice = tiles_per_tick
		if to_slice > len(scheduled_tiles):
			to_slice = len(scheduled_tiles)
		var tiles = scheduled_tiles.slice(0, to_slice)
		for i in range(to_slice):
			scheduled_tiles.pop_front()
		var thread = Thread.new()
		threads.append(thread)
		thread.start(Callable(self, "place_tiles"), tiles)
		#for i in range(tiles_per_tick):
		#	var tile = scheduled_tiles.pop_front()
		#	if tile:
		#		set_cell_item(tile[0].x, tile[0].y, tile[0].z, tile[1], tile[2])

func place_tiles(tiles):
	print("placing tiles")
	mutex.try_lock()
	for i in range(len(tiles)):
		var tile = tiles[i]
		set_cell_item(Vector3i(tile[0].x, tile[0].y, tile[0].z), tile[1], tile[2])
	mutex.unlock()

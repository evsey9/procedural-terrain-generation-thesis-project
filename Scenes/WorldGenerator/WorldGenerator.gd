extends Node3D
@export var enabled = true

var CHUNK_SIZE = GameGlobal.CHUNK_SIZE
var GENERATOR_DISTANCE = GameGlobal.CHUNK_RENDER_DISTANCE
const THREADS_PER_CHUNK = 1 # Should be a square and divisible by 2

@export var gridmap : NodePath
@onready var player = get_tree().get_nodes_in_group("player")[0]

# Memory variables
var mutex : Mutex = Mutex.new()
var generated_chunks = {}
var pending_chunks = {}

var chunk_generator = preload("res://Scenes/WorldGenerator/ChunkGenerator/ChunkGenerator.tscn")
var generator_instance

var noise = FastNoiseLite.new()

# Generator options
var BASE_HEIGHT = 0

# Called when the node enters the scene tree for the first time.
func _ready():
	randomize()
	return
	noise.noise_type = FastNoiseLite.TYPE_SIMPLEX_SMOOTH
	noise.seed = randi()
	noise.fractal_octaves = 12
	noise.fractal_gain = 0.4
	#noise.fractal_gain = 0.5
	#noise.fractal_lacunarity = 2
	# noise.octaves = 128
	#noise.period = 42.0
	#noise.persistence = 0.1
	#noise.frequency = 1 / 42
	noise.frequency = 1 / 62.0
	generator_instance = chunk_generator.instantiate()
	var generator_position = Vector3(0, GameGlobal.MAX_HEIGHT, 0)
	generator_instance.mutex = mutex
	generator_instance.transform.origin = generator_position
	generator_instance.gridmap = get_node(gridmap)
	generator_instance.noise = noise
	generator_instance.BASE_HEIGHT = BASE_HEIGHT
	generator_instance.THREADS_PER_CHUNK = THREADS_PER_CHUNK
	get_parent().call_deferred("add_child", generator_instance)
	#print("chuunkgen starting?")
	if enabled:
		$ChunkCheckTimer.start()

func _physics_process(delta):
	#generator_instance.post_semaphore(1)
	#generate_chunks()
	pass

func _on_ChunkCheckTimer_timeout():
	#generate_chunks()
	pass
	#print("generated chunks")

func generate_chunks():
	var chunks_to_check = {}
	var player_chunk = GameGlobal.pos_to_chunk_coord(player.transform.origin)
	# print(player_chunk)
	chunks_to_check[player_chunk] = true
	for i in range(1, GENERATOR_DISTANCE + 1):
		#print("i: ", i)
		var a = range(-i, i + 1)
		#print("forward row: ", a)
		for x in a:
			#print("x: ", x)
			chunks_to_check[player_chunk + Vector2(x, -i)] = true
		
		var b = range(-i + 1, i + 1)
		#print("right column: ", b)
		for y in b:
			#print("y: ", y)
			chunks_to_check[player_chunk + Vector2(i, y)] = true
		
		var c = range(i - 1, -i - 1, -1)
		#print("backward row: ", c)
		for x in c:
			#print("x: ", x)
			chunks_to_check[player_chunk + Vector2(x, i)] = true
		
		var d = range(i, -i, -1)
		#print("left column: ", d)
		for y in d:
			#print("y: ", y)
			chunks_to_check[player_chunk + Vector2(-i, y)] = true
	
	#print("Checking chunks len: ", len(chunks_to_check))
	#print("Checking chunks: ", chunks_to_check.keys())
	for chunk in chunks_to_check.keys():
		if not generated_chunks.has(chunk):
			var heightmap = []
			for y in range(GameGlobal.CHUNK_SIZE):
				heightmap.append([])
				for x in range(GameGlobal.CHUNK_SIZE):
					var h = BASE_HEIGHT
					var vec = Vector2(chunk.x * GameGlobal.CHUNK_SIZE + x, chunk.y * GameGlobal.CHUNK_SIZE + y)
					h += round(noise.get_noise_2dv(vec) * 10)
					heightmap[y].append(h)
			generated_chunks[chunk] = true
			mutex.lock()
			generator_instance.pending_chunks.append(chunk)
			generator_instance.pending_heightmaps.append(heightmap)
			mutex.unlock()
	generator_instance.post_semaphore(0)


func _on_ChunkGenTimer_timeout():
	#generator_instance.post_semaphore(2)
	pass

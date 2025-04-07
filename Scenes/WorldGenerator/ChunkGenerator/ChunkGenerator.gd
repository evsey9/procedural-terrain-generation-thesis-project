extends Node3D

# Variables that are passed by WORLD_GENERATOR
var noise: FastNoiseLite
var gridmap: GridMap
var BASE_HEIGHT = 0
var THREADS_PER_CHUNK = 1

# Everything else
var mutex : Mutex
var semaphores = [Semaphore.new(), Semaphore.new(), Semaphore.new()]
var threads = []
var chunk
var heightmap
var pending_chunks = []
var pending_heightmaps = []
# Called when the node enters the scene tree for the first time.

func _physics_process(delta):
	var inactive_threads = 0
	#for thread in threads:
	#	if not thread.is_active():
	#		inactive_threads += 1
	#if inactive_threads == THREADS_PER_CHUNK:
	#	queue_free()

func _ready():
	print("starting chunkgen")
	if not gridmap:
		gridmap = get_tree().get_nodes_in_group("world_gridmap")[0]
	#chunk = GameGlobal.pos_to_chunk_coord(translation)
	#mutex = Mutex.new()
	#semaphore = Semaphore.new()
	for i in range(THREADS_PER_CHUNK):
		var curthread = Thread.new()
		threads.append(curthread)
		curthread.start(Callable(self, "_chunk_thread").bind(i), 2)
		print("thread", i, "started")
	#_chunk_thread(0)
	#queue_free()
	
func _chunk_thread(num):
	#print("thread", num)
	var a = range(num / int(sqrt(THREADS_PER_CHUNK)), GameGlobal.CHUNK_SIZE / int(sqrt(THREADS_PER_CHUNK)) * (num / int(sqrt(THREADS_PER_CHUNK)) + 1))
	var b = range(num % int(sqrt(THREADS_PER_CHUNK)), GameGlobal.CHUNK_SIZE / int(sqrt(THREADS_PER_CHUNK)) * (num % int(sqrt(THREADS_PER_CHUNK)) + 1))
	while true:
		#print("check")
		if chunk != null:
			#print("chunk not null")
			for i in a:
				for j in b:
					#semaphores[1].wait()
					#while mutex.try_lock() == ERR_BUSY:
					#	mutex.try_lock()
					#mutex.lock()
					#print("setting", i, " ", j)
					var h = 0
					#h = BASE_HEIGHT
					h += heightmap[i][j]
					Callable(gridmap, "set_cell_item").bind(Vector3i(chunk.x * GameGlobal.CHUNK_SIZE + j, h, chunk.y * GameGlobal.CHUNK_SIZE + i), 0).call_deferred()
					#gridmap.set_cell_item(Vector3i(chunk.x * GameGlobal.CHUNK_SIZE + j, h, chunk.y * GameGlobal.CHUNK_SIZE + i), 0)
					#print("setting height ", h)
					for y in range(-10, h):
						Callable(gridmap, "set_cell_item").bind(Vector3i(chunk.x * GameGlobal.CHUNK_SIZE + j, y, chunk.y * GameGlobal.CHUNK_SIZE + i), 2).call_deferred()
						#gridmap.set_cell_item(Vector3i(chunk.x * GameGlobal.CHUNK_SIZE + j, y, chunk.y * GameGlobal.CHUNK_SIZE + i), 2)
					mutex.unlock()
			#print("next chunk")
			#if false or pending_chunks:
			#	mutex.lock()
			#	chunk = pending_chunks.pop_front()
			#	mutex.unlock()
			#else:
			#	chunk = null
			chunk = null
		else:
			#print("waiting for sem")
			semaphores[2].wait()
			if pending_chunks:
				mutex.lock()
				chunk = pending_chunks.pop_front()
				heightmap = pending_heightmaps.pop_front()
				mutex.unlock()
			#print()
	print("finished")

func _exit_tree():
	for thread in threads:
		thread.wait_to_finish()
		print("finished thread")

func post_semaphore(sem):
	semaphores[sem].post()
	#print("sem", semaphore.post())

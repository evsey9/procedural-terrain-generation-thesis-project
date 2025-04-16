class_name ImageFunctions
extends Node

static func convert_to_matrix(image: Image) -> Array:
	if image.get_format() != Image.FORMAT_L8:
		image.convert(Image.FORMAT_L8)
	var array = []
	var image_width := image.get_width()
	var image_height := image.get_height()
	for y in range(image_height):
		array.push_back([])
		for x in range(image_width):
			array[y].push_back(image.get_pixel(x, y).r)
	return array

static func matrix_to_image(input_array: Array) -> Image:
	var input_array_width := len(input_array[0])
	var input_array_height := len(input_array)
	var image = Image.create_empty(input_array_width, input_array_height, false, Image.FORMAT_L8)
	for y in range(input_array_height):
		for x in range(input_array_width):
			var value = input_array[y][x]
			image.set_pixel(x, y, Color(value, value, value))
	return image

static func matrix_power(input_array: Array, power: float) -> Array:
	var array = []
	var input_array_width := len(input_array[0])
	var input_array_height := len(input_array)
	for y in range(input_array_height):
		array.push_back([])
		for x in range(input_array_width):
			array[y].push_back(input_array[y][x] ** power)
	return array

static func matrix_add(input_array_1: Array, input_array_2: Array) -> Array:
	var array = []
	var input_array_width := len(input_array_1[0])
	var input_array_height := len(input_array_1)
	for y in range(input_array_height):
		array.push_back([])
		for x in range(input_array_width):
			array[y].push_back(input_array_1[y][x] + input_array_2[y][x])
	return array

static func matrix_sub(input_array_1: Array, input_array_2: Array) -> Array:
	var array = []
	var input_array_width := len(input_array_1[0])
	var input_array_height := len(input_array_1)
	for y in range(input_array_height):
		array.push_back([])
		for x in range(input_array_width):
			array[y].push_back(input_array_1[y][x] - input_array_2[y][x])
	return array

static func matrix_mult(input_array_1: Array, input_array_2: Array) -> Array:
	var array = []
	var input_array_width := len(input_array_1[0])
	var input_array_height := len(input_array_1)
	for y in range(input_array_height):
		array.push_back([])
		for x in range(input_array_width):
			array[y].push_back(input_array_1[y][x] * input_array_2[y][x])
	return array

static func matrix_apply(input_array: Array, callable: Callable) -> Array:
	var array = []
	var input_array_width := len(input_array[0])
	var input_array_height := len(input_array)
	for y in range(input_array_height):
		array.push_back([])
		for x in range(input_array_width):
			var a = input_array[y][x]
			var b = callable.call(a)
			array[y].push_back(b)
	return array

static func matrix_apply_baked_curve(input_array: Array, curve: Curve) -> Array:
	return matrix_apply(input_array, func (x): curve.sample_baked(x))

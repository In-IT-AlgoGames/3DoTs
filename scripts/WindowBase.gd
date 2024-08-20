extends Control
var start: Vector2 
var initialPosition: Vector2 

var is_moving := false

func _input(event):
	# Vérifie si le bouton gauche de la souris est enfoncé
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_LEFT:
			if event.pressed:
				# Enregistre la position initiale lorsque le bouton est pressé
				start = event.position
				initialPosition = global_position
				is_moving = true
			else:
				# Arrête le déplacement lorsque le bouton est relâché
				is_moving = false

	# Vérifie si la souris est en mouvement
	elif event is InputEventMouseMotion and is_moving:
		set_global_position(Vector2(initialPosition + (event.position - start)))

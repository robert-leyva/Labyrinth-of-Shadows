Flujo del Proyecto:

1. Inicialización del Juego
-Configuración de jugadores
Se limita la selección a 2 jugadores.
Cada jugador elige su personaje entre:
Thalia: Habilidad de evasión de trampas (con color Magenta).
Garrick: Puede romper muros ilusorios (Celeste).
Lira: Posee Bola de Fuego para quemar trampas (Dorado).
Orion: Tiene capacidad de evasión según su velocidad (Salmón).
Mara: Puede curar con su habilidad (Rojo oscuro).

-Configuración del Tablero (Board)
Inicializar Casillas (Squares):
Se crea una matriz de celdas.
Generar el Laberinto:
Se usa un algoritmo DFS recursivo que “rompe” algunas paredes para crear caminos.
Establecer la Meta (Exit):
Se define una celda meta (por ejemplo, en la esquina inferior derecha).
Colocar Jugadores:
Se ubican en posiciones iniciales designadas (generalmente en una zona de inicio del laberinto).
Colocar Trampas:
Se ubican en celdas libres (no en paredes, no en la meta y no donde ya haya jugadores).
Tipos de trampas:
EmergingSpears, ToxicFog, IllusoryWalls, etc.

2. Bucle Principal del Juego
-Inicio del Turno
Se selecciona al jugador actual según el turno.
Procesar Efectos:
Se actualizan los efectos de estado (por ejemplo, de trampas que afectan HP o velocidad).
Renderización del Estado de Juego
Se limpia la consola y se imprime el tablero:
Se muestran paredes, celdas libres, la meta y los íconos de los jugadores.
Se muestra el detalle de cada jugador (HP, velocidad, posición).
-Turno del Jugador Actual
Mostrar Instrucciones:
Dependiendo del jugador (por ejemplo, Mara tendrá opción de curación pulsando ‘H’, mientras otros usan W/A/S/D para moverse).
Movimiento del Jugador (por pasos, según su velocidad)
Entrada de usuario:
Captura de teclas: W, A, S, D o H.
Validación del Movimiento:
Se verifica que la celda destino exista y que no sea una pared (para evitar “deformar” el laberinto).
Actualización de la Posición:
Se remueve al jugador de la celda origen y se agrega en la celda destino.
Renderización Continua:
Se muestra el estado actualizado del tablero tras cada movimiento.
Verificación de la Meta:
Si el jugador alcanza la celda meta, se anuncia el ganador y finaliza el juego.
Interacción con Trampas (si se encuentra una en la celda destino)
Se detecta la existencia de una trampa en la nueva posición.
Activación de la Trampa:
Se aplica la interacción de la trampa, la cual puede:
Reducir puntos de vida.
Modificar la velocidad.
Cambiar la posición del jugador (como en IllusoryWalls).
Según el personaje, puede haber capacidad de evasión o activación de una habilidad especial (por ejemplo, Lira usando Bola de Fuego).
Mensajes de Detalle:
Se muestran en consola mensajes informando que la trampa fue activada o evitada, junto con las estadísticas actualizadas del jugador.
Finalización del Turno:
La activación de una trampa suele terminar el turno del jugador.
-Cambio de Turno
Se actualiza el turno para pasar al siguiente jugador de la lista.

3. Fin del Juego
-Condición de Victoria:
Cuando un jugador alcanza la celda meta, se notifica que ha ganado la partida.
-Mensaje Final:
Se muestra un mensaje de “¡Fin del juego!” y se detiene el bucle principal.

Requisitos y programas necesarios:
-Terminal de consola moderna como PowerShell 7 o Windows Terminal.
-Que la terminal utilice una fuente monoespaciada que soporte Unicode y emojis.
-Dotnet y un IDE compatible.
-Libreria Spectre.Console

Como Jugar:
Al ejecutar el proyecto te va a pedir que elijas la cantidad de jugadores(la limite a 2 para un mejor funcionamiento y comodidad por las limitantes visuales de la consola, pero puede ampliarse a 5).
Despues seleccionas escribiendo el indice de los 5 jugadores a escoger con sus estadisticas y habilidades especiales correspondientes:

Thalia: Evade trampas automáticamente con un 50% de probabilidad.

Garrick: Rompe muros ilusorios si tiene usos disponibles (2 usos).

Lira: Puede activar la habilidad "Bola de Fuego" hasta 3 veces para quemar trampas en línea recta.

Orion: Evade trampas automáticamente si su velocidad es mayor a 3.

Mara: Puede curarse (por 30 HP) cuando lo decidas, con un límite de 2 curaciones por partida.

Una vez seleccionados los jugadores se genera el laberinto proceduralmente y comienza la partida.

Durante el juego:
Durante el turno del jugador se podra mover libremente por el laberinto, paso a paso consumiendo movimientos(el valor la velocidad del personaje).
Controles:
W,A,S,D para los movimientos y H para la habilidad especial de Mara.
Interacción:
Si el jugador se mueve a una celda con una trampa, ésta se activa o se puede intentar evadir según el personaje, modificando puntos de vida, velocidad o incluso reposicionando al jugador.
Objetivo: El objetivo es llegar a la celda meta. Al alcanzarla, el juego termina y ese jugador gana.

-Durante la Partida: Se imprimen mensajes en consola al activar trampas, evadirlas o al usar habilidades. Estos mensajes indican cómo se modifican los atributos del jugador (puntos de vida, velocidad, etc.).

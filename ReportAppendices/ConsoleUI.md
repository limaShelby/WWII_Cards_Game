# ConsoleUI

## Manager
Es la clase fundamental de la aplicación, encargada de dirigir y controlar la partida. Para conocer a fondo su implementación siga el siguiente enlace: ["Manager"](../ConsoleUI/Manager.cs)

## DeckSetup
Esta clase estática posee como método fundamental `ManageDecks()`, que permite al usuario conocer los decks que dispone, qué cartas contiene cada uno, crear nuevos deck y eliminarlos (siempre que no sean los deck por defecto). `DeckSetup` posee gran cantidad de métodos auxiliares para el correcto funcionamiento de `ManageDecks()`. Para conocer a fondo su implementación siga el siguiente enlace: ["DeckSetup"](../ConsoleUI/DeckSetup.cs)

## GameSetup
Acoplando los métodos `SetDeck()`, `SetMode()` y `BuildDeck()` para el correcto funcionamiento de `SetGame()` se consigue que el usuario pueda sentar las bases del juego y comenzar la partida. Para conocer a fondo su implementación siga el siguiente enlace: ["GameSetup"](../ConsoleUI/GameSetup.cs)

## Extensions
Esta clase estática contiene métodos extensores utilizados en el resto de las clases del proyecto.

## Program
`Program` se limita a iniciar la aplicación y permite al usuario realizar todas las acciones posibles, apoyándose de las clases `DeckSetup` y `GameSetup`.
# WWII Cards Game

## Ejecución del proyecto
**Para ejecutar el proyecto es necesario tener instalado .NET Core 6.0.**
Dirígase a la carpeta del proyecto, si su sistema operativo es Linux ejecute en la terminal:

```bash
make dev
```

, si su sistema operativo es Windows ejecute en la terminal:

```bash
dotnet run --project WWII_Cards_Game
```

## Guía
Una vez iniciado el juego en la consola puede utilizar tres comandos:

- *play*: Al ejecutar este comando debe seleccionar los decks que desea utilizar y el modo de juego (`human` o `bot`). Una vez realizado esto, comienza la partida, que detallaremos más adelante.
- *create*: Al ejecutar este comando tiene varias opciones para manejar los decks disponibles. Si utiliza el comando `show` se imprimirá una lista con los nombres de los decks, si utiliza `show + "nombre del deck"` (`+` significa *espacio*) se imprimirá una lista con los nombres de las cartas de dicho deck, si utiliza `delete + "nombre del deck"` se eliminará dicho deck (note que los decks por defecto **AmericanDeck** y **GermanDeck** no pueden ser eliminados), si utiliza `create + deck` comenzará la creación de un nuevo deck de acuerdo a las instrucciones brindadas en la consola (note que para poder completar la construcción de un deck es necesario crear un total de 16 cartas), más adelante se detalla la estructura de una orden.
- *exit*: Al ejecutar este comando se termina la ejecución del juego.

**Nota**: Siempre que no haya comenzado el juego, puede retornar al inicio, utilizando el comando `home`. Si está en el medio de la creación de un deck, puede detenerla utilizando el comando `done` al finalizar la creación de una carta.

## Partida
El objetivo de la partida es sencillo, destruir la base del rival(*HQ* -headquarters-). Para conseguirlo cada jugador dispone de un conjunto de cartas y de un campo donde puede desplegarlas.

### Cartas
Existen dos tipos de cartas, las unidades y las órdenes, ambas presentan como características comunes: el *nombre*, el *costo* de utilización, la *orden* a ejecutar (las unidades pueden no portar órdenes) y una *descripción* (fundamentalmente de la orden que poseen).
- *Unidades*: Las unidades son cartas capaces de ser desplegadas en el campo y de atacar a otra unidad del rival o al propio HQ. Para poder realizar esto poseen *daño* y *defensa*. Si cuentan con una orden, también pueden ejecutarla.
- *Órdenes*: Las órdenes no son desplegadas en el campo, sino que ejecutan la orden que presentan (valga la redundancia) y una vez realizado esto, son eliminadas de la partida.

### Campo
Cada jugador cuenta con un campo donde pueden desplegarse un máximo de 5 unidades

### Mano
Cada jugador cuenta con una mano de 6 cartas al comienzo del partida y recibe 2 cartas a medida que empiece una nueva ronda (hasta que se acaben las cartas del deck).

### Créditos
Para desplegar una unidad en el campo, realizar un ataque con esta, ejecutar la orden que posee o ejecutar una orden(carta) de la mano, es necesario pagar el costo de la carta. Este costo es sustraído de los *créditos* de cada jugador, y por supuesto, si los créditos no son suficientes no podrá realizarse dicha acción.
Cada jugador comienza con 5 créditos y a medida que pasa una ronda se incrementan en una cantidad también variable, pues comienza en 1 y con el transcurso del juego aumenta hasta alcanzar un máximo de 12 por ronda.

### Deck
Cada jugador comienza con un deck de 32 cartas que va tomando en su mano a medida que pasen las rondas.

### Ataques
Al realizar un ataque se sustraen los daños con las defensas, tanto de la carta que ataca como la atacada. Si la defensa disminuye a 0 o por debajo, la carta es destruida.
Solo se puede atacar el HQ del rival si no posee unidades en el campo.

### Comandos para jugar
Antes de enumerar los comandos, aclaremos que cada carta, sea de la mano, del campo del jugador o de su rival, posee una coordenada (ejemplos: `h0` -la carta en la posición 0 de la mano del jugador-, `f1` -la carta en la posición 1 del campo del jugador-, `o5` -la carta en la posición 5 del campo del rival-).

**Comandos**
- *dp* o *deploy*: Para desplegar una unidad al campo o ejecutar una carta orden de la mano. Ejemplos: `dp h0`, `deploy h2`.
- *ds* o *description*: Para conocer la descripción de la carta. Ejemplos: `ds f1`, `description h0`, `ds o2`.
- *p* o *perform*: Para ejecutar la orden asociada a una unidad desplegada. Ejemplos: `p f0`, `perform f5`.
- *a* o *attack*: Para ejecutar el ataque de una unidad desplegada. Ejemplos: `f0 a o3`, `f2 attack o1`, `f0 a hq`, `f3 attack hq`.
- *e*: Para finalizar el turno.

### Condiciones de victoria
Un jugador pierde la partida si su HQ es destruido o si se queda sin cartas (ni en el deck, ni en la mano, ni en el campo).

## Órdenes
En el proceso de construcción de una carta las órdenes deben seguir una determinada sintáxis para su correcta ejecución. Esta sería:

> `ACCIÓN + (SELECTOR) + By + VALOR + ;` o `ACCIÓN + By + VALOR + ;`

### Acción
Las acciones disponibles son las siguientes:
- `IncreaseDamage`: Incrementa el daño del selector
- `IncreaseDefense`: Incrementa la defensa del selector
- `DecreaseDamage`: Disminuye el daño del selector
- `DecreaseDefense`: Disminuye la defensa del selector
- `IncreaseHQ`: Incrementa el HQ del dueño de la carta
- `IncreaseCredits`: Incrementa los créditos del dueño de la carta
Las acciones *IncreaseHQ* y *IncreaseCredits* no van seguidas de un selector, sino que directamente se les asigna un *Valor*.

### Selector
Los selectores disponibles son los siguientes:
- `OwnerMax`: Selecciona la carta de mayor daño del dueño de la carta
- `OwnerMin`: Selecciona la carta de menor daño del dueño de la carta
- `OpponentMax`: Selecciona la carta de mayor daño del oponente
- `OpponentMin`: Selecciona la carta de menor daño del oponente
- `Self`: Selecciona a la propia carta, si esta es una unidad
Los selectores deben utilizarse rodeados por paréntesis.

### Valor
El valor no es más que un número entero.

### Ejemplos válidos de órdenes

- `IncreaseDamage (Self) By 100;`
- `DecreaseDefense (OpponentMax) By 50;`
- `IncreaseDamage (OwnerMin) By 50;`
- `IncreaseCredits By 20;`
- `IncreaseHQ By 40;`
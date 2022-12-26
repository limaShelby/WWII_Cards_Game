# MainLib
El proyecto MainLib puede dividirse en tres grandes partes: *diseño de cartas*, *diseño de jugadores* y *auxiliares*.

## Diseño de cartas
Las clases que forman parte de *diseño de cartas* son las encargadas de brindar la estructura y funcionalidad detrás de las cartas del juego.

### Card
`Card` es la clase abstracta fundamental. Garantiza que sus clases herederas puedan ejecutar órdenes mediante el método `Perform()` que evalua el `AST` heredado por dichas clases; y además, las dota de las propiedades *Name*, *Cost* y *Description* fundamentales para toda carta.

> `Perform()` es el método encargado de recorrer el `AST` para evaluar las instrucciones almacenadas en sus nodos. Recibe como parámetros a dos objetos de tipo *Player* (se trata más adelante) para poder realizar modificaciones al estado de la partida.

### IAttackable
`IAttackable` es la interfaz que garantiza que las cartas que la implementen pueden interactuar unas con otras de manera directa (mediante ataques), para ello cuenta con dos propiedades *Damage* y *Defense*.

```cs
    public interface IAttackable
    {
        int Damage { get; }
        int Defense { get; set; }
    }
```

### Unit
`Unit` es la clase concreta que representa a una carta de tipo *unidad*. Hereda, por supuesto, de `Card` e implementa la interfaz `IAttackable`. Cuenta con el método `Attack()` que la diferencia por completo de `Order`.

> `Attack()` es método que se encarga de ejecutar un ataque entre cartas manejando las diferencias entre los *daños* y *defensas*; para ello recibe como parámetro un objeto de tipo `IAttackable`.

```cs
    public void Attack(IAttackable targetCard)
    {
         Defense -= targetCard.Damage;
         targetCard.Defense -= Damage;
    }
```

### Order
`Order` es la clase concreta que representa a un carta de tipo *orden*. Hereda, por supuesto, de `Card`.

### HQ
`HeadQuarters -HQ-` es la clase concreta que implementa `IAttackable` permitiendo que sea atacada por una *unidad*.  

## Diseño de jugadores
Las clases que forman parte de *diseño de jugadores* son las encargadas de brindar la estructura y funcionalidad detrás de los jugadores.

### Player
`Player` es la clase abtracta que se encarga de inicializar todos las propiedades necesarias en sus clases herederas y las dota de los métodos para ejecutar las acciones permitidas en el juego como: `ExecuteAttack()`, `ExecuteDeployment()`, `ExecutePerformance()`, así como otros métodos auxiliares para su funcionamiento. Define, además, el método `Play()` que tendrá que ser implementado por sus clases herederas.

### UserPlayer
`UserPlayer` es la clase concreta que representa a un *usuario*. Hereda, por supuesto, de `Player` y su contenido es la implementación del método `Play()` de acuerdo a las posibilidades y opciones del usuario. Para conocer a `UserPlayer` a fondo siga el siguiente enlace: ["UserPlayer"](../MainLib/UserPlayer.cs)

### AI_Player
`AI_Player` es la clase concreta que representa a un *bot*. Hereda, por supuesto, de `Player` y su contenido es la implementación del método `Play()` siguiendo una determinada estrategia. Para conocer a `AI_Player` a fondo siga el siguiente enlace: ["AI_Player"](../MainLib/AI_Player.cs)

## Auxiliares
Las clases auxiliares no representan estructuras necesarias *per se* en el juego, pero son utilizadas para mejorar la dinámica e interactividad de la aplicación.

### UnitInitializer y OrderInitializer
Son estructuras que almacenan los valores necesarios para la construcción de unidades y órdenes, lo que les permite ser serializadas y deserializadas en archivos *json*, mejorando la dinámica del almacenamiento, creación y uso de los decks por el usuario.

### UI
Como su nombre indica, `UI`(*User Interface*) es la clase estática que constituye el motor visual de la aplicación, donde se engloban los métodos que muestran al usuario el estado de la partida y los errores que pueda cometer durante el uso de la aplicación.
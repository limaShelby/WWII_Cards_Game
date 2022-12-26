# WWII Cards Game

> Informe del Proyecto de Programación II.
> Facultad de Matemática y Computación - Universidad de La Habana.
> Curso 2022 - 2023.
> Estudiantes: Ernesto Castellanos Vázquez, Richard E. Ávila Entenza, Javier Lima García.

WWII Cards Game (WWII_CG) es una biblioteca de clases y una aplicación visual para un juego de tipo Trading Card asociado a la Segunda Guerra Mundial.

## Estructura
*WWII_CG* está formado por tres proyectos: InterpreterLib, MainLib y ConsoleUI.

### InterpreterLib
*InterpreterLib* es una biblioteca de clases cuyo objetivo es la construcción de un `AST` (árbol de sintáxis abstracta) a partir de las órdenes que reciben las cartas en su construcción. Dicho AST es evaluado por las propias cartas para la ejecución de las órdenes de manera dinámica durante la partida. Para más información seguir el siguiente enlace: ["IntepreterLib"](./ReportAppendices\InterpreterLib.md)

### MainLib
*MainLib* como indica su nombre es la biblioteca de clases principal, donde se encuentra todo el diseño de clases e interfaces necesarios para el correcto funcionamiento de la aplicación. Para más información seguir el siguiente enlace: ["MainLib"](./ReportAppendices\MainLib.md)

### ConsoleUI
*ConsoleUI* es la aplicación de consola, la interfaz gráfica del juego. Contiene las clases necesarias para brindar la interactividad a la hora de jugar y de construir nuevas cartas. Para más información seguir el siguiente enlace: ["ConsoleUI"](./ReportAppendices\ConsoleUI.md)
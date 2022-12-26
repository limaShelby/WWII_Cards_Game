# InterpreterLib

## Estructura

### Lexer
La clase `Lexer` es el analizador sintáctico del proyecto. Su objetivo es crear un objeto *lexer* muy semejante a un *IEnumerator* que avanza a lo largo de un *input string* con el método `GetNextToken()` validando cada elemento del string de acuerdo con los *tokens* definidos en el struct `Syntax`; dicho objeto lexer es utilizado en la construcción del *parser* que veremos más adelante.

#### Elementos fundamentales de `Lexer`

- `GetNextToken()`: Es el método fundamental en `Lexer` que se encarga de construir *tokens* a medida que avanza a largo de un string, el resto de métodos en la clase no son más que sus auxiliares.

```cs
        public Token GetNextToken()
        {
            if (char.IsWhiteSpace(CurrentChar))
            {
                SkipWhiteSpace();
            }

            if (CurrentChar == '@')
            {
                return new Token(Syntax.EOF, "@");
            }

            if (char.IsDigit(CurrentChar))
            {
                return new Token(Syntax.Integer, GetInteger());
            }

            if (CurrentChar == ';' || CurrentChar == '(' || CurrentChar == ')')
            {
                char temporaryChar = CurrentChar;
                Advance();
                return new Token(GetSymbol(temporaryChar), temporaryChar.ToString());
            }

            if (char.IsLetter(CurrentChar)) return GetWordToken();

            throw new Exception("Error parsing input");
        }
```
- `struct Syntax`: Es la estructura que contiene que tipos pueden tomar cada *token* creado de acuerdo a la gramática definida para el lenguaje, la cual se tratará más adelante.

### Parser
La clase `Parser` es la encargada, apoyándose de un objeto *lexer* de la construcción de un AST de acuerdo a la gramática definida.

#### Elementos fundamentales de `Parser`

- `Eat()`: Es el método encargado de validar si el *currentToken* es del tipo esperado de acuerdo con la gramática.

```cs
    private void Eat(string tokenType)
        {
            if (CurrentToken.Type == tokenType) CurrentToken = _lexer.GetNextToken();
            else throw new Exception("Error parsing input");
        }
```

- `Selector()`: Es el método encargado de construir un nodo selector para el futuro `AST`.
- `Expression()`:  Es el método encargado de construir un nodo de tipo *action* para el futuro `AST`.
- `Statement()`: Es el método encargado de construir un nodo statement, formado por los nodos anteriores, para el futuro `AST`.
- `Parse()`: Es el método que apoyándose de los anteriores retorna un `AST`.

### AST
AST contiene una serie de clases que heredan de la clase *AST*, incluida en el .cs también, que dan la estructura del `AST` que será retornado por `Parser`. Los nodos del árbol pueden ser:

- `UnaryAction`: Contiene la acción a producir sobre una determinada carta, localizada en su propiedad-nodo `SelectedCard`.
- `HQ_Action`: Contiene la acción de incrementar el *HQ* con el valor a hacerlo.
- `CreditsAction`: Contiene la acción de incrementar los *créditos* con el valor a hacerlo.
- `SelectedCard`: Contiene el token que representa la carta a seleccionar.

### Token
`Token` es la estructura utilizada durante todo `InterpreterLib` que representa los elementos determinados en la gramática del lenguaje. Está formado por un *valor* y por un *tipo*.

## Gramática
El lenguaje definido para el interprete consta de la siguiente gramática:

> `statement: expression SEMI | expression SEMI statement`
> `expression: (IncreaseDamage| DecreaseDamage| IncreaseDefense| DecreaseDefense) LPAREN selector RPAREN By Value | (IncreaseHQ | IncreaseCredits) By Value`
> `selector: OwnerMax | OwnerMin | OpponentMax | OpponentMin | Self`
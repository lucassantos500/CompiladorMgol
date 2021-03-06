# CompiladorMgol
Este repositório tem como objetivo o desenvolvimento de um compilador que recebe como entrada um arquivo fonte em uma linguagem Mgol, realiza a fase de análise e síntese gerando um arquivo objeto em linguagem C. O arquivo final deverá ser compilável em compilador C, ou seja, o código gerado deverá estar completo para compilação e execução.

| Índice                                                                 |
|------------------------------------------------------------------------|
| [Analisador Léxico](../master/Analisador/analisadorLexico.cs)          |
| [Controle Dados/Símbolos](../master/Controle/controleDadosSimbolos.cs) |
| [Tabela de Erros](../master/Tabelas/tabelaErros.cs)                    |
| [Tabela de Símbolos](../master/Tabelas/tabelaSimbolo.cs)               |
| [Tabela de Transição](.../master/Tabelas/tabelaTransicao.cs)           |


O desenvolvimento será composto por **três etapas**:

# Etapa 1 - Analisador Léxico

Esta etapa visa o desenvolvimento de um **analisador léxico** que reconheça a tabela de tokens disponíveis para a
linguagem Mgol e será implementado em **C#**.
Para tal:
- Desenvolver um diagrama de estados de um autômato finito determinístico (AFD) que reconheça a linguagem regular expressa através da Tabela de simbolos.

**Autômato finito determinístico**
![alt text](https://uploaddeimagens.com.br/images/001/609/118/original/AFD.png?1536771895)

**Legenda**

| Estado          | q0     | q1       | q2        | q5         | q7      | q9         | q10 | q11 | q12 | q13 | q14 | q15 | q16  | q17  | q18  | q19 |
|-----------------|--------|----------|-----------|------------|---------|------------|-----|-----|-----|-----|-----|-----|------|------|------|-----|
| Token retornado | Inicio | num(int) | num(real) | num(cient) | literal | comentário | id  | OPR | RCB | OPR | OPR | OPM | AB_P | FC_P | PT_V | EOF |

- Implementar o AFD na forma de tabela de transições.

**Tabela de transição**

|       | q0  | q1 | q2 | q3 | q4 | q5 | q6 | q7 | q8 | q9 | q10 | q11 | q12 | q13 | q14 | q15 | q16 | q17 | q18 |
|-------|-----|----|----|----|----|----|----|----|----|----|-----|-----|-----|-----|-----|-----|-----|-----|-----|
| \n    | q0  |    |    |    |    |    |    |    |    |    |     |     |     |     |     |     |     |     |     |
| \t    | q0  |    |    |    |    |    |    |    |    |    |     |     |     |     |     |     |     |     |     |
| space | q0  |    |    |    |    |    |    |    |    |    |     |     |     |     |     |     |     |     |     |
| D     | q1  |    | q2 | q5 | q5 | q5 |    |    |    |    | q10 |     |     |     |     |     |     |     |     |
| "     | q6  |    |    |    |    |    |    |    |    |    |     |     |     |     |     |     |     |     |     |
| L     | q10 |    |    |    |    |    |    |    |    |    | q10 |     |     |     |     |     |     |     |     |
| {     | q8  |    |    |    |    |    |    |    |    |    |     |     |     |     |     |     |     |     |     |
| <     | q11 |    |    |    |    |    |    |    |    |    |     |     |     |     |     |     |     |     |     |
| >     | q13 |    |    |    |    |    |    |    |    |    |     | q14 |     |     |     |     |     |     |     |
| =     | q14 |    |    |    |    |    |    |    |    |    |     | q14 |     | q14 |     |     |     |     |     |
| +     | q15 |    |    | q4 |    |    |    |    |    |    |     |     |     |     |     |     |     |     |     |
| -     | q15 |    |    |    |    |    |    |    |    |    |     | q12 |     |     |     |     |     |     |     |
| *     | q15 |    |    |    |    |    |    |    |    |    |     |     |     |     |     |     |     |     |     |
| /     | q15 |    |    |    |    |    |    |    |    |    |     |     |     |     |     |     |     |     |     |
| (     | q16 |    |    |    |    |    |    |    |    |    |     |     |     |     |     |     |     |     |     |
| )     | q17 |    |    |    |    |    |    |    |    |    |     |     |     |     |     |     |     |     |     |
| ;     | q18 |    |    |    |    |    |    |    |    |    |     |     |     |     |     |     |     |     |     |
| EOF   | q19 |    |    |    |    |    |    |    |    |    |     |     |     |     |     |     |     |     |     |
| .     |     | q2 |    |    |    |    | q6 |    | q8 |    |     |     |     |     |     |     |     |     |     |
| E     |     | q3 | q3 |    |    |    |    |    |    |    |     |     |     |     |     |     |     |     |     |
| C     |     |    |    |    |    |    |    |    |    |    |     |     |     |     |     |     |     |     |     |
| "     |     |    |    |    |    |    | q7 |    |    |    |     |     |     |     |     |     |     |     |     |
| -     |     |    |    |    |    |    |    |    |    |    |     |     |     |     |     |     |     |     |     |
| }     |     |    |    |    |    |    |    |    | q9 |    |     |     |     |     |     |     |     |     |     |

A tabela de transição foi representada no analisador léxico utilizando uma matriz com 29 colunas e 24 linhas e os símbolos foram representando utilizando a [Tabela ASCII](https://upload.wikimedia.org/wikipedia/commons/d/dd/ASCII-Table.svg).



- Desenvolver uma tabela de Símbolos, utilizar estrutura de dados para armazenar as palavras chave da linguagem e os identificadores reconhecidos pelo analisador léxico.

**Palavras chaves**
  
| Token     | Significado                                  |
|-----------|----------------------------------------------|
| inicio    | Delimita o início do programa                |
| varinicio | Delimita o início da declaração de variáveis |
| varfim    | Delimita o fim da declaração de variáveis    |
| escreva   | Imprime na saída padrão                      |
| leia      | Lê da saída padrão                           |
| se        | Estrutura condicional                        |
| entao     | Elemento de estrutura condicional            |
| fimse     | Elemento de estrutura condicional            |
| fim       | Delimita o fim do programa                   |
| inteiro   | Tipo de dado                                 |
| lit       | Tipo de dado                                 |
| real      | Tipo de dado                                 |

- Desenvolver uma estrutura que, dados os símbolos de entrada e transições do AFD, caso não seja possível identificar o tipo de token, retorne na tela o tipo de erro léxico encontrado seguido dos números de linha e coluna do texto fonte onde o erro foi descoberto.

**Tabela de tokens**

| Token      | Significado                                                                  | Características                   | Atributos                                                     |
|------------|------------------------------------------------------------------------------|-----------------------------------|---------------------------------------------------------------|
| Num        | Constante numérica                                                           | D+( (\.D+) | (E | e)(+ | -)?D+))? | Token, Tipo e lexema                                          |
| Literal    | Constante literal                                                            | ".*"                              | Token, Tipo e lexema                                          |
| id         | Identificador                                                                | L(L|D|_)*                         | Token, Tipo e lexema                                          |
| Comentário | Ignorar comentários, ou seja, reconhecer mas não retornar o token            | {.*}                              |                                                               |
| EOF        | Final de Arquivo                                                             | Flag da linguagem                 | Token                                                         |
| OPR        | Operadores relacionais                                                       | <, >, >= , <= , =, <>             | Token, lexema                                                 |
| RCB        | Atribuição                                                                   | <-                                | Token, lexema                                                 |
| OPM        | Operadores aritméticos                                                       | + , -, *, /                       | Token, lexema                                                 |
| AB_P       | Abre Parênteses                                                              | (                                 | Token, lexema                                                 |
| FC_P       | Fecha Parênteses                                                             | )                                 | Token, lexema                                                 |
| PT_V       | Ponto e vírgula                                                              | )                                 | Token, lexema                                                 |
| ERRO       | Qualquer coisa diferente de qualquer símbolo token e palavra-chave definida. |                                   | Token, descrição do erro, linha e coluna onde o erro ocorreu. |
  
  
  
  

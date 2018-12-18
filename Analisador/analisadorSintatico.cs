using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controle;
using Tabelas;
using System.Collections;
using System.IO;

namespace Analisador
{
    public class analisadorSintatico
    {
        static int Iniciado = 0, IniciadoC = 0;
        static int ERRO = 0;

        static int Tx_18 = 0;
        static int Tx_25 = 0;

        public static Stack<controleDadosSimbolos> Pilha_SEM;

        public static void getSintatico()
        {
            Pilha_SEM = new Stack<controleDadosSimbolos>();
            Stack<int> estado = new Stack<int>();
            int state;
            analisadorLexico.Preencher_analisadorLexico();
            controleDadosSimbolos simbolo = analisadorLexico.getLex(0);
            estado.Push(0);


            string erroencontrado;

            while (true)
            {
                //estado no topo da pilha
                state = estado.Peek();

                //identificando erro
                if (state == 132)
                {


                    estado.Pop();
                    state = estado.Peek();
                    erroencontrado = getErro(state);
                    Console.WriteLine("\nErro encontrado no estado: " + state + " - " + erroencontrado);
                    Console.Write("\n" + simbolo.Lexema);
                    analisadorLexico.getLinhaColuna();
                    return;
                }

                //se vai empilhar
                if ((getACTION(state, simbolo.Token) > 0) && (getACTION(state, simbolo.Token) != 151))
                {
                    estado.Push(getACTION(state, simbolo.Token));
                    Pilha_SEM.Push(simbolo);
                    simbolo = analisadorLexico.getLex(analisadorLexico.pos);//proximo lexema

                    //se vai haver redução
                }
                else if (getACTION(state, simbolo.Token) <= 0)
                {
                    int reduce = getACTION(state, simbolo.Token) * (-1);//voltar ao numero verdadeiro da producao a ser reduzida
                    String sentenca = getSentenca(reduce);//procurando a sentenca que esta sendo reduzida

                    //encontrar a quantidade de simbolos de beta
                    int qtdSimbolosBeta = 0;
                    Boolean isBeta = false;

                    //guardar o não terminal
                    StringBuilder nonTerminal = new StringBuilder();
                    string ALFA = "";
                    string BETA = "";

                    //recuperando a quantidade de simbolos beta
                    for (int i = 0; i < sentenca.Length; i++)
                    {
                        //antes de ->
                        if (isBeta == false && sentenca[i] != '-')
                        {
                            ALFA += sentenca[i];
                        }
                        // apos ->
                        if (sentenca[i] == '-' && sentenca[i + 1] == '>')
                        {
                            isBeta = true;
                            i = i + 2;
                        }
                        if (isBeta == true)
                        {
                            BETA += sentenca[i];
                        }
                        if (sentenca[i] == ' ' && isBeta == true)
                        {
                            qtdSimbolosBeta++;
                        }
                    }

                    int j = 0;
                    //Recuperando o não terminal
                    while (sentenca[j] != ' ')
                    {
                        nonTerminal.Append(sentenca[j]);
                        j++;
                    }
                    //Desempilha qtdSimbolosBeta de estados
                    for (int i = 0; i < qtdSimbolosBeta; i++)
                    {
                        estado.Pop();
                    }

                    state = estado.Peek();//estado t é o topo da pilha
                    estado.Push(getGOTO(state, nonTerminal.ToString()));

                    Console.WriteLine("\n---------------------------------------------------------------------\n");
                    Console.WriteLine("\nSentenca reduzida: " + sentenca);//imprime a produção A->B
                    string[] K = ALFA.Split(' ');

                    callSemantica(reduce, K[0], qtdSimbolosBeta);//Chamando o analisador semantico

                    //Se ha aceitacao
                }
                else if (getACTION(state, simbolo.Token) == 151)
                {
                    Console.WriteLine("\nAceitacao!!!");
                    escreverPontoC(Tx_18, Tx_25);
                    return;
                }

            }

        }

        public static void escreverTXT(string texto)
        {
            if (Iniciado == 0)
            {
                using (StreamWriter writer = new StreamWriter("C:\\Users\\lucas\\Desktop\\Compilador\\Texto2.txt"))
                {
                    writer.WriteLine(texto);
                    Iniciado = 1;
                    writer.Close();

                }
            }
            else if (Iniciado == 1)
            {
                using (StreamWriter writer = new StreamWriter("C:\\Users\\lucas\\Desktop\\Compilador\\Texto2.txt", true))
                {
                    writer.WriteLine(texto);
                    writer.Close();
                }
            }
        }


        public static void callSemantica(int reduc, string ALFA, int qtdBETA)
        {

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(regras_sem[reduc - 1]);
            Console.ResetColor();

            Dictionary<string, controleDadosSimbolos> Hash_SEM = new Dictionary<string, controleDadosSimbolos>();
            controleDadosSimbolos aux;



            if (reduc == 18 || reduc == 25)
            {
                aux = new controleDadosSimbolos(Pilha_SEM.Peek().Lexema, Pilha_SEM.Peek().Token, Pilha_SEM.Peek().Tipo);
                Pilha_SEM.Pop();
                //Hash_SEM["OPRD2"] = aux;
                Hash_SEM.Add("OPRD2", aux);
                aux = new controleDadosSimbolos(Pilha_SEM.Peek().Lexema, Pilha_SEM.Peek().Token, Pilha_SEM.Peek().Tipo);
                Pilha_SEM.Pop();
                //Hash_SEM[aux.Token] = aux;
                Hash_SEM.Add(aux.Token, aux);
                aux = new controleDadosSimbolos(Pilha_SEM.Peek().Lexema, Pilha_SEM.Peek().Token, Pilha_SEM.Peek().Tipo);
                Pilha_SEM.Pop();
                //Hash_SEM["OPR1"] = aux;
                Hash_SEM.Add("OPRD1", aux);

            }
            else
            {
                for (int i = 0; i < qtdBETA; i++)
                {
                    aux = new controleDadosSimbolos(Pilha_SEM.Peek().Lexema, Pilha_SEM.Peek().Token, Pilha_SEM.Peek().Tipo);
                    Hash_SEM.Add(aux.Token, aux);
                    Pilha_SEM.Pop();
                }
            }

            controleDadosSimbolos S = new controleDadosSimbolos("", ALFA, "");

            if (reduc == 5)
            {
                escreverTXT("\n\n\n");
            }

            else if (reduc == 6)
            {
                controleDadosSimbolos ID;
                ID = Hash_SEM["id"];
                controleDadosSimbolos TIPO;
                TIPO = Hash_SEM["TIPO"];

                if (TIPO.Tipo == "lit")
                {
                    escreverTXT("literal " + ID.Lexema + ";");
                }
                else if (TIPO.Tipo == "real")
                {
                    escreverTXT("double " + ID.Lexema + ";");
                }
                else
                {
                    escreverTXT(TIPO.Tipo + " " + ID.Lexema + ";");
                }

                analisadorLexico.att_tipo(ID.Lexema, TIPO.Tipo);
            }
            else if (reduc == 7)
            {
                S.Tipo = "int";
            }
            else if (reduc == 8)
            {
                S.Tipo = "real";
            }
            else if (reduc == 9)
            {
                S.Tipo = "lit";
            }
            else if (reduc == 11)
            {
                controleDadosSimbolos ID = Hash_SEM["id"];
                if (analisadorLexico.existe(ID.Lexema))
                {
                    if (ID.Tipo == "lit")
                    {
                        escreverTXT("scanf(\"%s\",&" + ID.Lexema + ");");
                    }
                    else if (ID.Tipo == "int")
                    {
                        escreverTXT("scanf(\"%d\",&" + ID.Lexema + ");");
                    }
                    else if (ID.Tipo == "real")
                    {
                        escreverTXT("scanf(\"%f\",&" + ID.Lexema + ");");
                    }
                }
                else
                {
                    Console.WriteLine("Erro: Variável não declarada");
                    ERRO++;
                }
            }
            else if (reduc == 12)
            {
                controleDadosSimbolos ARG = Hash_SEM["ARG"];
                if (ARG.Tipo == "lit")
                {
                    escreverTXT("printf(\"%s\"," + ARG.Lexema + ");");
                }
                else if (ARG.Tipo == "real")
                {
                    escreverTXT("printf(\"%lf\"," + ARG.Lexema + ");");
                }
                else if (ARG.Tipo == "int")
                {
                    escreverTXT("printf(\"%d\"," + ARG.Lexema + ");");
                }
                else
                {
                    escreverTXT("printf(" + ARG.Lexema + ");");
                }
            }
            else if (reduc == 13)
            {
                controleDadosSimbolos literal = Hash_SEM["literal"];
                S.Lexema = literal.Lexema;
                S.Tipo = "literal";
            }
            else if (reduc == 14)
            {
                controleDadosSimbolos num = Hash_SEM["num"];
                S.Lexema = num.Lexema;
                S.Tipo = "num";
            }
            else if (reduc == 15)
            {
                controleDadosSimbolos ID = Hash_SEM["id"];
                if (analisadorLexico.existe(ID.Lexema))
                {
                    S.Lexema = ID.Lexema;
                    S.Tipo = ID.Tipo;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n-->Erro: Variável não declarada.\n");
                    analisadorLexico.getLinhaColuna();
                    Console.ResetColor();
                    ERRO++;

                }
            }
            else if (reduc == 17)
            {
                controleDadosSimbolos ID = Hash_SEM["id"];
                if (analisadorLexico.existe(ID.Lexema))
                {
                    controleDadosSimbolos LD = Hash_SEM["LD"];
                    if ((ID.Tipo == LD.Tipo) || (ID.Tipo == "real" && (LD.Tipo == "num" || LD.Tipo == "int")))
                    {
                        controleDadosSimbolos rcb = Hash_SEM["rcb"];
                        rcb.Tipo = "=";
                        escreverTXT(ID.Lexema + " " + rcb.Tipo + " " + LD.Lexema + ";");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n-->Erro: Tipos diferentes para atribuição.\n");
                        analisadorLexico.getLinhaColuna();
                        Console.ResetColor();
                        ERRO++;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n-->Erro: Variável não declarada.\n");
                    analisadorLexico.getLinhaColuna();
                    Console.ResetColor();
                    ERRO++;
                }
            }
            else if (reduc == 18)
            {
                string tx = "T" + Tx_18;

                controleDadosSimbolos OPRD1 = Hash_SEM["OPRD1"];
                controleDadosSimbolos OPRD2 = Hash_SEM["OPRD2"];

                string[] tipos_equivalentes = { "num", "real", "int" };

                if (OPRD1.Tipo != "lit" && OPRD2.Tipo != "lit")
                {
                    S.Lexema = tx;
                    S.Tipo = "int";
                    Tx_18 += 1;

                    controleDadosSimbolos opm = Hash_SEM["opm"];
                    escreverTXT(tx + " = " + OPRD1.Lexema + " " + opm.Lexema + " " + OPRD2.Lexema + ";");

                }
                else
                {
                    S.Lexema = tx;
                    S.Tipo = "num";
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("-->Erro: Operandos com tipos incompatíveis.");
                    analisadorLexico.getLinhaColuna();
                    Console.ResetColor();
                    ERRO++;
                }

            }
            else if (reduc == 19)
            {
                controleDadosSimbolos OPRD = Hash_SEM["OPRD"];
                S.Lexema = OPRD.Lexema;
                S.Tipo = OPRD.Tipo;
            }
            else if (reduc == 20)
            {
                controleDadosSimbolos ID = Hash_SEM["id"];
                if (analisadorLexico.existe(ID.Lexema))
                {
                    S.Lexema = ID.Lexema;
                    S.Tipo = ID.Tipo;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("-->Erro: Variável não declarada.");
                    analisadorLexico.getLinhaColuna();
                    Console.ResetColor();
                    ERRO++;
                }
            }
            else if (reduc == 21)
            {
                controleDadosSimbolos num = Hash_SEM["num"];
                S.Lexema = num.Lexema;
                S.Tipo = num.Token;
            }
            else if (reduc == 23)
            {
                escreverTXT("}");
            }
            else if (reduc == 24)
            {
                controleDadosSimbolos EXP_R = Hash_SEM["EXP_R"];
                escreverTXT("if(" + EXP_R.Lexema + "){");
            }
            else if (reduc == 25)
            {
                controleDadosSimbolos OPRD1 = Hash_SEM["OPRD1"];
                controleDadosSimbolos OPRD2 = Hash_SEM["OPRD2"];

                if ((OPRD1.Tipo == "lit" && OPRD2.Tipo == "lit") || (OPRD1.Tipo != "lit" && OPRD2.Tipo != "lit"))
                {
                    string tx = "T" + Tx_25;
                    S.Lexema = tx;
                    S.Tipo = "boolean";
                    Tx_25++;

                    controleDadosSimbolos opr = Hash_SEM["opr"];
                    if (opr.Lexema == "==")
                    {
                        escreverTXT(tx + " = " + OPRD1.Lexema + " == " + OPRD2.Lexema + ";");
                    }
                    else if (opr.Lexema == "<>")
                    {
                        escreverTXT(tx + " = " + OPRD1.Lexema + " != " + OPRD2.Lexema + ";");
                    }
                    else
                    {
                        escreverTXT(tx + " = " + OPRD1.Lexema + " " + opr.Lexema + " " + OPRD2.Lexema + ";");
                    }

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("-->Erro: Operandos com tipos incompatíveis.");
                    analisadorLexico.getLinhaColuna();
                    Console.ResetColor();
                    ERRO++;
                }
            }

            Pilha_SEM.Push(S);
        }


        public static String getSentenca(int line)
        {
            EnumeracaoDaGramatica[] Enumeracao = new EnumeracaoDaGramatica[30];

            Enumeracao[0] = new EnumeracaoDaGramatica(1, "P' -> P");
            Enumeracao[1] = new EnumeracaoDaGramatica(2, "P -> inicio V A");
            Enumeracao[2] = new EnumeracaoDaGramatica(3, "V -> varinicio LV");
            Enumeracao[3] = new EnumeracaoDaGramatica(4, "LV -> D LV");
            Enumeracao[4] = new EnumeracaoDaGramatica(5, "LV -> varfim ;");
            Enumeracao[5] = new EnumeracaoDaGramatica(6, "D -> id TIPO ;");
            Enumeracao[6] = new EnumeracaoDaGramatica(7, "TIPO -> int");
            Enumeracao[7] = new EnumeracaoDaGramatica(8, "TIPO -> real");
            Enumeracao[8] = new EnumeracaoDaGramatica(9, "TIPO -> lit");
            Enumeracao[9] = new EnumeracaoDaGramatica(10, "A -> ES A");
            Enumeracao[10] = new EnumeracaoDaGramatica(11, "ES -> leia id ;");
            Enumeracao[11] = new EnumeracaoDaGramatica(12, "ES -> escreva ARG ;");
            Enumeracao[12] = new EnumeracaoDaGramatica(13, "ARG -> literal");
            Enumeracao[13] = new EnumeracaoDaGramatica(14, "ARG -> num");
            Enumeracao[14] = new EnumeracaoDaGramatica(15, "ARG -> id");
            Enumeracao[15] = new EnumeracaoDaGramatica(16, "A -> CMD A");
            Enumeracao[16] = new EnumeracaoDaGramatica(17, "CMD -> id rcb LD ;");
            Enumeracao[17] = new EnumeracaoDaGramatica(18, "LD -> OPRD opm OPRD");
            Enumeracao[18] = new EnumeracaoDaGramatica(19, "LD -> OPRD");
            Enumeracao[19] = new EnumeracaoDaGramatica(20, "OPRD -> id");
            Enumeracao[20] = new EnumeracaoDaGramatica(21, "OPRD -> num");
            Enumeracao[21] = new EnumeracaoDaGramatica(22, "A -> COND A");
            Enumeracao[22] = new EnumeracaoDaGramatica(23, "COND -> CABECALHO CORPO");
            Enumeracao[23] = new EnumeracaoDaGramatica(24, "CABECALHO -> se ( EXP_R ) entao");
            Enumeracao[24] = new EnumeracaoDaGramatica(25, "EXP_R -> OPRD opr OPRD");
            Enumeracao[25] = new EnumeracaoDaGramatica(26, "CORPO -> ES CORPO");
            Enumeracao[26] = new EnumeracaoDaGramatica(27, "CORPO -> CMD CORPO");
            Enumeracao[27] = new EnumeracaoDaGramatica(28, "CORPO -> COND CORP");
            Enumeracao[28] = new EnumeracaoDaGramatica(29, "CORPO -> fimse");
            Enumeracao[29] = new EnumeracaoDaGramatica(30, "A -> fim");

            return Enumeracao[line - 1].Sentenca;

        }

        public static String getErro(int line)
        {

            tabelaErros tabelaerrosintatico = new tabelaErros();
            tabelaerrosintatico.tabeladeerros.Add(0, "E0 - Codigo inicializado de forma incorreta - inicio");
            tabelaerrosintatico.tabeladeerros.Add(2, "E2 - Inicio de declaracao de varaveis nao declarado - varfim");
            tabelaerrosintatico.tabeladeerros.Add(3, "E3 - Tipos de dados, elementos de estrutura condicional (entao e fimse), delimitacao de declaracao de variaveis, atribuiao, operadores (relacionais e aritmeticos), inicio de programa e declaracao de variaveis, parenteses ou ponto e virgula nao permitidos");
            tabelaerrosintatico.tabeladeerros.Add(4, "E4 - Espaco para declaracao de variaveis: Necessario declaracao das variaveis, nao sendo permitidos atribuicoes, operacoes, estruturas condicionais, parenteses ou ponto e virgula");
            tabelaerrosintatico.tabeladeerros.Add(6, "E6 - Tipos de dados, elementos de estrutura condicional (entao e fimse), delimitacao de declaracao de variaveis, atribuiao, operadores (relacionais e aritmeticos), inicio de programa e declaracao de variaveis, parenteses ou ponto e virgula nao permitidos");
            tabelaerrosintatico.tabeladeerros.Add(7, "E7 - Tipos de dados, elementos de estrutura condicional (entao e fimse), delimitacao de declaracao de variaveis, atribuiao, operadores (relacionais e aritmeticos), inicio de programa e declaracao de variaveis, parenteses ou ponto e virgula nao permitidos");
            tabelaerrosintatico.tabeladeerros.Add(8, "E8 - Tipos de dados, elementos de estrutura condicional (entao e fimse), delimitacao de declaracao de variaveis, atribuiao, operadores (relacionais e aritmeticos), inicio de programa e declaracao de variaveis, parenteses ou ponto e virgula nao permitidos");
            tabelaerrosintatico.tabeladeerros.Add(9, "E9 - Tipos de dados, elementos de estrutura condicional (entao e fim), delimitacao de declaracao de variaveis, atribuicao, operadores (relacionais e aritmeticos), inicio de programa e declaracao de variaveis, parenteses ou ponto e virgula nao permitidos");
            tabelaerrosintatico.tabeladeerros.Add(11, "E11 - Leitura de variaveis: esperavasse um identificador");
            tabelaerrosintatico.tabeladeerros.Add(12, "E12 - Escrita de variaveis: esperavasse um identificador, um inteiro ou um literal");
            tabelaerrosintatico.tabeladeerros.Add(13, "E13 - Atribuicao de variaveis: esperavasse uma atribuicao");
            tabelaerrosintatico.tabeladeerros.Add(14, "E14 - Inicio de expressao do condicional SE: esperavasse um abre parenteses");
            tabelaerrosintatico.tabeladeerros.Add(16, "E16 - Espaco para declaracao de variaveis: esperavasse um varfim ou um identificador");
            tabelaerrosintatico.tabeladeerros.Add(17, "E17 - Final de atribuicao, expressao, escrita, leitura ou espaco para declaracao de variaveis: esperavasse um ponto e virgula");
            tabelaerrosintatico.tabeladeerros.Add(18, "E18 - Espaco para declaracao de variaveis: esperavasse um tipo para o identificador");
            tabelaerrosintatico.tabeladeerros.Add(23, "E23 - Tipos de dados, elementos de estrutura condicional (entao e fim), delimitacao de declaracao de variaveis, atribuicao, operadores (relacionais e aritmeticos), inicio de programa e declaracao de variaveis, parenteses ou ponto e virgula nao permitidos");
            tabelaerrosintatico.tabeladeerros.Add(24, "E24 - Tipos de dados, elementos de estrutura condicional (entao e fim), delimitacao de declaracao de variaveis, atribuicao, operadores (relacionais e aritmeticos), inicio de programa e declaracao de variaveis, parenteses ou ponto e virgula nao permitidos");
            tabelaerrosintatico.tabeladeerros.Add(25, "E25 - Tipos de dados, elementos de estrutura condicional (entao e fim), delimitacao de declaracao de variaveis, atribuicao, operadores (relacionais e aritmeticos), inicio de programa e declaracao de variaveis, parenteses ou ponto e virgula nao permitidos");
            tabelaerrosintatico.tabeladeerros.Add(27, "E27 - Final de atribuicao, expressao, escrita, leitura ou espaco para declaracao de variaveis: esperavasse um ponto e virgula");
            tabelaerrosintatico.tabeladeerros.Add(28, "E28 - Final de atribuicao, expressao, escrita, leitura ou espaco para declaracao de variaveis: esperavasse um ponto e virgula");
            tabelaerrosintatico.tabeladeerros.Add(32, "E32 - Atribuicao de variaveis ou expressao aritmetica: esperavasse um identificador ou um numeral");
            tabelaerrosintatico.tabeladeerros.Add(33, "E33 - Atribuicao de variaveis ou expressao relacional: esperavasse um identificador ou um numeral");
            tabelaerrosintatico.tabeladeerros.Add(36, "E36 - Final de atribuicao, expressao, escrita, leitura ou espaco para declaracao de variaveis: esperavasse um ponto e virgula");
            tabelaerrosintatico.tabeladeerros.Add(44, "E44 - Final de atribuicao, expressao, escrita, leitura ou espaco para declaracao de variaveis: esperavasse um ponto e virgula");
            tabelaerrosintatico.tabeladeerros.Add(45, "E45 - Expressao aritmetica: esperavasse um operador aritmetica");
            tabelaerrosintatico.tabeladeerros.Add(48, "E48 - Fim de expressao do condicional SE: esperavasse um fecha parenteses");
            tabelaerrosintatico.tabeladeerros.Add(49, "E49 - Expressao relacional: esperavasse um operador relacional");
            tabelaerrosintatico.tabeladeerros.Add(53, "E53 - Expressao aritmetica: esperavasse um identificador ou um numeral");
            tabelaerrosintatico.tabeladeerros.Add(54, "E54 - Estrutura condicional SE: esperavasse um entao");
            tabelaerrosintatico.tabeladeerros.Add(57, "E57 - Expressao relacional: esperavasse um identificador ou um numeral");

            return (String)tabelaerrosintatico.tabeladeerros[line];
        }

        public static int getGOTO(int line, String nonTerminal)
        {
            int[] G = {
                             //      //  //  //  //  //  //  //  //   //  //
            128, 80, 86, 65,133, 68,134,135,136,137,138,139,140,141,142,143,
            0  ,  1,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            1  ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            2  ,  0,  3,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            3  ,  0,  0,  5,  0,  0,  0,  6,  0,  7,  0,  0,  8,  9,  0,  0,
            4  ,  0,  0,  0, 15, 16,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            5  ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            6  ,  0,  0, 19,  0,  0,  0,  6,  0,  7,  0,  0,  8,  9,  0,  0,
            7  ,  0,  0, 20,  0,  0,  0,  6,  0,  7,  0,  0,  8,  9,  0,  0,
            8  ,  0,  0, 21,  0,  0,  0,  6,  0,  7,  0,  0,  8,  9,  0,  0,
            9  ,  0,  0,  0,  0,  0,  0, 23,  0, 24,  0,  0, 25,  9, 22,  0,
            10 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            11 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            12 ,  0,  0,  0,  0,  0,  0,  0, 28,  0,  0,  0,  0,  0,  0,  0,
            13 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            14 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            15 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            16 ,  0,  0,  0, 34, 16,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            17 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            18 ,  0,  0,  0,  0,  0, 36,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            19 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            20 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            21 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            22 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            23 ,  0,  0,  0,  0,  0,  0, 23,  0, 24,  0,  0, 25,  9, 40,  0,
            24 ,  0,  0,  0,  0,  0,  0, 23,  0, 24,  0,  0, 25,  9, 41,  0,
            25 ,  0,  0,  0,  0,  0,  0, 23,  0, 24,  0,  0, 25,  9, 50,  0,
            26 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            27 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            28 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            29 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            30 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            31 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            32 ,  0,  0,  0,  0,  0,  0,  0,  0,  0, 44, 45,  0,  0,  0,  0,
            33 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, 49,  0,  0,  0, 48,
            34 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            35 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            36 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            37 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            38 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            39 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            40 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            41 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            42 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            43 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            44 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            45 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            46 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            47 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            48 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            49 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            50 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            51 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            52 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            53 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, 55,  0,  0,  0,  0,
            54 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            55 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            56 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
            57 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, 58,  0,  0,  0,  0,
            58 ,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
        };

            /*
            1° linha = tabela ASC
            1° coluna = Estados
            LV = 133, TIPO = 134, ES = 135, ARG = 136, CMD = 137, LD = 138, OPRD = 139, COND = 140, CABECALHO = 141, CORPO = 142, EXP_R = 143 
            */
            tabelaSintatico[,] tabelaGOTO = new tabelaSintatico[60, 16];

            int t = 0;

            for (int i = 0; i < 60; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    tabelaGOTO[i, j] = new tabelaSintatico(G[t]);
                    t++;
                }
            }

            int word = 0;//guardar numero de cada variavel na tabela
            int column = 0;

            switch (nonTerminal)
            {
                case "LV":
                    word = 133;
                    break;

                case "TIPO":
                    word = 134;
                    break;

                case "ES":
                    word = 135;
                    break;

                case "ARG":
                    word = 136;
                    break;

                case "CMD":
                    word = 137;
                    break;

                case "LD":
                    word = 138;
                    break;

                case "OPRD":
                    word = 139;
                    break;

                case "COND":
                    word = 140;
                    break;

                case "CABECALHO":
                    word = 141;
                    break;

                case "CORPO":
                    word = 142;
                    break;

                case "EXP_R":
                    word = 143;
                    break;

                case "P":
                    word = 80;
                    break;

                case "V":
                    word = 86;
                    break;

                case "A":
                    word = 65;
                    break;

                case "D":
                    word = 68;
                    break;
            }

            for (int i = 0; i < 16; i++)
            {
                if (tabelaGOTO[0, i].Elemento == word)
                {
                    column = i;
                }
            }

            return tabelaGOTO[line + 1, column].Elemento;
        }

        public static int getACTION(int line, String terminal)
        {
            int[] A =
            {
            128, 133, 134, 135,  59, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146,  40,  41, 147, 148, 149, 150,  36,
              0,   2, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,
              1, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 151,
              2, 132,   4, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,
              3, 132, 132, 132, 132, 132, 132, 132,  11,  13,  12, 132, 132, 132, 132,  14, 132, 132, 132, 132, 132,  10, 132,
              4, 132, 132,  17, 132, 132, 132, 132, 132,  18, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,
              5,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,
              6, 132, 132, 132, 132, 132, 132, 132,  11,  13,  12, 132, 132, 132, 132,  14, 132, 132, 132, 132, 132,  10, 132,
              7, 132, 132, 132, 132, 132, 132, 132,  11,  13,  12, 132, 132, 132, 132,  14, 132, 132, 132, 132, 132,  10, 132,
              8, 132, 132, 132, 132, 132, 132, 132,  11,  13,  12, 132, 132, 132, 132,  14, 132, 132, 132, 132, 132,  10, 132,
              9, 132, 132, 132, 132, 132, 132, 132,  11,  13,  12, 132, 132, 132, 132,  14, 132, 132, 132, 132,  26, 132, 132,
             10, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30, -30,
             11, 132, 132, 132, 132, 132, 132, 132, 132,  27, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,
             12, 132, 132, 132, 132, 132, 132, 132, 132,  31, 132,  29,  30, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,
             13, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,  32, 132, 132, 132, 132, 132, 132, 132, 132, 132,
             14, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,  33, 132, 132, 132, 132, 132, 132,
             15,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,
             16, 132, 132,  17, 132, 132, 132, 132, 132,  18, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,
             17, 132, 132, 132,  35, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,
             18, 132, 132, 132, 132,  37,  38, 132, 132, 132, 132,  39, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,
             19, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10, -10,
             20, -16, -16, -16, -16, -16, -16, -16, -16, -16, -16, -16, -16, -16, -16, -16, -16, -16, -16, -16, -16, -16, -16,
             21, -22, -22, -22, -22, -22, -22, -22, -22, -22, -22, -22, -22, -22, -22, -22, -22, -22, -22, -22, -22, -22, -22,
             22, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23, -23,
             23, 132, 132, 132, 132, 132, 132, 132,  11,  13,  12, 132, 132, 132, 132,  14, 132, 132, 132, 132,  26, 132, 132,
             24, 132, 132, 132, 132, 132, 132, 132,  11,  13,  12, 132, 132, 132, 132,  14, 132, 132, 132, 132,  26, 132, 132,
             25, 132, 132, 132, 132, 132, 132, 132,  11,  13,  12, 132, 132, 132, 132,  14, 132, 132, 132, 132,  26, 132, 132,
             26, -29, -29, -29, -29, -29, -29, -29, -29, -29, -29, -29, -29, -29, -29, -29, -29, -29, -29, -29, -29, -29, -29,
             27, 132, 132, 132,  42, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,
             28, 132, 132, 132,  43, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,
             29, -13, -13, -13, -13, -13, -13, -13, -13, -13, -13, -13, -13, -13, -13, -13, -13, -13, -13, -13, -13, -13, -13,
             30, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14, -14,
             31, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15, -15,
             32, 132, 132, 132, 132, 132, 132, 132, 132,  46, 132, 132,  47, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,
             33, 132, 132, 132, 132, 132, 132, 132, 132,  46, 132, 132,  47, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,
             34,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,
             35,  -5,  -5,  -5,  -5,  -5,  -5,  -5,  -5,  -5,  -5,  -5,  -5,  -5,  -5,  -5,  -5,  -5,  -5,  -5,  -5,  -5,  -5,
             36, 132, 132, 132,  51, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,
             37,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,  -7,
             38,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,
             39,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,  -9,
             40, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26, -26,
             41, -27, -27, -27, -27, -27, -27, -27, -27, -27, -27, -27, -27, -27, -27, -27, -27, -27, -27, -27, -27, -27, -27,
             42, -11, -11, -11, -11, -11, -11, -11, -11, -11, -11, -11, -11, -11, -11, -11, -11, -11, -11, -11, -11, -11, -11,
             43, -12, -12, -12, -12, -12, -12, -12, -12, -12, -12, -12, -12, -12, -12, -12, -12, -12, -12, -12, -12, -12, -12,
             44, 132, 132, 132,  52, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,
             45, 132, 132, 132, -19, 132, 132, 132, 132, 132, 132, 132, 132, 132,  53, 132, 132, 132, 132, 132, 132, 132, 132,
             46, -20, -20, -20, -20, -20, -20, -20, -20, -20, -20, -20, -20, -20, -20, -20, -20, -20, -20, -20, -20, -20, -20,
             47, -21, -21, -21, -21, -21, -21, -21, -21, -21, -21, -21, -21, -21, -21, -21, -21, -21, -21, -21, -21, -21, -21,
             48, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,  54, 132, 132, 132, 132, 132,
             49, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,  57, 132, 132, 132,
             50, -28, -28, -28, -28, -28, -28, -28, -28, -28, -28, -28, -28, -28, -28, -28, -28, -28, -28, -28, -28, -28, -28,
             51,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,  -6,
             52, -17, -17, -17, -17, -17, -17, -17, -17, -17, -17, -17, -17, -17, -17, -17, -17, -17, -17, -17, -17, -17, -17,
             53, 132, 132, 132, 132, 132, 132, 132, 132,  46, 132, 132,  47, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,
             54, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,  56, 132, 132, 132, 132,
             55, -18, -18, -18, -18, -18, -18, -18, -18, -18, -18, -18, -18, -18, -18, -18, -18, -18, -18, -18, -18, -18, -18,
             56, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24, -24,
             57, 132, 132, 132, 132, 132, 132, 132, 132,  46, 132, 132,  47, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132,
             58, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25, -25
        };
            //inicio = 133, varinicio = 134, varfim = 135, int(inteiro) = 136, real = 137, lit = 138, leia = 139, id = 140, escreva = 141, literal = 142, num = 143, rcb = 144
            //opm = 145, se = 146, então = 147, opr = 148, fimse = 149, fim = 150.


            tabelaSintatico[,] tabelaACTION = new tabelaSintatico[60, 23];
            int t = 0;

            for (int i = 0; i < 60; i++)
            {
                for (int j = 0; j < 23; j++)
                {
                    tabelaACTION[i, j] = new tabelaSintatico(A[t]);
                    t++;
                }
            }
            int word = 0;
            int column = 0;

            switch (terminal)
            {
                case "inicio":
                    word = 133;
                    break;

                case "varinicio":
                    word = 134;
                    break;

                case "varfim":
                    word = 135;
                    break;

                case "inteiro":
                    word = 136;
                    break;

                case "real":
                    word = 137;
                    break;

                case "lit":
                    word = 138;
                    break;

                case "leia":
                    word = 139;
                    break;

                case "escreva":
                    word = 141;
                    break;

                case "literal":
                    word = 142;
                    break;

                case "num":
                    word = 143;
                    break;

                case "rcb":
                    word = 144;
                    break;

                case "opm":
                    word = 145;
                    break;

                case "se":
                    word = 146;
                    break;

                case "entao":
                    word = 147;
                    break;

                case "opr":
                    word = 148;
                    break;

                case "fimse":
                    word = 149;
                    break;

                case "fim":
                    word = 150;
                    break;

                case "pt_v":
                    word = 59;
                    break;

                case "ab_p":
                    word = 40;
                    break;

                case "fc_p":
                    word = 41;
                    break;

                case "EOF":
                    word = 36;
                    break;

                case "id":
                    word = 140;
                    break;
            }

            for (int i = 0; i < 23; i++)
            {
                if (tabelaACTION[0, i].Elemento == word)
                {
                    column = i;
                }
            }

            return tabelaACTION[line + 1, column].Elemento;

        }

        public static void escreverTXT2(string texto)
        {
            if (IniciadoC == 0)
            {
                using (StreamWriter writer = new StreamWriter("C:\\Users\\lucas\\Desktop\\Compilador\\PROGRAMA.c"))
                {
                    writer.WriteLine(texto);
                    IniciadoC = 1;
                    writer.Close();

                }
            }
            else if (IniciadoC == 1)
            {
                using (StreamWriter writer = new StreamWriter("C:\\Users\\lucas\\Desktop\\Compilador\\PROGRAMA.c", true))
                {
                    writer.WriteLine(texto);
                    writer.Close();
                }
            }
        }


        public static void escreverPontoC(int tx_18, int tx_25)
        {
            var linhas = File.ReadAllLines("C:\\Users\\lucas\\Desktop\\Compilador\\Texto2.txt");
            escreverTXT2("#include<stdio.h>");
            escreverTXT2("typedef char literal[256];");
            escreverTXT2("void main(void){");
            escreverTXT2("/*----Variaveis temporarias----*/");
            
            for (int i = 0; i < tx_18; i++)
            {
                escreverTXT2("int T" + i + ";");
            }
            for (int i = tx_18; i < tx_18 + tx_25; i++)
            {
                escreverTXT2("int T" + i + ";");
            }

            escreverTXT2("/*------------------------------*/");

            
            string[] k = File.ReadAllLines("C:\\Users\\lucas\\Desktop\\Compilador\\Texto2.txt");
            
            foreach(string value in k)
            {
                escreverTXT2(value);
            }

            escreverTXT2("}");

        }




        public static string[] regras_sem = {
            "-",
            "-",
            "-",
            "-",
            "Imprimir três linhas brancas no arquivo objeto;",
            "id.tipo <- TIPO.tipo\nImprimir ( TIPO.tipo id.lexema ; )",
            "TIPO.tipo <- inteiro.tipo",
            "TIPO.tipo <- real.tipo",
            "TIPO.tipo <- literal.tipo",
            "-",
            "Verificar se o campo tipo do identificador está preenchido indicando a declaração do identificador (execução da regra semântica de número 6).\nSe sim, então:\n\t Se id.tipo = literal Imprimir ( scanf(“%s”, id.lexema); )\n\t Se id.tipo = inteiro Imprimir ( scanf(“%d”, &id.lexema); )\n\t Se id.tipo = real Imprimir ( scanf(“%lf”, &id.lexema); )\nCaso Contrário:\n\tEmitir na tela “Erro: Variável não declarada”.",
            "Gerar código para o comando escreva no arquivo objeto.\nImprimir ( printf(“ARG.lexema”); )",
            "ARG.atributos <- literal.atributos (Copiar todos os atributos de literal para os atributos de ARG).",
            "ARG.atributos <- num.atributos (Copiar todos os atributos de literal para os atributos de ARG).",
            "Verificar se o identificador foi declarado (execução da regra semântica de número 6).\nSe sim, então:\n\tARG.atributos <- id.atributos (copia todos os atributos de id para os de ARG).\nCaso Contrário:\n\tEmitir na tela “Erro: Variável não declarada”",
            "-",
            "Verificar se id foi declarado (execução da regra semântica de número 6).\nSe sim, então:\n\tRealizar verificação do tipo entre os operandos id e LD (ou seja, se ambos são do mesmo tipo).\n\tSe sim, então:\n\t\tImprimir (id.lexema rcb.tipo LD.lexema) no arquivo objeto.\n\tCaso contrário emitir:”Erro: Tipos diferentes para atribuição”.\nCaso contrário emitir “Erro: Variável não declarada”.",
            "Verificar se tipo dos operandos são equivalentes e diferentes de literal.\nSe sim, então:\n\tGerar uma variável numérica temporária Tx, em que x é um número gerado sequencialmente.\nLD.lexema <- Tx\nImprimir (Tx = OPRD.lexema opm.tipo OPRD.lexema) no arquivo objeto.\nCaso contrário emitir “Erro: Operandos com tipos incompatíveis”.",
            "LD.atributos <- OPRD.atributos (Copiar todos os atributos de OPRD para os atributos de LD).",
            "Verificar se o identificador está declarado.\nSe sim, então:\n\tOPRD.atributos	<- id.atributos\nCaso contrário emitir “Erro: Variável não declarada”",
            "OPRD.atributos	<- num.atributos (Copiar todos os atributos de num para os atributos de OPRD).",
            "-",
            "Imprimir ( } ) no arquivo objeto.",
            "Imprimir ( if (EXP_R.lexema) { ) no arquivo objeto.",
            "Verificar se os tipos de dados de OPRD são iguais ou equivalentes para a realização de comparação relacional.\nSe sim, então:\n\tGerar uma variável booleana temporária Tx, em que x é um número gerado sequencialmente.\n\tEXP_R.lexema <-	Tx\n\tImprimir (Tx = OPRD.lexema opr.tipo OPRD.lexema) no arquivo objeto.\nCaso contrário emitir “Erro: Operandos com tipos incompatíveis”.",
            "-",
            "-",
            "-",
            "-",
            "-"
        };


    }
}
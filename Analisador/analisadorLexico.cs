using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Tabelas;
using Controle;

namespace Analisador
{
    public class analisadorLexico
    {
        public static int pos;
        public static int linhaerro = 1, colunaerro = 0;
        public static int erro = 0;

        static BufferedStream lerArquivo;

        public static controleDadosSimbolos getLex(int Posi)
        {
            pos = Posi;

            long tam;

            int []S = {128,  1,  0, 69, 43, 45, 42, 47, 62, 60, 61, 40, 41, 59, 34,129,123,125,130, 10, 32, 32, 46, 95,
                    131, 19, 25,132, 11, 12, 13, 14,  4,  1, 26,  9, 10,  8, 15,132, 17,132,  3,131,131,131,132,132,
                      1,  0,  0,  0,  0,  6,  0,  0,  7,  0,  2,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                      2,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                      3,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                      4,  0,  0,  0,  0,  0,  0,  0,  0,  0,  5,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                      5,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                      6,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                      7,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                      8,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                      9,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                     10,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                     11,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                     12,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                     13,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                     14,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                     15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 16, 132, 15, 15,132, 15, 15, 15, 15, 15,
                       16,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                     17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 132, 17, 18,132, 17, 17, 17, 17, 17,
                     18,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                     19, 19,  0, 22,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, 20,  0,
                     20, 21,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,
                     21, 21,  0, 22,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                     22, 24,132,132, 23, 23,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,
                     23, 24,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,
                     24, 24,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                     25, 25, 25,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, 25,
                     26,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
                    132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132,132};

            Stack<int> estados = new Stack<int>();

            tabelaTransicao[,] tabelaT = new tabelaTransicao[29,24];

            int t = 0;
            for (int i = 0; i < 29; i++)
            {
                for(int j = 0; j < 24; j++)
                {
                    tabelaT[i, j] = new tabelaTransicao(S[t]);
                    t++;
                }
            }
            
            if(pos == 0)
            {
                tabelaSimbolo.startTabelaSimbolos();
            }

            tabelaErros tabelahashe = new tabelaErros();

            tabelahashe.tabeladeerros.Add(1, "Identificador não permitido");
            tabelahashe.tabeladeerros.Add(16, "Constantes literais nao permitidas");
            tabelahashe.tabeladeerros.Add(18, "Erro de foramatacao de comentario (chaves)");
            tabelahashe.tabeladeerros.Add(21, "Constantes numericas nao permitidas");
            tabelahashe.tabeladeerros.Add(23, "Constantes numericas nao permitidas");
            tabelahashe.tabeladeerros.Add(24, "Constantes numericas nao permitidas");

            try
            {
                StringBuilder bffCaracter = new StringBuilder();
                StreamReader stream = File.OpenText("C:\\Users\\lucas\\Downloads\\Compilador-em-Java-master\\Compilador-em-Java-master\\texto.txt");

                int caracter = 0, linha = 1, coluna = 0;

                stream.BaseStream.Position = pos;

                //lerarquivo...

                tam = stream.BaseStream.Length;

                estados.Push(tabelaT[linha, coluna].Elemento);
                //
                //Console.WriteLine("\nTabela de Transição, linha " + linha + " coluna " + coluna + " é " + tabelaT[linha, coluna].Elemento);
                //
                while (pos <= tam || erro == 0)
                {
                    //
                    //Console.WriteLine("\nTamanho: " + tam + " Posição: " + pos);
                    //
                    caracter = stream.Read();
                    int test = 0;

                    //
                    //Console.WriteLine("\nCaracter em ascII: " + caracter + " Caracter: " + (char)caracter);
                    //

                    for (int i = 0; i < 24; i++)
                    {
                        if (tabelaT[0, i].Elemento == caracter)
                        {
                            coluna = i;
                            test = i;
                        }
                    }

                    //
                    //Console.WriteLine("\nColuna do caracter: " + coluna + " Teste: " + test);
                    //

                    if (test == 0 && caracter != 10 && caracter != 13 && caracter == 32)
                    {
                        coluna = 17;
                        //
                        //Console.WriteLine("\n1 if Coluna do caracter: " + coluna + " Teste: " + test);
                        //
                    }

                    if (test == 0 && (caracter == 10 || caracter == 13 || caracter == 32))
                    {
                        coluna = 19;
                        //
                        //Console.WriteLine("\n2 if Coluna do caracter: " + coluna + " Teste: " + test);
                        //
                    }

                    if (caracter == -1)
                    {
                        coluna = 18;
                        //
                        //Console.WriteLine("\n3 if Coluna do caracter: " + coluna + " Teste: " + test);
                        //
                    }

                    if (char.IsDigit((char)caracter))
                    {
                        coluna = 1;
                        //
                        //Console.WriteLine("\nÉ digito Coluna do caracter: " + coluna + " Teste: " + test);
                        //
                    }

                    /*
                    //tratar o espaço no literal
                    if(coluna == 21 && caracter == 32)
                    {
                        bffCaracter.Append((char)caracter);
                    }
                    //
                    */

                    //para o /
                    if (caracter == 92)
                    {
                        coluna = 2;
                    }

                    if (char.IsLetter((char)caracter))
                    {
                        coluna = 2;
                        //
                        //Console.WriteLine("\nÉ letra Coluna do caracter: " + coluna + " Teste: " + test);
                        //
                    }

                    if ((caracter == 69 || caracter == 101) && (estados.Peek() == 19 || estados.Peek() == 21))
                    {
                        coluna = 3;
                        //
                        //Console.WriteLine("\n4 if Coluna do caracter: " + coluna + " Teste: " + test);
                        //
                    }
                    //
                    //Console.WriteLine("\nCabeça da pilha:" + estados.Peek());
                    //

                    for (int i = 0; i < 29; i++)
                    {
                        if (tabelaT[i, 0].Elemento == estados.Peek())
                        {
                            linha = i;
                        }
                    }
                    //
                    //Console.WriteLine("\nLinha: " + linha);
                    //

                    estados.Push(tabelaT[linha, coluna].Elemento);

                    //
                    //Console.WriteLine("\nEstado: " + estados.Peek());
                    //
                    
                    if (tabelaT[linha, coluna].Elemento == 0)
                    {
                        //
                        //Console.WriteLine("\nBuffer: "+bffCaracter.ToString());
                        //
                        if (tabelaSimbolo.tabelaS.ContainsKey(bffCaracter.ToString()) == true)
                        {
                            controleDadosSimbolos aux;
                            aux = (controleDadosSimbolos)tabelaSimbolo.tabelaS[bffCaracter.ToString()];
                            return aux;
                        }
                        else
                        {
                            controleDadosSimbolos simaux;
                            estados.Pop();
                            switch (estados.Peek())
                            {
                                case 1:
                                    simaux = new controleDadosSimbolos(bffCaracter.ToString(), "opr", " ");
                                    return simaux;
                                case 2:
                                    simaux = new controleDadosSimbolos(bffCaracter.ToString(), "opr", " ");
                                    return simaux;
                                case 3:
                                    simaux = new controleDadosSimbolos("EOF", "EOF", " ");
                                    return simaux;
                                case 4:
                                    simaux = new controleDadosSimbolos(bffCaracter.ToString(), "opr", " ");
                                    return simaux;
                                case 5:
                                    simaux = new controleDadosSimbolos(bffCaracter.ToString(), "opr", " ");
                                    return simaux;
                                case 6:
                                    simaux = new controleDadosSimbolos(bffCaracter.ToString(), "rcb", " ");
                                    return simaux;
                                case 7:
                                    simaux = new controleDadosSimbolos(bffCaracter.ToString(), "opr", " ");
                                    return simaux;
                                case 8:
                                    simaux = new controleDadosSimbolos(bffCaracter.ToString(), "pt_v", " ");
                                    return simaux;
                                case 9:
                                    simaux = new controleDadosSimbolos(bffCaracter.ToString(), "ab_p", " ");
                                    return simaux;
                                case 10:
                                    simaux = new controleDadosSimbolos(bffCaracter.ToString(), "fc_p", " ");
                                    return simaux;
                                case 11:
                                    simaux = new controleDadosSimbolos(bffCaracter.ToString(), "opm", " ");
                                    return simaux;
                                case 12:
                                    simaux = new controleDadosSimbolos(bffCaracter.ToString(), "opm", " ");
                                    return simaux;
                                case 13:
                                    simaux = new controleDadosSimbolos(bffCaracter.ToString(), "opm", " ");
                                    return simaux;
                                case 14:
                                    simaux = new controleDadosSimbolos(bffCaracter.ToString(), "opm", " ");
                                    return simaux;
                                case 26:
                                    simaux = new controleDadosSimbolos(bffCaracter.ToString(), "opm", " ");
                                    return simaux;
                                case 16:
                                    simaux = new controleDadosSimbolos(bffCaracter.ToString(), "literal", "literal");
                                    return simaux;
                                case 18:
                                    Console.WriteLine("Comentario " + bffCaracter);
                                    break;
                                case 19:
                                    simaux = new controleDadosSimbolos(bffCaracter.ToString(), "num", "inteiro");
                                    return simaux;
                                case 21:
                                    simaux = new controleDadosSimbolos(bffCaracter.ToString(), "num", "real");
                                    return simaux;
                                case 24:
                                    simaux = new controleDadosSimbolos(bffCaracter.ToString(), "num", "real");
                                    return simaux;
                                case 25:
                                    simaux = new controleDadosSimbolos(bffCaracter.ToString(), "id", " ");
                                    tabelaSimbolo.tabelaS.Add(simaux.Lexema, simaux);
                                    return simaux;
                                default:
                                    Console.WriteLine("Erro na leitura da pilha ou formato do comentário errado!");
                                    Console.WriteLine("\nLinha" + linhaerro + " Coluna " + colunaerro);
                                    erro = 1;
                                    break;
                            }
                        }

                        estados.Clear();
                        estados.Push(tabelaT[1, coluna].Elemento);
                        bffCaracter.Remove(0, bffCaracter.Length);
                    }

                    if(tabelaT[linha,coluna].Elemento == 132)
                    {
                        Console.WriteLine("ERRO ENCONTRADO - " + tabelahashe.tabeladeerros[linha]);
                        Console.WriteLine("\nlinha na tabela: " + linha + " Coluna: " + coluna);
                        Console.WriteLine("\nLinha: " + linhaerro + " Coluna: " + colunaerro);

                        erro = 1;
                        controleDadosSimbolos simerro = new controleDadosSimbolos("ERRO", "ERRO", " ");
                        return simerro;
                    }

                    if (caracter != 10 && caracter != 13 && caracter != 32)
                    {
                        bffCaracter.Append((char) caracter);
                    }

                    //
                    //Console.WriteLine("\nBuffer: " + bffCaracter.ToString());
                    //

                    pos++;

                    if(caracter == 13)
                    {
                        linhaerro++;
                        colunaerro = 0;
                    }
                    else
                    {
                        colunaerro++;
                    }
                }

                stream.Close();
                lerArquivo.Close();

            } catch (Exception e)
            {
                Console.Error.WriteLine("Erro na abertura do arquivo: %s.\n", e);
            }
            return null;
           
        }

       public static void getLinhaeColuna()
        {
            Console.WriteLine("Linha: " + linhaerro + " Coluna" + colunaerro);
        }
    }
}

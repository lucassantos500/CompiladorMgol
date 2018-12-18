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
        public static int pos = 0, erro = 0, linhaerro = 1, colunaerro = 0;
        //public static tabelaSimbolo tabelahash;
        public static Dictionary<string, controleDadosSimbolos> tabelaSimbolo = new Dictionary<string, controleDadosSimbolos>();

        //pos = Posição que a leitura esta sendo feita
        //linhaerro, colunaerro = Linha e coluna que ocorreu o erro
        //erro = Se ocorreu erro ou não

        public static void Preencher_analisadorLexico()
        {
            controleDadosSimbolos sim1 = new controleDadosSimbolos("inicio", "inicio", " ");
            tabelaSimbolo.Add("inicio", sim1);
            //tabeladesimbolos.Add("inicio", sim1);
            controleDadosSimbolos sim2 = new controleDadosSimbolos("varinicio", "varinicio", " ");
            tabelaSimbolo.Add("varinicio", sim2);
            //tabelahash.tabeladesimbolos.Add("varinicio", sim2);
            controleDadosSimbolos sim3 = new controleDadosSimbolos("varfim", "varfim", " ");
            tabelaSimbolo.Add("varfim", sim3);
            //tabelahash.tabeladesimbolos.Add("varfim", sim3);
            controleDadosSimbolos sim4 = new controleDadosSimbolos("escreva", "escreva", " ");
            tabelaSimbolo.Add("escreva", sim4);
            //tabelahash.tabeladesimbolos.Add("escreva", sim4);
            controleDadosSimbolos sim5 = new controleDadosSimbolos("leia", "leia", " ");
            tabelaSimbolo.Add("leia", sim5);
            //tabelahash.tabeladesimbolos.Add("leia", sim5);
            controleDadosSimbolos sim6 = new controleDadosSimbolos("se", "se", " ");
            tabelaSimbolo.Add("se", sim6);
            //tabelahash.tabeladesimbolos.Add("se", sim6);
            controleDadosSimbolos sim7 = new controleDadosSimbolos("entao", "entao", " ");
            tabelaSimbolo.Add("entao", sim7);
            //tabelahash.tabeladesimbolos.Add("entao", sim7);
            controleDadosSimbolos sim8 = new controleDadosSimbolos("fimse", "fimse", " ");
            tabelaSimbolo.Add("fimse", sim8);
            //tabelahash.tabeladesimbolos.Add("fimse", sim8);
            controleDadosSimbolos sim9 = new controleDadosSimbolos("fim", "fim", " ");
            tabelaSimbolo.Add("fim", sim9);
            //tabelahash.tabeladesimbolos.Add("fim", sim9);
            controleDadosSimbolos sim10 = new controleDadosSimbolos("inteiro", "inteiro", " ");
            tabelaSimbolo.Add("inteiro", sim10);
            //tabelahash.tabeladesimbolos.Add("inteiro", sim10);
            controleDadosSimbolos sim11 = new controleDadosSimbolos("literal", "literal", " ");
            tabelaSimbolo.Add("literal", sim11);
            //tabelahash.tabeladesimbolos.Add("literal", sim11);
            controleDadosSimbolos sim12 = new controleDadosSimbolos("real", "real", " ");
            tabelaSimbolo.Add("real", sim12);
            //tabelahash.tabeladesimbolos.Add("real", sim12);
        }

        public static controleDadosSimbolos getLex(int Posi) //Função que lê um arquivo, acha lexemas, insere na tabela e retorna o lexema
        {
            pos = Posi; //Atualização da posição da leitura
            long tam; //Tamanho da Stream

            int[] S = {128,  1,  0, 69, 43, 45, 42, 47, 62, 60, 61, 40, 41, 59, 34,129,123,125,130, 10, 32, 32, 46, 95,
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

            Stack<int> estados = new Stack<int>();//Pilha dos estados da tabela
            tabelaTransicao[,] tabelaT = new tabelaTransicao[29, 24];//Tabela de transição

            int t = 0;                                                  //
            for (int i = 0; i < 29; i++)                               //
            {                                                         //
                for (int j = 0; j < 24; j++)                          //
                {                                                   //Adicionando o vetor S a tabela de transição
                    tabelaT[i, j] = new tabelaTransicao(S[t]);     //
                    t++;                                          //
                }                                                //
            }                                                   //

            tabelaErros tabelahashe = new tabelaErros();//Tabela de erros simples

            tabelahashe.tabeladeerros.Add(1, "Identificador não permitido");                           //
            tabelahashe.tabeladeerros.Add(16, "Constantes literais nao permitidas");                  //
            tabelahashe.tabeladeerros.Add(18, "Erro de foramatacao de comentario (chaves)");         //
            tabelahashe.tabeladeerros.Add(21, "Constantes numericas nao permitidas");              //
            tabelahashe.tabeladeerros.Add(23, "Constantes numericas nao permitidas");             //
            tabelahashe.tabeladeerros.Add(24, "Constantes numericas nao permitidas");            //

            //Tente
            try
            {
                StringBuilder bffCaracter = new StringBuilder();//Criando o buffer de caracteres
                StreamReader stream = File.OpenText("C:\\Users\\lucas\\Desktop\\Compilador\\FONTE.ALG");//Abrindo o arquivo (Mudar caso necessário)

                int caracter = 0, linha = 1, coluna = 0;//Caracter = caracter atual
                stream.BaseStream.Position = pos;//Setando a leitura do arquivo para a posição necessária
                tam = stream.BaseStream.Length;//Tamanho total do arquivo
                estados.Push(tabelaT[linha, coluna].Elemento);//Colocando na tabela de estados o estado inicial 131

                while (pos <= tam || erro == 0)//Enquanto a posição for menor que o tamanho total do arquivo ou encontrar algum erro
                {
                    caracter = stream.Read();//Lendo o caracter atual
                    int test = 0;//Teste se o caracter esta na tabela ou não

                    for (int i = 0; i < 24; i++)                    //  
                    {                                              //
                        if (tabelaT[0, i].Elemento == caracter)   //
                        {                                        //
                            coluna = i;                         //Testando se está ou não na tabela de transição
                            test = 1;                          //
                        }                                     //
                    }                                        //
                                                             //Caso o caracter não esteja na tabela

                    if (test == 0 && caracter != 10 && caracter != 13 && caracter == 32)//Coloque ele na coluna que representa a coluna dos outros caracteres
                    {
                        coluna = 17;
                    }

                    if (test == 0 && (caracter == 10 || caracter == 13 || caracter == 32))//Verifico se o caracter é espaço, tab ou quebra linha
                    {
                        coluna = 19;
                    }

                    if (caracter == -1)//Verifico se o caracter é de fim de arquivo 
                    {
                        coluna = 18;
                    }

                    if (char.IsDigit((char)caracter))//Verifico se o caracter é digido
                    {
                        coluna = 1;
                    }

                    //Tratando o problema do espaço no literal
                    if (coluna == 21 && caracter == 32 && estados.Peek() == 15)//Se for espaço e o estado atual é literal não ignore
                    {
                        bffCaracter.Append((char)caracter);
                    }

                    //tratando o problema do \n
                    if (caracter == 92 && estados.Peek() == 15)//Se for \n e o estado atual é de literal não ignore
                    {
                        coluna = 2;
                    }

                    if (char.IsLetter((char)caracter))//verificando se o caracter é letra
                    {
                        coluna = 2;
                    }

                    if ((caracter == 69 || caracter == 101) && (estados.Peek() == 19 || estados.Peek() == 21))//Tratando e|E
                    {
                        coluna = 3;
                    }

                    //Procura o estado atual que se encontra
                    for (int i = 0; i < 29; i++)
                    {
                        if (tabelaT[i, 0].Elemento == estados.Peek())
                        {
                            linha = i;
                        }
                    }

                    estados.Push(tabelaT[linha, coluna].Elemento);//Adicionana na tabela de estados

                    //Verificando se o estado atual é vazio ou de erro
                    if (tabelaT[linha, coluna].Elemento == 0)//Caso seja o fim de um lexema
                    {
                        if(tabelaSimbolo.ContainsKey(bffCaracter.ToString()))
                        //if (tabelahash.tabeladesimbolos.ContainsKey(bffCaracter.ToString()) == true)//Verifica se este lexema já esta na tabela
                        {
                            controleDadosSimbolos aux;
                            //aux = (controleDadosSimbolos)tabelahash.tabeladesimbolos[bffCaracter.ToString()];
                            aux = tabelaSimbolo[bffCaracter.ToString()];
                            return aux;//Se já estiver na tabela retorne ele
                        }
                        else//Caso não esteja na tabela - Buscar qual token este estado pertence
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
                                    tabelaSimbolo.Add(simaux.Lexema, simaux);

                                    return simaux;
                                default:
                                    Console.WriteLine("Erro na leitura da pilha ou formato do comentário errado!");
                                    Console.WriteLine("\nLinha" + linhaerro + " Coluna " + colunaerro);
                                    erro = 1;
                                    break;
                            }
                        }

                        estados.Clear();//Limpar a tabela de estados após 
                        estados.Push(tabelaT[1, coluna].Elemento);//Adiciona o estado inicial
                        bffCaracter.Remove(0, bffCaracter.Length);//E limpa o buffer
                    }

                    if (tabelaT[linha, coluna].Elemento == 132)//Caso o estado atual seja de erro
                    {

                        Console.WriteLine("ERRO ENCONTRADO - " + tabelahashe.tabeladeerros[linha]);        //
                        Console.WriteLine("\nlinha na tabela: " + linha + " Coluna: " + coluna);          //
                        Console.WriteLine("\nLinha: " + linhaerro + " Coluna: " + colunaerro);           //Retornando o erro
                        erro = 1;                                                                       //
                        controleDadosSimbolos simerro = new controleDadosSimbolos("ERRO", "ERRO", " ");//
                        return simerro;                                                               //
                    }

                    //Caso o caracter atual não seja espaço, tab ou quebra linha guarde ele no buffer
                    if (caracter != 10 && caracter != 13 && caracter != 32)
                    {
                        bffCaracter.Append((char)caracter);
                    }

                    pos++; //Avança a posição

                    if (caracter == 13)
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

            }
            catch (Exception e)//Caso não consiga abrir o arquivo
            {
                Console.Error.WriteLine("Erro na abertura do arquivo:" + e + "\n");//retorne o erro
            }
            return null;

        }

        public static void getLinhaColuna()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nLinha: " + linhaerro + " Coluna: " + colunaerro);
            Console.ResetColor();
        }

        public static void att_tipo(string id, string tipo)
        {
          
            tabelaSimbolo[id].Tipo = tipo;
            
        }


        public static bool existe(string lexema)
        {
            if(tabelaSimbolo.ContainsKey(lexema))
            {

                if (tabelaSimbolo[lexema].Tipo != " ")
                {
                    return true;
                }
            }
            return false;
        }

    }
}

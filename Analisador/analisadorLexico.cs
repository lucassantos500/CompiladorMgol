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
    class analisadorLexico
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

            tabelahashe.addnatabela(1, "Identificador não permitido");
            tabelahashe.addnatabela(16, "Constantes literais nao permitidas");
            tabelahashe.addnatabela(18, "Erro de foramatacao de comentario (chaves)");
            tabelahashe.addnatabela(21, "Constantes numericas nao permitidas");
            tabelahashe.addnatabela(23, "Constantes numericas nao permitidas");
            tabelahashe.addnatabela(24, "Constantes numericas nao permitidas");

            try
            {
                StringBuilder bffCaracter = new StringBuilder();
                StreamReader stream = File.OpenText("C:\\Users\\lucas\\Downloads\\Compilador-em-Java-master\\Compilador-em-Java-master\\texto.txt");

                int caracter = 0, linha = 1, coluna = 0;

                stream.BaseStream.Position = pos;

                //lerarquivo...

                tam = stream.BaseStream.Length;

                estados.Push(tabelaT[linha, coluna].Elemento);

                while (pos <= tam || erro == 0)
                {
                    caracter = stream.Read();
                    int test = 0;

                    for(int i = 0; i < 24; i++)
                    {
                        if (tabelaT[0, i].Elemento == caracter)
                        {
                            coluna = i;
                            test = i;
                        }
                    }

                    if(test == 0 && caracter != 10 && caracter != 13 && caracter == 32)
                    {
                        coluna = 17;
                    }

                    if (test == 0 && (caracter == 10 || caracter == 13 || caracter == 32))
                    {
                        coluna = 19;
                    }

                    if (caracter == -1)
                    {
                        coluna = 18;
                    }
                    


                }
                
                

                
            } catch (Exception e)
            {
                //erro de abertura de arquivo
            }

            return null;
        }

       
    }
}

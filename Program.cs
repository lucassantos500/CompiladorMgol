using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analisador;
using Controle;
using Tabelas;
//Classe temporaria que representa a MAIN


namespace CompiladorMgol
{
    class Program
    {
        static void Main(string[] args)
        {
            controleDadosSimbolos simb;
            do
            {
                simb = analisadorLexico.getLex(analisadorLexico.pos);
                Console.WriteLine("--------------------");
                Console.WriteLine("\nToken: " + simb.Token + " \nLexema: " + simb.Lexema + " \nTipo: " + simb.Tipo);
                
            } while (simb.Lexema != "EOF" && simb.Lexema != "ERRO");
            Console.ReadKey();//Pausando para leitura
        }
    }
}

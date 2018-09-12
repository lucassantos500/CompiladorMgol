using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using Controle;

namespace Tabelas
{
    class tabelaSimbolo
    {
        public static Hashtable tabelaS;

        public static void startTabelaSimbolos()
        {
            tabelaSimbolo.tabelaS = new Hashtable();
            controleDadosSimbolos simbolo1 = new controleDadosSimbolos("inicio", "inicio", " ");
            tabelaSimbolo.tabelaS.Add("inicio", simbolo1);
            controleDadosSimbolos simbolo2 = new controleDadosSimbolos("varinicio", "varinicio", " ");
            tabelaSimbolo.tabelaS.Add("varinicio", simbolo2);
            controleDadosSimbolos simbolo3 = new controleDadosSimbolos("varfim", "varfim", " ");
            tabelaSimbolo.tabelaS.Add("varfim", simbolo3);
            controleDadosSimbolos simbolo4 = new controleDadosSimbolos("escreva", "escreva", " ");
            tabelaSimbolo.tabelaS.Add("escreva", simbolo4);
            controleDadosSimbolos simbolo5 = new controleDadosSimbolos("leia", "leia", " ");
            tabelaSimbolo.tabelaS.Add("leia", simbolo5);
            controleDadosSimbolos simbolo6 = new controleDadosSimbolos("se", "se", " ");
            tabelaSimbolo.tabelaS.Add("se", simbolo6);
            controleDadosSimbolos simbolo7 = new controleDadosSimbolos("entao", "entao", " ");
            tabelaSimbolo.tabelaS.Add("entao", simbolo7);
            controleDadosSimbolos simbolo8 = new controleDadosSimbolos("fimse", "fimse", " ");
            tabelaSimbolo.tabelaS.Add("fimse", simbolo8);
            controleDadosSimbolos simbolo9 = new controleDadosSimbolos("fim", "fim", " ");
            tabelaSimbolo.tabelaS.Add("fim", simbolo9);
            controleDadosSimbolos simbolo10 = new controleDadosSimbolos("inteiro", "inteiro", " ");
            tabelaSimbolo.tabelaS.Add("inteiro", simbolo10);
            controleDadosSimbolos simbolo11 = new controleDadosSimbolos("literal", "literal", " ");
            tabelaSimbolo.tabelaS.Add("literal", simbolo11);
            controleDadosSimbolos simbolo12 = new controleDadosSimbolos("real", "real", " ");
            tabelaSimbolo.tabelaS.Add("real", simbolo12);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using Controle;
//Classe que representa a tabela de símbolos

namespace Tabelas
{
    public class tabelaSimbolo
    {
        public Hashtable tabeladesimbolos;

        //Função que carrega os tokens iniciais na tabela de símbolos

        public tabelaSimbolo()
        {
            tabeladesimbolos = new Hashtable();
        }

    }
}

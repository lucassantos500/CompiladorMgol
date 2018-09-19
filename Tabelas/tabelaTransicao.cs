using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabelas
{
    public class tabelaTransicao
    {
        int elemento;
        public tabelaTransicao(int elemento)
        {
            this.Elemento = elemento;
        }

        public int Elemento { get => elemento; set => elemento = value; }
    }
}

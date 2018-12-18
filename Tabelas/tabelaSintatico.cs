using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabelas
{
    public class tabelaSintatico
    {
        private int elemento;

        public tabelaSintatico(int elemento)
        {
            this.elemento = elemento;
        }

        public int Elemento { get => elemento; set => elemento = value; }
    }
}

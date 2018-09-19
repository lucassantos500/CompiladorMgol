using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Tabelas
{
    public class tabelaErros
    {
        public Hashtable tabeladeerros;

        public tabelaErros()
        {
        }

        public void addnatabela(int cod, String texto)
        {
            tabeladeerros.Add(cod, texto);
        }

    }
}

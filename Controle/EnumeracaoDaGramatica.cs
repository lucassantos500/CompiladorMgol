using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controle
{
    public class EnumeracaoDaGramatica
    {
        private int enumeracao;
        private String sentenca;

        public EnumeracaoDaGramatica(int enumeracao, String sentenca)
        {
            this.Enumeracao = enumeracao;
            this.Sentenca = sentenca;
        }

        public int Enumeracao { get => enumeracao; set => enumeracao = value; }
        public string Sentenca { get => sentenca; set => sentenca = value; }
    }
}

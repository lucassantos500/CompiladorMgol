using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Controle
{
    public class controleDadosSimbolos
    {
        private string lexema, token, tipo;

        public controleDadosSimbolos(string lexema, string token, string tipo)
        {
            this.lexema = lexema;
            this.token = token;
            this.tipo = tipo;
        }

        public string Lexema { get => lexema; set => lexema = value; }
        public string Token { get => token; set => token = value; }
        public string Tipo { get => tipo; set => tipo = value; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticEvolution
{
    public class Coelho
    {
        public bool Vivo { get; set; }
        public int Estomago { get; set; }
        public int posicaoX { get; set; }
        public int posicaoY { get; set; }

        public List<Tuple<int, int>> moverCoelho()
        {
            List<Tuple<int, int>> saida = new List<Tuple<int, int>>();
            saida.Add(new Tuple<int, int>(posicaoX, posicaoY));

            int numX = new Random().Next(1,99) % new Random().Next(1,5) == 0 ? 1 : -1;
            int numY = new Random().Next(1,99) % new Random().Next(1,5) == 0 ? 1 : -1;

            int eixoX = 0;
            int eixoY = 0;

            if (numX == numY)
            {
                eixoX = numX + posicaoX < 0 ? 9 : numX + posicaoX > 9 ? 0 : numX + posicaoX;
                eixoY = numY + posicaoY < 0 ? 9 : numY + posicaoY > 9 ? 0 : numY + posicaoY;
            }
            if (new Random().Next(1, 99) % new Random().Next(1, 5) == 0)
            {
                eixoX = numX + posicaoX < 0 ? 9 : numX + posicaoX > 9 ? 0 : numX + posicaoX;
                eixoY = posicaoY;
            }
            else
            {
                eixoX = posicaoX;
                eixoY = numY + posicaoY < 0 ? 9 : numY + posicaoY > 9 ? 0 : numY + posicaoY;
            }

            saida.Add(new Tuple<int, int>(eixoX, eixoY));

            return saida;
        }
        public Coelho validaVida(bool comidaNoCampo)
        {
            if (this.Vivo && this.Estomago > 0)
            {
                if (comidaNoCampo)
                {
                    this.Estomago += 3;
                }
                this.Estomago--;
            }
            else
            {
                this.Vivo = false;
            }

            return this;
        }
    }
}

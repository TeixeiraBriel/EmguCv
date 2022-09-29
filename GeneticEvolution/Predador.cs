using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticEvolution
{
    public class Predador
    {
        public bool Vivo { get; set; }
        public int Estomago { get; set; }
        public int tipo { get; set; }
        public int posicaoX { get; set; }
        public int posicaoY { get; set; }
        public bool Multiplicar { get; set;}
        public string dataNascimento { get; set; }

        public List<Tuple<int, int>> moverPredador(Tuple<int,int> presa = null)
        {
            int eixoX = 0;
            int eixoY = 0;

            List<Tuple<int, int>> saida = new List<Tuple<int, int>>();
            saida.Add(new Tuple<int, int>(posicaoX, posicaoY));

            //Valida existe grama e move pra ela
            if (presa != null)
            {
                if (presa.Item1 == this.posicaoX)
                {
                    eixoX = this.posicaoX;
                    eixoY = this.posicaoY > presa.Item2 ? this.posicaoY - 1 : this.posicaoY + 1;
                }
                else
                {
                    eixoX = this.posicaoX > presa.Item1 ? this.posicaoX - 1 : this.posicaoX + 1;
                    eixoY = this.posicaoY;
                }
                saida.Add(new Tuple<int, int>(eixoX, eixoY));
                return saida;
            }

            int numX = new Random().Next(1,99) % new Random().Next(1,5) == 0 ? 1 : -1;
            int numY = new Random().Next(1,99) % new Random().Next(1,5) == 0 ? 1 : -1;

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
        public Predador validaVida(bool comidaNoCampo)
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

        public static void moverTodosPredadores(List<List<Predador>> Lista, List<Grama> _matrizGrama)
        {
            List<List<Predador>> list = Lista;
            List<Predador> listPredadorMorto = new List<Predador>();
            List<Tuple<Predador,int,int>> listPredadorMover = new List<Tuple<Predador, int, int>>();
            List<Predador> coelhosComidos = new List<Predador>();

            foreach (var predador in list)
            {
                foreach (var item in predador)
                {
                    int tipo = item.tipo;
                    if (item.Vivo)
                    {
                        Tuple<int, int> tupla = null;
                        if (item.tipo == 0)
                        {
                            Grama gramaMaisProx = _matrizGrama.FirstOrDefault(x => x.posicaoX == item.posicaoX);
                            if (gramaMaisProx == null)
                                gramaMaisProx = _matrizGrama.FirstOrDefault(y => y.posicaoX == item.posicaoY);

                            if (gramaMaisProx != null)
                            {
                                tupla = new Tuple<int, int>(gramaMaisProx.posicaoX, gramaMaisProx.posicaoY);
                            }
                        }
                        else
                        {
                            Predador coelhoMaisProx = list[0].FirstOrDefault(x => x.posicaoX == item.posicaoX);
                            if (coelhoMaisProx == null)
                                coelhoMaisProx = list[0].FirstOrDefault(y => y.posicaoY == item.posicaoY);

                            if (coelhoMaisProx != null)
                            {
                                tupla = new Tuple<int, int>(coelhoMaisProx.posicaoX, coelhoMaisProx.posicaoY);
                            }

                        }

                        var resultado = item.moverPredador(tupla);

                        if (item.tipo == 0)
                        {
                            var comida = _matrizGrama.FirstOrDefault(x => x.posicaoX == resultado[1].Item1 && x.posicaoY == resultado[1].Item2);
                            if (comida != null)
                            {
                                item.validaVida(true);
                                if (item.Estomago > 5)//NECESSIDADE COELHO MULTIPLICAR
                                {
                                    item.Estomago -= 6 / 2;
                                    item.Multiplicar = true;
                                }
                                _matrizGrama.Remove(comida);
                            }
                            else
                            {
                                item.validaVida(false);
                            }
                        }
                        else
                        {
                            var comidaLobo = list[0].FirstOrDefault(x => x.posicaoX == resultado[1].Item1 && x.posicaoY == resultado[1].Item2);
                            if (comidaLobo != null && coelhosComidos.FirstOrDefault(x => x == comidaLobo) == null)
                            {
                                item.validaVida(true);
                                if (item.Estomago > 8)//NECESSIDADE MULTIPLICAR
                                {
                                    item.Estomago -= 8 / 2;
                                    item.Multiplicar = true;
                                }
                                listPredadorMorto.Add(comidaLobo);
                                coelhosComidos.Add(comidaLobo);
                            }
                            else
                            {
                                item.validaVida(false);
                            }
                        }

                        listPredadorMover.Add(new Tuple<Predador, int, int>(item, resultado[1].Item1, resultado[1].Item2));
                    }
                }
            }

            foreach (var item in listPredadorMover)
            {
                var predadorPosAtual = item.Item1;
                var predadorPosDestino = list[0].FirstOrDefault(x => x.posicaoX == item.Item2 && x.posicaoY == item.Item3);

                if (item.Item1.tipo == 1)
                    predadorPosDestino = list[0].FirstOrDefault(x => x.posicaoX == item.Item2 && x.posicaoY == item.Item3);

                if (predadorPosAtual.Estomago == 0)
                {
                    listPredadorMorto.Add(item.Item1);
                }

                if (predadorPosDestino == null)
                {
                    predadorPosAtual.posicaoX = item.Item2;
                    predadorPosAtual.posicaoY = item.Item3;
                }
            }

            foreach (var item in listPredadorMorto)
            {
                if (item.tipo == 0)
                    list[0].Remove(item);
                else
                    list[1].Remove(item);
            }
        }
    }
}

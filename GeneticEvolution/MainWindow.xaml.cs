using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GeneticEvolution
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variaveis
        List<Grama> _matrizGrama;
        List<Coelho> _matrizCoelho;
        int contadorCoelhos = 1;
        int _contGeracao = 1;
        List<Grama> CriaMatrizGrama()
        {
            List<Grama> matrizGrama = new List<Grama>();

            matrizGrama.Add(new Grama() { Vivo = true, Maturidade = 10, posicaoX = 2, posicaoY = 2 });
            matrizGrama.Add(new Grama() { Vivo = true, Maturidade = 10, posicaoX = 8, posicaoY = 8 });

            return matrizGrama;
        }
        List<Coelho> CriaMatrizCoelho()
        {
            List<Coelho> matrizCoelho = new List<Coelho>();
            matrizCoelho.Add(new Coelho() { Vivo = true, Estomago = 50, posicaoX = 5, posicaoY = 5 });

            return matrizCoelho;
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            _matrizGrama = CriaMatrizGrama();
            _matrizCoelho = CriaMatrizCoelho();
            imprimeCampos(_matrizGrama, _matrizCoelho);
        }

        void metodoPrincipal()
        {
            bool novasPlantas = false;
            TabelaPrincipal.Children.Clear();

            //Movo o coelho
            List<List<Tuple<int, int>>> mudançasCoelhos = new List<List<Tuple<int, int>>>();
            foreach (var coelho in _matrizCoelho)
            {
                if (coelho.Vivo)
                {
                    var resultado = coelho.moverCoelho();
                    mudançasCoelhos.Add(resultado);
                    
                    if (coelho.validaVida(_matrizGrama.FirstOrDefault(x => x.posicaoX == resultado[1].Item1 && x.posicaoY == resultado[1].Item2) != null).Vivo)
                    {
                        contadorCoelhos++;
                    }
                }
            }
            //Atualizo a matriz
            foreach (var move in mudançasCoelhos)
            {
                var coelhoPosAtual = _matrizCoelho.FirstOrDefault(x => x.posicaoX == move[0].Item1 && x.posicaoY == move[0].Item2);
                var coelhoPosDestino = _matrizCoelho.FirstOrDefault(x => x.posicaoX == move[1].Item1 && x.posicaoY == move[1].Item2);

                if (coelhoPosAtual.Estomago == 0)
                    _matrizCoelho.Remove(coelhoPosAtual);

                if (coelhoPosDestino == null)
                {
                    coelhoPosAtual.posicaoX = move[1].Item1;
                    coelhoPosAtual.posicaoY = move[1].Item2;
                }
            }

            Tuple<bool, int> Mudanças = new Tuple<bool, int>(false, 0);
            if (_matrizCoelho.Count < 95)
            {
                Mudanças = comerGramas();
            }
            contadorCoelhos += Mudanças.Item2;
            _matrizGrama = propagarAlimento(_matrizGrama, _contGeracao, out novasPlantas);
            imprimeCampos(_matrizGrama, _matrizCoelho);
            if (novasPlantas)
            {
                tituloGeracao.Text = $"\nGeração {_contGeracao} -- Novas plantas nasceram!";
                novasPlantas = false;
            }

            tituloGeracao.Text = $"Geração {_contGeracao}";
            contadorCoelhoes.Text = $"     Coelho|Grama     {_matrizCoelho.Count}|{_matrizGrama.Count}";
            _contGeracao++;
        }

        List<Grama> propagarAlimento(List<Grama> matrizEntrada, int Geracao, out bool novasPlantas)
        {
            List<Grama> matriz = matrizEntrada;
            novasPlantas = false;

            if (Geracao % 10 == 0)
            {
                List<Tuple<int, int>> mudancas = new List<Tuple<int, int>>();
                foreach (var grama in matriz)
                {
                    int i = grama.posicaoX;
                    int j = grama.posicaoY;

                    mudancas.Add(new Tuple<int, int>(j, i - 1 < 0 ? 0 : i - 1));
                    mudancas.Add(new Tuple<int, int>(j, i + 1 > 9 ? 9 : i + 1));
                    mudancas.Add(new Tuple<int, int>(j - 1 < 0 ? 0 : j - 1, i));
                    mudancas.Add(new Tuple<int, int>(j + 1 > 9 ? 9 : j + 1, i));
                }

                foreach (var item in mudancas)
                {
                    var gramaPosDestino = _matrizGrama.FirstOrDefault(x => x.posicaoX == item.Item1 && x.posicaoY == item.Item2);
                    if (gramaPosDestino == null)
                    {
                        matriz.Add(new Grama() { Vivo = true, Maturidade = 10, posicaoX = item.Item1, posicaoY = item.Item2 });
                    }
                }

                novasPlantas = true;
            }

            return matriz;
        }

        Tuple<bool, int> comerGramas()
        {
            Tuple<bool, int> mudanças = new Tuple<bool, int>(false, 0);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (_matrizCoelho.FirstOrDefault(x => x.posicaoX == i && x.posicaoY == j) != null && _matrizGrama.FirstOrDefault(x => x.posicaoX == i && x.posicaoY == j) != null)
                    {
                        List<Tuple<int, int>> opcoes = new List<Tuple<int, int>>();

                        int x = new Random().Next(9);
                        int y = new Random().Next(9);
                        bool trocar = true;
                        do
                        {
                            if (!opcoes.Exists(tupla => tupla.Item1 == x && tupla.Item2 == y))
                            {
                                if (_matrizCoelho.FirstOrDefault(xx => xx.posicaoX == x && xx.posicaoY == y) != null)
                                {
                                    opcoes.Add(new Tuple<int, int>(x, y));
                                    x = x + 1 < 10 ? x + 1 : 0;
                                    y = y + 1 < 10 ? y + 1 : 0;
                                }
                                else
                                {
                                    trocar = false;
                                }
                            }
                            else
                            {
                                if (x % 2 == 1)
                                    x = x + 1 < 10 ? x + 1 : 0;
                                else
                                    x = new Random().Next(9);

                                if (y % 2 == 1)
                                    y = y + 1 < 10 ? y + 1 : 0;
                                else
                                    y = new Random().Next(9);
                            }
                        } while (trocar);

                        var gramaRemover = _matrizGrama.FirstOrDefault(xx => xx.posicaoX == i && xx.posicaoY == j);
                        _matrizGrama.Remove(gramaRemover);
                        _matrizCoelho.Add(new Coelho() { Vivo = true, Estomago = 6, posicaoX = x, posicaoY = y });
                        mudanças = new Tuple<bool, int>(true, mudanças.Item2 + 1);
                    }
                }
            }
            return mudanças;
        }

        #region Manipula Front
        private void btnProxGeracao_Click(object sender, RoutedEventArgs e)
        {
            metodoPrincipal();
        }

        void adicionaInfoCampo(int x, int y, bool grama = false, bool coelho = false, string texto = "")
        {
            if (grama && coelho)
            {
                TextBlock txt2 = new TextBlock();
                txt2.Text = string.IsNullOrEmpty(texto) ? $"Coelho {x}x{y}" : texto;
                txt2.Foreground = Brushes.Blue;
                txt2.FontWeight = FontWeights.Bold;

                TextBlock txt1 = new TextBlock();
                txt1.Text = $"Grama {x}x{y}";
                txt1.Foreground = Brushes.Green;
                txt1.FontWeight = FontWeights.Bold;

                StackPanel stkPnl = new StackPanel();
                stkPnl.Children.Add(txt1);
                stkPnl.Children.Add(txt2);

                Grid.SetColumn(stkPnl, x);
                Grid.SetRow(stkPnl, y);

                TabelaPrincipal.Children.Add(stkPnl);
            }
            else if (coelho)
            {
                TextBlock txt2 = new TextBlock();
                txt2.Text = string.IsNullOrEmpty(texto) ? $"Coelho {x}x{y}" : texto;
                txt2.Foreground = Brushes.Blue;
                txt2.FontWeight = FontWeights.Bold;
                Grid.SetColumn(txt2, x);
                Grid.SetRow(txt2, y);
                TabelaPrincipal.Children.Add(txt2);
            }
            else
            {
                TextBlock txt1 = new TextBlock();
                txt1.Text = $"Grama {x}x{y}";
                txt1.Foreground = Brushes.Green;
                txt1.FontWeight = FontWeights.Bold;
                Grid.SetColumn(txt1, x);
                Grid.SetRow(txt1, y);
                TabelaPrincipal.Children.Add(txt1);
            }
        }

        void imprimeCampos(List<Grama> matrizGrama, List<Coelho> matrizCoelho)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (matrizGrama.FirstOrDefault(x => x.posicaoX == i && x.posicaoY == j) != null && matrizCoelho.FirstOrDefault(x => x.posicaoX == i && x.posicaoY == j) != null)
                    {
                        adicionaInfoCampo(i, j, true, true, $"Coelho {i}x{j} v:{matrizCoelho.Find(x => x.posicaoX == i && x.posicaoY == j).Estomago}");
                    }
                    else if (matrizGrama.Find(x => x.posicaoX == i && x.posicaoY == j) != null)
                    {
                        adicionaInfoCampo(i, j, true, false);
                    }
                    else if (matrizCoelho.Find(x => x.posicaoX == i && x.posicaoY == j) != null)
                    {
                        adicionaInfoCampo(i, j, false, true, $"Coelho {i}x{j} v:{matrizCoelho.Find(x => x.posicaoX == i && x.posicaoY == j).Estomago}");
                    }
                }
            }
        }
        #endregion
    }
}

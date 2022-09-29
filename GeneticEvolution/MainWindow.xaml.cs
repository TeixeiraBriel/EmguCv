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
        List<List<Grama>> _matrizGrama;
        List<List<Coelho>> _matrizCoelho;
        int contadorCoelhos = 1;
        int _contGeracao = 1;

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
            contadorCoelhos = 0;
            foreach (var linhaMatriz in _matrizCoelho)
            {
                foreach (var coelho in linhaMatriz)
                {
                    if (coelho.Vivo)
                    {
                        var resultado = coelho.moverCoelho();
                        mudançasCoelhos.Add(resultado);
                        if (coelho.validaVida(_matrizGrama[resultado[1].Item1][resultado[1].Item2].Vivo).Vivo)
                        {
                            contadorCoelhos++;
                        }
                    }
                }
            }
            //Atualizo a matriz
            foreach (var move in mudançasCoelhos)
            {
                if (!_matrizCoelho[move[1].Item1][move[1].Item2].Vivo)
                {
                    if (_matrizCoelho[move[0].Item1][move[0].Item2].Vivo)
                    {
                        _matrizCoelho[move[1].Item1][move[1].Item2].Estomago = _matrizCoelho[move[0].Item1][move[0].Item2].Estomago;
                        _matrizCoelho[move[1].Item1][move[1].Item2].Vivo = true;
                    }
                    _matrizCoelho[move[0].Item1][move[0].Item2].Estomago = 0;
                    _matrizCoelho[move[0].Item1][move[0].Item2].Vivo = false;
                }
            }

            Tuple<bool, int> Mudanças = new Tuple<bool, int>(false, 0);
            if (contadorCoelhos < 95)
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
            contadorCoelhoes.Text = $"Coelhos {contadorCoelhos}";
            _contGeracao++;
        }

        List<List<Grama>> CriaMatrizGrama()
        {
            List<List<Grama>> matrizGrama = new List<List<Grama>>();
            for (int i = 0; i < 10; i++)
            {
                List<Grama> gramaLinha = new List<Grama>();
                for (int j = 0; j < 10; j++)
                {
                    if ((i == 2 && j == 2) || (i == 5 && j == 5))
                    {
                        gramaLinha.Add(new Grama() { Vivo = true, Maturidade = 10, posicaoX = i, posicaoY = j });
                    }
                    else
                    {
                        gramaLinha.Add(new Grama() { Vivo = false, Maturidade = 10, posicaoX = i, posicaoY = j });
                    }
                }
                matrizGrama.Add(gramaLinha);
            }

            return matrizGrama;
        }

        List<List<Coelho>> CriaMatrizCoelho()
        {
            List<List<Coelho>> matrizCoelho2 = new List<List<Coelho>>();
            for (int i = 0; i < 10; i++)
            {
                List<Coelho> coelhoLinha = new List<Coelho>();
                for (int j = 0; j < 10; j++)
                {
                    if (i == 5 && j == 5)
                    {
                        coelhoLinha.Add(new Coelho() { Vivo = true, Estomago = 50, posicaoX = i, posicaoY = j });
                    }
                    else
                    {
                        coelhoLinha.Add(new Coelho() { Vivo = false, Estomago = 5, posicaoX = i, posicaoY = j });
                    }
                }
                matrizCoelho2.Add(coelhoLinha);
            }

            return matrizCoelho2;
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

        void imprimeCampos(List<List<Grama>> matrizGrama, List<List<Coelho>> matrizCoelho)
        {
            for (int i = 0; i < matrizGrama.Count; i++)
            {
                for (int j = 0; j < matrizGrama[0].Count; j++)
                {
                    if (matrizGrama[i][j].Vivo && matrizCoelho[i][j].Vivo)
                    {
                        adicionaInfoCampo(i, j, true, true, $"Coelho {matrizCoelho[i][j].posicaoX}x{matrizCoelho[i][j].posicaoY} v:{matrizCoelho[i][j].Estomago}");
                    }
                    else if (matrizGrama[i][j].Vivo)
                    {
                        adicionaInfoCampo(i, j, true, false);
                    }
                    else if (matrizCoelho[i][j].Vivo)
                    {
                        adicionaInfoCampo(i, j, false, true, $"Coelho {matrizCoelho[i][j].posicaoX}x{matrizCoelho[i][j].posicaoY} v:{matrizCoelho[i][j].Estomago}");
                    }
                }
            }
        }

        List<List<Grama>> propagarAlimento(List<List<Grama>> matrizEntrada, int Geracao, out bool novasPlantas)
        {
            List<List<Grama>> matriz = matrizEntrada;
            novasPlantas = false;

            if (Geracao % 10 == 0)
            {
                List<Tuple<int, int>> mudancas = new List<Tuple<int, int>>();
                for (int j = 0; j < matriz.Count; j++)
                {
                    for (int i = 0; i < matriz[0].Count; i++)
                    {
                        if (matriz[j][i].Vivo)
                        {

                            mudancas.Add(new Tuple<int, int>(j, i - 1 < 0 ? 0 : i - 1));
                            mudancas.Add(new Tuple<int, int>(j, i + 1 > matriz[0].Count - 1 ? matriz[0].Count - 1 : i + 1));
                            mudancas.Add(new Tuple<int, int>(j - 1 < 0 ? 0 : j - 1, i));
                            mudancas.Add(new Tuple<int, int>(j + 1 > matriz[0].Count - 1 ? matriz[0].Count - 1 : j + 1, i));
                        }
                    }
                }

                foreach (var item in mudancas)
                {
                    matriz[item.Item1][item.Item2].Vivo = true;
                }

                novasPlantas = true;
            }

            return matriz;
        }

        Tuple<bool, int> comerGramas()
        {
            Tuple<bool, int> mudanças = new Tuple<bool, int>(false, 0);
            int contadorProle = 55;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (_matrizCoelho[i][j].Vivo && _matrizGrama[i][j].Vivo)
                    {
                        List<Tuple<int, int>> opcoes = new List<Tuple<int, int>>();

                        int x = new Random().Next(9);
                        int y = new Random().Next(9);
                        bool trocar = true;
                        do
                        {
                            if (!opcoes.Exists(tupla => tupla.Item1 == x && tupla.Item2 == y))
                            {
                                if (_matrizCoelho[x][y].Vivo)
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


                        _matrizGrama[i][j].Vivo = false;
                        _matrizCoelho[x][y].Vivo = true;
                        _matrizCoelho[x][y].Estomago = 6;
                        mudanças = new Tuple<bool, int>(true, mudanças.Item2 + 1);
                    }
                }
            }
            return mudanças;
        }

        private void btnProxGeracao_Click(object sender, RoutedEventArgs e)
        {
            metodoPrincipal();
        }
    }
}

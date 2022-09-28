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
        List<string[]> _matrizGrama;
        List<string[]> _matrizCoelho;
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
            int tempoCrescimento = 4;

            TabelaPrincipal.Children.Clear();
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

        List<string[]> CriaMatrizGrama()
        {
            string[] col1 = new string[10] { "O", "O", "O", "O", "O", "O", "O", "O", "O", "O" };
            string[] col2 = new string[10] { "O", "O", "O", "O", "O", "O", "O", "O", "O", "O" };
            string[] col3 = new string[10] { "O", "O", "X", "O", "O", "O", "O", "O", "O", "O" };
            string[] col4 = new string[10] { "O", "O", "O", "O", "O", "O", "O", "O", "O", "O" };
            string[] col5 = new string[10] { "O", "O", "O", "O", "O", "O", "O", "O", "O", "O" };
            string[] col6 = new string[10] { "O", "O", "O", "O", "O", "O", "O", "O", "O", "O" };
            string[] col7 = new string[10] { "O", "O", "O", "O", "O", "O", "O", "O", "O", "O" };
            string[] col8 = new string[10] { "O", "O", "O", "O", "O", "O", "O", "O", "O", "O" };
            string[] col9 = new string[10] { "O", "O", "O", "O", "O", "O", "O", "O", "O", "O" };
            string[] col10 = new string[10] { "O", "O", "O", "O", "O", "O", "O", "O", "O", "O" };
            List<string[]> matriz = new List<string[]>();
            matriz.Add(col1);
            matriz.Add(col2);
            matriz.Add(col3);
            matriz.Add(col4);
            matriz.Add(col5);
            matriz.Add(col6);
            matriz.Add(col7);
            matriz.Add(col8);
            matriz.Add(col9);
            matriz.Add(col10);

            return matriz;
        }

        List<string[]> CriaMatrizCoelho()
        {
            string[] col1 = new string[10] { "O", "O", "O", "O", "O", "O", "O", "O", "O", "O" };
            string[] col2 = new string[10] { "X", "O", "O", "O", "O", "O", "O", "O", "O", "O" };
            string[] col3 = new string[10] { "O", "O", "O", "O", "O", "O", "O", "O", "O", "O" };
            string[] col4 = new string[10] { "O", "O", "O", "O", "O", "O", "O", "O", "O", "O" };
            string[] col5 = new string[10] { "O", "O", "O", "O", "O", "O", "O", "O", "O", "O" };
            string[] col6 = new string[10] { "O", "O", "O", "O", "O", "O", "O", "O", "O", "O" };
            string[] col7 = new string[10] { "O", "O", "O", "O", "O", "O", "O", "O", "O", "O" };
            string[] col8 = new string[10] { "O", "O", "O", "O", "O", "O", "O", "O", "O", "O" };
            string[] col9 = new string[10] { "O", "O", "O", "O", "O", "O", "O", "O", "O", "O" };
            string[] col10 = new string[10] { "O", "O", "O", "O", "O", "O", "O", "O", "O", "O" };
            List<string[]> matriz = new List<string[]>();
            matriz.Add(col1);
            matriz.Add(col2);
            matriz.Add(col3);
            matriz.Add(col4);
            matriz.Add(col5);
            matriz.Add(col6);
            matriz.Add(col7);
            matriz.Add(col8);
            matriz.Add(col9);
            matriz.Add(col10);

            return matriz;
        }

        void adicionaInfoCampo(int x, int y, bool grama = false, bool coelho = false)
        {
            if (grama && coelho)
            {
                TextBlock txt2 = new TextBlock();
                txt2.Text = $"Coelho {x}x{y}";
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
                txt2.Text = $"Coelho {x}x{y}";
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

        void imprimeCampos(List<string[]> matrizGrama, List<string[]> matrizCoelho)
        {
            for (int i = 0; i < matrizGrama.Count; i++)
            {
                for (int j = 0; j < matrizGrama[0].Length; j++)
                {
                    if (matrizGrama[i][j] == "X" && matrizCoelho[i][j] == "X")
                    {
                        adicionaInfoCampo(i, j, true, true);
                    }
                    else if(matrizGrama[i][j] == "X")
                    {
                        adicionaInfoCampo(i, j, true, false);
                    }
                    else if (matrizCoelho[i][j] == "X")
                    {
                        adicionaInfoCampo(i, j, false, true);
                    }
                }
            }
        }

        List<string[]> propagarAlimento(List<string[]> matrizEntrada, int Geracao, out bool novasPlantas)
        {
            List<string[]> matriz = matrizEntrada;
            novasPlantas = false;

            if (Geracao % 10 == 0)
            {
                List<Tuple<int, int>> mudancas = new List<Tuple<int, int>>();
                for (int j = 0; j < matriz.Count; j++)
                {
                    for (int i = 0; i < matriz[0].Length; i++)
                    {
                        if (matriz[j][i] == "X")
                        {

                            mudancas.Add(new Tuple<int, int>(j, i - 1 < 0 ? 0 : i - 1));
                            mudancas.Add(new Tuple<int, int>(j, i + 1 > matriz[0].Length - 1 ? matriz[0].Length - 1 : i + 1));
                            mudancas.Add(new Tuple<int, int>(j - 1 < 0 ? 0 : j - 1, i));
                            mudancas.Add(new Tuple<int, int>(j + 1 > matriz[0].Length - 1 ? matriz[0].Length - 1 : j + 1, i));
                        }
                    }
                }

                foreach (var item in mudancas)
                {
                    matriz[item.Item1][item.Item2] = "X";
                }

                novasPlantas = true;
            }

            return matriz;
        }

        Tuple<bool,int> comerGramas()
        {
            Tuple<bool, int> mudanças = new Tuple<bool, int>(false,0);
            int contadorProle = 5;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (_matrizCoelho[i][j] == "X" && _matrizGrama[i][j] == "X")
                    {
                        List<Tuple<int,int>> opcoes = new List<Tuple<int, int>>();

                        int x = new Random().Next(9);
                        int y = new Random().Next(9);
                        opcoes.Add(new Tuple<int, int>(x, y));
                        bool trocar = true;
                        do
                        {
                            if (!opcoes.Exists(tupla => tupla.Item1 == x && tupla.Item2 == y))
                            {
                                if (_matrizCoelho[x][y] == "X")
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
                                x = new Random().Next(9);
                                y = y + 1 < 10 ? y + 1 : 0;
                            }
                        } while (trocar);


                        _matrizGrama[i][j] = "O";
                        _matrizCoelho[x][y] = "X";
                        mudanças = new Tuple<bool, int>(true, mudanças.Item2 + 1);
                        if (mudanças.Item2 == 5)
                        {
                            return mudanças;
                        }
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

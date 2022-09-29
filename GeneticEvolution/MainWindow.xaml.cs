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
        int _comidaEstomagoTipo0 = 3;
        int _comidaEstomagoTipo1 = 3;
        int _tempoMaturidade = 3;
        int _qtdGrama = 3;
        int _qtdCoelho = 2;
        int _qtdLobo = 1;
        List<Grama> _matrizGrama;
        List<Predador> _matrizCoelho;
        List<Predador> _matrizLobo;
        int _contGeracao = 1;
        List<Grama> CriaMatrizGrama(int tempoMaturidade, int qtd)
        {
            List<Grama> matrizGrama = new List<Grama>();

            for (int i = 0; i < qtd; i++)
            {
                bool trocar = true;
                do
                {
                    int x = new Random().Next(9);
                    int y = new Random().Next(9);
                    if (!matrizGrama.Exists(a => a.posicaoX == x && a.posicaoY == y))
                    {
                        matrizGrama.Add(new Grama() { Vivo = true, Maturidade = tempoMaturidade, posicaoX = x, posicaoY = y });
                        trocar = false;
                    }
                } while (trocar);
            }

            return matrizGrama;
        }
        List<Predador> CriaMatrizPredador(int qtdEstomago, int tipo, int qtd)
        {
            List<Predador> matriz = new List<Predador>();
            for (int i = 0; i < qtd; i++)
            {
                bool trocar = true;
                do
                {
                    int x = new Random().Next(9);
                    int y = new Random().Next(9);
                    if (!matriz.Exists(a => a.posicaoX == x && a.posicaoY == y))
                    {
                        matriz.Add(new Predador() { Vivo = true, Estomago = qtdEstomago, tipo = tipo, posicaoX = x, posicaoY = y, Multiplicar = false, dataNascimento = "G1"});
                        trocar = false;
                    }
                } while (trocar);
            }

            return matriz;
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            inputQtdGrama.Text = _qtdGrama.ToString();
            inputQtdCoelho.Text = _qtdCoelho.ToString();
            inputQtdLobo.Text = _qtdLobo.ToString();
            _matrizGrama = CriaMatrizGrama(_tempoMaturidade, _qtdGrama);
            _matrizCoelho = CriaMatrizPredador(_comidaEstomagoTipo0, 0, _qtdCoelho);
            _matrizLobo = CriaMatrizPredador(_comidaEstomagoTipo1, 1, _qtdLobo);
            tituloGeracao.Text = $"Geração {_contGeracao}";
            contadorCoelhoes.Text = $"     Coelho|Grama     {_matrizCoelho.Count}|{_matrizGrama.Count}";
            imprimeCampos();
        }

        void metodoPrincipal()
        {
            TabelaPrincipal.Children.Clear();

            //Mover predadores
            List<List<Predador>> list = new List<List<Predador>>() { _matrizCoelho, _matrizLobo };
            Predador.moverTodosPredadores(list, _matrizGrama);

            Tuple<bool, int> Mudanças = new Tuple<bool, int>(false, 0);
            mutiplicarPredador(_matrizCoelho);
            mutiplicarPredador(_matrizLobo);
            _matrizGrama = propagarAlimento(_matrizGrama, _contGeracao);
            imprimeCampos();

            tituloGeracao.Text = $"Geração {_contGeracao}";
            contadorCoelhoes.Text = $"     Grama|Coelho|Lobo     {_matrizGrama.Count}|{_matrizCoelho.Count}|{_matrizLobo.Count}";
            _contGeracao++;
        }

        List<Grama> propagarAlimento(List<Grama> matrizEntrada, int Geracao)
        {
            List<Grama> matriz = matrizEntrada;

            List<Tuple<int, int>> mudancas = new List<Tuple<int, int>>();
            foreach (var grama in matriz)
            {
                if (grama.Maturidade > 1)
                {
                    grama.Maturidade--;
                    continue;
                }

                int i = grama.posicaoX;
                int j = grama.posicaoY;

                mudancas.Add(new Tuple<int, int>(j, i - 1 < 0 ? 0 : i - 1));
                mudancas.Add(new Tuple<int, int>(j, i + 1 > 9 ? 9 : i + 1));
                mudancas.Add(new Tuple<int, int>(j - 1 < 0 ? 0 : j - 1, i));
                mudancas.Add(new Tuple<int, int>(j + 1 > 9 ? 9 : j + 1, i));
                grama.Maturidade = _tempoMaturidade;
            }

            foreach (var item in mudancas)
            {
                var gramaPosDestino = _matrizGrama.FirstOrDefault(x => x.posicaoX == item.Item1 && x.posicaoY == item.Item2);
                if (gramaPosDestino == null)
                {
                    matriz.Add(new Grama() { Vivo = true, Maturidade = _tempoMaturidade, posicaoX = item.Item1, posicaoY = item.Item2 });
                }
            }

            return matriz;
        }

        void mutiplicarPredador(List<Predador> matriz)
        {
            List<Predador> novosPredadores = new List<Predador>();

            foreach (var predador in matriz)
            {
                if (predador.Multiplicar)
                {
                    int estomago = _comidaEstomagoTipo1;
                    if (predador.tipo == 0)
                        estomago = _comidaEstomagoTipo0;

                    bool trocar = true;
                    do
                    {
                        int x = new Random().Next(9);
                        int y = new Random().Next(9);
                        if (!matriz.Exists(a => a.posicaoX == x && a.posicaoY == y) && !novosPredadores.Exists(a => a.posicaoX == x && a.posicaoY == y))
                        {
                            novosPredadores.Add(new Predador() { Vivo = true, tipo = predador.tipo, Estomago = estomago, posicaoX = x, posicaoY = y, Multiplicar = false, dataNascimento = $"G{_contGeracao}" });
                            predador.Multiplicar = false;
                            trocar = false;
                        }
                    } while (trocar);
                }
            }

            foreach (var item in novosPredadores)
            {
                matriz.Add(item);
            }
        }

        #region Manipula Front
        private void btnProxGeracao_Click(object sender, RoutedEventArgs e)
        {
            metodoPrincipal();
        }

        void imprimeCoordenada(int x, int y)
        {
            var existeCoelho = _matrizCoelho.FirstOrDefault(a => a.posicaoX == x && a.posicaoY == y);
            var existeGrama = _matrizGrama.FirstOrDefault(a => a.posicaoX == x && a.posicaoY == y);
            var existeLobo = _matrizLobo.FirstOrDefault(a => a.posicaoX == x && a.posicaoY == y);

            StackPanel stkPnl = new StackPanel();
            Grid.SetColumn(stkPnl, x);
            Grid.SetRow(stkPnl, y);
            if (existeCoelho != null)
            {
                TextBlock txt = new TextBlock();
                txt.Text = $"C{x}x{y} - {existeCoelho.dataNascimento} - E:{existeCoelho.Estomago}";
                txt.Foreground = Brushes.Blue;
                txt.FontWeight = FontWeights.Bold;
                stkPnl.Children.Add(txt);
            }
            if (existeGrama != null)
            {
                TextBlock txt = new TextBlock();
                txt.Text = $"G{x}x{y} - M:{existeGrama.Maturidade}";
                txt.Foreground = Brushes.Green;
                txt.FontWeight = FontWeights.Bold;
                stkPnl.Children.Add(txt);
            }
            if (existeLobo != null)
            {
                TextBlock txt = new TextBlock();
                txt.Text = $"L{x}x{y} - {existeLobo.dataNascimento} - E:{existeLobo.Estomago}";
                txt.Foreground = Brushes.Red;
                txt.FontWeight = FontWeights.Bold;
                stkPnl.Children.Add(txt);
            }

            TabelaPrincipal.Children.Add(stkPnl);
        }

        void imprimeCampos()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    imprimeCoordenada(i, j);
                }
            }
        }
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            TabelaPrincipal.Children.Clear();
            _qtdGrama = int.Parse(inputQtdGrama.Text);
            _qtdCoelho = int.Parse(inputQtdCoelho.Text);
            _qtdLobo = int.Parse(inputQtdLobo.Text);
            _matrizGrama = CriaMatrizGrama(_tempoMaturidade, _qtdGrama);
            _matrizCoelho = CriaMatrizPredador(_comidaEstomagoTipo0, 0, _qtdCoelho);
            _matrizLobo = CriaMatrizPredador(_comidaEstomagoTipo1, 1, _qtdLobo);
            imprimeCampos();
            _contGeracao = 1;
        }
        #endregion

    }
}

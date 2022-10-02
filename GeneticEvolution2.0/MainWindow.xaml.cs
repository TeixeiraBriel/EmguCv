using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GeneticEvolution2._0
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        int _Contador = 1;
        int _contadorMoverGrama = 3;
        int _velocidadeBunny = 5;
        DispatcherTimer Timer = new DispatcherTimer();
        DispatcherTimer TimerGrama = new DispatcherTimer();
        List<Grama> _listGrama = new List<Grama>();

        public MainWindow()
        {
            InitializeComponent();
            _listGrama.Add(Grama.criarGrama(true, 100,0));
            _listGrama.Add(Grama.criarGrama(true, 100,600));
            _listGrama.Add(Grama.criarGrama(true, 1200, 0));
            _listGrama.Add(Grama.criarGrama(true, 1200, 600));
            inicializaTimer();
        }

        #region Timer
        public void inicializaTimer()
        {
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            Timer.Start();

            TimerGrama.Tick += new EventHandler(TimerGrama_Tick);
            TimerGrama.Interval = new TimeSpan(0, 0, 0, _contadorMoverGrama, 0);
            TimerGrama.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            Timer.Stop();
            try
            {
                moveBunny(_listGrama.FirstOrDefault());
                imprimeGrama();
            }
            catch (Exception ex)
            {
                string erro = ex.Message;
            }
            finally
            {
                Timer.Start();
            }
        }

        private void TimerGrama_Tick(object sender, EventArgs e)
        {
            TimerGrama.Stop();
            try
            {
                Contador.Text = $"Contador:{_Contador}";
                propagarGrama();
            }
            catch (Exception ex)
            {
                string erro = ex.Message;
            }
            finally
            {
                _Contador++;
                TimerGrama.Start();
            }
        }
        #endregion

        void propagarGrama()
        {
            List<Grama> listGrama = new List<Grama>();

            foreach (var grama in _listGrama)
            {
                listGrama.Add(grama);
                for (int i = 0; i < 4; i++)
                {
                    double x = grama.posicaoX;
                    double y = grama.posicaoY;

                    switch (i)
                    {
                        case 0:
                            x = x + 60;
                            break;
                        case 1:
                            x = x - 60;
                            break;
                        case 2:
                            y = y + 60;
                            break;
                        case 3:
                            y = y - 60;
                            break;
                    }

                    Rectangle gramaFilha = new Rectangle()
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Height = 50,
                        Width = 50,
                        Fill = Brushes.Green,
                        Margin = new Thickness(x, 0, 0, y)
                    };

                    var newGrama = new Grama { Vivo = true, Maturidade = 0, posicaoX = x, posicaoY = y, representacaoVisual = gramaFilha };
                    if (!_listGrama.Exists(a => a.posicaoY == y && a.posicaoX == x) && !listGrama.Exists(a => a.posicaoY == y && a.posicaoX == x))
                    {
                        if (x >= 0 && y >= 0 && x <= 1200 && y <= 600)
                        {
                            listGrama.Add(newGrama);
                        }
                    }
                }
            }

            _listGrama = listGrama;
        }

        void imprimeGrama()
        {
            foreach (var grama in _listGrama)
            {
                try
                {
                    PainelJogo.Children.Remove(grama.representacaoVisual);
                    PainelJogo.Children.Add(grama.representacaoVisual);
                }
                catch
                {
                    PainelJogo.Children.Add(grama.representacaoVisual);
                }
            }
        }

        void moveBunny(Grama grass)
        {
            try
            {
                double diffLeft = grass.representacaoVisual.Margin.Left - Bunny.Margin.Left;
                double diffBottom = grass.representacaoVisual.Margin.Bottom - Bunny.Margin.Bottom;
                bool proxLeft = diffLeft < 10 && diffLeft > -10;
                bool proxBottom = diffBottom < 10 && diffBottom > -10;
                if (proxLeft && proxBottom)
                {
                    PainelJogo.Children.Remove(grass.representacaoVisual);
                    _listGrama.Remove(grass);
                    return;
                }

                bool xMaior = grass.representacaoVisual.Margin.Left > Bunny.Margin.Left;
                bool yMaior = grass.representacaoVisual.Margin.Bottom > Bunny.Margin.Bottom;

                double x = xMaior ? Bunny.Margin.Left + _velocidadeBunny : Bunny.Margin.Left - _velocidadeBunny;
                double y = yMaior ? Bunny.Margin.Bottom + _velocidadeBunny : Bunny.Margin.Bottom - _velocidadeBunny;

                if (proxLeft)
                {
                    x = Bunny.Margin.Left;
                }
                else if (proxBottom)
                {
                    y = Bunny.Margin.Bottom;
                }

                Bunny.Margin = new Thickness(x, 0, 0, y);
            }
            catch (Exception)
            {
            }
        }
    }
}

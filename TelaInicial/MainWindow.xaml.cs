using AutoIt;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace Desktop
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogar_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap imgbtnJogarOnline = Properties.Resources.btnJogarOnline1280x720;
                Bitmap imgbtnAccountSettings = Properties.Resources.btnAccountSettings1280x720;
                Bitmap imgbtnEntrarOnline = Properties.Resources.btnEntrarOnline1280x720;
                Bitmap imgcarregandoLogin = Properties.Resources.carregandoLogin1280x720;

                TextoResultado1.Visibility = Visibility.Visible;
                TextoResultado1.Text = "Executando...";
                Thread.Sleep(300);

                clicarImag(imgbtnJogarOnline);
                Thread.Sleep(1500);
                if (encontraImag(imgbtnJogarOnline))
                {
                    clicarImag(imgbtnJogarOnline);
                }
                else
                {
                    if (encontraImag(imgbtnAccountSettings))
                    {
                        TextoResultado1.Text = "Favor selecione o personagem.";
                    }
                    else
                    {
                        if (encontraImag(imgbtnEntrarOnline))
                        {
                            clicarImag(imgbtnEntrarOnline);

                            if (encontraImag(imgbtnAccountSettings))
                            {
                                TextoResultado1.Text = "Favor selecione o personagem.";
                                btnLogar.Visibility = Visibility.Collapsed;
                                btnJogar.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                TextoResultado1.Text = "Erro ao entrar na tela de personagens.";
                            }
                        }
                        else
                        {
                            TextoResultado1.Text = "Erro ao entrar na tela de personagens.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TextoResultado1.Text = "Falha!";
            }
        }
        private void btnJogar_Click(object sender, EventArgs e)
        {

            try
            {
                Bitmap imgbtnCriaJogo = Properties.Resources.btnCriaJogo1280x720;
                Bitmap imgbtnDificuldade = Properties.Resources.btnDificuldade1280x720;

                clicarImag(imgbtnCriaJogo);
                Thread.Sleep(1500);
                if (encontraImag(imgbtnCriaJogo))
                {
                    clicarImag(imgbtnCriaJogo);
                }
                else
                {
                    clicarImag(imgbtnDificuldade, true, 10, 10);
                    AutoItX.Send("{TAB}");
                    AutoItX.Send("rimgt123");
                    AutoItX.Send("{ENTER}");

                    TextoResultado1.Text = "Server iniciado, bom jogo!";
                }
            }
            catch (Exception ex)
            {
                TextoResultado1.Text = "Falha!";
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void btnAndar_Click(object sender, RoutedEventArgs e)
        {
            int cont = 0;
            string ultimaTecla = "w";
            string penultimaTecla = "a";
            string[] teclas = new string[4] { "w", "a", "s", "d" };
            AutoItX.ControlFocus("Hero Siege", "", "");
            do
            {
                int indice = new Random().Next(4);
                if (teclas[indice] == ultimaTecla || teclas[indice] == penultimaTecla)
                {
                    continue;
                }

                int repeticoes = new Random().Next(5);
                AutoItX.ControlFocus("Hero Siege", "", "");
                do
                {
                    repeticoes--;
                    bool continuaAndando = validaDirecaoLivre(teclas[indice]);
                    if (!continuaAndando)
                    {
                        break;
                    }
                } while (repeticoes > 0);

                AutoItX.ControlClick("Hero Siege", "", "", "left");
                cont++;
                penultimaTecla = ultimaTecla;
                ultimaTecla = teclas[indice];
            } while (cont < 100);
        }

        private void clicarImag(Bitmap myPic, bool ajusteClique = false, int ajusteX = 0, int ajusteY = 0)
        {
            bool repetir = true;
            int count = 0;
            do
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                Bitmap screenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

                Graphics g = Graphics.FromImage(screenCapture);

                g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                 Screen.PrimaryScreen.Bounds.Y,
                                 0, 0,
                                 screenCapture.Size,
                                 CopyPixelOperation.SourceCopy);

                Tuple<bool, int, int> isInCapture = IsInCapture(myPic, screenCapture);

                if (isInCapture.Item1)
                {
                    if (ajusteClique)
                    {
                        AutoItX.MouseMove(isInCapture.Item2 + ajusteX, isInCapture.Item3 + ajusteY, 10);
                        Thread.Sleep(300);
                        AutoItX.MouseClick("left", isInCapture.Item2 + ajusteX, isInCapture.Item3 + ajusteY);
                        AutoItX.MouseClick("left");
                    }
                    else
                    {
                        AutoItX.MouseMove(isInCapture.Item2, isInCapture.Item3, 10);
                        Thread.Sleep(300);
                        AutoItX.MouseClick("left", isInCapture.Item2, isInCapture.Item3);
                        AutoItX.MouseClick("left");
                    }
                    repetir = false;
                }

                sw.Stop();

                TimeSpan ts = sw.Elapsed;
                if (!repetir)
                {
                    return;
                }

                Thread.Sleep(500);
                count++;
            } while (repetir && count < 10);

            throw new Exception("Não encontrou img.");
        }

        private bool encontraImag(Bitmap myPic)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Bitmap screenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            Graphics g = Graphics.FromImage(screenCapture);

            g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                             Screen.PrimaryScreen.Bounds.Y,
                             0, 0,
                             screenCapture.Size,
                             CopyPixelOperation.SourceCopy);

            Tuple<bool, int, int> isInCapture = IsInCapture(myPic, screenCapture);

            if (isInCapture.Item1)
            {
                return true;
            }
            else
            {
                return false;
            }

            sw.Stop();

            TimeSpan ts = sw.Elapsed;
        }

        private Tuple<bool, int, int> IsInCapture(Bitmap searchFor, Bitmap searchIn)
        {
            int width = 0;
            int height = 0;
            for (int x = 0; x < searchIn.Width; x++)
            {
                for (int y = 0; y < searchIn.Height; y++)
                {
                    bool invalid = false;
                    int k = x, l = y;
                    for (int a = 0; a < searchFor.Width; a++)
                    {
                        l = y;
                        for (int b = 0; b < searchFor.Height; b++)
                        {
                            if (searchFor.GetPixel(a, b) != searchIn.GetPixel(k, l))
                            {
                                invalid = true;
                                break;
                            }
                            else
                                l++;
                        }
                        if (invalid)
                            break;
                        else
                            k++;
                    }
                    if (!invalid)
                    {
                        width = x;
                        height = y;
                        return new Tuple<bool, int, int>(true, width, height);
                    }
                }
            }
            return new Tuple<bool, int, int>(false, width, height);
        }

        bool validaDirecaoLivre(string keyPress)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var sizeHandle = AutoItX.WinGetClientSize("Hero Siege");
            var posHandle = AutoItX.WinGetPos("Hero Siege");

            Bitmap screenCapture = new Bitmap(posHandle.Width, posHandle.Height);

            Graphics g = Graphics.FromImage(screenCapture);
            g.CopyFromScreen(posHandle.X,
                             posHandle.Y,
                             0, 0,
                             screenCapture.Size,
                             CopyPixelOperation.SourceCopy);


            for (int i = 0; i < 100; i++)
            {
                AutoItX.Send(keyPress);
            }

            Thread.Sleep(300);
            Bitmap screenCapture2 = new Bitmap(posHandle.Width - 1100, posHandle.Height - 600);

            g = Graphics.FromImage(screenCapture2);

            g.CopyFromScreen(posHandle.X + 1090,
                             posHandle.Y + 100,
                             0, 0,
                             screenCapture2.Size,
                             CopyPixelOperation.SourceCopy);

            screenCapture.Save("printA", ImageFormat.Png);
            screenCapture2.Save("printB", ImageFormat.Jpeg);

            Tuple<bool, int, int> isInCapture = IsInCapture(screenCapture2, screenCapture);
            sw.Stop();

            TimeSpan ts = sw.Elapsed;

            return !isInCapture.Item1;
        }

        private void btnTeste_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var sizeHandle = AutoItX.WinGetClientSize("Hero Siege");
            var posHandle = AutoItX.WinGetPos("Hero Siege");

            Bitmap screenCapture = new Bitmap(posHandle.Width, posHandle.Height);

            Graphics g = Graphics.FromImage(screenCapture);
            g.CopyFromScreen(posHandle.X,
                             posHandle.Y,
                             0, 0,
                             screenCapture.Size,
                             CopyPixelOperation.SourceCopy);

            Bitmap screenCapture2 = new Bitmap(posHandle.Width - 1100, posHandle.Height - 600);

            g = Graphics.FromImage(screenCapture2);

            g.CopyFromScreen(posHandle.X + 1090,
                             posHandle.Y + 100,
                             0, 0,
                             screenCapture2.Size,
                             CopyPixelOperation.SourceCopy);

            screenCapture.Save("printA", ImageFormat.Png);
            screenCapture2.Save("printB", ImageFormat.Jpeg);

            Tuple<bool, int, int> isInCapture = IsInCapture(screenCapture2, screenCapture);
            sw.Stop();

            TimeSpan ts = sw.Elapsed;

            bool teste = !isInCapture.Item1;
        }
    }
}

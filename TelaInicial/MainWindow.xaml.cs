using AutoIt;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.NetworkInformation;
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
            Bitmap cavalo = Properties.Resources.montaria_cavalo_desmontado;

            TextoResultado1.Visibility = Visibility.Visible;
            TextoResultado1.Text = "Executando...";
            clicarImag(cavalo);
        }
        private void btnLogar_Click2(object sender, EventArgs e)
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
                        btnLogar.Visibility = Visibility.Collapsed;
                        btnJogar.Visibility = Visibility.Visible;
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
        private void btnJogar_Click2(object sender, EventArgs e)
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
            }
        }

        private void btnJogar_Click(object sender, RoutedEventArgs e)
        {
            do
            {
                bool vender = btnVender.IsChecked == true;
                bool altTab = btnAltTab.IsChecked == true;
                if (AutoItX.WinActive("BlueStacks App Player") == 0)
                {
                    AutoItX.WinActivate("BlueStacks App Player");
                    AutoItX.ControlFocus("BlueStacks App Player", "", "");
                }
                if (encontraImag(Properties.Resources.btn_repetir_sw, salvarPrint: true))
                {
                    if (vender)//venderItens
                    {
                        clicarImag(Properties.Resources.btn_vender_Itens_sw);
                        Thread.Sleep(500);
                        AutoItX.ControlSend("BlueStacks App Player", "", "", "1");
                        Thread.Sleep(500);
                        if (encontraImag(Properties.Resources.btn_sim_sw))
                        {
                            clicarImag(Properties.Resources.btn_sim_sw);
                            Thread.Sleep(500);
                        }
                        else
                        {
                            AutoItX.Send("{ESCAPE}");
                            Thread.Sleep(500);
                            AutoItX.Send("{ESCAPE}");
                        }
                    }

                    clicarImag(Properties.Resources.btn_repetir_sw);

                    Thread.Sleep(1000);

                    if (encontraImag(Properties.Resources.btn_sim_sw))
                    {
                        clicarImag(Properties.Resources.btn_sim_sw);
                        Thread.Sleep(1000);
                    }

                    try
                    {
                        clicarImag(Properties.Resources.btn_inicia_batalha_repeticao_sw);
                    }
                    catch
                    {
                        AutoItX.Send("2");
                    }

                    Thread.Sleep(300);
                };

                if (altTab)
                {
                    AutoItX.Send("!{TAB}");
                    Thread.Sleep(60000);
                }
                else
                {
                    Thread.Sleep(15000);
                }
            } while (true);
        }

        bool validaDirecaoLivre(string keyPress)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var sizeHandle = AutoItX.WinGetClientSize("World of Warcraft");
            var posHandle = AutoItX.WinGetPos("World of Warcraft");

            Bitmap screenCapture = new Bitmap(posHandle.Width, posHandle.Height);

            Graphics g = Graphics.FromImage(screenCapture);
            g.CopyFromScreen(posHandle.X,
                             posHandle.Y,
                             0, 0,
                             screenCapture.Size,
                             CopyPixelOperation.SourceCopy);

            if (keyPress == "a" || keyPress == "d")
                AutoItX.AutoItSetOption("SendKeyDownDelay", 500);
            else if (keyPress == "w")
                AutoItX.AutoItSetOption("SendKeyDownDelay", 10000);
            else
                return false;

            AutoItX.Send(keyPress);
            AutoItX.AutoItSetOption("SendKeyDownDelay", 0);

            Thread.Sleep(300);
            Bitmap screenCapture2 = new Bitmap(posHandle.Width - 1585, posHandle.Height - 850);

            g = Graphics.FromImage(screenCapture2);
            g.CopyFromScreen(posHandle.X + 1570,
                             posHandle.Y + 35,
                             0, 0,
                             screenCapture2.Size,
                             CopyPixelOperation.SourceCopy);

            screenCapture.Save("printA.png", ImageFormat.Png);
            screenCapture2.Save("printB.png", ImageFormat.Png);

            Tuple<bool, int, int> isInCapture = IsInCapture(screenCapture2, screenCapture);
            sw.Stop();

            TimeSpan ts = sw.Elapsed;

            return !isInCapture.Item1;
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

                try
                {
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                     Screen.PrimaryScreen.Bounds.Y,
                                     0, 0,
                                     screenCapture.Size,
                                     CopyPixelOperation.SourceCopy);
                }
                catch
                {
                    Thread.Sleep(millisecondsTimeout: 500);
                    g = Graphics.FromImage(screenCapture);
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                     Screen.PrimaryScreen.Bounds.Y,
                                     0, 0,
                                     screenCapture.Size,
                                     CopyPixelOperation.SourceCopy);
                }

                Tuple<bool, int, int> isInCapture = IsInCapture(myPic, screenCapture);

                if (isInCapture.Item1)
                {
                    if (ajusteClique)
                    {
                        AutoItX.MouseMove(isInCapture.Item2 + ajusteX, isInCapture.Item3 + ajusteY, 10);
                        Thread.Sleep(300);
                        AutoItX.MouseMove(isInCapture.Item2 + ajusteX, isInCapture.Item3 + ajusteY);
                        AutoItX.MouseClick("left");
                    }
                    else
                    {
                        AutoItX.MouseMove(isInCapture.Item2, isInCapture.Item3, 10);
                        Thread.Sleep(300);
                        AutoItX.MouseMove(isInCapture.Item2, isInCapture.Item3);
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

        private bool encontraImag(Bitmap myPic, bool salvarPrint = false)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Bitmap screenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            Graphics g = Graphics.FromImage(screenCapture);

            try
            {
                g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                 Screen.PrimaryScreen.Bounds.Y,
                                 0, 0,
                                 screenCapture.Size,
                                 CopyPixelOperation.SourceCopy);
            }
            catch
            {
                Thread.Sleep(millisecondsTimeout: 500);
                g = Graphics.FromImage(screenCapture);
                g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                 Screen.PrimaryScreen.Bounds.Y,
                                 0, 0,
                                 screenCapture.Size,
                                 CopyPixelOperation.SourceCopy);
            }

            Tuple<bool, int, int> isInCapture = IsInCapture(myPic, screenCapture);

            if (isInCapture.Item1)
            {
                if (salvarPrint)
                {
                    string filename = "print" + DateTime.Now.ToString("_dd-MM_hh-mm");
                    screenCapture.Save($"prints\\{filename}.png", ImageFormat.Png);
                }
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GeneticEvolution2._0
{
    public class Grama
    {
        public bool Vivo { get; set; }
        public int Maturidade { get; set; }
        public double posicaoX { get; set; }
        public double posicaoY { get; set; }
        public Rectangle representacaoVisual { get; set; }

        public static Grama criarGrama(bool fixo = false, double x = 0, double y = 0)
        {
            if (!fixo)
            {
                x = new Random().Next(225);
                x += new Random().Next(225);
                x += new Random().Next(225);
                x += new Random().Next(225);
                y = new Random().Next(225);
                y += new Random().Next(225);
            }

            Rectangle grama = new Rectangle()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Height = 50,
                Width = 50,
                Fill = Brushes.Green,
                Margin = new Thickness(x, 0, 0, y)
            };

            return new Grama { Vivo = true, Maturidade = 0, posicaoX = x, posicaoY = y, representacaoVisual = grama};
        }
    }
}

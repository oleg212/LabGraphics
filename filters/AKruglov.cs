using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace filters
{
    public class ReflectorFilter : filters
    {
        int R, G, B;
        bool IsPrepDone = false;
        protected bool Preparation(Bitmap Img)
        {
            R = G = B = 0;
            for (int i = 0; i < Img.Width; i++)
                for (int j = 0; j < Img.Height; j++)
                {
                    if (R < Img.GetPixel(i, j).R)
                        R = Img.GetPixel(i, j).R;
                    if (G < Img.GetPixel(i, j).G)
                        G = Img.GetPixel(i, j).G;
                    if (B < Img.GetPixel(i, j).B)
                        B = Img.GetPixel(i, j).B;
                }
            return true;
        }
        protected override Color MakeNewColor(Bitmap Img, int x, int y)
        {
            if (!IsPrepDone)
                IsPrepDone = Preparation(Img);
            Color SourceColor = Img.GetPixel(x, y);
            Color ResColor = Color.FromArgb(SourceColor.R * 255 / R, SourceColor.G * 255 / G, SourceColor.B * 255 / B);
            return ResColor;
        }
    }

    public class LinStretchFilter : filters
    {
        int R, G, B;
        int r, g, b;
        bool IsPrepDone = false;
        protected bool Preparation(Bitmap Img)
        {
            R = G = B = 0;
            r = g = b = 255;
            for (int i = 0; i < Img.Width; i++)
                for (int j = 0; j < Img.Height; j++)
                {
                    int cur = Img.GetPixel(i, j).R;
                    if (R < cur)
                        R = cur;
                    if (r > cur)
                        r = cur;
                    cur = Img.GetPixel(i, j).G;
                    if (G < cur)
                        G = cur;
                    if (g > cur)
                        g = cur;
                    cur = Img.GetPixel(i, j).B;
                    if (B < cur)
                        B = cur;
                    if (b > cur)
                        b = cur;
                }
            return true;
        }
        protected override Color MakeNewColor(Bitmap Img, int x, int y)
        {
            if (!IsPrepDone)
                IsPrepDone = Preparation(Img);
            Color SourceColor = Img.GetPixel(x, y);
            Color ResColor = Color.FromArgb((SourceColor.R - r) * 255 / (R - r), (SourceColor.G - g) * 255 / (G - g), (SourceColor.B - b) * 255 / (B - b));
            return ResColor;
        }
    }

    public class GreyWorldFilter : filters
    {
        double R, G, B, A;
        bool IsPrepDone = false;
        protected bool Preparation(Bitmap Img)
        {
            R = G = B = 0.0;
            for (int i = 0; i < Img.Width; i++)
                for (int j = 0; j < Img.Height; j++)
                {
                    R += Img.GetPixel(i, j).R;
                    G += Img.GetPixel(i, j).G;
                    B += Img.GetPixel(i, j).B;
                }
            R /= Img.Width * Img.Height;
            G /= Img.Width * Img.Height;
            B /= Img.Width * Img.Height;
            /*            int[] r = new int[256];
                        int[] g = new int[256];
                        int[] b = new int[256];
                        for (int i = 0; i < Img.Width; i++)
                            for (int j = 0; j < Img.Height; j++)
                            {
                                r[Img.GetPixel(i, j).R]++;
                                g[Img.GetPixel(i, j).G]++;
                                b[Img.GetPixel(i, j).B]++;
                            }
                        int w = Img.Width;
                        int h = Img.Height;
                        for (int i = 0; i < 256; i++)
                        {
                            r[i] /= w * h;
                            g[i] /= w * h;
                            b[i] /= w * h;
                        }
                        for (int i = 0; i < 256; i++)
                        {
                            R += r[i];
                            G += g[i];
                            B += b[i];
                        }*/
            A = (R + G + B) / 3.0;
            return true;
        }
        protected override Color MakeNewColor(Bitmap Img, int x, int y)
        {
            if (!IsPrepDone)
                IsPrepDone = Preparation(Img);
            Color SourceColor = Img.GetPixel(x, y);
            Color ResColor = Color.FromArgb(clamp((int)(SourceColor.R * A / R), 0, 255), clamp((int)(SourceColor.G * A / G), 0, 255), clamp((int)(SourceColor.B * A / B), 0, 255));
            return ResColor;
        }
    }

    public class SourceColorFilter : filters
    {
        Color srcColor, targetColor;
        bool IsPrepDone = false;

        public SourceColorFilter()
        {
            srcColor = Color.Cyan;
            targetColor = Color.Yellow;
        }
        protected bool Preparation(Bitmap Img)
        {
            return true;
        }
        protected override Color MakeNewColor(Bitmap Img, int x, int y)
        {
            if (!IsPrepDone)
                IsPrepDone = Preparation(Img);
            Color SourceColor = Img.GetPixel(x, y);
            Color ResColor = Color.FromArgb(clamp((int)(SourceColor.R * targetColor.R / (srcColor.R + 1)), 0, 255), clamp((int)(SourceColor.G * targetColor.G / (srcColor.G + 1)), 0, 255), clamp((int)(SourceColor.B * targetColor.B / (srcColor.B + 1)), 0, 255));
            return ResColor;
        }
    }
}


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

    public class GlassFilter : filters
    {

        Random rnd = new Random();
        protected override Color MakeNewColor(Bitmap Img, int x, int y)
        {
            double value = rnd.NextDouble() * 1;
            double value1 = rnd.NextDouble() * 1;
            int l = (int)(y + ((value1 - 0.5) * 10));
            int k = (int)(x + ((value - 0.5) * 10));


            return Img.GetPixel(clamp(k, 0, Img.Width - 1), clamp(l, 0, Img.Height - 1));
        }
    }
    public class MoveFilter : filters
    {

        protected override Color MakeNewColor(Bitmap Img, int x, int y)
        {

            int k = x + 50;
            int l = y;
            Color rezolt = Color.White;
            if ((k < 0) || (k > Img.Width - 1) || (l < 0) || (l > Img.Height)) return rezolt;
            return Img.GetPixel(clamp(k, 0, Img.Width - 1), clamp(l, 0, Img.Height - 1));
        }
    }
    public class RotateFilter : filters
    {

        protected override Color MakeNewColor(Bitmap Img, int x, int y)
        {
            int x0 = Img.Width / 2;
            int y0 = Img.Height / 2;
            double corner = 0.8;
            int k = (int)((x - x0) * Math.Cos(corner) - (y - y0) * Math.Sin(corner) + x0);
            int l = (int)((x - x0) * Math.Sin(corner) + (y - y0) * Math.Cos(corner) + y0);
            Color rezolt = Color.White;
            if ((k < 0) || (k > Img.Width - 1) || (l < 0) || (l > Img.Height - 1)) return rezolt;
            return Img.GetPixel(k, l);
        }
    }
    public class TisFilter : MatrixFilter
    {
        public TisFilter()
        {
            //float norm = 0;
            kernel = new float[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)

                    if ((i + j) % 2 == 0) kernel[i, j] = 0;
                    else if (i + j == 3) kernel[i, j] = -1;
                    else kernel[i, j] = 1;
            /*            for (int i = 0; i < 3; i++)
                            for (int j = 0; j < 3; j++)
                                kernel[i, j] /= norm;*/
            //kernel[1, 1] = 9;
        }
    }
    public class YBorderFilter : MatrixFilter
    {
        public YBorderFilter()
        {
            //float norm = 0;
            kernel = new float[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (i == 0) kernel[i, j] = -1;
                    else if (i == 1) kernel[i, j] = 0;
                    else kernel[i, j] = 1;
        }
    }
    public class XBorderFilter : MatrixFilter
    {
        public XBorderFilter()
        {
            //float norm = 0;
            kernel = new float[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (j == 0) kernel[i, j] = -1;
                    else if (j == 1) kernel[i, j] = 0;
                    else kernel[i, j] = 1;
        }
    }



}
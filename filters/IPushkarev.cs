using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace filters
{
    public class BinaryFilter : filters
    {
        double sat;
        bool IsPrepDone = false;
        protected double Preparation(Bitmap Img)
        {
            double sum = 0; ;
            for (int i = 0; i < Img.Width; i++)
                for (int j = 0; j < Img.Height; j++)
                {
                    sum += Img.GetPixel(i, j).GetSaturation();
                }
            sat=sum/ Img.Width/ Img.Height;
            IsPrepDone = true;
            return sat;
        }
        protected override Color MakeNewColor(Bitmap Img, int x, int y)
        {   if (!IsPrepDone) { Preparation(Img); }
            
            Color SourceColor = Img.GetPixel(x, y);
            Color ResColor = SourceColor;
            if (SourceColor.GetSaturation() >sat) ResColor = Color.FromArgb(0, 0, 0);
            else ResColor = Color.FromArgb(255, 255, 255);

            return ResColor;
        }
    }
    abstract public class MorphologyFilters : filters {
        protected int[,] kernel = null;
        protected int kx,ky;

        protected void setkernel(int[,] ker, int size)
        {
            kx = size;
            ky = size;
            kernel = new int[kx, ky];
            for (int i = 0; i < kx; i++)
            {
                for (int j = 0; j < ky; j++)
                {
                    kernel[i, j] = ker[i, j];
                }
            }
        }
        public bool StrictKernelAdd(Bitmap Img, int x, int y)
        {

            int sizex= Img.Width;
            int sizey= Img.Height;
            for (int i = x; i < x + kx; i++)
            {
                for (int j = y; j < y + ky; j++)
                {   if ((i < sizex) && (j < sizey))
                    {
                        if (Img.GetPixel(i, j).R == kernel[i - x, j - y]) return true;
                    }
                }
            }
            return false;            
        }
        public bool  KernelAdd(Bitmap Img, int x, int y)
        {

            int sizex = Img.Width;
            int sizey = Img.Height;
            for (int i = x; i < x + kx; i++)
            {
                for (int j = y; j < y + ky; j++)
                {
                    if ((i < sizex) && (j < sizey))
                    {
                        if (Img.GetPixel(i, j).R != kernel[i - x, j - y]) return false;
                    }
                }
            }
            return true;
        }


    }
    public class ErosionFilter : MorphologyFilters
    {
        public ErosionFilter(int[,] ker, int size)
        {
            setkernel(ker,size);

        }
       
        protected override Color MakeNewColor(Bitmap Img, int x, int y)
        {
            Color ResColor = Color.FromArgb(0, 0, 0);
            if (!StrictKernelAdd(Img, x, y)) ResColor = Color.FromArgb(255, 255, 255);
            return ResColor;
        }
    }

    public class DilationFilter : MorphologyFilters
    {
        public DilationFilter(int[,] ker, int size)
        {
            setkernel(ker, size);
        }

        protected override Color MakeNewColor(Bitmap Img, int x, int y)
        {
            Color ResColor = Color.FromArgb(0, 0, 0);
            if (!KernelAdd(Img, x, y)) ResColor = Color.FromArgb(255, 255, 255);
            return ResColor;
        }
    }
}
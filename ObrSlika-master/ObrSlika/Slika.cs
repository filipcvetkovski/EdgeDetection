﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ObrSlika
{
    public partial class Slika : Form
    {
        string pat_slika = "";
        public Slika()
        {

            InitializeComponent();
        }

        private void Slika_Load(object sender, EventArgs e)
        {

        }

        private void otvoriSlika_Click(object sender, EventArgs e)
        {
            prg1.Value = 0;
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Слики (*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp )";

                if (open.ShowDialog() == DialogResult.OK)
                {
                    pat_slika = open.FileName;

                    pBox.Image = new Bitmap(open.FileName);

                }
                LogFile.WriteInformationLog("Korisnikot otvori slika");


            }

            catch (Exception)
            {
                throw new ApplicationException("Сликата не може да се отвори");

                LogFile.WriteErrorLog("Проблем со отварање на слика");
            }
          
        }

        private void CannyАлгоритамToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (pat_slika == "") MessageBox.Show("Немате одбрано слика, Обидете се повторно");
            else
            {
                pBox.Invalidate();
                prg1.Invalidate();
                prg1.Value = 0;
                Bitmap p = new Bitmap(pat_slika);
                Bitmap obr = (Bitmap)Canny_Konvertiraj(p);
                pBox.Image = new Bitmap(obr);
                LogFile.WriteInformationLog("Обработена слика со Кени Алгоритмот");
            }
        }
        private Bitmap Canny_Konvertiraj(Bitmap im)
        {

            Bitmap b;
            int[,] gx = new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] gy = new int[,] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };
            b = im;
            Bitmap b1 = new Bitmap(im);
            prg1.Maximum = b.Height;
            prg1.Step = 1;
            for (int i = 1; i < b.Height - 1; i++)
            {
                for (int j = 1; j < b.Width - 1; j++)
                {
                    float new_x = 0, new_y = 0;
                    float c;
                    for (int hw = -1; hw < 2; hw++)
                    {
                        for (int wi = -1; wi < 2; wi++)
                        {
                            c = (b.GetPixel(j + wi, i + hw).B + b.GetPixel(j + wi, i + hw).R + b.GetPixel(j + wi, i + hw).G) / 3;
                            new_x += gx[hw + 1, wi + 1] * c;
                            new_y += gy[hw + 1, wi + 1] * c;
                        }
                    }
                    if (new_x * new_x + new_y * new_y > 128 * 128)
                        b1.SetPixel(j, i, Color.Black);
                    else
                        b1.SetPixel(j, i, Color.White);

                }
                prg1.Increment(1);
            }
            return b1;

        }

        private void одјавиСеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            login s1 = new login();
            this.Hide();
            s1.Show();
        }
        private Bitmap Laplace(Image slika)
        {
            Bitmap b;
            b = new Bitmap(slika);
            Bitmap b1 = new Bitmap(b.Clone() as Bitmap);
            prg1.Maximum = b.Width;

            float SUM = 0;
            prg1.Step = 1;
            float d = 1.0f;
            int[,] MASK = new int[5, 5];
            MASK[0, 0] = -1; MASK[0, 1] = -1; MASK[0, 2] = -1; MASK[0, 3] = -1; MASK[0, 4] = -1;
            MASK[1, 0] = -1; MASK[1, 1] = -1; MASK[1, 2] = -1; MASK[1, 3] = -1; MASK[1, 4] = -1;
            MASK[2, 0] = -1; MASK[2, 1] = -1; MASK[2, 2] = 24; MASK[2, 3] = -1; MASK[2, 4] = -1;
            MASK[3, 0] = -1; MASK[3, 1] = -1; MASK[3, 2] = -1; MASK[3, 3] = -1; MASK[3, 4] = -1;
            MASK[4, 0] = -1; MASK[4, 1] = -1; MASK[4, 2] = -1; MASK[4, 3] = -1; MASK[4, 4] = -1;
            float k = 0;
            for (int column = 1; column < b.Width - 1; column++)
            {
                for (int row = 1; row < b.Height - 1; row++)
                {
                    SUM = 0;
                    if (column == 0 || column == 1 || column == b.Width - 2 || column == b.Width - 1)
                        SUM = 0;
                    else if (row == 0 || row == 1 || row == b.Height - 2 || row == b.Height - 1)
                        SUM = 0;

                    else
                    {
                        for (int I = -2; I <= 2; I++)
                        {
                            for (int J = -2; J <= 2; J++)
                            {
                                k = SUM;
                                SUM += ((((b.GetPixel(column, row).R + b.GetPixel(column, row).G + b.GetPixel(column, row).B)) / d) * MASK[I + 2, J + 2]);
                            }
                        }
                    }

                    if (k > 255) k = 255;
                    if (k < 255) k = 0;
                    b1.SetPixel(column, row, Color.FromArgb((int)k, (int)k, (int)k));
                }
                prg1.Increment(1);
            }

            return b1;

        }

        private void LaplasАлгоритамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                if (pat_slika == "") MessageBox.Show("Немате одбрано слика, Обидете се повторно");
                else
                {
                    pBox.Invalidate();
                    prg1.Invalidate();
                    prg1.Value = 0;
                    Bitmap p = new Bitmap(pat_slika);
                    Bitmap obr = (Bitmap)Laplace(p);
                    pBox.Image = new Bitmap(obr);
                    LogFile.WriteInformationLog("Обработена слика со Лаплас Алгоритмот");

                }
            }
        }

        public Bitmap FilterProcessImage(Bitmap image)
        {
            Bitmap ret = new Bitmap(image.Width, image.Height);
            for (int i = 1; i < image.Width - 1; i++)
            {
                for (int j = 1; j < image.Height - 1; j++)
                {
                    Color cr = image.GetPixel(i + 1, j);
                    Color cl = image.GetPixel(i - 1, j);
                    Color cu = image.GetPixel(i, j - 1);
                    Color cd = image.GetPixel(i, j + 1);
                    Color cld = image.GetPixel(i - 1, j + 1);
                    Color clu = image.GetPixel(i - 1, j - 1);
                    Color crd = image.GetPixel(i + 1, j + 1);
                    Color cru = image.GetPixel(i + 1, j - 1);
                    int power = getMaxD(cr.R, cl.R, cu.R, cd.R, cld.R, clu.R, cru.R, crd.R);
                    if (power > 50)
                        ret.SetPixel(i, j, Color.Yellow);
                    else
                        ret.SetPixel(i, j, Color.Black);
                }
            }
            return ret;
        }

private int getD(int cr, int cl, int cu, int cd, int cld, int clu, int cru, int crd, int[,] matrix)
        {
            return Math.Abs(matrix[0, 0] * clu + matrix[0, 1] * cu + matrix[0, 2] * cru
               + matrix[1, 0] * cl + matrix[1, 2] * cr
                  + matrix[2, 0] * cld + matrix[2, 1] * cd + matrix[2, 2] * crd);
        }
        private int getMaxD(int cr, int cl, int cu, int cd, int cld, int clu, int cru, int crd)
        {
            int max = int.MinValue;
            for (int i = 0; i < templates.Count; i++)
            {
                int newVal = getD(cr, cl, cu, cd, cld, clu, cru, crd, templates[i]);
                if (newVal > max)
                    max = newVal;
            }
            return max;
        }
        private List<int[,]> templates = new List<int[,]>
{
   new int[,] {{ -3, -3, 5 }, { -3, 0, 5 }, { -3, -3, 5 } },
   new int[,] {{ -3, 5, 5 }, { -3, 0, 5 }, { -3, -3, -3 } },
   new int[,] {{ 5, 5, 5 }, { -3, 0, -3 }, { -3, -3, -3 } },
   new int[,] {{ 5, 5, -3 }, { 5, 0, -3 }, { -3, -3, -3 } },
   new int[,] {{ 5, -3, -3 }, { 5, 0, -3 }, { 5, -3, -3 } },
   new int[,] {{ -3, -3, -3 }, { 5, 0, -3 }, { 5, 5, -3 } },
   new int[,] {{ -3, -3, -3 }, { -3, 0, -3 }, { 5, 5, 5 } },
   new int[,] {{ -3, -3, -3 }, { -3, 0, 5 }, { -3, 5, 5 } }
};

        private void филтерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                if (pat_slika == "") MessageBox.Show("Немате одбрано слика, Обидете се повторно");
                else
                {
                    pBox.Invalidate();
                    prg1.Invalidate();
                    prg1.Value = 0;
                    Bitmap p = new Bitmap(pat_slika);
                    Bitmap obr = (Bitmap)FilterProcessImage(p);
                    pBox.Image = new Bitmap(obr);
                    LogFile.WriteInformationLog("Обработена слика со Криш Филтер");



                }
            }
        }
    }

}


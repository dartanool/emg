using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;


namespace Labs21
{
    public partial class Form1 : Form
    {
        private Image<Bgr, byte> sourceImage; //глобальная переменная
        private Image<Bgr, byte> secImage; //глобальная переменная
        private Image<Hsv, byte> hsvImage;  //глобальная переменная
        private Image<Bgr, byte> thImage;  //глобальная переменная
        private int channelIndex;
        private double h,s,v;
        private int brightnes,x;
        private int сontrastv;
        private int brightnes2, сontrastv2;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            imageBox2.Image = Pic.BWfilter(sourceImage);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            imageBox2.Image = Pic.Median(sourceImage);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            imageBox2.Image = Pic.Sepia(sourceImage);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog(); // открытие диалога выбора файла

            if (result == DialogResult.OK) // открытие выбранного файла
            {
                string fileName = openFileDialog.FileName;
                sourceImage = new Image<Bgr, byte>(fileName);
                hsvImage = sourceImage.Convert<Hsv, byte>();

            }

            imageBox1.Image = sourceImage.Resize(640, 480, Inter.Linear);
            imageBox2.Image = sourceImage.Split()[channelIndex].Resize(640, 480, Inter.Linear);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            channelIndex = (int)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            brightnes = (int)numericUpDown2.Value;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            imageBox2.Image = Pic.Brightness(sourceImage, brightnes);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            сontrastv = (int)numericUpDown3.Value;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            imageBox2.Image = Pic.Contrast(sourceImage, сontrastv);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result= openFileDialog.ShowDialog(); // открытие диалога выбора файла
            if (result == DialogResult.OK) // открытие выбранного файла
            {
                string fileName = openFileDialog.FileName;
                secImage = new Image<Bgr, byte>(fileName);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Image<Bgr, byte> intersectionImage = sourceImage.And(secImage);
            imageBox2.Image = intersectionImage;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Image<Bgr, byte> complementedImage = sourceImage.Not();
            imageBox2.Image = complementedImage;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Image<Bgr, byte> xorImage = sourceImage.Xor(secImage);
            imageBox2.Image = xorImage;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            s = (double)numericUpDown5.Value;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            imageBox2.Image = Pic.HSV(hsvImage, h,s,v);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int[,] filterMatrix = new int[3, 3]; 
            filterMatrix[0, 0] = int.Parse(textBox1.Text);
            filterMatrix[0, 1] = int.Parse(textBox2.Text); 
            filterMatrix[0, 2] = int.Parse(textBox3.Text);
            filterMatrix[1, 0] = int.Parse(textBox4.Text); 
            filterMatrix[1, 1] = int.Parse(textBox5.Text);
            filterMatrix[1, 2] = int.Parse(textBox6.Text); 
            filterMatrix[2, 0] = int.Parse(textBox7.Text);
            filterMatrix[2, 1] = int.Parse(textBox8.Text); 
            filterMatrix[2, 2] = int.Parse(textBox9.Text);
            imageBox2.Image = Pic.WindowFilter(sourceImage, filterMatrix);
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            Image<Bgr, byte> xorImage = sourceImage.Xor(secImage);
            imageBox2.Image = xorImage;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog(); // открытие диалога выбора файла
            if (result == DialogResult.OK) // открытие выбранного файла
            {
                string fileName = openFileDialog.FileName;
                thImage = new Image<Bgr, byte>(fileName);
            }
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            brightnes2 = (int)numericUpDown7.Value;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            imageBox2.Image = Pic.Brightness(sourceImage, brightnes2)+ Pic.Contrast(sourceImage, сontrastv2)+Pic.Median(sourceImage)+sourceImage.And(thImage);
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void button11_Click_1(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            imageBox2.Image = Pic.CartoonFilter(sourceImage, x);
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            x = (int)numericUpDown9.Value;
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            сontrastv2 = (int)numericUpDown8.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            h = (double)numericUpDown4.Value;
            
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            v = (double)numericUpDown6.Value;
        }
    }
    public class Pic
    {
        public static Image<Gray, byte> BWfilter(Image<Bgr, byte> sourceImage)
        {
            var grayImage = new Image<Gray, byte>(sourceImage.Size);
            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    grayImage.Data[y, x, 0] = Convert.ToByte(
            0.299 * sourceImage.Data[y, x, 2] +
            0.587 * sourceImage.Data[y, x, 1] +
            0.114 * sourceImage.Data[y, x, 0]
          );
                }
            }
            return grayImage;
        }
        public static Image<Bgr, byte> Sepia(Image<Bgr, byte> sourceImage)
        {
            var sepiaImage = new Image<Bgr, byte>(sourceImage.Size);
            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    Bgr color = sourceImage[y, x];
                    double red = (0.393 * color.Red) + (0.769 * color.Green) + (0.189 * color.Blue);
                    double green = (0.349 * color.Red) + (0.686 * color.Green) + (0.168 * color.Blue);
                    double blue = (0.272 * color.Red) + (0.534 * color.Green) + (0.131 * color.Blue);

                    sepiaImage[y, x] = new Bgr(blue, green, red);
                }
            }
            return sepiaImage;
        }
       
        public static Image<Bgr, byte> Median(Image<Bgr, byte> sourceImage)
        {
            var medianImage = new Image<Bgr, byte>(sourceImage.Size);

            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    List<byte> blueList = new List<byte>();
                    List<byte> greenList = new List<byte>();
                    List<byte> redList = new List<byte>();

                    for (int i = -4; i < 5; i++)
                    {
                        for (int j = -4; j < 5; j++)
                        {
                            int neighx = x + i;
                            int neighy = y + j;

                            if (neighx >= 0 && neighx < sourceImage.Width && neighy >= 0 && neighy < sourceImage.Height)
                            {
                                Bgr color = sourceImage[neighy, neighx];
                                blueList.Add((byte)color.Blue);
                                greenList.Add((byte)color.Green);
                                redList.Add((byte)color.Red);
                            }
                        }
                    }

                    blueList.Sort();
                    greenList.Sort();
                    redList.Sort();

                    int medianBlue = blueList[blueList.Count / 2];
                    int medianGreen = greenList[greenList.Count / 2];
                    int medianRed = redList[redList.Count / 2];

                    medianImage[y, x] = new Bgr(medianBlue, medianGreen, medianRed);
                }
            }
            return medianImage;
        }


        public static Image<Bgr, byte> Brightness( Image<Bgr, byte> sourceImage, int brightnes)
        {

            var brightImage = new Image<Bgr, byte>(sourceImage.Size);

            for (int x = 0; x <  sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++ )
                {
                    Bgr color = sourceImage[y, x];
                    double blue = color.Blue + (double)brightnes;
                    double green = color.Green + (double)brightnes;
                    double red = color.Red + (double)brightnes;


                    brightImage[y, x] = new Bgr(blue, green, red);
                }
            }
            return brightImage;
        }
        public static Image<Bgr, byte> Contrast(Image<Bgr, byte> sourceImage, int сontrastv)
        {

            var contrastImage = new Image<Bgr, byte>(sourceImage.Size);
            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    Bgr color = sourceImage[y, x];
                    double blue = color.Blue * (double)сontrastv;
                    double green = color.Green * (double)сontrastv;
                    double red = color.Red * (double)сontrastv;


                    contrastImage[y, x] = new Bgr(blue, green, red);
                }
            }
            return contrastImage;
        }




        public static Image<Bgr, byte> HSV(Image<Hsv, byte> hsvImage, double h,double s, double v)
        {

            Image<Hsv, byte>result = hsvImage.Copy();
            result = result.Add(new Hsv(h,s,v));

            Image<Bgr, byte> resultImage = result.Convert<Bgr, byte>();
            return resultImage;
        }




        public static Image<Bgr, byte> WindowFilter(Image<Bgr, byte> sourceImage, int[,] filterMatrix)
        {
            var filteredImage = new Image<Bgr, byte>(sourceImage.Size);
            int summ = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    summ += filterMatrix[i + 1, j + 1];
                }
            }

            for (int x = 1; x < sourceImage.Width - 1; x++)
            {
                for (int y = 1; y < sourceImage.Height - 1; y++)
                {
                    //Bgr color = sourceImage[y, x]; 
                    double red = 0, green = 0, blue = 0;
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int x1 = x + i;
                            int y1 = y + j;

                            if (x1 >= 0 && x1 < sourceImage.Width && y1 >= 0 && y1 < sourceImage.Height)
                            {
                                Bgr neighborColor = sourceImage[y1, x1];
                                red += (double)((neighborColor.Red * filterMatrix[i + 1, j + 1]) / summ);
                                green += (double)((neighborColor.Green * filterMatrix[i + 1, j + 1]) / summ);
                                blue += (double)((neighborColor.Blue * filterMatrix[i + 1, j + 1]) / summ);
                            }
                        }
                    }
                    filteredImage[y, x] = new Bgr(blue, green, red);
                }
            }
            return filteredImage;
        }

        public static Image<Bgr, byte> CartoonFilter(Image<Bgr, byte> sourceImage, int x)
        {
            var result = Pic.BWfilter(Median(sourceImage));
            var edges = result.Convert<Gray, byte>();
            edges = edges.ThresholdAdaptive(new Gray(x), AdaptiveThresholdType.MeanC, ThresholdType.Binary, 3, new Gray(0.03));
            var andImage = sourceImage.And(new Bgr(255, 255, 255), edges);
            Image<Bgr, byte> cartoonImage = andImage.Convert<Bgr, byte>();
            return cartoonImage;
        }
    }
}


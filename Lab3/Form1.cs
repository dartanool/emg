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

namespace Lab3
{
    public partial class Form1 : Form
    {
        private Image<Bgr, byte> sourceImage;
        float x, y, shift;
        float cenx, ceny, angle;
        int qY, qX;
        private PointF[] srcPoints;
        private int pointsSelected = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog(); // открытие диалога выбора файла

            if (result == DialogResult.OK) // открытие выбранного файла
            {
                string fileName = openFileDialog.FileName;
                sourceImage = new Image<Bgr, byte>(fileName);

            }

            imageBox1.Image = sourceImage.Resize(640, 480, Inter.Linear);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            y = float.Parse(textBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            imageBox2.Image=Pic.Scalingg(sourceImage,x,y);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            cenx = float.Parse(textBox5.Text);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            ceny = float.Parse(textBox4.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            imageBox2.Image = Pic.Rotation(sourceImage, cenx, ceny, angle);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            angle = float.Parse(textBox6.Text);
        }

        

        private void button5_Click(object sender, EventArgs e)
        {
            imageBox2.Image = Pic.Reflection(sourceImage, qX,qY);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            qX = (int)numericUpDown1.Value;
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            qY = (int)numericUpDown2.Value;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            imageBox2.Image = Pic.Projection(sourceImage, srcPoints);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            imageBox2.Image = Pic.shearing(sourceImage,shift);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            shift = float.Parse(textBox3.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            x = float.Parse(textBox1.Text);
        }

        private void imageBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = (int)(e.Location.X / imageBox1.ZoomScale);
            int y = (int)(e.Location.Y / imageBox1.ZoomScale);

            Point center = new Point(x, y);
            int radius = 2;
            int thickness = 2;
            var color = new Bgr(Color.Blue).MCvScalar;
            // функция, рисующая на изображении круг с заданными параметрами
            CvInvoke.Circle(sourceImage, center, radius, color, thickness);
            srcPoints[pointsSelected] = new PointF(x, y);
            pointsSelected++;
            if (pointsSelected == 4)
            {
                imageBox2.Image = Pic.Projection(sourceImage, srcPoints);
            }

        }
    }



        public class Pic
    {
        public static Image<Bgr, byte> Scalingg(Image<Bgr, byte> sourceImage, float X, float Y)
        {
            var newImage = new Image<Bgr, byte>((int)(sourceImage.Width * X), (int)(sourceImage.Height * Y));
            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    // вычисление новых координат пикселя
                    int newX = (int)(x * X);
                    int newY = (int)(y * Y);
                    // копирование пикселя в новое изображение
                    newImage[newY, newX] = sourceImage[y, x];
                }
            }
            return newImage;
        }

        //СДВИГ

        public static Image<Bgr, byte> shearing(Image<Bgr, byte> sourceImage, float shift)
        {
            var newImage = new Image<Bgr, byte>(sourceImage.Size);
            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    // вычисление новых координат пикселя
                    int newX = (int)(x + shift * (sourceImage.Height - y));
                    int newY = (int)(y);
                    if (newX >= 0 && newX < sourceImage.Width && newY >= 0 && newY < sourceImage.Height)
                    {
                        // копирование пикселя в новое изображение
                        newImage[newY, newX] = sourceImage[y, x];
                    }
                   
                }
            }
            return newImage;
        }

        //rotation
        public static Image<Bgr, byte> Rotation(Image<Bgr, byte> sourceImage, float cenx, float ceny, float angle)
        {
            var newImage = new Image<Bgr, byte>(sourceImage.Size);
            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    // вычисление новых координат пикселя
                    int newX = (int)(Math.Cos(angle)*(x-cenx) -Math.Sin(angle)*(y - ceny)+cenx);
                    int newY = (int)(Math.Sin(angle) * (x - cenx) + Math.Cos(angle) * (y - ceny) + ceny);
                    if (newX >= 0 && newX < sourceImage.Width && newY >= 0 && newY < sourceImage.Height)
                    {
                        // копирование пикселя в новое изображение
                        newImage[newY, newX] = sourceImage[y, x];
                    }

                }
            }
            return newImage;
        }

        //reflection

        public static Image<Bgr, byte> Reflection(Image<Bgr, byte> sourceImage, int qX, int qY)
        {
            var newImage = new Image<Bgr, byte>(sourceImage.Size);
            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    // вычисление новых координат пикселя
                    int newX = (int)(x * qX - sourceImage.Width);
                    int newY = (int)(y * qY - sourceImage.Height);
                    if (newX >= 0 && newX < sourceImage.Width && newY >= 0 && newY < sourceImage.Height)
                    {
                        // копирование пикселя в новое изображение
                        newImage[newY, newX] = sourceImage[y, x];
                    }

                }
            }
            return newImage;
        }




        ////// Фильтрация


        ///


        public static Image<Bgr, byte> Projection(Image<Bgr, byte> sourceImage, PointF[] srcPoints)
        {
            PointF[] destPoints =
            {
            new PointF(0, 0),
            new PointF(sourceImage.Width - 1, 0),
            new PointF(sourceImage.Width - 1, sourceImage.Height - 1),
            new PointF(0, sourceImage.Height - 1)
            };

            var homographyMatrix = CvInvoke.GetPerspectiveTransform(srcPoints, destPoints);

            var destImage = new Image<Bgr, byte>(sourceImage.Size);
            CvInvoke.WarpPerspective(sourceImage, destImage, homographyMatrix, destImage.Size);

            return destImage;
        }


    }

}

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


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Image<Bgr, byte> sourceImage; //глобальная переменная
        private double cannyThreshold = 80;
        private double cannyThresholdLinking = 40;
        private VideoCapture capture;


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
                
                imageBox2.Image = Canny(sourceImage).Resize(540,480, Inter.Linear);
                imageBox1.Image = sourceImage.Resize(640, 480, Inter.Linear);
            }

        }

        private void imageBox1_Click(object sender, EventArgs e)
        {

        }

        private void imageBox2_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            cannyThreshold = (double)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            cannyThresholdLinking = (double)numericUpDown2.Value;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // инициализация веб-камеры
            capture = new VideoCapture();
            capture.ImageGrabbed += ProcessFrame;
            capture.Start(); // начало обработки видеопотока

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog(); // открытие диалога выбора файла
            if (result == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                capture = new VideoCapture(fileName);
                capture.ImageGrabbed += ProcessFrame;
                capture.Start(); // начало обработки видеопотока
            }
        }   

        private void ProcessFrame(object sender, EventArgs e)
        {
            var frame = new Mat();
            capture.Retrieve(frame); // получение текущего кадра
            Image<Bgr, byte> image = frame.ToImage<Bgr, byte>();
            imageBox1.Image = image.Resize(540, 480, Inter.Linear);
            imageBox2.Image = Canny(image).Resize(540, 480, Inter.Linear);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            capture.Stop(); // остановка обработки видеопотока
        }

        public Image<Bgr, byte> Canny(Image<Bgr, byte> sourceImage)
        {
            Image<Gray, byte> grayImage = sourceImage.Convert<Gray, byte>();
            var tempImage = grayImage.PyrDown();
            var destImage = tempImage.PyrUp();
            Image<Gray, byte> cannyEdges = destImage.Canny(cannyThreshold, cannyThresholdLinking);

            var cannyEdgesBgr = cannyEdges.Convert<Bgr, byte>();
            var resultImage = sourceImage.Sub(cannyEdgesBgr); // попиксельное вычитание

            for (int channel = 0; channel < resultImage.NumberOfChannels; channel++)
                for (int x = 0; x < resultImage.Width; x++)
                    for (int y = 0; y < resultImage.Height; y++) // обход по пискелям
                    {
                        // получение цвета пикселя
                        byte color = resultImage.Data[y, x, channel];
                        if (color <= 50)
                            color = 0;
                        else if (color <= 100)
                            color = 25;
                        else if (color <= 150)
                            color = 180;
                        else if (color <= 200)
                            color = 210;
                        else
                            color = 255;
                        resultImage.Data[y, x, channel] = color; // изменение цвета пикселя
                    }
            return resultImage;
        }
    }
}

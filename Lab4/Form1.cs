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

namespace Lab4
{
    public partial class Form1 : Form
    {
        private Image<Bgr, byte> sourceImage, contoursImage; 
        private Image<Gray, byte> binarizedImage; //глобальная переменная
        int threshold, minArea;
        private VectorOfVectorOfPoint contours;
        private VectorOfPoint approxContour;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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

        private void button2_Click(object sender, EventArgs e)
        {
            var grayImage = sourceImage.Convert<Gray, byte>();
            int kernelSize = 5; // радиус размытия
            var bluredImage = grayImage.SmoothGaussian(kernelSize);
            var color = new Gray(255); // этим цветом будут закрашены пиксели, имеющие значение > threshold
            binarizedImage = bluredImage.ThresholdBinary(new Gray(threshold), color);
            imageBox2.Image = binarizedImage;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            contours = new VectorOfVectorOfPoint(); // контейнер для хранения контуров
            CvInvoke.FindContours(binarizedImage, // исходное чёрно-белое изображение
             contours, // найденные контуры
             null, // объект для хранения иерархии контуров (в данном случае не используется)
             RetrType.List, // структура возвращаемых данных (в данном случае список)
             ChainApproxMethod.ChainApproxSimple); // метод аппроксимации (сжимает горизонтальные,
                                                   //вертикальные и диагональные сегменты
                                                   //и оставляет только их конечные точки)
            contoursImage = sourceImage.CopyBlank(); //создание "пустой" копии исходного изображения
            for (int i = 0; i < contours.Size; i++)
            {
                var points = contours[i].ToArray();
                contoursImage.Draw(points, new Bgr(Color.GreenYellow), 2); // отрисовка точек
            }
            imageBox2.Image = contoursImage;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            threshold = (int)numericUpDown1.Value;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int count=0;
            for (int i = 0; i < contours.Size; i++)
            {
                approxContour = new VectorOfPoint();
                CvInvoke.ApproxPolyDP(
                contours[i], // исходный контур
                approxContour, // контур после аппроксимации
                CvInvoke.ArcLength(contours[i], true) * 0.05, // точность аппроксимации, прямо
                                                              //пропорциональная площади контура
                true); // контур становится закрытым (первая и последняя точки соединяются)

                // проверка на площадь треугольника > минимально допустимой площади
                if (CvInvoke.ContourArea(approxContour, false) > minArea)
                {
                    if (approxContour.Size == 3) // если контур содержит 3 точки, то рисуется треугольник
                    {
                        var points = approxContour.ToArray();
                        contoursImage.Draw(new Triangle2DF(points[0], points[1], points[2]),  new Bgr(Color.GreenYellow), 2);
                        count++;                    }
                }
            }

            var resultImage = contoursImage;
            string numOf = $"{count}";
            CvInvoke.PutText(resultImage, numOf, new Point(10, 30), FontFace.HersheyComplex, 0.7, new MCvScalar(255, 255, 255), 2); 
            imageBox2.Image = resultImage;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bool check = false;
            int count = 0;
            for (int i = 0; i < contours.Size; i++)
            {
                approxContour = new VectorOfPoint();
                CvInvoke.ApproxPolyDP(
                contours[i], // исходный контур
                approxContour, // контур после аппроксимации
                CvInvoke.ArcLength(contours[i], true) * 0.05, // точность аппроксимации, прямо
                                                              //пропорциональная площади контура
                true); // контур становится закрытым (первая и последняя точки соединяются)
                check = isRectangle(approxContour.ToArray());
                // проверка на площадь треугольника > минимально допустимой площади
                if (CvInvoke.ContourArea(approxContour, false) > minArea)
                {
                    if (approxContour.Size == 4 && check) // если контур содержит 3 точки, то рисуется треугольник
                    {
                        var points = approxContour.ToArray();
                        contoursImage.Draw(CvInvoke.MinAreaRect(approxContour), new Bgr(Color.GreenYellow), 2);
                        count++;
                    }
                }
            }

            var resultImage = contoursImage;
            string numOf = $"{count}";
            CvInvoke.PutText(resultImage, numOf, new Point(10, 30), FontFace.HersheyComplex, 0.7, new MCvScalar(255, 255, 255), 2);
            imageBox2.Image = resultImage;

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            minArea = (int)numericUpDown2.Value;
        }
        private bool isRectangle(Point[] points)
        {
            int delta = 10; // максимальное отклонение от прямого угла
            LineSegment2D[] edges = PointCollection.PolyLine(points, true);
            for (int i = 0; i < edges.Length; i++) // обход всех ребер контура
            {
                double angle = Math.Abs(edges[(i + 1) %
                edges.Length].GetExteriorAngleDegree(edges[i]));
                if (angle < 90 - delta || angle > 90 + delta) // если угол непрямой
                {
                    return false;
                }
            }
            return true;
        }

    }
}

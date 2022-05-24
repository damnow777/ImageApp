using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using System.Windows.Forms;

namespace ImageApp2
{
    public partial class Form1 : Form
    {
        OpenFileDialog fileDialog = new OpenFileDialog();


        public Form1()
        {
            InitializeComponent();
        }

        // START TIMER
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        // LOAD FILE
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                fileDialog.Title = "Wybierz obraz";
                fileDialog.Filter = "jpg files (*.jpg) |*.jpg| PNG files (*.png)|*.png| All Files (*.*) |*.*";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    Bitmap image = new Bitmap(fileDialog.FileName);
                    int x, y;

                    //for (x = 0; x < image.Width; x++)
                    //{
                    //    for (y = 0; y < image.Height; y++)
                    //    {
                    //        Color pixelColor = image.GetPixel(x, y);
                    //        Color newColor = Color.Gray;//FromArgb(pixelColor.R, 0, 0);
                    //        image.SetPixel(x, y, newColor);
                    //    }
                    //}
                    pictureBox1.Image = image;
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Błąd wczytywania obrazu", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // CLEAR PICTUREBOX
        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
                ImageClear();
            else
                MessageBox.Show("Brak wczytanego obrazu", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        // CLEAR PICTUREBOX METHOD
        private void ImageClear()
        {
            pictureBox1.CancelAsync();
            pictureBox1.Image.Dispose();
            pictureBox1.Image = null;
            redBar.Value = 0;
            greenBar.Value = 0;
            blueBar.Value = 0;
        }

        // SAVE IMAGE METHOD
        private void SaveImage()
        {
            try
            {
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Title = "Zapisz obraz";
                fileDialog.Filter = "jpg files (*.jpg) |*.jpg| PNG files (*.png)|*.png| All Files (*.*) |*.*";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (fileDialog.FileName != "")
                    {
                        pictureBox1.Image.Save(fileDialog.FileName);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Błąd zapisu obrazu", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        // EXIT APP
        private void button3_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                DialogResult result = MessageBox.Show("Czy chcesz zapisać obraz?", "Uwaga", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    SaveImage();
                }
            }
            Application.Exit();
        }

        // SAVE IMAGE
        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
                SaveImage();
            else
                MessageBox.Show("Brak obrazu do zapisu", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // GET TIME
        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToLongTimeString();
        }

        // RGB TRACKBAR METHOD
        private void setColor()
        {
            float changeRed = redBar.Value * 0.1f;
            float changeGreen = greenBar.Value * 0.1f;
            float changeBlue = blueBar.Value * 0.1f;

            if (pictureBox1.Image != null)
            {
                Image image = pictureBox1.Image;
                Bitmap bmpInverter = new Bitmap(image.Width, image.Height);
                ImageAttributes imageAttributes = new ImageAttributes();
                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                {
                new float[] { 1+changeRed, 0, 0, 0, 0},
                new float[] { 0, 1+changeGreen, 0, 0, 0},
                new float[] { 0, 0, 1+changeBlue, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {0, 0, 0, 0, 1}
                });

                imageAttributes.SetColorMatrix(colorMatrix);
                Graphics g = Graphics.FromImage(bmpInverter);
                g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
                g.Dispose();
                pictureBox1.Image = bmpInverter;
            }

        }

        private void redBar_Scroll(object sender, EventArgs e)
        {
            setColor();
        }

        private void greenBar_Scroll(object sender, EventArgs e)
        {
            setColor();
        }

        private void blueBar_Scroll(object sender, EventArgs e)
        {
            setColor();
        }

        // RESET
        private void reset()
        {
            Image file;
            if (pictureBox1 != null)
            {
                file = Image.FromFile(fileDialog.FileName);
                pictureBox1.Image = file;
                redBar.Value = 0;
                greenBar.Value = 0;
                blueBar.Value = 0;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            reset();
        }

        // SKALA SZAROŚCI
        private void button2_Click_1(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            int x, y;
            Bitmap bitmap = new Bitmap(pictureBox1.Image);
            double R = 0.299, G = 0.527, B = 0.114;
            for (x = 0; x < pictureBox1.Image.Width; x++)
            {
                for (y = 0; y < pictureBox1.Image.Height; y++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    int wartoscSkali = (int)(
                        (pixelColor.R * R) +
                        (pixelColor.G * G) +
                        (pixelColor.B * B));
                    Color skalaSzarosci = Color.FromArgb(
                        pixelColor.A,
                        wartoscSkali,
                        wartoscSkali,
                        wartoscSkali);
                    bitmap.SetPixel(x, y, skalaSzarosci);
                }
            }
            pictureBox1.Image = bitmap;
        }

        // HISTOGRAM RGB
        private void button3_Click_1(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Bitmap c = new Bitmap(pictureBox1.Image);
                int[] red = new int[256];
                int[] green = new int[256];
                int[] blue = new int[256];
                for (int x = 0; x < pictureBox1.Width; x++)
                {
                    for (int y = 0; y < pictureBox1.Height; y++)
                    {
                        Color pixel = ((Bitmap)pictureBox1.Image).GetPixel(x, y);
                        red[pixel.R]++;
                        green[pixel.G]++;
                        blue[pixel.B]++;
                    }
                }


                chart1.Series["red"].Points.Clear();
                chart1.Series["green"].Points.Clear();
                chart1.Series["blue"].Points.Clear();
                for (int i = 0; i < 256; i++)
                {
                    chart1.Series["red"].Points.AddXY(i, red[i]);
                    chart1.Series["green"].Points.AddXY(i, green[i]);
                    chart1.Series["blue"].Points.AddXY(i, blue[i]);
                }
                chart1.Invalidate();
            }
            else
            {
                MessageBox.Show("Brak zdjęcia", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // HISTOGRAM
        private void button4_Click_1(object sender, EventArgs e)
        {
            {
                int z, b;
                if (pictureBox1.Image != null)
                {
                    Bitmap bitmap = new Bitmap(pictureBox1.Image);
                    double R = 0.299, G = 0.527, B = 0.114;
                    for (z = 0; z < pictureBox1.Image.Width; z++)
                    {
                        for (b = 0; b < pictureBox1.Image.Height; b++)
                        {
                            Color pixelColor = bitmap.GetPixel(z, b);
                            int wartoscSkali = (int)(
                                (pixelColor.R * R) +
                                (pixelColor.G * G) +
                                (pixelColor.B * B));
                            Color skalaSzarosci = Color.FromArgb(
                                pixelColor.A,
                                wartoscSkali,
                                wartoscSkali,
                                wartoscSkali);
                            bitmap.SetPixel(z, b, skalaSzarosci);
                        }
                    }

                    int[] red = new int[256];
                    int[] green = new int[256];
                    int[] blue = new int[256];
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        for (int y = 0; y < bitmap.Height; y++)
                        {
                            Color pixel = ((Bitmap)bitmap).GetPixel(x, y);
                            red[pixel.R]++;
                            green[pixel.G]++;
                            blue[pixel.B]++;
                        }
                    }

                    chart2.Series["red"].Points.Clear();
                    chart2.Series["green"].Points.Clear();
                    chart2.Series["blue"].Points.Clear();
                    for (int i = 0; i < 256; i++)
                    {
                        chart2.Series["red"].Points.AddXY(i, red[i]);
                        chart2.Series["green"].Points.AddXY(i, green[i]);
                        chart2.Series["blue"].Points.AddXY(i, blue[i]);
                    }
                    chart2.Invalidate();
                }
                else
                {
                    MessageBox.Show("Brak zdjęcia", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


    }
}

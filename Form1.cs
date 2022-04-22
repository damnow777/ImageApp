using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        // LOAD FILE
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Title = "Wybierz obraz";
                fileDialog.Filter = "jpg files (*.jpg) |*.jpg| PNG files (*.png)|*.png| All Files (*.*) |*.*";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    Bitmap image = new Bitmap(fileDialog.FileName);
                    //int x, y;

                    //for (x = 0; x < image.Width; x++)
                    //{
                    //    for (y = 0; y < image.Height; y++)
                    //    {
                    //        Color pixelColor = image.GetPixel(x, y);
                    //        Color newColor = Color.FromArgb(pixelColor.G, 0, 0);
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
    }
}

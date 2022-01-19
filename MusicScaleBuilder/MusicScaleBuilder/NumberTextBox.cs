using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicScaleBuilder
{
    /// <summary>
    /// Текстовое поле числового типа
    /// </summary>
    public partial class NumberTextBox : UserControl
    {
        public NumberTextBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Вид числа 
        /// </summary>
        public enum NumberView
        {
            /// <summary>
            /// Целое число
            /// </summary>
            ViewInt = 0,
            /// <summary>
            /// Вещественное число
            /// </summary>
            ViewDouble = 1
        }

        NumberView View;

        private void NumberTextBox_Resize(object sender, EventArgs e)
        {
            textBox1.Size = (sender as NumberTextBox).Size;
        }

        private void TextBox1_Resize(object sender, EventArgs e)
        {

        }

        private void NumberTextBox_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Вид числа в текстовом поле
        /// </summary>
        public NumberView NumberValueView
        {
            get { return View; }
            set { View = value; }
        }

        /// <summary>
        /// Метод принудительного вызывания события назатия клавиши в текстовом поле, но системного
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void KeyPressInControl(object sender, KeyPressEventArgs e)
        {
            try
            {
                NumberView ValueView = this.NumberValueView;
                char sign = e.KeyChar;
                TextBox Text;
                if ((sender is TextBox))
                {
                    Text = (sender as TextBox);
                }
                else
                {
                    Text = textBox1;
                    KeyPressInControl(Text, e);
                    return;
                }
                if (Text != this.textBox1)
                {
                    Text = textBox1;
                    KeyPressInControl(Text, e);
                    return;
                }

                if (sign == '.')
                {
                    e.KeyChar = ',';
                }
                sign = e.KeyChar;
                if (sign == ',' && ValueView == NumberView.ViewInt)
                {
                    e.Handled = true;
                    return;
                }
                else
                {
                    if (e.KeyChar == 8)
                    {
                        return;
                    }
                    else if(char.IsDigit(sign))
                    {
                        return;

                    }
                    else if (sign == ',')
                    {
                        if (this.textBox1.Text.Contains(","))
                        {
                            e.Handled = true;
                        }
                        if (this.textBox1.SelectionStart == 0 || this.textBox1.Text == "")
                        {
                            e.Handled = true;
                            this.textBox1.Text = "0," + this.textBox1.Text;
                        }
                        return;
                    }
                }
                e.Handled = true;
                if (e.KeyChar == 27)
                {
                    (sender as TextBox).Text = "0";
                }
            }
            catch
            {

            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (this.NumberValueView == NumberView.ViewInt)
            {
                if (!int.TryParse(this.textBox1.Text, out int a))
                {
                    this.textBox1.Text = "0";
                }
            }
            else
            {
                if (!double.TryParse(this.textBox1.Text, out double a))
                {
                    this.textBox1.Text = "0";
                }
            }
        }
    }
}

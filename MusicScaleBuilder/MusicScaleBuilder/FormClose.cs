using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MusicScaleBuilder
{
    public partial class FormClose : Form
    {
        public FormClose()
        {
            InitializeComponent();
        }

        private void ExitCansel_Click(object sender, EventArgs e)
        {
            if (File.Exists("Exit.txt"))
            {
                File.Delete("Exit.txt");
            }
            this.Hide();

            string Message = "Уважаемый пользователь ";
            if (File.Exists("User.txt"))
            {
                StreamReader reader = new StreamReader("User.txt");
                Message += "С именем " + reader.ReadLine();
                reader.Close();
            }

            Message += Environment.NewLine + " Работа программы продолжается " + Environment.NewLine;
            Message += " Все возможности программы вам предоставлены " + Environment.NewLine;
            Message += "С уважением, Создатель программы, Сидоров Антон Дмитриевич";


            MessageBox.Show(Message);
            //  
            this.Close();

        }

        private void Reload_Click(object sender, EventArgs e)
        {
            DialogResult res;

            string Message = "Уважаемый пользователь ";
            if (File.Exists("User.txt"))
            {
                StreamReader reader = new StreamReader("User.txt");
                Message += "С именем " + reader.ReadLine();
                reader.Close();
            }

            Message += Environment.NewLine + " Вы действительно хотите перезапустить программу?" + Environment.NewLine;
            Message += "После перезагрузки программа будет работать сначала" + Environment.NewLine;
            Message += "С уважением, Создатель программы, Сидоров Антон Дмитриевич";

            res = MessageBox.Show(Message, "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.No)
            {
                return;

            }
            

            Properties.Settings.Default.CloseFile = "Reload";
            
            this.Hide();
            PermamentForm form = (PermamentForm)Application.OpenForms[0];
            form.Hide();


            Message = "Уважаемый пользователь ";
            if (File.Exists("User.txt"))
            {
                StreamReader reader = new StreamReader("User.txt");
                Message += "С именем " + reader.ReadLine();
                reader.Close();
            }

            Message += Environment.NewLine + "Программы перезапущена " + Environment.NewLine;
            Message += " Все возможности программы вам предоставлены " + Environment.NewLine;
            Message += "С уважением, Создатель программы, Сидоров Антон Дмитриевич";


            MessageBox.Show(Message);


            this.Close();

        }

        private void Exit_Click(object sender, EventArgs e)
        {
            DialogResult res;

            string Message = "Уважаемый пользователь ";
            if (File.Exists("User.txt"))
            {
                StreamReader reader = new StreamReader("User.txt");
                Message += "С именем " + reader.ReadLine();
                reader.Close();
            }

            Message += Environment.NewLine + "Вы действительно хотите выйти из программы?" + Environment.NewLine;
            Message += "После выхода вы также можете снова запуститить прогамму" + Environment.NewLine;
            Message += "С уважением, Создатель программы, Сидоров Антон Дмитриевич";

            res = MessageBox.Show(Message, "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.No)
            {
                return;

            }
            Properties.Settings.Default.CloseFile = "Close";

            this.Hide();

            

            Message = "Уважаемый пользователь ";
            if (File.Exists("User.txt"))
            {
                StreamReader reader = new StreamReader("User.txt");
                Message += "С именем " + reader.ReadLine();
                reader.Close();
            }

            Message += Environment.NewLine + "Работа программы завершена" + Environment.NewLine;
            Message += "Спасибо, что воспользовались программой" + Environment.NewLine;
            Message += " Вы также можете снова запустить программу " + Environment.NewLine;
            Message += "С уважением, Создатель программы, Сидоров Антон Дмитриевич";

            MessageBox.Show(Message);
            //

            this.Close();

        }

        private void FormClose_Load(object sender, EventArgs e)
        {

        }

        private void FormClose_Load_1(object sender, EventArgs e)
        {
            string Message = "Уважаемый пользователь ";

            Properties.Settings.Default.CloseFile = "No";

            Message += Environment.NewLine + "Вы действительно хотите выйти из программы?" + Environment.NewLine;
            Message += "Вы, также, можете запустить эту программу снова" + Environment.NewLine;
            Message += "Вы, также можете перезапустить программу" + Environment.NewLine;
            Message += "С уважением, Создатель программы, Сидоров Антон Дмитриевич";
            label3.Text = Message;
            this.Icon = ((PermamentForm)Application.OpenForms[0]).Icon;
        }
    }
}

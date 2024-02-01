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
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Diagnostics;

namespace TextEditing
{
    public partial class Codingcs : Form
    {

        public List<string> links = new List<string>();
        public string openfile = String.Empty;

        public Codingcs()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            links.Add("System.Core.dll");
            richTextBox1.Text = "System.Core.dll";
            autocompleteMenu1.Items = File.ReadAllLines("cs-reserv-list.dicr"); //динамическое подключение словаря
        }

        private void fastColoredTextBox1_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            string text = fastColoredTextBox1.Text;
            string[] lines = text.Split('\n');
            label2.Text = "Символов: " + text.Length.ToString();
            label1.Text = "Строк: " + lines.Length.ToString();
        }

        private void открыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CSharp sourse code (*.cs)| *.cs";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            fastColoredTextBox1.Text = File.ReadAllText(openFileDialog1.FileName);
            openfile = openFileDialog1.FileName;
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(openfile, fastColoredTextBox1.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("файла не существует");
            }
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "CSharp sourse code (*.cs)| *.cs";
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            File.WriteAllText(saveFileDialog1.FileName, fastColoredTextBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "DLL file (*.dll)| *.dll";
            if (openFileDialog2.ShowDialog() == DialogResult.Cancel)
                return;
            links.Add(openFileDialog2.FileName);
            richTextBox1.Text += "\n" + openFileDialog2.FileName;
        }

        private void скомпилироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
           if(File.ReadAllText(@"files/run-app-file.cs") != fastColoredTextBox1.Text)
           {
                File.WriteAllText(@"run-app-file.cs", fastColoredTextBox1.Text);
           }
            string sourceName = @"run-app-file.cs";
            FileInfo sourceFile = new FileInfo(sourceName);
            CodeDomProvider provider = null;
            bool compileOK = false;

            provider = CodeDomProvider.CreateProvider("CSharp");

            if(provider != null)
            {
                String exeName = String.Format(@"{0}\{1}.exe",
                    System.Environment.CurrentDirectory,
                    sourceFile.Name.Replace(".", "_"));

                CompilerParameters cp = new CompilerParameters(links.ToArray(), fastColoredTextBox1.Text,true);
                cp.GenerateExecutable = true;
                cp.OutputAssembly = exeName;

                cp.GenerateInMemory = false;

                cp.TreatWarningsAsErrors = false;

                CompilerResults cr = provider.CompileAssemblyFromFile(cp,sourceName);

                if(cr.Errors.Count > 0)
                {
                    foreach(CompilerError ce in cr.Errors)
                    {
                        MessageBox.Show(ce.ToString());
                    }
                }                 

                if (cr.Errors.Count > 0)
                {
                    MessageBox.Show("приложение не запущено");
                }
                else
                {
                    MessageBox.Show("приложение запущено");
                    Process.Start(@"run-app-file_cs.exe");
                }
            }
        }

        private void вернутьсяНазадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            Hide();
        }
    }
}

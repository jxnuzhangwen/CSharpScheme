using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Interpreter.Scheme;

namespace CShareSchemeApp
{
    public partial class SchemeForm : LargeForm
    {
        private Scheme s = new Scheme();
        public SchemeForm()
        {
            InitializeComponent();
        }
        private int writeStartIndex;
        private void SchemeForm_Load(object sender, EventArgs e)
        {
            this.consoleTextBox.Text = ">";
            writeStartIndex = this.consoleTextBox.Text.Length;
            this.consoleTextBox.SelectionStart = writeStartIndex;
        }

        private void consoleTextBox_KeyUp(object sender, KeyEventArgs e)
        {

            if (this.consoleTextBox.SelectionStart < writeStartIndex)
            {
                this.consoleTextBox.SelectionStart = writeStartIndex;
                e.Handled = false;
                e.SuppressKeyPress = true;
            }
            if (e.KeyCode == Keys.Enter)
            {
                InputPort ins = new InputPort(new StringReader(getInput()));
                try
                {
                    Object x = null;
                    for (; ; )
                    {
                        if (InputPort.isEOF(x = ins.read())) break;
                        var result = s.eval(x);
                        if(result is char[]){
                            result = new String((char[])result);
                        }
                        this.consoleTextBox.AppendText(result.ToString());
                       
                    }
                    this.consoleTextBox.AppendText(System.Environment.NewLine);
                    this.consoleTextBox.AppendText(">");
                    writeStartIndex = this.consoleTextBox.Text.Length;
                    this.consoleTextBox.SelectionStart = writeStartIndex;
                }
                catch(Exception ex) {
                    this.consoleTextBox.AppendText(ex.Message.ToString());
                    this.consoleTextBox.AppendText(System.Environment.NewLine);
                    this.consoleTextBox.AppendText(">");
                    writeStartIndex = this.consoleTextBox.Text.Length;
                    this.consoleTextBox.SelectionStart = writeStartIndex;
                }
            }
                /*
            else if (e.Modifiers.CompareTo(Keys.Control)==0 && e.KeyCode == Keys.C) {
                this.consoleTextBox.AppendText(System.Environment.NewLine);
                this.consoleTextBox.AppendText(">");
                writeStartIndex = this.consoleTextBox.Text.Length;
                this.consoleTextBox.SelectionStart = writeStartIndex;
            }
            */

        }

        private string getInput() {
            return this.consoleTextBox.Text.Substring(writeStartIndex);
        }


        private void consoleTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(e.KeyCode.ToString());
            if (this.consoleTextBox.SelectionLength > 0)
            {
                e.Handled = false;
                e.SuppressKeyPress = true;
            }
            if (this.consoleTextBox.SelectionStart < writeStartIndex)
            {
                this.consoleTextBox.SelectionStart = writeStartIndex;
                e.Handled = false;
                e.SuppressKeyPress = true;
            }
            if (this.consoleTextBox.SelectionStart == writeStartIndex && 
                (e.KeyCode == Keys.Back || e.KeyCode == Keys.Left) ) {
                e.Handled = false;
                e.SuppressKeyPress = true;
            }


        }

        private void consoleTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.consoleTextBox.SelectionStart < writeStartIndex)
            {
                this.consoleTextBox.SelectionStart = writeStartIndex;
            }
        }
    }
}

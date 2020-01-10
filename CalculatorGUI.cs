using System;
using System.Windows.Forms;
using System.Drawing;
using static CalculatorApp.Program;

namespace CalculatorApp
{
    public class CalculatorForm : Form
    {
        public TextBox textBox;

        public CalculatorForm()
        {
            Size = new Size(400, 600);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            InitializeTextBox();
            InitializeTableLayoutPanel();
        }

        public void InitializeTextBox()
        {
            textBox = new TextBox();
            textBox.AcceptsReturn = false;
            textBox.Dock = DockStyle.Fill;
            textBox.Multiline = false;
            textBox.Font = new Font(textBox.Font.FontFamily, 30);
            textBox.Text = "Enter An Expression";
            textBox.BackColor = Color.Wheat;

            Controls.Add(textBox);
            Text = "Calculator";
        }

        public void InitializeTableLayoutPanel()
        {
            TableLayoutPanel tableLayout = new TableLayoutPanel();
            tableLayout.Location = new Point(0, 50);
            tableLayout.Size = new Size(380, 510);
            tableLayout.TabIndex = 0;
            tableLayout.BackColor = Color.Black;
            tableLayout.ColumnCount = 4;
            tableLayout.RowCount = 6;

            Button[] numbers = InitializeButtons();
            Button[] emptyButtons = InitializeEmptyButtons();

            for (int i = 0; i < 4; i++)
            {
                tableLayout.Controls.Add(emptyButtons[i], i, 0);
                tableLayout.Controls.Add(emptyButtons[i + 4], i, 1);
                tableLayout.Controls.Add(emptyButtons[i + 8], 3, i + 2);
            }

            tableLayout.Controls.Add(emptyButtons[12], 0, 5);
            tableLayout.Controls.Add(emptyButtons[13], 2, 5);

            for (int i = 0; i < 3; i++)
            {
                tableLayout.Controls.Add(numbers[i + 1], i, 2);
                tableLayout.Controls.Add(numbers[i + 4], i, 3);
                tableLayout.Controls.Add(numbers[i + 7], i, 4);
            }
            tableLayout.Controls.Add(numbers[0], 1, 5);

            Controls.Add(tableLayout);
        }

        public Button[] InitializeButtons()
        {
            Button[] numbers = new Button[10];

            for (int i = 0; i < 10; i++)
            {
                numbers[i] = new Button();
                numbers[i].Size = new Size(91, 80);
                numbers[i].Text = Convert.ToString(i);
                numbers[i].Font = new Font(numbers[i].Font.FontFamily, 40);
                numbers[i].Click += new EventHandler(ClickButton);
                numbers[i].ForeColor = Color.Wheat;
                numbers[i].BackColor = Color.Black;
            }
            return numbers;
        }

        public Button[] InitializeEmptyButtons()
        {
            Button[] emptyButtons = new Button[20];

            for (int i = 0; i < 14; i++)
            {
                emptyButtons[i] = new Button();
                emptyButtons[i].Size = new Size(91, 80);
                emptyButtons[i].Text = "";
                emptyButtons[i].Font = new Font(emptyButtons[i].Font.FontFamily, 40);
                if (i != 13)
                    emptyButtons[i].Click += new EventHandler(ClickButton);
                emptyButtons[i].ForeColor = Color.Wheat;
                emptyButtons[i].BackColor = Color.Black;

            }
            emptyButtons[3].Text = "AC";
            emptyButtons[3].Font = new Font(emptyButtons[3].Font.FontFamily, 20);
            emptyButtons[3].Click += new EventHandler(ClickACButton);
            emptyButtons[6].Text = "(";
            emptyButtons[7].Text = ")";
            emptyButtons[8].Text = "/";
            emptyButtons[9].Text = "*";
            emptyButtons[10].Text = "+";
            emptyButtons[11].Text = "-";
            emptyButtons[12].Text = "^";
            emptyButtons[13].Text = "=";
            emptyButtons[13].Click += new EventHandler(ClickEqualsButton);
            return emptyButtons;
        }

        public void ClickButton(Object sender, EventArgs e)
        {
            textBox.Text += (sender as Button).Text;
        }

        public void ClickACButton(Object sender, EventArgs e)
        {
            textBox.Text = "";
        }

        public void ClickEqualsButton(Object sender, EventArgs e)
        {
            
            RunProgram(calculator);
        }
    }
}
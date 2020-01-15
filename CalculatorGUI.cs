using System;
using System.Windows.Forms;
using System.Drawing;
using static CalculatorApp.Program;

namespace CalculatorApp
{
    public class CalculatorForm : Form
    {
        public TextBox textBox;
        public TableLayoutPanel tableLayout;

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

            InitializeNumberButtons(tableLayout);
            InitializeOperationButtons(tableLayout);
            InitializeTrigButtons(tableLayout);
            InitializeOtherButtons(tableLayout);

            Controls.Add(tableLayout);
        }

        public Button[] InitializeNumberButtons(TableLayoutPanel tableLayout)
        {
            Button[] numberButtons = new Button[10];

            for (int i = 0; i < 10; i++)
            {
                numberButtons[i] = new Button();
                numberButtons[i].Size = new Size(91, 80);
                numberButtons[i].Text = Convert.ToString(i);
                numberButtons[i].Font = new Font(numberButtons[i].Font.FontFamily, 40);
                numberButtons[i].Click += new EventHandler(ClickButton);
                numberButtons[i].ForeColor = Color.Wheat;
                numberButtons[i].BackColor = Color.Black;
            }
            
            for (int i = 0; i < 3; i++)
            {
                tableLayout.Controls.Add(numberButtons[i + 1], i, 2);
                tableLayout.Controls.Add(numberButtons[i + 4], i, 3);
                tableLayout.Controls.Add(numberButtons[i + 7], i, 4);
            }
            tableLayout.Controls.Add(numberButtons[0], 1, 5);
            
            return numberButtons;
        }

        public Button[] InitializeOperationButtons(TableLayoutPanel tableLayout)
        {
            Button[] operationButtons = new Button[8];

            for (int i = 0; i < 8; i++)
            {
                operationButtons[i] = new Button();
                operationButtons[i].Size = new Size(91, 80);
                operationButtons[i].Text = "";
                operationButtons[i].Font = new Font(operationButtons[i].Font.FontFamily, 40);
                if (i != 7)
                    operationButtons[i].Click += new EventHandler(ClickButton);
                operationButtons[i].ForeColor = Color.Wheat;
                operationButtons[i].BackColor = Color.Black;
            }
            
            operationButtons[0].Text = "(";
            tableLayout.Controls.Add(operationButtons[0], 1, 0);
            operationButtons[1].Text = ")";
            tableLayout.Controls.Add(operationButtons[1], 2, 0);
            
            operationButtons[2].Text = "^";
            operationButtons[3].Text = "/";
            operationButtons[4].Text = "*";
            operationButtons[5].Text = "+";
            operationButtons[6].Text = "-";
            
            for (int i = 0; i < 5; i++)
                tableLayout.Controls.Add(operationButtons[i + 2], 3, i + 1); 
            
            operationButtons[7].Text = "=";
            operationButtons[7].Click += new EventHandler(ClickEqualsButton);
            tableLayout.Controls.Add(operationButtons[7], 2, 5);
            
            return operationButtons;
        }
        
        public Button[] InitializeTrigButtons(TableLayoutPanel tableLayout)
        {
            Button[] trigButtons = new Button[3];
            
            for (int i = 0; i < 3; i++)
            {
                trigButtons[i] = new Button();
                trigButtons[i].Size = new Size(91, 80);
                trigButtons[i].Text = "";
                trigButtons[i].Font = new Font(trigButtons[i].Font.FontFamily, 20);
                trigButtons[i].ForeColor = Color.Wheat;
                trigButtons[i].BackColor = Color.Black;
                trigButtons[i].Click += new EventHandler(ClickTrigButton);
            }
            
            trigButtons[0].Text = "SIN";
            trigButtons[1].Text = "COS";         
            trigButtons[2].Text = "TAN";

            for (int i = 0; i < 3; i++)
                tableLayout.Controls.Add(trigButtons[i], i, 1);
                
             return trigButtons;
        }
        
        public Button[] InitializeOtherButtons(TableLayoutPanel tableLayout) 
        {
            Button[] otherButtons = new Button[3];
            
            for (int i = 0; i < 3; i++)
            {
                otherButtons[i] = new Button();
                otherButtons[i].Size = new Size(91, 80);
                otherButtons[i].Text = "";
                otherButtons[i].Font = new Font(otherButtons[i].Font.FontFamily, 20);
                otherButtons[i].ForeColor = Color.Wheat;
                otherButtons[i].BackColor = Color.Black;
            }
            otherButtons[0].Text = "AC";
            otherButtons[0].Font = new Font(otherButtons[0].Font.FontFamily, 20);
            otherButtons[0].Click += new EventHandler(ClickClearButton);
            
            otherButtons[1].Text = "DEL";
            otherButtons[1].Font = new Font(otherButtons[1].Font.FontFamily, 20);
            otherButtons[1].Click += new EventHandler(ClickDeleteButton);
            
            otherButtons[2].Text = "INV";
            otherButtons[2].Font = new Font(otherButtons[1].Font.FontFamily, 20);
            otherButtons[2].Click += new EventHandler(ClickInverseButton);

            tableLayout.Controls.Add(otherButtons[0], 3, 0);
            tableLayout.Controls.Add(otherButtons[1], 0, 5);
            tableLayout.Controls.Add(otherButtons[2], 0, 0);
            
            return otherButtons;
        }

        public void ClickButton(Object sender, EventArgs e)
        {
            if (textBox.Text == "Enter An Expression")
                textBox.Text = "";
            textBox.Text += (sender as Button).Text;
        }
        
        public void ClickTrigButton(Object sender, EventArgs e)
        {
            if (textBox.Text == "Enter An Expression")
                textBox.Text = "";
            textBox.Text = (sender as Button).Text;
            textBox.Text += "(";
        }

        public void ClickEqualsButton(Object sender, EventArgs e)
        {
            Run(calculator);
        }

        public void ClickClearButton(Object sender, EventArgs e)
        {
            textBox.Text = "";
        }
        
        public void ClickDeleteButton(Object sender, EventArgs e)
        {
            //Add delete functionality
        }
        
        public void ClickInverseButton(Object sender, EventArgs e)
        {
            //Add inverse functionality
        }
    }
}

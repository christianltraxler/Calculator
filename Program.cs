using System;
using System.Windows.Forms;

namespace CalculatorApp
{
    //Fix bracket functionality
    //Add more buttons/operations (sin, cos, tan,
    //Add decimal number functionality

    public class Program
    {
        public static CalculatorForm calculator = new CalculatorForm();

        public static void Main()
        {
            Application.Run(calculator);
        }

        public static void RunProgram(CalculatorForm calculator)
        {
            string expression = "";
            expression = calculator.textBox.Text;

            bool solvable = CheckSolvable(expression);
            if (solvable == true)
            {

                LinkedList expressionList = new LinkedList();
                for (int i = 0; i < expression.Length; i++)
                    expressionList.AddLast(expression[i].ToString());

                string number = "";
                int counter = 0, startPosition = 0, endPosition = 0, digits = 0;

                Node current = expressionList.head;

                while (current != null)
                {
                    if ((current.data.ToString().ToCharArray()[0] > 47) && (current.data.ToString().ToCharArray()[0] < 59))
                    {
                        if (number == "")
                            startPosition = counter;
                        number += current.data.ToString();
                        digits++;
                    }
                    else if (number != "")
                    {
                        endPosition = counter - 1;
                        if (digits == 1)
                            expressionList.Replace(Convert.ToDouble(number), startPosition, startPosition);
                        else
                            expressionList.Replace(Convert.ToDouble(number), startPosition, endPosition);
                        number = "";
                        counter = counter - digits + 1;
                        digits = 0;
                    }
                    counter++;
                    current = current.next;
                }

                if (digits != 0)
                {
                    endPosition = counter - 1;
                    expressionList.Replace(Convert.ToDouble(number), startPosition, endPosition);
                }

                Brackets(expressionList);

                Search("^", expressionList);
                Search("/*x", expressionList);
                Search("+-", expressionList);

                calculator.textBox.Text = expressionList.Print();
            }
            else
                calculator.textBox.Text = "ERROR";

        }

        static bool CheckSolvable(string expression)
        {
            int numForward = 0, numBackward = 0;
            bool previousForward = false, previousOperation = false;
            string unmatched = "";

            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '+' || expression[i] == '-'
                       || expression[i] == '*' || expression[i] == '/'
                       || expression[i] == '^')
                {
                    if (i == 0 || i == expression.Length - 1)
                        return false;
                    if (previousForward == true || previousOperation == true)
                        return false;
                }
                else if (expression[i] == ')')
                {
                    if (previousOperation == true)
                        return false;

                    numBackward++;
                    if (unmatched.Length == 0)
                        return false;
                    else if (unmatched[unmatched.Length - 1] == '(')
                    {
                        string temp = "";
                        for (int n = 0; n < unmatched.Length - 1 && unmatched.Length != 1; n++)
                        {
                            temp += unmatched[n];
                        }
                        unmatched = temp;
                    }

                    previousForward = false;
                }
                else if (expression[i] == '(')
                {
                    numForward++;
                    previousForward = true;
                    previousOperation = false;
                    unmatched += '(';
                }
                else if (expression[i] > 47 && expression[i] < 59)
                {
                    previousForward = false;
                    previousOperation = false;
                }
                else
                    return false;
            }

            if (unmatched != "" || numForward != numBackward)
                return false;





            return true;
        }

        static double Operation(double x, char op, double y)
        {
            switch (op)
            {
                case '+': return x + y;
                case '-': return x - y;
                case '*': return x * y;
                case 'x': return x * y;
                case '/': return x / y;
                case '^': return Math.Pow(x, y);
            }
            return 0;
        }

        static void Brackets(LinkedList expressionList)
        {
            int numBrackets = 0;
            Node current = expressionList.head;
            while (current != null)
            {
                if (current.data.ToString() == "(")
                    numBrackets++;
                current = current.next;
            }
            if (numBrackets != 0)
            {
                current = expressionList.head;
                Node last = expressionList.head;
                while (last.next != null)
                {
                    last = last.next;
                }
                if (current.data.ToString() == "(" && last.data.ToString() == ")")
                {
                    expressionList.Delete(0);
                    expressionList.Delete(expressionList.Length() - 2);
                }

                for (int i = 0; i < numBrackets; i++)
                {
                    current = expressionList.head;
                    int counter = 0, forwardPosition = 0, backwardPosition = 0;
                    while (current != null)
                    {
                        if (current.data.ToString() == "(")
                        {
                            forwardPosition = counter;
                        }
                        else if (current.data.ToString() == ")")
                        {
                            backwardPosition = counter;
                        }
                        counter++;
                        current = current.next;
                    }

                    Search("^", expressionList, forwardPosition + 1, backwardPosition - 1);
                    Search("/*x", expressionList, forwardPosition + 1, backwardPosition - 1);
                    Search("+-", expressionList, forwardPosition + 1, backwardPosition - 1);
                    expressionList.Delete(forwardPosition);
                    expressionList.Delete(forwardPosition + 2);
                }
            }
        }

        static void Search(string op, LinkedList expressionList)
        {
            Node current = expressionList.head.next;
            int numOperations = 0;

            while (current != null)
            {
                for (int i = 0; i < op.Length; i++)
                {
                    if (Convert.ToString(current.data) == Convert.ToString(op[i]))
                        numOperations++;
                }
                current = current.next;
            }
            for (int i = 0; i < numOperations; i++)
                Solve(op, expressionList);
        }

        static void Search(string op, LinkedList expressionList, int startPosition, int endPosition)
        {
            Node current = expressionList.head.next;
            int numOperations = 0;
            int counter = 0;

            while (counter < startPosition)
            {
                counter++;
                current = current.next;
            }

            Node first = expressionList.head;
            Node last = expressionList.head;
            while (last.next == null)
            {
                last = last.next;
            }

            if (first.data.ToString() == "(" && last.data.ToString() == ")")
            {
                expressionList.Delete(0);
                expressionList.Delete(expressionList.Length() - 2);
            }

            while (current != null && counter < endPosition)
            {
                for (int i = 0; i < op.Length; i++)
                {
                    if (Convert.ToString(current.data) == Convert.ToString(op[i]))
                        numOperations++;
                }
                current = current.next;
                counter++;
            }
            for (int i = 0; i < numOperations; i++)
            {
                Solve(op, expressionList, startPosition, endPosition);

            }
        }

        static void Solve(string op, LinkedList expressionList)
        {
            Node previous = expressionList.head;
            int counter = 0;
            Node current = expressionList.head.next;
            bool done = false;
            while (current != null && done == false)
            {
                for (int i = 0; i < op.Length; i++)
                {
                    if (Convert.ToString(current.data) == Convert.ToString(op[i]))
                    {
                        //Console.WriteLine($"{previous.data} {current.next.data}");
                        double number;
                        number = Operation(Convert.ToDouble(previous.data), op[i], Convert.ToDouble(current.next.data));
                        expressionList.Replace(number, counter, counter + 2);
                        counter = counter + number.ToString().Length - 3;
                        done = true;
                    }
                }
                previous = previous.next;
                current = current.next;
                counter++;
            }
        }

        static void Solve(string op, LinkedList expressionList, int startPosition, int endPosition)
        {
            Node previous = expressionList.head;
            int counter = 0;
            Node current = expressionList.head.next;
            bool done = false;

            while (current != null && counter < startPosition)
            {
                previous = previous.next;
                current = current.next;
                counter++;
            }

            while (current != null && done == true)
            {
                for (int i = 0; i < op.Length; i++)
                {
                    if (Convert.ToString(current.data) == Convert.ToString(op[i]))
                    {
                        //Console.WriteLine($"{previous.data} {current.next.data}");
                        double number;
                        number = Operation(Convert.ToDouble(previous.data), op[i], Convert.ToDouble(current.next.data));
                        expressionList.Replace(number, counter, counter + 2);
                        counter = counter + number.ToString().Length - 3;
                        done = true;
                    }
                }
                previous = previous.next;
                current = current.next;
                counter++;
            }
        }
    }
}
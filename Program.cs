using System;
using System.Windows.Forms;

namespace CalculatorApp
{
    //Fix bracket functionality 
    //Add trig function functionality
    //Add inverse functionality (trig/sqrt)
    //Add decimal number functionality
    //Add recent answer functionality

    public class Program
    {
        public static CalculatorForm calculator = new CalculatorForm();

        public static void Main()
        { 
            Application.Run(calculator);
        }

        public static void Run(CalculatorForm calculator)
        {
            // Get the expression from the textbox
            string expression = calculator.textBox.Text;

            // Make sure that the expression is solvable
            bool solvable = CheckSolvable(expression);

            // Solve the expression if it is solvable
            if (solvable == true)
            {
                // Create a lLinkedList instance for the expression
                LinkedList expressionList = new LinkedList();

                // Add each character to the LinkedList
                for (int i = 0; i < expression.Length; i++)
                    expressionList.AddLast(expression[i].ToString());
                
                // Initialize variables
                string number = "";
                int counter = 0, startPosition = 0, endPosition = 0, digits = 0;

                // Get the LinkedList head
                Node current = expressionList.head;

                // Iterate through the LinkedList to insert numbers
                while (current != null)
                {
                    // If it is a number
                    if ((current.data.ToString().ToCharArray()[0] > 47) && (current.data.ToString().ToCharArray()[0] < 59))
                    {
                        // If a number has not been started
                        if (number == "")
                            startPosition = counter;

                        // Add the data to number
                        number += current.data.ToString();

                        // Increment the digits
                        digits++;
                    }
                    // If it is not a number, must be operation
                    else if (number != "")
                    {
                        // The end of the number should be the previous node
                        endPosition = counter - 1;

                        // Replace the string with a number
                        if (digits == 1)
                            expressionList.Replace(Convert.ToDouble(number), startPosition, startPosition);
                        else
                            expressionList.Replace(Convert.ToDouble(number), startPosition, endPosition);

                        // Reset number, digits, counter
                        number = "";
                        counter = counter - digits + 1;
                        digits = 0;
                    }
                    // Increment counter, LinkedList node
                    counter++;
                    current = current.next;
                }

                // Convert any remaining digits
                if (digits != 0)
                {
                    // 
                    endPosition = counter - 1;
                    expressionList.Replace(Convert.ToDouble(number), startPosition, endPosition);
                }

                // Solve expressions within brackets
                SolveBrackets(expressionList);

                // Solve each type of operation in order of BEDMAS
                SolveExpression(expressionList);

                //Output the result to the textbox
                calculator.textBox.Text = expressionList.Text();
            }
            else
            {
                // Expression is not solvable
                calculator.textBox.Text = "ERROR";
                Console.WriteLine("Expression Not Solvable");
            }
            Console.WriteLine(expression + " = " + calculator.textBox.Text);
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

        static double SolveOperation(double x, char op, double y)
        {
            // Solve operation
            switch (op)
            {
                case '+': return x + y;
                case '-': return x - y;
                case '*': return x * y;
                case 'x': return x * y;
                case '/': return x / y;
                case '^': return Math.Pow(x, y);
                default: throw new Exception("Operation Not Found");
            }
        }

        static void SolveBrackets(LinkedList expressionList)
        {
            // Initialize variables
            int numBrackets = 0, forwardPosition = 0, backwardPosition = 0;
            bool done = false;

            // Get LinkedList head
            Node current = expressionList.head;

            // Iterate through LinkedList for brackets
            while (current != null)
            {
                // Find the number of pairs of brackets
                if (current.data.ToString() == "(")
                    numBrackets++;

                // Increment LinkedList node
                current = current.next;
            }

            // If there are brackets
            if (numBrackets != 0)
            {
                // Check for surrounding brackets, remove them if necessary
                bool removed = DeleteSurroundingBrackets(expressionList);

                // For each pair for brackets, remove the bracket
                for (int i = Convert.ToInt32(removed); i < numBrackets; i++)
                {
                    // Initialize variables
                    current = expressionList.head;
                    int position = 0;

                    // Iterate through LinkedList, find last/innermost pair of brackets
                    while (current != null && done == false)
                    {
                        // If the current node is the forward bracket
                        if (current.data.ToString() == "(")
                        {
                            // Set the forwardPosition to be the position of the current node
                            forwardPosition = position;
                        }
                        // If the current node is the backwards bracket
                        else if (current.data.ToString() == ")")
                        {
                            // Set the backwardPosition to be the position of the current node
                            backwardPosition = position;
                            Console.WriteLine(forwardPosition + " " + backwardPosition);
                            // Exit loop once bracket positions have been determined
                            done = true;
                        }
                            // Increment the counters for position/LinkedList node
                            position++;
                            current = current.next;
                    }

                    // Once brackets are found, solve expression
                    // Delete the brackets
                    expressionList.Delete(backwardPosition);
                    expressionList.Delete(forwardPosition);

                    // Solve expression inside the brackets
                    SolvePartialExpression(expressionList, forwardPosition, backwardPosition - 2);
                }
            }
        }

        static bool DeleteSurroundingBrackets(LinkedList expressionList)
        {
            // Intialize node variables
            Node current = expressionList.head;
            Node last = expressionList.head;
            bool removed = false;

            // Find last LinkedList node
            while (last.next != null)
            {
                last = last.next;
            }

            // If the brackets
            if (current.data.ToString() == "(" && last.data.ToString() == ")")
            {
                // Delete the surrounding brackets
                expressionList.Delete(0);
                expressionList.Delete(expressionList.Length() - 1);

                // Set removed to true
                removed = true;
            }
            
            // Return removed
            return(removed);

        }

        static void SolveExpression(LinkedList expressionList)
        {
            // Intialize variables
            string op = "^*x/+-";
            bool done = false;

            for (int i = 0; i < op.Length && done == false; i++)
            {
                Node current = expressionList.head.next;
                Node previous = expressionList.head;
                int position = 1;

                // Cycle through the exoressionList
                while (current != null && done == false)
                {
                    if (Convert.ToString(current.data) == Convert.ToString(op[i]))
                    {
                        // Solve the expression based on the operation found
                        double number = SolveOperation(Convert.ToDouble(previous.data), op[i], Convert.ToDouble(current.next.data));
                        expressionList.Replace(number, position - 1, position + 1);
                        position--;

                        // Recursively try to solve the expression 
                        SolveExpression(expressionList);

                        // Stop cycling
                        done = true;
                    }
                    // Increment cycle variables
                    current = current.next;
                    previous = previous.next;
                    position++;
                }
            }


            
        }

        static void SolvePartialExpression(LinkedList expressionList, int startPosition, int endPosition)
        {
            Node startNode = expressionList.head.next;
            Node beforeStartNode = expressionList.head;
            int position = 1;
            string op = "^*x/+-";
            bool done = false;

            while (startNode != null && position < startPosition)
            {
                startNode = startNode.next;
                beforeStartNode = beforeStartNode.next;
                position++;
            }

            if (position > startPosition)
                startPosition = position;

            for (int i = 0; i < op.Length && done == false; i++)
            {
                Node current = startNode, previous = beforeStartNode;
                position = startPosition;

                while (current != null && position < endPosition)
                {
                    if (Convert.ToString(current.data) == Convert.ToString(op[i]))
                    {
                        double number = SolveOperation(Convert.ToDouble(previous.data), op[i], Convert.ToDouble(current.next.data));
                        expressionList.Replace(number, position - 1, position + 1);
                        position--;

                        // Recursively try to solve the expression 
                        SolvePartialExpression(expressionList, startPosition, endPosition - 2);

                        done = true;
                    }
                    current = current.next;
                    previous = previous.next;
                    position++;
                }
            }
        }
    }
}

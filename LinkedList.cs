using System;

namespace CalculatorApp
{
    public class LinkedList
    {
        public Node head;

        public string Print()
        {
            Node current = head;
            string print = "";
            while (current != null)
            {
                print += current.data;
                print += " ";
                current = current.next;
            }
            Console.WriteLine(print);
            return print;
        }

        public int Length()
        {
            Node current = head;
            int counter = 0;
            while (current != null)
            {
                current = current.next;
                counter++;
            }
            counter++;
            return counter;
        }

        public void AddFirst(Object data)
        {
            Node toAdd = new Node { data = data };
            toAdd.next = head;
            head = toAdd;
        }

        public void AddLast(Object data)
        {
            if (head == null)
            {
                head = new Node { data = data };
                head.data = data;
            }
            else
            {
                Node toAdd = new Node { data = data };
                toAdd.next = null;
                Node current = head;
                while (current.next != null)
                {
                    current = current.next;
                }
                current.next = toAdd;
            }
        }

        public void Insert(Object data, int position)
        {
            Node toAdd = new Node { data = data };

            if (position == 0)
                AddFirst(data);
            else if (position == Length() - 1)
                AddLast(data);
            else
            {
                int counter = 0;
                Node current = head;

                while (counter < position - 1)
                {
                    current = current.next;
                    counter++;
                }               
                    
                if (current.next != null)
                    toAdd.next = current.next;
                current.next = toAdd;
            }
        }

        public void Delete(int position)
        {
            Node current = head;
            if (position == 0)
            {
                head = current.next;
            }
            else if (position == Length() - 1)
            {
                while (current.next.next != null)
                    current = current.next;
                current.next = null;
            }
            else
            {
                int counter = 0;
                while (current.next.next != null && counter < position - 1)
                {
                    current = current.next;
                    counter++;
                }
                current.next = current.next.next;
            }
        }

        public void Replace(Object data, int startPosition, int endPosition)
        {
            int count = 0;
            while (count <= endPosition - startPosition)
            {
                Delete(startPosition);
                count++;
            }
            Insert(data, startPosition);
            Console.WriteLine(Print());
        }
    }

    public class Node
    {
        public Node next;
        public Object data;
    }
}

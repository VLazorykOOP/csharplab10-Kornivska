using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1. Префіксний у постфіксний (Stack)");
            Console.WriteLine("2. Співробітники (Queue)");
            Console.WriteLine("3. Завдання 1 та 2 з ArrayList, IEnumerable, IComparable, ICloneable");
            Console.WriteLine("4. Каталог музичних дисків (Hashtable)");
            Console.WriteLine("5. Вихід");
            Console.Write("Оберіть завдання: ");

            switch (Console.ReadLine())
            {
                case "1": Task1(); break;
                case "2": Task2(); break;
                case "3": Task3(); break;
                case "4": Task4(); break;
                case "5": return;
                default: Console.WriteLine("Невірний вибір. Спробуйте ще раз."); break;
            }
        }
    }

    static void Task1()
    {
        Console.Write("Введіть префіксний вираз: ");
        string[] tokens = Console.ReadLine()?.Split() ?? new string[0];
        Stack<string> stack = new();

        for (int i = tokens.Length - 1; i >= 0; i--)
        {
            if (IsOperator(tokens[i]))
            {
                string op1 = stack.Pop();
                string op2 = stack.Pop();
                stack.Push(op1 + " " + op2 + " " + tokens[i]);
            }
            else
            {
                stack.Push(tokens[i]);
            }
        }

        Console.WriteLine("Постфіксний вираз: " + stack.Peek());
    }

    static bool IsOperator(string s) => s is "+" or "-" or "*" or "/";

    static void Task2()
    {
        Queue<string> men = new();
        Queue<string> women = new();

        foreach (var line in File.ReadAllLines("employees.txt"))
        {
            if (line.ToLower().Contains("чоловік"))
                men.Enqueue(line);
            else
                women.Enqueue(line);
        }

        Console.WriteLine("\nЧоловіки:");
        foreach (var m in men) Console.WriteLine(m);

        Console.WriteLine("\nЖінки:");
        foreach (var w in women) Console.WriteLine(w);
    }

    static void Task3()
    {
        ArrayList employees = new();
        foreach (var line in File.ReadAllLines("employees.txt"))
        {
            employees.Add(new Employee(line));
        }

        employees.Sort();

        Console.WriteLine("\nВідсортовані співробітники:");
        foreach (Employee e in employees)
        {
            Console.WriteLine(e);
        }
    }

    static void Task4()
    {
        Hashtable catalog = new();

        while (true)
        {
            Console.WriteLine("\nКаталог дисків:");
            Console.WriteLine("1. Додати диск");
            Console.WriteLine("2. Додати пісню до диска");
            Console.WriteLine("3. Видалити диск");
            Console.WriteLine("4. Видалити пісню з диска");
            Console.WriteLine("5. Переглянути диск");
            Console.WriteLine("6. Пошук за виконавцем");
            Console.WriteLine("7. Назад до меню");
            Console.Write("Ваш вибір: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Назва диска: ");
                    string disk = Console.ReadLine()!;
                    if (!catalog.Contains(disk)) catalog[disk] = new List<Song>();
                    break;
                case "2":
                    Console.Write("Назва диска: ");
                    disk = Console.ReadLine()!;
                    if (catalog.Contains(disk))
                    {
                        Console.Write("Назва пісні: ");
                        string title = Console.ReadLine()!;
                        Console.Write("Виконавець: ");
                        string artist = Console.ReadLine()!;
                        ((List<Song>)catalog[disk]).Add(new Song(title, artist));
                    }
                    break;
                case "3":
                    Console.Write("Назва диска для видалення: ");
                    catalog.Remove(Console.ReadLine()!);
                    break;
                case "4":
                    Console.Write("Назва диска: ");
                    disk = Console.ReadLine()!;
                    if (catalog.Contains(disk))
                    {
                        Console.Write("Назва пісні: ");
                        string title = Console.ReadLine()!;
                        ((List<Song>)catalog[disk]).RemoveAll(s => s.Title == title);
                    }
                    break;
                case "5":
                    foreach (DictionaryEntry entry in catalog)
                    {
                        Console.WriteLine($"\nДиск: {entry.Key}");
                        foreach (Song s in (List<Song>)entry.Value)
                        {
                            Console.WriteLine(s);
                        }
                    }
                    break;
                case "6":
                    Console.Write("Виконавець: ");
                    string search = Console.ReadLine()!;
                    foreach (DictionaryEntry entry in catalog)
                    {
                        foreach (Song s in (List<Song>)entry.Value)
                        {
                            if (s.Artist.Contains(search)) Console.WriteLine(s);
                        }
                    }
                    break;
                case "7": return;
                default: Console.WriteLine("Невірний вибір"); break;
            }
        }
    }
}

class Employee : IComparable, ICloneable
{
    public string Line { get; }
    public string Surname { get; }

    public Employee(string line)
    {
        Line = line;
        Surname = line.Split(' ')[0];
    }

    public int CompareTo(object? obj)
    {
        return Surname.CompareTo(((Employee)obj!).Surname);
    }

    public object Clone() => new Employee(Line);

    public override string ToString() => Line;
}

class Song
{
    public string Title { get; }
    public string Artist { get; }
    public Song(string title, string artist) { Title = title; Artist = artist; }
    public override string ToString() => $"{Artist} - {Title}";
}

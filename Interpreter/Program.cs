/*Основная идея паттерна интерпретатор заключается в том, чтобы представить простой язык или грамматику в виде набора классов, каждый из которых представляет один элемент грамматики (называемый терминалом или нетерминалом). Затем составляется древовидная структура из этих классов для представления структуры предложений в языке. Каждый узел дерева может интерпретировать свою часть грамматики.*/
using System;

// Интерфейс выражения
interface IExpression
{
    bool Interpret(string context);
}

// Терминальное выражение (проверка на палиндром)
class PalindromeExpression : IExpression
{
    public bool Interpret(string context)
    {
        // Удаляем пробелы из строки и приводим к нижнему регистру
        string processedString = context.Replace(" ", "").ToLower();

        // Проверяем, является ли строка палиндромом
        for (int i = 0; i < processedString.Length / 2; i++)
        {
            if (processedString[i] != processedString[processedString.Length - 1 - i])
            {
                return false;
            }
        }
        return true;
    }
}

// Терминальное выражение (проверка наличия подстроки)
class SubstringExpression : IExpression
{
    private readonly string _substring;

    public SubstringExpression(string substring)
    {
        _substring = substring;
    }

    public bool Interpret(string context)
    {
        return context.Contains(_substring);
    }
}

// Нетерминальное выражение (комбинация палиндрома и поиска подстроки)
class AndExpression : IExpression
{
    private readonly IExpression _expression1;
    private readonly IExpression _expression2;

    public AndExpression(IExpression expression1, IExpression expression2)
    {
        _expression1 = expression1;
        _expression2 = expression2;
    }

    public bool Interpret(string context)
    {
        return _expression1.Interpret(context) && _expression2.Interpret(context);
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Создание терминальных выражений
        IExpression palindromeExpression = new PalindromeExpression();
        IExpression substringExpression = new SubstringExpression("hello");

        // Создание нетерминального выражения (логическое И)
        IExpression expression = new AndExpression(palindromeExpression, substringExpression);

        // Проверка строки на палиндром и наличие подстроки
        string input1 = "level";
        string input2 = "hello";
        string input3 = "level hello";

        Console.WriteLine($"{input1} is a palindrome and contains 'hello': {expression.Interpret(input1)}"); // Выведет: False
        Console.WriteLine($"{input2} is a palindrome and contains 'hello': {expression.Interpret(input2)}"); // Выведет: False
        Console.WriteLine($"{input3} is a palindrome and contains 'hello': {expression.Interpret(input3)}"); // Выведет: False
    }
}
using System;
using System.Collections.Generic;

// Интерфейс выражения для условий сотрудников
interface IEmployeeExpression
{
    bool Interpret(Employee employee);
}

// Класс сотрудника
class Employee
{
    public string Name { get; }
    public string Position { get; }
    public double Salary { get; }
    public int YearsOfExperience { get; }

    public Employee(string name, string position, double salary, int yearsOfExperience)
    {
        Name = name;
        Position = position;
        Salary = salary;
        YearsOfExperience = yearsOfExperience;
    }
}

// Терминальное выражение (проверка по должности)
class PositionExpression : IEmployeeExpression
{
    private readonly string _requiredPosition;

    public PositionExpression(string requiredPosition)
    {
        _requiredPosition = requiredPosition;
    }

    public bool Interpret(Employee employee)
    {
        return employee.Position.Equals(_requiredPosition, StringComparison.OrdinalIgnoreCase);
    }
}

// Терминальное выражение (проверка по стажу работы)
class ExperienceExpression : IEmployeeExpression
{
    private readonly int _requiredYears;

    public ExperienceExpression(int requiredYears)
    {
        _requiredYears = requiredYears;
    }

    public bool Interpret(Employee employee)
    {
        return employee.YearsOfExperience >= _requiredYears;
    }
}

// Терминальное выражение (проверка по зарплате)
class SalaryExpression : IEmployeeExpression
{
    private readonly double _requiredSalary;

    public SalaryExpression(double requiredSalary)
    {
        _requiredSalary = requiredSalary;
    }

    public bool Interpret(Employee employee)
    {
        return employee.Salary >= _requiredSalary;
    }
}

// Нетерминальное выражение (логическое И)
class AndEmployeeExpression : IEmployeeExpression
{
    private readonly IEmployeeExpression _expression1;
    private readonly IEmployeeExpression _expression2;

    public AndEmployeeExpression(IEmployeeExpression expression1, IEmployeeExpression expression2)
    {
        _expression1 = expression1;
        _expression2 = expression2;
    }

    public bool Interpret(Employee employee)
    {
        return _expression1.Interpret(employee) && _expression2.Interpret(employee);
    }
}

// Нетерминальное выражение (логическое ИЛИ)
class OrEmployeeExpression : IEmployeeExpression
{
    private readonly IEmployeeExpression _expression1;
    private readonly IEmployeeExpression _expression2;

    public OrEmployeeExpression(IEmployeeExpression expression1, IEmployeeExpression expression2)
    {
        _expression1 = expression1;
        _expression2 = expression2;
    }

    public bool Interpret(Employee employee)
    {
        return _expression1.Interpret(employee) || _expression2.Interpret(employee);
    }
}


// Программа
class Program
{
    static void Main(string[] args)
    {
        // Создание терминальных выражений
        IEmployeeExpression positionExpression = new PositionExpression("Manager");
        IEmployeeExpression experienceExpression = new ExperienceExpression(5);
        IEmployeeExpression salaryExpression = new SalaryExpression(5000);

        // Создание нетерминального выражения (логическое И между стажем и зарплатой)
        IEmployeeExpression seniorManagerExpression = new AndEmployeeExpression(experienceExpression, positionExpression);

        // Создание нетерминального выражения (логическое ИЛИ между стажем и зарплатой)
        IEmployeeExpression seniorOrWellPaidExpression = new OrEmployeeExpression(experienceExpression, salaryExpression);

        // Создание нескольких сотрудников
        var employee1 = new Employee("Alice", "Manager", 6000, 10);
        var employee2 = new Employee("Bob", "Developer", 4500, 7);
        var employee3 = new Employee("Charlie", "Manager", 4000, 3);

        // Проверка, является ли сотрудник старшим менеджером
        Console.WriteLine($"{employee1.Name} is a Senior Manager: {seniorManagerExpression.Interpret(employee1)}"); // Выведет: True
        Console.WriteLine($"{employee2.Name} is a Senior Manager: {seniorManagerExpression.Interpret(employee2)}"); // Выведет: False
        Console.WriteLine($"{employee3.Name} is a Senior Manager: {seniorManagerExpression.Interpret(employee3)}"); // Выведет: False

        // Проверка, является ли сотрудник старшим или хорошо оплачиваемым
        Console.WriteLine($"{employee1.Name} is a Senior or Well Paid: {seniorOrWellPaidExpression.Interpret(employee1)}"); // Выведет: True
        Console.WriteLine($"{employee2.Name} is a Senior or Well Paid: {seniorOrWellPaidExpression.Interpret(employee2)}"); // Выведет: True
        Console.WriteLine($"{employee3.Name} is a Senior or Well Paid: {seniorOrWellPaidExpression.Interpret(employee3)}"); // Выведет: False
    }
}

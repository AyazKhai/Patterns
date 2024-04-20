using System;

// Базовый интерфейс (Component)
interface Employee
{
    string GetDetails();
    double GetSalary();
}

// Конкретный компонент (ConcreteComponent)
class BasicEmployee : Employee
{
    public string GetDetails()
    {
        return "Basic Employee";
    }

    public double GetSalary()
    {
        return 3000.0; // базовая зарплата
    }
}

// Декоратор (Decorator)
abstract class BonusDecorator : Employee
{
    protected Employee employee;

    public BonusDecorator(Employee employee)
    {
        this.employee = employee;
    }

    public abstract string GetDetails();
    public abstract double GetSalary();
}

// Конкретный декоратор (ConcreteDecorator) — Медицинская страховка
class HealthInsurance : BonusDecorator
{
    public HealthInsurance(Employee employee) : base(employee)
    {
    }

    public override string GetDetails()
    {
        return employee.GetDetails() + ", Health Insurance";
    }

    public override double GetSalary()
    {
        return employee.GetSalary() + 200.0; // дополнительная стоимость страховки
    }
}

// Конкретный декоратор (ConcreteDecorator) — Премия
class Bonus : BonusDecorator
{
    public Bonus(Employee employee) : base(employee)
    {
    }

    public override string GetDetails()
    {
        return employee.GetDetails() + ", Bonus";
    }

    public override double GetSalary()
    {
        return employee.GetSalary() + 500.0; // стоимость премии
    }
}

// Клиентский код
class Client
{
    static void Main(string[] args)
    {
        // Создаем базового сотрудника
        Employee employee = new BasicEmployee();
        Console.WriteLine( employee.GetSalary());
        // Добавляем медицинскую страховку
        employee = new HealthInsurance(employee);

        // Добавляем премию
        employee = new Bonus(employee);

        // Получаем детали и зарплату
        Console.WriteLine($"Employee Details: {employee.GetDetails()}");
        Console.WriteLine($"Total Salary: ${employee.GetSalary()}");
    }
}

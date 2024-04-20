using System;
using System.Collections.Generic;

// Интерфейс для наблюдателя
interface IEmployeeObserver
{
    void Update(string employeeName, string action, string details);
}

// Конкретный наблюдатель (сотрудник)
class EmployeeObserver : IEmployeeObserver
{
    private readonly string _name;

    public EmployeeObserver(string name)
    {
        _name = name;
    }

    public void Update(string employeeName, string action, string details)
    {
        Console.WriteLine($"Уведомление для {_name}: Сотрудник {employeeName} был {action}. {details}");
    }
}

// Класс, который представляет отдел кадров
class HumanResourcesDepartment
{
    private readonly List<IEmployeeObserver> _observers = new List<IEmployeeObserver>();

    public void Attach(IEmployeeObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IEmployeeObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify(string employeeName, string action, string details)
    {
        foreach (var observer in _observers)
        {
            observer.Update(employeeName, action, details);
        }
    }

    public void HireEmployee(string employeeName)
    {
        Notify(employeeName, "принят на работу", "Добро пожаловать в команду!");
    }

    public void PromoteEmployee(string employeeName, string newPosition)
    {
        Notify(employeeName, "повышен", $"Теперь должность: {newPosition}");
    }

    public void TerminateEmployee(string employeeName)
    {
        Notify(employeeName, "уволен", "Желаем удачи в будущем.");
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Создаем отдел кадров
        var hrDepartment = new HumanResourcesDepartment();

        // Создаем наблюдателей
        var observer1 = new EmployeeObserver("Менеджер");
        var observer2 = new EmployeeObserver("Директор");

        // Подписываем наблюдателей на отдел кадров
        hrDepartment.Attach(observer1);
        hrDepartment.Attach(observer2);

        // Принять сотрудника на работу
        hrDepartment.HireEmployee("Александр");

        // Повысить сотрудника
        hrDepartment.PromoteEmployee("Александр", "Менеджер по продажам");

        // Уволить сотрудника
        hrDepartment.TerminateEmployee("Александр");
    }
}

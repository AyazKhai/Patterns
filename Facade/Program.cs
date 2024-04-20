using System;

// Подсистема управления информацией о сотрудниках
class EmployeeInfoSubsystem
{
    public void GetEmployeeDetails(int employeeId)
    {
        Console.WriteLine($"Получение информации о сотруднике с ID {employeeId}.");
    }
}

// Подсистема управления рабочим временем
class TimeTrackingSubsystem
{
    public void TrackWorkHours(int employeeId, int hours)
    {
        Console.WriteLine($"Отслеживание {hours} часов для сотрудника с ID {employeeId}.");
    }
}

// Подсистема управления отпусками
class LeaveManagementSubsystem
{
    public void ApplyForLeave(int employeeId, int days)
    {
        Console.WriteLine($"Заявка на {days} дней отпуска для сотрудника с ID {employeeId}.");
    }
}

// Подсистема управления зарплатой
class PayrollSubsystem
{
    public void ProcessPayroll(int employeeId)
    {
        Console.WriteLine($"Начисление зарплаты для сотрудника с ID {employeeId}.");
    }
}

// Фасад для взаимодействия с разными подсистемами отдела кадров
class HumanResourcesFacade
{
    private readonly EmployeeInfoSubsystem _employeeInfo;
    private readonly TimeTrackingSubsystem _timeTracking;
    private readonly LeaveManagementSubsystem _leaveManagement;
    private readonly PayrollSubsystem _payroll;

    public HumanResourcesFacade()
    {
        _employeeInfo = new EmployeeInfoSubsystem();
        _timeTracking = new TimeTrackingSubsystem();
        _leaveManagement = new LeaveManagementSubsystem();
        _payroll = new PayrollSubsystem();
    }

    public void ManageEmployee(int employeeId, int hoursWorked, int leaveDays)
    {
        // Получаем информацию о сотруднике
        _employeeInfo.GetEmployeeDetails(employeeId);

        // Отслеживаем рабочие часы
        _timeTracking.TrackWorkHours(employeeId, hoursWorked);

        // Обрабатываем заявку на отпуск
        _leaveManagement.ApplyForLeave(employeeId, leaveDays);

        // Обрабатываем зарплату
        _payroll.ProcessPayroll(employeeId);
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Клиентский код взаимодействует с фасадом
        HumanResourcesFacade hrFacade = new HumanResourcesFacade();

        // Управление сотрудником с ID 101
        hrFacade.ManageEmployee(101, 40, 5);
    }
}

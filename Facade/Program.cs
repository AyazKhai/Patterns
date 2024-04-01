/*Предположим, у нас есть веб-приложение, которое взаимодействует с базой данных для получения информации о пользователях и их заказах, а также отправляет электронные письма для подтверждения заказов. Внутренняя структура системы может быть сложной, с различными классами и методами для работы с базой данных и отправки электронных писем.

В этом случае мы можем использовать паттерн фасад для создания простого интерфейса, который скрывает сложность работы с базой данных и отправкой писем.*/

using System;

// Подсистема работы с базой данных
class DatabaseSubsystem
{
    public void GetUserOrders(int userId)
    {
        Console.WriteLine($"Getting orders for user with ID {userId} from the database.");
    }
}

// Подсистема отправки электронных писем
class EmailSubsystem
{
    public void SendConfirmationEmail(string emailAddress, string message)
    {
        Console.WriteLine($"Sending confirmation email to {emailAddress}: {message}");
    }
}

// Фасад для взаимодействия с базой данных и отправкой электронных писем
class OrderFacade
{
    private DatabaseSubsystem database;
    private EmailSubsystem email;

    public OrderFacade()
    {
        database = new DatabaseSubsystem();
        email = new EmailSubsystem();
    }

    public void ProcessOrder(int userId, string emailAddress)
    {
        // Получаем заказы пользователя из базы данных
        database.GetUserOrders(userId);

        // Отправляем подтверждение заказа по электронной почте
        email.SendConfirmationEmail(emailAddress, "Your order has been processed successfully!");
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Клиентский код взаимодействует с фасадом
        OrderFacade orderFacade = new OrderFacade();
        orderFacade.ProcessOrder(123, "example@example.com");
    }
}

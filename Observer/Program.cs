using System;
using System.Collections.Generic;

// Интерфейс для наблюдателя
interface IStockObserver
{
    void Update(string stockName, double price);
}

// Конкретный наблюдатель (инвестор)
class Investor : IStockObserver
{
    private readonly string _name;

    public Investor(string name)
    {
        _name = name;
    }

    public void Update(string stockName, double price)
    {
        Console.WriteLine($"Уведомление для инвестора {_name}: Цена на акцию {stockName} изменилась и теперь составляет {price}");
    }
}

// Класс, который представляет биржу и акцию
class StockMarket
{
    private readonly Dictionary<string, double> _stocks = new Dictionary<string, double>();
    private readonly List<IStockObserver> _observers = new List<IStockObserver>();

    public void AddStock(string stockName, double price)
    {
        _stocks[stockName] = price;
    }

    public void Attach(IStockObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IStockObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify(string stockName, double price)
    {
        foreach (var observer in _observers)
        {
            observer.Update(stockName, price);
        }
    }

    public void ChangeStockPrice(string stockName, double newPrice)
    {
        if (_stocks.ContainsKey(stockName))
        {
            _stocks[stockName] = newPrice;
            Notify(stockName, newPrice);
        }
        else
        {
            Console.WriteLine($"Акция {stockName} не найдена на бирже.");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Создание биржи
        var stockMarket = new StockMarket();

        // Добавление акций
        stockMarket.AddStock("Apple", 150.0);
        stockMarket.AddStock("Microsoft", 200.0);
        stockMarket.AddStock("Google", 300.0);

        // Создание наблюдателей (инвесторов)
        var investor1 = new Investor("Иван");
        var investor2 = new Investor("Петр");

        // Подписываем инвесторов на акции
        stockMarket.Attach(investor1);
        stockMarket.Attach(investor2);

        // Изменение цены на акцию
        stockMarket.ChangeStockPrice("Apple", 160.0);
    }
}
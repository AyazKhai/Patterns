// Компонент (Component)
interface Beverage
{
    string GetDescription();
    double Cost();
}

// Конкретный компонент (ConcreteComponent)
class Coffee : Beverage
{
    public string GetDescription()
    {
        return "Coffee";
    }

    public double Cost()
    {
        return 2.0;
    }
}

// Декоратор (Decorator)
abstract class CondimentDecorator : Beverage
{
    protected Beverage beverage;

    public CondimentDecorator(Beverage beverage)
    {
        this.beverage = beverage;
    }

    public abstract string GetDescription();

    public abstract double Cost();
}

// Конкретный декоратор (ConcreteDecorator)
class Milk : CondimentDecorator
{
    public Milk(Beverage beverage) : base(beverage)
    {
    }

    public override string GetDescription()
    {
        return beverage.GetDescription() + ", Milk";
    }

    public override double Cost()
    {
        return beverage.Cost() + 0.5;
    }
}

// Конкретный декоратор (ConcreteDecorator)
class Sugar : CondimentDecorator
{
    public Sugar(Beverage beverage) : base(beverage)
    {
    }

    public override string GetDescription()
    {
        return beverage.GetDescription() + ", Sugar";
    }

    public override double Cost()
    {
        return beverage.Cost() + 0.2;
    }
}

// Клиентский код
class Client
{
    static void Main(string[] args)
    {
        // Создаем базовый напиток
        Beverage coffee = new Coffee();
        
        // Добавляем молоко
        coffee = new Milk(coffee);

        // Добавляем сахар
        coffee = new Sugar(coffee);

        // Получаем описание и стоимость напитка
        Console.WriteLine($"Description: {coffee.GetDescription()}");
        Console.WriteLine($"Cost: ${coffee.Cost()}");
    }
}
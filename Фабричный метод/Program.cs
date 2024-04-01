using System;

// Абстрактный класс, представляющий продукт
abstract class Product
{
    public abstract void Display();
}

// Конкретный продукт
class ConcreteProductA : Product
{
    public override void Display()
    {
        Console.WriteLine("Concrete Product A");
    }
}

// Конкретный продукт
class ConcreteProductB : Product
{
    public override void Display()
    {
        Console.WriteLine("Concrete Product B");
    }
}

// Абстрактный класс, представляющий создание продукта
abstract class Creator
{
    // Фабричный метод
    public abstract Product FactoryMethod();

    // Другие методы
    public void SomeOperation()
    {
        Product product = FactoryMethod();
        product.Display();
        // Дополнительные операции с продуктом
    }
}

// Конкретный создатель
class ConcreteCreatorA : Creator
{
    public override Product FactoryMethod()
    {
        return new ConcreteProductA();
    }
}

// Конкретный создатель
class ConcreteCreatorB : Creator
{
    public override Product FactoryMethod()
    {
        return new ConcreteProductB();
    }
}

class Program
{
    static void Main()
    {
        Creator creatorA = new ConcreteCreatorA();
        creatorA.SomeOperation(); // Вывод: Concrete Product A

        Creator creatorB = new ConcreteCreatorB();
        creatorB.SomeOperation(); // Вывод: Concrete Product B
    }
}
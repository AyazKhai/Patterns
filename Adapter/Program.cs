using System;

// Существующий класс с устаревшим интерфейсом
class OldComponent
{
    public void DoSomethingOld()
    {
        // ...
        Console.WriteLine("Doing something old...");
    }
}

// Новый интерфейс, который ожидается в клиентском коде
interface NewComponent
{
    void DoSomethingNew();
}

// Адаптер, преобразующий вызовы нового интерфейса в вызовы старого компонента
class Adapter : NewComponent
{
    private OldComponent oldComponent;

    public Adapter(OldComponent oldComponent)
    {
        this.oldComponent = oldComponent;
    }

    public void DoSomethingNew()
    {
        oldComponent.DoSomethingOld();
    }
}

// Клиентский код использует новый интерфейс
class Client
{
    static void Main(string[] args)
    {
        NewComponent newComponent = new Adapter(new OldComponent());
        newComponent.DoSomethingNew();
    }
}
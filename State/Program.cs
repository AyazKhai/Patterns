using System;

// Интерфейс состояния
interface ITrafficLightState
{
    void ChangeState(TrafficLight trafficLight);
}

// Конкретное состояние: красный свет
class RedLightState : ITrafficLightState
{
    public void ChangeState(TrafficLight trafficLight)
    {
        Console.WriteLine("Подождите, красный свет!");
        trafficLight.SetState(new GreenLightState());
    }
}

// Конкретное состояние: зеленый свет
class GreenLightState : ITrafficLightState
{
    public void ChangeState(TrafficLight trafficLight)
    {
        Console.WriteLine("Идите, зеленый свет!");
        trafficLight.SetState(new YellowLightState());
    }
}

// Конкретное состояние: желтый свет
class YellowLightState : ITrafficLightState
{
    public void ChangeState(TrafficLight trafficLight)
    {
        Console.WriteLine("Приготовьтесь, желтый свет!");
        trafficLight.SetState(new RedLightState());
    }
}

// Класс светофора
class TrafficLight
{
    private ITrafficLightState _state;

    public TrafficLight()
    {
        _state = new RedLightState();
    }

    public void SetState(ITrafficLightState state)
    {
        _state = state;
    }

    public void Change()
    {
        _state.ChangeState(this);
    }
}

class Program
{
    static void Main(string[] args)
    {
        var trafficLight = new TrafficLight();

        trafficLight.Change(); // Переключение с красного на зеленый
        trafficLight.Change(); // Переключение с зеленого на желтый
        trafficLight.Change(); // Переключение с желтого на красный
    }
}
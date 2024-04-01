// Интерфейс Subject
interface IImage
{
    void Display();
}

// RealSubject
class RealImage : IImage
{
    private string filename;

    public RealImage(string filename)
    {
        this.filename = filename;
        LoadImageFromDisk();
    }

    private void LoadImageFromDisk()
    {
        Console.WriteLine($"Loading image: {filename}");
    }

    public void Display()
    {
        Console.WriteLine($"Displaying image: {filename}");
    }
}

// Proxy
class ProxyImage : IImage
{
    private RealImage realImage;
    private string filename;

    public ProxyImage(string filename)
    {
        this.filename = filename;
    }

    public void Display()
    {
        if (realImage == null)
        {
            realImage = new RealImage(filename);
        }
        realImage.Display();
    }
}

// Клиентский код
class Client
{
    static void Main()
    {
        // Использование Proxy
        IImage image1 = new ProxyImage("image1.jpg");
        IImage image2 = new ProxyImage("image2.jpg");

        // Первый раз будет загрузка изображения
        image1.Display();

        // Второй раз изображение загружено из кэша Proxy
        image1.Display();

        // Первый раз будет загрузка изображения
        image2.Display();
    }
}

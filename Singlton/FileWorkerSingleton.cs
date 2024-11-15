﻿using System;
using System.IO;


namespace Patterns.Singlton
{
    /// <summary>
    /// Класс для работы с текстом. Сохранение изменений в файл 
    /// выполняется только по запросу Save.
    /// До этого изменения хранятся в динамической памяти.
    /// Реализация паттерна Одиночка.
    /// </summary>
    public sealed class FileWorkerSingleton
    {
        /// <summary>
        /// Закрытое статическое поле класса, в котором хранится единственный экземпляр
        /// класса одиночки. Инициализация экземпляра выполняется лениво - то есть
        /// будет выполняться только при первом обращении.
        /// Также данная конструкция гарантирует, что при обращении из нескольких потоков будет
        /// создан только одни экземпляр класса.
        /// </summary>
        private static readonly Lazy<FileWorkerSingleton> instance =
            new Lazy<FileWorkerSingleton>(() => new FileWorkerSingleton());

        /// <summary>
        /// Открытое свойство для доступа к экземпляру класса одиночки.
        /// </summary>
        public static FileWorkerSingleton Instance { get { return instance.Value; } }

        /// <summary>
        /// Путь к файлу записи.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Содержимое файла в динамической памяти.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Создать новый экземпляр для работы с текстом.
        /// Для того, чтобы у пользователя класса не было возможности создавать 
        /// новые экземпляры класса конструктор необходимо сделать закрытым.
        /// </summary>
        private FileWorkerSingleton()
        {
            FilePath = "test2.txt";
            ReadTextFromFile();
        }

        /// <summary>
        /// Добавить текст в файл (без сохранения в постоянную память).
        /// </summary>
        /// <param name="text"> Добавляемый текст. </param>
        public void WriteText(string text)
        {
            Text += text;
        }

        /// <summary>
        /// Сохранить текст в файл.
        /// </summary>
        public void Save()
        {
            using (var writer = new StreamWriter(FilePath, false))
            {
                writer.WriteLine(Text);
            }
        }

        /// <summary>
        /// Прочитать данные из файла.
        /// </summary>
        private void ReadTextFromFile()
        {
            if (!File.Exists(FilePath))
            {
                Text = "";
            }
            else
            {
                using (var reader = new StreamReader(FilePath))
                {
                    Text = reader.ReadToEnd();
                }
            }
        }
    }
}

using System;

namespace Field_Sotnikov
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Engine game = new Engine(25, 40);
            game.Playing();

            // Зміни
            // 1. Змінна для виходу з while у Playing
            // 2. Конструктори Engine через this()
            // 3. Переміщення та копання ями через один dictionary
            // 4. Переміщення вверх та вниз тепер по одній клітинці, тобто гравець наступає на драбину
            // 5. У IsMoveAble тільки відповідь
            // 6. Непрохідні та прохідні клітинки наслідуються від відповідних класів
            // 7. Перевірка телепорта та золота у делегаті

        }
    }
}

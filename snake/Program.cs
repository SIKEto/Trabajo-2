using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace snake
{
    enum Direccion
    {
        Abajo, Izq, Der, Arriba
    }
    class program
    {
        public static void Main()
        {
            var velocidad = 50;
            var posicioncomida = Point.Empty;
            var tamPantalla = new Size(70, 20);
            var snakee = new List<Point>();
            var longserpiente = 4;
            var puntuacion = 0;
            var posicion = new Point(0, 0);
            snakee.Add(posicion);
            var direccion = Direccion.Abajo;

            Pantalla(tamPantalla);
            Marcador(puntuacion);

            while(Movimiento(snakee,posicion,longserpiente,tamPantalla))
            {
                Thread.Sleep(velocidad);
                direccion = DameDireccion(direccion);
                posicion = ProximaPosicion(direccion, posicion);
                if (posicion.Equals(posicioncomida))
                {
                    posicioncomida = Point.Empty;
                    longserpiente++;
                    puntuacion += 1;
                    Marcador(puntuacion);
                }

                if(posicioncomida == Point.Empty)
                   posicioncomida = showfood(tamPantalla, snakee);

            }

            Console.ResetColor();
            Console.SetCursorPosition(tamPantalla.Width / 2 - 4, tamPantalla.Height / 2);
            Console.Write("Game Over");
            Thread.Sleep(2000);
            Console.ReadKey();

        }
        public static void Pantalla(Size tam)
        {
            Console.WindowHeight = tam.Height + 2;
            Console.WindowWidth = tam.Width + 2;
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
            Console.Title = "Snake";
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            for (int row = 0; row < tam.Height; row++)

                for (int col = 0; col < tam.Width; col++)
                {
                    Console.SetCursorPosition(col + 1, row + 1);
                    Console.Write(" ");
                }

        }

        private static void Marcador(int score)
        {
            Console.SetCursorPosition(1, 0);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Score " + score.ToString());
        }

        private static Direccion DameDireccion(Direccion dirActual)
        {
            if (!Console.KeyAvailable) return dirActual;

            var tecla = Console.ReadKey(true).Key;
            switch (tecla)
            {
                case ConsoleKey.DownArrow:
                    dirActual = Direccion.Abajo;
                    break;
                case ConsoleKey.UpArrow:
                    dirActual = Direccion.Arriba;
                    break;
                case ConsoleKey.RightArrow:
                    dirActual = Direccion.Der;
                    break;
                case ConsoleKey.LeftArrow:
                    dirActual = Direccion.Izq;
                    break;
            }
            return dirActual;
        }

        private static Point ProximaPosicion(Direccion dir, Point posActual)
        {
            Point proximaPosicion = new Point(posActual.X, posActual.Y);
            switch (dir)
            {
                case Direccion.Arriba:
                    proximaPosicion.Y--;
                    break;
                case Direccion.Izq:
                    proximaPosicion.X--;
                    break;
                case Direccion.Abajo:
                    proximaPosicion.Y++;
                    break;
                case Direccion.Der:
                    proximaPosicion.X++;
                    break;
            }    
            return proximaPosicion;
        }

        private static Point showfood(Size tamPantallam, List<Point> snake)
        {
            var puntocomida = Point.Empty;
            var snakehead = snake.Last();
            var rnd = new Random();
            do
            {
                var x = rnd.Next(0, tamPantallam.Width - 1);
                var y = rnd.Next(0, tamPantallam.Height - 1);
                if (snake.All(p => p.X != x || p.Y !=y) && 
                    Math.Abs(x - snakehead.X) + Math.Abs(y - snakehead.Y) > 8)
                    puntocomida = new Point(x, y);

            } while (puntocomida == Point.Empty);

            Console.BackgroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(puntocomida.X + 1, puntocomida.Y + 1);

            Console.Write(" ");
            return puntocomida;
        }
        
        private static bool Movimiento(List<Point> snake, Point posicion, int longSnake, Size tamPantalla)
        {
            var ultimopunto = snake.Last();
            if (ultimopunto.Equals(posicion)) return true;
            if (snake.Any(x => x.Equals(posicion))) return false;

            if (posicion.X < 0 || posicion.X >= tamPantalla.Width || posicion.Y < 0 || posicion.Y >= tamPantalla.Height) 
                return false;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(ultimopunto.X + 1, ultimopunto.Y + 1);
            Console.Write(" ");

            snake.Add(posicion);

            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(posicion.X + 1, posicion.Y + 1);
            Console.Write(" ");

            if (snake.Count > longSnake)
            {
                var removepoint = snake[0];
                snake.RemoveAt(0);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(removepoint.X + 1, removepoint.Y + 1);
                Console.Write(" ");
            }

            return  true;
        }


    }
}
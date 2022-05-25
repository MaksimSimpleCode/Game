using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PackMan
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            bool isPlaing = true;

            Random random = new Random();
            bool isAlive = true; 
            int ghostX, ghostY;
            int ghostDX=0, ghostDY=-1;

            int pacmanX, pacmanY;
            int pacmanDX=0, pacmanDY =1;

            int allDots = 0;
            int collectionDots = 0;

            char[,] map = ReadMap("map1",out pacmanX, out pacmanY,out ghostX, out ghostY, ref  allDots);

            

            DrawMap(map);
            while (isPlaing)
            {
                Console.SetCursorPosition(0, 20);
                Console.WriteLine($"Собрано:{collectionDots}/{allDots}");

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    ChangeDirection(key, ref pacmanDX, ref pacmanDY);
                }
                if (map[pacmanX + pacmanDX, pacmanY + pacmanDY] != '#')
                {
                    CollectDots(map, pacmanX, pacmanY, ref collectionDots);
                    Move(map,'@',ref pacmanX, ref pacmanY, pacmanDX, pacmanDY);
                    
                }
                if (map[ghostX + ghostDX, ghostY + ghostDY] != '#')
                {

                    Move(map, '$', ref ghostX, ref ghostY, ghostDX, ghostDY);

                    //TODO Сделать так чтобы энеми заходил в проемы время от времени
                    #region Попытка заходить в нычки, при необходимости убрать 
                    if ((map[ghostX - 1, ghostY] != '#') && (map[ghostX + ghostDX, ghostY + ghostDY] != '#'))
                    {
                        ChangeDirectionDown(random, ref ghostDX, ref ghostDY);
                    }
                    if ((map[ghostX + 1, ghostY] != '#') && (map[ghostX + ghostDX, ghostY + ghostDY] != '#'))
                    {
                        ChangeDirectionUp(random, ref ghostDX, ref ghostDY);
                    }
                    #endregion
                }
                else
                {
                    ChangeDirection(random,ref ghostDX, ref ghostDY);
                }

                if (ghostX==pacmanX && ghostY== pacmanY)
                {
                    isAlive = false;
                }
                Thread.Sleep(200);

                if(collectionDots == allDots || !isAlive)
                {
                    isPlaing = false;
                }
               
            }
            Console.SetCursorPosition(0, 21);
            if (collectionDots == allDots)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine("Вы победили!");
            }
            else if (!isAlive)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Вас съели");
                Console.ReadLine();
            }

        } 
        static void Move (char[,] map,char symbol,ref int X, ref int Y, int DX, int DY )
        {
            Console.SetCursorPosition(Y, X);
            Console.Write(map[X,Y]);

            X += DX;
            Y += DY;
            Console.SetCursorPosition(Y, X);
            Console.Write(symbol);
        }
       
        static void CollectDots(char[,] map ,int pacmanX, int pacmanY, ref int collectDots)
        {
            if (map[pacmanX, pacmanY] == '.')
            {
                collectDots++;
                map[pacmanX, pacmanY] = ' ';
            }
        }
        static void ChangeDirection(Random random, ref int DX, ref int DY)
        {
            int ghostDir = random.Next(1, 5);
            switch (ghostDir)
            {
                case 1:
                    DX = -1; DY = 0;
                    break;
                case 2:
                    DX = 1; DY = 0;
                    break;
                case 3:
                    DX = 0; DY = -1;
                    break;
                case 4:
                    DX = 0; DY = 1;
                    break;
            }
        }
        static void ChangeDirectionUp(Random random, ref int DX, ref int DY)
        {
            int ghostDir = random.Next(1, 3);
            switch (ghostDir)
            {
                case 1:
                    DX = 1; DY = 0;
                    break;
            }
        }
        static void ChangeDirectionDown(Random random, ref int DX, ref int DY)
        {
            int ghostDir = random.Next(1, 3);
            switch (ghostDir)
            {
                case 1:
                    DX = -1; DY = 0;
                    break;
            }
        }
        static void ChangeDirection(ConsoleKeyInfo key,ref int DX,ref int DY)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    DX = -1; DY = 0;
                    break;
                case ConsoleKey.DownArrow:
                    DX = 1; DY = 0;
                    break;
                case ConsoleKey.LeftArrow:
                    DX = 0; DY = -1;
                    break;
                case ConsoleKey.RightArrow:
                    DX = 0; DY = 1;
                    break;
            }
        }

        static void DrawMap(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i,j]);
                }
                Console.WriteLine();
            }
        }

        static char[,] ReadMap(string mapName, out int pacManX, out int pacManY, out int ghostX, out int ghostY, ref int allDots)
        {
            pacManX = 0;
            pacManY = 0;
            ghostX = 0;
            ghostY = 0;
            string path = @$"D:\Programming\Games\PacMan\PackMan\Maps\{mapName}.txt";
            string[] newFile = File.ReadAllLines(path);
            char[,] map = new char[newFile.Length, newFile[0].Length];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                var test = map.GetLength(1);
                for (int j = 0; j < test; j++)
                {
                    map[i, j] = newFile[i][j];
                    if (map[i, j] == '@')
                    {
                        pacManX = i;
                        pacManY = j;
                        map[i, j] = '.';
                    }
                    else if (map[i, j] == '$')
                    {
                        ghostX = i;
                        ghostY = j;
                        map[i, j] = '.';
                    }
                    else if(map[i,j]==' ')
                    {
                        map[i, j] = '.';
                        allDots++;
                    }
                }
            }
            return map;
        }
    }
}
  
    
       
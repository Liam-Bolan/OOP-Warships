
//Version Number 1.0

using System;
using System.IO;
namespace OOP_Warships
{
    class Program
    {

        const string TrainingGame = "Training.txt";

        private static void GetRowColumn(ref int Row, ref int Column, ref string Mtype, ref int bombcount)
        {
            
            Console.WriteLine();
            Console.WriteLine($"BOMB COUNT: {bombcount}\n");

            if (bombcount >0)
            {
                Console.Write("Please enter type (M) missile, (B) Bomb: ");
                Mtype = (Console.ReadLine().ToUpper());
            }
            if(bombcount == 0)
            {
                
                Console.Write("\nPlease enter type (M) missile: ");
                Mtype = (Console.ReadLine().ToUpper());
            }
            Console.Write("\nPlease enter column: ");
            Column = Convert.ToInt32(Console.ReadLine());
            while(Column < 0 || Column > 10)
            {
                Console.Write("\nPlease enter column: ");
                Column = Convert.ToInt32(Console.ReadLine());
            }     
            Console.Write("\nPlease enter row: ");
            Row = Convert.ToInt32(Console.ReadLine());
            while(Row < 0 || Row > 10)
            {
                Console.Write("\nPlease enter row: ");
                Row = Convert.ToInt32(Console.ReadLine());
            }
            Console.WriteLine();
            Console.Clear();
            
        }

        private static void DisplayMenu()
        {
            Console.WriteLine("MAIN MENU");
            Console.WriteLine("");
            Console.WriteLine("1. Start new game");
            Console.WriteLine("2. Load training game");
            Console.WriteLine("9. Quit");
            Console.WriteLine();

        }

        private static int GetMainMenuChoice()
        {
            int Choice = 0;
            Console.Write("Please enter your choice: ");
            Choice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            return Choice;
        }

        private static void PlayGame(ref GameBoard Board)
        {
            bool GameWon = false;
            int row=0;
            int col=0;
            int bombcnt = 2;
            bool bombsleft = true;
            string Wtype="";
            while (GameWon == false)
            {
                Board.PrintBoard();
                GetRowColumn(ref row, ref col, ref Wtype, ref bombcnt);
                if (Wtype == "M")
                {
                    Missile MyMissile = new Missile();
                    MyMissile.Fire(row, col, Board);
                }
                if (bombcnt == 0)
                {
                    bombsleft = false;
                }
                if (Wtype == "B" && bombsleft == true)
                {
                    Bomb MyBomb = new Bomb();
                    MyBomb.Fire(row, col, Board);
                    bombcnt -= 1;
                }
                
                if (Board.CheckWin() == true)
                    {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("All ships sunk!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                    }
            }
        }


        static void Main(string[] args)
        {
           

            GameBoard Board = new GameBoard();

            int MenuOption = 0;
            while (MenuOption != 9)
            {
                DisplayMenu();
                MenuOption = GetMainMenuChoice();
                if (MenuOption == 1)
                {
                    Console.Clear();
                    Board.PlaceRandomShips();
                    PlayGame(ref Board);
                }
                if (MenuOption == 2)
                {
                    Board.LoadBoard(TrainingGame);
                    PlayGame(ref Board);
                }
            }
        }
    }



    class Missile
    {
        protected int startRow;
        protected int startCol;

        public virtual void Fire(int row, int col, GameBoard Board)
        {
            startRow = row;
            startCol = col;
            Board.CheckLocation(startRow,startCol);
        }

    }



    class Bomb : Missile
    {
        private int blastRadius;
       

        public Bomb()
        {
            blastRadius = 1;
            
        }

        public override void Fire(int row, int col, GameBoard Board)
        {
            for (int startRow = row - blastRadius; startRow <= row + blastRadius; startRow++)
            {
                for (int startCol = col - blastRadius; startCol <= col + blastRadius; startCol++)
                {
                    if (startCol >= 0 && startCol < 10 && startRow >= 0 && startRow < 10)
                    {
                        
                        Board.CheckLocation(startRow,startCol);
                        
                    }
                }
            }
        }
    }



    class GameBoard
    {
        private char[,] Board = new char[10, 10];
        public Ship[] Ships = new Ship[5];


        public GameBoard()
        {

            for (int Row = 0; Row < 10; Row++)
            {
                for (int Column = 0; Column < 10; Column++)
                {
                    Board[Row, Column] = '-';
                }
            }
            
            Ships[0] = new Ship("Aircraft Carrier", 5);
            Ships[1] = new Ship("Battleship", 4);
            Ships[2] = new Ship("Submarine", 3);
            Ships[3] = new Ship("Destroyer", 3);
            Ships[4] = new Ship("Patrol Boat", 2);
        }

        public void PrintBoard()
        {
            Console.WriteLine();
            Console.WriteLine("The board looks like this: ");
            Console.WriteLine();
            Console.Write(" ");
            for (int Column = 0; Column < 10; Column++)
            {
                Console.Write(" " + Column + "  ");
            }
            Console.WriteLine();
            for (int Row = 0; Row < 10; Row++)
            {
                Console.Write(Row + " ");
                for (int Column = 0; Column < 10; Column++)
                {
                    if (Board[Row, Column] == '-')
                    {
                        Console.Write(" ");
                    }
                    else if (Board[Row, Column] == 'A' || Board[Row, Column] == 'B' || Board[Row, Column] == 'S' || Board[Row, Column] == 'D' || Board[Row, Column] == 'P')
                    {
                        Console.Write(" ");
                    }
                    
                    else
                    {
                        Console.Write(Board[Row, Column]);
                    }
                    if (Column != 9)
                    {
                        Console.Write(" | ");
                    }
                }
                Console.WriteLine();
            }
        }

        public void CheckLocation(int Row, int Column)
        {
            if (Board[Row, Column] == 'm' || Board[Row, Column] == 'h' || Board[Row, Column] == '-')
                        {
                            if (Board[Row, Column]  == '-')
                            {
                                
                                Board[Row, Column] ='m';
                                
                            }
                            Console.WriteLine("Sorry, (" + Row + "," + Column + ") is a miss.");
                        }
                        else
                        {
                            foreach (Ship ThisShip in Ships)
                            {
                                if (ThisShip.GetShipType() == Board[Row, Column])
                                {
                                    Console.WriteLine("Hit a " + ThisShip.GetName() + " at (" + Column + "," + Row + ").");
                                }
                            }
                            
                            Board[Row, Column]= 'h';
                            
            }
        }

        public void SetLocation(int Row, int Column, char Value)
        {
            Board[Row, Column] = Value;
        }

        public char GetLocation(int Row, int Column)
        {
            return Board[Row, Column];
        }

        public void LoadBoard(string TrainingGame)
        {
            string Line = "";
            StreamReader BoardFile = new StreamReader(TrainingGame);
            for (int Row = 0; Row < 10; Row++)
            {
                Line = BoardFile.ReadLine();
                for (int Column = 0; Column < 10; Column++)
                {
                    Board[Row, Column] = Line[Column];
                }
            }
            BoardFile.Close();
        }

        public void PlaceRandomShips()
        {
            Random RandomNumber = new Random();
            bool Valid;
            char Orientation = ' ';
            int Row = 0;
            int Column = 0;
            int HorV = 0;
            foreach (var Ship in Ships)
            {
                Valid = false;
                while (Valid == false)
                {
                    Row = RandomNumber.Next(0, 10);
                    Column = RandomNumber.Next(0, 10);
                    HorV = RandomNumber.Next(0, 2);
                    if (HorV == 0)
                    {
                        Orientation = 'v';
                    }
                    else
                    {
                        Orientation = 'h';
                    }
                    Valid = ValidateBoatPosition(Ship, Row, Column, Orientation);
                }
                Console.WriteLine("Computer placing the " + Ship.GetName());
                PlaceShip(Ship, Row, Column, Orientation);
            }
        }

        private bool ValidateBoatPosition(Ship Ship, int Row, int Column, char Orientation)
        {
            if (Orientation == 'v' && Row + Ship.GetSize() > 10)
            {
                return false;
            }
            else if (Orientation == 'h' && Column + Ship.GetSize() > 10)
            {
                return false;
            }
            else
            {
                if (Orientation == 'v')
                {
                    for (int Scan = 0; Scan < Ship.GetSize(); Scan++)
                    {
                        if (Board[Row + Scan, Column] != '-')
                        {
                            return false;
                        }
                    }
                }
                else if (Orientation == 'h')
                {
                    for (int Scan = 0; Scan < Ship.GetSize(); Scan++)
                    {
                        if (Board[Row, Column + Scan] != '-')
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void PlaceShip(Ship Ship, int Row, int Column, char Orientation)
        {
            if (Orientation == 'v')
            {
                for (int Scan = 0; Scan < Ship.GetSize(); Scan++)
                {
                    Board[Row + Scan, Column] = Ship.GetName()[0];
                }
            }
            else if (Orientation == 'h')
            {
                for (int Scan = 0; Scan < Ship.GetSize(); Scan++)
                {
                    Board[Row, Column + Scan] = Ship.GetName()[0];
                }
            }
        }

        public bool CheckWin()
        {
            for (int Row = 0; Row < 10; Row++)
            {
                for (int Column = 0; Column < 10; Column++)
                {
                    if (Board[Row, Column] == 'A' || Board[Row, Column] == 'B' || Board[Row, Column] == 'S' || Board[Row, Column] == 'D' || Board[Row, Column] == 'P')
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }


    class Ship
    {
        private string _Name;

        public string GetName()
        {
            return _Name;
        }

        private int _Size;

        public int GetSize()
        {
            return _Size;
        }


        public char GetShipType()
        {
            return _Name[0];
        }

        public Ship()
        {
        }

        public Ship(string ShipName, int ShipSize)
        {
            _Name = ShipName;
            _Size = ShipSize;
        }


    }
}

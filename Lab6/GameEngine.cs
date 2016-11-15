using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab6.Form1;

namespace Lab6
{
    class GameEngine
    {

        public CellSelection[,] enginegrid;//Set CellSelection to public in the Form1.cs so that it can be referenced here.
        public CellSelection nextentry = CellSelection.X;//Initialize next move to X since default is user starting
        public GameEngine()
        {
            enginegrid = new CellSelection[3, 3];//Initialize grid when the game engine is constructed
        }
        public bool LegalMove(int i, int j)//Method will check if the move is allowed and return true or false
        {
            if (enginegrid[i, j] == CellSelection.N)
            {
                enginegrid[i, j] = nextentry;//Place the correct item in the location
                if (nextentry == CellSelection.O)
                {
                    nextentry = CellSelection.X;
                }
                else
                {
                    nextentry = CellSelection.O;
                }
                return true;
            }
            return false;
        }
        public string ComputerMove()//Will be called to perform a move and manipulate the game board inside of the engine
            //Will return n if no game status is to report, l if user has lost or w if user has won
        {
            //The first thing the computer needs to check before they move is to see if you wont, if so return w;

            //Below here will be the logic for if you did not win and are not about to win and the computer cannot win, i.e. just a random choice of alowed locations;
            string gamestatus = Checker();//Checks if the game is over before the AI moves
            if (gamestatus == "w")
            {
                return "w";
            }
            else if (gamestatus == "l")
            {
                return "l";
            }
            Random rnd = new Random();
            int rndi = rnd.Next(0, 3);
            int rndj = rnd.Next(0, 3);
            while (true)
            {
                if (enginegrid[rndi, rndj] == CellSelection.N)
                {
                    enginegrid[rndi, rndj] = nextentry;
                    if (nextentry == CellSelection.O)
                    {
                        nextentry = CellSelection.X;
                        
                    }
                    else
                    {
                        nextentry = CellSelection.O;
                        
                    }
                    break;
                }
                rndi = rnd.Next(0, 3);
                rndj = rnd.Next(0, 3);
            }
            gamestatus = Checker();//Checks if the game is now over
            if (gamestatus == "w")
            {
                return "w";
            }
            else if (gamestatus == "l")
            {
                return "l";
            }
            return "n";
        }
        private string Checker()//Will be called to check if the game is over before and after AI move.
        {
            for (int i = 0; i < 3; i++)//i is going across top, we will check if columns give a win
            {
                if ((enginegrid[i, 0] == enginegrid[i, 1]) && (enginegrid[i, 0] == enginegrid[i, 2]) && (enginegrid[i, 1] == enginegrid[i, 2]))//If this is true there is a win in the column going down
                {
                    if ((enginegrid[i, 0] != CellSelection.N) && (enginegrid[i, 1] != CellSelection.N) && (enginegrid[i, 2] != CellSelection.N))
                    {
                        if (enginegrid[i, 0] == CellSelection.O)
                        {
                            return "l";
                        }
                        else
                        {
                            return "w";
                        }
                    }
                }
            }

            for (int j = 0; j < 3; j++)//j is going down the side, we will check if the rows give a win
            {
                if ((enginegrid[0, j] == enginegrid[1, j]) && (enginegrid[0, j] == enginegrid[2, j]) && (enginegrid[1, j] == enginegrid[2, j]))//If this is true there is a win in the column going down
                {
                    if ((enginegrid[0, j] != CellSelection.N) && (enginegrid[1, j] != CellSelection.N) && (enginegrid[2, j] != CellSelection.N))
                    {
                        if (enginegrid[0, j] == CellSelection.O)
                        {
                            return "l";
                        }
                        else
                        {
                            return "w";
                        }
                    }
                }
            }

            if ((enginegrid[0, 0] == enginegrid[1, 1]) && (enginegrid[0, 0] == enginegrid[2, 2]) && (enginegrid[1, 1] == enginegrid[2, 2]))//Check diagonals
            {
                if ((enginegrid[0, 0] != CellSelection.N) && (enginegrid[1, 1] != CellSelection.N) && (enginegrid[2, 2] != CellSelection.N))
                {
                    if (enginegrid[0, 0] == CellSelection.O)
                    {
                        return "l";
                    }
                    else
                    {
                        return "w";
                    }
                }
            }

            if ((enginegrid[2, 0] == enginegrid[1, 1]) && (enginegrid[2, 0] == enginegrid[0, 2]) && (enginegrid[1, 1] == enginegrid[0, 2]))//Check anti - diagonals
            {
                if ((enginegrid[2, 0] != CellSelection.N) && (enginegrid[1, 1] != CellSelection.N) && (enginegrid[0, 2] != CellSelection.N))
                {
                    if (enginegrid[2, 0] == CellSelection.O)
                    {
                        return "l";
                    }
                    else
                    {
                        return "w";
                    }
                }
            }
            return "n";
        }
    }
}

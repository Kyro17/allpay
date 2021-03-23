using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightsOut
{
    public partial class MainForm : Form
    {
        private int gridOffset;  //Distance from upper-left side of window
        private int lengthOfGrid;    //Size in pixels of grid
        private int numOfCellsInRow;      //Number of cells in grid
        private int cellLength; 

        private bool[,] grid;       //Stores on/off state of cells in grid
        private Random rand;        //Used to generate random numbers

        public MainForm()
        {
            InitializeComponent();

            gridOffset = 20;
            lengthOfGrid = 200;
            numOfCellsInRow = 5;
            cellLength = lengthOfGrid / numOfCellsInRow;

            rand = new Random();    //Initializes rnadom number generator

            grid = new bool[numOfCellsInRow, numOfCellsInRow];

            //Turn entire grid on
            for (int i = 0; i < numOfCellsInRow; i++)
            {
                for (int j = 0; j < numOfCellsInRow; j++)
                {
                  grid[i, j] = true;
                }
            }
                

        }

        private bool PlayerWon()
        {
            //Check if all lights are off
            for (int i = 0; i < numOfCellsInRow; i++)
                for (int j = 0; j < numOfCellsInRow; j++)
                    if (grid[i, j] == true)
                        return false;

            return true;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int i = 0; i < numOfCellsInRow; i++)
            {
                for (int j = 0; j < numOfCellsInRow; j++)
                {
                    //Get pen and brush for on/off grid selection
                    Brush brush;
                    Pen pen;

                    if (grid[i, j])
                    {   //On
                        pen = Pens.Black;
                        brush = Brushes.Red;
                    }
                    else
                    {   //Off
                        pen = Pens.Red;
                        brush = Brushes.Black;
                    }


                    int x = j * cellLength + gridOffset;
                    int y = i * cellLength + gridOffset;

                    //Draw inner rectangle and outline
                    g.DrawRectangle(pen, x, y, cellLength, cellLength);
                    g.FillRectangle(brush, x + 1, y + 1, cellLength - 1, cellLength - 1);
                }
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            //Make sure click was inside the grid
            if (e.X < gridOffset || e.X > cellLength * numOfCellsInRow + gridOffset ||
                e.Y < gridOffset || e.Y > cellLength * numOfCellsInRow + gridOffset)
                return;

            //Find column and row of click
            int r = (e.Y - gridOffset) / cellLength;
            int c = (e.X - gridOffset) / cellLength;

            //change clicked box and surrounding boxes
            for (int i = r-1; i <= r+1; i++)
                for (int j = c-1; j <= c+1; j++)
                    if (i >= 0 && i < numOfCellsInRow && j >= 0 && j < numOfCellsInRow)
                        grid[i,j] = !grid[i,j];

            //update
            this.Invalidate();

            //Check if grid is solved
            if (PlayerWon())
            {
                //Display winner dialog box
                MessageBox.Show(this, "Congratulations! You Win!", "Lights Out",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            
            
            //Fill grid with new colours
            for (int i = 0; i < numOfCellsInRow; i++) 
                for (int j = 0; j < numOfCellsInRow; j++) 
                    grid[i, j] = rand.Next(2) == 1;

           
            this.Invalidate();
        }
       
        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

    
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alea;
using Alea.Parallel;

namespace CsTestProject
{
    class Fake2DBoolArray
    {
        bool[] arr;
        int rowSize;  // size in x direction
        int columnSize;  // size in y direction

        /// <summary>
        /// Copies the 2D bool array into the single dimentional array
        /// </summary>
        /// <param name="">2D array to be copied</param>
        public Fake2DBoolArray(bool[,] inputArr)
        {
            this.arr = new bool[inputArr.GetLength(0) * inputArr.GetLength(1)];
            this.rowSize = inputArr.GetLength(0);
            this.columnSize = inputArr.GetLength(1);
            Parallel.For(0, inputArr.GetLength(1), i =>
            {
                Parallel.For(0, inputArr.GetLength(0), j =>
                {
                    arr[i * inputArr.GetLength(0) + j] = inputArr[j, i];
                });
            });
        }

        /// <summary>
        /// Create a fake 2D array from a 1D array, given the x and y lengths, in the format: y0,x0; y0,x1; y0,x2; - y1,x0; y1,x1; y1,x2; etc.
        /// </summary>
        /// <param name="inputArr"></param>
        /// <param name="xLength"></param>
        /// <param name="yLength"></param>
        public Fake2DBoolArray(bool[] inputArr, int xLength, int yLength)
        {
            if (xLength * yLength != inputArr.Length)
            {
                throw new ArgumentException("X and Y lengths must multiply to give the total size of the array");
            }
            bool[] anArr = arr;
            Gpu.Default.For(0, inputArr.Length, i => anArr[i] = inputArr[i]);
        }

        public bool this[int x, int y]  // Indexer declaration
        {
            get
            {
                return this.arr[y * this.rowSize + x];
            }
            set
            {
                this.arr[y * this.rowSize + x] = value;
            }
        }
    }
}

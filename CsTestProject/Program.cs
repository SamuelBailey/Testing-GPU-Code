using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alea;
using Alea.Parallel;
using System.Threading;

namespace CsTestProject
{
    class Thing
    {
        public bool isThing;
        public int thingCount;
        public String thingName;
    }

    class Program
    {
        [GpuManaged]
        static void Main(string[] args)
        {
            List<Thing> things = new List<Thing>()
            {
                new Thing(),
                new Thing(),
                new Thing(),
                new Thing(),
            };

            bool isMyThingInTheList = things.Where(t => t.isThing).Where(t => t.thingCount == 3).Select(t => t.thingName).Any(thingName => thingName.Equals("My thing"));

            bool[] boolArr = { true, false, false, true, false, true };

            var arg1 = Enumerable.Range(0, boolArr.Length).ToArray();
            for (int j = 0; j < 2; j++)
            {
                Gpu.Default.For(0, boolArr.Length, i => boolArr[i] = !boolArr[i]);
            }
            //Gpu.Default.Synchronize();
            foreach (bool value in boolArr)
            {
                Console.WriteLine(value);
            }
            Console.WriteLine();
            bool[] anotherBoolArr = new bool[boolArr.Length];
            bool[] boolArr2 = boolArr;
            Gpu.Default.For(0, boolArr.Length, i =>
            {

            });
            //Gpu.Default.Synchronize();
            foreach (bool value in boolArr)
            {
                Console.WriteLine(value);
            }

            Console.ReadLine();
            //Fake2DBoolArray boolArr2D = new Fake2DBoolArray(boolArr, 2, 3);

            //GpuDevice gpuDevice = new GpuDevice();
            //int[][] grid = new int[10][];
            //for (int i = 0; i < grid.Length; i++)
            //{
            //    grid[i] = new int[10];
            //}
            //
            //grid = gpuDevice.Increment2DArray(grid);
            //foreach (int[] row in grid)
            //{
            //    foreach (int num in row)
            //    {
            //        Console.Write(num + ", ");
            //    }
            //    Console.WriteLine();
            //}
            //Console.ReadLine();
        }

        class Pretend2DInt
        {
            private int[] values;
            private int x;
            private int y;

            public Pretend2DInt(int x, int y)
            {
                this.x = x;
                this.y = y;
                values = new int[x * y];
            }

            public Pretend2DInt(int[,] values)
            {
                this.values = new int[values.GetLength(0) * values.GetLength(1)];
                for (int i = 0; i < values.GetLength(0); i++)
                {
                    for (int j = 0; j < values.GetLength(1); j++)
                    {
                        this.values[(i * values.GetLength(0)) + j] = values[i, j];
                    }
                }
            }

            public Pretend2DInt(int[][] values)
            {
                this.values = new int[values.Length * values[0].Length];
                for (int i = 0; i < values.GetLength(0); i++)
                {
                    for (int j = 0; j < values.GetLength(1); j++)
                    {
                        this.values[(i * values.GetLength(0)) + j] = values[i][j];
                    }
                }
            }

            public int[] Value
            {
                get
                {
                    return values;
                }
            }

            public int X
            {
                get
                {
                    return x;
                }
            }

            public int Y
            {
                get
                {
                    return y;
                }
            }

            public int this[int x, int y]
            {
                get
                {
                    return values[(x * this.x) + y];
                }
                set
                {
                    values[(x * this.x) + y] = value;
                }
            }
        }

        class GpuDevice
        {
            public GpuDevice() { }

            [GpuManaged]
            public int[][] Increment2DArray(int[][] inputArray)
            {
                int[][] outputArray = new int[inputArray.Length][];
                for (int i = 0; i < inputArray.Length; i++)
                {
                    outputArray[i] = new int[inputArray[i].Length];
                }

                var arr = new Pretend2DInt(inputArray);

                int[] initialArray = arr.Value;
                int rowLength = arr.X;
                Gpu.Default.For(0, initialArray.Length, (i) =>                    
                {
                    int rowNum = i / rowLength;
                    int columnNum = i % rowLength;
                    initialArray[i]++;
                });
                return outputArray;
            }
        }
    }
}

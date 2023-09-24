using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] arr = {
            { 5, 3, 8, 4, 6, 1, 9 },
            { 10, 1, 4, 2, 5, 8, 0 },
            { 7, 6, 2, 0, 2, 1, 3 },
            { 8, 3, 7, 1, 0, 6, 9 },
            { 5, 2, 8, 0, 7, 3, 6 },
            { 1, 8, 4, 5, 2, 0, 9 },
            { 4, 3, 8, 7, 5, 0, 1 },
            { 2, 3, 7, 4, 9, 1, 6 },
            { 7, 9, 3, 5, 6, 0, 2 },
            { 9, 5, 6, 1, 7, 3, 2 },
            { 4, 2, 8, 1, 3, 0, 5 }
        };

            Console.WriteLine("Original array:");
            PrintArray(arr);

            // Restore the original array
            arr = new int[,] { { 5, 3, 8, 4, 6, 1, 9 }, { 10, 1, 4, 2, 5, 8, 0 }, { 7, 6, 2, 0, 2, 1, 3 },
            { 8, 3, 7, 1, 0, 6, 9 }, { 5, 2, 8, 0, 7, 3, 6 },
            { 1, 8, 4, 5, 2, 0, 9 },
            { 4, 3, 8, 7, 5, 0, 1 },
            { 2, 3, 7, 4, 9, 1, 6 },
            { 7, 9, 3, 5, 6, 0, 2 },
            { 9, 5, 6, 1, 7, 3, 2 },
            { 4, 2, 8, 1, 3, 0, 5 } };

            //
            int[,] originalArr = (int[,])arr.Clone();

            Console.WriteLine("\nSorting by row elements in each row:");
            int ticksQuickSortElements = SortRowsElements(arr, QuickSort);
            PrintArray(arr);
            Console.WriteLine($"QuickSort elements: {ticksQuickSortElements} ticks");

            arr = (int[,])originalArr.Clone(); // Восстанавливаем исходный массив

            Console.WriteLine("\nSorting by row elements in each row:");
            int ticksBubbleSortElements = SortRowsElements(arr, BubbleSort);
            PrintArray(arr);
            Console.WriteLine($"BubbleSort elements: {ticksBubbleSortElements} ticks");

            arr = (int[,])originalArr.Clone(); // Восстанавливаем исходный массив

            Console.WriteLine("\nSorting rows by the average of row elements:");
            int ticksAverageSort = SortRowsByAverage(arr);
            PrintArray(arr);
            Console.WriteLine($"AverageSort: {ticksAverageSort} ticks");

            // Определяем лучший и худший алгоритм сортировки
            string bestAlgorithm;
            string worstAlgorithm;

            if (ticksQuickSortElements <= ticksBubbleSortElements && ticksQuickSortElements <= ticksAverageSort)
            {
                bestAlgorithm = "QuickSort elements";
                if (ticksBubbleSortElements >= ticksAverageSort)
                {
                    worstAlgorithm = "BubbleSort elements";
                }
                else
                {
                    worstAlgorithm = "AverageSort";
                }
            }
            else if (ticksBubbleSortElements <= ticksQuickSortElements && ticksBubbleSortElements <= ticksAverageSort)
            {
                bestAlgorithm = "BubbleSort elements";
                if (ticksQuickSortElements >= ticksAverageSort)
                {
                    worstAlgorithm = "QuickSort elements";
                }
                else
                {
                    worstAlgorithm = "AverageSort";
                }
            }
            else
            {
                bestAlgorithm = "AverageSort";
                if (ticksQuickSortElements >= ticksBubbleSortElements)
                {
                    worstAlgorithm = "QuickSort elements";
                }
                else
                {
                    worstAlgorithm = "BubbleSort elements";
                }
            }

            Console.WriteLine($"\nBest algorithm: {bestAlgorithm}");
            Console.WriteLine($"\nWorst algorithm: {worstAlgorithm}");
        }

        static void PrintArray(int[,] arr)
        {
            int n = arr.GetLength(0);
            int m = arr.GetLength(1);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Console.Write(arr[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        static int SortRowsElements(int[,] arr, Action<int[]> sortMethod)
        {
            int n = arr.GetLength(0);
            int m = arr.GetLength(1);
            int totalTicks = 0;

            for (int i = 0; i < n; i++)
            {
                int[] row = new int[m];
                for (int j = 0; j < m; j++)
                {
                    row[j] = arr[i, j];
                }

                int ticks = 0;
                sortMethod(row);
                totalTicks += ticks;

                for (int j = 0; j < m; j++)
                {
                    arr[i, j] = row[j];
                }
            }

            return totalTicks;
        }

        static int SortRowsByAverage(int[,] arr)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            int n = arr.GetLength(0);
            int m = arr.GetLength(1);

            var averages = new double[n];

            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < m; j++)
                {
                    sum += arr[i, j];
                }
                averages[i] = sum / m;
            }

            var sortedIndices = averages.Select((avg, index) => new { Avg = avg, Index = index })
                                        .OrderBy(item => item.Avg)
                                        .Select(item => item.Index)
                                        .ToArray();

            int[,] newArr = new int[n, m];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    newArr[i, j] = arr[sortedIndices[i], j];
                }
            }

            Array.Copy(newArr, arr, n * m);

            stopwatch.Stop();
            return (int)stopwatch.ElapsedTicks;
        }

        static void QuickSort(int[] arr)
        {
            QuickSort(arr, 0, arr.Length - 1);
        }

        static void QuickSort(int[] arr, int left, int right)
        {
            if (left < right)
            {
                int partitionIndex = Partition(arr, left, right);

                QuickSort(arr, left, partitionIndex - 1);
                QuickSort(arr, partitionIndex + 1, right);
            }
        }

        static int Partition(int[] arr, int left, int right)
        {
            int pivot = arr[right];
            int i = left - 1;

            for (int j = left; j < right; j++)
            {
                if (arr[j] < pivot)
                {
                    i++;
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }

            int temp1 = arr[i + 1];
            arr[i + 1] = arr[right];
            arr[right] = temp1;

            return i + 1;
        }

        static void BubbleSort(int[] arr)
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Optimizacija_algoritma
{
    class Program
    {

        static int[][] arrays = new int[4][];
        static int arraySize = 50_000_000;
        static Random random = new Random();

        private static void generateArrays()
        {
            for(int i = 0; i<4; i++)
                arrays[i] = new int[arraySize];

            for (int j = 0; j < arraySize; j++)
                arrays[0][j] = arrays[1][j] = arrays[2][j] = arrays[3][j] = random.Next();
        }
        static int Partition(int[] array, int low,
                                     int high)
        {
            //1. Select a pivot point.
            int pivot = array[high];

            int lowIndex = (low - 1);

            //2. Reorder the collection.
            for (int j = low; j < high; j++)
            {
                if (array[j] <= pivot)
                {
                    lowIndex++;

                    int temp = array[lowIndex];
                    array[lowIndex] = array[j];
                    array[j] = temp;
                }
            }

            int temp1 = array[lowIndex + 1];
            array[lowIndex + 1] = array[high];
            array[high] = temp1;

            return lowIndex + 1;
        }

        static void QuickSort(int[] array, int low, int high)
        {
            if (low < high)
            {
                int partitionIndex = Partition(array, low, high);

                //3. Recursively continue sorting the array
                QuickSort(array, low, partitionIndex - 1);
                QuickSort(array, partitionIndex + 1, high);
            }
        }

        static void DualPivotQuickSort(int[] arr, int left, int right) //Quick sort sa dva pivota (Multipivot Quick sort)
        {
            if (left < right)
            {
                int[] piv;
                piv = PartitionDualPivot(arr, left, right);

                DualPivotQuickSort(arr, left, piv[0] - 1);
                DualPivotQuickSort(arr, piv[0] + 1, piv[1] - 1);
                DualPivotQuickSort(arr, piv[1] + 1, right);
            }
        }

        private static void Swap(int[] arr, int i, int j) //Pomocna metoda koja vrsi zamjenu dva elementa niza
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

        static int[] PartitionDualPivot(int[] arr, int left, int right) //Metoda koja pronalazi dva pivota kod multipivot Quick sort-a
        {
            if (arr[left] > arr[right])
                Swap(arr, left, right);


            int j = left + 1;
            int g = right - 1, k = left + 1;
            int p = arr[left], q = arr[right];

            while (k <= g)
            {


                if (arr[k] < p)
                {
                    Swap(arr, k, j);
                    j++;
                }


                else if (arr[k] >= q)
                {
                    while (arr[g] > q && k < g)
                        g--;

                    Swap(arr, k, g);
                    g--;

                    if (arr[k] < p)
                    {
                        Swap(arr, k, j);
                        j++;
                    }
                }
                k++;
            }
            j--;
            g++;


            Swap(arr, left, j);
            Swap(arr, right, g);

            return new int[] { j, g };
        }

        private static void DualPivotQuickSortParallel(int[] arr, int left, int right) //Paralelni multipivot Quick sort
        {

            if (left < right)
            {

                if (right - left < 20000)
                {
                    QuickSort(arr, left, right);
                }
                else
                {

                    int[] piv;
                    piv = PartitionDualPivot(arr, left, right);
                    Parallel.Invoke(

                        () => DualPivotQuickSortParallel(arr, left, piv[0] - 1),
                        () => DualPivotQuickSortParallel(arr, piv[0] + 1, piv[1] - 1),
                        () => DualPivotQuickSortParallel(arr, piv[1] + 1, right));
                }
            }
        }

        static int[] PartitionDualPivotWithCacheOptimisation(int[] arr, int left, int right) //Optimizacija kesa za pronalazak pivota
        {
            if (arr[left] > arr[right])
                Swap(arr, left, right);


            int j = left + 1;
            int g = right - 1, k = left + 1;
            int p = arr[left], q = arr[right];

            while (j <= g)
            {

                if (arr[j] < p)
                {
                    Swap(arr, j, k);
                    k++;
                }


                else if (arr[j] >= q)
                {
                    while (arr[g] > q && j < g)
                        g--;

                    Swap(arr, j, g);
                    g--;

                    if (arr[j] < p)
                    {
                        Swap(arr, j, k);
                        k++;
                    }
                }
                j++;
            }
            k--;
            g++;


            Swap(arr, left, k);
            Swap(arr, right, g);
            return new int[] { k, g };
        }


        static void DualPivotQuickSortWithCacheOptimisation(int[] arr, int left, int right) //Multi pivot sa cache optimizacijom
        {
            if (left < right)
            {

                int[] piv;
                piv = PartitionDualPivotWithCacheOptimisation(arr, left, right);

                DualPivotQuickSortWithCacheOptimisation(arr, left, piv[0] - 1);
                DualPivotQuickSortWithCacheOptimisation(arr, piv[0] + 1, piv[1] - 1);
                DualPivotQuickSortWithCacheOptimisation(arr, piv[1] + 1, right);
            }
        }

        static bool isSorted(int[] a)
        {
            int j = a.Length - 1;
            if (j < 1) return true;
            int ai = a[0], i = 1;
            while (i <= j && ai <= (ai = a[i])) i++;
            return i > j;
        }

        private static void DualPivotQuickSortParallelWithCacheOptimisation(int[] arr, int left, int right) //Paralelni multipivot Quick sort sa optimizacijom cahce memorije
        {

            if (left < right)
            {

                if (right - left < 20000)
                {
                    QuickSort(arr, left, right);
                }
                else
                {

                    int[] piv;
                    piv = PartitionDualPivotWithCacheOptimisation(arr, left, right);
                    Parallel.Invoke(

                        () => DualPivotQuickSortParallelWithCacheOptimisation(arr, left, piv[0] - 1),
                        () => DualPivotQuickSortParallelWithCacheOptimisation(arr, piv[0] + 1, piv[1] - 1),
                        () => DualPivotQuickSortParallelWithCacheOptimisation(arr, piv[1] + 1, right));
                }
            }
        }



        static void Main(string[] args)
        {
            generateArrays();
            Console.WriteLine("=======================================================");
            Console.WriteLine("Broj elemenata u nizu je {0}", arraySize);
            Console.WriteLine("Poredjenje vremena mjerenja algoritma:");
            Console.WriteLine("=======================================================");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            QuickSort(arrays[0], 0, arrays[0].Length - 1);
            stopwatch.Stop();
            Console.WriteLine("QuickSort vrijeme: {0}", stopwatch.ElapsedMilliseconds);
            Console.WriteLine("=======================================================");

            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            DualPivotQuickSortParallel(arrays[1], 0, arrays[1].Length - 1);
            stopwatch1.Stop();
            Console.WriteLine("QuickSort paralelizacija na vise jezgara vrijeme: {0}", stopwatch1.ElapsedMilliseconds);
            Console.WriteLine("=======================================================");

            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            DualPivotQuickSortWithCacheOptimisation(arrays[2], 0, arrays[2].Length - 1);
            stopwatch.Stop();
            Console.WriteLine("QuickSort optimizacija za kes memoriju vrijeme: {0}", stopwatch2.ElapsedMilliseconds);
            Console.WriteLine("=======================================================");

            Stopwatch stopwatch3 = new Stopwatch();
            stopwatch3.Start();
            DualPivotQuickSortParallelWithCacheOptimisation(arrays[3], 0, arrays[3].Length - 1);
            stopwatch3.Stop();
            Console.WriteLine("QuickSort kombinovane optimizacije: {0}", stopwatch3.ElapsedMilliseconds);
            Console.WriteLine("=======================================================");

            Console.WriteLine("Da li je prvi niz sortiran: {0}", isSorted(arrays[0]));
            Console.WriteLine("Da li je drugi niz sortiran: {0}", isSorted(arrays[1]));
            Console.WriteLine("Da li je treci niz sortiran: {0}", isSorted(arrays[2]));
            Console.WriteLine("Da li je cetvrti niz sortiran: {0}", isSorted(arrays[3]));

            Console.Read();
        }
    }
}

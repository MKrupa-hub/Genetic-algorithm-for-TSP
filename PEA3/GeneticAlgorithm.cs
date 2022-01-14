using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEA3
{

    class GeneticAlgorithm
    {
        private int timeStop;
        private int[,] numArray = new int[,] { };
        List<Result> testList = new List<Result>();
        private int city;
        private double mutationFactor;
        private double crossingFactor;
        private int finalDistane = 999999999;
        private int shortest;
        SimplePriorityQueue<Result> tmpQueue = new SimplePriorityQueue<Result>();

        private int populationSize;
        private Random rnd = new Random();

        public GeneticAlgorithm(int timeStop, int[,] numArray, int city, int populationSize, double mutationFactor, double crossingFactor)
        {
            this.numArray = numArray;
            this.timeStop = timeStop;
            this.city = city;
            this.populationSize = populationSize;
            this.mutationFactor = mutationFactor;
            this.crossingFactor = crossingFactor;
        }
        //tworzenie populacji poczatkowej
        public void GeneratePopulation()
        {
            int[] tmpPath = new int[] { };
            tmpPath = new int[city + 1];
            Boolean next = false;
            List<Result> tmpList = new List<Result>();

            for (int i = 0; i < populationSize; i++)
            {
                do
                {
                    for (int k = 0; k < tmpPath.Length - 1; k++)
                    {
                        tmpPath[k] = k;
                    }
                    int j;
                    for (int k = 1; k < tmpPath.Length - 1; k++)
                    {
                        j = rnd.Next(1, tmpPath.Length - 1);
                        (tmpPath[k], tmpPath[j]) = (tmpPath[j], tmpPath[k]);
                    }


                    next = testList.Contains(new Result(tmpPath));


                }
                while (next);

                next = false;
                testList.Add(new Result(tmpPath.ToArray()));

            }



            

        }
        //okreslenie drogi jaka wynosi dla danego rozwiazania
        public int GetDisance(int[] table)
        {
            int dist = 0;
            for (int i = 0; i < table.Length - 1; i++)
            {
                dist += numArray[table[i], table[i + 1]];
            }


            return dist;
        }
        //krzyzowanie 
        public List<List<int>> Crossing( int[] first,  int[] second)
        {
            List<List<int>> tmp = new List<List<int>>();
            int pivot = rnd.Next(1, city);
            List<int> copy1 = first.ToList();

            List<int> copy2 = second.ToList();
            List<int> out1 = first.ToList();
            List<int> out2 = second.ToList();
            out1.Clear();
            out2.Clear();
            //usuwam ostatni element(o wartości 0) w celu latwiejszego krzyzowania
            copy1.RemoveAt(copy1.Count() - 1);
            copy2.RemoveAt(copy2.Count() - 1);
            for (int i = 0; i < pivot; i++)
            {
                out1.Add(copy1[i]);

            }
            foreach (int el in copy2)
            {
                if (!out1.Contains(el))
                {
                    out1.Add(el);

                }

            }
            for (int i = 0; i < pivot; i++)
            {
                out2.Add(copy2[i]);

            }
            foreach (int el in copy1)
            {
                if (!out2.Contains(el))
                {
                    out2.Add(el);

                }

            }

            out1.Add(0);
            out2.Add(0);
            tmp.Add(out1);
            tmp.Add(out2);
            return tmp;

        }

        //mutacja 
        public int[] Mutation(int[] tab)
        {

            int firstIndex = rnd.Next(1, city);
            int secondIndex;
            do
            {
                secondIndex = rnd.Next(1, city);
            } while (firstIndex == secondIndex);
            (tab[firstIndex], tab[secondIndex]) = (tab[secondIndex], tab[firstIndex]);

            
            return tab;
        }
        //zmniejszenie populacji polegające na pozostawieniu tylko najlepszych osobnikow
        public void FitPopulation()
        {
            
            tmpQueue.Clear();
            
            foreach (Result el in testList)
            {

                tmpQueue.Enqueue(el, GetDisance(el.getpath()));

            }
            
            testList.Clear();
            
            for (int i = 0; i < populationSize; i++)
            {
                testList.Add(tmpQueue.Dequeue());
            }


        }

        //glowna petla algorytmu
        public void Loop()
        {
            int[] tmpPath = new int[] { };
            List<List<int>> tmplist = new List<List<int>>();
            Stopwatch stopWatch = new Stopwatch();
            int[] firstTmp = new int[] { };
            int[] secondTmp = new int[] { };
            int firstIndex;
            int secondIndex;
            int i = 0;
            stopWatch.Start();
            GeneratePopulation();
            do
            {
                for (int k = 0; k < populationSize; k += 2)
                {

                    firstIndex = rnd.Next(0, populationSize);
                    //drugi  index musi być inny od pierwszego
                    do
                    {
                        secondIndex = rnd.Next(0, populationSize);
                    } while (firstIndex != secondIndex);
                    //przypisanie do czasowych tablic
                    firstTmp = testList[firstIndex].getpath();
                    secondTmp = testList[secondIndex].getpath();
                        
                    if (rnd.Next(0, 101) <= crossingFactor * 100)
                    {
                        tmplist = Crossing(firstTmp,secondTmp);
                        firstTmp = tmplist[0].ToArray();
                        secondTmp = tmplist[1].ToArray();

                    }
                    if (rnd.Next(0, 101) <= mutationFactor * 100)
                    {
                        firstTmp = Mutation(firstTmp);
                        
                    }
                    if (rnd.Next(0, 101) <= mutationFactor * 100)
                    {
                        secondTmp = Mutation(secondTmp);
                        
                    }
                    //dodanie o testowej populacji
                    testList.Add(new Result(firstTmp));
                    testList.Add(new Result(secondTmp));
                }
                //selekcja zmniejszajaca populacje do rownej populationsize
                FitPopulation();
            }
            while (stopWatch.Elapsed.TotalMilliseconds < timeStop);

            for (i = 0; i < testList.Count; i++)
            {

                if (finalDistane > GetDisance(testList[i].getpath()))
                {
                    finalDistane = GetDisance(testList[i].getpath());
                    shortest = i;
                }

            }
            testList[shortest].display();
            Console.WriteLine("--" + GetDisance(testList[shortest].getpath())+ "--");


        }


    }
}
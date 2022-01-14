namespace PEA2
{
    using PEA3;
    using System;
    using System.IO;


    internal class Program
    {

        internal static void Main(string[] args)
        {
            int timeStop = 1000;
            int[,] numArray = new int[,] { };
            int city = 0;
            string choice = "";
            string filename = "UWAGA BRAK PLIKU ! ";
            double mutationFactor = 0.01;
            double crossingFactor = 0.8;
            int populationSize = 10;
            do
            {
                Console.WriteLine("---------------------------");
                Console.WriteLine(" 1. Wybierz macierz z pliku," + "                               || aktualny plik: " + filename + "||");
                Console.WriteLine(" 2. Wprowadzenie kryterium stopu," + "                          || aktualny próg stopu: " + timeStop / 1000 + "[s]" + "/" + timeStop + "[ms]||");
                Console.WriteLine(" 3. Ustawienie wielkości populacji początkowej," + "            || aktualna populacja: " + populationSize + "            || ");
                Console.WriteLine(" 4. Ustawienie współczynnika mutacji," + "                      || aktualny wsp. mutacji: " + mutationFactor + "       || ");
                Console.WriteLine(" 5. Ustawienia współczynnika krzyzowania, " + "                 || aktualny wsp. krzyzowania: " + crossingFactor + "    || ");
                Console.WriteLine(" 6. Uruchomienie algorytmu oraz wyświetlenie wyników");
                Console.WriteLine(" 7. Wyjście.");
                Console.Write("\r\nWybor: ");
                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":

                        Console.Clear();
                        bool fileFound = false;
                        do
                        {
                            try
                            {
                                Console.WriteLine("---------------------------");
                                Console.WriteLine("\r\nPodaj nazwe pliku");
                                Console.Write("\r\nNazwa: ");
                                filename = Console.ReadLine();
                                using (StreamReader file = new StreamReader(filename + ".txt"))
                                {

                                    string ln;
                                    city = Int32.Parse(file.ReadLine());
                                    int ft = 0; int st = 0;
                                    numArray = new int[city, city];
                                    while ((ln = file.ReadLine()) != null)
                                    {
                                        char[] separators = new char[] { ' ', '.' };
                                        string[] subs = ln.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                        foreach (var single_number in subs)
                                        {

                                            numArray[ft, st] = Int32.Parse(single_number);
                                            st++;
                                        }

                                        st = 0;
                                        ft++;


                                    }
                                    //dla miast tych samych wpisujemy droge -1
                                    for (int i = 0; i < city; i++)
                                    {
                                        numArray[i, i] = -1;
                                    }
                                    file.Close();
                                    fileFound = true;
                                }
                            }
                            catch (FileNotFoundException e) { Console.Clear(); Console.Write("Plik nie istnieje\n"); }
                        }
                        while (!fileFound);
                        break;
                    case "2":
                        int s_choice;
                        Console.Clear();
                        Console.WriteLine("---------------------------");
                        Console.WriteLine("1. Podaj czas w [s]");
                        Console.WriteLine("2. Podaj czas w [ms]");
                        Console.Write("\r\nWybor: ");
                        s_choice = Int32.Parse(Console.ReadLine());
                        if (s_choice == 1)
                        {
                            Console.WriteLine("\r\nPodaj czas w [s]");
                            timeStop = Int32.Parse(Console.ReadLine()) * 1000;

                        }
                        else if (s_choice == 2)
                        {
                            Console.WriteLine("\r\nPodaj czas w [ms]");
                            timeStop = Int32.Parse(Console.ReadLine());
                        }

                        break;


                    case "3":

                        Console.Clear();
                        Console.WriteLine("---------------------------");
                        Console.WriteLine("1. Podaj wielkosc populacji");
                        Console.Write("\r\nWartosc: ");
                        populationSize = Int32.Parse(Console.ReadLine());
                        break;

                    case "4":
                        bool good = true;
                        double test = mutationFactor;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("---------------------------");
                            Console.WriteLine("1. Podaj wspolczynnik mutacji");
                            Console.WriteLine("Wartosc z przedzialu 0 do 1");
                            Console.Write("\r\nWartosc: ");
                            try
                            {
                                test = double.Parse(Console.ReadLine());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Zly format(dozwolony np. 0,2)");
                            }
                            if (test >= 0 && test <= 1)
                            {
                                mutationFactor = test;
                                good = false;
                            }
                        }
                        while (good);
                        break;
                    case "5":
                        good = true;
                        do
                        {
                            test = crossingFactor;
                            Console.Clear();
                            Console.WriteLine("---------------------------");
                            Console.WriteLine("1. Podaj wspolczynnik krzyzowania");
                            Console.WriteLine("Wartosc z przedzialu 0 do 1");
                            Console.Write("\r\nWartosc: ");
                            try
                            {
                                test = double.Parse(Console.ReadLine());
                            }
                            catch (Exception e)
                            { Console.WriteLine("Zly format(dozwolony np. 0,2)"); }

                            if (test >= 0 && test <= 1)
                            {
                                crossingFactor = test;
                                good = false;
                            }
                        }
                        while (good);

                        break;
                    case "6":

                            GeneticAlgorithm ga = new GeneticAlgorithm(timeStop, numArray, city, populationSize, mutationFactor, crossingFactor);
                            ga.Loop();
                        
                        break;


                    default:

                        break;



                }
            } while (choice != "7");
        }

       
    }
}

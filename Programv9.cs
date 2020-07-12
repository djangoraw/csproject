using System;
using System.Linq;

namespace Kolkoikrzyzyk
{
    class Program
    {
        enum Tura
        {
            Gracz0,
            GraczX
        }
        enum StatusGry
        {
            X, // wygrana x
            O, // wygrana 0
            Remis,
            Nieskonczona
        }

        static string[] poz = new string[9] { "0", "1", "2", "3", "4", "5", "6", "7", "8" }; // pozycje na planszy
        public static string NazwaGraczO { get; set; }
        public static string NazwaGraczX { get; set; }

        public static int PunktyGracz1 { get; set; }
        public static int PunktyGracz2 { get; set; }

        static StatusGry graRes = StatusGry.Nieskonczona;

        static void Main(string[] args) // metoda main
        {
            int wybor;
            var tura = Tura.Gracz0;

            Intro();

            do
            {
                while (graRes == StatusGry.Nieskonczona) // 
                {
                    Plansza();
                    Console.WriteLine($"{Environment.NewLine}Punkty: {NazwaGraczO} - {PunktyGracz1}     {NazwaGraczX} - {PunktyGracz2}");
                    Console.WriteLine($"Kolej ma gracz: {(tura == Tura.Gracz0 ? NazwaGraczO : NazwaGraczX)}");

                    wybor = NastepnaPozycja();
                    if (poz[wybor] == wybor.ToString()) // sprawdza czy pozycja jest juz zajeta
                    {
                        poz[wybor] = tura == Tura.Gracz0 ? "O" : "X";
                    }
                    else
                    {
                        Console.WriteLine("Ta pozycja jest już zajęta! Wybierz inną.");
                        Console.ReadLine();
                        Console.Clear();
                        continue;
                    }

                    tura = tura == Tura.Gracz0 ? Tura.GraczX : Tura.Gracz0;
                    Console.Clear();
                    graRes = CzyWygral();
                }

                // koniec
                Plansza();

                switch (graRes)
                {
                    case StatusGry.X:
                        Console.WriteLine($"Gracz {NazwaGraczX} wygrał!");
                        PunktyGracz2++;
                        break;
                    case StatusGry.O:
                        Console.WriteLine($"Gracz {NazwaGraczO} wygrał!");
                        PunktyGracz1++;
                        break;
                    case StatusGry.Remis:
                        Console.WriteLine("Remis!");
                        break;
                }
            }
            while (WyborCoDalej());
        }

        static int NastepnaPozycja()
        {
            int wybor;
            while (true)
            {
                Console.WriteLine("Którą pozycję chciałbyś zająć?");
                if (int.TryParse(Console.ReadLine(), out wybor))
                    if (wybor >= 0 && wybor <= 8) break;
                    else Console.WriteLine("Pozycje mają numery od 0 do 8! Spróbuj ponownie.");
            }
            return wybor;
        }

        static bool WyborCoDalej()
        {
            bool playing = true;
            int wybor;

            Console.WriteLine("Punkty: {0} - {1}     {2} - {3}", NazwaGraczO, PunktyGracz1, NazwaGraczX, PunktyGracz2);
            Console.WriteLine("");
            Console.WriteLine("Co teraz chcesz zrobić?");
            Console.WriteLine("1. Graj ponownie");
            Console.WriteLine("2. Wyjdź z gry.");
            Console.WriteLine("");

            while (true)
            {
                Console.WriteLine("Wybierz: ");
                if (int.TryParse(Console.ReadLine(), out wybor))
                    if (wybor == 1 || wybor == 2) break;
            }

            switch (wybor)
            {
                case 1:
                    poz = poz.Select((x, i) => poz[i] = i.ToString()).ToArray(); // reset planszy
                    Console.Clear();
                    graRes = StatusGry.Nieskonczona;
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("Dziękuję za grę!");
                    Console.ReadLine();
                    playing = false;
                    break;
            }

            return playing;
        }

        static bool CzyDalej()
        {
            return poz.Where(x => x == "X" || x == "O").Count() < poz.Length;
        }

        static void Intro() // intro
        {
            Console.WriteLine("Dzień dobry! Witam w grze Kółko i Krzyżyk.");
            Console.WriteLine("Jak się nazywa gracz nr 1?");
            NazwaGraczO = Console.ReadLine();
            Console.WriteLine("Jak się nazywa gracz nr 2?");
            NazwaGraczX = Console.ReadLine();
            Console.WriteLine("Dobrze. {0} to O a {1} to X.", NazwaGraczO, NazwaGraczX);
            Console.WriteLine("{0} zaczyna.", NazwaGraczO);
            Console.ReadLine();
            Console.Clear();
        }

        static readonly int[][] pozWygrana = new[] // pozycje ktore wygrywaja
        {
        new[] {0, 1, 2},
        new[] {3, 4, 5},
        new[] {6, 7, 8},
        new[] {0, 3, 6},
        new[] {1, 4, 7},
        new[] {2, 5, 8},
        new[] {0, 4, 8},
        new[] {2, 4, 6}
    };
        static StatusGry CzyWygral()
        {
            var czyWygrana = pozWygrana.Select(x => x.Select(i => poz[i]).Distinct()) // sprawdza czy dana pozycja jest wygrana
                        .Where(x => x.Count() == 1).ToList();

            if (czyWygrana.Count() == 0)
                return CzyDalej() ? StatusGry.Nieskonczona : StatusGry.Remis;
            else
                return czyWygrana.SingleOrDefault().SingleOrDefault() == "X" ? StatusGry.X : StatusGry.O;
        }

        static void Plansza() // rysowanie planszy
        {
            PozycjaLewa(poz[0]);
            PozycjaSrodek(poz[1]);
            PozycjaPrawa(poz[2]);
            Console.ResetColor();
            Console.WriteLine("-------------------");
            PozycjaLewa(poz[3]);
            PozycjaSrodek(poz[4]);
            PozycjaPrawa(poz[5]);
            Console.ResetColor();
            Console.WriteLine("-------------------");
            PozycjaLewa(poz[6]);
            PozycjaSrodek(poz[7]);
            PozycjaPrawa(poz[8]);
            Console.ResetColor();
            Console.WriteLine("-------------------");
        }
        static void PozycjaLewa(string poz)
        {
            KolorKonsoli(poz);
            Console.Write("   {0}", poz);
            Console.ResetColor();
            Console.Write("  |  ");
        }
        static void PozycjaSrodek(string poz) 
        {
            KolorKonsoli(poz);
            Console.Write("{0}", poz);
            Console.ResetColor();
            Console.Write("  |  ");
        }
        static void PozycjaPrawa(string poz)
        {
            KolorKonsoli(poz);
            Console.WriteLine("{0}   ", poz);
            Console.ResetColor();
        }
        static void KolorKonsoli(string poz) // kolory 
        {
            switch (poz)
            {
                case "O":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "X":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JakaToMelodia.Model
{
    public class Model
    {
        // --------- Stała - liczba utworów w bazie -------------
        private const int LiczbaUtworów = 30;
        // --------- Zmienne modelu -----------------------------
        public int Wynik;
        public int Czas;
        public int NrUtworu;
        // --------- Zmienne używane przy losowaniu -------------
        private static readonly Random rnd = new Random();
        public int Odpowiedź { get; private set; }
        public int OdpID { get; private set; }
        // --------- Wartości przycisków odpowiedzi -------------
        public int[] WartościPrzycisków = new int[4];
        // --------- Flagi utworów ---------------
        public bool[] CzyGrało = new bool[LiczbaUtworów];

        // -------------------- METODY --------------------------
        // --- metoda losująca utwór oraz odpowiedzi ------------
        public void Losuj()
        {
            do
            {
                Odpowiedź = rnd.Next(0, LiczbaUtworów);
            } while (CzyGrało[Odpowiedź] == true); // --- losoawnie utworu, który jeszcze nie leciał
            CzyGrało[Odpowiedź] = true; // --- ustawienie flagi utworu na "odtworzony"
            OdpID = rnd.Next(0, 4); // --- wylosowanie, który przycisk będzie zawierał dobrą odpowiedź
            WartościPrzycisków[OdpID] = Odpowiedź; // --- naniesienie odpowiedzi na dobry przycisk

            for (int i = 0; i < 4; i++)
            {
                if (i != OdpID) // --- dla każdego przycisku innego, niż ten z dobrą odpowiedzią
                {
                    bool powt;
                    do
                    {
                        int los = rnd.Next(0, LiczbaUtworów);
                        int j = i - 1;

                        if (los == Odpowiedź) powt = true; // --- sprawdzenie, czy tytuł nie jest dobrą odpowiedzią
                        else powt = false;

                        while (j >= 0 && powt == false) // --- sprawdzenie czy tytuł utworu nie znajduje się na jednym z wcześniejszych przycisków
                        {
                            if (los == WartościPrzycisków[j])
                            {
                                powt = true;
                            }
                            else j -= 1;
                        }

                        if (powt == false) WartościPrzycisków[i] = los; // --- ustawienie przycisku jeśli się wszystko zgadza

                    } while (powt == true);   // --- jeśli tytuł nie może trafić na przycisk - ponowne losowanie
                }
            }
        }
        // ----- Konstruktor ------
        public Model()
        {
            Wynik = 0;
            NrUtworu = 0;
            for (int i = 0; i < LiczbaUtworów; i++) CzyGrało[i] = false; // --- ustawienie wstępne flag utworów na "jeszczde nieodtworzone"
            Losuj();
        }
    }

    public static class Baza
    {
        public static List<string> Tytuł = new List<string> 
        {
            "Kiedy Powiem Sobie Dość",
            "You're My Heart, You're My Soul",
            "The Final Countdown",
            "Again",
            "Another Brick in the Wall",
            "Child in Time",
            "Californication",
            "Teksański",
            "Clocks",
            "Kayleigh",
            "I Was Made For Lovin' You",
            "Imagine",
            "In The End",
            "Whole Lotta Love",
            "It's My Life",
            "Knockin' On Heaven's Door",
            "Smoke On The Water",
            "Wind of Change",
            "Sultans of Swing",
            "Love Will Tear Us Apart",
            "The Unforgiven",
            "Take on Me",
            "Fear of the Dark",
            "Zombie",
            "Enjoy the Silence",
            "Voyage, Voyage",
            "Freedom '90",
            "Mamma Mia",
            "Stayin' Alive",
            "Pokolenie"
        };
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JakaToMelodia
{

    public partial class MainWindow : Window
    {
        // ---------- Okno --------------------------
        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += tick; // ------- Dodanie zdarzenia opisującego działanie zegara ---------
            timer.Interval = new TimeSpan(0, 0, 1); // ------ ustawienie zegara na odliczanie sekund ------------
        }

        // ---------- Obiekt zegara -----------
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        // -------- Zmienna odtwarzacza --------
        System.Media.SoundPlayer player = null;

        // ---------- Zmienna dla modelu ------------
        Model.Model model = null;

        // ------------ Działanie zegara -------------------------
        private void tick(object sender, EventArgs e)
        {
            if (model != null)
            {
                model.Czas -= 1;
                Czas.Text = model.Czas.ToString();
                if (model.Czas <= 0) koniec(true); // jeśli czas się skończy - komunikat końcowy
            }      
        }

        // ------------ Dodanie zawartości dla przycisków i kontrolek gry ------------
        private void przygotujZawartość()
        {
            if (model != null)
            {
                Odp1.Content = Model.Baza.Tytuł[model.WartościPrzycisków[0]];
                Odp2.Content = Model.Baza.Tytuł[model.WartościPrzycisków[1]];
                Odp3.Content = Model.Baza.Tytuł[model.WartościPrzycisków[2]];
                Odp4.Content = Model.Baza.Tytuł[model.WartościPrzycisków[3]];
                Czas.Text = model.Czas.ToString();
                Wynik.Text = model.Wynik.ToString();
                NrUtworu.Text = model.NrUtworu.ToString();

                player = new System.Media.SoundPlayer("..\\..\\Music\\" + model.Odpowiedź + ".wav");
                player.Play();
            }
        }

        // -------- MENU ----------

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Menu.Visibility = System.Windows.Visibility.Hidden;
            Poziom.Visibility = System.Windows.Visibility.Visible;
        }

        private void Wyjdź_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // --------POZIOM ---------

        private void Poziom_Click(object sender, RoutedEventArgs e)
        {
            Poziom.Visibility = System.Windows.Visibility.Hidden;
            Gra.Visibility = System.Windows.Visibility.Visible; 

            model = new Model.Model(); // --- utworzenie instancji modelu, aktywowanie konstruktorów ---

            if (sender.Equals(Łatwy)) model.Czas = 25; // --- sprawdzenie, który przycisk poziomu trudności został wciśnięty
            else if (sender.Equals(Średni)) model.Czas = 20;
            else model.Czas = 15;

            przygotujZawartość(); // --- naniesienie na przyciski zawartości ---
            timer.Start(); // --- wystartowanie zegara ---
        }

        // -------- GRA -----------

        private void Odp_Click(object sender, RoutedEventArgs e)
        {
            Komunikat.Visibility = System.Windows.Visibility.Visible; // --- pojawienie się komunikatu ---
            player.Stop();
            timer.Stop();

            int przyciskID = -1;
            if (sender.Equals(Odp1)) przyciskID = 0; // --- sprawdzamy jaki przycisk został naciśnięty
            else if (sender.Equals(Odp2)) przyciskID = 1;
            else if (sender.Equals(Odp3)) przyciskID = 2;
            else if (sender.Equals(Odp4)) przyciskID = 3;
            if (model.OdpID == przyciskID) // --- została wybrana prawidłowa odpowiedź
            {
                KomunikatDobrze.Visibility = System.Windows.Visibility.Visible;
                model.Wynik += 1;
            }
            else // --- zła odpowiedź
            {
                KomunikatŹle.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void KomunikatButton_Click(object sender, RoutedEventArgs e)
        {
            Komunikat.Visibility = System.Windows.Visibility.Hidden;
            KomunikatDobrze.Visibility = System.Windows.Visibility.Hidden;
            KomunikatŹle.Visibility = System.Windows.Visibility.Hidden;

            model.NrUtworu++;
            if (model.NrUtworu >= 10) // --- został osiągnięty limit pytań (10) - koniec gry - komunikat
            {
                koniec(false);
            }
            else // --- nowe pytanie, wylosowanie odpowiedzi
            {
                model.Losuj();
                przygotujZawartość();
                timer.Start();
            }
        }

        private void KoniecButton_Click(object sender, RoutedEventArgs e)
        {
            Koniec.Visibility = System.Windows.Visibility.Hidden;
            Poziom.Visibility = System.Windows.Visibility.Hidden;
            Menu.Visibility = System.Windows.Visibility.Visible;
        }

        private void koniec(bool czyCzas)
        {
            timer.Stop();
            player.Stop();
            Gra.Visibility = System.Windows.Visibility.Hidden;

            Koniec.Visibility = System.Windows.Visibility.Visible;
            KoniecWynik.Text = model.Wynik.ToString();
            if (czyCzas) KoniecText.Text = " KONIEC CZASU";
            else
            {
                KoniecPozostałyCzas.Text = "POZOSTAŁY CZAS: " + model.Czas;
                KoniecText.Text = "       KONIEC";
            }
        }
    }
}
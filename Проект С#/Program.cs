using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AirlineReservationSystem
{
    class Program : Form
    {
        private TextBox textBox; // Элемент управления для вывода информации

        // Конструктор формы
        public Program()
        {
            // Устанавливаем название формы
            this.Text = "'Strat' Airline Reservation System";

            // Загружаем и устанавливаем иконку
            string iconFilePath = "C:\\Users\\user\\Desktop\\plane.ico"; // Путь к файлу с иконкой
            this.Icon = new System.Drawing.Icon(iconFilePath);

            // Инициализируем элементы управления
            InitializeComponents();

            // Вызываем метод для выполнения кода
            RunFlightBookingSystem();
        }

        // Инициализация элементов управления формы
        private void InitializeComponents()
        {
            // Создаем элемент управления TextBox
            textBox = new TextBox();
            textBox.Multiline = true;
            textBox.Dock = DockStyle.Fill;
            textBox.ReadOnly = true;

            // Добавляем элемент управления на форму
            this.Controls.Add(textBox);
        }

        // Метод для выполнения кода
        private void RunFlightBookingSystem()
        {
            // Создаем экземпляр рейса
            Flight flight1 = new Flight("ABC123", 50);

            // Бронируем место 10
            flight1.BookSeat(10);

            // Выводим список доступных мест за вычетом забронированных
            DisplayAvailableSeats(flight1);
        }

        // Метод для отображения доступных мест на форме
        private void DisplayAvailableSeats(Flight flight)
        {
            // Очищаем содержимое textBox
            textBox.Clear();

            // Формируем текст с информацией о доступных местах
            StringBuilder sb = new StringBuilder();
            DateTime currentDateTime = DateTime.Now;
            sb.AppendLine($"\nAvailable seats for flight {flight.FlightNumber}");
            sb.AppendLine($"\nas of current time: {currentDateTime}\n");
            foreach (var seat in flight.Seats)
            {
                if (!seat.IsBooked)
                    sb.AppendLine($"Seat Number: {seat.SeatNumber}, Class: {seat.Class}");
            }

            // Выводим текст на textBox
            textBox.Text = sb.ToString();
        }

        // Определение класса Seat
        public class Seat
        {
            public int SeatNumber { get; set; }
            public string Class { get; set; }
            public bool IsBooked { get; set; }

            public Seat(int seatNumber, string seatClass)
            {
                SeatNumber = seatNumber;
                Class = seatClass;
                IsBooked = false;
            }
        }

        // Определение класса Flight
        public class Flight
        {
            public string FlightNumber { get; set; }
            public List<Seat> Seats { get; set; }

            public Flight(string flightNumber, int totalSeats)
            {
                FlightNumber = flightNumber;
                Seats = new List<Seat>();
                InitializeSeats(totalSeats);
            }

            private void InitializeSeats(int totalSeats)
            {
                for (int i = 1; i <= totalSeats; i++)
                {
                    string seatClass = i <= totalSeats / 2 ? "First Class" : "Standard";
                    Seats.Add(new Seat(i, seatClass));
                }
            }

            public void BookSeat(int seatNumber)
            {
                Seat seat = Seats.Find(s => s.SeatNumber == seatNumber);
                if (seat != null && !seat.IsBooked)
                {
                    seat.IsBooked = true;
                }
            }
        }

        // Основной метод программы
        static void Main(string[] args)
        {
            // Запускаем приложение и отображаем форму
            Application.Run(new Program());
        }
    }
}

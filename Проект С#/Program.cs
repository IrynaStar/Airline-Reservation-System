using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AirlineReservationSystem
{
    class Program : Form
    {
        private TextBox textBox; // Элемент управления для вывода информации
        private TextBox seatInput; // Элемент управления для ввода номера места
        private Button bookButton; // Кнопка для бронирования места
        private Flight flight1; // Экземпляр рейса

        // Конструктор формы
        public Program()
        {
            // Устанавливаем название формы
            this.Text = "'Strat' Airline Reservation System";

            // Получаем полный путь к иконке в ресурсах проекта
            string iconPath = @"C:\Users\user\Source\Repos\IrynaStar\Airline-Reservation-System\Проект С#\Properties\Resources\plane.ico";

            // Устанавливаем иконку формы
            this.Icon = new System.Drawing.Icon(iconPath);



            // Инициализируем элементы управления
            InitializeComponents();

            // Создаем экземпляр рейса
            flight1 = new Flight("ABC123", 50);

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

            // Создаем элемент управления TextBox для ввода номера места
            seatInput = new TextBox();
            seatInput.Width = 100;

            // Создаем кнопку для бронирования места
            bookButton = new Button();
            bookButton.Text = "Book Seat";
            bookButton.Click += BookButton_Click;

            // Создаем панель для размещения элементов в верхней части формы
            Panel topPanel = new Panel();
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 40; // Задаем высоту панели
            topPanel.Padding = new Padding(10); // Задаем отступы внутри панели

            // Размещаем элементы внутри панели
            Label labelSeatNumber = new Label();
            labelSeatNumber.Text = "Enter Seat Number";
            labelSeatNumber.AutoSize = true;
            labelSeatNumber.Dock = DockStyle.Left;

            seatInput.Dock = DockStyle.Left;

            bookButton.Dock = DockStyle.Left;

            topPanel.Controls.Add(labelSeatNumber);
            topPanel.Controls.Add(seatInput);
            topPanel.Controls.Add(bookButton);

            // Создаем SplitContainer для размещения панели и текстового блока
            SplitContainer splitContainer = new SplitContainer();
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Orientation = Orientation.Vertical;
            splitContainer.Panel1.Controls.Add(topPanel); // Верхняя панель в первую панель SplitContainer
            splitContainer.Panel2.Controls.Add(textBox); // TextBox во вторую панель SplitContainer

            // Добавляем SplitContainer на форму
            this.Controls.Add(splitContainer);
        }

        // Метод для выполнения кода
        private void RunFlightBookingSystem()
        {
            // Бронируем место 10 (например, если бронь ставит себе компания на всякий случай)
            flight1.BookSeat(10);

            // Выводим список доступных мест за вычетом забронированных (из которых пользователь себе сам выберет место)
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

        // Прописываем обработчик события нажатия кнопки бронирования места
        private void BookButton_Click(object sender, EventArgs e)
        {
            // Получаем номер места из текстового поля
            if (int.TryParse(seatInput.Text, out int seatNumber))
            {
                // Проверяем, что такое место существует на самолете
                if (seatNumber >= 1 && seatNumber <= flight1.Seats.Count)
                {
                    // Бронируем выбранное место
                    flight1.BookSeat(seatNumber);

                    // Выводим обновленный список доступных мест
                    DisplayAvailableSeats(flight1);

                    // Оформляем покупку билета
                    MessageBox.Show($"Seat {seatNumber} booked successfully! Ticket purchase completed.");
                }
                else
                {
                    // Выводим сообщение об ошибке, если введен неверный номер места
                    MessageBox.Show("Invalid seat number. Please enter a valid seat number.");
                }
            }
            else
            {
                // Выводим сообщение об ошибке, если введено некорректное значение
                MessageBox.Show("Invalid input. Please enter a valid seat number.");
            }
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

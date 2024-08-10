using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AirlineReservationSystem
{
    // Определение интерфейсов

    public interface ISeat
    {
        int SeatNumber { get; set; }
        string Class { get; set; }
        bool IsBooked { get; set; }
    }

    public interface IFlight
    {
        string FlightNumber { get; set; }
        List<ISeat> Seats { get; set; }
        void BookSeat(int seatNumber);
        int TotalSeats { get; }
        int BookedSeats { get; }
        int AvailableSeats { get; }
    }

    public interface IBookingSystem
    {
        void BookSeat(int seatNumber);
        void DisplayAvailableSeats();
    }

    public interface IUserInterface
    {
        void ShowMessage(string message);
        void UpdateSeatDisplay(string seatInfo);
    }

    public interface IReservation
    {
        void ReserveSeat(IFlight flight, int seatNumber);
    }

    public interface ISeatManager
    {
        void BookSeat(IFlight flight, int seatNumber);
        List<ISeat> GetAvailableSeats(IFlight flight);
    }

    public interface IFlightManager
    {
        IFlight CreateFlight(string flightNumber, int totalSeats);
    }

    public interface IMessageHandler
    {
        void ShowMessage(string message);
    }

    // Реализация класса Seat

    public class Seat : ISeat
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

    // Реализация класса Flight

    public class Flight : IFlight
    {
        public string FlightNumber { get; set; }
        public List<ISeat> Seats { get; set; }

        public Flight(string flightNumber, int totalSeats)
        {
            FlightNumber = flightNumber;
            Seats = new List<ISeat>();
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
            ISeat seat = Seats.Find(s => s.SeatNumber == seatNumber);
            if (seat != null && !seat.IsBooked)
            {
                seat.IsBooked = true;
            }
        }

        public int TotalSeats => Seats.Count;
        public int BookedSeats => Seats.FindAll(seat => seat.IsBooked).Count;
        public int AvailableSeats => TotalSeats - BookedSeats;
    }

    // Реализация класса Reservation

    public class Reservation : IReservation
    {
        public void ReserveSeat(IFlight flight, int seatNumber)
        {
            flight.BookSeat(seatNumber);
        }
    }

    // Реализация класса SeatManager

    public class SeatManager : ISeatManager
    {
        public void BookSeat(IFlight flight, int seatNumber)
        {
            flight.BookSeat(seatNumber);
        }

        public List<ISeat> GetAvailableSeats(IFlight flight)
        {
            return flight.Seats.FindAll(seat => !seat.IsBooked);
        }
    }

    // Реализация класса FlightManager

    public class FlightManager : IFlightManager
    {
        public IFlight CreateFlight(string flightNumber, int totalSeats)
        {
            return new Flight(flightNumber, totalSeats);
        }
    }

    // Реализация класса MessageHandler

    public class MessageHandler : IMessageHandler
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }

    // Реализация класса Program, который также является формой

    public class Program : Form, IUserInterface, IBookingSystem
    {
        private RichTextBox textBox; 
        private TextBox seatInput; // Элемент управления для ввода номера места
        private Button bookButton; // Кнопка для бронирования места
        private IFlight flight1; // Экземпляр рейса
        private ISeatManager seatManager; // Менеджер мест
        private IReservation reservation; // Система бронирования
        private IMessageHandler messageHandler; // Обработчик сообщений

        // Конструктор формы
        public Program()
        {
            // Устанавливаем название формы
            this.Text = "'Strat' Airline Reservation System";

            // Получаем полный путь к иконке в ресурсах проекта
            string iconPath = @"C:\Users\user\Source\Repos\IrynaStar\Airline-Reservation-System\Проект С#\Properties\Resources\plane.ico";

            // Устанавливаем иконку формы
            this.Icon = new System.Drawing.Icon(iconPath);

            this.Size = new System.Drawing.Size(900, 600); // Ширина 900, высота 600

            // Инициализируем элементы управления
            InitializeComponents();

            // Создаем экземпляры менеджеров и обработчика сообщений
            seatManager = new SeatManager();
            reservation = new Reservation();
            messageHandler = new MessageHandler();

            // Создаем экземпляр рейса
            var flightManager = new FlightManager();
            flight1 = flightManager.CreateFlight("ABC123", 50);

            // Бронируем место 10 авиакомпанией
            reservation.ReserveSeat(flight1, 10);

            // Отображаем список доступных мест
            DisplayAvailableSeats();
        }

        // Инициализация элементов управления формы
        private void InitializeComponents()
        {
            // Создаем элемент управления RichTextBox
            textBox = new RichTextBox();
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
            splitContainer.Panel2.Controls.Add(textBox); // RichTextBox во вторую панель SplitContainer

            // Добавляем SplitContainer на форму
            this.Controls.Add(splitContainer);
        }

        // Метод для отображения доступных мест на форме
        public void DisplayAvailableSeats()
        {
            // Очищаем содержимое textBox
            textBox.Clear();

            // Формируем текст с информацией о доступных местах
            StringBuilder sb = new StringBuilder();
            DateTime currentDateTime = DateTime.Now;
            sb.AppendLine($"\nAvailable seats for flight {flight1.FlightNumber}");
            sb.AppendLine($"\nas of current time: {currentDateTime}\n");

            // Добавляем информацию о количестве мест
            sb.AppendLine($"Total Seats: {flight1.TotalSeats}");
            sb.AppendLine($"Booked Seats: {flight1.BookedSeats}");
            sb.AppendLine($"Available Seats: {flight1.AvailableSeats}\n");

            // Добавляем информацию о доступных местах
            var availableSeats = seatManager.GetAvailableSeats(flight1);
            foreach (var seat in availableSeats)
            {
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
                if (seatNumber >= 1 && seatNumber <= flight1.TotalSeats)
                {
                    // Бронируем выбранное место
                    BookSeat(seatNumber);

                    // Выводим обновленный список доступных мест
                    DisplayAvailableSeats();

                    // Оформляем покупку билета
                    ShowMessage($"Seat {seatNumber} booked successfully! Ticket purchase completed.");
                }
                else
                {
                    // Выводим сообщение об ошибке, если введен неверный номер места
                    ShowMessage("Invalid seat number. Please enter a valid seat number.");
                }
            }
            else
            {
                // Выводим сообщение об ошибке, если введено некорректное значение
                ShowMessage("Invalid input. Please enter a valid seat number.");
            }
        }

        // Реализация методов интерфейсов
        public void BookSeat(int seatNumber)
        {
            seatManager.BookSeat(flight1, seatNumber);
        }

        public void UpdateSeatDisplay(string seatInfo)
        {
            textBox.Text = seatInfo;
        }

        public void ShowMessage(string message)
        {
            messageHandler.ShowMessage(message);
        }

        // Основной метод программы
        static void Main()
        {
            // Запускаем приложение и отображаем форму
            Application.Run(new Program());
        }
    }
}

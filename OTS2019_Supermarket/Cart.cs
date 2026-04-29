using OTS_Supermarket.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTS_Supermarket
{
    public class Cart
    {
        public int Size;
        public List<Item> Items;
        public double Amount { get; set; }
        public double Budget { get; set; }
        public int Monitor_counter { get => monitor_counter; set => monitor_counter = value; }
        public int Keyboard_counter { get => keyboard_counter; set => keyboard_counter = value; }
        public int Laptop_counter { get => laptop_counter; set => laptop_counter = value; }
        public int Computer_counter { get => computer_counter; set => computer_counter = value; }
        public int Chair_counter { get => chair_counter; set => chair_counter = value; }

        private int monitor_counter = 0;
        private int keyboard_counter = 0;
        private int laptop_counter = 0;
        private int computer_counter = 0;
        private int chair_counter = 0;
        private double discount = 0;

        public Cart()
        {
            Size = 0;
            Amount = 0;
            Items = new List<Item>();
        }

        public void AddOneToCart(Item item)
        {
            if (Size >= 10)
            {
                throw new Exception("Number of items in cart must be 10 or less!");
            }
            else
            {
                Items.Add(item);
                Amount = Amount + item.Price;
                Size++;
                if (item.GetType().Equals(typeof(Monitor)))
                {
                    monitor_counter++;
                }
                else if (item.GetType().Equals(typeof(Keyboard)))
                {
                    keyboard_counter++;
                }
                else if (item.GetType().Equals(typeof(Laptop)))
                {
                    laptop_counter++;
                }
                else if (item.GetType().Equals(typeof(Computer)))
                {
                    computer_counter++;
                }
                else if (item.GetType().Equals(typeof(Chair)))
                {
                    chair_counter++;
                }
            }
        }

        public void AddMultipleToCart(Item item, int quantity)
        {
            if (Size + quantity > 10)
            {
                throw new Exception("Number of items in cart must be 10 or less!");
            }
            else
            {
                for (int i = 0; i < quantity; i++)
                {
                    Items.Add(item);
                    Amount = Amount + item.Price;
                    Size++;
                }

                if (item.GetType().Equals(typeof(Monitor)))
                {
                    monitor_counter += quantity;
                }
                else if (item.GetType().Equals(typeof(Keyboard)))
                {
                    keyboard_counter += quantity;
                }
                else if (item.GetType().Equals(typeof(Laptop)))
                {
                    laptop_counter += quantity;
                }
                else if (item.GetType().Equals(typeof(Computer)))
                {
                    computer_counter += quantity;
                }
                else if (item.GetType().Equals(typeof(Chair)))
                {
                    chair_counter += quantity;
                }

            }
        }

        public void DeleteAll() {

            if (Size == 0)
            {
                throw new Exception("Cannot restore empty cart!");
            }
            Size = 0;
            chair_counter = 0;
            computer_counter = 0;
            keyboard_counter = 0;
            laptop_counter = 0;
            monitor_counter = 0;
            Items.Clear();
        }

        public string Print() {
            if (Size == 0) {
                throw new Exception("Cannot print empty cart!");
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Size; i++)
            {
                sb.Append(Items[i]);
            }
            return sb.ToString();
        }

        public void Calculate(String stringDate)
        {
            DateTime date;
            DateTime today = DateTime.Today;
            string[] format = { "yyyy-MM-dd" };
            bool dateFormatOK = DateTime.TryParseExact(stringDate, format, CultureInfo.InvariantCulture,
                       DateTimeStyles.None, out date);
            if (!dateFormatOK) {
                throw new Exception("Wrong date format! Date must be in format yyyy-MM-dd");
            }

            int dayOfTheWeek = (int)date.DayOfWeek; //Sunday is 0
            double days = date.Subtract(today).TotalDays; //pronalazenje broja dana do dostave
            bool isNotWeekend = (dayOfTheWeek == 0 || dayOfTheWeek == 6) ? false : true;


            if (days == 0)
            {
                throw new Exception("Date of delivery can't be today's date!");
            }
            else if (days > 7)
            {
                throw new Exception("Days for delivery must be less than 7!");
            }
            else if (days <= 3)
            {
                if (isNotWeekend && Size > 8 && Amount > 1500 && (computer_counter >= 3 || laptop_counter >= 3) && monitor_counter >= 3)
                {
                    discount = 0.1;
                }
                else if (isNotWeekend && Size > 8 && laptop_counter >= 1 && laptop_counter < 8)
                {
                    discount = 0.08;
                }
                else if (Size > 5 && laptop_counter >= 1 && computer_counter >= 1 && chair_counter >= 1)
                {
                    discount = 0.05;
                }
                else if (Size > 5 && Size < 8 && Amount > 1200)
                {
                    discount = 0.05;
                }

            }
            else if (isNotWeekend && days > 3 && days <= 7)
            {
                if (Size > 8 && Amount > 1500 && (computer_counter >= 3 || laptop_counter >= 3) && monitor_counter >= 3)
                {
                    discount = 0.2;
                }
                else if (Size > 5 && Amount > 1200 && (chair_counter >= 1 || (keyboard_counter >= 1 && monitor_counter >= 1)))
                {
                    discount = 0.18;
                }
                else if (Size > 5 && computer_counter + laptop_counter >= 4)
                {
                    discount = 0.15;
                }
            }

            double price = Amount - (Amount * discount);

            if (price > Budget)
            {
                throw new Exception("Not enough budget!");
            }
            Budget = Budget - price;
        }
    }
}

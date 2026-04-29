using NUnit.Framework;
using System;
using Monitor = OTS_Supermarket.Models.Monitor;

namespace OTS_Supermarket.Test
{
    [TestFixture]
    public class CartTest
    {
        [Test]
        public void AddOneToCart_OneMonitorToCart_Success()
        {
            // ARRANGE
            Cart cart = new Cart();
            Monitor monitor = new Monitor();
            // ACT
            cart.AddOneToCart(monitor);
            // ASSERT
            Assert.That(1, Is.EqualTo(cart.Monitor_counter));
            //Assert.AreEqual(1, cart.Monitor_counter);
        }

        [Test]
        public void AddOneToCart_OneMonitorToCart_Size()
        {
            // ARRANGE
            Cart cart = new Cart();
            Monitor monitor = new Monitor();
            // ACT
            cart.AddOneToCart(monitor);
            // ASSERT
            Assert.That(1, Is.EqualTo(cart.Size));
            //Assert.AreEqual(1, cart.Monitor_counter);
        }

        [Test]
        public void AddOneToCart_OneMonitorToCartSizeAlready10_Exception()
        {
            // ARRANGE
            Cart cart = new Cart();
            Monitor monitor = new Monitor();
            cart.Size = 10;
            // ACT
            Exception exception = Assert.Throws<Exception>(() => cart.AddOneToCart(monitor));
            // ASSERT
            Assert.That(exception.Message, Is.EqualTo("Number of items in cart must be 10 or less!"));
        }

        [TestCase(5, 6)]
        [TestCase(6, 7)]
        public void AddOneToCart_OneMonitorToCart_SuccessDataDriven(int counter, int expectedResult)
        {
            Cart cart = new Cart();
            Monitor monitor = new Monitor();
            cart.Size = counter;

            cart.AddOneToCart(monitor);

            Assert.That(expectedResult, Is.EqualTo(cart.Size));
        }

        [TestCase(5, ExpectedResult = 6)]
        [TestCase(6, ExpectedResult = 7)]
        public int AddOneToCart_OneMonitorToCart_SuccessDataDrivenWithReturnValue(int counter)
        {
            Cart cart = new Cart();
            Monitor monitor = new Monitor();
            cart.Size = counter;
            cart.AddOneToCart(monitor);
            return cart.Size;
        }

        [TestCaseSource(typeof(CartTxtParser), "GetTestCasesData", new object[] { "PICTResults.txt" })]
        public void Test(int size, int amount, int laptop, int monitor, int chair, int computer, int keyboard, string stringDate, double discount )
        {
            // ARRANGE
            Cart cart = new Cart();
            cart.Size = size;
            cart.Monitor_counter = monitor;
            cart.Laptop_counter = laptop;
            cart.Computer_counter = computer;
            cart.Keyboard_counter = keyboard;
            cart.Chair_counter = chair;
            cart.Amount = amount;
            cart.Budget = 100000;

            // ACT
            cart.Calculate(stringDate);

            // ASSERT
            double expectedFinalBudget = 100000 - (amount - amount * discount);
            Assert.That(cart.Budget, Is.EqualTo(expectedFinalBudget).Within(0.001));
        }

    }
}

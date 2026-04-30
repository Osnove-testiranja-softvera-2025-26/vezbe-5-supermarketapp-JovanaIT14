using NUnit.Framework;
using OTS_Supermarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTS_Supermarket.Test
{
    [TestFixture]
    public class CartTest
    {
        [Test]
        public void AddOneToCart_ShouldAddItemToCart_Success()
        {
            //ARRANGE-preduslovi potrebni za metodu koju zelimo da istestiramo
            Cart cart = new Cart();
            cart.Size = 5;
            Monitor monitor = new Monitor();

            //ACT
            cart.AddOneToCart(monitor);

            //ASSERT-prvo ono sto provjeravamo, drugo ono sto ocekujemo
            Assert.That(cart.Size, Is.EqualTo(6));
            Assert.That(cart.Amount, Is.EqualTo(100));
        }

        [Test]
        public void AddOneToCart_WhenCartFull_ShouldThrowException()
        {
            
            Cart cart = new Cart();
            cart.Size = 10;
            Monitor monitor = new Monitor();

           
            var ex = Assert.Throws<Exception>(() => cart.AddOneToCart(monitor));
            Assert.That(ex.Message, Is.EqualTo("Number of items in cart must be 10 or less!"));
        }

        [Test]
        public void AddMultipleToCart_ShouldIncreaseCountersAndAmount()
        {
           
            Cart cart = new Cart();
            Laptop laptop = new Laptop { Price = 500 };

           
            cart.AddMultipleToCart(laptop, 3);

           
            Assert.Multiple(() =>
            {
                Assert.That(cart.Size, Is.EqualTo(3));
                Assert.That(cart.Laptop_counter, Is.EqualTo(3));
                Assert.That(cart.Amount, Is.EqualTo(1500));
            });
        }

        [Test]
        public void DeleteAll_WhenCartIsNotEmpty_ShouldResetCart()
        {
            
            Cart cart = new Cart();
            cart.AddOneToCart(new Monitor());
            cart.AddOneToCart(new Keyboard());

           
            cart.DeleteAll();

            
            Assert.Multiple(() =>
            {
                Assert.That(cart.Size, Is.EqualTo(0));
                Assert.That(cart.Items.Count, Is.EqualTo(0));
                Assert.That(cart.Monitor_counter, Is.EqualTo(0));
            });
        }

        [Test]
        public void DeleteAll_WhenCartIsEmpty_ShouldThrowException()
        {
            
            Cart cart = new Cart();

            var ex = Assert.Throws<Exception>(() => cart.DeleteAll());
            Assert.That(ex.Message, Is.EqualTo("Cannot restore empty cart!"));
        }

        [Test]
        public void Calculate_WrongDateFormat_ShouldThrowException()
        {
           
            Cart cart = new Cart();
            cart.AddOneToCart(new Monitor());

            var ex = Assert.Throws<Exception>(() => cart.Calculate("31.12.2024"));
            Assert.That(ex.Message, Is.EqualTo("Wrong date format! Date must be in format yyyy-MM-dd"));
        }

        [Test]
        public void Calculate_DeliveryDateIsToday_ShouldThrowException()
        {
            
            Cart cart = new Cart();
            cart.AddOneToCart(new Monitor());
            string today = DateTime.Today.ToString("yyyy-MM-dd");

         
            var ex = Assert.Throws<Exception>(() => cart.Calculate(today));
            Assert.That(ex.Message, Is.EqualTo("Date of delivery can't be today's date!"));
        }

        [Test]
        public void Calculate_NotEnoughBudget_ShouldThrowException()
        {
            
            Cart cart = new Cart();
            cart.Budget = 50;
            cart.AddOneToCart(new Monitor { Price = 100 });
            string deliveryDate = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");

           
            var ex = Assert.Throws<Exception>(() => cart.Calculate(deliveryDate));
            Assert.That(ex.Message, Is.EqualTo("Not enough budget!"));
        }

        [Test]
        public void Calculate_ApplyBigDiscount_ShouldUpdateBudgetCorrectly()
        {
            
            Cart cart = new Cart { Budget = 2000 };
            DateTime futureDate = DateTime.Today.AddDays(5);
            
            if (futureDate.DayOfWeek == DayOfWeek.Saturday) futureDate = futureDate.AddDays(2);
            if (futureDate.DayOfWeek == DayOfWeek.Sunday) futureDate = futureDate.AddDays(1);

            cart.Amount = 1600;
            cart.Size = 9;
            cart.Computer_counter = 3;
            cart.Monitor_counter = 3;

           
            cart.Calculate(futureDate.ToString("yyyy-MM-dd"));

            
            Assert.That(cart.Budget, Is.EqualTo(720));
        }

        [Test]
        public void Calculate_ApplyComboDiscount_ShouldApply5Percent()
        {
           
            Cart cart = new Cart { Budget = 1000 };
            cart.Size = 6;
            cart.Amount = 500;
            cart.Laptop_counter = 1;
            cart.Computer_counter = 1;
            cart.Chair_counter = 1;
            string deliveryDate = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd");

           
            cart.Calculate(deliveryDate);

           
            Assert.That(cart.Budget, Is.EqualTo(525));
        }

        [Test]
        public void Print_WhenCartIsEmpty_ShouldThrowException()
        {
            
            Cart cart = new Cart();

            
            var ex = Assert.Throws<Exception>(() => cart.Print());
            Assert.That(ex.Message, Is.EqualTo("Cannot print empty cart!"));
        }
    }


}

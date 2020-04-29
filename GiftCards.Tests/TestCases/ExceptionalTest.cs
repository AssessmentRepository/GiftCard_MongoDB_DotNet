using GiftCards.BusinessLayer;
using GiftCards.DataLayer;
using GiftCards.Entities;
using GiftCards.Tests.Exceptions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Pixogram.Tests.TestCases
{
    public class ExceptionalTest
    {
        private Mock<IMongoCollection<Gift>> _mockCollection;
        private Mock<IMongoCollection<Buyer>> _buyermockCollection;
        private Mock<IMongoDBContext> _mockContext;
        private Gift _gift;
        private Buyer _buyer;
        private ContactUs _contactUs;
        private Mock<IOptions<Mongosettings>> _mockOptions;


        public ExceptionalTest()
        {
            _gift = new Gift
            {
               GiftName = "Toys",
               Ammount=500
            };
            _contactUs = new ContactUs
            {
                Name = "John",
                PhoneNumber = 9888076466,
                Email = "jjj@gmail.com",
                Address = "Paris"
            };
            _buyer = new Buyer
            {
                FirstName = "abc",
                LastName = "abc",
                PhoneNumber = 123456789,
                Email = "bbb@gmail.com",

                Address = "123456789",

            };
            _mockCollection = new Mock<IMongoCollection<Gift>>();
            _mockCollection.Object.InsertOne(_gift);
            _buyermockCollection = new Mock<IMongoCollection<Buyer>>();
            _buyermockCollection.Object.InsertOne(_buyer);
            _mockContext = new Mock<IMongoDBContext>();
            //MongoSettings initialization
            _mockOptions = new Mock<IOptions<Mongosettings>>();

        }
        Mongosettings settings = new Mongosettings()
        {
            Connection = "mongodb://localhost:27017",
            DatabaseName = "guestbook"
        };


        [Fact]
        public async void CreateNewGift_Null_Failure()
        {
            // Arrange
            _gift = null;

            //Act 
            var bookRepo = new GiftRepository(_mockContext.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => bookRepo.Create(_gift));
        }

        [Fact]
        public async void CreateNewBuyer_Null_Buyer_Failure()
        {
            // Arrange
            _buyer = null;

            //Act 
            var bookRepo = new BuyerRepository(_mockContext.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => bookRepo.Create(_buyer));
        }

        [Fact]
        public async void CreateNewContactUs_Null_Failure()
        {
            // Arrange
           ContactUs _contactUs = null;

            //Act 
            var bookRepo = new ContactUsRepository(_mockContext.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => bookRepo.Create(_contactUs));
        }


        [Fact]
        public async Task ExceptionTestFor_ValidCreate()
        {
            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoDBContext(_mockOptions.Object);
            var userRepo = new ContactUsRepository(context);

            //Act
            //Assert
            var ex = await Assert.ThrowsAsync<ContactUsAlreadyExist>(async () => await userRepo.Create(_contactUs));
            Assert.Equal("Already Exist", ex.Messages);

        }

        [Fact]
        public async Task ExceptionTestFor_GiftNotFound()
        {
            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoDBContext(_mockOptions.Object);
            var buyerRepo = new BuyerRepository(context);

            var ex = await Assert.ThrowsAsync<GiftNotFoundException>(() => buyerRepo.SearchGift(_gift.GiftName));

            Assert.Equal("Gift Not Found", ex.Messages);

        }

    }
}

using GiftCards.BusinessLayer;
using GiftCards.DataLayer;
using GiftCards.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Pixogram.Tests.TestCases
{
    public class BusinessLogicTests
    {
        private Mock<IMongoCollection<Buyer>> _mockCollection;
        private Mock<IMongoCollection<ContactUs>> _contactmockCollection;
        private Mock<IMongoCollection<GiftOrder>> _giftOrdercontactmockCollection;
        private Mock<IMongoCollection<Gift>> _giftmockCollection;
        private Mock<IMongoDBContext> _mockContext;
        private Buyer _buyer;
        private ContactUs _contactUs;
        private GiftOrder _giftOrder;
        private Gift _gift;

        private readonly IList<Buyer> _list;
        private readonly IList<ContactUs> _contactlist;
        private readonly IList<GiftOrder> _orederlist;
        // MongoSettings declaration
        private Mock<IOptions<Mongosettings>> _mockOptions;


        public BusinessLogicTests()
        {
            _buyer = new Buyer
            {
                FirstName = "abc",
                LastName = "abc",
                PhoneNumber = 123456789,
                Email = "bbb@gmail.com",

                Address = "123456789",
            };

            _contactUs = new ContactUs
            {
                Name = "John",
                PhoneNumber = 9888076466,
                Email = "jjj@gmail.com",
                Address = "Paris"
            };
            _gift = new Gift
            {
                GiftName = "Toys",
                Ammount = 500
            };
            _giftOrder = new GiftOrder
            {
                OrderedGift = _gift,
                GiftBuyer = _buyer
            };
            _mockCollection = new Mock<IMongoCollection<Buyer>>();
            _mockCollection.Object.InsertOne(_buyer);


            _contactmockCollection = new Mock<IMongoCollection<ContactUs>>();
            _contactmockCollection.Object.InsertOne(_contactUs);
            _giftOrdercontactmockCollection = new Mock<IMongoCollection<GiftOrder>>();
            _giftOrdercontactmockCollection.Object.InsertOne(_giftOrder);
            _giftmockCollection = new Mock<IMongoCollection<Gift>>();
            _giftmockCollection.Object.InsertOne(_gift);


            _contactlist = new List<ContactUs>();
            _contactlist.Add(_contactUs);

            _orederlist = new List<GiftOrder>();
            _orederlist.Add(_giftOrder);
            _mockContext = new Mock<IMongoDBContext>();
            //MongoSettings initialization
            _mockOptions = new Mock<IOptions<Mongosettings>>();
            _list = new List<Buyer>();
            _list.Add(_buyer);
        }
        Mongosettings settings = new Mongosettings()
        {
            Connection = "mongodb://localhost:27017",
            DatabaseName = "guestbook"
        };


        [Fact]
        public async Task TestFor_validAmount()
        {
            //mocking
            _giftmockCollection.Setup(op => op.InsertOneAsync(_gift, null,
            default(CancellationToken))).Returns(Task.CompletedTask);
            _mockContext.Setup(c => c.GetCollection<Gift>(typeof(Gift).Name)).Returns(_giftmockCollection.Object);

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoDBContext(_mockOptions.Object);
            var userRepo = new GiftRepository(context);

            //Act
            await userRepo.Create(_gift);
            var result = await userRepo.Get(_gift.GiftId);

            var Min = 100;
            var Max = 1000000;

            //Action
            var actual = _gift.Ammount;

            //Assert
            Assert.InRange(result.Ammount, Min, Max);
            Assert.InRange(actual, Min, Max);
        }
        [Fact]
        public async Task Testfor_ValidEmailAsync()
        {
            //mocking
            _contactmockCollection.Setup(op => op.InsertOneAsync(_contactUs, null,
            default(CancellationToken))).Returns(Task.CompletedTask);
            _mockContext.Setup(c => c.GetCollection<ContactUs>(typeof(ContactUs).Name)).Returns(_contactmockCollection.Object);

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoDBContext(_mockOptions.Object);
            var userRepo = new ContactUsRepository(context);

            //Act
            await userRepo.Create(_contactUs);
            var result = await userRepo.Get(_contactUs.ContactUsId);

            ////Action
            bool CheckEmail = Regex.IsMatch(result.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            bool isEmail = Regex.IsMatch(_contactUs.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

            //Assert
            Assert.True(isEmail);
            Assert.True(CheckEmail);
        }

    }
}

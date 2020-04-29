using GiftCards.BusinessLayer;
using GiftCards.DataLayer;
using GiftCards.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
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
  public  class BoundaryTest
    {
        private Mock<IMongoCollection<Buyer>> _mockCollection;
        private Mock<IMongoDBContext> _mockContext;
        private Buyer _buyer;
        private readonly IList<Buyer> _list;
        // MongoSettings declaration
        private Mock<IOptions<Mongosettings>> _mockOptions;

        public BoundaryTest()
        {
        _buyer = new Buyer
            {
              FirstName="XYZ",
              LastName="abc",
                 PhoneNumber= 123456789,
                Email = "bbb@gmail.com",
               
                Address = "123456789",
             
            };
            _mockCollection = new Mock<IMongoCollection<Buyer>>();
            _mockCollection.Object.InsertOne(_buyer);
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
        public async Task BoundaryTestfor_ValidUserEmailAsync()
        {
            //mocking
            _mockCollection.Setup(op => op.InsertOneAsync(_buyer, null,
            default(CancellationToken))).Returns(Task.CompletedTask);
            _mockContext.Setup(c => c.GetCollection<Buyer>(typeof(Buyer).Name)).Returns(_mockCollection.Object);

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoDBContext(_mockOptions.Object);
            var userRepo = new BuyerRepository(context);

            //Act
            await userRepo.Create(_buyer);
            var result = await userRepo.Get(_buyer.BuyerId);

            ////Action
            //  var getregisteredUser = await _userServices.GetUser(user.UserName);
            bool CheckEmail = Regex.IsMatch(result.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            bool isEmail = Regex.IsMatch(_buyer.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

            //Assert
            Assert.True(isEmail);
            Assert.True(CheckEmail);
        }


        [Fact]
        public async Task BoundaryTestFor_validUserNameLengthAsync()
        {
            //mocking
            _mockCollection.Setup(op => op.InsertOneAsync(_buyer, null,
            default(CancellationToken))).Returns(Task.CompletedTask);
            _mockContext.Setup(c => c.GetCollection<Buyer>(typeof(Buyer).Name)).Returns(_mockCollection.Object);

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoDBContext(_mockOptions.Object);
            var userRepo = new BuyerRepository(context);

            //Act
            await userRepo.Create(_buyer);
            var result = await userRepo.Get(_buyer.BuyerId);

            var MinLength = 3;
            var MaxLength = 50;

            //Action
            var actualLength = _buyer.FirstName.Length;

            //Assert
            Assert.InRange(result.FirstName.Length, MinLength, MaxLength);
            Assert.InRange(actualLength, MinLength, MaxLength);
        }

        [Fact]
        public async Task BoundaryTestfor_ValidUserNameAsync()
        {
            //mocking
            _mockCollection.Setup(op => op.InsertOneAsync(_buyer, null,
            default(CancellationToken))).Returns(Task.CompletedTask);
            _mockContext.Setup(c => c.GetCollection<Buyer>(typeof(Buyer).Name)).Returns(_mockCollection.Object);

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoDBContext(_mockOptions.Object);
            var userRepo = new BuyerRepository(context);

            //Act
            await userRepo.Create(_buyer);
            var result = await userRepo.Get(_buyer.BuyerId);

            bool getisUserName = Regex.IsMatch(result.FirstName, @"^[a-zA-Z0-9]{4,10}$", RegexOptions.IgnoreCase);
            bool isUserName = Regex.IsMatch(_buyer.FirstName, @"^[a-zA-Z0-9]{4,10}$", RegexOptions.IgnoreCase);
            //Assert
            Assert.True(isUserName);
            Assert.True(getisUserName);
        }

        [Fact]
        public async Task BoundaryTestfor_ValidId()
        {
            //mocking
            _mockCollection.Setup(op => op.InsertOneAsync(_buyer, null,
            default(CancellationToken))).Returns(Task.CompletedTask);
            _mockContext.Setup(c => c.GetCollection<Buyer>(typeof(Buyer).Name)).Returns(_mockCollection.Object);

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoDBContext(_mockOptions.Object);
            var userRepo = new BuyerRepository(context);

            //Act
            await userRepo.Create(_buyer);
            var result = await userRepo.Get(_buyer.BuyerId);

            Assert.InRange(_buyer.BuyerId.Length, 20, 30);

        }

        [Fact]
        public async Task BoundaryTestFor_PhoneNumberLengthAsync()
        {
            //mocking
            _mockCollection.Setup(op => op.InsertOneAsync(_buyer, null,
            default(CancellationToken))).Returns(Task.CompletedTask);
            _mockContext.Setup(c => c.GetCollection<Buyer>(typeof(Buyer).Name)).Returns(_mockCollection.Object);

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoDBContext(_mockOptions.Object);
            var userRepo = new BuyerRepository(context);

            //Act
            await userRepo.Create(_buyer);
            var result = await userRepo.Get(_buyer.BuyerId);

            var MinLength = 10;
            var MaxLength = 10;

            //Action
            var actualLength = _buyer.PhoneNumber.ToString().Length;

            //Assert
            Assert.InRange(result.PhoneNumber.ToString().Length, MinLength, MaxLength);
            Assert.InRange(actualLength, MinLength, MaxLength);
        }

    }
}

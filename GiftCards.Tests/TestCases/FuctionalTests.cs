using GiftCards.BusinessLayer;
using GiftCards.DataLayer;
using GiftCards.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Pixogram.Tests.TestCases
{
    public class FuctionalTests
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


        public FuctionalTests()
        {
            _buyer = new Buyer
            {
                FirstName = "abc",
                LastName = "abc",
                PhoneNumber = 123456789,
                Email = "bbb@gmail.com",

                Address = "123456789",
            };

            _contactUs =new ContactUs
            {
            Name="John",
            PhoneNumber=9888076466,
            Email="jjj@gmail.com",
            Address="Paris"
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
        public async Task GetAllUsers()
        {
            //Arrange
            //Mock MoveNextAsync
            Mock<IAsyncCursor<Buyer>> _userCursor = new Mock<IAsyncCursor<Buyer>>();
            _userCursor.Setup(_ => _.Current).Returns(_list);
            _userCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            //Mock FindSync
            _mockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Buyer>>(),
            It.IsAny<FindOptions<Buyer, Buyer>>(),
             It.IsAny<CancellationToken>())).Returns(_userCursor.Object);

            //Mock GetCollection
            _mockContext.Setup(c => c.GetCollection<Buyer>(typeof(Buyer).Name)).Returns(_mockCollection.Object);

            //Jayanth Creating one more instance of DB

            _mockOptions.Setup(s => s.Value).Returns(settings);

            // Creating one more instance of DB
            // Passing _mockOptions instaed of _mockContext
            var context = new MongoDBContext(_mockOptions.Object);

            var userRepo = new BuyerRepository(context);

            //Act
            var result = await userRepo.GetAll();

            //Assert 
            //loop only first item and assert
            foreach (Buyer user in result)
            {
                Assert.NotNull(user);
                Assert.Equal(user.FirstName, _buyer.FirstName);
                Assert.Equal(user.Email, _buyer.Email);
                break;
            }
        }


        [Fact]
        public async void TestFor_CreateNewUser()
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

            //Assert
            Assert.Equal(_buyer.FirstName, result.FirstName);
        }
        [Fact]
        public async Task TestFor_GetBuyerById()
        {
            //Arrange

            //mocking
            _mockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Buyer>>(),
            It.IsAny<FindOptions<Buyer, Buyer>>(),
             It.IsAny<CancellationToken>()));

            _mockContext.Setup(c => c.GetCollection<Buyer>(typeof(Buyer).Name));

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoDBContext(_mockOptions.Object);
            var userRepo = new BuyerRepository(context);

            //Act
            await userRepo.Create(_buyer);
            var result = await userRepo.Get(_buyer.BuyerId);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllContactUs()
        {
            //Arrange
            //Mock MoveNextAsync
            Mock<IAsyncCursor<ContactUs>> _userCursor = new Mock<IAsyncCursor<ContactUs>>();
            _userCursor.Setup(_ => _.Current).Returns(_contactlist);
            _userCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            //Mock FindSync
            _contactmockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<ContactUs>>(),
            It.IsAny<FindOptions<ContactUs, ContactUs>>(),
             It.IsAny<CancellationToken>())).Returns(_userCursor.Object);

            //Mock GetCollection
            _mockContext.Setup(c => c.GetCollection<ContactUs>(typeof(ContactUs).Name)).Returns(_contactmockCollection.Object);

            //Jayanth Creating one more instance of DB

            _mockOptions.Setup(s => s.Value).Returns(settings);

            // Creating one more instance of DB
            // Passing _mockOptions instaed of _mockContext
            var context = new MongoDBContext(_mockOptions.Object);

            var ContactUsRepo = new ContactUsRepository(context);

            //Act
            var result = await ContactUsRepo.AllContactUs();

            //Assert 
            //loop only first item and assert
            foreach (ContactUs user in result)
            {
                Assert.NotNull(user);
                Assert.Equal(user.Name, _contactUs.Name);
                Assert.Equal(user.Email, _contactUs.Email);
                break;
            }
        }

        [Fact]
        public async Task ViewGiftCardOrders()
        {
            //Arrange
            //Mock MoveNextAsync
            Mock<IAsyncCursor<GiftOrder>> _userCursor = new Mock<IAsyncCursor<GiftOrder>>();
            _userCursor.Setup(_ => _.Current).Returns(_orederlist);
            _userCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);

            //Mock FindSync
            _giftOrdercontactmockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<GiftOrder>>(),
            It.IsAny<FindOptions<GiftOrder, GiftOrder>>(),
             It.IsAny<CancellationToken>())).Returns(_userCursor.Object);

            //Mock GetCollection
            _mockContext.Setup(c => c.GetCollection<GiftOrder>(typeof(GiftOrder).Name)).Returns(_giftOrdercontactmockCollection.Object);

            //Jayanth Creating one more instance of DB

            _mockOptions.Setup(s => s.Value).Returns(settings);

            // Creating one more instance of DB
            // Passing _mockOptions instaed of _mockContext
            var context = new MongoDBContext(_mockOptions.Object);

            var ContactUsRepo = new GiftOrderRepository(context);

            //Act
            var result = await ContactUsRepo.ViewGiftCardOrders();

            //Assert 
            //loop only first item and assert
            foreach (GiftOrder user in result)
            {
                Assert.NotNull(user);
                break;
            }
        }

        [Fact]
        public async Task TestFoR_PlaceOrder()
        {
            //mocking
            _giftOrdercontactmockCollection.Setup(op => op.InsertOneAsync(_giftOrder, null,
            default(CancellationToken))).Returns(Task.CompletedTask);
            _mockContext.Setup(c => c.GetCollection<GiftOrder>(typeof(GiftOrder).Name)).Returns(_giftOrdercontactmockCollection.Object);

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoDBContext(_mockOptions.Object);
            var userRepo = new GiftRepository(context);
            var orderRepo = new GiftOrderRepository(context);

            //Act
            await userRepo.PlaceOrderAsync(_buyer,_gift.GiftId);

            var result = await orderRepo.Get(_giftOrder.GiftOrderId);

            //Assert
            Assert.Equal(_buyer, result.GiftBuyer);
            Assert.Equal(_gift, result.OrderedGift);
        }


        [Fact]
        public async Task TestFor_SearchGiftByNamme()
        {
            //Arrange

            //mocking
            _giftmockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Gift>>(),
            It.IsAny<FindOptions<Gift, Gift>>(),
             It.IsAny<CancellationToken>()));

            _mockContext.Setup(c => c.GetCollection<Gift>(typeof(Gift).Name));

            //Craetion of new Db
            _mockOptions.Setup(s => s.Value).Returns(settings);
            var context = new MongoDBContext(_mockOptions.Object);
            var userRepo = new GiftRepository(context);

            //Act
            await userRepo.Create(_gift);
            var result = await userRepo.Get(_gift.GiftName);

            //Assert
            Assert.NotNull(result);
        }




        
     
        

        //[Fact]
        //public async Task TestFor_UpDate()
        //{
        //    //Arrange
        //    _mockCollection.Setup(s => s.UpdateOneAsync(It.IsAny<FilterDefinition<User>>(), It.IsAny<UpdateDefinition<User>>(), It.IsAny<UpdateOptions>(), It.IsAny<CancellationToken>()));
        //    //mocking
        //    _mockContext.Setup(c => c.GetCollection<User>(typeof(User).Name)).Returns(_mockCollection.Object);

        //    //Craetion of new Db
        //    _mockOptions.Setup(s => s.Value).Returns(settings);
        //    var context = new MongoUserDBContext(_mockOptions.Object);
        //    var userRepo = new UserRepository(context);

        //    //Act
        //    await userRepo.Create(_user);
        //    userRepo.Update(_user);
        //    var result = await userRepo.Get(_user.Id);

        //    //Assert
        //    Assert.Equal(_user.UserName, result.UserName);

        //}

        //[Fact]
        //public async Task TestFor_DeleteUser()
        //{
        //    //Arrange

        //    //mocking
        //    _mockCollection.Setup(op => op.FindOneAndDelete(_user.Id, null, default(CancellationToken)));
        //    _mockContext.Setup(c => c.GetCollection<User>(typeof(User).Name));

        //    //Craetion of new Db
        //    _mockOptions.Setup(s => s.Value).Returns(settings);
        //    var context = new MongoUserDBContext(_mockOptions.Object);
        //    var userRepo = new UserRepository(context);

        //    //Act
        //    await userRepo.Create(_user);
        //    userRepo.Delete(_user.Id);
        //    var result = await userRepo.Get(_user.Id);

        //    //Assert
        //    Assert.Null(result);

        //}


        //[Fact]
        //public async Task TestFor_ResetPassword()
        //{
        //    //Arrange
        //    string Password = "bb123";

        //    //mocking
        //    _mockCollection.Setup(op => op.InsertOneAsync(_user, null,
        //    default(CancellationToken))).Returns(Task.CompletedTask);
        //    _mockContext.Setup(c => c.GetCollection<User>(typeof(User).Name));

        //    //Craetion of new Db
        //    _mockOptions.Setup(s => s.Value).Returns(settings);
        //    var context = new MongoUserDBContext(_mockOptions.Object);
        //    var userRepo = new UserRepository(context);


        //    //Act
        //    await userRepo.ResetPassword(_user.Id, Password);
        //    var result = await userRepo.Get(_user.Id);

        //    //Assert
        //    Assert.Equal(Password, result.Password);
        //}

        //[Fact]
        //public async Task TestFor_GetProfile()
        //{
        //    //Arrange
        //    //mocking
        //    _mockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<User>>(),
        //    It.IsAny<FindOptions<User, User>>(),
        //     It.IsAny<CancellationToken>()));

        //    //Craetion of new Db
        //    _mockOptions.Setup(s => s.Value).Returns(settings);
        //    var context = new MongoUserDBContext(_mockOptions.Object);
        //    var userRepo = new UserRepository(context);

        //    //Act
        //    var result = await userRepo.GetProfile(_user.Id);

        //    //Assert
        //    Assert.NotNull(result);
        //}

        //[Fact]
        //public async Task TestFor_UpDateUserProfile()
        //{
        //    //Arrange

        //    //mocking
        //    _mockCollection.Setup(s => s.UpdateOneAsync(It.IsAny<FilterDefinition<User>>(), It.IsAny<UpdateDefinition<User>>(), It.IsAny<UpdateOptions>(), It.IsAny<CancellationToken>()));
        //    _mockContext.Setup(c => c.GetCollection<User>(typeof(User).Name)).Returns(_mockCollection.Object);

        //    //Craetion of new Db
        //    _mockOptions.Setup(s => s.Value).Returns(settings);
        //    var context = new MongoUserDBContext(_mockOptions.Object);
        //    var userRepo = new UserRepository(context);

        //    //Act
        //    userRepo.Update(_user);
        //    var result = await userRepo.Get(_user.Id);

        //    //Assert
        //    Assert.Equal(_user, result);

        //}

        //[Fact]
        //public async Task TestFor_AddContent()
        //{
        //    //Arrange
        //    //mocking
        //   _ContentMockCollection.Setup(op => op.InsertOneAsync(_content, null,
        //    default(CancellationToken))).Returns(Task.CompletedTask);
        //    _mockContext.Setup(c => c.GetCollection<Content>(typeof(Content).Name)).Returns(_ContentMockCollection.Object);

        //    //Craetion of new Db
        //    _mockOptions.Setup(s => s.Value).Returns(settings);
        //    var context = new MongoUserDBContext(_mockOptions.Object);
        //    var userRepo = new UserRepository(context);
        //    //mocking


        //    //Act
        //    var updated = userRepo.AddContent(contentslist, _user.Id);
        //    var result = await userRepo.GetAllContent(_user.Id,contentslist);

        //    //Assert
        //    Assert.NotNull(result);

        //}

        //[Fact]
        //public async Task TestFor_OrganizeImage()
        //{
        //    //Arrange
        //    //Arrange
        //    //Mock MoveNextAsync
        //    Mock<IAsyncCursor<Content>> _userCursor = new Mock<IAsyncCursor<Content>>();
        //    _userCursor.Setup(_ => _.Current).Returns(contentslist);
        //    _userCursor
        //        .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
        //        .Returns(true)
        //        .Returns(false);

        //    //Mock FindSync
        //    _ContentMockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Content>>(),
        //    It.IsAny<FindOptions<Content, Content>>(),
        //     It.IsAny<CancellationToken>())).Returns(_userCursor.Object);

        //    //Mock GetCollection
        //    _mockContext.Setup(c => c.GetCollection<Content>(typeof(Content).Name)).Returns(_ContentMockCollection.Object);

        //    _mockOptions.Setup(s => s.Value).Returns(settings);
        //    var context = new MongoUserDBContext(_mockOptions.Object);
        //    var userRepo = new UserRepository(context);


        //  //  Act
        //    var updated = await userRepo.OrganizeImage(_user.Id, contentslist);

        //  //  Assert
        //    Assert.NotNull(updated);

        //}

        //[Fact]
        //public async Task TestFor_OrganizeVideo()
        //{
        //    //Arrange

        //    //Mock MoveNextAsync
        //    Mock<IAsyncCursor<Content>> _userCursor = new Mock<IAsyncCursor<Content>>();
        //    _userCursor.Setup(_ => _.Current).Returns(contentslist);
        //    _userCursor
        //        .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
        //        .Returns(true)
        //        .Returns(false);

        //    //Mock FindSync
        //    _ContentMockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Content>>(),
        //    It.IsAny<FindOptions<Content, Content>>(),
        //     It.IsAny<CancellationToken>())).Returns(_userCursor.Object);

        //    //Mock GetCollection
        //    _mockContext.Setup(c => c.GetCollection<Content>(typeof(Content).Name)).Returns(_ContentMockCollection.Object);

        //    _mockOptions.Setup(s => s.Value).Returns(settings);
        //    var context = new MongoUserDBContext(_mockOptions.Object);
        //    var userRepo = new UserRepository(context);

        //    //Act
        //    var updated = await userRepo.OrganizeVideo(_user.Id, contentslist);

        //    //Assert
        //    Assert.NotNull(updated);

        //}

        //[Fact]
        //public async Task TestFor_GetAllContents()
        //{
        //    //Arrange
        //    //Mock MoveNextAsync
        //    Mock<IAsyncCursor<Content>> _userCursor = new Mock<IAsyncCursor<Content>>();
        //    _userCursor.Setup(_ => _.Current).Returns(contentslist);
        //    _userCursor
        //        .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
        //        .Returns(true)
        //        .Returns(false);

        //    //Mock FindSync
        //    _ContentMockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Content>>(),
        //    It.IsAny<FindOptions<Content, Content>>(),
        //     It.IsAny<CancellationToken>())).Returns(_userCursor.Object);

        //    //Mock GetCollection
        //    _mockContext.Setup(c => c.GetCollection<Content>(typeof(Content).Name)).Returns(_ContentMockCollection.Object);

        //    _mockOptions.Setup(s => s.Value).Returns(settings);
        //    var context = new MongoUserDBContext(_mockOptions.Object);
        //    var userRepo = new UserRepository(context);

        //    //Act
        //    var updated = await userRepo.GetAllContent(_user.Id, contentslist);

        //    //Assert
        //    Assert.NotNull(updated);

        //}

        //[Fact]
        //public async Task TestFor_UpdateContent()
        //{
        //    //Arrange

        //    //mocking
        //    _ContentMockCollection.Setup(s => s.UpdateOneAsync(It.IsAny<FilterDefinition<Content>>(), It.IsAny<UpdateDefinition<Content>>(), It.IsAny<UpdateOptions>(), It.IsAny<CancellationToken>()));
        //    _mockContext.Setup(c => c.GetCollection<Content>(typeof(Content).Name)).Returns(_ContentMockCollection.Object);

        //    //Craetion of new Db
        //    _mockOptions.Setup(s => s.Value).Returns(settings);
        //    var context = new MongoUserDBContext(_mockOptions.Object);
        //    var userRepo = new UserRepository(context);

        //    //Act

        //    var updatedContent = await userRepo.UpdateContent(_user.Id, _content);
        //   // var result = await userRepo.Get(_content.Id);

        //    //Assert
        //    Assert.NotNull(updatedContent);
        //}
        //[Fact]
        //public async Task TestFor_AddComment()
        //{
        //    //Arrange

        //    //mocking
        //    _feedbackmMockCollection.Setup(op => op.InsertOneAsync(_feedback, null,
        //    default(CancellationToken))).Returns(Task.CompletedTask);
        //    _mockContext.Setup(c => c.GetCollection<Feedback>(typeof(Feedback).Name)).Returns(_feedbackmMockCollection.Object);

        //    //Craetion of new Db
        //    _mockOptions.Setup(s => s.Value).Returns(settings);
        //    var context = new MongoUserDBContext(_mockOptions.Object);
        //    var userRepo = new UserRepository(context);

        //    //Act

        //    var updatedComment = await userRepo.AddComment(_feedback);

        //    //Assert
        //    Assert.Equal(_feedback, updatedComment);
        //}
        //[Fact]
        //public async Task TestFor_FollowUser()
        //{
        //    //Arrange
        //   var _senderusers = new User
        //    {
        //        FirstName = "Baby",
        //        LastName = "Bab",
        //        UserName = "bb",
        //        Email = "bbb@gmail.com",
        //        Password = "123456789",
        //        ConfirmPassword = "123456789",
        //        ProfilePicture = "Pho"
        //    };

        //    //Craetion of new Db
        //   // _mockOptions.Setup(s => s.Value).Returns(settings);
        //    var context = new MongoUserDBContext(_mockOptions.Object);
        //    var userRepo = new UserRepository(context);

        //    //Act

        //    var isFollowed = await userRepo.FollowUser(_user.Id, _senderusers.Id);

        //    //Assert
        //    Assert.True(isFollowed);
        //}

        //[Fact]
        //public async Task TestFor_FollowList()
        //{
        //    //Arrange
        //    //Mock MoveNextAsync
        //    Mock<IAsyncCursor<Follow>> _userCursor = new Mock<IAsyncCursor<Follow>>();
        //    _userCursor.Setup(_ => _.Current).Returns(followedlist);
        //    _userCursor
        //        .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
        //        .Returns(true)
        //        .Returns(false);

        //    //Mock FindSync
        //    _followMockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<Follow>>(),
        //    It.IsAny<FindOptions<Follow, Follow>>(),
        //     It.IsAny<CancellationToken>())).Returns(_userCursor.Object);

        //    //Mock GetCollection
        //    _mockContext.Setup(c => c.GetCollection<Follow>(typeof(Follow).Name)).Returns(_followMockCollection.Object);

        //    _mockOptions.Setup(s => s.Value).Returns(settings);
        //    var context = new MongoUserDBContext(_mockOptions.Object);
        //    var userRepo = new UserRepository(context);

        //    //Act
        //    var listOfFollewers = await userRepo.FollowList(_user.Id);

        //    //Assert
        //    Assert.NotNull(listOfFollewers);
        //}

        //[Fact]
        //public async Task TestFor_HideMedia()
        //{
        //    //mocking
        //    //  Craetion of new Db
        //    var context = new MongoUserDBContext(_mockOptions.Object);
        //    var userRepo = new UserRepository(context);

        //  //  Act
        //    var IsHided = await userRepo.HideMedia(_content.Image,_content.Visibility,_content.Video);

        //    //Assert
        //    Assert.True(IsHided);
        //}


        //[Fact]
        //public async Task TestFor_ActivityLog()
        //{
        //    //Arrange

        //    Mock<IAsyncCursor<ILog>> _userCursor = new Mock<IAsyncCursor<ILog>>();
        //    _userCursor.Setup(_ => _.Current).Returns(_loglist);
        //    _userCursor
        //        .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
        //        .Returns(true)
        //        .Returns(false);

        //    //Mock FindSync
        //    _iLogmockCollection.Setup(op => op.FindSync(It.IsAny<FilterDefinition<ILog>>(),
        //    It.IsAny<FindOptions<ILog, ILog>>(),
        //     It.IsAny<CancellationToken>())).Returns(_userCursor.Object);

        //    //Mock GetCollection
        //    _mockContext.Setup(c => c.GetCollection<ILog>(typeof(ILog).Name)).Returns(_iLogmockCollection.Object);

        //    _mockOptions.Setup(s => s.Value).Returns(settings);
        //    var context = new MongoUserDBContext(_mockOptions.Object);

        //    var userRepo = new UserRepository(context);
        //    //Act

        //    var ILog = await userRepo.ActivityLog(_user.Id);

        //    //Assert
        //    Assert.NotNull(ILog);
        //}


    }
}

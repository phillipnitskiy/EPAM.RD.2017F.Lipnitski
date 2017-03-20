namespace ServiceLibrary.Tests
{
    using System;
    using System.Linq;
    using Moq;
    using ServiceLibrary;
    using NUnit.Framework;
    using System.Collections.Generic;
    using Interfaces;

    [TestFixture]
    public class UserStorageTests
    {
        private IMasterUserStorage userStorage;
        private Mock<IUserIdGenerator> userIdGenerator;
        private Mock<IUserValidator> userValidator;

        [SetUp]
        public void BeforeStart()
        {
            int i = 0;
            this.userIdGenerator = new Mock<IUserIdGenerator>();
            this.userIdGenerator.Setup(uIdG => uIdG.Generate(It.IsAny<User>()))
                .Returns<User>((u) => ++i);

            this.userValidator = new Mock<IUserValidator>();
            this.userValidator.Setup(uV => uV.Validate(It.IsAny<User>()))
                .Returns<User>((u) => true);

            this.userStorage = new MasterUserStorage(this.userIdGenerator.Object, this.userValidator.Object);
        }

        [Test]
        public void ConstructorWithTwoParams_NullUserIdGenerator_ExceptionThrown()
        {
            IUserIdGenerator nullUserIdGenerator = null;

            TestDelegate testDelegate = () => new UserStorage(nullUserIdGenerator, null);

            Assert.Catch<ArgumentNullException>(testDelegate);
        }

        [Test]
        public void Add_NullUser_ExceptionThrown()
        {
            User nullUser = null;

            TestDelegate testDelegate = () => this.userStorage.Add(nullUser);

            Assert.Catch<ArgumentNullException>(testDelegate);
        }

        [Test]
        public void Add_UserValidatorExecution_Executed()
        {
            User defaultUser = new User();
            this.userValidator.Setup(uV => uV.Validate(It.IsAny<User>()))
                .Returns<User>((u) => true).Verifiable();

            this.userStorage.Add(defaultUser);

            this.userValidator.Verify();
        }

        [Test]
        public void Add_UserIdGenerationExecution_Executed()
        {
            User defaultUser = new User();
            int i = 0;
            this.userIdGenerator.Setup(uIdG => uIdG.Generate(It.IsAny<User>()))
                .Returns<User>((u) => ++i).Verifiable();

            this.userStorage.Add(defaultUser);

            this.userIdGenerator.Verify();
        }

        [Test]
        public void Add_InvalidUser_ExceptionThrown()
        {
            this.userValidator.Setup(uV => uV.Validate(It.IsAny<User>()))
                .Returns<User>((u) => false);
            User invalidUser = new User { FirstName = "Richard", LastName = "Richardson", DateOfBirth = DateTime.Now };

            TestDelegate testDelegate = () => this.userStorage.Add(invalidUser);

            Assert.Catch<ArgumentException>(testDelegate);
        }

        [Test]
        public void Delete_UserWhichNotExist_ExceptionThrown()
        {
            User user = new User { FirstName = "Richard", LastName = "Richardson", DateOfBirth = DateTime.Now };
            this.userStorage.Add(user);
            int userWhichNotExistId = this.userIdGenerator.Object.Generate(new User { FirstName = "R", LastName = "S", DateOfBirth = DateTime.Now });

            TestDelegate testDelegate = () => this.userStorage.Delete(userWhichNotExistId);

            Assert.Catch<InvalidOperationException>(testDelegate);
        }

        [Test]
        public void Delete_UserFromEmptyStorage_ExceptionThrown()
        {
            int userWhichNotExistId = this.userIdGenerator.Object.Generate(new User { FirstName = "R", LastName = "S", DateOfBirth = DateTime.Now });

            TestDelegate testDelegate = () => this.userStorage.Delete(userWhichNotExistId);

            Assert.Catch<InvalidOperationException>(testDelegate);
        }

        [Test]
        public void Delete_User_UserDeleted()
        {
            User userToDelete = new User { FirstName = "R", LastName = "S", DateOfBirth = DateTime.Now };

            this.userStorage.Add(userToDelete);

            int idToDelete = this.userStorage
                .Search(x => x.FirstName == userToDelete.FirstName && x.LastName == userToDelete.LastName)
                .First()
                .Id;

            this.userStorage.Delete(idToDelete);
            IEnumerable<User> foundUsers = this.userStorage
                .Search(x => x.FirstName == userToDelete.FirstName && x.LastName == userToDelete.LastName);

            Assert.AreEqual(0, foundUsers.Count());
        }

        [Test]
        public void Search_NullPredicate_ExceptionThrown()
        {
            Func<User, bool> nullPredicate = null;

            TestDelegate testDelegate = () => this.userStorage.Search(nullPredicate);

            Assert.Catch<ArgumentNullException>(testDelegate);
        }

        [Test]
        public void Search_ByFirstNameInEmptyStorage_Completed()
        {
            int countExpected = 0;
            Func<User, bool> predicate = x => x.FirstName == "Richard";

            var searchResult = this.userStorage.Search(predicate);

            Assert.AreEqual(countExpected, searchResult.Count());
        }

        [Test]
        public void Search_ByFirstName2SameUserExist_Completed()
        {
            this.userStorage.Add(new User { FirstName = "Richard", LastName = "R", DateOfBirth = DateTime.Now });
            this.userStorage.Add(new User { FirstName = "Richard", LastName = "P", DateOfBirth = DateTime.Now });
            Func<User, bool> predicate = x => x.FirstName == "Richard";
            int countExpected = 2;

            var searchResult = this.userStorage.Search(predicate);

            Assert.AreEqual(countExpected, searchResult.Count());
        }

        //[Test]
        //public void Save_SimpleStorageLoader_SaveMethodCalled()
        //{
        //    Mock<IStorageLoader> storageLoaderMock = new Mock<IStorageLoader>();
        //    storageLoaderMock.Setup(sL => sL.Save(It.IsAny<ICollection<User>>())).Verifiable();

        //    userStorage.Save(storageLoaderMock.Object);

        //    storageLoaderMock.Verify();
        //}

        //[Test]
        //public void Save_NullStorageLoader_ExceptionThrown()
        //{
        //    IStorageLoader nullStorageLoader = null;

        //    TestDelegate testDelegate = () => userStorage.Save(nullStorageLoader);

        //    Assert.Catch<ArgumentNullException>(testDelegate);
        //}
        
        //[Test]
        //public void Load_SimpleStorageLoader_SaveMethodCalled()
        //{
        //    Mock<IStorageLoader> storageLoaderMock = new Mock<IStorageLoader>();
        //    storageLoaderMock.Setup(sL => sL.Load()).Verifiable();

        //    userStorage.Load(storageLoaderMock.Object);

        //    storageLoaderMock.Verify();
        //}

        //[Test]
        //public void Load_NullStorageLoader_ExceptionThrown()
        //{
        //    IStorageLoader nullStorageLoader = null;

        //    TestDelegate testDelegate = () => userStorage.Load(nullStorageLoader);

        //    Assert.Catch<ArgumentNullException>(testDelegate);
        //}

    }
}

using labWork3.Core;
using labWork3.DB;
using labWork3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace labWork3Tests.Core
{
    public class DBBackedContactRepositoryTest :
        BaseContactRepositoryTest<DBBackedContactRepository>
    {
        internal List<Contact> mockDBSetList = new();
        internal RepositoryDBContext _context;
        public DBBackedContactRepositoryTest()
        {
            //mockDBSetList = new List<Contact>();

            var mockContext = new Mock<RepositoryDBContext>();
            mockContext.Setup(c => c.Contacts).Returns(getMockDBSet());
            _context = mockContext.Object;

            this.contactRepository = new DBBackedContactRepository(_context);
        }

        DbSet<Contact> getMockDBSet()
        {
            var queryable = mockDBSetList.AsQueryable();

            var dbSet = new Mock<DbSet<Contact>>();
            dbSet.As<IQueryable<Contact>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<Contact>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<Contact>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<Contact>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<Contact>())).Callback<Contact>((s) => mockDBSetList.Add(s));
            dbSet.Setup(d => d.Remove(It.IsAny<Contact>())).Callback<Contact>((s) => mockDBSetList.Remove(s));

            return dbSet.Object;
        }

        [Fact]
        void DBBackedRepository_Add_Add1Contact_Expected_1Contact_Inserted()
        {
            var contact = new Contact
            {
                FirstName = "aaa",
                LastName = "bbb",
                PhoneNumber = "999",
                Email = "test"
            };

            this.contactRepository.AddContact(contact);
            Assert.Single(mockDBSetList);
        }

        [Fact]
        void DBBackedRepository_LoadOnCreate_Expected_1Contact_Loaded_From_DB()
        {
            var contact = new Contact
            {
                FirstName = "aaa",
                LastName = "bbb",
                PhoneNumber = "999",
                Email = "test"
            };
            this.contactRepository.AddContact(contact);

            //reload repository
            this.contactRepository = new DBBackedContactRepository(_context);
            var contactsList = this.contactRepository.GetAllContacts();
            Assert.Single(contactsList);
        }
    }
}

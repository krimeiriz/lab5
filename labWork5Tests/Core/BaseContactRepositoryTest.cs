using labWork3.Core;
using labWork3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace labWork3Tests.Core
{
    
    public abstract class BaseContactRepositoryTest<T> : IDisposable 
        where T : ContactRepository
    {
        internal T contactRepository;

        
        public void Dispose()
        {
            contactRepository.ResetRepository();
        }


        [Fact]
        public void AddContact_Add1Contact()
        {
            var contact1 = new Contact{FirstName = "test", LastName = "test", PhoneNumber = "777", Email = "test"};
            contactRepository.AddContact(contact1);

            var expected = 1;
            var actual = contactRepository.GetAllContacts().Count;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAllContacts_Get2Contacts()
        {
            var contact1 = new Contact{FirstName = "test", LastName = "test", PhoneNumber = "777", Email = "test"};
            contactRepository.AddContact(contact1);
            var contact2 = new Contact{FirstName = "test", LastName = "test", PhoneNumber = "777", Email = "test"};
            contactRepository.AddContact(contact2);

            var expected = 2;
            var actual = contactRepository.GetAllContacts().Count;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new string[] { "Alex", "John" }, "alex", 1)]
        [InlineData(new string[] { "Alex", "John" }, "ALEX", 1)]
        [InlineData(new string[] { "Alex", "John", "Alexey" }, "Alex", 2)]
        [InlineData(new string[] { "Alex", "John" }, "al", 1)]
        [InlineData(new string[] { }, "alex", 0)]
        public void FindByFirstname(
            string[] names,
            string searchMask,
            int resultSize
            )
        {
            foreach (string name in names)
            {
                Contact c = new Contact { FirstName = name, LastName = "test", PhoneNumber = "777", Email = "test" };
                contactRepository.AddContact(c);
            }

            var actual = contactRepository.FindByFirstname(searchMask).Count;

            Assert.Equal(resultSize, actual);
        }

        [Theory]
        [InlineData(new string[] { "Smith", "Li" }, "smith", 1)]
        [InlineData(new string[] { "smith", "li" }, "SMITH", 1)]
        [InlineData(new string[] { "Smith", "Li", "Linden" }, "li", 2)]
        [InlineData(new string[] { "Smith", "Li" }, "sm", 1)]
        [InlineData(new string[] { }, "sm", 0)]
        public void FindByLastname(
            string[] range,
            string searchMask,
            int resultSize
            )
        {
            foreach (string lastname in range)
            {
                Contact c = new Contact { FirstName = "test", LastName = lastname, PhoneNumber = "777", Email = "test" };
                contactRepository.AddContact(c);
            }

            var actual = contactRepository.FindByLastname(searchMask).Count;

            Assert.Equal(resultSize, actual);
        }

        [Theory]
        [InlineData(new string[] { "Alex Johnson", "Li Sharper" }, "alex johnson", 1)]
        [InlineData(new string[] { "alex johnson", "li Sharper" }, "ALEX JOHNSON", 1)]
        [InlineData(new string[] { "Alex Johnson", "Li Sharper", "Alexey Johnson" }, "Alex Johnson", 2)]
        [InlineData(new string[] { "Alex Johnson", "Li Sharper" }, "al jo", 1)]
        [InlineData(new string[] { }, "alex johnson", 0)]
        public void FindByFullname(
            string[] fullnameRange,
            string searchMask,
            int resultSize
        )
        {
            foreach (string name in fullnameRange)
            {
                string[] tokensName = name.Split(new char[] { ' ' });
                Contact c = new Contact { FirstName = tokensName[0], LastName = tokensName[1], PhoneNumber = "777", Email = "test" };
                contactRepository.AddContact(c);
            }

            var tokens = searchMask.Split(new char[] { ' ' });
            var actual = contactRepository.FindByFullname(tokens[0], tokens[1]).Count;

            Assert.Equal(resultSize, actual);
        }

        [Theory]
        [InlineData(new string[] { "755633", "112233" }, "755633", 1)]
        [InlineData(new string[] { "755633", "112233", "7556331" }, "755633", 2)]
        [InlineData(new string[] { "755633", "112233" }, "755", 1)]
        [InlineData(new string[] { }, "755633", 0)]
        public void FindByPhonenumber(
           string[] range,
           string searchMask,
           int resultSize
           )
        {
            foreach (string number in range)
            {
                Contact c = new Contact { FirstName = "test", LastName = "test", PhoneNumber = number, Email = "test" };
                contactRepository.AddContact(c);
            }

            var actual = contactRepository.FindByPhoneNumber(searchMask).Count;

            Assert.Equal(resultSize, actual);
        }

        [Theory]
        [InlineData(new string[] { "sm@z.com", "l@mail.com" }, "sm@z.com", 1)]
        [InlineData(new string[] { "sm@z.com", "l@mail.com" }, "sm", 1)]
        [InlineData(new string[] { "sm@z.com", "l@mail.com", "ksm@z.com" }, "sm@z.com", 2)]
        [InlineData(new string[] { }, "sm", 0)]
        public void FindByEmail(
            string[] range,
            string searchMask,
            int resultSize
        )
        {
            foreach (string email in range)
            {
                Contact c = new Contact { FirstName = "test", LastName = "test", PhoneNumber = "777", Email = email };
                contactRepository.AddContact(c);
            }

            var actual = contactRepository.FindByEmail(searchMask).Count;

            Assert.Equal(resultSize, actual);
        }

        [Theory]
        [InlineData(
           new string[]
                {
              "Alex Morgan 777 email1",
              "Li Sharper 666 email2"
                },
           "777",
           1
       )]
        [InlineData(
           new string[]
                {
              "Alex Morgan 777 email1",
              "Li Sharper 666 email2"
                },
           "SHARPER",
           1
       )]
        [InlineData(
           new string[]
                {
              "Alex Morgan 777 email1",
              "Li Sharper 666 email2"
                },
           "EMAIL",
           2
       )]
        [InlineData(
           new string[] { },
           "777",
           0
       )]
        public void FindByWholeFields(
            string[] contactsRange,
            string searchMask,
            int resultSize
        )
        {
            foreach (string name in contactsRange)
            {
                string[] tokensContact = name.Split(new char[] { ' ' });
                Contact c = new Contact
                {
                    FirstName = tokensContact[0],
                    LastName = tokensContact[1],
                    PhoneNumber = tokensContact[2],
                    Email = tokensContact[3]
                };
                contactRepository.AddContact(c);
            }

            var actual = contactRepository.FindByAnyField(searchMask).Count;

            Assert.Equal(resultSize, actual);
        }

    }
}

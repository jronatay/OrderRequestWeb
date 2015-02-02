using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void string_equals_when_argument_is_null_throws_null_reference_exception()
        {
            var customer = new Customer();

            customer.FirstName.Equals(null);
        }

        [TestMethod]
        public void string_isnullorempty_returns_true_when_argument_isnullorempty()
        {
            var customer = new Customer();
            Assert.IsTrue(string.IsNullOrEmpty(customer.FirstName));
        }

        class Customer
        {
            public string FirstName { get; set; }
        }
    }
}

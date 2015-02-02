using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLibrary.CustomerModels;

namespace EntityLibrary
{
    public class CustomerRepository
    {
        private OrderRequestEntities db;
        public CustomerRepository(OrderRequestEntities db)
        {
            this.db = db;
        }

        public void Save(Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
        }

        public bool IsEmailExist(string EmailAddress)
        {
            var result = db.Customers.Where(customer => customer.EmailAddress == EmailAddress).ToList();
            return result.Count > 0;
        }

        public bool IsCustomerSignInExist(SignInInputModel CustomerSignInInput)
        {
            var result = db.Customers.Where(customer => customer.EmailAddress == CustomerSignInInput.EmailAddress && customer.Password == CustomerSignInInput.Password).ToList();
            return result.Count > 0;
        }

        public Customer CustomerLoggingIn(SignInInputModel CustomerSignInInput)
        {
            var result = db.Customers.Where(customer => customer.EmailAddress == CustomerSignInInput.EmailAddress && customer.Password == CustomerSignInInput.Password).First();
            return result;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

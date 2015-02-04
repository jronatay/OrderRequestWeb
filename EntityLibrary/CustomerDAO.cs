using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLibrary.CustomerModels;

namespace EntityLibrary
{
    public class CustomerDAO
    {
        private OrderRequestEntities db;
        public CustomerDAO(OrderRequestEntities db)
        {
            this.db = db;
        }

        public void Save(Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
        }

        public Customer CustomerLoggingIn(SignInInputModel CustomerSignInInput)
        {
            var result = db.Customers.Where(customer => customer.EmailAddress == CustomerSignInInput.EmailAddress && customer.Password == CustomerSignInInput.Password).First();
            return result;
        }

        
    }
}

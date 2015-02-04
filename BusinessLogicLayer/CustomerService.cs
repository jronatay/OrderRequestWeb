using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLibrary;
using EntityLibrary.CustomerModels;
using System.Web;
using System.Text.RegularExpressions;




namespace BusinessLogicLayer
{
    public class CustomerService
    {
        
        private EntityLibrary.CustomerDAO CustomerRepository = new CustomerDAO(new OrderRequestEntities());
        private OrderRequestEntities db = new OrderRequestEntities();
        public void Save(Customer customer)
        {
            SetCustomerRegistrationDate(customer); 
            CustomerRepository.Save(customer); 
        }

        public void SetCustomerRegistrationDate(Customer customer)
        {
            customer.RegistrationDate = DateTime.Now.ToLocalTime();
        }
        
        public bool ISCustomerDataMatchesRegex(Customer customer)
        {
            string namePattern = @"^[^\d^`~!@#\$%\^&\*\(\)\-_\+=<>\?,;:'\|/\[\]\{\}""]+[^\d^`~!@#\$%\^&\*\(\)\-_\+=<>\?,;:\|/\[\]\{\}""]$";
            string passwordPattern = @"^[^\s^<>]*$";
            string emailPattern = @"[\w-]+@([\w-]+\.)+[\w-]+";
            string addressPattern = @"^[^`~!@#\$%\^&\*\(\)_\+=<>\?,;:'\|/\[\]\{\}""]+[^`~!@#\$%\^&\*\(\)_\+=<>\?,;:\|/\[\]\{\}""]$";
            string zipCodePattern = @"^[A-Z0-9]{2,5}$|^[A-Z0-9]{2,5}-[A-Z0-9]{2,4}$";
            string CityRegionPattern = @"^[^\d^`~!@#\$%\^&\*\(\)_\+=<>\?;:'\|/\[\]\{\}""]+[^\d^`~!@#\$%\^&\*\(\)_\+=<>\?;:\|/\[\]\{\}""]$";

            Regex nameRegex = new Regex(namePattern);
            Regex passwordRegex = new Regex(passwordPattern);
            Regex emailRegex = new Regex(emailPattern);
            Regex addressRegex = new Regex(addressPattern);
            Regex zipCodeRegex = new Regex(zipCodePattern);
            Regex CityRegionRegex = new Regex(CityRegionPattern);
            if (nameRegex.IsMatch(customer.FirstName) && nameRegex.IsMatch(customer.LastName) && passwordRegex.IsMatch(customer.Password) && emailRegex.IsMatch(customer.EmailAddress) &&
                addressRegex.IsMatch(customer.Address1)  && zipCodeRegex.IsMatch(customer.ZipCode) && CityRegionRegex.IsMatch(customer.City))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public bool IsCustomerDataValid(Customer customer)
        {
            if (ISCustomerDataMatchesRegex(customer)) 
                return true; 
            else
                return false;
        }

        public bool IsEmailExixt(Customer customer)
        {
            var result = db.Customers.Where(customerdata => customerdata.EmailAddress == customer.EmailAddress).ToList();
            return result.Count > 0;
           
        }

        public bool IsCustomerSignInExist(SignInInputModel CustomerSignInInput)
        {
            var result = db.Customers.Where(customer => customer.EmailAddress == CustomerSignInInput.EmailAddress && customer.Password == CustomerSignInInput.Password).ToList();
            return result.Count > 0;
            
        }
        
        public string LoggedInUser(SignInInputModel CustomerSignInInput)
        {
            Customer customer = CustomerRepository.CustomerLoggingIn(CustomerSignInInput);
            return customer.FirstName+","+customer.Id.ToString();
        }

       
       
       
    }
    
}

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
        
        private EntityLibrary.CustomerRepository CustomerRepository = new CustomerRepository(new OrderRequestEntities());
        public void Save(Customer customer)
        {
            if (IsCustomerDataValid(customer)) { SetCustomerRegistrationDate(customer); CustomerRepository.Save(customer); }
        }
        public void SetCustomerRegistrationDate(Customer customer)
        {
            customer.RegistrationDate = DateTime.Now.ToUniversalTime();
        }
        public Boolean IsNull(Customer customer)
        {
            if (customer.FirstName.Equals(null) && customer.FirstName.Equals(" ") &&
                customer.LastName.Equals(null) && customer.LastName.Equals(" ") &&
                customer.Address1.Equals(null) && customer.Address1.Equals(" ") &&
                customer.ZipCode.Equals(null) && customer.ZipCode.Equals(" ") &&
                customer.City.Equals(null) && customer.City.Equals(" ") &&
                customer.Country.Equals(null) && customer.Country.Equals(" ") &&
                customer.EmailAddress.Equals(null) && customer.EmailAddress.Equals(" ") &&
                customer.Password.Equals(null) && customer.Password.Equals(" "))
            { 
                return true; 
            }
            else { return false; }
        }
        public Boolean IsLengthRight(Customer customer)
        {
           
            int FirstNameLength = customer.FirstName.Count();
            int LastNameLength = customer.LastName.Count();
            int Address1Length = customer.Address1.Count();
            int ZipCodeLength = customer.ZipCode.Count();
            int CityLength = customer.City.Count();
            int EmailAddressLength = customer.EmailAddress.Count();
            int PasswordLength = customer.Password.Count();

            if (FirstNameLength > 2 && FirstNameLength < 21 &&
               LastNameLength > 2 && LastNameLength < 21 &&
               Address1Length > 2 && Address1Length < 201 &&
               ZipCodeLength > 2 && ZipCodeLength < 21 &&
               CityLength > 2 && CityLength < 51 &&
               EmailAddressLength > 2 && EmailAddressLength < 51 &&
               PasswordLength > 2 && PasswordLength < 21)
            {
                return true;
            }
            else { return false; }

        }
        public Boolean ISCustomerDataMatchesRegex(Customer customer)
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
                addressRegex.IsMatch(customer.Address1) && addressRegex.IsMatch(customer.Address2) && zipCodeRegex.IsMatch(customer.ZipCode) && CityRegionRegex.IsMatch(customer.City))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean IsCustomerDataValid(Customer customer)
        {
            if (!IsNull(customer) && IsLengthRight(customer) && !IsEmailExixt(customer) && ISCustomerDataMatchesRegex(customer)) return true; else return false;
        }

        public Boolean IsEmailExixt(Customer customer)
        {
            return CustomerRepository.IsEmailExist(customer.EmailAddress);
        }
        public Boolean IsCustomerSignInExist(SignInInputModel CustomerSignInInput)
        {
            return CustomerRepository.IsCustomerSignInExist(CustomerSignInInput);
        }
        public string LoggedInUser(SignInInputModel CustomerSignInInput)
        {
            Customer customer = CustomerRepository.CustomerLoggingIn(CustomerSignInInput);
            return customer.FirstName+","+customer.Id.ToString();
        }

       
       
       
    }
    
}

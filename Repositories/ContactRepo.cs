using Microsoft.EntityFrameworkCore;
using Sips.Data;
using Sips.SipsModels;
using Sips.ViewModels;

namespace Sips.Repositories
{
    public class ContactRepo
    {
        private readonly SipsdatabaseContext _db;

        public ContactRepo(SipsdatabaseContext db)
        {
            this._db = db;
        }

        public void RegisterUser(Contact contact)
        {
            
            _db.Contacts.Add(contact);
            _db.SaveChanges();

        }
        public Contact GetRegisteredUser(string email)
        {
            var user = _db.Contacts.FirstOrDefault(u => u.Email == email);
            return user;

        }

        public string GetFullNameByEmail(string email)
        {
            var user = _db.Contacts.FirstOrDefault(u => u.Email == email);
            return user != null ? $"{user.FirstName} {user.LastName}" : null;
        }

        public IEnumerable<ContactVM> GetAll()
        {
            var contacts = _db.Contacts.ToList();
            List<ContactVM> contactsVM = new List<ContactVM>();


            foreach (var c in contacts)
            {
                ContactVM conVM = new ContactVM()
                {
                    UserId = c.UserId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email,
                    Unit = c.Unit,
                    Street = c.Street,
                    City = c.City,
                    Province = c.Province,
                    PostalCode = c.PostalCode,
                    BirthDate = c.BirthDate,
                    IsDrinkRedeemed = c.IsDrinkRedeemed
                };

                contactsVM.Add(conVM);
            }

            return contactsVM;
        }
        public ContactVM GetById(int id)
        {
            var c = _db.Contacts.FirstOrDefault(p => p.UserId == id);

            ContactVM conVM = new ContactVM()
            {
                UserId = c.UserId,
                FirstName = c.FirstName,
                LastName = c.LastName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Unit = c.Unit,
                Street = c.Street,
                City = c.City,
                Province = c.Province,
                PostalCode = c.PostalCode,
                BirthDate = c.BirthDate,
                IsDrinkRedeemed = c.IsDrinkRedeemed

            };

            return conVM;
        }

        public string Add(Contact contact)
        {
            string message = string.Empty;
            try
            {
                _db.Add(contact);
                _db.SaveChanges();
                message = $"Customer {contact.Email} added successfully";
            }
            catch (Exception e)
            {
                message = $" Error saving customer {contact.Email}: {e.Message}";
            }
            return message;
        }

        public string Update(ContactVM edittingcontact)
        {
            string message = string.Empty;
            try
            {
                Contact contact = _db.Contacts.FirstOrDefault(p => p.UserId == edittingcontact.UserId);

                //contact.Email = edittingcontact.Email;
                contact.FirstName = edittingcontact.FirstName;
                contact.LastName = edittingcontact.LastName;
                contact.PhoneNumber = edittingcontact.PhoneNumber;
                contact.City = edittingcontact.City;
                contact.Street = edittingcontact.Street;
                contact.Province = edittingcontact.Province;
                contact.Unit = edittingcontact.Unit;
                contact.PostalCode = edittingcontact.PostalCode;
                contact.BirthDate = edittingcontact.BirthDate;

                _db.SaveChanges();
                message = $"Customer {edittingcontact.Email} updated successfully";
            }
            catch (Exception e)
            {
                message = $" Error updating customer {edittingcontact.Email} : {e.Message}";
            }
            return message;
        }

        public string Delete(ContactVM contactVM)
        {
            string message = string.Empty;
            Contact contact = _db.Contacts.FirstOrDefault(c => c.UserId == contactVM.UserId);

            try
            {
                message = $"{contact.Email} deleted successfully";

                _db.Contacts.Remove(contact);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                message = $" Error deleting contact-{contact.Email}: {e.Message}";
            }
            return message;
        }
    }
}



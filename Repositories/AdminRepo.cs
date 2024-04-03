using Microsoft.EntityFrameworkCore;
using Sips.Data;
using Sips.SipsModels;
using Sips.ViewModels;

namespace Sips.Repositories
{
    public class AdminRepo
    {
        private readonly SipsdatabaseContext _db;

        public AdminRepo(SipsdatabaseContext db)
        {
            this._db = db;
        }

        public void DeleteRelatedRecords(string paymentId)
        {
            // Delete related transactions
            var transactions = _db.Transactions.Where(t => t.TransactionId == paymentId).ToList();
            foreach (var transaction in transactions)
            {
                _db.Transactions.Remove(transaction);
            }

            // Delete related order details
            var orderDetails = _db.OrderDetails.Where(od => od.TransactionId == paymentId).ToList();
            foreach (var orderDetail in orderDetails)
            {
                _db.OrderDetails.Remove(orderDetail);
            }

            // Save changes to the database
            _db.SaveChanges();
        }

    }
}

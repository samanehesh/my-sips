using Sips.Data;
using Sips.SipsModels;
using Sips.ViewModels;

namespace Sips.Repositories
{
    public class TransactionRepo
    {
        private readonly SipsdatabaseContext _db;

        public TransactionRepo(SipsdatabaseContext db)
        {
            _db = db;
        }
        public List<PayPalVM> GetTransactions()
        {
            SipsdatabaseContext db = _db;
            List<PayPalVM> transactions = _db.PayPalVMs
                    .Select(transaction => new PayPalVM
                    {
                        TransactionId = transaction.TransactionId,
                        Amount = transaction.Amount,
                        PayerName = transaction.PayerName,
                        PayerEmail = transaction.PayerEmail,
                        CreatedDate = transaction.CreatedDate,
                        PaymentMethod = transaction.PaymentMethod,
                        Currency = transaction.Currency
                    })
                    .ToList();
            return transactions;
        }

    }


}

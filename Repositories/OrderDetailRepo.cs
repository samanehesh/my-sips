using Microsoft.EntityFrameworkCore;
using Sips.SipsModels;
using Sips.ViewModels;


namespace Sips.Repositories
{
    public class OrderDetailRepo
    {
        private readonly SipsdatabaseContext _db;

        public OrderDetailRepo(SipsdatabaseContext db)
        {
            this._db = db;
        }
        public IEnumerable<OrderDetailVM> GetAll()
        {
            //var orders = _db.OrderDetails.ToList();
            var transactions = _db.Transactions
                .Include(c => c.User)
                .Include(p => p.OrderDetails)
                .Include(p => p.Store)
                .ToList();
            List<OrderDetailVM> OrdersDetailVM = new List<OrderDetailVM>();


            foreach (var transaction in transactions)
            {
                var orderDetail = transaction.OrderDetails.Where(od => od.TransactionId == transaction.TransactionId).ToList();

                OrderDetailVM orderVM = new OrderDetailVM()
                {
                    OrderDetailIds = orderDetail.Select(od => od.OrderDetailId).ToList(),

                    TransactionId = transaction.TransactionId,
                    DateOrdered = transaction.DateOrdered,
                    StoreId = transaction.StoreId,
                    UserId = transaction.UserId,
                    //StatusId = transaction.StatusId,
                    UserEmail = transaction.User.Email,
                    totalPrice = 0,
                    totalQuantity = 0,
                    ItemTypes = new List<string>() // Initialize ItemTypes as an empty list

                    //totalPrice = transaction.OrderDetails.price;
                };
                var addIns = new List<AddInOrderDetail>(); // Assuming AddInOrderDetail is the type of objects in the list
                foreach (var orderID in orderVM.OrderDetailIds)
                {
                    var addinPrice = 0.0; // Reset addinPrice for each order detail

                    var order = _db.OrderDetails
                                        .Include(m => m.MilkChoice)
                                        .Include(c => c.Item)
                                        .Where(id => id.OrderDetailId == orderID)
                                        .FirstOrDefault();

                     addIns = _db.AddInOrderDetails
                                    .Include(ai => ai.AddIn) // Include the AddIn entity
                                    .Where(a => a.OrderDetailId == orderID)
                                    .ToList();

                    if (addIns.Any())
                    {
                        foreach (var item in addIns)
                        {
                            // Check if AddIn is not null and then access its PriceModifier
                            if (item.AddIn != null)
                            {
                                addinPrice += (double)item.AddIn.PriceModifier;
                            }
                        }
                    }
                    var milkPrice = order.MilkChoice.PriceModifier;
                    var price = ((double)(order?.Price) + (double)milkPrice + addinPrice)* order?.Quantity;

                    var quantity = order?.Quantity;
                    orderVM.totalQuantity += quantity;
                    orderVM.totalPrice += price;

                    var itemType = order?.Item.Name;
                    orderVM.ItemTypes.Add(itemType);

                }

                OrdersDetailVM.Add(orderVM);
            }

            return OrdersDetailVM;
        }



        public OrderDetailVM GetById(string id)
        {

            //List<string>? ItemTypes = null;

            var transaction = _db.Transactions
                            .Include(c => c.User)
                            .Include(p => p.OrderDetails)
                            .Include(p => p.Store)
                            .FirstOrDefault(p => p.TransactionId == id);

            //var orderDetail = transaction.OrderDetails.Where(od => od.TransactionId == transaction.TransactionId).ToList();


            var orderVM = new OrderDetailVM
            {
                orderDetail = transaction.OrderDetails
                    .Where(od => od.TransactionId == transaction.TransactionId)
                    .ToList(),

                TransactionId = transaction?.TransactionId,
                DateOrdered = transaction?.DateOrdered,
                StoreId = transaction?.StoreId,
                UserId = transaction?.UserId,
                //StatusId = transaction.StatusId,
                UserEmail = transaction?.User.Email,
                totalPrice = 0,
                totalQuantity = 0,
                ItemTypes = new List<string>(), // Initialize ItemTypes as an empty list
                addInIds = new List<int>(), // Initialize ItemTypes as an empty list
                //addInIdsDictionary = new Dictionary<int, int>(),
            };

            var orderDetailIds = orderVM.orderDetail.Select(od => od.OrderDetailId).ToList();
            var addIns = new List<AddInOrderDetail>();

            orderVM.addInIdsDictionary = new Dictionary<int, List<int>>();

            foreach (var orderID in orderDetailIds)
            {
                var addinPrice = 0.0;


                var order = _db.OrderDetails
                                    .Include(m => m.MilkChoice)
                                    .Include(c => c.Item)
                                    .Where(id => id.OrderDetailId == orderID)
                                    .FirstOrDefault();                //addIns = _db.AddInOrderDetails.Where(a => a.OrderDetailId == orderID).ToList();

                addIns = _db.AddInOrderDetails
                               .Include(ai => ai.AddIn) // Include the AddIn entity
                               .Where(a => a.OrderDetailId == orderID)
                               .ToList();

                orderVM.addInIds = new List<int>(); // Initialize ItemTypes as an empty list


                if (addIns.Any())
                {
                    foreach (var item in addIns)
                    {
                        if(item.AddIn != null)
                        {
                            var addinId = (item.AddIn.AddInId);
                            orderVM.addInIds.Add(addinId);
                            addinPrice += (double)(item.AddIn.PriceModifier);
                        }
                    }
                }
                orderVM.addInIdsDictionary.Add(orderID, orderVM.addInIds);


                var milkPrice = order.MilkChoice.PriceModifier;
                var price = ((double)(order?.Price) + (double)milkPrice + addinPrice) * order?.Quantity;

                var quantity = order?.Quantity;
                orderVM.totalQuantity += quantity;
                orderVM.totalPrice += price;

                var itemType = order?.Item.Name;
                orderVM.ItemTypes.Add(itemType);

            }

            return orderVM;
        }


        public string Delete(OrderDetailVM orderVM)
        {
            string message = string.Empty;
            Transaction transaction = _db.Transactions.Include(c => c.User).FirstOrDefault(c => c.TransactionId == orderVM.TransactionId);
            List<OrderDetail> orderDetails = _db.OrderDetails.Where(c => c.TransactionId == orderVM.TransactionId).ToList();


            try
            {
                foreach (OrderDetail order in orderDetails)
                {
                    _db.OrderDetails.Remove(order);
                    _db.SaveChanges();

                }

                message = $"Transaction with Id:{transaction?.TransactionId} deleted successfully";

                _db.Transactions.Remove(transaction);
                _db.SaveChanges();

            }
            catch (Exception e)
            {
                message = $"Error deleting transaction-{transaction?.TransactionId}: {e.Message}";
            }
            return message;
        }

    }
}

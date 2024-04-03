using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Sips.Data;
using Sips.Repositories;
using Sips.SipsModels;
using Sips.ViewModels;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Authorization;
using Sips.Data.Services;
using System.Text.Encodings.Web;

namespace Sips.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly SipsdatabaseContext _db;
        private PayPalVM payPalVM;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public TransactionController(SipsdatabaseContext db, IConfiguration configuration, IEmailService emailService)
        {
            _db = db;
            _configuration = configuration;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            TransactionRepo transactionRepo = new TransactionRepo(_db);

            return View(transactionRepo.GetTransactions());
        }
    
        public IActionResult PayPal(PayPalVM payPalVM)
        {
            _db.PayPalVMs.Add(payPalVM);
            _db.SaveChanges();
            return View(payPalVM);
        }
        public IActionResult Checkout()
        {
            // Other code for PayPal client ID
            var payPalClient = _configuration["PayPal:ClientId"];
            ViewData["PayPalClientId"] = payPalClient;
            string cartSession = HttpContext.Session.GetString("Cart");
            List<CartVM> cartItems = new List<CartVM>();

            if (cartSession != null)
            {
                List<CartVM>  sessionCartItems = JsonConvert.DeserializeObject<List<CartVM>>(cartSession);

                decimal total = 0;
                int quantity = 0;
                string lastItemId = "0";

                foreach (var item in sessionCartItems.OrderBy(c => c.UniqueItemId))
                {
                    if (item.UniqueItemId != lastItemId)
                    {
                        lastItemId = item.UniqueItemId;
                        cartItems.Add(item);
                    }
                    else {
                        cartItems.Last().Quantity += item.Quantity;
                        cartItems.Last().Subtotal += item.Subtotal;
                    }
                }
            }
            else
            {
                cartItems = new List<CartVM>();
            }

            return View(cartItems);

        }

        [HttpPost]
        public IActionResult PaySuccess([FromBody] PayPalVM payPalVM)
        {

            try
            {
                // Get the email of the currently logged-in user
                var userEmail = User.Identity.Name;

                // Find the user in the Contact table based on the email
                var user = _db.Contacts.FirstOrDefault(c => c.Email == userEmail);


                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                var paymentNotification = new PaymentNotification
                {
                    PaymentId = payPalVM.TransactionId,
                    Amount = payPalVM.Amount,
                    CurrencyCode = payPalVM.Currency,
                    CurrencySymbol = payPalVM.CurrencySymbol,
                    PayerId = payPalVM.PayerId,
                    PayerFullName = payPalVM.PayerFullName,
                    CaptureId = payPalVM.CaptureId
                };

                _db.PaymentNotifications.Add(paymentNotification);
                _db.SaveChanges();


                // Construct the Transaction object using properties from the OrderDetail
                var transaction = new Transaction
                {
                    TransactionId = payPalVM.TransactionId,
                    DateOrdered = payPalVM.CreatedDate, // Assuming CreatedDate is in correct format
                    StoreId = 1,
                    UserId = user.UserId,
                    StatusId = 1,
                };
                _db.Transactions.Add(transaction);
                _db.SaveChanges();

                // Retrieve cart data from the session
                var cartJson = HttpContext.Session.GetString("Cart");
                var cartItems = JsonConvert.DeserializeObject<List<CartVM>>(cartJson);

                // Parse the cart items and create OrderDetail objects
                foreach (var cartItem in cartItems)
                {
                    Sweetness swettness = _db.Sweetnesses.FirstOrDefault(s => s.SweetnessPercent == cartItem.SweetnessPercent);
                    Ice ice = _db.Ices.FirstOrDefault(I => I.IcePercent == cartItem.IcePercent);
                    MilkChoice milk = _db.MilkChoices.FirstOrDefault(m => m.MilkType == cartItem.MilkType);


                    // If milk is null, set it to "No Milk" which has ID 5
                    if (milk == null)
                    {
                        milk = _db.MilkChoices.FirstOrDefault(m => m.MilkChoiceId == 5); // Assuming ID 5 corresponds to "No Milk"
                    }

                    var cartOrderDetail = new OrderDetail
                    {

                        TransactionId = payPalVM.TransactionId, // Assuming TransactionId is already set in OrderDetail
                        ItemId = cartItem.ItemId,
                        Price = cartItem.BasePrice,
                        Quantity = cartItem.Quantity,
                        SweetnessId = swettness.SweetnessId,
                        IceId = ice.IceId,
                        MilkChoiceId = milk.MilkChoiceId,
                        IsBirthdayDrink = false, // Assuming this property is not available in CartVM
                        PromoValue = null, // Assuming this property is not available in CartVM
                        SizeId = 1, // Assuming this property is not available in CartVM

                    };
                    _db.OrderDetails.Add(cartOrderDetail);
                    _db.SaveChanges();


                    var orderDetailId = cartOrderDetail.OrderDetailId;

                    List<AddIn> addInNames = cartItem.AddInNames;

                    foreach (var item in addInNames)
                    {
                        var addIn = _db.AddIns.FirstOrDefault(a => a.AddInName == item.AddInName);

                        if (addIn != null) // Ensure add-in exists
                        {
                            var addInOrderDetail = new AddInOrderDetail
                            {
                                OrderDetailId = orderDetailId,
                                AddInId = addIn.AddInId, // Assuming AddInId is the primary key of the AddIn table
                                Quantity = 1,
                            };

                            _db.AddInOrderDetails.Add(addInOrderDetail);
                            //_db.SaveChangesAsync();
                        }
                    }
                    _db.SaveChanges();



                }

                var response = _emailService.SendSingleEmail(new SipsModels.ComposeEmailModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Subject = "Thank you for your SIPS purchase!",
                    Email = user.Email,
                    Body = $"Thank you for supporting your local SIPS store! Here is your transaction ID: {payPalVM.TransactionId}. We hope you enjoy your drink, and visit us again soon!"
                });

                // Save the PaymentId of the PayPalVM item to the session variable as a string
                HttpContext.Session.SetString("PayPalConfirmationModelId", payPalVM.TransactionId);

                // Construct the redirect URL
                var redirectUrl = Url.Action("Confirmation", "Transaction");

                // Return a JSON response with the redirect URL
                return Json(new { redirectUrl });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        //Confirmation Page
        public IActionResult Confirmation()
        {
            try
            {
                // Retrieve the PaymentId of the PayPalVM item from the session variable
                var payPalConfirmationModelPaymentId = HttpContext.Session.GetString("PayPalConfirmationModelId");

                if (!string.IsNullOrEmpty(payPalConfirmationModelPaymentId))
                {
                    // Retrieve the PayPalVM item from the database using the PaymentId
                    var orderDetailModel = _db.OrderDetails.FirstOrDefault(model => model.TransactionId == payPalConfirmationModelPaymentId);

                    if (orderDetailModel != null)
                    {
                        // Clear the session variable to ensure it's not available on subsequent requests
                        HttpContext.Session.Remove("PayPalConfirmationModelId");
                        HttpContext.Session.Remove("Cart");

                        // This action should only be accessed via a server-side redirect, not directly from the client.
                        // If a client tries to access it directly, you may want to handle it appropriately.

                        return View("Confirmation", orderDetailModel);
                    }
                    else
                    {
                        // Handle the case when the PayPalVM item with the specified PaymentId is not found
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    // Handle the case when the session variable is empty or not available
                    // Redirect or return an error view as appropriate for your application
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return RedirectToAction("Index", "Home"); // Redirect to an error view
            }
        }

        [Produces("application/json")]
        [HttpGet]
        public JsonResult GetCartItems(string itemId)
        {
            string cartSession = HttpContext.Session.GetString("Cart");
            List<CartVM> cartItems;

            if (cartSession != null)
            {
                cartItems = JsonConvert.DeserializeObject<List<CartVM>>(cartSession);
                cartItems = cartItems.Where(c => c.UniqueItemId == itemId).ToList();
            }
            else
            {
                cartItems = new List<CartVM>();
            }

            return Json(cartItems);
        }
        
        public JsonResult AddToCart([FromBody] CartVM cartVM)
        {
            string cartSession = HttpContext.Session.GetString("Cart");
            List<CartVM> cartItems;

            if (cartSession != null)
            {
                cartItems = JsonConvert.DeserializeObject<List<CartVM>>(cartSession);
            }
            else
            {
                cartItems = new List<CartVM>();
            }

            // Assuming UniqueItemId is already set in cartVM based on the item and its selected options
            var existingItem = cartItems.FirstOrDefault(c => c.UniqueItemId == cartVM.UniqueItemId);
            if (existingItem != null)
            {
                // Update the quantity of the existing item
                existingItem.Quantity += cartVM.Quantity;
            }
            else
            {
                // If the item is new, add it to the cart
                cartItems.Add(cartVM);
            }


            // Serialize and store the updated cart items in the session
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));

            // Return the response
            return Json(new { success = true, message = "Item added/updated in the cart." });
        }

        public JsonResult AddOneToCart([FromBody] string uniqueItemId)
        {
            string cartSession = HttpContext.Session.GetString("Cart");
            List<CartVM> cartItems;

            if (cartSession != null)
            {
                cartItems = System.Text.Json.JsonSerializer.Deserialize<List<CartVM>>(cartSession);
            }
            else
            {
                cartItems = new List<CartVM>();
            }

            // Check if the item is already in the cart
            var existingItem = cartItems.FirstOrDefault(c => c.UniqueItemId == uniqueItemId);
            if (existingItem != null)
            {
                // Increment the quantity
                existingItem.Quantity++;
                // Update the UniqueItemId
                existingItem.UniqueItemId = uniqueItemId;
            }

            // Serialize and store the updated cart items in the session
            HttpContext.Session.SetString("Cart", System.Text.Json.JsonSerializer.Serialize(cartItems));

            return Json(new { newItem = existingItem });
        }


        public JsonResult RemoveFromCart([FromBody] string uniqueItemId)
        {
            string cartSession = HttpContext.Session.GetString("Cart");
            List<CartVM> cartItems;

            if (cartSession != null)
            {
                cartItems = System.Text.Json.JsonSerializer.Deserialize<List<CartVM>>(cartSession);
            }
            else
            {
                cartItems = new List<CartVM>();
            }

            // Check if the item is already in the cart
            var existingItem = cartItems.FirstOrDefault(c => c.UniqueItemId == uniqueItemId);
            if (existingItem != null)
            {
                // Reduce the quantity by 1
                existingItem.Quantity--;

                // If the remaining quantity is 0 or the current quantity is 1, remove the item completely
                if (existingItem.Quantity <= 0)
                {
                    cartItems.Remove(existingItem);
                }
            }

            // Serialize and store the updated cart items in the session
            HttpContext.Session.SetString("Cart", System.Text.Json.JsonSerializer.Serialize(cartItems));

            // Return the response
            return Json(new { success = true, message = "Item removed from the cart." });
        }
    }
}

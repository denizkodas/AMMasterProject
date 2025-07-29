namespace AMMasterProject.Helpers
{

    public class OrderV2Helper
    {
        #region Model


        private readonly MyDbContext _dbContext;
        ////private readonly MembershipHelper _membershipHelper;
        private readonly NotificationHelper _notificationhelper;
        private readonly ProductHelper _productHelper;
        private readonly GlobalHelper _globalhelper;
        private readonly UserHelper _userhelper;
        private readonly WebsettingHelper _websettinghelper;

     
        #endregion

        #region DI


        public OrderV2Helper(MyDbContext context, MembershipHelper membershipHelper, NotificationHelper notificationHelper, ProductHelper productHelper, GlobalHelper globalhelper, UserHelper userhelper, WebsettingHelper websettinghelper)
        {
            _dbContext = context;
            //_membershipHelper = membershipHelper;
            _notificationhelper = notificationHelper;
            _productHelper = productHelper;
            _globalhelper = globalhelper;
            _userhelper = userhelper;
            _websettinghelper = websettinghelper;
        }
        #endregion
        public string invoicenumberactive(int buyerid, string Id, string type)
        {

            string InvoiceNumber = "";

            var ordersToUpdate = _dbContext.OrderMasters.FirstOrDefault(u => u.BuyerId == buyerid && u.OrderStatus == "cart");
            if (ordersToUpdate == null)
            {
                InvoiceNumber = GlobalHelper.GetInvoiceNumber(Id, type);
            }
            else
            {
                InvoiceNumber = ordersToUpdate.InvoiceNumber;
            }

            return InvoiceNumber;

        }

      
    }
}

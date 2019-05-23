using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData
{
    public interface ICheckout
    {
        IEnumerable<Checkout> GetAll();
        Checkout GetById(int checkoutId);
        void Add(Checkout newCheckout);
        void CheckOutItem(int assetId, int libraryCardId);
        void CheckInItem(int assetId);
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int id);
        bool IsCheckedOut(int id);

        void PlaceHold(int assetId, int libraryCardId);
        string GetCurrentHoldPatronName(int id);
        string GetCurrentCheckoutPatron(int assetId);
        DateTime GetCurrentHoldPlaced(int id);
        IEnumerable<Hold> GetCurrentHolds(int id);
        Checkout GetLatestCheckout(int id);

        void MarkLost(int assetId);
        void MarkFound(int assetId);
    }
}

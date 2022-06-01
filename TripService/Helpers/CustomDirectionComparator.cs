using System.Collections.Generic;
using TripService.Models;

namespace TripService.Helpers
{
    public class CustomDirectionComparator : IComparer<PurchaseDirectionEvents>
    {
        public int Compare(PurchaseDirectionEvents x, PurchaseDirectionEvents y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            return x.EventDate.CompareTo(y.EventDate);
        }
    }
}
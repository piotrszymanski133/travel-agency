using System;
using CommonComponents.Models;

namespace TransportService.Services
{
    public class PriceCalculator
    {
        public static int CalculateTransportOfferPrice(TransportOffer transportOffer)
        {
            int transpotrTypeFactor;
            if (transportOffer.TransportName == "Own")
                return 0;
            else if (transportOffer.TransportName == "Bus")
                transpotrTypeFactor = 1;
            else
                transpotrTypeFactor = 3;
            return transportOffer.Persons * transpotrTypeFactor *
                   (Math.Abs(transportOffer.DestinationCity.GetHashCode()) % 200);
        }

        public static int CalculateTransportOfferPrice(string transportName, int persons, string destinationCity)
        {
            int transpotrTypeFactor;
            if (transportName == "Own")
                return 0;
            else if (transportName == "Bus")
                transpotrTypeFactor = 1;
            else
                transpotrTypeFactor = 3;
            return persons * transpotrTypeFactor * (Math.Abs(destinationCity.GetHashCode()) % 200);
        }
    }
}
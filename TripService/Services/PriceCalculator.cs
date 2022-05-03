using ApiGateway.Models;
using CommonComponents.Models;

namespace TripService.Services
{
    public class PriceCalculator
    {
        private static int NORMAL_PRICE_PER_PERSON = 140;
        private static int PREMIUM_PRICE_PER_PERSON = 200;
        public static int CalculateLowestPrice(Hotel hotelOffer, TripParameters tripParameters)
        {
            if (hotelOffer.IsOnlyPremiumAvailable)
                return CalculatePrice(tripParameters, PREMIUM_PRICE_PER_PERSON, hotelOffer.Stars);
            else
                return CalculatePrice(tripParameters, NORMAL_PRICE_PER_PERSON, hotelOffer.Stars);


        }

        private static int CalculatePrice(TripParameters tripParameters, int pricePerPerson, int stars)
        {
            int duration = (tripParameters.EndDate - tripParameters.StartDate).Days;
            return (int)((tripParameters.Adults * pricePerPerson +
                    tripParameters.ChildrenUnder18 * pricePerPerson +
                    tripParameters.ChildrenUnder10 * pricePerPerson * 0.7 +
                    tripParameters.ChildrenUnder3 * pricePerPerson * 0.5) * duration * (stars * 0.2));
        }
    }
}
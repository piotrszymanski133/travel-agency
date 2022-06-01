using System;
using System.Collections.Generic;
using CommonComponents.Models;
using TransportService.Models;

namespace TransportService.Services
{
    public class PriceCalculator
    {
        public static Dictionary<string, DistanceFactorTuple> distances = new()
        {
            {"Zanzibar", new DistanceFactorTuple { Distance = 6720, PriceFactor = 1.2 }},
            {"Cypr", new DistanceFactorTuple { Distance = 2154, PriceFactor = 1.12 }},
            {"Malta", new DistanceFactorTuple { Distance = 1841, PriceFactor = 0.76 }},
            {"Tunezja", new DistanceFactorTuple { Distance = 2186, PriceFactor = 1.01 }},
            {"Grecja", new DistanceFactorTuple { Distance = 1453, PriceFactor = 0.92 }},
            {"Bułgaria", new DistanceFactorTuple { Distance = 1109, PriceFactor = 0.84 }},
            {"Sri Lanka", new DistanceFactorTuple { Distance = 7345, PriceFactor = 0.87 }},
            {"Zjednoczone Emiraty Arabskie", new DistanceFactorTuple { Distance = 4282, PriceFactor = 1.21 }},
            {"Tajlandia", new DistanceFactorTuple { Distance = 8043, PriceFactor = 1.29 }},
            {"Egipt", new DistanceFactorTuple { Distance = 2902, PriceFactor = 0.79 }},
            {"Mauritius", new DistanceFactorTuple { Distance = 8827, PriceFactor = 1.03 }},
            {"Chorwacja", new DistanceFactorTuple { Distance = 828, PriceFactor = 0.87 }},
            {"Malediwy", new DistanceFactorTuple { Distance = 7288, PriceFactor = 0.91 }},
            {"Wyspy Zielonego Przylądka", new DistanceFactorTuple { Distance = 5558, PriceFactor = 1.14 }},
            {"Wyspy Kanaryjskie", new DistanceFactorTuple { Distance = 3965, PriceFactor = 1.3 }},
            {"Turcja", new DistanceFactorTuple { Distance = 1851, PriceFactor = 0.84 }},
            {"Madera", new DistanceFactorTuple { Distance = 3646, PriceFactor = 1.11 }},
            {"Madagaskar", new DistanceFactorTuple { Distance = 8424, PriceFactor = 0.75 }},
            {"Dominikana", new DistanceFactorTuple { Distance = 8066, PriceFactor = 1.12 }},
            {"Polska", new DistanceFactorTuple { Distance = 282, PriceFactor = 1.08 }}
        };
        public static int CalculateTransportOfferPrice(TransportOffer transportOffer)
        {
            double transpotrTypeFactor;
            if (transportOffer.TransportName == "Own")
                return 0;
            else if (transportOffer.TransportName == "Bus")
                transpotrTypeFactor = 0.25;
            else
                transpotrTypeFactor = 0.5;

            DistanceFactorTuple kilometers;
            var ret = distances.TryGetValue(transportOffer.DestinationCountry, out kilometers) 
                ? kilometers : new DistanceFactorTuple { Distance = 255, PriceFactor = 1.0};
            return (int) (transportOffer.Persons * transpotrTypeFactor * ret.Distance * ret.PriceFactor);
        }

        public static int CalculateTransportOfferPrice(string transportName, int persons, string destinationCountry)
        {
            double transpotrTypeFactor;
            if (transportName == "Own")
                return 0;
            else if (transportName == "Bus")
                transpotrTypeFactor = 0.25;
            else
                transpotrTypeFactor = 0.5;

            DistanceFactorTuple kilometers;
            var ret = distances.TryGetValue(destinationCountry, out kilometers) 
                ? kilometers : new DistanceFactorTuple { Distance = 255, PriceFactor = 1.0};
            return (int) (persons * transpotrTypeFactor * ret.Distance * ret.PriceFactor);
        }
    }
}
using System;
using System.Collections.Generic;
using CommonComponents.Models;

namespace TransportService.Services
{
    public class PriceCalculator
    {
        public static Dictionary<string, int> distances = new()
        {
            {"Zanzibar",6720},
            {"Cypr",2154},
            {"Malta",1841},
            {"Tunezja",2186},
            {"Grecja",1453},
            {"Bułgaria",1109},
            {"Sri Lanka",7345},
            {"Zjednoczone Emiraty Arabskie",4282},
            {"Tajlandia",8043},
            {"Egipt",2902},
            {"Mauritius",8827},
            {"Chorwacja",828},
            {"Malediwy",7288},
            {"Wyspy Zielonego Przylądka",5558},
            {"Wyspy Kanaryjskie",3965},
            {"Turcja",1851},
            {"Madera",3646},
            {"Madagaskar",8424},
            {"Dominikana",8066},
            {"Polska",282}
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

            int kilemeters=255;
            var ret = distances.TryGetValue(transportOffer.DestinationCountry, out kilemeters) ? kilemeters : 255;
            return (int) (transportOffer.Persons * transpotrTypeFactor * ret);
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

            int kilemeters=255;
            var ret = distances.TryGetValue(destinationCountry, out kilemeters) ? kilemeters : 255;
            return (int) (persons * transpotrTypeFactor * ret);
        }
    }
}
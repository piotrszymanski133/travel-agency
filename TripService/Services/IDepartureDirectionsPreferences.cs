using TripService.Models;

namespace TripService.Services
{
    public interface IDepartureDirectionsPreferences
    {
        public void AddDirectionEvent(PurchaseDirectionEvents pEvent);
        public void AddPreferencesEvent(PurchasePreferencesEvents pEvent);
        
        public void GetTopDirectionPreference();

        public PopularGeneralPreferences GetGeneralPreferences();
        public string GetCountryPreferences();
    }
}
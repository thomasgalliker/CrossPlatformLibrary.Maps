using Microsoft.Practices.ServiceLocation;

namespace MapsSample.ViewModels
{
    public class ViewModelLocator
    {
        public RestaurantMapViewModel RestaurantMapViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RestaurantMapViewModel>();
            }
        }
    }
}
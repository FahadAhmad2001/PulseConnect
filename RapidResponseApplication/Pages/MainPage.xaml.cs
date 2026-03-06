using RapidResponseApplication.Models;
using RapidResponseApplication.PageModels;

namespace RapidResponseApplication.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}
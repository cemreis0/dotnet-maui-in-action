using System.Collections.ObjectModel;
using CellBoutique.Models;


namespace CellBoutique
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Product> Products { get; set; } = new();

        public MainPage()
        {
            InitializeComponent();

            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Populate the Products collection with sample data
            Products.Add(new Product
            {
                Title = "Stylish Handbag",
                Description = "A sleek and stylish handbag for all occasions.",
                Price = 59.99,
                Image = "https://picsum.photos/100/150"
            });

            Products.Add(new Product
            {
                Title = "Elegant Watch",
                Description = "A timeless piece to elevate your style.",
                Price = 129.99,
                Image = "https://picsum.photos/100/150"
            });

            Products.Add(new Product
            {
                Title = "Running Shoes",
                Description = "Comfortable and durable shoes for running.",
                Price = 89.99,
                Image = "https://picsum.photos/100/150"
            });
        }

    }

}

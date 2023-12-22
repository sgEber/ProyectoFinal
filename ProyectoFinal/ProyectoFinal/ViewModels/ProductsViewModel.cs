using ProyectoFinal.Models;
using ProyectoFinal.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProyectoFinal.ViewModels
{
    public class ProductsViewModel : INotifyPropertyChanged
    {
        private ApiService _apiService = new ApiService();
        private List<Product> _products;
        private Product _newProduct;
        private bool _isAddingNewProduct;

        public List<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        // Propiedades para un nuevo producto
        public Product NewProduct
        {
            get { return _newProduct; }
            set
            {
                _newProduct = value;
                OnPropertyChanged(nameof(NewProduct));
            }
        }

        // Propiedad que controla la visibilidad de los campos de entrada para nuevo producto
        public bool IsAddingNewProduct
        {
            get { return _isAddingNewProduct; }
            set
            {
                _isAddingNewProduct = value;
                OnPropertyChanged(nameof(IsAddingNewProduct));
            }
        }

        // Comandos
        public ICommand LoadProductsCommand { get; }
        public ICommand AddNewProductCommand { get; }
        public ICommand SaveNewProductCommand { get; }

        public ProductsViewModel()
        {
            LoadProductsCommand = new Command(async () => await LoadProducts());
            AddNewProductCommand = new Command(PrepareNewProduct);
            SaveNewProductCommand = new Command(async () => await AddProduct(NewProduct));

            // Inicializar el nuevo producto
            NewProduct = new Product();
        }

        private void PrepareNewProduct()
        {
            // Preparar un nuevo objeto Product para la entrada del usuario
            NewProduct = new Product();
            IsAddingNewProduct = true; // Mostrar los campos de entrada
        }

        public async Task LoadProducts()
        {
            Products = await _apiService.GetProductsAsync();
        }

        public async Task AddProduct(Product product)
        {
            var newProduct = await _apiService.AddProductAsync(product);
            if (newProduct != null)
            {
                Products.Add(newProduct);
                OnPropertyChanged(nameof(Products));
                IsAddingNewProduct = false; // Ocultar los campos de entrada después de agregar
            }
        }

        public async Task UpdateProduct(Product product)
        {
            await _apiService.UpdateProductAsync(product);
            var productToUpdate = Products.FirstOrDefault(p => p.Id == product.Id);
            if (productToUpdate != null)
            {
                int index = Products.IndexOf(productToUpdate);
                Products[index] = product;
                OnPropertyChanged(nameof(Products));
            }
        }

        public async Task DeleteProduct(int productId)
        {
            await _apiService.DeleteProductAsync(productId);
            var productToRemove = Products.FirstOrDefault(p => p.Id == productId);
            if (productToRemove != null)
            {
                Products.Remove(productToRemove);
                OnPropertyChanged(nameof(Products));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

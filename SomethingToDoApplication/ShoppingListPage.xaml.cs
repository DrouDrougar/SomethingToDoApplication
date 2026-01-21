using SomethingToDoApp.Models;
using SomethingToDoApp.Sqlite;
using SomethingToDoApp.ViewModels;
using SomethingToDoApplication.Models;
using SomethingToDoApplication.ViewModels;
using SQLite;

namespace SomethingToDoApplication;

public partial class ShoppingListPage : ContentPage
{

    private readonly LocalDbService _dbService;
    private int _editTaskId;
    private readonly SQLiteAsyncConnection _connection;

    public ShoppingListPage(LocalDbService dbService)
    {
        InitializeComponent();

        BindingContext = new ShoppingViewModel();

        _dbService = dbService;

        Task.Run(async () => listView.ItemsSource = await _dbService.GetShoppingTasks());
    }


    private async void AddButton_Clicked(object sender, EventArgs e)
    {
        string taskName = AddShoppingNameField.Text;
        string taskDescription = AddShoppingDescriptionField.Text;
        int shoppingQuantity = int.TryParse(AddShoppingQuantityField.Text, out var quantity) ? quantity : 0;

        if (_editTaskId == 0)
        {
            await _dbService.CreateShoppingTask(new ShoppingTaskModel
            {
                ShoppingName = taskName,
                ShoppingDescription = taskDescription,
                ShoppingQuantity = shoppingQuantity
            });
        }
        else
        {
            await _dbService.UpdateShoppingTask(new ShoppingTaskModel
            {
                Id = _editTaskId,
                ShoppingName = taskName,
                ShoppingDescription = taskDescription,
                ShoppingQuantity = shoppingQuantity
            });

            _editTaskId = 0;
        }

        AddShoppingDescriptionField.Text = string.Empty;
        AddShoppingNameField.Text = string.Empty;
        AddShoppingQuantityField.Text = string.Empty;
        listView.ItemsSource = await _dbService.GetShoppingTasks();
    }

    private async void CollectionView_SlectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var item = e.CurrentSelection.FirstOrDefault() as ShoppingTaskModel;
        if (item == null) return;

        var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");
        switch (action)
        {
            case "Edit":
                _editTaskId = item.Id;
                AddShoppingNameField.Text = item.ShoppingName;
                AddShoppingDescriptionField.Text = item.ShoppingDescription;
                AddShoppingQuantityField.Text = item.ShoppingQuantity.ToString();
                break;
            case "Delete":
                await _dbService.DeleteShoppingTask(item);
                listView.ItemsSource = await _dbService.GetShoppingTasks();
                break;
        }
    }
}
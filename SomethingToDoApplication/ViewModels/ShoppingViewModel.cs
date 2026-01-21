using SomethingToDoApp.Models;
using SomethingToDoApp.ViewModels;
using SomethingToDoApplication.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SomethingToDoApplication.ViewModels
{
    public class ShoppingViewModel : BaseViewModel
    {
          public ObservableCollection<ShoppingTaskModel> ShoppingCollection { get; set; }

        public ShoppingViewModel()
        {
            var ShoppingCollection = new ObservableCollection<ShoppingTaskModel>
            {
                new ShoppingTaskModel()
                {
                    Id = 1,
                    ShoppingName = "Test",
                    ShoppingDescription = "This is a test description",
                    ShoppingQuantity = 1
                }
            };
        }
    }
}


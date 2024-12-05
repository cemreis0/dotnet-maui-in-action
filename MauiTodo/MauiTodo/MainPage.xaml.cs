using MauiTodo.Data;
using MauiTodo.Models;
using System.Collections.ObjectModel;

namespace MauiTodo
{
    public partial class MainPage : ContentPage
    {

        readonly Database _database;

        public ObservableCollection<TodoItem> Todos { get; set; } = new();

        public MainPage()
        {
            InitializeComponent();

            _database = new Database();

            _ = Initialize();
        }

        private async Task Initialize()
        {
            var todos = await _database.GetTodos();
           
            foreach (TodoItem todo in todos)
            {
                Todos.Add(todo);
            }
        }

        private async void AddButtonClicked(object sender, EventArgs e)
        {
            var todo = new TodoItem
            {
                Due = DueDatePicker.Date,
                Title = TodoTitleEntry.Text
            };

            var inserted = await _database.AddTodo(todo);

            if (inserted != 0)
            {
                Todos.Add(todo);
                TodoTitleEntry.Text = String.Empty;
                DueDatePicker.Date = DateTime.Now;
            }
        }

        private async void SwipeItem_Invoked(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;

            await App.Current.MainPage.
            DisplayAlert(item.Text, $"You invoked the {item.Text} action.", "OK");
        }
}

}

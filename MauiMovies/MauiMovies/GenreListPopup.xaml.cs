using CommunityToolkit.Maui.Views;
using MauiMovies.Models;
using System.Collections.ObjectModel;

namespace MauiMovies;

public partial class GenreListPopup : Popup
{
	public ObservableCollection<UserGenre> Genres { get; set; }

	public GenreListPopup()
	{
		BindingContext = this;
		this.Genres = new ObservableCollection<UserGenre>();
	}
}
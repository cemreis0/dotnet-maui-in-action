﻿using CommunityToolkit.Maui.Views;
using MauiMovies.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Windows.Input;

namespace MauiMovies
{
    public partial class MainPage : ContentPage
    {

        string _apiKey = "";
        string _baseUri = "https://api.themoviedb.org/3/";
        string _imageBaseUrl = "https://image.tmdb.org/t/p/w500";
        private TrendingMovies _movieList;
        private GenreList _genres;
        public ObservableCollection<Genre> Genres { get; set; } = [];
        public ObservableCollection<MovieResult> Movies { get; set; } = [];
        public bool IsLoading { get; set; }
        private readonly HttpClient _httpClient;
        private List<UserGenre> _genreList { get; set; } = [];

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            _httpClient = new HttpClient { BaseAddress = new Uri(_baseUri) };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            IsLoading = true;
            OnPropertyChanged(nameof(IsLoading));

            _genres = await _httpClient.GetFromJsonAsync<GenreList>($"genre/movie/list?api_key={_apiKey}&language=en-US");

            _movieList = await _httpClient.GetFromJsonAsync<TrendingMovies>($"trending/movie/week?api_key={_apiKey}&language=en-US");

            foreach (var movie in _movieList.results)
            {
                movie.poster_path = $"{_imageBaseUrl}{movie.poster_path}";
            }

            foreach (var genre in _genres.genres)
            {
                _genreList.Add(new UserGenre
                {
                    id = genre.id,
                    name = genre.name,
                    Selected = true
                });
            }

            LoadFilteredMovies();

            IsLoading = false;
            OnPropertyChanged(nameof(IsLoading));
        }

        private void LoadFilteredMovies()
        {
            Movies.Clear();
            if (_genreList.Any(g => g.Selected))
            {
                var selectedGenreIds = _genreList.Where(g => g.Selected).Select(g => g.id);

                foreach (var movie in _movieList.results)
                {
                    if (movie.genre_ids.Any(id => selectedGenreIds.Contains(id)))
                    {
                        Movies.Add(movie);
                    }
                }
            }
            else
            {
                foreach(var movie in _movieList.results)
                {
                    Movies.Add(movie);
                }
            }
        }

        public ICommand ChooseGenres => new Command(async () => await ShowGenreList());

        public ICommand ShowMovie => new Command<MovieResult>((movie) => ShowMovieDetails(movie));

        private async Task ShowGenreList()
        {
            var genrePopup = new GenreListPopup(_genreList);
 
            var selected = await this.ShowPopupAsync(genrePopup);

            if ((bool) selected)
            {
                Genres.Clear();
                foreach (var genre in _genreList)
                {
                    if (genre.Selected)
                    {
                        Genres.Add(new Genre
                        {
                            name = genre.name
                        });
                    }
                }

                LoadFilteredMovies();
            }
        }

        private void ShowMovieDetails(MovieResult movie)
        {
            if (movie == null)
            {
                Debug.WriteLine("Movie is null, cannot display popup.");
                return;
            }

            var moviePopup = new MovieDetailsPopup(movie, _genres.genres);

            if (moviePopup == null || moviePopup.Content == null)
            {
                Debug.WriteLine("Failed to create MovieDetailsPopup or its content.");
                return;
            }

            this.ShowPopup(moviePopup);
        }
    }
}

using People.Models;
using People.ViewModels;
using System.Collections.Generic;

namespace People;

public partial class MainPage : ContentPage
{
    public mathiasveraPaginaPrincipalViewModel ViewModel { get; }

    public MainPage()
    {
        InitializeComponent();
        ViewModel = new mathiasveraPaginaPrincipalViewModel();
        BindingContext = ViewModel;
    }

    public void OnNewButtonClicked(object sender, EventArgs args)
    {
        ViewModel.StatusMessage = "";

        ViewModel.AddPersonCommand.Execute(null);
    }

    public void OnGetButtonClicked(object sender, EventArgs args)
    {
        ViewModel.StatusMessage = "";

        ViewModel.GetPeopleCommand.Execute(null);
    }
}


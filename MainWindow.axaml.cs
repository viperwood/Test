using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attempt4.Models;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace Attempt4;

public partial class MainWindow : Window
{
    public bool isAdmin = false;
    public MainWindow()
    {
        InitializeComponent();
        InitializeWindow();
        DataContext = isAdmin;
    }
    public MainWindow(bool isAdmin)
    {
        InitializeComponent();
        InitializeWindow();
        DataContext = isAdmin;
        this.isAdmin = isAdmin;
    }

    private void SearchTextBoxOnKeyUp(object? sender, KeyEventArgs e)
    {
        loadServices();
    }

    private void InitializeWindow()
    {
        SortComboBox.SelectionChanged += SortComboBox_SelectionChanged;
        FilterComboBox.SelectionChanged += FilterComboBox_LostFocus;
        AddServiceButton.Click += AddServices_Click;
        SearchTextBox.KeyUp += SearchTextBoxOnKeyUp;
        loadServices();
    }


    private async void AddServices_Click(object? sender, RoutedEventArgs e)
    {
        AddingWindow addingService = new AddingWindow();
        await addingService.ShowDialog(this);
        loadServices();
    }
    private async void DeleteServices_Click(object? sender, RoutedEventArgs e)
    {
        int button = (int)(sender as Button).Tag;
        Helper.Database.Services.Remove(Helper.Database.Services.Find(button));
        Helper.Database.SaveChanges();
        loadServices();
    }
    private async void RedactServices_Click(object? sender, RoutedEventArgs e)
    {
        int button = (int)(sender as Button).Tag;
        AddingWindow addingService = new AddingWindow(button);
        addingService.ShowDialog(this);
        loadServices();
    }
    
    private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        loadServices();
    }
    private void FilterComboBox_LostFocus(object sender, RoutedEventArgs e)
    {
        loadServices();
    }
    

    private void loadServices()
    {
        List<Service> services;

        switch (FilterComboBox.SelectedIndex)
        {
            case 1:
                services = Helper.Database.Services.Where(x => x.Discount >= 0 && x.Discount < 5).ToList();
                break;
            case 2:
                services = Helper.Database.Services.Where(x => x.Discount >= 5 && x.Discount < 15).ToList();
                break;
            case 3:
                services = Helper.Database.Services.Where(x => x.Discount >= 15 && x.Discount < 30).ToList();
                break;
            case 4:
                services = Helper.Database.Services.Where(x => x.Discount >= 30 && x.Discount < 70).ToList();
                break;
            case 5:
                services = Helper.Database.Services.Where(x => x.Discount >= 70 && x.Discount < 100).ToList();
                break;
            case 0:
                default:
                services = Helper.Database.Services.ToList();
                break;
        }


   
        
        
        
        
        
        if (SearchTextBox.Text != null)
        {
            services = services.Where(t => t.Title.Contains(SearchTextBox.Text)).ToList();
        }
        
        
        

        if (SortComboBox.SelectedIndex == 0)
        {
            services = services.OrderBy(x => x.FullCost).ToList();
        }
        else
        {
            services = services.OrderByDescending(x => x.FullCost).ToList();
        }

        
        
        ServiceListBox.Items = services.Select(x => new
        {
            Title = x.Title,
            Discount = x.Discount,
            Id = x.Id,
            Cost = x.Cost,
            IsAdmin = isAdmin,
            OldPriceString = x.Discount != null ? $"{Math.Round(x.Cost,2)}" : "",
            CurrentPriceString = $"{x.FullCost} рублей за {x.Durationinseconds} минут",
            DiscountString = x.Discount != null ? $"* скидка {x.Discount}%" : "",
            Color = x.Discount != 0 ? Brushes.LightGreen : Brushes.White,
            x.PathToMainImage
        });
        
    }



}


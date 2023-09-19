using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Attempt4.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Attempt4;

public partial class AddingWindow : Window
{
    public Service service;

    private readonly FileDialogFilter fileFilter = new FileDialogFilter()
    {
        Extensions = new List<string>() { "png", "jpg", "jpeg" },
        Name = "Image File"
    };
    
    
    public AddingWindow()
    {
        InitializeComponent();
        service = new Service();
        
        SaveButton.Click += AddButton_Click;
        SelectFileButton.Click += SelectFileButton_Click;
        AddImageButton.Click += AddImageButton_Click;
        //Closing += AddingService_Closing;
        
        DataContext = service;
    }
    public AddingWindow(int Id)
    {
        InitializeComponent();
        service = Helper.Database.Services.Find(Id);
            
        SaveButton.Click += SaveButton_Click;
        SelectFileButton.Click += SelectFileButton_Click;
        AddImageButton.Click += AddImageButton_Click;
        Closing += AddingService_Closing;
        DataContext = service;
    }

    private void AddingService_Closing(object? sender, CancelEventArgs e)
    {
        //Close();
    }

    private async void SelectFileButton_Click(object? sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filters?.Add(fileFilter);
        string[]? result = await dialog.ShowAsync(this);
        if (result == null)
        {
            return;
        }
        string imageName = Path.GetFileName(result[0]);
        File.Copy(result[0], $"./assets/Images/{imageName}", true);
        service.Mainimagepath = $"\\Images\\{imageName}";
        ImagePathTextBox.Text = service.Mainimagepath;
    }
    private async void AddImageButton_Click(object? sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filters?.Add(fileFilter);
        string[]? result = await dialog.ShowAsync(this);
        if (result == null)
        {
            return;
        }

        string imageName = Path.GetFileName(result[0]);
        Directory.CreateDirectory("./assets/Servicephotos/");
        File.Copy(result[0], $"./assets/Servicephotos/{imageName}", true);
        
        service.Servicephotos.Add(new Servicephoto()
        {
            Photopath = imageName
        });
        ImagesListBox.Items = service.Servicephotos.ToList();
    }
    private void DeleteImageButton_Click(object? sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        service.Servicephotos.Remove((Servicephoto)button.Tag);
        try
        {
            Helper.Database.Servicephotos.Remove((Servicephoto)button.Tag);
        }
        catch 
        {
            
        }
        ImagesListBox.Items = service.Servicephotos.ToList();
    }

    private void SaveButton_Click(object? sender, RoutedEventArgs e)
    {
        Helper.Database.SaveChanges();
        this.Close();
    }
    
    private void AddButton_Click(object? sender, RoutedEventArgs e)
    {
        Helper.Database.Add(service);
        Helper.Database.SaveChanges();
        this.Close();
    }
    
}
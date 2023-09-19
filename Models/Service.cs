using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;

namespace Attempt4.Models;

public partial class Service
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public decimal Cost { get; set; }

    public int Durationinseconds { get; set; }

    public string? Description { get; set; }

    public decimal? Discount { get; set; }

    public string? Mainimagepath { get; set; }

    public virtual ICollection<Clientservice> Clientservices { get; set; } = new List<Clientservice>();

    public virtual ICollection<Servicephoto> Servicephotos { get; set; } = new List<Servicephoto>();

   

    public decimal? FullCost 
    { 
        get
        {
            return Discount != null ? Math.Round((decimal)(Cost - (Cost * (Discount / 100))), 2) : Cost;
        }
    }  
    
    public Bitmap? PathToMainImage
    {
        get
        {
            return new Bitmap($"{(Mainimagepath != null ? Mainimagepath.Trim() : "./ServiceSchools/online lessons.jpg")}");
        }
    }
    
}

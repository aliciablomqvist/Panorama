using PanoramaApp.Data;
using PanoramaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PanoramaApp.Services
{
public class TrailerPlayer
{
    public string GetTrailerUrl(Movie movie)
    {
        return movie.TrailerUrl;
    }

    public void Play(string trailerUrl)
    {
        Console.WriteLine($"Playing trailer: {trailerUrl}");
    }
}
}

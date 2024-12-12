// <copyright file="Privacy.cshtml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PanoramaApp.Pages;

public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> logger;

    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        this.logger = logger;
    }

    public void OnGet()
    {
    }
}
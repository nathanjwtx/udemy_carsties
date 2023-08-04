﻿namespace AuctionService.DTOs;

public class UpdateAuctionDto
{
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int? Mileage { get; set; }
    public int? Year { get; set; }
}

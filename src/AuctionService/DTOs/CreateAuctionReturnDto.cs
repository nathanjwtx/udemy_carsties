using System.Text.Json.Serialization;
using AuctionService.Models;

namespace AuctionService;
public class CreateAuctionReturnDto
{
    public string Id { get; set;}
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string ImageUrl { get; set; }
    public string Seller { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Status Status { get; set; }
}

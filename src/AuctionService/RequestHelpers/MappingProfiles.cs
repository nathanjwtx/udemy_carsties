using AuctionService.DTOs;
using AuctionService.Models;
using AutoMapper;

namespace AuctionService.RequestHelpers;
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
        CreateMap<Item, AuctionDto>();
        CreateMap<CreateAuctionDto, Auction>().ForMember(d => d.Item, o => o.MapFrom(s => s));
        CreateMap<CreateAuctionDto, Item>();
        CreateMap<Item, CreateAuctionReturnDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.Make, o => o.MapFrom(s => s.Make))
            .ForMember(d => d.Model, o => o.MapFrom(s => s.Model))
            .ForMember(d => d.Year, o => o.MapFrom(s => s.Year))
            .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.ImageUrl))
            .ForMember(d => d.Seller, o => o.MapFrom(s => s.Auction.Seller))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Auction.Status));
    }
}

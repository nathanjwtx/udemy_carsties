using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;
using AuctionService.DTOs;
using AuctionService.Models;
using AutoMapper;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly AuctionDbContext _context;

    public AuctionsController(IMapper mapper, AuctionDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
    {
        var auctions = await _context.Auctions
            .Include(x => x.Item)
            .OrderBy(x => x.Item.Make)
            .ToListAsync();

        return _mapper.Map<List<AuctionDto>>(auctions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        var auction = await _context.Auctions
        .Include(x => x.Item)
        .Where(x => x.Id == id)
        .FirstOrDefaultAsync();

        if (auction != null)
        {
            return _mapper.Map<AuctionDto>(auction);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<CreateAuctionReturnDto>> MakeNewAuction([FromBody] CreateAuctionDto createAuctionDto)
    {
        if (createAuctionDto == null) return BadRequest();

        var auction = _mapper.Map<Auction>(createAuctionDto);

        await _context.AddAsync<Auction>(auction);
        var result = await _context.SaveChangesAsync();
        
        // result will return +1 for each change successfully saved
        if (result > 0)
        {
            return Created($"/api/auctions/{auction.Id}", _mapper.Map<CreateAuctionReturnDto>(auction.Item));
        }
    
        return BadRequest("Something bad happened!"); 
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Auction>> UpdateAuction([FromBody] UpdateAuctionDto updateAuctionDto, Guid id)
    {
        var auction = await _context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(a => a.Id == id)
            .Select(a => new UpdateAuctionDto
            {
                Color = a.Item.Color,
                Make = a.Item.Make,
                Mileage = a.Item.Mileage,
                Model = a.Item.Model,
                Year = a.Item.Year
            }).ConfigureAwait(false);

        // if (auction == null) return BadRequest("Oops, something went wrong!");
        //
        // auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        // if (!string.IsNullOrEmpty(updateAuctionDto.Model))
        // {
        //     auction.Item.Model = updateAuctionDto.Model;
        // }
        // auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;
        // auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Year;
        // auction.Item.Color = updateAuctionDto.Color;
        //
        // _context.Auctions.Update(auction);
        // await _context.SaveChangesAsync().ConfigureAwait(false);

        return Ok(auction);
    }
}

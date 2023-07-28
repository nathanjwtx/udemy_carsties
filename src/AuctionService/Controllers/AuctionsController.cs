﻿using System.Reflection.Metadata.Ecma335;
using AuctionService.DTOs;
using AuctionService.Models;
using AutoMapper;
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
    public async Task<ActionResult<Item>> MakeNewAuction([FromBody] CreateAuctionDto createAuctionDto)
    {
        if (createAuctionDto == null) return BadRequest();

        var auction = _mapper.Map<Auction>(createAuctionDto);

        await _context.AddAsync<Auction>(auction);
        await _context.SaveChangesAsync();

        return Ok();
    }
}
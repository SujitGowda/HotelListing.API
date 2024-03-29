﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Data.Contracts;
using AutoMapper;
using HotelListing.API.Data.Models.Hotels;
using Microsoft.AspNetCore.Authorization;
using HotelListing.API.Data.Models;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HotelsController : ControllerBase
    {
       // private readonly HotelListingDbContext _context;
        private readonly IHotelsRepository _hotelsRepository;
        private readonly IMapper _mapper;

        public HotelsController(IHotelsRepository hotelsRepository,IMapper mapper)
        {
           // _context = context;
            this._hotelsRepository = hotelsRepository;
            this._mapper = mapper;
        }

        // GET: api/Hotels
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<HotelsModel>>> GetHotels()
        {
            var hotels = await _hotelsRepository.GetAllAsync();
            return Ok(_mapper.Map<List<HotelsModel>>(hotels));
        }

        [HttpGet]
        public async Task<ActionResult<PageResult<HotelsModel>>> GetPagedHotels([FromQuery] QueryParameters queryParameters)
        {
            var pagedPagedResult = await _hotelsRepository.GetAllAsync<HotelsModel>(queryParameters);// countries.ToListAsync();
            //var Records = _mapper.Map<List<HotelsModel>>(country);
            return Ok(pagedPagedResult);
        }


        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelsModel>> GetHotel(int id)
        {
            var hotel = await _hotelsRepository.GetAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<HotelsModel>(hotel));
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelsModel hotelsModel)
        {
            if (id != hotelsModel.Id)
            {
                return BadRequest();
            }

            // _hotelsRepository.Entry(hotel).State = EntityState.Modified;
            var hotel = await _hotelsRepository.GetAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            _mapper.Map(hotelsModel, hotel);
            try
            {
                await _hotelsRepository.UpdateAsync(hotel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await HotelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDetails hotelModel)
        {
            var hotel= _mapper.Map<Hotel>(hotelModel);
            await _hotelsRepository.AddAsync(hotel);

            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _hotelsRepository.GetAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            await _hotelsRepository.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> HotelExists(int id)
        {
            return await _hotelsRepository.Exists(id);
        }
    }
}

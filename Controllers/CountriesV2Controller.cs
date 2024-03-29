﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Data.Models.Country;
using AutoMapper;
using HotelListing.API.Data.Contracts;
using Microsoft.AspNetCore.Authorization;
using HotelListing.API.Data.Exceptions;
using Microsoft.AspNetCore.OData.Query;

namespace HotelListing.API.Controllers
{
    [Route("api/v{version:apiVersion}/countries")]
    [ApiController]
    [ApiVersion("2.0")]
    public class CountriesV2Controller : ControllerBase
    {
        private readonly ICountriesRepository _countriesRepository;

        //private readonly HotelListingDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CountriesV2Controller> _logger;

        public CountriesV2Controller(ICountriesRepository countriesRepository,IMapper mapper,ILogger<CountriesV2Controller> logger)
        {
            //_context = context;
            this._countriesRepository = countriesRepository;
            this._mapper = mapper;
            this._logger = logger;
        }

        // GET: api/Countries
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<GetCountryModel>>> Getcountries()
        {
            var country = await _countriesRepository.GetAllAsync();// countries.ToListAsync();
            var Records =_mapper.Map<List<GetCountryModel>>(country);
            return Ok(Records);
        }

        // GET: api/Countries/5  
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDetailsModel>> GetCountry(int id)
        {
            var country = await _countriesRepository.GetDetails(id);

            if (country == null)
            {
                //_logger.LogWarning($"No Record found in {nameof(GetCountry)}. with ID {id}.");
                //return NotFound();
                throw  new NotFoundException(nameof(GetCountry),id);
            }

            var countryDetails = _mapper.Map<CountryDetailsModel>(country);
            return countryDetails;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryModel updateCountryDetails)
        {
            if (id != updateCountryDetails.Id)
            {
                return BadRequest("INvalid Record ID");
            }

            var country = await _countriesRepository.GetAsync(id);//countries.FindAsync(id);
            if (country == null)
            {
                //return NotFound();
                throw new NotFoundException(nameof(GetCountry), id);
            }

            _mapper.Map(updateCountryDetails, country);

            try
            {
                await _countriesRepository.UpdateAsync(country);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CountryExists(id))
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

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryModel createCounty)
        {
            //var countryOld = new Country
            //{
            //    Name = createCounty.Name,
            //    ShortName = createCounty.ShortName,
            //};
            var country= _mapper.Map<Country>(createCounty);
            //_countriesRepository.countries.Add(country);
            await _countriesRepository.AddAsync(country);

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _countriesRepository.GetAsync(id);
            if (country == null)
            {
                //return NotFound();
                throw new NotFoundException(nameof(GetCountry), id);
            }
            await _countriesRepository.DeleteAsync(id);
            //_countriesRepository.countries.Remove(country);
            //await _countriesRepository.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _countriesRepository.Exists(id);
        }
    }
}

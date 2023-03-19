using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using HotelListing.API.Data.Contracts;
using HotelListing.API.Data.Models.Country;
using HotelListing.API.Data.Models;
using HotelListing.API.Data.Exceptions;

namespace HotelListing.API.Controllers
{
    [Route("api/v{version:apiVersion}/countries")]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    public class CountriesController : ControllerBase
    {
        private readonly ICountriesRepository _countriesRepository;

        //private readonly HotelListingDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController(ICountriesRepository countriesRepository,IMapper mapper,ILogger<CountriesController> logger)
        {
            //_context = context;
            this._countriesRepository = countriesRepository;
            this._mapper = mapper;
            this._logger = logger;
        }

        // GET: api/Countries
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<GetCountryModel>>> Getcountries()
        {
            var countries = await _countriesRepository.GetAllAsync<GetCountryModel>();
            return Ok(countries);
        }// GET: api/Countries/?StartIndex=0&PageSize=15&PageNumber=1
        [HttpGet]
        public async Task<ActionResult<PageResult<GetCountryModel>>> GetPagedcountries([FromQuery] QueryParameters queryParameters)
        {
            var pagedCountryResult = await _countriesRepository.GetAllAsync<GetCountryModel>(queryParameters);// countries.ToListAsync();
            //var Records = _mapper.Map<List<GetCountryModel>>(country);
            return Ok(pagedCountryResult);
        }

        // GET: api/Countries/5  
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDetailsModel>> GetCountry(int id)
        {


            var country = await _countriesRepository.GetDetails(id);
            return Ok(country);
           
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryModel updateCountryDetails)
        {
            try
            {
                await _countriesRepository.UpdateAsync(id, updateCountryDetails);
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
        public async Task<ActionResult<CountryDetailsModel>> PostCountry(CreateCountryModel createCounty)
        {
            var country = await _countriesRepository.AddAsync<CreateCountryModel, GetCountryModel>(createCounty);
            return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            await _countriesRepository.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _countriesRepository.Exists(id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiftCards.BusinessLayer;
using GiftCards.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GiftCards_DotNet_MongoDb.Controllers
{
  //[Route("api/[controller]/[action]")]
    [ApiController]
    public class ContactUsController : Controller
    {
        private readonly IContactUsRepository _contactUsRepository;

        public ContactUsController(IContactUsRepository contactUsRepository)
        {
            _contactUsRepository = contactUsRepository;
        }

        [HttpGet]
       [Route("api/contactUs")]
        public async Task<ActionResult<IEnumerable<ContactUs>>> Get()
        {
            var contactUs = await _contactUsRepository.AllContactUs();
            return Ok(contactUs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactUs>> Get(string id)
        {
            var info = await _contactUsRepository.Get(id);
            return Ok(info);
        }

        [HttpPost]
       [Route("api/contactUs/addValues")]
        public ActionResult Post(ContactUs model)
        {
           // ContactUs model = new ContactUs { Name = "abc", Email="aaaa", Address="aaaaa" };

            try
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                    return BadRequest("Please enter Name");
                else if (string.IsNullOrWhiteSpace(model.Email))
                    return BadRequest("Please enter Email");
                else if (string.IsNullOrWhiteSpace(model.Address))
                    return BadRequest("Please enter Address");

                _contactUsRepository.Create(model);

                return Ok("Your product has been added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
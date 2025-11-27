
using backend.repository;
using backend.models;
using Microsoft.AspNetCore.Mvc;

/*
We will implement a Wishlist-like queue system for users to request books that are currently unavailable
or they are currently already borrow one.
*/
namespace backend.controllers
{
    [ApiController]
    [Route("api/queue/[controller]")]
    public class QueuingController : ControllerBase
    {
        
    }
}
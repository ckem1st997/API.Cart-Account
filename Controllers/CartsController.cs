using API.Cart_Account.Models;
using API.Cart_Account.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Cart_Account.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartsController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public ActionResult<List<Cart>> Get() =>
            _cartService.Get();

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public ActionResult<Cart> Get(string id)
        {
            var book = _cartService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpGet("{cartid}", Name = "GetAllCartId")]
        public ActionResult<List<Cart>> GetAllCartId(string cartid)
        {
            var book = _cartService.GetAllCartId(cartid);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost("addCart")]
        public ActionResult<Cart> Create(Cart book)
        {
            try
            {
                var check = _cartService.GetCSL(book);
                if (check != null)
                {
                    check.Count += book.Count;
                    _cartService.Update(check.Id, check);
                    return Ok(true);
                }
                else if (_cartService.GetAllCartId(book.CartId).Count >= 10)
                    return Ok(false);
                else
                {
                    book.DateCreated = DateTime.Now;
                    _cartService.Create(book);
                    return Ok(true);
                }
            }
            catch (Exception)
            {
                return Ok(false);
            }

            // return CreatedAtRoute("GetBook", new { id = book.Id.ToString() }, book);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Cart bookIn)
        {
            var book = _cartService.Get(id);

            if (book == null)
            {
                return Ok(false);
            }

            _cartService.Update(id, bookIn);

            return Ok(true);
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = _cartService.Get(id);

            if (book == null)
            {
                return Ok(false);
            }

            _cartService.Remove(book.Id);

            return Ok(true);
        }
        [HttpGet("DeleteAll")]
        public IActionResult DeleteAll()
        {
            _cartService.RemoveAll();

            return NoContent();
        }
    }
}

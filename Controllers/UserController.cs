using API.Cart_Account.Models;
using API.Cart_Account.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Cart_Account.Controllers
{
    [Route("accountapi/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;
 
        public UserController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        public class info
        {
            public string Name { get; set; }
            public string Tuoi { get; set; }
            public string Image { get; set; }
            public string Lop { get; set; }
            public string Nhiemvu { get; set; }
            public string Fb { get; set; }
            public string TinhTrang { get; set; }
        }
        [AllowAnonymous]
        [HttpGet("info")]
        public IActionResult Info()
        {
            List<info> infos = new List<info>();
            var info = new info();
            info.Name = "Nguyễn Khả Hợp";
            info.Tuoi = "2x";
            info.Image = "https://res.cloudinary.com/imageshared/image/upload/v1627521341/Avatar/hc_ylsjpr.jpg";
            info.Lop = "DCCTPM62C";
            info.Nhiemvu = "Dựng khung và liên kết các trang của thành viên. Viết API";
            info.Fb = "";
            info.TinhTrang = "Độc thân";
            infos.Add(info);
            var info1 = new info();
            info1.Name = "Bùi Lê Cảnh";
            info1.Tuoi = "22";
            info1.Image = "https://res.cloudinary.com/imageshared/image/upload/v1627521341/Avatar/c%E1%BA%A3nh_v3zx80.jpg";
            info1.Lop = "DCCTPM62C";
            info1.Nhiemvu = "Code FrontEnd page Login and Register, call API";
            info1.Fb = "https://www.facebook.com/canh.kaka.39";
            info1.TinhTrang = "Độc thân";
            infos.Add(info1);
            var info2 = new info();
            info2.Name = "Trần Duy Linh";
            info2.Tuoi = "22";
            info2.Image = "https://res.cloudinary.com/imageshared/image/upload/v1627521341/Avatar/linh_ekvuim.png";
            info2.Lop = "DCCTPM62D";
            info2.Nhiemvu = "Code page giới thiệu and call API";
            info2.Fb = "https://www.facebook.com/qjimkkml";
            info2.TinhTrang = "Độc thân";
            infos.Add(info2);
            var info3 = new info();
            info3.Name = "Phạm Văn Hiếu";
            info3.Tuoi = "22";
            info3.Image = "https://res.cloudinary.com/imageshared/image/upload/v1627521341/Avatar/hieu_offgep.jpg";
            info3.Lop = "DCCTPM62C";
            info3.Nhiemvu = "Code page giỏ hàng and call API";
            info3.Fb = "https://www.facebook.com/profile.php?id=100008857501029";
            info3.TinhTrang = "Độc thân";
            infos.Add(info3);
            var info5 = new info();
            info5.Name = "Nguyễn Thu Trang";
            info5.Tuoi = "22";
            info5.Image = "https://res.cloudinary.com/imageshared/image/upload/v1627521341/Avatar/trang_vxlwb1.jpg";
            info5.Lop = "DCCTPM62C";
            info5.Nhiemvu = "Code page thông tin người dùng and call API";
            info5.Fb = "https://www.facebook.com/trang.99911141";
            info5.TinhTrang = "Độc thân";
            infos.Add(info5);


            return Ok(infos);

        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult Login([Bind] LoginViewModel user)
        {
            ICollection<ValidationResult> results = null;
            if (Validate(user, out results))
            {
                if (_userService.ValidateAdmin(user.Username, user.Password))
                {
                    var users = _userService.GetAccount(user.Username);
                    if (users != null)
                    {
                        if (users.ConfirmEmail && users.Active && !users.LockAccount)
                        {
                            var authClaims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, users.Email),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(ClaimTypes.Role, users.Role),
                                new Claim("id",users.Id)
                            };
                            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                            var token = new JwtSecurityToken(
                                issuer: _configuration["JWT:ValidIssuer"],
                                audience: _configuration["JWT:ValidAudience"],
                                expires: user.Remember ? DateTime.Now.AddYears(1) : DateTime.Now.AddHours(3),
                                claims: authClaims,
                                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                                );

                            return Ok(new
                            {
                                token = new JwtSecurityTokenHandler().WriteToken(token),
                                expiration = token.ValidTo,
                                id=users.Id
                            });
  
                        }
                        else if (!users.ConfirmEmail)
                        {
                            return Ok("Email chưa được kích hoạt nha !");
                        }
                    }
                    else
                        return Ok("Tên đăng nhập không tồn tại");
                }
                else
                    return Ok("Tên đăng nhập hoặc mật khẩu không chính xác.");
            }
            return Ok(false);
        }

        [HttpGet]
    
        public IEnumerable<Account> Get()
        {
            return _userService.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetAccount")]
        public ActionResult<Account> Get(string id)
        {
            var book = _userService.Get(id);
            var list = new Account()
            {
                Address=book.Address,
                CreateDate=book.CreateDate,
                Email=book.Email,
                Id=book.Id,
                Mobile=book.Mobile,
                Fullname=book.Fullname,
            };
            if (book == null)
            {
                return NotFound();
            }

            return list;
        }

        [AllowAnonymous]
        static bool Validate<T>(T obj, out ICollection<ValidationResult> results)
        {
            results = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);
        }
        [AllowAnonymous]
        [HttpPost("addAcount")]
        public ActionResult Create(RegisterViewModel login)
        {
            ICollection<ValidationResult> results = null;
            if (Validate(login, out results))
            {
                var dk = _userService.GetEmail(login.Username);
                if (dk != null)
                {
                    return Ok("Tên đăng nhập này có rồi nha !");
                }
                else
                {
                    var hashedPassword = new PasswordHasher<Account>().HashPassword(new Account(), login.Password);
                    var model = new Account()
                    {
                        Email = login.Username,
                        Password = hashedPassword,
                        Active = true,
                        ConfirmEmail = true,
                        CreateDate = DateTime.Now,
                        LockAccount = false,
                        Mobile="",
                        Role = "User"

                    };
                    _userService.Create(model);
                    return Ok(true);
                }
            }
            return Ok("Xin vui lòng kiểm tra lại !");
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Account bookIn)
        {
            var book = _userService.Get(id);

            if (book == null)
            {
                return Ok(false);
            }

            _userService.Update(id, bookIn);

            return Ok(true);
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = _userService.Get(id);

            if (book == null)
            {
                return Ok(false);
            }

            _userService.Remove(book.Id);

            return Ok(true);
        }
        //[HttpGet("DeleteAll")]
        //public IActionResult DeleteAll()
        //{
        //    _userService.RemoveAll();

        //    return NoContent();
        //}



    }
}

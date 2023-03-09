using E_comerce_Inventory.DataAccess.Repository.Interface;
using E_comerce_Inventory.Models.DataModels;
using E_comerce_Inventory.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel :PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWorkUnit _workUnit;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
           IWorkUnit workUnit)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _workUnit = workUnit;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            //[Required(ErrorMessage = "El campo es requerido"), StringLength(15,MinimumLength = 3,ErrorMessage = "Minimo de 3 caracteres y maximo de 15")]
            //public string UserName { get; set; }

            [Required(ErrorMessage = "El campo es requerido"), StringLength(15,MinimumLength = 3,ErrorMessage = "Minimo de 3 caracteres y maximo de 15")]
            public string Name { get; set; }
            [Required(ErrorMessage = "El campo es requerido"), StringLength(25,MinimumLength = 3,ErrorMessage = "Minimo de 3 caracteres y maximo de 25")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "El campo es requerido"), StringLength(25,MinimumLength = 3,ErrorMessage = "Minimo de 3 caracteres y maximo de 25")]
            public string Country { get; set; }
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            public string Role { get; set; }

            [Required]
            [StringLength(100,ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password",ErrorMessage = "Las contraseñas no coinciden")]
            public string ConfirmPassword { get; set; }

            public IEnumerable<SelectListItem> ListRole { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            //iniializamos las lista de Roles
            Input = new InputModel()
            {
                ListRole = _roleManager.Roles.Where(r => r.Name != DS.Role_Customer).Select(n => n.Name).Select(itemRole => new SelectListItem
                {
                    Text = itemRole,
                    Value = itemRole,
                })
            };
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new UserAplication
                {
                    UserName = Input.Email,
                    Name = Input.Name,
                    LastName = Input.LastName,
                    Email = Input.Email,
                    Country = Input.Country,
                    Role = Input.Role
                };

                var result = await _userManager.CreateAsync(user,Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    //La primer avez creara todos los roles

                    if (!await _roleManager.RoleExistsAsync(DS.Role_Admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(DS.Role_Admin));
                    }
                    if (!await _roleManager.RoleExistsAsync(DS.Role_Customer))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(DS.Role_Customer));
                    }
                    if (!await _roleManager.RoleExistsAsync(DS.Role_Sales))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(DS.Role_Sales));
                    }
                    if (!await _roleManager.RoleExistsAsync(DS.Role_Inventory))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(DS.Role_Inventory));
                    }

                    //REGISTRO DE USUARIOS DEFAULT , CUADNO UN ADMIN REGISTRE A ALGUIEN NO ENRARA AL IF YA QUE Role contendra un rol dado pro el admin
                    if (user.Role == null)
                    {
                        await _userManager.AddToRoleAsync(user,DS.Role_Customer);
                    } else
                    {
                        await _userManager.AddToRoleAsync(user,user.Role);
                    }

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity",userId = user.Id,code = code,returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email,"Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation",new { email = Input.Email,returnUrl = returnUrl });
                    } else
                    {
                        if (user.Role == null)//es decir customer
                        {
                            await _signInManager.SignInAsync(user,isPersistent: false);
                            return LocalRedirect(returnUrl);

                        } else //El admin esta registrando un nuevo usuario
                        {
                            //method , Controller , Area
                            return RedirectToAction("Index","User",new { Area = "Admin" });
                        }


                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

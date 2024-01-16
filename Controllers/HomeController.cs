using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MJMLIssue.Models;
using Mjml.Net;
using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MJMLIssue.Controllers;
public class HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager):Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    private readonly UserManager<IdentityUser> _userManager = userManager;

    public async Task<IActionResult> IndexAsync()
    {
        var returnUrl = Url.Content("~/");
        IdentityUser User = new()
        {
            Email = "johnDoe@gmail.com",
            UserName = "johnDoe@gmail.com"
        };
        var fullname = "John Doe";
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(User);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = Url.Page("/Account/ConfirmEmail", pageHandler: null, values: new { area = "Identity", userId = 1, code = code, returnUrl = returnUrl }, protocol: Request.Scheme);
        var mjPrefetch = $@"<mjml>
                            <mj-body background-color='#FFF'>
                            <mj-section background-color='#FFF' background-repeat='no-repeat' text-align='center' padding='0'>
                                <mj-column>
                                    <mj-text align='left' color='#008EAA' font-size='13px' line-height='22px' padding-bottom='5px'
                                        padding-top='25px' padding='10px 25px'>
                                        <p style='line-height: 60px; text-align: center; margin: 10px 0;font-size:55px;color:#008EAA;'>
                                            <b>Welcome</b><br>{@fullname}<br>
                                        </p>                      
                                    </mj-text>
                                    <mj-text align='left' color='#008EAA' font-size='13px' line-height='22px' padding-bottom='20px'
                                        padding-top='0px' padding='10px 25px'>
                                        <p style='line-height: 30px; text-align: center; margin: 10px 0;color:#008EAA;font-size:18px;font-weight:bold;'>
                                            Thank You for your interest.
                                            </p>
                                        <p style='line-height: 30px; text-align: justify; margin: 10px 0;color:#008EAA;font-size:18px;'>We
                                            are looking forward to sharing our ideas and will also keep you posted on our progress and
                                            development of the upcoming launch of our products. 
                                        </p>                                      
                                        <p style='line-height: 30px; text-align: center; margin: 10px 0;color:#008EAA;font-size:24px;FONT-WEIGHT:700;'>The
                                            Universal Team
                                        </p>
                                    </mj-text>
                                </mj-column>
                            </mj-section>
                            <mj-section background-color='#FFF' background-repeat='no-repeat' text-align='center' padding-bottom='40px' padding='0'>
                                <mj-column>
                                    <mj-button background-color='#D04139' border-radius='3px' font-size='18px' font-weight='bold'
                                        inner-padding='10px 25px' padding-bottom='30px' padding='10px 25px'><a
                                            href='{HtmlEncoder.Default.Encode(@callbackUrl)}'
                                            style='color:#FFFFFF; text-decoration:none;'>Click to Verify</a>
                                    </mj-button>                                  
                                  <mj-text align='center' color='#008EAA' line-height='22px' padding='0px 25px 0px 25px'>                                        
                                        <p style='line-height: 16px; text-align: center; margin: 10px 0;font-size:14px;color:#008EAA;'>
                                            Copyright &copy; {@DateTime.Now.Year}, All Rights Reserved.</p>
                                    </mj-text>
                                </mj-column>
                            </mj-section>
                            </mj-body>
                            </mjml>";

        var mjmlRenderer = new MjmlRenderer();
        var options = new MjmlOptions
        {
            Beautify = false
        };
        var (RenderedgBody, error) = mjmlRenderer.Render(mjPrefetch, options);

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

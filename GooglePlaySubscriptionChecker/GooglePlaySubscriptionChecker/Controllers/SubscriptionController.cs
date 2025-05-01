using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly GooglePlayService _googlePlayService;

    public SubscriptionController(GooglePlayService googlePlayService)
    {
        _googlePlayService = googlePlayService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSubscription(
        string packageName, string subscriptionId, string token)
    {
        try
        {
            var result = await _googlePlayService.GetSubscriptionStatusAsync(
                packageName, subscriptionId, token);
            return Ok(result);
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}

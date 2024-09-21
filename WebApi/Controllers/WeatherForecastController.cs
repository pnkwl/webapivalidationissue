using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    private readonly ILogger<WeatherForecastController> _logger;
    
    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    [HttpPost("post-no-polymorph")]
    public IActionResult Post([FromBody] ComplexModelNoPolymorph model)
    {
        return NoContent();
    }
    
    [HttpPost("post-polymorph")]
    public IActionResult Post([FromBody] ComplexModelWithPolymorph model)
    {
        return NoContent();
    }
}

public record ComplexModelNoPolymorph(List<NonPolymorphicModel> NonPolymorphicModelsLvl1);
public record NonPolymorphicModel(List<AnotherModel> ModelsLvl2);

public record ComplexModelWithPolymorph(List<BaseModel> PolymorphicModelsLvl1);
public sealed record DerivedModel(List<AnotherModel> ModelsLvl2) : BaseModel;

public sealed record AnotherModel;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$t")]
[JsonDerivedType(typeof(DerivedModel), "derivedtype")]
public abstract record BaseModel;
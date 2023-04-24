using Attendance_Management_System_Data.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using static Attendance_Management_System_Data.Constants;

namespace Attendance_Management_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : Controller
    {
        private readonly HttpClient _client;
        public PersonController()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Authorize]
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetPerson(int employeeId)
        {
            try
            {
                HttpResponseMessage getData = await _client.GetAsync($"{FaceRecognitionConstants.PersonBaseUrl}/{employeeId}");

                if (!getData.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = "Something went wrong" });
                }

                return StatusCode(StatusCodes.Status200OK, JsonConvert.DeserializeObject<ApiResponseDto<PersonDto>>(getData.Content.ReadAsStringAsync().Result));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }
    }
}

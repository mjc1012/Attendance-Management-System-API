using Attendance_Management_System_Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using static Attendance_Management_System_Data.Constants;

namespace Attendance_Management_System_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FaceToRecognizeController : Controller
    {
        private readonly HttpClient _client;
        public FaceToRecognizeController()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        [HttpPost]
        public async Task<IActionResult> RecognizeFace([FromBody] FaceToRecognizeDto request)
        {
            try
            {
                HttpResponseMessage getData = await _client.PostAsJsonAsync(FaceRecognitionConstants.FaceToRecognizeBaseUrl, request);

                if (!getData.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = "Something went wrong." });
                }
                try
                {
                    return StatusCode(StatusCodes.Status200OK, JsonConvert.DeserializeObject<ApiResponseDto<PersonDto>>(getData.Content.ReadAsStringAsync().Result));
                }
                catch(Exception)
                {
                    return StatusCode(StatusCodes.Status200OK, JsonConvert.DeserializeObject<ApiResponseDto<PersonDto>>(getData.Content.ReadAsStringAsync().Result));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }
    }
}

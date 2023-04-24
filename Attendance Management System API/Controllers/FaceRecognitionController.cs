using Attendance_Management_System_Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using static Attendance_Management_System_Data.Constants;

namespace Attendance_Management_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceRecognitionController : Controller
    {
        private readonly HttpClient _client;
        public FaceRecognitionController()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        [HttpGet("train-model")]
        public async Task<IActionResult> TrainModel()
        {
            try
            {
                HttpResponseMessage getData = await _client.GetAsync($"{FaceRecognitionConstants.FaceRecognitionBaseUrl}/train-model");

                if (!getData.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = "Something went wrong." });
                }

                return StatusCode(StatusCodes.Status200OK, JsonConvert.DeserializeObject<ResponseDto>(getData.Content.ReadAsStringAsync().Result));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message }); ;
            }


        }
    }
}

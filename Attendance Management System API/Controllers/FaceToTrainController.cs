using Attendance_Management_System_Data.Dtos;
using Attendance_Management_System_Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net.Http.Headers;
using static Attendance_Management_System_Data.Constants;

namespace Attendance_Management_System_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FaceToTrainController : Controller
    {
        private readonly HttpClient _client;
        public FaceToTrainController()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        //[Authorize]
        [HttpGet("{employeeId}/missing-expression")]
        public async Task<IActionResult> GetMissingFaceExpression(int employeeId)
        {
            try
            {
                HttpResponseMessage getData = await _client.GetAsync($"{FaceRecognitionConstants.FaceToTrainBaseUrl}/{employeeId}/missing-expression");

                if (!getData.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = "Something went wrong" });
                }

                return StatusCode(StatusCodes.Status200OK, JsonConvert.DeserializeObject<ApiResponseDto<FaceExpressionDto>>(getData.Content.ReadAsStringAsync().Result));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("{employeeId}/person-faces")]
        public async Task<IActionResult> GetFacesToTrain(int employeeId)
        {
            try
            {
                HttpResponseMessage getData = await _client.GetAsync($"{FaceRecognitionConstants.FaceToTrainBaseUrl}/{employeeId}/person-faces");

                if (!getData.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = "Something went wrong." });
                }

                return StatusCode(StatusCodes.Status200OK, JsonConvert.DeserializeObject<ApiResponseDto<List<FaceToTrainDto>>>(getData.Content.ReadAsStringAsync().Result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = "Something went wrong." });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateFaceToTrain([FromBody] FaceToTrainDto request)
        {
            try
            {
                HttpResponseMessage getData = await _client.PostAsJsonAsync(FaceRecognitionConstants.FaceToTrainBaseUrl, request);
                if (!getData.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = "Something went wrong." });
                }

                return StatusCode(StatusCodes.Status200OK, JsonConvert.DeserializeObject<ApiResponseDto<PersonDto>>(getData.Content.ReadAsStringAsync().Result));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = "Something went wrong." });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFaceToTrain(int id)
        {
            try
            {
                HttpResponseMessage getData = await _client.DeleteAsync($"{FaceRecognitionConstants.FaceToTrainBaseUrl}/{id}");
                if (!getData.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = "Something went wrong." });
                }

                return StatusCode(StatusCodes.Status200OK, JsonConvert.DeserializeObject<ResponseDto>(getData.Content.ReadAsStringAsync().Result));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }
    }
}

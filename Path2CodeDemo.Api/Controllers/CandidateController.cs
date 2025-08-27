using Microsoft.AspNetCore.Mvc;
using Path2CodeDemo.Application.IService;
using Path2CodeDemo.Application.RequestModels;
using Path2CodeDemo.Domain;

namespace Path2CodeDemo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateService _candidateService;
        private readonly ILogger<CandidateController> _logger;

        public CandidateController(ICandidateService candidateService, ILogger<CandidateController> logger)
        {
            _candidateService = candidateService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all candidates asynchronously.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> containing a list of <see cref="Candidate"/> objects.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetCandidatesAsync()
        {
            return Ok((List<Candidate>?)await _candidateService.GetAllCandidatesAsync());
        }

        /// <summary>
        /// Retrieves a candidate by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the candidate.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the candidate if found; otherwise, a 404 Not Found response.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Candidate? candidate = await _candidateService.GetCandidateByIdAsync(id);

            if (candidate == null)
            {
                _logger.LogInformation("Candidate with id: {CandidateId} not found.", 12);
                return NotFound();
            }

            return Ok(candidate);
        }

        /// <summary>
        /// Creates a new candidate based on the provided request data.
        /// </summary>
        /// <param name="request">The request object containing candidate details.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> that returns a 201 Created response with the location of the newly created candidate.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCandidateRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            var id = await _candidateService.AddCandidateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        /// <summary>
        /// Handles file upload requests. Accepts a file from the client, reads its contents into memory,
        /// and sends a command to save the resume. Returns the unique identifier of the saved file.
        /// </summary>
        /// <param name="file">The file to be uploaded, provided as an <see cref="IFormFile"/>.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the file identifier if successful, or a BadRequest result if the file is empty.
        /// </returns>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("Upload attempt with empty file.");
                return BadRequest("File is empty");
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var command = new SaveResumeRequest
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Content = memoryStream.ToArray()
            };
            await _candidateService.SaveResume(command);
            _logger.LogInformation("File {FileName} uploaded successfully.", file.FileName);

            return Ok(new { file.FileName });
        }
    }
}

using backend.Helpers.Extensions;
using backend.Models.Domain;
using backend.Models.DTOs.Request;
using backend.Models.DTOS.Response;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("/api/queues")]
    [ApiController]
    public class QueuingController : ControllerBase
    {
        private readonly IUserQueueService _userQueueService;

        public QueuingController(IUserQueueService userQueueService)
        {
            _userQueueService = userQueueService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDTO<IEnumerable<UserQueueResponseDTO>>>> GetAllUserQueue()
        {
            var queues = await _userQueueService.GetAllUserQueue();
            return this.ApiOk(queues, "Get all queue elems successfully");
        }

        [HttpGet("{bookId:int}")]
        public async Task<ActionResult<ApiResponseDTO<IEnumerable<UserQueueResponseDTO>>>> GetUserQueueByBookId(int bookId)
        {
            var queues = await _userQueueService.GetUserQueueByBookId(bookId);
            return this.ApiOk(queues, "Get queue by book id successfully");
        }

        [HttpGet("{bookId:int}/{userId:int}")]
        public async Task<ActionResult<ApiResponseDTO<UserQueueResponseDTO>>> GetUserQueue(int bookId, int userId)
        {
            var queue = await _userQueueService.GetUserQueue(bookId, userId);
            return queue != null ? this.ApiOk(queue, "Get user queue elem successfully") : this.ApiNotFound<UserQueue>("Queue user is not found");
        }

        [HttpPost]
        public async Task<IActionResult> EnqueueUser(UserQueueRequestDTO userQueueRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return this.ApiBadRequest<object>("Validation failed", errors);
            }
            var res = await _userQueueService.EnqueueUser(userQueueRequest);
            return this.ApiCreated(res, "User enqueued successfully");
        }

        [HttpDelete("{bookId:int}")]
        public async Task<ActionResult<ApiResponseDTO<UserQueueResponseDTO>>> DequeueUser(int bookId)
        {
            var queue = await _userQueueService.DequeueUser(bookId);
            if (queue == null)
            {
                return this.ApiNotFound<UserQueue>("User queue not found");
            }
            return this.ApiOk(queue, "User dequeued successfully");
        }
    }
}
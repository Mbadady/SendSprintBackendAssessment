namespace BackendAssessment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders(int? skip, int? take)
        {
            var response = await _orderService.GetAllOrdersAsync(skip, take);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var response = await _orderService.GetOrderByIdAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}

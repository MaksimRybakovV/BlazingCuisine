using AutoMapper;
using BlazingCuisine.Server.Data;

namespace BlazingCuisine.Server.Services
{
    public class BaseService<T>
    {
        protected readonly ApplicationDataContext _context;
        protected readonly IMapper _mapper;
        protected readonly ILogger<T> _logger;

        public BaseService(ApplicationDataContext context, IMapper mapper, ILogger<T> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
    }
}

using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Queries
{
    public record GetUsersQuery() : IRequest<List<User>>;

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<User>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.ListAsync(cancellationToken);
        }
    }
}

using Grpc.Core;
using UserManagementService.DbContexts;
using Users;

namespace UserManagementService.Services
{
	public class UserServiceImpl : UserService.UserServiceBase
	{
		private readonly UserDbContext _dbContext;

		public UserServiceImpl(UserDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public override async Task<UserResponse> GetUserById(UserRequest request, ServerCallContext context)
		{
			var user = await _dbContext.Users.FindAsync(request.UserId);

			if (user == null)
				throw new RpcException(new Status(StatusCode.NotFound, "User not found"));

			return new UserResponse
			{
				Id = user.Id,
				FullName = user.FullName,
				Email = user.Email,
				Phone = user.Phone,
				Role = user.Role
			};
		}

		public override async Task<ChildResponse> GetChildById(ChildRequest request, ServerCallContext context)
		{
			var child = await _dbContext.Children.FindAsync(request.ChildId);

			if (child == null)
				throw new RpcException(new Status(StatusCode.NotFound, "Child not found"));

			return new ChildResponse
			{
				Id = child.Id,
				UserId = child.UserId,
				FullName = child.FullName,
				Birthday = child.Birthday.ToString("yyyy-MM-dd"),
				Gender = child.Gender
			};
		}
	}
}

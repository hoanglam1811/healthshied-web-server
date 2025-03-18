using Authentication;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using UserManagementService.DbContexts;
using UserManagementService.Entities;

namespace UserManagementService.Services
{
	public class AuthenticationServiceImpl : AuthenticationService.AuthenticationServiceBase
	{
		private readonly UserDbContext _dbContext;

		public AuthenticationServiceImpl(UserDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public override async Task<UserResponse> Login(LoginRequest request, ServerCallContext context)
		{
			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

			if (user == null)
				throw new RpcException(new Status(StatusCode.NotFound, "User with email not found"));

      if(!PasswordManager.VerifyPassword(request.Password, user.PasswordHash)){
				throw new RpcException(new Status(StatusCode.NotFound, "Wrong password"));
      }

			return new UserResponse
			{
				Id = user.Id,
				FullName = user.FullName,
				Email = user.Email,
				Phone = user.Phone,
				Role = user.Role,
				Status = user.Status
			};
		}

    public override async Task<UserResponse> Register(RegisterRequest request, ServerCallContext context)
		{
			var check = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

			if (check != null)
				throw new RpcException(new Status(StatusCode.NotFound, "User with email already exists"));

      var userCreated = await _dbContext.Users.AddAsync(new User
      {
				FullName = request.FullName,
				Email = request.Email,
				Phone = request.Phone,
				Role = "Client",
        PasswordHash = PasswordManager.HashPassword(request.Password),
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
				Status = "Active"
      });
      await _dbContext.SaveChangesAsync();
      var user = userCreated.Entity;

			return new UserResponse
			{
        Id = user.Id,
        FullName = user.FullName,
        Email = user.Email,
        Phone = user.Phone,
        Role = user.Role,
        Status = user.Status
      };
		}
	}
}

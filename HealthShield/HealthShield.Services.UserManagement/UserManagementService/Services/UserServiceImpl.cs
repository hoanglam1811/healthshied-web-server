using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using UserManagementService.DbContexts;
using UserManagementService.Entities;
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
				Role = user.Role,
				Status = user.Status
			};
		}

		public override async Task<UserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
		{
			var user = new User
			{
				FullName = request.FullName,
				Email = request.Email,
				Phone = request.Phone,
				Role = request.Role,
				PasswordHash = request.Password,
				Status = "Active"
			};
			_dbContext.Users.Add(user);
			await _dbContext.SaveChangesAsync();
			return new UserResponse
			{
				Id = user.Id,
				FullName = user.FullName,
				Email = user.Email,
				Phone = user.Phone,
				Role = user.Role,
				Status = "Active"
			};
		}

		public override async Task<UsersResponse> GetAllUsers(Empty request, ServerCallContext context)
		{
			var users = await _dbContext.Users.ToListAsync();
			return new UsersResponse
			{
				Users = { users.Select(u => new UserResponse
				{
					Id = u.Id,
					FullName = u.FullName,
					Email = u.Email,
					Phone = u.Phone,
					Role = u.Role,
					Status = u.Status
				}) }
			};
		}

		public override async Task<UserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
		{
			var user = await _dbContext.Users.FindAsync(request.Id);
			if (user == null)
				throw new RpcException(new Status(StatusCode.NotFound, "User not found"));

			user.FullName = request.FullName;
			user.Email = request.Email;
			user.Phone = request.Phone;
			user.Role = request.Role;
			await _dbContext.SaveChangesAsync();

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

		public override async Task<Empty> DeleteUser(UserRequest request, ServerCallContext context)
		{
			var user = await _dbContext.Users.FindAsync(request.UserId);
			if (user == null)
				throw new RpcException(new Status(StatusCode.NotFound, "User not found"));

			_dbContext.Users.Remove(user);
			await _dbContext.SaveChangesAsync();
			return new Empty();
		}

		public override async Task<ChildrenResponse> GetAllChildren(Empty request, ServerCallContext context)
		{
			var children = await _dbContext.Children.ToListAsync();
			return new ChildrenResponse
			{
				Children = { children.Select(c => new ChildResponse
				{
					Id = c.Id,
					UserId = c.UserId,
					FullName = c.FullName,
					Birthday = c.Birthday.ToString("yyyy-MM-dd"),
					Gender = c.Gender
				}) }
			};
		}

		public override async Task<ChildResponse> CreateChild(CreateChildRequest request, ServerCallContext context)
		{
			var child = new Child
			{
				UserId = request.UserId,
				FullName = request.FullName,
				Birthday = DateTime.Parse(request.Birthday),
				Gender = request.Gender
			};
			_dbContext.Children.Add(child);
			await _dbContext.SaveChangesAsync();
			return new ChildResponse
			{
				Id = child.Id,
				UserId = child.UserId,
				FullName = child.FullName,
				Birthday = child.Birthday.ToString("yyyy-MM-dd"),
				Gender = child.Gender
			};
		}

		public override async Task<Empty> DeleteChild(ChildRequest request, ServerCallContext context)
		{
			var child = await _dbContext.Children.FindAsync(request.ChildId);
			if (child == null)
				throw new RpcException(new Status(StatusCode.NotFound, "Child not found"));

			_dbContext.Children.Remove(child);
			await _dbContext.SaveChangesAsync();
			return new Empty();
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

		public override async Task<ChildrenResponse> GetChildByCustomerId(UserRequest request, ServerCallContext context)
		{
			var children = await _dbContext.Children.Where(c => c.UserId == request.UserId).ToListAsync();
			var response = new ChildrenResponse();
			response.Children.AddRange(children.Select(c => new ChildResponse
			{
				Id = c.Id,
				UserId = c.UserId,
				FullName = c.FullName,
				Birthday = c.Birthday.ToString("yyyy-MM-dd"),
				Gender = c.Gender
			}));
			return response;
		}

		public override async Task<StaffScheduleResponse> GetStaffScheduleById(StaffScheduleRequest request, ServerCallContext context)
		{
			var schedule = await _dbContext.StaffSchedules.FindAsync(request.StaffScheduleId);

			if (schedule == null)
				throw new RpcException(new Status(StatusCode.NotFound, "Staff schedule not found"));

			return new StaffScheduleResponse
			{
				Id = schedule.Id,
				StaffId = schedule.StaffId,
				ShiftDate = schedule.ShiftDate.ToString("yyyy-MM-dd"),
				ShiftTime = (Users.ShiftTime)schedule.ShiftTime
			};
		}

		public override async Task<StaffScheduleResponse> CreateStaffSchedule(CreateStaffScheduleRequest request, ServerCallContext context)
		{
			var schedule = new StaffSchedule
			{
				StaffId = request.StaffId,
				ShiftDate = DateTime.Parse(request.ShiftDate),
				ShiftTime = (Entities.ShiftTime)(int)request.ShiftTime
			};

			_dbContext.StaffSchedules.Add(schedule);
			await _dbContext.SaveChangesAsync();

			return new StaffScheduleResponse
			{
				Id = schedule.Id,
				StaffId = schedule.StaffId,
				ShiftDate = schedule.ShiftDate.ToString("yyyy-MM-dd"),
				ShiftTime = (Users.ShiftTime)schedule.ShiftTime
			};
		}

		public override async Task<StaffSchedulesResponse> GetAllStaffSchedules(Empty request, ServerCallContext context)
		{
			var schedules = await _dbContext.StaffSchedules.ToListAsync();

			return new StaffSchedulesResponse
			{
				Schedules = { schedules.Select(s => new StaffScheduleResponse
		{
			Id = s.Id,
			StaffId = s.StaffId,
			ShiftDate = s.ShiftDate.ToString("yyyy-MM-dd"),
			ShiftTime = (Users.ShiftTime)s.ShiftTime
		}) }
			};
		}

		public override async Task<StaffScheduleResponse> UpdateStaffSchedule(UpdateStaffScheduleRequest request, ServerCallContext context)
		{
			var schedule = await _dbContext.StaffSchedules.FindAsync(request.Id);
			if (schedule == null)
				throw new RpcException(new Status(StatusCode.NotFound, "Staff schedule not found"));

			schedule.ShiftDate = DateTime.Parse(request.ShiftDate);
			schedule.ShiftTime = (Entities.ShiftTime)request.ShiftTime;
			await _dbContext.SaveChangesAsync();

			return new StaffScheduleResponse
			{
				Id = schedule.Id,
				StaffId = schedule.StaffId,
				ShiftDate = schedule.ShiftDate.ToString("yyyy-MM-dd"),
				ShiftTime = (Users.ShiftTime)schedule.ShiftTime
			};
		}

		public override async Task<Empty> DeleteStaffSchedule(StaffScheduleRequest request, ServerCallContext context)
		{
			var schedule = await _dbContext.StaffSchedules.FindAsync(request.StaffScheduleId);
			if (schedule == null)
				throw new RpcException(new Status(StatusCode.NotFound, "Staff schedule not found"));

			_dbContext.StaffSchedules.Remove(schedule);
			await _dbContext.SaveChangesAsync();

			return new Empty();
		}


		public override async Task<StaffSchedulesResponse> GetScheduleByStaffId(StaffRequest request, ServerCallContext context)
		{
			var schedules = await _dbContext.StaffSchedules
				.Where(s => s.StaffId == request.StaffId)
				.ToListAsync();

			return new StaffSchedulesResponse
			{
				Schedules = { schedules.Select(s => new StaffScheduleResponse
		{
			Id = s.Id,
			StaffId = s.StaffId,
			ShiftDate = s.ShiftDate.ToString("yyyy-MM-dd"),
			ShiftTime = (Users.ShiftTime)s.ShiftTime
		}) }
			};
		}

	}
}

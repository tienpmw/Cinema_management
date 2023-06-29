using CinemaWebAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using System.Net.NetworkInformation;
using System.Text;

namespace CinemaWebAPI.Policies
{
	public class PermissionRequirement : IAuthorizationRequirement
	{
		public PermissionRequirement()
		{
		}
	}
	public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
	{

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
		{
			if (context.User == null || context.User.Claims.Count() == 0)
			{
				context.Fail();
			}
			else
			{
				string? deviceId = context.User.Claims.FirstOrDefault(x => x.Type == "DeviceId").Value;
				if (string.IsNullOrEmpty(deviceId))
				{
					context.Fail();
				}
				else
				{
					if (deviceId != Util.Instance.GetHexDeviceId())
					{
						context.Fail();
					}
					else
					{
						context.Succeed(requirement);
					}
				}
			}
			return Task.CompletedTask;
		}
	}
}
